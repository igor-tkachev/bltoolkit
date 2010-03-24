using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	class ExpressionParser : ReflectionHelper
	{
		static protected readonly ParameterExpression ParametersParam = Expression.Parameter(typeof(object[]),   "ps");
		       protected readonly ParameterExpression ExpressionParam = Expression.Parameter(typeof(Expression), "expr");

		protected Dictionary<Expression,Expression> ExpressionAccessors;

		#region ParentQueries

		protected class ParentQuery
		{
			public QuerySource         Parent;
			public ParameterExpression Parameter;
		}

		protected readonly List<ParentQuery> ParentQueries = new List<ParentQuery>();

		#endregion
	}
}
