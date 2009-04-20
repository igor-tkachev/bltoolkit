using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using BLToolkit.Data.DataProvider;

namespace BLToolkit.Data.Linq
{
	using Mapping;
	using Reflection;
	using Sql;

	class ExpressionParser<T> : ReflectionHelper
	{
		#region Init

		public ExpressionParser()
		{
			_info.SqlBuilder = new SqlBuilder();
		}

		readonly ExpressionInfo<T>   _info            = new ExpressionInfo<T>();
		readonly ParameterExpression _expressionParam = Expression.Parameter(typeof(Expression),        "expr");
		readonly ParameterExpression _dataReaderParam = Expression.Parameter(typeof(IDataReader),       "rd");
		readonly ParameterExpression _mapSchemaParam  = Expression.Parameter(typeof(MappingSchema),     "ms");
		readonly ParameterExpression _infoParam       = Expression.Parameter(typeof(ExpressionInfo<T>), "info");

		#endregion

		#region Parse

		public ExpressionInfo<T> Parse(DataProviderBase dataProvider, MappingSchema mappingSchema, Expression expression)
		{
			_info.DataProvider  = dataProvider;
			_info.MappingSchema = mappingSchema;
			_info.Expression    = expression;

			ParseInfo.CreateRoot(expression, _expressionParam).Match(
				//
				// db.Select(() => 1)
				// db.Select(() => @p)
				//
				pi => pi.IsLambda(0, body => body.IsConstant(ConstantQuery) || body.IsMemberAccess((ex,_) => ConstantQuery(ex))),
				//
				// db.Select(() => new { ... })
				//
				pi => pi.IsLambda((_,body) => BuildNew(ParseSelect(null, null, body), body)),
				//
				// db.Table.ToList()
				//
				pi => pi.IsConstant<IQueryable>((value,_) => SimpleQuery(value.ElementType, null)),
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

		QuerySource ParseSequence(ParseInfo<Expression> info)
		{
			QuerySource select = null;

			if (info.IsConstant<IQueryable>((value,expr) =>
				{
					select = new QuerySource.Table(_info.MappingSchema, _info.SqlBuilder, expr);
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

		QuerySource ParseSelect(QuerySource select, ParseInfo<ParameterExpression> parm, ParseInfo body)
		{
			if (select != null)
				SetAlias(select, parm.Expr.Name);

			switch (body.NodeType)
			{
				case ExpressionType.Parameter   : return select;
				case ExpressionType.New         : return new QuerySource.Expr        (_info.SqlBuilder, select, body.ConvertTo<NewExpression>());
				case ExpressionType.MemberInit  : return new QuerySource.Expr        (_info.SqlBuilder, select, body.ConvertTo<MemberInitExpression>());
				case ExpressionType.MemberAccess: return new QuerySource.MemberAccess(_info.SqlBuilder, select, body.ConvertTo<MemberExpression>());
				default                         : throw  new InvalidOperationException();
			}
		}

		#endregion

		#region Parse Where

		QuerySource ParseWhere(QuerySource select, ParseInfo<ParameterExpression> parm, ParseInfo body)
		{
			SetAlias(select, parm.Expr.Name);

			if (CheckForSubQuery(select, body))
			{
				var subQuery = new QuerySource.SubQuery(_info.SqlBuilder, select);

				_info.SqlBuilder = subQuery.SqlBuilder;

				select = subQuery;
			}

			ParseSearchCondition(_info.SqlBuilder.Where.SearchCondition.Conditions, select, body);

			return select;
		}

		static bool CheckForSubQuery(QuerySource query, ParseInfo expr)
		{
			var makeSubquery = false;

			expr.Walk(pi =>
			{
				if (pi.NodeType == ExpressionType.MemberAccess)
				{
					var field = query.GetField(pi.Expr);

					if (field is QueryField.ExprColumn)
						makeSubquery = pi.StopWalking = true;
				}

				return pi;
			});

			return makeSubquery;
		}

		#endregion

		#region Build Select

		void BuildSelect(QuerySource query)
		{
			query.Match
			(
				table    => { table.Select(this); _info.SetQuery(); }, // QueryInfo.Table
				expr     => BuildNew(query, expr.Expression),          // QueryInfo.Expr
				subQuery => _info.SetQuery(),                          // QueryInfo.SubQuery
				member   => BuildNew(query, member.Expression)         // QueryInfo.Member
			);
		}

		void BuildNew(QuerySource query, ParseInfo expr)
		{
			var info = BuildNewExpression(query, expr);

			var mapper = Expression.Lambda<Func<ExpressionInfo<T>,IDataReader,MappingSchema,Expression,T>>(
				info, new [] { _infoParam, _dataReaderParam, _mapSchemaParam, _expressionParam });

			_info.SetQuery(mapper.Compile());
		}

		ParseInfo BuildNewExpression(QuerySource query, ParseInfo expr)
		{
			return expr.Walk(pi =>
			{
				if (pi.NodeType == ExpressionType.MemberAccess)
				{
					var ma = pi.ConvertTo<MemberExpression>();
					var ex = pi.Create(((MemberExpression)pi.Expr).Expression, pi.Property(Member.Expression));

					if (query.ParentQuery != null)
					{
						var field = query.ParentQuery.GetField(ma);

						if (field != null)
						{
							if (field is QueryField.Column || field is QueryField.SubQueryColumn)
								return BuildField(ma, field);

							if (field is QueryField.ExprColumn)
							{
								var col = (QueryField.ExprColumn)field;

								pi = BuildNewExpression(col.QuerySource, col.Expr);
								pi.IsReplaced = pi.StopWalking = true;

								return pi;
							}

							if (field is QuerySource.Table)
							{
								var table = (QuerySource.Table) field;
								var index = table.Select(this).Select(i => Expression.Constant(i, typeof(int)) as Expression);

								return ma.Parent.Replace(
									Expression.Convert(
										Expression.Call(_infoParam, _info.GetMapperMethodInfo(),
											Expression.Constant(table.ObjectType, typeof(Type)),
											_dataReaderParam,
											Expression.Constant(_info.GetMapperSlot(), typeof(int)),
											Expression.NewArrayInit(typeof(int), index)),
										table.ObjectType),
									ma.ParamAccessor);
							}

							throw new InvalidOperationException();
						}
					}
					else
					{
						var field = query.GetField(ma);

						if (field != null)
							return BuildField(ma, field);
					}

					if (ex.NodeType == ExpressionType.Constant)
					{
						// field = localVariable
						//
						var c = ex.Parent.Create((ConstantExpression)ex.Expr, ex.Property<ConstantExpression>(Constant.Value));

						return pi.Parent.Replace(
							Expression.MakeMemberAccess(
								Expression.Convert(c.ParamAccessor, ex.Expr.Type),
								ma.Expr.Member),
							c.ParamAccessor);
					}
				}
				else if (pi.NodeType == ExpressionType.Parameter)
				{
					var field = query.ParentQuery.GetField(pi.Expr);

					if (field != null)
					{
						if (field is QuerySource.Table)
						{
							var table = (QuerySource.Table)field;
							var index = table.Select(this).Select(i => Expression.Constant(i, typeof(int)) as Expression);

							return pi.Parent.Replace(
								Expression.Convert(
									Expression.Call(_infoParam, _info.GetMapperMethodInfo(),
										Expression.Constant(table.ObjectType, typeof(Type)),
										_dataReaderParam,
										Expression.Constant(_info.GetMapperSlot(), typeof(int)),
										Expression.NewArrayInit(typeof(int), index)),
									table.ObjectType),
								pi.ParamAccessor);
						}

						if (field is QuerySource.MemberAccess)
						{
							var ma = (QuerySource.MemberAccess)field;
							return BuildNewExpression(ma, ma.Expression);
						}

						throw new InvalidOperationException();
					}
				}
				else if (pi.NodeType == ExpressionType.Constant)
				{
					if (query.ParentQuery == null)
					{
						var field = query.GetField(pi);

						if (field != null)
						{
							var idx = field.Select(this);
							return BuildField(pi, idx, pi.Expr.Type);
						}
					}
				}

				return pi;
			});
		}

		ParseInfo BuildField(ParseInfo<MemberExpression> ma, QueryField field)
		{
			var memberType = ma.Expr.Member.MemberType == MemberTypes.Field ?
				((FieldInfo)   ma.Expr.Member).FieldType :
				((PropertyInfo)ma.Expr.Member).PropertyType;

			var idx = field.Select(this);

			return BuildField(ma, idx, memberType);
		}

		private ParseInfo BuildField(ParseInfo ma, int[] idx, Type memberType)
		{
			MethodInfo mi;

			if (!MapSchema.Converters.TryGetValue(memberType, out mi))
				throw new LinqException("Cannot find converter for the '{0}' type.", memberType.FullName);

			return ma.Parent.Replace(
				Expression.Call(_mapSchemaParam, mi,
					Expression.Call(_dataReaderParam, DataReader.GetValue,
						Expression.Constant(idx[0], typeof(int)))),
				ma.ParamAccessor);
		}

		static void SetAlias(QuerySource query, string alias)
		{
			query.Match
			(
				table  =>
				{
					if (table.SqlTable.Alias == null)
						table.SqlTable.Alias = alias;
				},
				_ => {},
				subQuery =>
				{
					var table = subQuery.SqlBuilder.From.Tables[0];
					if (table.Alias == null)
						table.Alias = alias;
				},
				_ => {}
			);
		}

		bool SimpleQuery(Type type, string alias)
		{
			var table = new SqlTable(_info.MappingSchema, type) { Alias = alias };

			foreach (var field in table.Fields.Values)
				_info.SqlBuilder.Select.Expr(field);

			_info.SqlBuilder.From.Table(table);
			_info.SetQuery();

			return true;
		}

		private bool ConstantQuery(ParseInfo parseInfo)
		{
			var expr = ParseExpression(null, parseInfo);

			_info.SqlBuilder.Select.Expr(expr);

			var pi = BuildField(parseInfo, new[] {0}, parseInfo.Expr.Type);

			var mapper = Expression.Lambda<Func<ExpressionInfo<T>,IDataReader,MappingSchema,Expression,T>>(
				pi, new [] { _infoParam, _dataReaderParam, _mapSchemaParam, _expressionParam });

			_info.SetQuery(mapper.Compile());

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
			var canbe = true;

			expr.Walk(pi =>
			{
				if (pi.NodeType == ExpressionType.Parameter)
					canbe = false;

				return pi;
			});

			return canbe;
		}

		#endregion

		#region Expression Parser

		public ISqlExpression ParseExpression(QuerySource query, ParseInfo parseInfo)
		{
			if (parseInfo.NodeType == ExpressionType.Parameter && query is QuerySource.MemberAccess)
			{
				var ma = (QuerySource.MemberAccess)query;
				return ParseExpression(ma.ParentQuery, ma.Expression);
			}

			if (parseInfo.NodeType == ExpressionType.Constant)
			{
				var c = (ConstantExpression)parseInfo.Expr;

				if (c.Value == null || IsConstant(parseInfo.Expr.Type))
					return new SqlValue(((ConstantExpression)parseInfo.Expr).Value);
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
						var field = query.GetField(parseInfo.Expr);

						if (field != null)
							return field.GetExpression(this);

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

		SqlBuilder.IPredicate ParsePredicate(QuerySource query, ParseInfo parseInfo)
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

		void ParseSearchCondition(ICollection<SqlBuilder.Condition> conditions, QuerySource query, ParseInfo parseInfo)
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
