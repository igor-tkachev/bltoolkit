using System;
#if ORACLE
using Oracle.DataAccess.Types;
#endif
using NUnit.Framework;

using BLToolkit.Common;

namespace Common
{
	[TestFixture]
	public class OperatorTest
	{
		[Test]
		public void StringTest()
		{
			Assert.AreEqual("123456", Operator<string>.Addition("123", "456"));

			Assert.IsTrue(Operator<string>.Equality("123", "123"));
		}

		[Test]
		public void IntTest()
		{
			Assert.AreEqual(579,   Operator<int>.Addition   (123, 456));
			Assert.AreEqual(-333,  Operator<int>.Subtraction(123, 456));

			Assert.AreEqual(56088, Operator<int>.Multiply(123, 456));
			Assert.AreEqual(0,     Operator<int>.Division(123, 456));
			Assert.AreEqual(123,   Operator<int>.Modulus (123, 456));


			Assert.AreEqual(72,    Operator<int>.BitwiseAnd (123, 456));
			Assert.AreEqual(507,   Operator<int>.BitwiseOr  (123, 456));
			Assert.AreEqual(435,   Operator<int>.ExclusiveOr(123, 456));

			Assert.AreEqual(-123,  Operator<int>.UnaryNegation (123));
			Assert.AreEqual(-124,  Operator<int>.OnesComplement(123));

			Assert.IsTrue(Operator<int>.Equality          (123, 123));
			Assert.IsTrue(Operator<int>.Inequality        (123, 456));
			Assert.IsTrue(Operator<int>.GreaterThan       (123, -56));
			Assert.IsTrue(Operator<int>.GreaterThanOrEqual(123, 123));
			Assert.IsTrue(Operator<int>.LessThan          (123, 456));
			Assert.IsTrue(Operator<int>.LessThanOrEqual   (-23, 123));
		}

#if ORACLE
		private class OracleDecimalOp : IOperable<OracleDecimal>
		{
			public OracleDecimal Addition         (OracleDecimal op1, OracleDecimal op2) { return (op1 + op2); }
			public OracleDecimal Subtraction      (OracleDecimal op1, OracleDecimal op2) { return (op1 - op2); }
			public OracleDecimal Multiply         (OracleDecimal op1, OracleDecimal op2) { return (op1 * op2); }
			public OracleDecimal Division         (OracleDecimal op1, OracleDecimal op2) { return (op1 / op2); }
			public OracleDecimal Modulus          (OracleDecimal op1, OracleDecimal op2) { return (op1 % op2); }

			public OracleDecimal BitwiseAnd       (OracleDecimal op1, OracleDecimal op2) { throw new InvalidOperationException(); }
			public OracleDecimal BitwiseOr        (OracleDecimal op1, OracleDecimal op2) { throw new InvalidOperationException(); }
			public OracleDecimal ExclusiveOr      (OracleDecimal op1, OracleDecimal op2) { throw new InvalidOperationException(); }

			public OracleDecimal UnaryNegation    (OracleDecimal op)             { return (-op); }
			public OracleDecimal OnesComplement   (OracleDecimal op)             { throw new InvalidOperationException(); }

			public bool Equality          (OracleDecimal op1, OracleDecimal op2) { return op1 == op2; }
			public bool Inequality        (OracleDecimal op1, OracleDecimal op2) { return op1 != op2; }
			public bool GreaterThan       (OracleDecimal op1, OracleDecimal op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(OracleDecimal op1, OracleDecimal op2) { return op1 >= op2; }
			public bool LessThan          (OracleDecimal op1, OracleDecimal op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (OracleDecimal op1, OracleDecimal op2) { return op1 <= op2; }
		}

		[Test]
		public void ExtensionTest()
		{
			Operator<OracleDecimal>.Op = new OracleDecimalOp();

			Assert.AreEqual((OracleDecimal)579,   Operator<OracleDecimal>.Addition   (123, 456));
			Assert.AreEqual((OracleDecimal)(-333),Operator<OracleDecimal>.Subtraction(123, 456));

			Assert.AreEqual((OracleDecimal)56088, Operator<OracleDecimal>.Multiply(123, 456));
			Assert.AreEqual((OracleDecimal)41,    Operator<OracleDecimal>.Division(123, 3));
			Assert.AreEqual((OracleDecimal)123,   Operator<OracleDecimal>.Modulus (123, 456));


			Assert.AreEqual(-(OracleDecimal)123,  Operator<OracleDecimal>.UnaryNegation (123));

			Assert.IsTrue(Operator<OracleDecimal>.Equality          (123, 123));
			Assert.IsTrue(Operator<OracleDecimal>.Inequality        (123, 456));
			Assert.IsTrue(Operator<OracleDecimal>.GreaterThan       (123, (-5)));
			Assert.IsTrue(Operator<OracleDecimal>.GreaterThanOrEqual(123, 123));
			Assert.IsTrue(Operator<OracleDecimal>.LessThan          (123, 456));
			Assert.IsTrue(Operator<OracleDecimal>.LessThanOrEqual   (123, 123));
		}
#endif
	}
}
