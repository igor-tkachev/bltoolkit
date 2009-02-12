using System;
using System.Linq;
using System.Linq.Expressions;

using BLToolkit.Mapping;
using BLToolkit.Reflection;

using FExpr = System.Func<System.Linq.Expressions.Expression>;
using FTest = System.Func<System.Linq.Expressions.Expression,System.Func<System.Linq.Expressions.Expression>,bool>;
using FParm = System.Func<System.Linq.Expressions.ParameterExpression,System.Func<System.Linq.Expressions.Expression>,bool>;

namespace BLToolkit.Data.Linq
{
	using Sql;
using System.Reflection;
	using System.Collections.ObjectModel;

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
			_info.MappingSchema = mappingSchema;
			_info.Expression    = expression;

			FExpr paramAccessor = () => _lambdaParam;

			Match(
				expression,
				//
				// db.Table.ToList()
				//
				expr => IsConstant<IQueryable>(expr, paramAccessor, (value, pa) => SimpleQuery(value.ElementType, null)),
				//
				// from p in db.Table select p
				// db.Table.Select(p => p)
				//
				expr => IsMethod(expr, paramAccessor, typeof(Queryable), "Select",
					(obj, pa) => IsConstant<IQueryable>(obj, pa),
					(arg, pa) => IsLambda<T>(arg, pa,
						(body, ba) => IsParameter(body, ba),
						(l,w)      => SimpleQuery(typeof(T), l.Parameters[0].Name))),

				expr => ParseSequence(expr, () => _lambdaParam)
			);

			return _info;
		}

		bool ParseSequence(Expression expr, FExpr prevParamAccessor)
		{
			if (IsConstant<IQueryable>(expr, prevParamAccessor, (value, pa) =>
				{
					_info.SqlBuilder.From.Table(new SqlTable(_info.MappingSchema, value.ElementType));
					return true;
				}))
				return true;

			if (expr.NodeType != ExpressionType.Call)
				throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'", expr), "expression");

			Func<Expression> methodParamAccessor = () => Expression.Convert(prevParamAccessor(), typeof(MethodCallExpression));

			var method = (MethodCallExpression)expr;

			Match(method,
				m => IsQueryableMethod(m, methodParamAccessor, "Select",
					(seq, pa) => ParseSequence(seq, pa),
					(arg, pa) => IsLambda<T>(arg, pa,
						(body, ba) => IsParameter(body, ba),
						(l,w)      => SimpleQuery(typeof(T), l.Parameters[0].Name))),

				m => IsQueryableMethod(m, methodParamAccessor, "Where",
					(seq, pa) => ParseSequence(seq, pa),
					(arg, pa) => IsLambda(arg, pa, 1, (l,w) => { /*ParseWhere(l, WrapLambda(pa, w));*/ return true; })),

				m => { throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'", m), "expression"); }
			);

			return true;
		}

		void ParseWhere(LambdaExpression lambda, Func<Expression> prevParamAccessor)
		{
		}

		#region Parameter accessor helpers

		Func<Expression> WrapLambda(Func<Expression> paramAccessor, bool wrap)
		{
			return wrap? () => Expression.Quote(paramAccessor()): paramAccessor;
		}

		#endregion

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

		#region Reflection Helpers

		static Expression LambdaExpressor<P>(Expression<Func<P,object>> func)
		{
			return func.Body;
		}

		static PropertyInfo _miArguments = (PropertyInfo)((MemberExpression)LambdaExpressor<MethodCallExpression>          (expr => expr.Arguments)).Member;
		static MethodInfo   _miItem      = ((MethodCallExpression)          LambdaExpressor<ReadOnlyCollection<Expression>>(col  => col[0])).        Method;

		#endregion

		#region Match

		static bool Match<E>(E expr, params Func<E,bool>[] matches)
		{
			foreach (var match in matches)
				if (match(expr))
					return true;
			return false;
		}

		static bool IsLambda(Expression expr, FExpr paramAccessor, FParm[] parameters, FTest body, Func<LambdaExpression,FExpr,bool> func)
		{
			if (expr.NodeType == ExpressionType.Quote)
			{
				expr = ((UnaryExpression)expr).Operand;
			}

			if (expr.NodeType == ExpressionType.Lambda)
			{
				var lambda = (LambdaExpression)expr;

				if (lambda.Parameters.Count == parameters.Length)
					for (int i = 0; i < parameters.Length; i++)
						if (!parameters[i](lambda.Parameters[i], paramAccessor))
							return false;

				return body(lambda.Body, paramAccessor) && func(lambda, null);
			}
			
			return false;
		}

		static bool IsLambda<P>(Expression expr, FExpr paramAccessor, FTest body, Func<LambdaExpression,FExpr,bool> func)
		{
			return IsLambda(expr, paramAccessor, new FParm[] { (p, pa) => p.Type == typeof(P) }, body, func);
		}

		static FParm[] _singleParam = new FParm[] { (p, pa) => true };

		static bool IsLambda(Expression expr, FExpr paramAccessor, int parameters, Func<LambdaExpression,FExpr,bool> func)
		{
			var parms = parameters != 1? new FParm[parameters]: _singleParam;

			if (parameters != 1)
				for (int i = 0; i < parms.Length; i++)
					parms[i] = _singleParam[0];

			return IsLambda(expr, paramAccessor, parms, (body, pa) => true, func);
		}

		static bool IsParameter(Expression expr, FExpr paramAccessor, FParm func)
		{
			return expr.NodeType == ExpressionType.Parameter? func((ParameterExpression)expr, paramAccessor): false;
		}

		static bool IsParameter(Expression expr, FExpr paramAccessor)
		{
			return IsParameter(expr, paramAccessor, (p, pa) => true);
		}

		static bool IsUnary(Expression expr, FExpr paramAccessor, ExpressionType nodeType, FTest func)
		{
			return expr.NodeType == nodeType? func(((UnaryExpression)expr).Operand, paramAccessor): false;
		}

		static bool IsConstant(Expression expr, FExpr paramAccessor, Func<ConstantExpression,FExpr,bool> func)
		{
			return expr.NodeType == ExpressionType.Constant? func((ConstantExpression)expr, () => Expression.Constant(paramAccessor())): false;
		}

		static bool IsConstant<CT>(Expression expr, FExpr paramAccessor, Func<CT,FExpr,bool> func)
		{
			if (expr.NodeType == ExpressionType.Constant)
			{
				var c = (ConstantExpression)expr;
				return c.Value is CT? func((CT)c.Value, () => Expression.Constant(paramAccessor())): false;
			}

			return false;
		}

		static bool IsConstant<CT>(Expression expr, FExpr paramAccessor)
		{
			return IsConstant<CT>(expr, paramAccessor, (p, pa) => true);
		}

		static bool IsMethod(
			MethodCallExpression method,
			FExpr                paramAccessor,
			Type                 declaringType,
			string               methodName,
			FTest[]              args,
			Func<MethodCallExpression,FExpr,bool> func)
		{
			if (declaringType == method.Method.DeclaringType && method.Method.Name == methodName && method.Arguments.Count == args.Length)
			{
				for (int i = 0; i < args.Length; i++)
				{
					Func<Expression> pa = () =>
						Expression.Call(
							Expression.Property(paramAccessor(), _miArguments),
							_miItem,
							new Expression[] { Expression.Constant(i, typeof(int)) });

					if (!args[i](method.Arguments[i], pa))
						return false;
				}

				return func(method, paramAccessor);
			}

			return false;
		}

		static bool IsMethod(MethodCallExpression method, FExpr paramAccessor, Type declaringType, string methodName, params FTest[] args)
		{
			return IsMethod(method, paramAccessor, declaringType, methodName, args, (p, pa) => true);
		}

		static bool IsQueryableMethod(MethodCallExpression method, FExpr paramAccessor, string methodName, params FTest[] args)
		{
			return IsMethod(method, paramAccessor, typeof(Queryable), methodName, args, (p, pa) => true);
		}

		static bool IsMethod(
			Expression expr,
			FExpr      paramAccessor,
			Type       declaringType,
			string     methodName,
			FTest[]    args,
			Func<MethodCallExpression,FExpr,bool> func)
		{
			return expr.NodeType == ExpressionType.Call? IsMethod((MethodCallExpression)expr, paramAccessor, declaringType, methodName, args, func) : false;
		}

		static bool IsMethod(Expression expr, FExpr paramAccessor, Type declaringType, string methodName, params FTest[] args)
		{
			return IsMethod(expr, paramAccessor, declaringType, methodName, args, (p, pa) => true);
		}

		#endregion
	}
}
