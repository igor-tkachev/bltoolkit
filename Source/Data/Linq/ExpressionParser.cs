using System;
using System.Linq;
using System.Linq.Expressions;

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

		ExpressionInfo<T>   _info        = new ExpressionInfo<T>();
		ParameterExpression _lambdaParam = Expression.Parameter(typeof(Expression), "exp");

		public ExpressionInfo<T> Parse(MappingSchema mappingSchema, Expression expression)
		{
			var op = ParseInfo.Unary.Operand;

			_info.MappingSchema = mappingSchema;
			_info.Expression    = expression;

			ParseInfo.Create(expression, () => _lambdaParam).Match(
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
						body => body.IsParameter(),
						l    => SimpleQuery(typeof(T), l.Expr.Parameters[0].Name))),
				//
				// everything else
				//
				pi =>
				{
					var select = ParseSequence(pi, true);
					select.BuildSelect(_info.SqlBuilder);
					BuildQuery(select);
					return true;
				}
			);

			return _info;
		}

		void BuildQuery(QueryInfo select)
		{
			select.ParseInfo.Match(
				p => p.IsConstant<IQueryable>((_,__) =>
				{
					_info.GetIEnumerable = db => _info.Query(db, _info.SqlBuilder);
					return true;
				}),

				p => { throw new NotImplementedException(); }
			);
		}

		QueryInfo ParseSequence(ParseInfo<Expression> info, bool top)
		{
			QueryInfo select = null;

			if (info.IsConstant<IQueryable>((value, _) =>
				{
					var table = new SqlTable(_info.MappingSchema, value.ElementType);
					_info.SqlBuilder.From.Table(table);
					select = new QueryInfo(info, table);
					return true;
				}))
				return select;

			if (info.NodeType != ExpressionType.Call)
				throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", info.Expr), "expression");

			ParseInfo.Create((MethodCallExpression)info.Expr, () => info.ConvertTo<MethodCallExpression>()).Match
			(
				pi => pi.IsQueryableMethod("Select", seq => select = ParseSequence(seq, false), (p,b) => select = ParseSelect(select, p, b, top)),
				pi => pi.IsQueryableMethod("Where",  seq => select = ParseSequence(seq, false), (p,b) =>          ParseWhere (select, p, b)),

				pi => { throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", pi.Expr), "expression"); }
			);

			return select;
		}

		QueryInfo ParseSelect(QueryInfo select, ParseInfo<ParameterExpression> parm, ParseInfo<Expression> body, bool top)
		{
			//select.ParseInfo = body;
			select.SetAlias(parm.Expr.Name, _info.SqlBuilder);

			if (body.IsParameter())
			{
				return select;
			}
			else if (body.IsNew())
			{
				body.Walk(pi =>
				{
					return true;
				});
			}
			else
			{
				throw new NotImplementedException();
			}

			return select;
		}

		void ParseWhere(QueryInfo select, ParseInfo<ParameterExpression> parm, ParseInfo<Expression> body)
		{
		}

		bool SimpleQuery(Type type, string alias)
		{
			var table = new SqlTable(_info.MappingSchema, type) { Alias = alias };

			_info.SqlBuilder
				.Select
					.Field(table.All)
				.From
					.Table(table);

			_info.GetIEnumerable = db => _info.Query(db, _info.SqlBuilder);

			return true;
		}
	}
}
