using System;

namespace BLToolkit.Common
{
	public interface IOperable<T>
	{
		T Addition             (T op1, T op2);
		T Subtraction          (T op1, T op2);
		T Multiply             (T op1, T op2);
		T Division             (T op1, T op2);
		T Modulus              (T op1, T op2);

		T BitwiseAnd           (T op1, T op2);
		T BitwiseOr            (T op1, T op2);
		T ExclusiveOr          (T op1, T op2);

		T UnaryNegation        (T op);
		T OnesComplement       (T op);
		
		bool Equality          (T op1, T op2);
		bool Inequality        (T op1, T op2);
		bool GreaterThan       (T op1, T op2);
		bool GreaterThanOrEqual(T op1, T op2);
		bool LessThan          (T op1, T op2);
		bool LessThanOrEqual   (T op1, T op2);
	}
}

