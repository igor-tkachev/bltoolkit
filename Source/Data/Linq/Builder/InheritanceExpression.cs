using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	class InheritanceExpression : Expression
	{
		public const ExpressionType InheritanceExpressionType = (ExpressionType)1050;

		public InheritanceExpression(Type type)
		{
			_type = type;
		}

		readonly Type _type;

		public override ExpressionType NodeType { get { return InheritanceExpressionType; } }
		public override Type           Type     { get { return _type;                     } }
	}
}
