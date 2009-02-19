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
			var op = ParseInfo._miOperand;

			_info.MappingSchema = mappingSchema;
			_info.Expression    = expression;

			ParseInfo.Create(expression, () => _lambdaParam).Match(
				//
				// db.Table.ToList()
				//
				pi => pi.IsConstant<IQueryable>((value, pa) => SimpleQuery(value.ElementType, null)),
				//
				// from p in db.Table select p
				// db.Table.Select(p => p)
				//
				pi => pi.IsMethod(typeof(Queryable), "Select",
					obj => obj.IsConstant<IQueryable>(),
					arg => arg.IsLambda<T>(
						body => body.IsParameter(),
						l    => SimpleQuery(typeof(T), l.Expr.Parameters[0].Name))),

				pi =>
				{
					var select = ParseSequence(pi);
					return true;
				}
			);

			return _info;
		}

		SelectInfo ParseSequence(ParseInfo<Expression> info)
		{
			SelectInfo select = null;

			if (info.IsConstant<IQueryable>((value, pi) =>
				{
					var table = new SqlTable(_info.MappingSchema, value.ElementType);
					select = new SelectInfo(value.ElementType, table);
					_info.SqlBuilder.From.Table(table);
					return true;
				}))
				return select;

			if (info.NodeType != ExpressionType.Call)
				throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'", info.Expr), "expression");

			ParseInfo.Create((MethodCallExpression)info.Expr, () => info.ConvertTo<MethodCallExpression>()).Match(
				pi => pi.IsQueryableMethod("Select",
					seq => { select = ParseSequence(seq); },
					arg => arg.IsLambda<T>(
						body => body.IsParameter(),
						l    => SimpleQuery(typeof(T), l.Expr.Parameters[0].Name))),

				pi => pi.IsQueryableMethod("Where",
					seq => { select = ParseSequence(seq); },
					arg => arg.IsLambda(1, l => { ParseWhere(l); return true; })),

				pi => { throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'", pi.Expr), "expression"); }
			);

			return select;
		}

		void ParseWhere(ParseInfo<LambdaExpression> lambda)
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
