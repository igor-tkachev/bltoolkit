using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using BLToolkit.Mapping;

namespace BLToolkit.Data.Linq
{
	using Sql;

	class ExpressionParser<T>
	{
		public ExpressionParser()
		{
			_info.SqlBuilder = new SqlBuilder();
		}

		ExpressionInfo<T>   _info            = new ExpressionInfo<T>();
		ParameterExpression _expressionParam = Expression.Parameter(typeof(Expression),    "expr");
		ParameterExpression _dataReaderParam = Expression.Parameter(typeof(IDataReader),   "rd");
		ParameterExpression _mapSchemaParam  = Expression.Parameter(typeof(MappingSchema), "ms");

		public ExpressionInfo<T> Parse(MappingSchema mappingSchema, Expression expression)
		{
			var op = ParseInfo.Unary.Operand;

			_info.MappingSchema = mappingSchema;
			_info.Expression    = expression;

			ParseInfo.Create(expression, () => _expressionParam).Match(
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
				throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", info.Expr), "expression");

			info.ConvertTo<MethodCallExpression>().Match
			(
				pi => pi.IsQueryableMethod("Select", seq => select = ParseSequence(seq), (p, b) => select = ParseSelect(select, p, b)),
				pi => pi.IsQueryableMethod("Where",  seq => select = ParseSequence(seq), (p, b) => ParseWhere(select, p, b)),

				pi => { throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", pi.Expr), "expression"); }
			);

			return select;
		}

		QueryInfo ParseSelect(QueryInfo select, ParseInfo<ParameterExpression> parm, ParseInfo<Expression> body)
		{
			SetAlias(select, parm.Expr.Name);

			switch (body.NodeType)
			{
				case ExpressionType.Parameter : return select;
				case ExpressionType.New       : return new QueryInfo.New       (select, parm, body.ConvertTo<NewExpression>());
				case ExpressionType.MemberInit: return new QueryInfo.MemberInit(select, parm, body.ConvertTo<MemberInitExpression>());
				default                       : throw  new NotImplementedException();
			}
		}

		void ParseWhere(QueryInfo select, ParseInfo<ParameterExpression> parm, ParseInfo<Expression> body)
		{
			throw  new NotImplementedException();
		}

		void BuildSelect(QueryInfo query)
		{
			query.Match(
				constantExpr => // QueryInfo.Constant
				{
					foreach (var field in constantExpr.Table.Fields.Values)
						_info.SqlBuilder.Select.Expr(field);

					_info.SetQuery();
				},
				newExpr    => BuildNew(query, newExpr.   Body, newExpr.   Parameter), // QueryInfo.New
				memberInit => BuildNew(query, memberInit.Body, memberInit.Parameter)  // QueryInfo.MemberInit
			);
		}

		void BuildNew(QueryInfo query, ParseInfo expr, ParseInfo<ParameterExpression> parm)
		{
			var info = expr.Walk(pi =>
			{
				pi.IsMemberAccess((ma,ex) =>
				{
					if (ex.Expr == parm.Expr)
					{
						var member = ma.Expr.Member;

						if (member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property)
						{
							var field = query.GetField(ma);

							if (field != null)
							{
								var idx = _info.SqlBuilder.Select.Add(field);

								var memberType = member.MemberType == MemberTypes.Field ?
									((FieldInfo)member).FieldType :
									((PropertyInfo)member).PropertyType;

								MethodInfo mi;

								if (!ParseInfo.MappingSchema.Converters.TryGetValue(memberType, out mi))
									throw new LinqException("Cannot find converter for the '{0}' type.", memberType.FullName);

								pi = ParseInfo.Create(
									Expression.Call(_mapSchemaParam, mi,
										Expression.Call(_dataReaderParam, ParseInfo.DataReader.GetValue,
											Expression.Constant(idx, typeof(int)))),
									pi.ParamAccessor);

								return true;
							}
						}
					}
					else if (ex.IsConstant(c =>
					{
						var e = Expression.MakeMemberAccess(Expression.Convert(c.ParamAccessor(), ex.Expr.Type), ma.Expr.Member);
						//var e = Expression.Convert(c.ParamAccessor(), pi.Expr.Type);
						//var e = Expression.Call(_mapSchemaParam, ParseInfo.MappingSchema.Converters[typeof(int)], c.ParamAccessor());
						pi = ParseInfo.Create(e, c.ParamAccessor); return true;
					}))
					{
						return true;
					}

					return false;
				});

				return pi;
			});

			var mapper = Expression.Lambda<Func<IDataReader,MappingSchema,Expression,T>>(
				info, new ParameterExpression[] { _dataReaderParam, _mapSchemaParam, _expressionParam });

			_info.SetQuery(mapper.Compile());
		}

		void SetAlias(QueryInfo query, string alias)
		{
			query.Match(
				constantExpr =>
				{
					if (constantExpr.Table.Alias == null)
						constantExpr.Table.Alias = alias;
				},
				_ => {},
				__ => {}
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
	}
}
