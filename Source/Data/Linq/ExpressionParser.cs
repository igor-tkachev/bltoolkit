using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq
{
	using Mapping;
	using Sql;
	using Reflection;

	class ExpressionParser<T> : ReflectionHelper
	{
		#region Init

		public ExpressionParser()
		{
			_info.SqlBuilder = new SqlBuilder();
		}

		readonly ExpressionInfo<T>   _info            = new ExpressionInfo<T>();
		readonly ParameterExpression _expressionParam = Expression.Parameter(typeof(Expression),    "expr");
		readonly ParameterExpression _dataReaderParam = Expression.Parameter(typeof(IDataReader),   "rd");
		readonly ParameterExpression _mapSchemaParam  = Expression.Parameter(typeof(MappingSchema), "ms");

		#endregion

		#region Parse

		public ExpressionInfo<T> Parse(MappingSchema mappingSchema, Expression expression)
		{
			_info.MappingSchema = mappingSchema;
			_info.Expression    = expression;

			ParseInfo.CreateRoot(expression, _expressionParam).Match(
				//
				// db.Table.ToList()
				//
				pi => pi.IsConstant<IQueryable>((value, _) => SimpleQuery(value.ElementType, null)),
				//
				// from p in db.Table select p
				// db.Table.Select(p => p)
				//
				pi => pi.IsMethod(typeof(Queryable), "Select",
					obj => obj.IsConstant<IQueryable>(),
					arg => arg.IsLambda<T>(
						body => body.NodeType == ExpressionType.Parameter,
						l    => SimpleQuery(typeof(T), l.Expr.Parameters[0].Name))),
				//
				// everything else
				//
				pi =>
				{
					BuildSelect(ParseSequence(pi));
					return true;
				}
			);

			return _info;
		}

		QueryInfo ParseSequence(ParseInfo<Expression> info)
		{
			QueryInfo select = null;

			if (info.IsConstant<IQueryable>((value,_) =>
				{
					var table = new SqlTable(_info.MappingSchema, value.ElementType);
					_info.SqlBuilder.From.Table(table);
					select = new QueryInfo.Constant(value.ElementType, table);
					return true;
				}))
				return select;

			if (info.NodeType != ExpressionType.Call)
				throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", info.Expr), "info");

			info.ConvertTo<MethodCallExpression>().Match
			(
				pi => pi.IsQueryableMethod("Select", seq => select = ParseSequence(seq), (p, b) => select = ParseSelect(select, p, b)),
				pi => pi.IsQueryableMethod("Where",  seq => select = ParseSequence(seq), (p, b) => select = ParseWhere (select, p, b)),

				pi => { throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", pi.Expr), "info"); }
			);

			return select;
		}

		#endregion

		#region Parse Select

		static QueryInfo ParseSelect(QueryInfo select, ParseInfo<ParameterExpression> parm, ParseInfo body)
		{
			SetAlias(select, parm.Expr.Name);

			switch (body.NodeType)
			{
				case ExpressionType.Parameter : return select;
				case ExpressionType.New       : return new QueryInfo.New       (select, parm, body.ConvertTo<NewExpression>());
				case ExpressionType.MemberInit: return new QueryInfo.MemberInit(select, parm, body.ConvertTo<MemberInitExpression>());
				default                       : throw  new InvalidOperationException();
			}
		}

		#endregion

		#region Parse Where

		QueryInfo ParseWhere(QueryInfo select, ParseInfo<ParameterExpression> parm, ParseInfo body)
		{
			SetAlias(select, parm.Expr.Name);

			if (CheckForSubQuery(select, parm, body))
			{
				var subQuery = new QueryInfo.SubQuery(select, _info.SqlBuilder);

				_info.SqlBuilder = subQuery.SqlBuilder;

				select = subQuery;
			}

			ParseSearchCondition(_info.SqlBuilder.Where.SearchCondition.Conditions, select, body);

			return select;
		}

		bool CheckForSubQuery(QueryInfo query, ParseInfo<ParameterExpression> parm, ParseInfo expr)
		{
			var makeSubquery = false;

			expr.Walk(pi =>
			{
				if (!makeSubquery)
					pi.IsMemberAccess((ma,ex) =>
					{
						if (IsParameter(ma.Expr, parm.Expr))
						{
							// field = p.FieldName
							//
							var member = ma.Expr.Member;

							if (member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property)
								makeSubquery = CheckFieldForSubQuery(query, ma);

							return true;
						}

						return false;
					});

				return pi;
			});

			return makeSubquery;
		}

		public bool CheckFieldForSubQuery(QueryInfo queryInfo, ParseInfo<MemberExpression> memberExpr)
		{
			bool? makeQuery = null;
			var   member    = memberExpr.Expr.Member;

			FieldWalker(queryInfo, memberExpr, _ => makeQuery = false, (_,__,___,____) => makeQuery = true);

			if (makeQuery == null)
				throw new LinqException("Member '{0}.{1}' is not an SQL column.", member.ReflectedType, member.Name);

			return makeQuery.Value;
		}

		#endregion

		#region Build Select

		void BuildSelect(QueryInfo query)
		{
			query.Match
			(
				constantExpr => // QueryInfo.Constant
				{
					foreach (var field in constantExpr.Table.Fields.Values)
						_info.SqlBuilder.Select.Expr(field);

					_info.SetQuery();
				},
				newExpr    => BuildNew(query, newExpr.   Body, newExpr.   Parameter), // QueryInfo.New
				memberInit => BuildNew(query, memberInit.Body, memberInit.Parameter), // QueryInfo.MemberInit
				subQuery   => _info.SetQuery()                                        // QueryInfo.SubQuery
			);
		}

		void BuildNew(QueryInfo query, ParseInfo expr, ParseInfo<ParameterExpression> parm)
		{
			var info = BuildNewExpression(query, expr, parm);

			var mapper = Expression.Lambda<Func<IDataReader,MappingSchema,Expression,T>>(
				info, new [] { _dataReaderParam, _mapSchemaParam, _expressionParam });

			_info.SetQuery(mapper.Compile());
		}

		ParseInfo BuildNewExpression(QueryInfo query, ParseInfo expr, ParseInfo<ParameterExpression> parm)
		{
			return expr.Walk(pi =>
			{
				pi.IsMemberAccess((ma,ex) =>
				{
					if (IsParameter(ma.Expr, parm.Expr))
					{
						// field = p.FieldName
						//
						var member = ma.Expr.Member;

						if (member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property)
						{
							var field = GetField(query, ma);

							if (field != null)
							{
								pi = field;
								return true;
							}
						}
					}
					else if (ex.IsConstant(c =>
					{
						// field = localVariable
						//
						var e = Expression.MakeMemberAccess(Expression.Convert(c.ParamAccessor, ex.Expr.Type), ma.Expr.Member);
						pi = pi.Parent.Replace(e, c.ParamAccessor);
						return true;
					}))
					{
						return true;
					}

					return false;
				});

				return pi;
			});
		}

		public ParseInfo GetField(QueryInfo queryInfo, ParseInfo<MemberExpression> memberExpr)
		{
			ParseInfo pi = null;

			var member = memberExpr.Expr.Member;

			FieldWalker(queryInfo, memberExpr,
				column =>
				{
					var idx = _info.SqlBuilder.Select.Add(column);

					var memberType = member.MemberType == MemberTypes.Field ?
						((FieldInfo)   member).FieldType :
						((PropertyInfo)member).PropertyType;

					MethodInfo mi;

					if (!MapSchema.Converters.TryGetValue(memberType, out mi))
						throw new LinqException("Cannot find converter for the '{0}' type.", memberType.FullName);

					pi = memberExpr.Parent.Replace(
						Expression.Call(_mapSchemaParam, mi,
							Expression.Call(_dataReaderParam, DataReader.GetValue,
								Expression.Constant(idx, typeof(int)))),
						memberExpr.ParamAccessor);
				},
				(query,expr,parms,_) =>
				{
					pi = BuildNewExpression(query,expr,parms);
					pi.IsReplaced = true;
				}
			);

			if (pi == null)
				throw new LinqException("Member '{0}.{1}' is not an SQL column.", member.ReflectedType, member.Name);

			return pi;
		}

		static void SetAlias(QueryInfo query, string alias)
		{
			query.Match
			(
				constantExpr =>
				{
					if (constantExpr.Table.Alias == null)
						constantExpr.Table.Alias = alias;
				},
				_ => {},
				_ => {},
				subQuery =>
				{
					var table = subQuery.SqlBuilder.From.Tables[0];
					if (table.Alias == null)
						table.Alias = alias;
				}
			);
		}

		bool SimpleQuery(Type type, string alias)
		{
			var table = new SqlTable(_info.MappingSchema, type) { Alias = alias };

			_info.SqlBuilder
				.Select
					.Field(table.All)
				.From
					.Table(table);

			_info.SetQuery();

			return true;
		}

		#endregion

		#region Build Parameter

		readonly Dictionary<Expression,ExpressionInfo<T>.Parameter> _parameters = new Dictionary<Expression, ExpressionInfo<T>.Parameter>();

		ExpressionInfo<T>.Parameter BuildParameter(ParseInfo expr)
		{
			ExpressionInfo<T>.Parameter p;

			if (_parameters.TryGetValue(expr.Expr, out p))
				return p;

			string name = null;

			var newExpr = expr.Walk(pi =>
			{
				pi.IsConstant(c =>
				{
					if (!TypeHelper.IsScalar(pi.Expr.Type))
					{
						var e = Expression.Convert(c.ParamAccessor, pi.Expr.Type);
						pi = pi.Parent.Replace(e, c.ParamAccessor);

						if (pi.Parent.NodeType == ExpressionType.MemberAccess)
						{
							var ma = (MemberExpression)pi.Parent.Expr;
							name = ma.Member.Name;
						}
					}

					return true;
				});

				return pi;
			});

			var mapper = Expression.Lambda<Func<Expression,object>>(Expression.Convert(newExpr, typeof(object)), new [] { _expressionParam });

			p = new ExpressionInfo<T>.Parameter
			{
				Expression   = expr.Expr,
				Accessor     = mapper.Compile(),
				SqlParameter = new SqlParameter(name, null)
			};

			_parameters.Add(expr.Expr, p);
			_info.Parameters.Add(p);

			return p;
		}

		static bool CanBeCompiled(ParseInfo expr)
		{
			bool canbe = true;

			expr.Walk(pi =>
			{
				if (pi.NodeType == ExpressionType.Parameter)
					canbe = false;

				return pi;
			});

			return canbe;
		}

		#endregion

		#region Field Walker

		void FieldWalker(
			QueryInfo                   query,
			ParseInfo<MemberExpression> memberExpr,
			Action<ISqlExpression>      processColumn,
			Action<QueryInfo,ParseInfo,ParseInfo<ParameterExpression>,string> processExpression)
		{
			var member = memberExpr.Expr.Member;

			query.Match
			(
				#region QueryInfo.Constant

				constantExpr =>
				{
					var field = constantExpr.Table[member.Name];
					if (field != null)
						processColumn(field);
				},

				#endregion

				#region QueryInfo.New

				newExpr =>
				{
					if (IsParameter(memberExpr.Expr, newExpr.Parameter.Expr))
					{
						FieldWalker(newExpr.SourceInfo, memberExpr, processColumn, processExpression);
					}
					else if (IsMember(memberExpr.Expr.Expression, newExpr.Body.Expr.Members))
					{
						FieldWalker(newExpr.SourceInfo, memberExpr, processColumn, processExpression);
					}
					else
					{
						var body = newExpr.Body.Expr;
						var mem  = member;

						if (mem is PropertyInfo)
							mem = ((PropertyInfo)member).GetGetMethod();

						if (body.Members != null)
						{
							for (var i = 0; i < body.Members.Count; i++)
							{
								if (mem == body.Members[i])
								{
									var arg = body.Arguments[i];

									if (arg is MemberExpression)
									{
										FieldWalker(
											newExpr.SourceInfo,
											newExpr.Body.Create((MemberExpression)arg, newExpr.Body.Index(body.Arguments, New.Arguments, i)),
											processColumn,
											processExpression);
									}
									else
									{
										string memberName = null;

										if (mem is MethodInfo)
										{
											var pi = TypeHelper.GetPropertyByMethod((MethodInfo) mem);
											if (pi != null)
												memberName = pi.Name;
										}
										else if (mem is PropertyInfo || mem is FieldInfo)
											memberName = mem.Name;

										processExpression(
											query,
											newExpr.Body.Create(arg, newExpr.Body.Index(body.Arguments, New.Arguments, i)),
											newExpr.Parameter,
											memberName);
									}

									return;
								}
							}
						}
					}
				},

				#endregion

				#region QueryInfo.MemberInit

				memberInit =>
				{
					if (IsParameter(memberExpr.Expr, memberInit.Parameter.Expr))
					{
						FieldWalker(memberInit.SourceInfo, memberExpr, processColumn, processExpression);
					}
					else if (IsMember(memberExpr.Expr.Expression, memberInit.Members))
					{
						FieldWalker(memberInit.SourceInfo, memberExpr, processColumn, processExpression);
					}
					else
					{
						var body = memberInit.Body.Expr;

						for (var i = 0; i < body.Bindings.Count; i++)
						{
							var binding = body.Bindings[i];

							if (member == binding.Member)
							{
								if (binding is MemberAssignment)
								{
									var ma = binding as MemberAssignment;

									var piBinding    = memberInit.Body.Create(ma.Expression, memberInit.Body.Index(body.Bindings, MemberInit.Bindings, i));
									var piAssign     = piBinding.      Create(ma.Expression, piBinding.ConvertExpressionTo<MemberAssignment>());
									var piExpression = piAssign.       Create(ma.Expression, piAssign.Property(MemberAssignmentBind.Expression));

									if (ma.Expression is MemberExpression)
									{
										FieldWalker(
											memberInit.SourceInfo,
											piExpression.Create(ma.Expression as MemberExpression, piExpression.Convert<MemberExpression>()),
											processColumn,
											processExpression);
									}
									else
									{
										processExpression(query, piExpression, memberInit.Parameter, null);
									}

									return;
								}
							}
						}

					}
				},

				#endregion

				#region QueryInfo.SubQuery

				subQuery =>
				{
					ISqlExpression col = null;
					object         key = null;

					FieldWalker(subQuery.SourceInfo, memberExpr,
						column =>
						{
							if (!subQuery.Columns.TryGetValue(column, out col))
							{
								key = column;
								col = subQuery.SubSql.Select.Columns[subQuery.SubSql.Select.Add(column)];
							}
						},
						(q,pi,_,memberName) =>
						{
							if (!subQuery.Columns.TryGetValue(pi, out col))
							{
								var idx = subQuery.SubSql.Select.Add(ParseExpression(q, pi), memberName);
								col = subQuery.SubSql.Select.Columns[idx];
								key = pi;
							}
						});

					if (key != null)
						subQuery.Columns.Add(key, col);

					processColumn(col);
				}

				#endregion
			);
		}

		static bool IsParameter(MemberExpression ex, ParameterExpression parm)
		{
			if (ex.Expression == parm) return true;

			if (ex.Expression is MemberExpression)
				return IsParameter(ex.Expression as MemberExpression, parm);

			return false;
		}

		static bool IsMember(Expression ex, IEnumerable<MemberInfo> members)
		{
			var me = ex as MemberExpression;

			if (me != null)
			{
				var member = me.Member;
				if (member is PropertyInfo)
					member = ((PropertyInfo)member).GetGetMethod();

				foreach (var m in members)
					if (m == member)
						return true;

				return IsMember(me.Expression, members);
			}

			return false;
		}

		#endregion

		#region Expression Parser

		ISqlExpression ParseExpression(QueryInfo query, ParseInfo parseInfo)
		{
			if (parseInfo.NodeType == ExpressionType.Constant)
			{
				var c = (ConstantExpression)parseInfo.Expr;

				if (c.Value == null || IsConstant(parseInfo.Expr.Type))
					return new SqlValue(((ConstantExpression) parseInfo.Expr).Value);
			}

			if (CanBeCompiled(parseInfo))
				return BuildParameter(parseInfo).SqlParameter;

			switch (parseInfo.NodeType)
			{
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				case ExpressionType.And:
				case ExpressionType.Divide:
				case ExpressionType.ExclusiveOr:
				case ExpressionType.Modulo:
				case ExpressionType.Multiply:
				case ExpressionType.Or:
				//case ExpressionType.Power:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
					{
						var pi = parseInfo.Convert<BinaryExpression>();
						var e  = parseInfo.Expr as BinaryExpression;
						var l  = ParseExpression(query, pi.Create(e.Left,  pi.Property(Binary.Left)));
						var r  = ParseExpression(query, pi.Create(e.Right, pi.Property(Binary.Right)));

						switch (parseInfo.NodeType)
						{
							case ExpressionType.Add            :
							case ExpressionType.AddChecked     : return new SqlBinaryExpression(l, "+", r, Precedence.Additive);
							case ExpressionType.And            : return new SqlBinaryExpression(l, "&", r, Precedence.Bitwise);
							case ExpressionType.Divide         : return new SqlBinaryExpression(l, "/", r, Precedence.Multiplicative);
							case ExpressionType.ExclusiveOr    : return new SqlBinaryExpression(l, "^", r, Precedence.Bitwise);
							case ExpressionType.Modulo         : return new SqlBinaryExpression(l, "%", r, Precedence.Multiplicative);
							case ExpressionType.Multiply       : return new SqlBinaryExpression(l, "*", r, Precedence.Multiplicative);
							case ExpressionType.Or             : return new SqlBinaryExpression(l, "|", r, Precedence.Bitwise);
							//case ExpressionType.Power:
							case ExpressionType.Subtract       :
							case ExpressionType.SubtractChecked: return new SqlBinaryExpression(l, "-", r, Precedence.Additive);
						}

						break;
					}

				case ExpressionType.MemberAccess:
					{
						var pi = parseInfo.ConvertTo<MemberExpression>();
						var e  = parseInfo.Expr as MemberExpression;

						switch (e.Expression.NodeType)
						{
							case ExpressionType.Parameter:
								{
									ISqlExpression sql = null;

									FieldWalker(query, pi,
										column     => sql = column,
										(q,p,_,__) => sql = ParseExpression(q, p));

									return sql;
								}
						}

						break;
					}
			}

			throw new LinqException("'{0}' cannot be converted to SQL.", parseInfo.Expr);
		}

		public static bool IsConstant(Type type)
		{
			return type == typeof(int) || type == typeof(string);
		}

		#endregion

		#region Predicate Parser

		SqlBuilder.IPredicate ParsePredicate(QueryInfo query, ParseInfo parseInfo)
		{
			switch (parseInfo.NodeType)
			{
				case ExpressionType.Equal:
				case ExpressionType.NotEqual:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
					{
						var pi = parseInfo.Convert<BinaryExpression>();
						var e  = parseInfo.Expr as BinaryExpression;
						var l  = ParseExpression(query, pi.Create(e.Left,  pi.Property(Binary.Left)));
						var r  = ParseExpression(query, pi.Create(e.Right, pi.Property(Binary.Right)));

						SqlBuilder.Predicate.Operator op;

						switch (parseInfo.NodeType)
						{
							case ExpressionType.Equal             : op = SqlBuilder.Predicate.Operator.Equal;          break;
							case ExpressionType.NotEqual          : op = SqlBuilder.Predicate.Operator.NotEqual;       break;
							case ExpressionType.GreaterThan       : op = SqlBuilder.Predicate.Operator.Greater;        break;
							case ExpressionType.GreaterThanOrEqual: op = SqlBuilder.Predicate.Operator.GreaterOrEqual; break;
							case ExpressionType.LessThan          : op = SqlBuilder.Predicate.Operator.Less;           break;
							case ExpressionType.LessThanOrEqual   : op = SqlBuilder.Predicate.Operator.LessOrEqual;    break;
							default: throw new InvalidOperationException();
						}

						return new SqlBuilder.Predicate.ExprExpr(l, op, r);
					}
			}

			throw new InvalidOperationException();
		}

		#endregion

		#region Search Condition Parser

		void ParseSearchCondition(ICollection<SqlBuilder.Condition> conditions, QueryInfo query, ParseInfo parseInfo)
		{
			switch (parseInfo.NodeType)
			{
				case ExpressionType.AndAlso:
					{
						var pi = parseInfo.Convert<BinaryExpression>();
						var e  = parseInfo.Expr as BinaryExpression;

						ParseSearchCondition(conditions, query, pi.Create(e.Left,  pi.Property(Binary.Left)));
						ParseSearchCondition(conditions, query, pi.Create(e.Right, pi.Property(Binary.Right)));

						return;
					}

				case ExpressionType.OrElse:
					{
						var pi = parseInfo.Convert<BinaryExpression>();
						var e  = parseInfo.Expr as BinaryExpression;

						var orCondition = new SqlBuilder.SearchCondition();

						ParseSearchCondition(orCondition.Conditions, query, pi.Create(e.Left,  pi.Property(Binary.Left)));
						ParseSearchCondition(orCondition.Conditions, query, pi.Create(e.Right, pi.Property(Binary.Right)));

						orCondition.Conditions[0].IsOr = true;

						conditions.Add(new SqlBuilder.Condition(false, orCondition));

						return;
					}

				case ExpressionType.Not:
					{
						var pi = parseInfo.Convert<UnaryExpression>();
						var e  = parseInfo.Expr as UnaryExpression;

						var notCondition = new SqlBuilder.SearchCondition();

						ParseSearchCondition(notCondition.Conditions, query, pi.Create(e.Operand, pi.Property(Unary.Operand)));

						conditions.Add(new SqlBuilder.Condition(true, notCondition));

						return;
					}
			}

			var predicate = ParsePredicate(query, parseInfo);
			conditions.Add(new SqlBuilder.Condition(false, predicate));
		}

		#endregion
	}
}
