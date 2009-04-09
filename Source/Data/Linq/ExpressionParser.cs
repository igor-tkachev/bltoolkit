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
		public ExpressionParser()
		{
			_info.SqlBuilder = new SqlBuilder();
		}

		readonly ExpressionInfo<T>   _info            = new ExpressionInfo<T>();
		readonly ParameterExpression _expressionParam = Expression.Parameter(typeof(Expression),    "expr");
		readonly ParameterExpression _dataReaderParam = Expression.Parameter(typeof(IDataReader),   "rd");
		readonly ParameterExpression _mapSchemaParam  = Expression.Parameter(typeof(MappingSchema), "ms");

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

			if (info.IsConstant<IQueryable>((value, _) =>
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

		#region Where

		QueryInfo ParseWhere(QueryInfo select, ParseInfo<ParameterExpression> parm, ParseInfo body)
		{
			SetAlias(select, parm.Expr.Name);

			if (CheckForSubQuery(select, parm, body))
			{
				var subQuery = new QueryInfo.SubQuery(select, _info.SqlBuilder);

				_info.SqlBuilder = subQuery.SqlBuilder;

				select = subQuery;
			}

			_info.SqlBuilder.Where.SearchCondition.Conditions.Add(ParseSearchCondition(select, body));

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

		#region Field Walker

		static void FieldWalker(
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
								key = pi;
								col = subQuery.SubSql.Select.Columns[subQuery.SubSql.Select.Add(ParseExpression(q, pi), memberName)];
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

		static ISqlExpression ParseExpression(QueryInfo query, ParseInfo parseInfo)
		{
			switch (parseInfo.NodeType)
			{
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				case ExpressionType.And:
				case ExpressionType.AndAlso:
				case ExpressionType.ArrayIndex:
				case ExpressionType.Coalesce:
				case ExpressionType.Divide:
				case ExpressionType.ExclusiveOr:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LeftShift:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.Modulo:
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
				case ExpressionType.NotEqual:
				case ExpressionType.Or:
				case ExpressionType.OrElse:
				case ExpressionType.Power:
				case ExpressionType.RightShift:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
					{
						var pi = parseInfo.Convert<BinaryExpression>();
						var e  = parseInfo.Expr as BinaryExpression;
						var l  = ParseExpression(query, pi.Create(e.Left,  pi.Property(Binary.Left)));
						var r  = ParseExpression(query, pi.Create(e.Right, pi.Property(Binary.Right)));

						switch (parseInfo.NodeType)
						{
							case ExpressionType.Add:
							case ExpressionType.AddChecked:
								return new SqlExpression("({0} + {1})", l, r);

							case ExpressionType.And:
							case ExpressionType.AndAlso:
							case ExpressionType.ArrayIndex:
							case ExpressionType.Coalesce:
								var c = ParseExpression(query, pi.Create(e.Conversion, pi.Property(Binary.Conversion)));
								break;

							case ExpressionType.Divide:
							case ExpressionType.ExclusiveOr:
							case ExpressionType.GreaterThan:
							case ExpressionType.GreaterThanOrEqual:
							case ExpressionType.LeftShift:
							case ExpressionType.LessThan:
							case ExpressionType.LessThanOrEqual:
							case ExpressionType.Modulo:
							case ExpressionType.Multiply:
							case ExpressionType.MultiplyChecked:
							case ExpressionType.NotEqual:
							case ExpressionType.Or:
							case ExpressionType.OrElse:
							case ExpressionType.Power:
							case ExpressionType.RightShift:
							case ExpressionType.Subtract:
							case ExpressionType.SubtractChecked:
								break;
						}

						throw new NotImplementedException();
					}

				case ExpressionType.ArrayLength:
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
				case ExpressionType.Not:
				case ExpressionType.Quote:
				case ExpressionType.TypeAs:
				case ExpressionType.UnaryPlus:
					{
						var pi = parseInfo.Convert<UnaryExpression>();
						var e  = parseInfo.Expr as UnaryExpression;
						var o  = ParseExpression(query, pi.Create(e.Operand, pi.Property(Unary.Operand)));

						switch (parseInfo.NodeType)
						{
							case ExpressionType.ArrayLength:
							case ExpressionType.Convert:
							case ExpressionType.ConvertChecked:
							case ExpressionType.Negate:
							case ExpressionType.NegateChecked:
							case ExpressionType.Not:
							case ExpressionType.Quote:
							case ExpressionType.TypeAs:
							case ExpressionType.UnaryPlus:
								break;
						}

						throw new NotImplementedException();
					}

				case ExpressionType.Call:
					{
						throw new NotImplementedException();

						/*
						var pi = parseInfo.Convert<MethodCallExpression>();
						var e  = parseInfo.Expr as MethodCallExpression;
						var o  = pi.Walk(e.Object,    MethodCall.Object,    func);
						var a  = pi.Walk(e.Arguments, MethodCall.Arguments, func);

						if (o != e.Object || a != e.Arguments)
							pi.Expr = Expression.Call(o.Expr, e.Method, a);

						return pi;
						*/
					}

				case ExpressionType.Conditional:
					{
						throw new NotImplementedException();

						/*
						var pi = parseInfo.Convert<ConditionalExpression>();
						var e  = parseInfo.Expr as ConditionalExpression;
						var s  = pi.Walk(e.Test,    Conditional.Test,    func);
						var t  = pi.Walk(e.IfTrue,  Conditional.IfTrue,  func);
						var f  = pi.Walk(e.IfFalse, Conditional.IfFalse, func);

						if (s != e.Test || t != e.IfTrue || f != e.IfFalse)
							pi.Expr = Expression.Condition(s.Expr, t.Expr, f.Expr);

						return pi;
						*/
					}

				case ExpressionType.Constant:
					{
						var pi = parseInfo.Convert<ConstantExpression>();
						var e  = parseInfo.Expr as ConstantExpression;

						if (IsConstant(e.Type))
							return new SqlValue(e.Value);

						throw new NotImplementedException();
					}

				case ExpressionType.Invoke:
					{
						throw new NotImplementedException();

						/*
						var pi = parseInfo.Convert<InvocationExpression>();
						var e  = parseInfo.Expr as InvocationExpression;
						var ex = pi.Walk(e.Expression, Invocation.Expression, func);
						var a  = pi.Walk(e.Arguments,  Invocation.Arguments,  func);

						if (ex != e.Expression || a != e.Arguments)
							pi.Expr = Expression.Invoke(ex, a);

						return pi;
						*/
					}

				case ExpressionType.Lambda:
					{
						throw new NotImplementedException();

						/*
						var pi = parseInfo.Convert<LambdaExpression>();
						var e  = parseInfo.Expr as LambdaExpression;
						var b  = pi.Walk(e.Body,       Lambda.Body,       func);
						var p  = pi.Walk(e.Parameters, Lambda.Parameters, func);

						if (b != e.Body || p != e.Parameters)
							pi.Expr = Expression.Lambda(b, p.ToArray());

						return pi;
						*/
					}

				case ExpressionType.ListInit:
					{
						throw new NotImplementedException();

						/*
						var pi = parseInfo.Convert<ListInitExpression>();
						var e  = parseInfo.Expr as ListInitExpression;
						var n  = pi.Walk(e.NewExpression, ListInit.NewExpression, func);
						var i  = pi.Walk(e.Initializers,  ListInit.Initializers, (p,pinf) =>
						{
							var args = pinf.Walk(p.Arguments, ElementInit.Arguments, func);
							return args != p.Arguments? Expression.ElementInit(p.AddMethod, args): p;
						});

						if (n != e.NewExpression || i != e.Initializers)
							pi.Expr = Expression.ListInit((NewExpression)n, i);

						return pi;
						*/
					}

				case ExpressionType.MemberAccess:
					{
						var pi = parseInfo.ConvertTo<MemberExpression>();
						var e  = parseInfo.Expr as MemberExpression;

						if (e.Expression.NodeType == ExpressionType.Parameter)
						{
							ISqlExpression sql = null;

							FieldWalker(query, pi,
								column     => sql = column,
								(q,p,_,__) => sql = ParseExpression(q, p));

							return sql;
						}

						throw new NotImplementedException();

						/*
						var ex = pi.Walk(e.Expression, Member.Expression, func);

						return pi;
						*/
					}

				case ExpressionType.MemberInit:
					{
						throw new NotImplementedException();

						/*
						Func<MemberBinding,ParseInfo,MemberBinding> modify = null; modify = (b,pinf) =>
						{
							switch (b.BindingType)
							{
								case MemberBindingType.Assignment:
									{
										var ma = (MemberAssignment)b;
										var ex = pinf.Convert<MemberAssignment>().Walk(ma.Expression, MemberAssignmentBind.Expression, func);

										if (ex != ma.Expression)
											ma = Expression.Bind(ma.Member, ex);

										return ma;
									}

								case MemberBindingType.ListBinding:
									{
										var ml = (MemberListBinding)b;
										var i  = pinf.Convert<MemberListBinding>().Walk(ml.Initializers, MemberListBind.Initializers, (p,psi) =>
										{
											var args = psi.Walk(p.Arguments, ElementInit.Arguments, func);
											return args != p.Arguments? Expression.ElementInit(p.AddMethod, args): p;
										});

										if (i != ml.Initializers)
											ml = Expression.ListBind(ml.Member, i);

										return ml;
									}

								case MemberBindingType.MemberBinding:
									{
										var mm = (MemberMemberBinding)b;
										var bs = pinf.Convert<MemberMemberBinding>().Walk(mm.Bindings, MemberMemberBind.Bindings, modify);

										if (bs != mm.Bindings)
											mm = Expression.MemberBind(mm.Member);

										return mm;
									}
							}

							return b;
						};

						var pi = parseInfo.Convert<MemberInitExpression>();
						var e  = parseInfo.Expr as MemberInitExpression;
						var ne = pi.Walk(e.NewExpression, MemberInit.NewExpression, func);
						var bb = pi.Walk(e.Bindings,      MemberInit.Bindings,      modify);

						if (ne != e.NewExpression || bb != e.Bindings)
							pi.Expr = Expression.MemberInit((NewExpression)ne, bb);

						return pi;
						*/
					}

				case ExpressionType.New:
					{
						throw new NotImplementedException();

						/*
						var pi = parseInfo.Convert<NewExpression>();
						var e  = parseInfo.Expr as NewExpression;
						var a  = pi.Walk(e.Arguments, New.Arguments, func);

						if (a != e.Arguments)
							pi.Expr = e.Members == null?
								Expression.New(e.Constructor, a):
								Expression.New(e.Constructor, a, e.Members);

						return pi;
						*/
					}

				case ExpressionType.NewArrayBounds:
					{
						throw new NotImplementedException();

						/*
						var pi = parseInfo.Convert<NewArrayExpression>();
						var e  = parseInfo.Expr as NewArrayExpression;
						var ex = pi.Walk(e.Expressions, NewArray.Expressions, func);

						if (ex != e.Expressions)
							pi.Expr = Expression.NewArrayBounds(e.Type, ex);

						return pi;
						*/
					}

				case ExpressionType.NewArrayInit:
					{
						throw new NotImplementedException();

						/*
						var pi = parseInfo.Convert<NewArrayExpression>();
						var e  = parseInfo.Expr as NewArrayExpression;
						var ex = pi.Walk(e.Expressions, NewArray.Expressions, func);

						if (ex != e.Expressions)
							pi.Expr = Expression.NewArrayInit(e.Type, ex);

						return pi;
						*/
					}

				case ExpressionType.TypeIs:
					{
						throw new NotImplementedException();

						/*
						var pi = parseInfo.Convert<TypeBinaryExpression>();
						var e  = parseInfo.Expr as TypeBinaryExpression;
						var ex = pi.Walk(e.Expression, TypeBinary.Expression, func);

						if (ex != e.Expression)
							pi.Expr = Expression.TypeIs(ex, e.Type);

						return pi;
						*/
					}

				case ExpressionType.Parameter:
					{
						throw new NotImplementedException();

						/*
						var pi = parseInfo.Convert<ParameterExpression>();
						return pi;
						*/
					}
			}

			throw new InvalidOperationException();
		}

		public static bool IsConstant(Type type)
		{
			return type == typeof(int) || type == typeof(string);
		}

		#endregion

		#region Predicate Parser

		static SqlBuilder.IPredicate ParsePredicate(QueryInfo query, ParseInfo parseInfo)
		{
			switch (parseInfo.NodeType)
			{
				case ExpressionType.Equal:
					{
						var pi = parseInfo.Convert<BinaryExpression>();
						var e  = parseInfo.Expr as BinaryExpression;
						var l  = ParseExpression(query, pi.Create(e.Left,  pi.Property(Binary.Left)));
						var r  = ParseExpression(query, pi.Create(e.Right, pi.Property(Binary.Right)));

						return new SqlBuilder.Predicate.ExprExpr(l, SqlBuilder.Predicate.Operator.Equal, r);
					}
			}

			throw new InvalidOperationException();
		}

		#endregion

		#region Search Condition Parser

		static SqlBuilder.Condition ParseSearchCondition(QueryInfo query, ParseInfo parseInfo)
		{
			var predicate = ParsePredicate(query, parseInfo);
			return new SqlBuilder.Condition(false, predicate);
		}

		#endregion
	}
}
