using System;
using System.Linq.Expressions;

using FExpr = System.Func<System.Linq.Expressions.Expression>;
using FParm = System.Func<BLToolkit.Data.Linq.ParseInfo<System.Linq.Expressions.ParameterExpression>, bool>;
using FTest = System.Func<BLToolkit.Data.Linq.ParseInfo<System.Linq.Expressions.Expression>, bool>;

namespace BLToolkit.Data.Linq
{
	class ParseInfo<T> : ParseInfo
		where T : Expression
	{
		public ParseInfo (T expr, Func<Expression> func)
		{
			Expr          = expr;
			ParamAccessor = func;
		}

		public T Expr;

		public ExpressionType NodeType { get { return Expr.NodeType; } }

		#region Match

		public bool Match(params Func<ParseInfo<T>,bool>[] matches)
		{
			foreach (var match in matches)
				if (match(this))
					return true;
			return false;
		}

			#region IsLambda

		public bool IsLambda(FParm[] parameters, FTest body, Func<ParseInfo<LambdaExpression>,bool> func)
		{
			var pi = this;

			if (pi.NodeType == ExpressionType.Quote)
			{
				pi = Create((pi.Expr as UnaryExpression).Operand as T, () => pi.Property<UnaryExpression>(_miOperand));
			}

			if (pi.NodeType == ExpressionType.Lambda)
			{
				var lambda = Create(pi.Expr as LambdaExpression, () => pi.ConvertTo<LambdaExpression>());

				if (lambda.Expr.Parameters.Count == parameters.Length)
					for (int i = 0; i < parameters.Length; i++)
						if (!parameters[i](Create(lambda.Expr.Parameters[i], () => lambda.Indexer(_miParameters, _miParamItem, i))))
							return false;

				return body(Create(lambda.Expr.Body, () => lambda.Property(_miBody))) && func(lambda);
			}
			
			return false;
		}

		public bool IsLambda<P>(FTest body, Func<ParseInfo<LambdaExpression>,bool> func)
		{
			return IsLambda(new FParm[] { e => e.Expr.Type == typeof(P) }, body, func);
		}

		static FParm[] _singleParam = new FParm[] { p => true };

		public bool IsLambda(int parameters, Func<ParseInfo<LambdaExpression>,bool> func)
		{
			var parms = parameters != 1? new FParm[parameters]: _singleParam;

			if (parameters != 1)
				for (int i = 0; i < parms.Length; i++)
					parms[i] = _singleParam[0];

			return IsLambda(parms, p => true, func);
		}

			#endregion

			#region IsParameter

		public bool IsParameter(FParm func)
		{
			return
				NodeType == ExpressionType.Parameter?
					func(Create(Expr as ParameterExpression, () => ConvertTo<ParameterExpression>())):
					false;
		}

		public bool IsParameter()
		{
			return IsParameter(p => true);
		}

			#endregion

			#region IsUnary

		[Obsolete]
		static bool IsUnary(ParseInfo<Expression> pi, ExpressionType nodeType, FTest func)
		{
			return
				pi.NodeType == nodeType?
					func(Create(((UnaryExpression)pi.Expr).Operand, () => pi.Property<UnaryExpression>(_miOperand))):
					false;
		}

			#endregion

			#region IsConstant

		[Obsolete]
		public bool IsConstant(Func<ParseInfo<ConstantExpression>,bool> func)
		{
			return
				NodeType == ExpressionType.Constant?
					func(Create(Expr as ConstantExpression, () => ConvertTo<ConstantExpression>())):
					false;
		}

		public bool IsConstant<CT>(Func<CT,FExpr,bool> func)
		{
			if (NodeType == ExpressionType.Constant)
			{
				var c = Expr as ConstantExpression;
				return c.Value is CT? func((CT)c.Value, () => Property<ConstantExpression>(_miValue).ConvertTo<CT>()): false;
			}

			return false;
		}

		public bool IsConstant<CT>()
		{
			return IsConstant<CT>((p1, p2) => true);
		}

			#endregion

			#region IsMethod

		public bool IsMethod(Type declaringType, string methodName, FTest[] args, Func<ParseInfo<MethodCallExpression>,bool> func)
		{
			return
				NodeType == ExpressionType.Call?
					ParseInfoExtension.IsMethod(
						Create(Expr as MethodCallExpression, () => ConvertTo<MethodCallExpression>()),
						declaringType, methodName, args, func):
					false;
		}

		public bool IsMethod(Type declaringType, string methodName, params FTest[] args)
		{
			return IsMethod(declaringType, methodName, args, p => true);
		}

			#endregion

		#endregion
	}
}
