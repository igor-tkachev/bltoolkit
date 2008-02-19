using System;
#if ORACLE
using Oracle.DataAccess.Types;
#endif
using System.Reflection;
using BLToolkit.Common;
using BLToolkit.Reflection;

using NUnit.Framework;

using Convert = BLToolkit.Common.Convert;

namespace Common
{
	[TestFixture]
	public class ConvertTest
	{
		[Test]
		public void StringTest()
		{
			string testStr = "123";

			Assert.AreEqual(123, Convert.ToSByte(testStr));
			Assert.AreEqual(123, Convert.ToInt16(testStr));
			Assert.AreEqual(123, Convert.ToInt32(testStr));
			Assert.AreEqual(123, Convert.ToInt64(testStr));

			Assert.AreEqual(123, Convert.ToByte(testStr));
			Assert.AreEqual(123, Convert.ToUInt16(testStr));
			Assert.AreEqual(123, Convert.ToUInt32(testStr));
			Assert.AreEqual(123, Convert.ToUInt64(testStr));

			Assert.AreEqual(123.0m, Convert.ToDecimal(testStr));
			Assert.AreEqual(123.0f, Convert.ToSingle(testStr));
			Assert.AreEqual(123.0,  Convert.ToDouble(testStr));

			string zeroStr = "0";

			Assert.AreEqual(0, Convert.ToSByte((string)null));
			Assert.AreEqual(0, Convert.ToSByte(zeroStr));
			Assert.AreEqual(0, Convert.ToInt16(zeroStr));
			Assert.AreEqual(0, Convert.ToInt32(zeroStr));
			Assert.AreEqual(0, Convert.ToInt64(zeroStr));

			Assert.AreEqual(0, Convert.ToByte(zeroStr));
			Assert.AreEqual(0, Convert.ToUInt16(zeroStr));
			Assert.AreEqual(0, Convert.ToUInt32(zeroStr));
			Assert.AreEqual(0, Convert.ToUInt64(zeroStr));

			Assert.AreEqual(0.0m, Convert.ToDecimal(zeroStr));
			Assert.AreEqual(0.0f, Convert.ToSingle(zeroStr));
			Assert.AreEqual(0.0,  Convert.ToDouble(zeroStr));

			Assert.IsTrue(Convert.ToBoolean("True"));
			Assert.IsTrue(Convert.ToBoolean("true"));

			Assert.IsFalse(Convert.ToBoolean("false"));
			Assert.IsFalse(Convert.ToBoolean("FALSE"));
			Assert.IsFalse(Convert.ToBoolean((string)null));

			Assert.AreEqual('T', Convert.ToChar("T"));
			Assert.AreEqual(0, Convert.ToChar((string)null));

			Assert.AreEqual(DateTime.Today, Convert.ToDateTime(DateTime.Today.ToString()));
			Assert.AreEqual(DateTime.MinValue, Convert.ToDateTime((string)null));

			Assert.AreEqual(TimeSpan.FromDays(123.0), Convert.ToTimeSpan("123"));
			Assert.AreEqual(TimeSpan.MinValue, Convert.ToTimeSpan((string)null));

			Assert.AreEqual(typeof(int).GUID, Convert.ToGuid(typeof(int).GUID.ToString()));
			Assert.AreEqual(Guid.Empty, Convert.ToGuid((string)null));

			Assert.AreEqual(typeof(int), Convert.ToType("System.Int32"));
			Assert.IsNull(Convert.ToType((string)null));
		}

		private static readonly Type[] NumericTypes =
		{
            typeof(SByte),
            typeof(Byte),
            typeof(Int16),
            typeof(UInt16),
            typeof(Int32),
            typeof(UInt32),
            typeof(Int64),
            typeof(UInt64),
            typeof(Single),
            typeof(Double),
            typeof(Decimal),
        };

		private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static;

		[Test]
		public void NumericTest()
		{
			TypeHelper helper = new TypeHelper(typeof(Convert));
			IConvertible src  = 123;

			// All types from SByte to Decimal can convert to each other
			//
			for (int from = 0; from < NumericTypes.Length; ++from)
			{
				Type typeFrom = NumericTypes[from];
				object   test = src.ToType(typeFrom, null);

				for (int to = 0; to < NumericTypes.Length; ++to)
				{
					if (from == to)
						continue;

					Type   typeTo = NumericTypes[to];
					MethodInfo mi = helper.GetMethod("To" + typeTo.Name, bindingFlags, typeFrom);

					Assert.IsNotNull(mi, string.Format("Missed To{0}({1})", typeTo.Name, typeFrom.Name));
					Assert.AreEqual(123, mi.Invoke(null, new object[] { test }));
				}
			}
		}

		[Test]
		public void DateTimeTest()
		{
			Assert.AreEqual(DateTime.MinValue + TimeSpan.FromDays(1), Convert.ToDateTime(1.0));
			Assert.AreEqual(TimeSpan.FromDays(1), Convert.ToTimeSpan(1.0));

			Assert.AreEqual(DateTime.MinValue + TimeSpan.FromTicks(1), Convert.ToDateTime(1L));
			Assert.AreEqual(TimeSpan.FromTicks(1), Convert.ToTimeSpan(1L));
		}

		[Test]
		public void ByteArrayTest()
		{
			Byte[] bytes;
			object value;

			bytes = new byte[] {123};
			value = (byte)123;
			Assert.AreEqual(bytes, Convert.ToByteArray(value));
			Assert.AreEqual(value, Convert.ToByte(bytes));

			bytes = new byte[] {123, 0};
			value = (short)123;
			Assert.AreEqual(bytes, Convert.ToByteArray(value));
			Assert.AreEqual(value, Convert.ToInt16(bytes));

			bytes = new byte[] {210, 2, 150, 73};
			value = 1234567890;
			Assert.AreEqual(bytes, Convert.ToByteArray(value));
			Assert.AreEqual(value, Convert.ToInt32(bytes));

			bytes = new byte[] {(byte) 't', (byte) 's', (byte) 't'};
			value = "tst";
			Assert.AreEqual(bytes, Convert.ToByteArray(value));
			Assert.AreEqual(value, Convert.ToString(bytes));

			bytes = new byte[] {12, 0, 0, 0, 0, 0, 0, 0, 192, 0, 0, 0, 0, 0, 0, 70};
			value = typeof(System.Runtime.InteropServices.ComTypes.IStream).GUID;
			Assert.AreEqual(bytes, Convert.ToByteArray(value));
			Assert.AreEqual(value, Convert.ToGuid(bytes));

			bytes = new byte[] {210, 10, 31, 235, 140, 169, 84, 171, 0, 0, 0, 0, 0, 0, 0, 0};
			value = 12345678901234567890m;
			Assert.AreEqual(bytes, Convert.ToByteArray(value));
			Assert.AreEqual(value, Convert.ToDecimal(bytes));

			bytes = new byte[] {121, 233, 246, 66};
			value = 123.456f;
			Assert.AreEqual(bytes, Convert.ToByteArray(value));
			Assert.AreEqual(value, Convert.ToSingle(bytes));

			bytes = new byte[] {119, 190, 159, 26, 47, 221, 94, 64};
			value = 123.456;
			Assert.AreEqual(bytes, Convert.ToByteArray(value));
			Assert.AreEqual(value, Convert.ToDouble(bytes));
		}

		[Test]
		public void ConvertT()
		{
			decimal d = ConvertTo<decimal>.From(123);
			Assert.AreEqual(123.0m, d);
		}

#if ORACLE
		[Test]
		public void TypeCast()
		{
			// Note that method BLToolkit.Common.Convert.ToDecimal(OracleNumber p) does not exist.
			//
			decimal d = ConvertTo<decimal>.From(new OracleDecimal(123));
			Assert.AreEqual(123.0m, d);

			// Method BLToolkit.Common.Convert.ToOracleString(string p) does not exist too.
			//
			OracleString s = ConvertTo<OracleString>.From("test");
			Assert.AreEqual("test", s.Value);
		}
#endif
	}
}
