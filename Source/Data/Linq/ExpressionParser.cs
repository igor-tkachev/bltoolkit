using System;
using System.Linq;
using System.Linq.Expressions;

using BLToolkit.Mapping;

namespace BLToolkit.Data.Linq
{
	using Sql;

	public class ExpressionParser
	{
		public ExpressionParser(MappingSchema mappingSchema)
		{
			_mappingSchema = mappingSchema;
		}

		SqlBuilder    _sql = new SqlBuilder();
		MappingSchema _mappingSchema;

		public SqlBuilder Parse(Expression expression)
		{
			if (expression.NodeType == ExpressionType.Call)
				ParseMethod((MethodCallExpression)expression);
			else
				throw new ArgumentException("Method call expected.", "expression");

			return _sql;
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
						var table = new SqlTable(_mappingSchema, value.ElementType) { Alias = alias };

						_sql.From.Tables.Add(new SqlBuilder.TableSource(table, alias));

						if (_sql.Select.Columns.Count == 0)
							_sql.Select.Field(table.All);
					}
					else
						throw new InvalidOperationException(string.Format("Unexpected constant '{0}'.", @const.Value));

					break;

				case ExpressionType.Call: ParseMethod((MethodCallExpression)expr); break;
				default                 : throw new ArgumentException("Method call expected.", "expression");
			}
		}

		public void ParseMethod1(MethodCallExpression expression)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.Call:               break;
				case ExpressionType.Add:                break;
				case ExpressionType.AddChecked:         break;
				case ExpressionType.And:                break;
				case ExpressionType.AndAlso:            break;
				case ExpressionType.ArrayIndex:         break;
				case ExpressionType.ArrayLength:        break;
				case ExpressionType.Coalesce:           break;
				case ExpressionType.Conditional:        break;
				case ExpressionType.Constant:           break;
				case ExpressionType.Convert:            break;
				case ExpressionType.ConvertChecked:     break;
				case ExpressionType.Divide:             break;
				case ExpressionType.Equal:              break;
				case ExpressionType.ExclusiveOr:        break;
				case ExpressionType.GreaterThan:        break;
				case ExpressionType.GreaterThanOrEqual: break;
				case ExpressionType.Invoke:             break;
				case ExpressionType.Lambda:             break;
				case ExpressionType.LeftShift:          break;
				case ExpressionType.LessThan:           break;
				case ExpressionType.LessThanOrEqual:    break;
				case ExpressionType.ListInit:           break;
				case ExpressionType.MemberAccess:       break;
				case ExpressionType.MemberInit:         break;
				case ExpressionType.Modulo:             break;
				case ExpressionType.Multiply:           break;
				case ExpressionType.MultiplyChecked:    break;
				case ExpressionType.Negate:             break;
				case ExpressionType.NegateChecked:      break;
				case ExpressionType.New:                break;
				case ExpressionType.NewArrayBounds:     break;
				case ExpressionType.NewArrayInit:       break;
				case ExpressionType.Not:                break;
				case ExpressionType.NotEqual:           break;
				case ExpressionType.Or:                 break;
				case ExpressionType.OrElse:             break;
				case ExpressionType.Parameter:          break;
				case ExpressionType.Power:              break;
				case ExpressionType.Quote:              break;
				case ExpressionType.RightShift:         break;
				case ExpressionType.Subtract:           break;
				case ExpressionType.SubtractChecked:    break;
				case ExpressionType.TypeAs:             break;
				case ExpressionType.TypeIs:             break;
				case ExpressionType.UnaryPlus:          break;
				default:
					break;
			}
		}
	}
}
