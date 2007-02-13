using System;
using System.Data.OracleClient;
using NUnit.Framework;

using BLToolkit.Common;

namespace Common
{
#if ORACLE

	[TestFixture]
	public class OperatorTest
	{
		private class OracleNumberOp : IOperable<OracleNumber>
		{
			public OracleNumber Addition         (OracleNumber op1, OracleNumber op2) { return (op1 + op2); }
			public OracleNumber Subtraction      (OracleNumber op1, OracleNumber op2) { return (op1 - op2); }
			public OracleNumber Multiply         (OracleNumber op1, OracleNumber op2) { return (op1 * op2); }
			public OracleNumber Division         (OracleNumber op1, OracleNumber op2) { return (op1 / op2); }
			public OracleNumber Modulus          (OracleNumber op1, OracleNumber op2) { return (op1 % op2); }

			public OracleNumber BitwiseAnd       (OracleNumber op1, OracleNumber op2) { throw new InvalidOperationException(); }
			public OracleNumber BitwiseOr        (OracleNumber op1, OracleNumber op2) { throw new InvalidOperationException(); }
			public OracleNumber ExclusiveOr      (OracleNumber op1, OracleNumber op2) { throw new InvalidOperationException(); }

			public OracleNumber UnaryNegation    (OracleNumber op)             { return (-op); }
			public OracleNumber OnesComplement   (OracleNumber op)             { throw new InvalidOperationException(); }

			public bool Equality          (OracleNumber op1, OracleNumber op2) { return (op1 == op2).IsTrue; }
			public bool Inequality        (OracleNumber op1, OracleNumber op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan       (OracleNumber op1, OracleNumber op2) { return (op1 >  op2).IsTrue; }
			public bool GreaterThanOrEqual(OracleNumber op1, OracleNumber op2) { return (op1 >= op2).IsTrue; }
			public bool LessThan          (OracleNumber op1, OracleNumber op2) { return (op1 <  op2).IsTrue; }
			public bool LessThanOrEqual   (OracleNumber op1, OracleNumber op2) { return (op1 <= op2).IsTrue; }
		}

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

		[Test]
		public void ExtensionTest()
		{
			Operator<OracleNumber>.Op = new OracleNumberOp();

			Assert.AreEqual((OracleNumber)579,   Operator<OracleNumber>.Addition   ((OracleNumber)123, (OracleNumber)456));
			Assert.AreEqual((OracleNumber)(-333),Operator<OracleNumber>.Subtraction((OracleNumber)123, (OracleNumber)456));

			Assert.AreEqual((OracleNumber)56088, Operator<OracleNumber>.Multiply((OracleNumber)123, (OracleNumber)456));
			Assert.AreEqual((OracleNumber)41,    Operator<OracleNumber>.Division((OracleNumber)123, (OracleNumber)3));
			Assert.AreEqual((OracleNumber)123,   Operator<OracleNumber>.Modulus ((OracleNumber)123, (OracleNumber)456));


			Assert.AreEqual(-(OracleNumber)123,  Operator<OracleNumber>.UnaryNegation ((OracleNumber)123));

			Assert.IsTrue(Operator<OracleNumber>.Equality          ((OracleNumber)123, (OracleNumber)123));
			Assert.IsTrue(Operator<OracleNumber>.Inequality        ((OracleNumber)123, (OracleNumber)456));
			Assert.IsTrue(Operator<OracleNumber>.GreaterThan       ((OracleNumber)123, (OracleNumber)(-5)));
			Assert.IsTrue(Operator<OracleNumber>.GreaterThanOrEqual((OracleNumber)123, (OracleNumber)123));
			Assert.IsTrue(Operator<OracleNumber>.LessThan          ((OracleNumber)123, (OracleNumber)456));
			Assert.IsTrue(Operator<OracleNumber>.LessThanOrEqual   ((OracleNumber)123, (OracleNumber)123));
		}
	}

#endif
}
