using System;
using System.Linq;
using System.Linq.Expressions;

using BLToolkit.Mapping;
using BLToolkit.Reflection;

using Test = System.Func<System.Linq.Expressions.Expression,bool>;
using Parm = System.Func<System.Linq.Expressions.ParameterExpression,bool>;

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

			if (!Match(expression,
					//
					// db.Table.ToList()
					//
					expr => Constant<IQueryable>(expr, value => SimpleQuery(value.ElementType, null)),
					//
					// from p in db.Table select p
					// db.Table.Select(p => p)
					//
					expr => Method(expr, typeof(Queryable), "Select", new Test[]
					{
						arg1 => Constant<IQueryable>(arg1, e => true),
						arg2 => Unary(arg2, ExpressionType.Quote, op => Lambda(op,
							new Parm[] { p => p.Type == typeof(T) },
							body => Parameter(body),
							l    => SimpleQuery(typeof(T), l.Parameters[0].Name))),
					})))
				throw new ArgumentException("Method call expected.", "expression");

			return _info;
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

		#region Match

		static bool Match(Expression expr, params Test[] matches)
		{
			foreach (var match in matches)
				if (match(expr))
					return true;
			return false;
		}

		static bool Lambda(Expression expr, Parm[] parameters, Test body, Func<LambdaExpression,bool> func)
		{
			if (expr.NodeType == ExpressionType.Lambda)
			{
				var lambda = (LambdaExpression)expr;

				if (lambda.Parameters.Count == parameters.Length)
					for (int i = 0; i < parameters.Length; i++)
						if (!parameters[i](lambda.Parameters[i]))
							return false;

				return body(lambda.Body) && func(lambda);
			}
			
			return false;
		}

		static bool Parameter(Expression expr, Parm func)
		{
			return expr.NodeType == ExpressionType.Parameter? func((ParameterExpression)expr): false;
		}

		static bool Parameter(Expression expr)
		{
			return Parameter(expr, p => true);
		}

		static bool Unary(Expression expr, ExpressionType nodeType, Test func)
		{
			return expr.NodeType == nodeType? func(((UnaryExpression)expr).Operand): false;
		}

		static bool Constant(Expression expr, Func<ConstantExpression,bool> func)
		{
			return expr.NodeType == ExpressionType.Constant? func((ConstantExpression)expr): false;
		}

		static bool Constant<CT>(Expression expr, Func<CT,bool> func)
		{
			if (expr.NodeType == ExpressionType.Constant)
			{
				var c = (ConstantExpression)expr;
				return c.Value is CT? func((CT)c.Value): false;
			}

			return false;
		}

		static bool Method(Expression expr, Type declaringType, string methodName, Test[] args, Func<MethodCallExpression,bool> func)
		{
			if (expr.NodeType == ExpressionType.Call)
			{
				var method = (MethodCallExpression)expr;

				if (declaringType == method.Method.DeclaringType && method.Method.Name == methodName && method.Arguments.Count == args.Length)
				{
					for (int i = 0; i < args.Length; i++)
						if (!args[i](method.Arguments[i]))
							return false;

					return func(method);
				}
			}

			return false;
		}

		static bool Method(Expression expr, Type declaringType, string methodName, params Test[] args)
		{
			return Method(expr, declaringType, methodName, args, f => true);
		}

		#endregion
	}
}
