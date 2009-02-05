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

		ExpressionInfo<T> _info = new ExpressionInfo<T>();

		public ExpressionInfo<T> Parse(MappingSchema mappingSchema, Expression expression)
		{
			_info.MappingSchema = mappingSchema;
			_info.Expression    = expression;

			switch (expression.NodeType)
			{
				case ExpressionType.Constant:
					ParseSource(expression, null);
					_info.GetIEnumerable = db => _info.Query(db, _info.SqlBuilder);

					break;

				case ExpressionType.Call:
					ParseMethod((MethodCallExpression)expression);
					break;

				default:
					throw new ArgumentException("Method call expected.", "expression");
			}

			return _info;
		}

		void ParseMethod(MethodCallExpression method)
		{
			if (method.Method.DeclaringType == typeof(Queryable))
			{
				switch (method.Method.Name)
				{
					case "Select": ParseSelect(method); break;
					default      : throw new InvalidOperationException(string.Format("Unexpected method '{}'.", method.Method.Name));
				}
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		void ParseSelect(MethodCallExpression method)
		{
			var arg1 = (LambdaExpression)((UnaryExpression)method.Arguments[1]).Operand;

			ParseSource(method.Arguments[0], arg1.Parameters[0].Name);
		}

		void ParseSource(Expression expr, string alias)
		{
			switch (expr.NodeType)
			{
				case ExpressionType.Constant:

					var @const = (ConstantExpression)expr;
					var value  = @const.Value as IQueryable;

					if (value != null)
					{
						var table = new SqlTable(_info.MappingSchema, value.ElementType) { Alias = alias };

						_info.SqlBuilder.From.Tables.Add(new SqlBuilder.TableSource(table, alias));

						if (_info.SqlBuilder.Select.Columns.Count == 0)
							_info.SqlBuilder.Select.Field(table.All);
					}
					else
						throw new InvalidOperationException(string.Format("Unexpected constant '{0}'.", @const.Value));

					break;

				case ExpressionType.Call: ParseMethod((MethodCallExpression)expr); break;
				default                 : throw new ArgumentException("Method call expected.", "expression");
			}
		}
	}
}
