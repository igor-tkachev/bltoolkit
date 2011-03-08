using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	class ChangeTypeExpression : Expression
	{
		public const int ChangeTypeType = 1000;

		public ChangeTypeExpression(Expression expression, Type type)
		{
			Expression = expression;
			_type       = type;
		}

		readonly Type _type;

		public Expression Expression              { get; private set; }
		public override   Type           Type     { get { return _type;                          } }
		public override   ExpressionType NodeType { get { return (ExpressionType)ChangeTypeType; } }

		public override bool CanReduce
		{
			get
			{
				return base.CanReduce;
			}
		}

		public override Expression Reduce()
		{
			return base.Reduce();
		}

		public override string ToString()
		{
			return "(" + Type + ")" + Expression;
		}
	}
}
