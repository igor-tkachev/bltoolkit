using System;
using System.Data.SqlTypes;

namespace BLToolkit.Common
{
	public static class Operator<T>
	{
		public  static IOperable<T> Op = GetOperable();
		private static IOperable<T> GetOperable()
		{
			Type t = typeof(T);

			// Scalar types.
			//
			if (t == typeof(String))      return (IOperable<T>)new S();

			if (t == typeof(SByte))       return (IOperable<T>)new S8();
			if (t == typeof(Int16))       return (IOperable<T>)new S16();
			if (t == typeof(Int32))       return (IOperable<T>)new S32();
			if (t == typeof(Int64))       return (IOperable<T>)new S64();

			if (t == typeof(Byte))        return (IOperable<T>)new U8();
			if (t == typeof(UInt16))      return (IOperable<T>)new U16();
			if (t == typeof(UInt32))      return (IOperable<T>)new U32();
			if (t == typeof(UInt64))      return (IOperable<T>)new U64();

			if (t == typeof(bool))        return (IOperable<T>)new B();
			if (t == typeof(Char))        return (IOperable<T>)new C();
			if (t == typeof(Single))      return (IOperable<T>)new R4();
			if (t == typeof(Double))      return (IOperable<T>)new R8();

			if (t == typeof(Decimal))     return (IOperable<T>)new D();
			if (t == typeof(DateTime))    return (IOperable<T>)new DT();
			if (t == typeof(TimeSpan))    return (IOperable<T>)new TS();
			if (t == typeof(Guid))        return (IOperable<T>)new G();

			// Nullable types.
			//
			if (t == typeof(SByte?))      return (IOperable<T>)new NS8();
			if (t == typeof(Int16?))      return (IOperable<T>)new NS16();
			if (t == typeof(Int32?))      return (IOperable<T>)new NS32();
			if (t == typeof(Int64?))      return (IOperable<T>)new NS64();

			if (t == typeof(Byte?))       return (IOperable<T>)new NU8();
			if (t == typeof(UInt16?))     return (IOperable<T>)new NU16();
			if (t == typeof(UInt32?))     return (IOperable<T>)new NU32();
			if (t == typeof(UInt64?))     return (IOperable<T>)new NU64();

			if (t == typeof(bool?))       return (IOperable<T>)new NB();
			if (t == typeof(Char?))       return (IOperable<T>)new NC();
			if (t == typeof(Single?))     return (IOperable<T>)new NR4();
			if (t == typeof(Double?))     return (IOperable<T>)new NR8();

			if (t == typeof(Decimal?))    return (IOperable<T>)new ND();
			if (t == typeof(DateTime?))   return (IOperable<T>)new NDT();
			if (t == typeof(TimeSpan?))   return (IOperable<T>)new NTS();
			if (t == typeof(Guid?))       return (IOperable<T>)new NG();

			// Sql types.
			//
			if (t == typeof(SqlString))   return (IOperable<T>)new DBS();

			if (t == typeof(SqlByte))     return (IOperable<T>)new DBU8();
			if (t == typeof(SqlInt16))    return (IOperable<T>)new DBS16();
			if (t == typeof(SqlInt32))    return (IOperable<T>)new DBS32();
			if (t == typeof(SqlInt64))    return (IOperable<T>)new DBS64();

			if (t == typeof(SqlSingle))   return (IOperable<T>)new DBR4();
			if (t == typeof(SqlDouble))   return (IOperable<T>)new DBR8();
			if (t == typeof(SqlDecimal))  return (IOperable<T>)new DBD();
			if (t == typeof(SqlMoney))    return (IOperable<T>)new DBM();

			if (t == typeof(SqlBoolean))  return (IOperable<T>)new DBB();
			if (t == typeof(SqlBinary))   return (IOperable<T>)new DBBin();
			if (t == typeof(SqlDateTime)) return (IOperable<T>)new DBDT();
			if (t == typeof(SqlGuid))     return (IOperable<T>)new DBG();

			return new Default<T>();
		}

		public static T Addition             (T op1, T op2) { return Op.Addition          (op1, op2); }
		public static T Subtraction          (T op1, T op2) { return Op.Subtraction       (op1, op2); }
		public static T Multiply             (T op1, T op2) { return Op.Multiply          (op1, op2); }
		public static T Division             (T op1, T op2) { return Op.Division          (op1, op2); }
		public static T Modulus              (T op1, T op2) { return Op.Modulus           (op1, op2); }

		public static T BitwiseAnd           (T op1, T op2) { return Op.BitwiseAnd        (op1, op2); }
		public static T BitwiseOr            (T op1, T op2) { return Op.BitwiseOr         (op1, op2); }
		public static T ExclusiveOr          (T op1, T op2) { return Op.ExclusiveOr       (op1, op2); }

		public static T UnaryNegation        (T op)         { return Op.UnaryNegation     (op);       }
		public static T OnesComplement       (T op)         { return Op.OnesComplement    (op);       }
	
		public static bool Equality          (T op1, T op2) { return Op.Equality          (op1, op2); }
		public static bool Inequality        (T op1, T op2) { return Op.Inequality        (op1, op2); }
		public static bool GreaterThan       (T op1, T op2) { return Op.GreaterThan       (op1, op2); }
		public static bool GreaterThanOrEqual(T op1, T op2) { return Op.GreaterThanOrEqual(op1, op2); }
		public static bool LessThan          (T op1, T op2) { return Op.LessThan          (op1, op2); }
		public static bool LessThanOrEqual   (T op1, T op2) { return Op.LessThanOrEqual   (op1, op2); }

		#region Default

		private class Default<Q> : IOperable<Q>
		{
			public Q Addition             (Q op1, Q op2) { throw new InvalidOperationException(); }
			public Q Subtraction          (Q op1, Q op2) { throw new InvalidOperationException(); }
			public Q Multiply             (Q op1, Q op2) { throw new InvalidOperationException(); }
			public Q Division             (Q op1, Q op2) { throw new InvalidOperationException(); }
			public Q Modulus              (Q op1, Q op2) { throw new InvalidOperationException(); }

			public Q BitwiseAnd           (Q op1, Q op2) { throw new InvalidOperationException(); }
			public Q BitwiseOr            (Q op1, Q op2) { throw new InvalidOperationException(); }
			public Q ExclusiveOr          (Q op1, Q op2) { throw new InvalidOperationException(); }

			public Q UnaryNegation        (Q op)         { throw new InvalidOperationException(); }
			public Q OnesComplement       (Q op)         { throw new InvalidOperationException(); }

			public bool Equality          (Q op1, Q op2) { throw new InvalidOperationException(); }
			public bool Inequality        (Q op1, Q op2) { throw new InvalidOperationException(); }
			public bool GreaterThan       (Q op1, Q op2) { throw new InvalidOperationException(); }
			public bool GreaterThanOrEqual(Q op1, Q op2) { throw new InvalidOperationException(); }
			public bool LessThan          (Q op1, Q op2) { throw new InvalidOperationException(); }
			public bool LessThanOrEqual   (Q op1, Q op2) { throw new InvalidOperationException(); }
		}

		#endregion

		#region Scalar Types.

		#region String

		private class S : IOperable<String>
		{
			public String Addition        (String op1, String op2) { return (op1 + op2); }
			public String Subtraction     (String op1, String op2) { throw new InvalidOperationException(); }
			public String Multiply        (String op1, String op2) { throw new InvalidOperationException(); }
			public String Division        (String op1, String op2) { throw new InvalidOperationException(); }
			public String Modulus         (String op1, String op2) { throw new InvalidOperationException(); }

			public String BitwiseAnd      (String op1, String op2) { throw new InvalidOperationException(); }
			public String BitwiseOr       (String op1, String op2) { throw new InvalidOperationException(); }
			public String ExclusiveOr     (String op1, String op2) { throw new InvalidOperationException(); }

			public String UnaryNegation   (String op)              { throw new InvalidOperationException(); }
			public String OnesComplement  (String op)              { throw new InvalidOperationException(); }

			public bool Equality          (String op1, String op2) { return op1 == op2; }
			public bool Inequality        (String op1, String op2) { return op1 != op2; }
			public bool GreaterThan       (String op1, String op2) { throw new InvalidOperationException(); }
			public bool GreaterThanOrEqual(String op1, String op2) { throw new InvalidOperationException(); }
			public bool LessThan          (String op1, String op2) { throw new InvalidOperationException(); }
			public bool LessThanOrEqual   (String op1, String op2) { throw new InvalidOperationException(); }
		}

		#endregion

		#region SByte

		private class S8 : IOperable<SByte>
		{
			public SByte Addition         (SByte op1, SByte op2) { return (SByte)(op1 + op2); }
			public SByte Subtraction      (SByte op1, SByte op2) { return (SByte)(op1 - op2); }
			public SByte Multiply         (SByte op1, SByte op2) { return (SByte)(op1 * op2); }
			public SByte Division         (SByte op1, SByte op2) { return (SByte)(op1 / op2); }
			public SByte Modulus          (SByte op1, SByte op2) { return (SByte)(op1 % op2); }

			public SByte BitwiseAnd       (SByte op1, SByte op2) { return (SByte)(op1 & op2); }
			public SByte BitwiseOr        (SByte op1, SByte op2) { return (SByte)(op1 | op2); }
			public SByte ExclusiveOr      (SByte op1, SByte op2) { return (SByte)(op1 ^ op2); }

			public SByte UnaryNegation    (SByte op)             { return (SByte)(-op); }
			public SByte OnesComplement   (SByte op)             { return (SByte)(~op); }

			public bool Equality          (SByte op1, SByte op2) { return op1 == op2; }
			public bool Inequality        (SByte op1, SByte op2) { return op1 != op2; }
			public bool GreaterThan       (SByte op1, SByte op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(SByte op1, SByte op2) { return op1 >= op2; }
			public bool LessThan          (SByte op1, SByte op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (SByte op1, SByte op2) { return op1 <= op2; }
		}

		#endregion

		#region Int16

		private class S16 : IOperable<Int16>
		{
			public Int16 Addition         (Int16 op1, Int16 op2) { return (Int16)(op1 + op2); }
			public Int16 Subtraction      (Int16 op1, Int16 op2) { return (Int16)(op1 - op2); }
			public Int16 Multiply         (Int16 op1, Int16 op2) { return (Int16)(op1 * op2); }
			public Int16 Division         (Int16 op1, Int16 op2) { return (Int16)(op1 / op2); }
			public Int16 Modulus          (Int16 op1, Int16 op2) { return (Int16)(op1 % op2); }

			public Int16 BitwiseAnd       (Int16 op1, Int16 op2) { return (Int16)(op1 & op2); }
			public Int16 BitwiseOr        (Int16 op1, Int16 op2) { return (Int16)(op1 | op2); }
			public Int16 ExclusiveOr      (Int16 op1, Int16 op2) { return (Int16)(op1 ^ op2); }

			public Int16 UnaryNegation    (Int16 op)             { return (Int16)(-op); }
			public Int16 OnesComplement   (Int16 op)             { return (Int16)(~op); }

			public bool Equality          (Int16 op1, Int16 op2) { return op1 == op2; }
			public bool Inequality        (Int16 op1, Int16 op2) { return op1 != op2; }
			public bool GreaterThan       (Int16 op1, Int16 op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Int16 op1, Int16 op2) { return op1 >= op2; }
			public bool LessThan          (Int16 op1, Int16 op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Int16 op1, Int16 op2) { return op1 <= op2; }
		}

		#endregion

		#region Int32

		private class S32 : IOperable<Int32>
		{
			public Int32 Addition         (Int32 op1, Int32 op2) { return (op1 + op2); }
			public Int32 Subtraction      (Int32 op1, Int32 op2) { return (op1 - op2); }
			public Int32 Multiply         (Int32 op1, Int32 op2) { return (op1 * op2); }
			public Int32 Division         (Int32 op1, Int32 op2) { return (op1 / op2); }
			public Int32 Modulus          (Int32 op1, Int32 op2) { return (op1 % op2); }

			public Int32 BitwiseAnd       (Int32 op1, Int32 op2) { return (op1 & op2); }
			public Int32 BitwiseOr        (Int32 op1, Int32 op2) { return (op1 | op2); }
			public Int32 ExclusiveOr      (Int32 op1, Int32 op2) { return (op1 ^ op2); }

			public Int32 UnaryNegation    (Int32 op)             { return (-op); }
			public Int32 OnesComplement   (Int32 op)             { return (~op); }

			public bool Equality          (Int32 op1, Int32 op2) { return op1 == op2; }
			public bool Inequality        (Int32 op1, Int32 op2) { return op1 != op2; }
			public bool GreaterThan       (Int32 op1, Int32 op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Int32 op1, Int32 op2) { return op1 >= op2; }
			public bool LessThan          (Int32 op1, Int32 op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Int32 op1, Int32 op2) { return op1 <= op2; }
		}

		#endregion

		#region Int64

		private class S64 : IOperable<Int64>
		{
			public Int64 Addition         (Int64 op1, Int64 op2) { return (op1 + op2); }
			public Int64 Subtraction      (Int64 op1, Int64 op2) { return (op1 - op2); }
			public Int64 Multiply         (Int64 op1, Int64 op2) { return (op1 * op2); }
			public Int64 Division         (Int64 op1, Int64 op2) { return (op1 / op2); }
			public Int64 Modulus          (Int64 op1, Int64 op2) { return (op1 % op2); }

			public Int64 BitwiseAnd       (Int64 op1, Int64 op2) { return (op1 & op2); }
			public Int64 BitwiseOr        (Int64 op1, Int64 op2) { return (op1 | op2); }
			public Int64 ExclusiveOr      (Int64 op1, Int64 op2) { return (op1 ^ op2); }

			public Int64 UnaryNegation    (Int64 op)             { return (-op); }
			public Int64 OnesComplement   (Int64 op)             { return (~op); }

			public bool Equality          (Int64 op1, Int64 op2) { return op1 == op2; }
			public bool Inequality        (Int64 op1, Int64 op2) { return op1 != op2; }
			public bool GreaterThan       (Int64 op1, Int64 op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Int64 op1, Int64 op2) { return op1 >= op2; }
			public bool LessThan          (Int64 op1, Int64 op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Int64 op1, Int64 op2) { return op1 <= op2; }
		}

		#endregion

		#region Byte

		private class U8 : IOperable<Byte>
		{
			public Byte Addition          (Byte op1, Byte op2) { return (Byte)(op1 + op2); }
			public Byte Subtraction       (Byte op1, Byte op2) { return (Byte)(op1 - op2); }
			public Byte Multiply          (Byte op1, Byte op2) { return (Byte)(op1 * op2); }
			public Byte Division          (Byte op1, Byte op2) { return (Byte)(op1 / op2); }
			public Byte Modulus           (Byte op1, Byte op2) { return (Byte)(op1 % op2); }

			public Byte BitwiseAnd        (Byte op1, Byte op2) { return (Byte)(op1 & op2); }
			public Byte BitwiseOr         (Byte op1, Byte op2) { return (Byte)(op1 | op2); }
			public Byte ExclusiveOr       (Byte op1, Byte op2) { return (Byte)(op1 ^ op2); }

			public Byte UnaryNegation     (Byte op)            { throw new InvalidOperationException(); }
			public Byte OnesComplement    (Byte op)            { return (Byte)(~op); }

			public bool Equality          (Byte op1, Byte op2) { return op1 == op2; }
			public bool Inequality        (Byte op1, Byte op2) { return op1 != op2; }
			public bool GreaterThan       (Byte op1, Byte op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Byte op1, Byte op2) { return op1 >= op2; }
			public bool LessThan          (Byte op1, Byte op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Byte op1, Byte op2) { return op1 <= op2; }
		}

		#endregion

		#region UInt16

		private class U16 : IOperable<UInt16>
		{
			public UInt16 Addition        (UInt16 op1, UInt16 op2) { return (UInt16)(op1 + op2); }
			public UInt16 Subtraction     (UInt16 op1, UInt16 op2) { return (UInt16)(op1 - op2); }
			public UInt16 Multiply        (UInt16 op1, UInt16 op2) { return (UInt16)(op1 * op2); }
			public UInt16 Division        (UInt16 op1, UInt16 op2) { return (UInt16)(op1 / op2); }
			public UInt16 Modulus         (UInt16 op1, UInt16 op2) { return (UInt16)(op1 % op2); }

			public UInt16 BitwiseAnd      (UInt16 op1, UInt16 op2) { return (UInt16)(op1 & op2); }
			public UInt16 BitwiseOr       (UInt16 op1, UInt16 op2) { return (UInt16)(op1 | op2); }
			public UInt16 ExclusiveOr     (UInt16 op1, UInt16 op2) { return (UInt16)(op1 ^ op2); }

			public UInt16 UnaryNegation   (UInt16 op)              { throw new InvalidOperationException(); }
			public UInt16 OnesComplement  (UInt16 op)              { return (UInt16)(~op); }

			public bool Equality          (UInt16 op1, UInt16 op2) { return op1 == op2; }
			public bool Inequality        (UInt16 op1, UInt16 op2) { return op1 != op2; }
			public bool GreaterThan       (UInt16 op1, UInt16 op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(UInt16 op1, UInt16 op2) { return op1 >= op2; }
			public bool LessThan          (UInt16 op1, UInt16 op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (UInt16 op1, UInt16 op2) { return op1 <= op2; }
		}

		#endregion

		#region UInt32

		private class U32 : IOperable<UInt32>
		{
			public UInt32 Addition        (UInt32 op1, UInt32 op2) { return (op1 + op2); }
			public UInt32 Subtraction     (UInt32 op1, UInt32 op2) { return (op1 - op2); }
			public UInt32 Multiply        (UInt32 op1, UInt32 op2) { return (op1 * op2); }
			public UInt32 Division        (UInt32 op1, UInt32 op2) { return (op1 / op2); }
			public UInt32 Modulus         (UInt32 op1, UInt32 op2) { return (op1 % op2); }

			public UInt32 BitwiseAnd      (UInt32 op1, UInt32 op2) { return (op1 & op2); }
			public UInt32 BitwiseOr       (UInt32 op1, UInt32 op2) { return (op1 | op2); }
			public UInt32 ExclusiveOr     (UInt32 op1, UInt32 op2) { return (op1 ^ op2); }
			 
			public UInt32 UnaryNegation   (UInt32 op)              { throw new InvalidOperationException(); }
			public UInt32 OnesComplement  (UInt32 op)              { return (~op); }

			public bool Equality          (UInt32 op1, UInt32 op2) { return op1 == op2; }
			public bool Inequality        (UInt32 op1, UInt32 op2) { return op1 != op2; }
			public bool GreaterThan       (UInt32 op1, UInt32 op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(UInt32 op1, UInt32 op2) { return op1 >= op2; }
			public bool LessThan          (UInt32 op1, UInt32 op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (UInt32 op1, UInt32 op2) { return op1 <= op2; }
		}

		#endregion

		#region UInt64

		private class U64 : IOperable<UInt64>
		{
			public UInt64 Addition        (UInt64 op1, UInt64 op2) { return (op1 + op2); }
			public UInt64 Subtraction     (UInt64 op1, UInt64 op2) { return (op1 - op2); }
			public UInt64 Multiply        (UInt64 op1, UInt64 op2) { return (op1 * op2); }
			public UInt64 Division        (UInt64 op1, UInt64 op2) { return (op1 / op2); }
			public UInt64 Modulus         (UInt64 op1, UInt64 op2) { return (op1 % op2); }

			public UInt64 BitwiseAnd      (UInt64 op1, UInt64 op2) { return (op1 & op2); }
			public UInt64 BitwiseOr       (UInt64 op1, UInt64 op2) { return (op1 | op2); }
			public UInt64 ExclusiveOr     (UInt64 op1, UInt64 op2) { return (op1 ^ op2); }

			public UInt64 UnaryNegation   (UInt64 op)              { throw new InvalidOperationException(); }
			public UInt64 OnesComplement  (UInt64 op)              { return (~op); }

			public bool Equality          (UInt64 op1, UInt64 op2) { return op1 == op2; }
			public bool Inequality        (UInt64 op1, UInt64 op2) { return op1 != op2; }
			public bool GreaterThan       (UInt64 op1, UInt64 op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(UInt64 op1, UInt64 op2) { return op1 >= op2; }
			public bool LessThan          (UInt64 op1, UInt64 op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (UInt64 op1, UInt64 op2) { return op1 <= op2; }
		}

		#endregion

		#region Boolean

		private class B : IOperable<Boolean>
		{
			public Boolean Addition       (Boolean op1, Boolean op2) { throw new InvalidOperationException(); }
			public Boolean Subtraction    (Boolean op1, Boolean op2) { throw new InvalidOperationException(); }
			public Boolean Multiply       (Boolean op1, Boolean op2) { throw new InvalidOperationException(); }
			public Boolean Division       (Boolean op1, Boolean op2) { throw new InvalidOperationException(); }
			public Boolean Modulus        (Boolean op1, Boolean op2) { throw new InvalidOperationException(); }

			public Boolean BitwiseAnd     (Boolean op1, Boolean op2) { return (op1 & op2); }
			public Boolean BitwiseOr      (Boolean op1, Boolean op2) { return (op1 | op2); }
			public Boolean ExclusiveOr    (Boolean op1, Boolean op2) { return (op1 ^ op2); }

			public Boolean UnaryNegation  (Boolean op)               { throw new InvalidOperationException(); }
			public Boolean OnesComplement (Boolean op)               { return !op; }

			public bool Equality          (Boolean op1, Boolean op2) { return op1 ==  op2; }
			public bool Inequality        (Boolean op1, Boolean op2) { return op1 !=  op2; }
			public bool GreaterThan       (Boolean op1, Boolean op2) { throw new InvalidOperationException(); }
			public bool GreaterThanOrEqual(Boolean op1, Boolean op2) { throw new InvalidOperationException(); }
			public bool LessThan          (Boolean op1, Boolean op2) { throw new InvalidOperationException(); }
			public bool LessThanOrEqual   (Boolean op1, Boolean op2) { throw new InvalidOperationException(); }
		}

		#endregion

		#region Char

		private class C : IOperable<Char>
		{
			public Char Addition          (Char op1, Char op2) { return (Char)(op1 + op2); }
			public Char Subtraction       (Char op1, Char op2) { return (Char)(op1 - op2); }
			public Char Multiply          (Char op1, Char op2) { return (Char)(op1 * op2); }
			public Char Division          (Char op1, Char op2) { return (Char)(op1 / op2); }
			public Char Modulus           (Char op1, Char op2) { return (Char)(op1 % op2); }

			public Char BitwiseAnd        (Char op1, Char op2) { return (Char)(op1 & op2); }
			public Char BitwiseOr         (Char op1, Char op2) { return (Char)(op1 | op2); }
			public Char ExclusiveOr       (Char op1, Char op2) { return (Char)(op1 ^ op2); }

			public Char UnaryNegation     (Char op)            { return (Char)(-op); }
			public Char OnesComplement    (Char op)            { return (Char)(~op); }

			public bool Equality          (Char op1, Char op2) { return op1 == op2; }
			public bool Inequality        (Char op1, Char op2) { return op1 != op2; }
			public bool GreaterThan       (Char op1, Char op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Char op1, Char op2) { return op1 >= op2; }
			public bool LessThan          (Char op1, Char op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Char op1, Char op2) { return op1 <= op2; }
		}

		#endregion

		#region Single

		private class R4 : IOperable<Single>
		{
			public Single Addition        (Single op1, Single op2) { return (op1 + op2); }
			public Single Subtraction     (Single op1, Single op2) { return (op1 - op2); }
			public Single Multiply        (Single op1, Single op2) { return (op1 * op2); }
			public Single Division        (Single op1, Single op2) { return (op1 / op2); }
			public Single Modulus         (Single op1, Single op2) { return (op1 % op2); }

			public Single BitwiseAnd      (Single op1, Single op2) { throw new InvalidOperationException(); }
			public Single BitwiseOr       (Single op1, Single op2) { throw new InvalidOperationException(); }
			public Single ExclusiveOr     (Single op1, Single op2) { throw new InvalidOperationException(); }

			public Single UnaryNegation   (Single op)              { return (-op); }
			public Single OnesComplement  (Single op)              { throw new InvalidOperationException(); }

			public bool Equality          (Single op1, Single op2) { return op1 == op2; }
			public bool Inequality        (Single op1, Single op2) { return op1 != op2; }
			public bool GreaterThan       (Single op1, Single op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Single op1, Single op2) { return op1 >= op2; }
			public bool LessThan          (Single op1, Single op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Single op1, Single op2) { return op1 <= op2; }
		}

		#endregion

		#region Double

		private class R8 : IOperable<Double>
		{
			public Double Addition        (Double op1, Double op2) { return (op1 + op2); }
			public Double Subtraction     (Double op1, Double op2) { return (op1 - op2); }
			public Double Multiply        (Double op1, Double op2) { return (op1 * op2); }
			public Double Division        (Double op1, Double op2) { return (op1 / op2); }
			public Double Modulus         (Double op1, Double op2) { return (op1 % op2); }

			public Double BitwiseAnd      (Double op1, Double op2) { throw new InvalidOperationException(); }
			public Double BitwiseOr       (Double op1, Double op2) { throw new InvalidOperationException(); }
			public Double ExclusiveOr     (Double op1, Double op2) { throw new InvalidOperationException(); }

			public Double UnaryNegation   (Double op)              { return (-op); }
			public Double OnesComplement  (Double op)              { throw new InvalidOperationException(); }

			public bool Equality          (Double op1, Double op2) { return op1 == op2; }
			public bool Inequality        (Double op1, Double op2) { return op1 != op2; }
			public bool GreaterThan       (Double op1, Double op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Double op1, Double op2) { return op1 >= op2; }
			public bool LessThan          (Double op1, Double op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Double op1, Double op2) { return op1 <= op2; }
		}

		#endregion

		#region Decimal

		private class D : IOperable<Decimal>
		{
			public Decimal Addition       (Decimal op1, Decimal op2) { return (op1 + op2); }
			public Decimal Subtraction    (Decimal op1, Decimal op2) { return (op1 - op2); }
			public Decimal Multiply       (Decimal op1, Decimal op2) { return (op1 * op2); }
			public Decimal Division       (Decimal op1, Decimal op2) { return (op1 / op2); }
			public Decimal Modulus        (Decimal op1, Decimal op2) { return (op1 % op2); }

			public Decimal BitwiseAnd     (Decimal op1, Decimal op2) { throw new InvalidOperationException(); }
			public Decimal BitwiseOr      (Decimal op1, Decimal op2) { throw new InvalidOperationException(); }
			public Decimal ExclusiveOr    (Decimal op1, Decimal op2) { throw new InvalidOperationException(); }

			public Decimal UnaryNegation  (Decimal op)               { return (-op); }
			public Decimal OnesComplement (Decimal op)               { throw new InvalidOperationException(); }

			public bool Equality          (Decimal op1, Decimal op2) { return op1 == op2; }
			public bool Inequality        (Decimal op1, Decimal op2) { return op1 != op2; }
			public bool GreaterThan       (Decimal op1, Decimal op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Decimal op1, Decimal op2) { return op1 >= op2; }
			public bool LessThan          (Decimal op1, Decimal op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Decimal op1, Decimal op2) { return op1 <= op2; }
		}

		#endregion

		#region DateTime

		private class DT : IOperable<DateTime>
		{
			public DateTime Addition      (DateTime op1, DateTime op2) { throw new InvalidOperationException(); }
			public DateTime Subtraction   (DateTime op1, DateTime op2) { throw new InvalidOperationException(); }
			public DateTime Multiply      (DateTime op1, DateTime op2) { throw new InvalidOperationException(); }
			public DateTime Division      (DateTime op1, DateTime op2) { throw new InvalidOperationException(); }
			public DateTime Modulus       (DateTime op1, DateTime op2) { throw new InvalidOperationException(); }

			public DateTime BitwiseAnd    (DateTime op1, DateTime op2) { throw new InvalidOperationException(); }
			public DateTime BitwiseOr     (DateTime op1, DateTime op2) { throw new InvalidOperationException(); }
			public DateTime ExclusiveOr   (DateTime op1, DateTime op2) { throw new InvalidOperationException(); }

			public DateTime UnaryNegation (DateTime op)                { throw new InvalidOperationException(); }
			public DateTime OnesComplement(DateTime op)                { throw new InvalidOperationException(); }

			public bool Equality          (DateTime op1, DateTime op2) { return op1 == op2; }
			public bool Inequality        (DateTime op1, DateTime op2) { return op1 != op2; }
			public bool GreaterThan       (DateTime op1, DateTime op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(DateTime op1, DateTime op2) { return op1 >= op2; }
			public bool LessThan          (DateTime op1, DateTime op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (DateTime op1, DateTime op2) { return op1 <= op2; }
		}

		#endregion

		#region TimeSpan

		private class TS : IOperable<TimeSpan>
		{
			public TimeSpan Addition      (TimeSpan op1, TimeSpan op2) { return (op1 + op2); }
			public TimeSpan Subtraction   (TimeSpan op1, TimeSpan op2) { return (op1 - op2); }
			public TimeSpan Multiply      (TimeSpan op1, TimeSpan op2) { throw new InvalidOperationException(); }
			public TimeSpan Division      (TimeSpan op1, TimeSpan op2) { throw new InvalidOperationException(); }
			public TimeSpan Modulus       (TimeSpan op1, TimeSpan op2) { throw new InvalidOperationException(); }

			public TimeSpan BitwiseAnd    (TimeSpan op1, TimeSpan op2) { throw new InvalidOperationException(); }
			public TimeSpan BitwiseOr     (TimeSpan op1, TimeSpan op2) { throw new InvalidOperationException(); }
			public TimeSpan ExclusiveOr   (TimeSpan op1, TimeSpan op2) { throw new InvalidOperationException(); }

			public TimeSpan UnaryNegation (TimeSpan op)                { return (-op); }
			public TimeSpan OnesComplement(TimeSpan op)                { throw new InvalidOperationException(); }

			public bool Equality          (TimeSpan op1, TimeSpan op2) { return op1 == op2; }
			public bool Inequality        (TimeSpan op1, TimeSpan op2) { return op1 != op2; }
			public bool GreaterThan       (TimeSpan op1, TimeSpan op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(TimeSpan op1, TimeSpan op2) { return op1 >= op2; }
			public bool LessThan          (TimeSpan op1, TimeSpan op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (TimeSpan op1, TimeSpan op2) { return op1 <= op2; }
		}

		#endregion

		#region Guid

		private class G : IOperable<Guid>
		{
			public Guid Addition          (Guid op1, Guid op2) { throw new InvalidOperationException(); }
			public Guid Subtraction        (Guid op1, Guid op2) { throw new InvalidOperationException(); }
			public Guid Multiply          (Guid op1, Guid op2) { throw new InvalidOperationException(); }
			public Guid Division          (Guid op1, Guid op2) { throw new InvalidOperationException(); }
			public Guid Modulus           (Guid op1, Guid op2) { throw new InvalidOperationException(); }

			public Guid BitwiseAnd        (Guid op1, Guid op2) { throw new InvalidOperationException(); }
			public Guid BitwiseOr         (Guid op1, Guid op2) { throw new InvalidOperationException(); }
			public Guid ExclusiveOr       (Guid op1, Guid op2) { throw new InvalidOperationException(); }

			public Guid UnaryNegation     (Guid op)            { throw new InvalidOperationException(); }
			public Guid OnesComplement    (Guid op)            { throw new InvalidOperationException(); }

			public bool Equality          (Guid op1, Guid op2) { return op1 == op2; }
			public bool Inequality        (Guid op1, Guid op2) { return op1 != op2; }
			public bool GreaterThan       (Guid op1, Guid op2) { throw new InvalidOperationException(); }
			public bool GreaterThanOrEqual(Guid op1, Guid op2) { throw new InvalidOperationException(); }
			public bool LessThan          (Guid op1, Guid op2) { throw new InvalidOperationException(); }
			public bool LessThanOrEqual   (Guid op1, Guid op2) { throw new InvalidOperationException(); }
		}

		#endregion

		#endregion

		#region Nullable Types.

		#region SByte

		private class NS8 : IOperable<SByte?>
		{
			public SByte? Addition        (SByte? op1, SByte? op2) { return (SByte?)(op1 + op2); }
			public SByte? Subtraction     (SByte? op1, SByte? op2) { return (SByte?)(op1 - op2); }
			public SByte? Multiply        (SByte? op1, SByte? op2) { return (SByte?)(op1 * op2); }
			public SByte? Division        (SByte? op1, SByte? op2) { return (SByte?)(op1 / op2); }
			public SByte? Modulus         (SByte? op1, SByte? op2) { return (SByte?)(op1 % op2); }

			public SByte? BitwiseAnd      (SByte? op1, SByte? op2) { return (SByte?)(op1 & op2); }
			public SByte? BitwiseOr       (SByte? op1, SByte? op2) { return (SByte?)(op1 | op2); }
			public SByte? ExclusiveOr     (SByte? op1, SByte? op2) { return (SByte?)(op1 ^ op2); }

			public SByte? UnaryNegation   (SByte? op)              { return (SByte)(- op); }
			public SByte? OnesComplement  (SByte? op)              { return (SByte)(~op); }

			public bool Equality          (SByte? op1, SByte? op2) { return op1 == op2; }
			public bool Inequality        (SByte? op1, SByte? op2) { return op1 != op2; }
			public bool GreaterThan       (SByte? op1, SByte? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(SByte? op1, SByte? op2) { return op1 >= op2; }
			public bool LessThan          (SByte? op1, SByte? op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (SByte? op1, SByte? op2) { return op1 <= op2; }
		}

		#endregion

		#region Int16

		private class NS16 : IOperable<Int16?>
		{
			public Int16? Addition        (Int16? op1, Int16? op2) { return (Int16?)(op1 + op2); }
			public Int16? Subtraction     (Int16? op1, Int16? op2) { return (Int16?)(op1 - op2); }
			public Int16? Multiply        (Int16? op1, Int16? op2) { return (Int16?)(op1 * op2); }
			public Int16? Division        (Int16? op1, Int16? op2) { return (Int16?)(op1 / op2); }
			public Int16? Modulus         (Int16? op1, Int16? op2) { return (Int16?)(op1 % op2); }

			public Int16? BitwiseAnd      (Int16? op1, Int16? op2) { return (Int16?)(op1 & op2); }
			public Int16? BitwiseOr       (Int16? op1, Int16? op2) { return (Int16?)(op1 | op2); }
			public Int16? ExclusiveOr     (Int16? op1, Int16? op2) { return (Int16?)(op1 ^ op2); }

			public Int16? UnaryNegation   (Int16? op)              { return (Int16)(- op); }
			public Int16? OnesComplement  (Int16? op)              { return (Int16)(~op); }

			public bool Equality          (Int16? op1, Int16? op2) { return op1 == op2; }
			public bool Inequality        (Int16? op1, Int16? op2) { return op1 != op2; }
			public bool GreaterThan       (Int16? op1, Int16? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Int16? op1, Int16? op2) { return op1 >= op2; }
			public bool LessThan          (Int16? op1, Int16? op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Int16? op1, Int16? op2) { return op1 <= op2; }
		}

		#endregion

		#region Int32

		private class NS32 : IOperable<Int32?>
		{
			public Int32? Addition        (Int32? op1, Int32? op2) { return (op1 + op2); }
			public Int32? Subtraction     (Int32? op1, Int32? op2) { return (op1 - op2); }
			public Int32? Multiply        (Int32? op1, Int32? op2) { return (op1 * op2); }
			public Int32? Division        (Int32? op1, Int32? op2) { return (op1 / op2); }
			public Int32? Modulus         (Int32? op1, Int32? op2) { return (op1 % op2); }

			public Int32? BitwiseAnd      (Int32? op1, Int32? op2) { return (op1 & op2); }
			public Int32? BitwiseOr       (Int32? op1, Int32? op2) { return (op1 | op2); }
			public Int32? ExclusiveOr     (Int32? op1, Int32? op2) { return (op1 ^ op2); }

			public Int32? UnaryNegation   (Int32? op)              { return (- op); }
			public Int32? OnesComplement  (Int32? op)              { return (~op); }

			public bool Equality          (Int32? op1, Int32? op2) { return op1 == op2; }
			public bool Inequality        (Int32? op1, Int32? op2) { return op1 != op2; }
			public bool GreaterThan       (Int32? op1, Int32? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Int32? op1, Int32? op2) { return op1 >= op2; }
			public bool LessThan          (Int32? op1, Int32? op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Int32? op1, Int32? op2) { return op1 <= op2; }
		}

		#endregion

		#region Int64

		private class NS64 : IOperable<Int64?>
		{
			public Int64? Addition        (Int64? op1, Int64? op2) { return (op1 + op2); }
			public Int64? Subtraction     (Int64? op1, Int64? op2) { return (op1 - op2); }
			public Int64? Multiply        (Int64? op1, Int64? op2) { return (op1 * op2); }
			public Int64? Division        (Int64? op1, Int64? op2) { return (op1 / op2); }
			public Int64? Modulus         (Int64? op1, Int64? op2) { return (op1 % op2); }

			public Int64? BitwiseAnd      (Int64? op1, Int64? op2) { return (op1 & op2); }
			public Int64? BitwiseOr       (Int64? op1, Int64? op2) { return (op1 | op2); }
			public Int64? ExclusiveOr     (Int64? op1, Int64? op2) { return (op1 ^ op2); }

			public Int64? UnaryNegation   (Int64? op)              { return (- op); }
			public Int64? OnesComplement  (Int64? op)              { return (~op); }

			public bool Equality          (Int64? op1, Int64? op2) { return op1 == op2; }
			public bool Inequality        (Int64? op1, Int64? op2) { return op1 != op2; }
			public bool GreaterThan       (Int64? op1, Int64? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Int64? op1, Int64? op2) { return op1 >= op2; }
			public bool LessThan          (Int64? op1, Int64? op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Int64? op1, Int64? op2) { return op1 <= op2; }
		}

		#endregion

		#region Byte

		private class NU8 : IOperable<Byte?>
		{
			public Byte? Addition         (Byte? op1, Byte? op2) { return (Byte?)(op1 + op2); }
			public Byte? Subtraction      (Byte? op1, Byte? op2) { return (Byte?)(op1 - op2); }
			public Byte? Multiply         (Byte? op1, Byte? op2) { return (Byte?)(op1 * op2); }
			public Byte? Division         (Byte? op1, Byte? op2) { return (Byte?)(op1 / op2); }
			public Byte? Modulus          (Byte? op1, Byte? op2) { return (Byte?)(op1 % op2); }

			public Byte? BitwiseAnd       (Byte? op1, Byte? op2) { return (Byte?)(op1 & op2); }
			public Byte? BitwiseOr        (Byte? op1, Byte? op2) { return (Byte?)(op1 | op2); }
			public Byte? ExclusiveOr      (Byte? op1, Byte? op2) { return (Byte?)(op1 ^ op2); }

			public Byte? UnaryNegation    (Byte? op)             { throw new InvalidOperationException(); }
			public Byte? OnesComplement   (Byte? op)             { return (Byte?)(~op); }

			public bool Equality          (Byte? op1, Byte? op2) { return op1 == op2; }
			public bool Inequality        (Byte? op1, Byte? op2) { return op1 != op2; }
			public bool GreaterThan       (Byte? op1, Byte? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Byte? op1, Byte? op2) { return op1 >= op2; }
			public bool LessThan          (Byte? op1, Byte? op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Byte? op1, Byte? op2) { return op1 <= op2; }
		}

		#endregion

		#region UInt16

		private class NU16 : IOperable<UInt16?>
		{
			public UInt16? Addition       (UInt16? op1, UInt16? op2) { return (UInt16?)(op1 + op2); }
			public UInt16? Subtraction    (UInt16? op1, UInt16? op2) { return (UInt16?)(op1 - op2); }
			public UInt16? Multiply       (UInt16? op1, UInt16? op2) { return (UInt16?)(op1 * op2); }
			public UInt16? Division       (UInt16? op1, UInt16? op2) { return (UInt16?)(op1 / op2); }
			public UInt16? Modulus        (UInt16? op1, UInt16? op2) { return (UInt16?)(op1 % op2); }

			public UInt16? BitwiseAnd     (UInt16? op1, UInt16? op2) { return (UInt16?)(op1 & op2); }
			public UInt16? BitwiseOr      (UInt16? op1, UInt16? op2) { return (UInt16?)(op1 | op2); }
			public UInt16? ExclusiveOr    (UInt16? op1, UInt16? op2) { return (UInt16?)(op1 ^ op2); }

			public UInt16? UnaryNegation  (UInt16? op)               { throw new InvalidOperationException(); }
			public UInt16? OnesComplement (UInt16? op)               { return (UInt16?)(~op); }

			public bool Equality          (UInt16? op1, UInt16? op2) { return op1 == op2; }
			public bool Inequality        (UInt16? op1, UInt16? op2) { return op1 != op2; }
			public bool GreaterThan       (UInt16? op1, UInt16? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(UInt16? op1, UInt16? op2) { return op1 >= op2; }
			public bool LessThan          (UInt16? op1, UInt16? op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (UInt16? op1, UInt16? op2) { return op1 <= op2; }
		}

		#endregion

		#region UInt32

		private class NU32 : IOperable<UInt32?>
		{
			public UInt32? Addition       (UInt32? op1, UInt32? op2) { return (op1 + op2); }
			public UInt32? Subtraction    (UInt32? op1, UInt32? op2) { return (op1 - op2); }
			public UInt32? Multiply       (UInt32? op1, UInt32? op2) { return (op1 * op2); }
			public UInt32? Division       (UInt32? op1, UInt32? op2) { return (op1 / op2); }
			public UInt32? Modulus        (UInt32? op1, UInt32? op2) { return (op1 % op2); }

			public UInt32? BitwiseAnd     (UInt32? op1, UInt32? op2) { return (op1 & op2); }
			public UInt32? BitwiseOr      (UInt32? op1, UInt32? op2) { return (op1 | op2); }
			public UInt32? ExclusiveOr    (UInt32? op1, UInt32? op2) { return (op1 ^ op2); }
			 
			public UInt32? UnaryNegation  (UInt32? op)               { throw new InvalidOperationException(); }
			public UInt32? OnesComplement (UInt32? op)               { return (~op); }

			public bool Equality          (UInt32? op1, UInt32? op2) { return op1 == op2; }
			public bool Inequality        (UInt32? op1, UInt32? op2) { return op1 != op2; }
			public bool GreaterThan       (UInt32? op1, UInt32? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(UInt32? op1, UInt32? op2) { return op1 >= op2; }
			public bool LessThan          (UInt32? op1, UInt32? op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (UInt32? op1, UInt32? op2) { return op1 <= op2; }
		}

		#endregion

		#region UInt64

		private class NU64 : IOperable<UInt64?>
		{
			public UInt64? Addition       (UInt64? op1, UInt64? op2) { return (op1 + op2); }
			public UInt64? Subtraction    (UInt64? op1, UInt64? op2) { return (op1 - op2); }
			public UInt64? Multiply       (UInt64? op1, UInt64? op2) { return (op1 * op2); }
			public UInt64? Division       (UInt64? op1, UInt64? op2) { return (op1 / op2); }
			public UInt64? Modulus        (UInt64? op1, UInt64? op2) { return (op1 % op2); }

			public UInt64? BitwiseAnd     (UInt64? op1, UInt64? op2) { return (op1 & op2); }
			public UInt64? BitwiseOr      (UInt64? op1, UInt64? op2) { return (op1 | op2); }
			public UInt64? ExclusiveOr    (UInt64? op1, UInt64? op2) { return (op1 ^ op2); }

			public UInt64? UnaryNegation  (UInt64? op)               { throw new InvalidOperationException(); }
			public UInt64? OnesComplement (UInt64? op)               { return (~op); }

			public bool Equality          (UInt64? op1, UInt64? op2) { return op1 == op2; }
			public bool Inequality        (UInt64? op1, UInt64? op2) { return op1 != op2; }
			public bool GreaterThan       (UInt64? op1, UInt64? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(UInt64? op1, UInt64? op2) { return op1 >= op2; }
			public bool LessThan          (UInt64? op1, UInt64? op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (UInt64? op1, UInt64? op2) { return op1 <= op2; }
		}

		#endregion

		#region Boolean

		private class NB : IOperable<Boolean?>
		{
			public Boolean? Addition      (Boolean? op1, Boolean? op2) { throw new InvalidOperationException(); }
			public Boolean? Subtraction   (Boolean? op1, Boolean? op2) { throw new InvalidOperationException(); }
			public Boolean? Multiply      (Boolean? op1, Boolean? op2) { throw new InvalidOperationException(); }
			public Boolean? Division      (Boolean? op1, Boolean? op2) { throw new InvalidOperationException(); }
			public Boolean? Modulus       (Boolean? op1, Boolean? op2) { throw new InvalidOperationException(); }

			public Boolean? BitwiseAnd    (Boolean? op1, Boolean? op2) { return (op1 & op2); }
			public Boolean? BitwiseOr     (Boolean? op1, Boolean? op2) { return (op1 | op2); }
			public Boolean? ExclusiveOr   (Boolean? op1, Boolean? op2) { return (op1 ^ op2); }

			public Boolean? UnaryNegation (Boolean? op)                { throw new InvalidOperationException(); }
			public Boolean? OnesComplement(Boolean? op)                { return !op; }

			public bool Equality          (Boolean? op1, Boolean? op2) { return op1 == op2; }
			public bool Inequality        (Boolean? op1, Boolean? op2) { return op1 != op2; }
			public bool GreaterThan       (Boolean? op1, Boolean? op2) { throw new InvalidOperationException(); }
			public bool GreaterThanOrEqual(Boolean? op1, Boolean? op2) { throw new InvalidOperationException(); }
			public bool LessThan          (Boolean? op1, Boolean? op2) { throw new InvalidOperationException(); }
			public bool LessThanOrEqual   (Boolean? op1, Boolean? op2) { throw new InvalidOperationException(); }
		}

		#endregion

		#region Char

		private class NC : IOperable<Char?>
		{
			public Char? Addition         (Char? op1, Char? op2) { return (Char?)(op1 + op2); }
			public Char? Subtraction      (Char? op1, Char? op2) { return (Char?)(op1 - op2); }
			public Char? Multiply         (Char? op1, Char? op2) { return (Char?)(op1 * op2); }
			public Char? Division         (Char? op1, Char? op2) { return (Char?)(op1 / op2); }
			public Char? Modulus          (Char? op1, Char? op2) { return (Char?)(op1 % op2); }

			public Char? BitwiseAnd       (Char? op1, Char? op2) { return (Char?)(op1 & op2); }
			public Char? BitwiseOr        (Char? op1, Char? op2) { return (Char?)(op1 | op2); }
			public Char? ExclusiveOr      (Char? op1, Char? op2) { return (Char?)(op1 ^ op2); }

			public Char? UnaryNegation    (Char? op)             { return (Char?)(-op); }
			public Char? OnesComplement   (Char? op)             { return (Char?)(~op); }

			public bool Equality          (Char? op1, Char? op2) { return op1 == op2; }
			public bool Inequality        (Char? op1, Char? op2) { return op1 != op2; }
			public bool GreaterThan       (Char? op1, Char? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Char? op1, Char? op2) { return op1 >= op2; }
			public bool LessThan          (Char? op1, Char? op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Char? op1, Char? op2) { return op1 <= op2; }
		}

		#endregion

		#region Single

		private class NR4 : IOperable<Single?>
		{
			public Single? Addition       (Single? op1, Single? op2) { return (op1 + op2); }
			public Single? Subtraction    (Single? op1, Single? op2) { return (op1 - op2); }
			public Single? Multiply       (Single? op1, Single? op2) { return (op1 * op2); }
			public Single? Division       (Single? op1, Single? op2) { return (op1 / op2); }
			public Single? Modulus        (Single? op1, Single? op2) { return (op1 % op2); }

			public Single? BitwiseAnd     (Single? op1, Single? op2) { throw new InvalidOperationException(); }
			public Single? BitwiseOr      (Single? op1, Single? op2) { throw new InvalidOperationException(); }
			public Single? ExclusiveOr    (Single? op1, Single? op2) { throw new InvalidOperationException(); }

			public Single? UnaryNegation  (Single? op)               { return (- op); }
			public Single? OnesComplement (Single? op)               { throw new InvalidOperationException(); }

			public bool Equality          (Single? op1, Single? op2) { return op1 == op2; }
			public bool Inequality        (Single? op1, Single? op2) { return op1 != op2; }
			public bool GreaterThan       (Single? op1, Single? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Single? op1, Single? op2) { return op1 >= op2; }
			public bool LessThan          (Single? op1, Single? op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Single? op1, Single? op2) { return op1 <= op2; }
		}

		#endregion

		#region Double

		private class NR8 : IOperable<Double?>
		{
			public Double? Addition       (Double? op1, Double? op2) { return (op1 + op2); }
			public Double? Subtraction    (Double? op1, Double? op2) { return (op1 - op2); }
			public Double? Multiply       (Double? op1, Double? op2) { return (op1 * op2); }
			public Double? Division       (Double? op1, Double? op2) { return (op1 / op2); }
			public Double? Modulus        (Double? op1, Double? op2) { return (op1 % op2); }

			public Double? BitwiseAnd     (Double? op1, Double? op2) { throw new InvalidOperationException(); }
			public Double? BitwiseOr      (Double? op1, Double? op2) { throw new InvalidOperationException(); }
			public Double? ExclusiveOr    (Double? op1, Double? op2) { throw new InvalidOperationException(); }

			public Double? UnaryNegation  (Double? op)               { return (- op); }
			public Double? OnesComplement (Double? op)               { throw new InvalidOperationException(); }

			public bool Equality          (Double? op1, Double? op2) { return op1 == op2; }
			public bool Inequality        (Double? op1, Double? op2) { return op1 != op2; }
			public bool GreaterThan       (Double? op1, Double? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Double? op1, Double? op2) { return op1 >= op2; }
			public bool LessThan          (Double? op1, Double? op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Double? op1, Double? op2) { return op1 <= op2; }
		}

		#endregion

		#region Decimal

		private class ND : IOperable<Decimal?>
		{
			public Decimal? Addition      (Decimal? op1, Decimal? op2) { return (op1 + op2); }
			public Decimal? Subtraction   (Decimal? op1, Decimal? op2) { return (op1 - op2); }
			public Decimal? Multiply      (Decimal? op1, Decimal? op2) { return (op1 * op2); }
			public Decimal? Division      (Decimal? op1, Decimal? op2) { return (op1 / op2); }
			public Decimal? Modulus       (Decimal? op1, Decimal? op2) { return (op1 % op2); }

			public Decimal? BitwiseAnd    (Decimal? op1, Decimal? op2) { throw new InvalidOperationException(); }
			public Decimal? BitwiseOr     (Decimal? op1, Decimal? op2) { throw new InvalidOperationException(); }
			public Decimal? ExclusiveOr   (Decimal? op1, Decimal? op2) { throw new InvalidOperationException(); }

			public Decimal? UnaryNegation (Decimal? op)                { return (- op); }
			public Decimal? OnesComplement(Decimal? op)                { throw new InvalidOperationException(); }

			public bool Equality          (Decimal? op1, Decimal? op2) { return op1 == op2; }
			public bool Inequality        (Decimal? op1, Decimal? op2) { return op1 != op2; }
			public bool GreaterThan       (Decimal? op1, Decimal? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual(Decimal? op1, Decimal? op2) { return op1 >= op2; }
			public bool LessThan          (Decimal? op1, Decimal? op2) { return op1 <  op2; }
			public bool LessThanOrEqual   (Decimal? op1, Decimal? op2) { return op1 <= op2; }
		}

		#endregion

		#region DateTime

		private class NDT : IOperable<DateTime?>
		{
			public DateTime? Addition      (DateTime? op1, DateTime? op2) { throw new InvalidOperationException(); }
			public DateTime? Subtraction   (DateTime? op1, DateTime? op2) { throw new InvalidOperationException(); }
			public DateTime? Multiply      (DateTime? op1, DateTime? op2) { throw new InvalidOperationException(); }
			public DateTime? Division      (DateTime? op1, DateTime? op2) { throw new InvalidOperationException(); }
			public DateTime? Modulus       (DateTime? op1, DateTime? op2) { throw new InvalidOperationException(); }

			public DateTime? BitwiseAnd    (DateTime? op1, DateTime? op2) { throw new InvalidOperationException(); }
			public DateTime? BitwiseOr     (DateTime? op1, DateTime? op2) { throw new InvalidOperationException(); }
			public DateTime? ExclusiveOr   (DateTime? op1, DateTime? op2) { throw new InvalidOperationException(); }

			public DateTime? UnaryNegation (DateTime? op)                 { throw new InvalidOperationException(); }
			public DateTime? OnesComplement(DateTime? op)                 { throw new InvalidOperationException(); }

			public bool Equality           (DateTime? op1, DateTime? op2) { return op1 == op2; }
			public bool Inequality         (DateTime? op1, DateTime? op2) { return op1 != op2; }
			public bool GreaterThan        (DateTime? op1, DateTime? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual (DateTime? op1, DateTime? op2) { return op1 >= op2; }
			public bool LessThan           (DateTime? op1, DateTime? op2) { return op1 <  op2; }
			public bool LessThanOrEqual    (DateTime? op1, DateTime? op2) { return op1 <= op2; }
		}

		#endregion

		#region TimeSpan

		private class NTS : IOperable<TimeSpan?>
		{
			public TimeSpan? Addition      (TimeSpan? op1, TimeSpan? op2) { return (op1 + op2); }
			public TimeSpan? Subtraction   (TimeSpan? op1, TimeSpan? op2) { return (op1 - op2); }
			public TimeSpan? Multiply      (TimeSpan? op1, TimeSpan? op2) { throw new InvalidOperationException(); }
			public TimeSpan? Division      (TimeSpan? op1, TimeSpan? op2) { throw new InvalidOperationException(); }
			public TimeSpan? Modulus       (TimeSpan? op1, TimeSpan? op2) { throw new InvalidOperationException(); }

			public TimeSpan? BitwiseAnd    (TimeSpan? op1, TimeSpan? op2) { throw new InvalidOperationException(); }
			public TimeSpan? BitwiseOr     (TimeSpan? op1, TimeSpan? op2) { throw new InvalidOperationException(); }
			public TimeSpan? ExclusiveOr   (TimeSpan? op1, TimeSpan? op2) { throw new InvalidOperationException(); }

			public TimeSpan? UnaryNegation (TimeSpan? op)                 { return (- op); }
			public TimeSpan? OnesComplement(TimeSpan? op)                 { throw new InvalidOperationException(); }

			public bool Equality           (TimeSpan? op1, TimeSpan? op2) { return op1 == op2; }
			public bool Inequality         (TimeSpan? op1, TimeSpan? op2) { return op1 != op2; }
			public bool GreaterThan        (TimeSpan? op1, TimeSpan? op2) { return op1 >  op2; }
			public bool GreaterThanOrEqual (TimeSpan? op1, TimeSpan? op2) { return op1 >= op2; }
			public bool LessThan           (TimeSpan? op1, TimeSpan? op2) { return op1 <  op2; }
			public bool LessThanOrEqual    (TimeSpan? op1, TimeSpan? op2) { return op1 <= op2; }
		}

		#endregion

		#region Guid?

		private class NG : IOperable<Guid?>
		{
			public Guid? Addition         (Guid? op1, Guid? op2) { throw new InvalidOperationException(); }
			public Guid? Subtraction      (Guid? op1, Guid? op2) { throw new InvalidOperationException(); }
			public Guid? Multiply         (Guid? op1, Guid? op2) { throw new InvalidOperationException(); }
			public Guid? Division         (Guid? op1, Guid? op2) { throw new InvalidOperationException(); }
			public Guid? Modulus          (Guid? op1, Guid? op2) { throw new InvalidOperationException(); }

			public Guid? BitwiseAnd       (Guid? op1, Guid? op2) { throw new InvalidOperationException(); }
			public Guid? BitwiseOr        (Guid? op1, Guid? op2) { throw new InvalidOperationException(); }
			public Guid? ExclusiveOr      (Guid? op1, Guid? op2) { throw new InvalidOperationException(); }

			public Guid? UnaryNegation    (Guid? op)             { throw new InvalidOperationException(); }
			public Guid? OnesComplement   (Guid? op)             { throw new InvalidOperationException(); }

			public bool Equality          (Guid? op1, Guid? op2) { return op1 == op2; }
			public bool Inequality        (Guid? op1, Guid? op2) { return op1 != op2; }
			public bool GreaterThan       (Guid? op1, Guid? op2) { throw new InvalidOperationException(); }
			public bool GreaterThanOrEqual(Guid? op1, Guid? op2) { throw new InvalidOperationException(); }
			public bool LessThan          (Guid? op1, Guid? op2) { throw new InvalidOperationException(); }
			public bool LessThanOrEqual   (Guid? op1, Guid? op2) { throw new InvalidOperationException(); }
		}

		#endregion

		#endregion

		#region Sql types.

		#region SqlString

		private class DBS : IOperable<SqlString>
		{
			public SqlString Addition      (SqlString op1, SqlString op2) { return (op1 + op2); }
			public SqlString Subtraction   (SqlString op1, SqlString op2) { throw new InvalidOperationException(); }
			public SqlString Multiply      (SqlString op1, SqlString op2) { throw new InvalidOperationException(); }
			public SqlString Division      (SqlString op1, SqlString op2) { throw new InvalidOperationException(); }
			public SqlString Modulus       (SqlString op1, SqlString op2) { throw new InvalidOperationException(); }

			public SqlString BitwiseAnd    (SqlString op1, SqlString op2) { throw new InvalidOperationException(); }
			public SqlString BitwiseOr     (SqlString op1, SqlString op2) { throw new InvalidOperationException(); }
			public SqlString ExclusiveOr   (SqlString op1, SqlString op2) { throw new InvalidOperationException(); }

			public SqlString UnaryNegation (SqlString op)                 { throw new InvalidOperationException(); }
			public SqlString OnesComplement(SqlString op)                 { throw new InvalidOperationException(); }

			public bool Equality           (SqlString op1, SqlString op2) { return (op1 == op2).IsTrue; }
			public bool Inequality         (SqlString op1, SqlString op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan        (SqlString op1, SqlString op2) { throw new InvalidOperationException(); }
			public bool GreaterThanOrEqual (SqlString op1, SqlString op2) { throw new InvalidOperationException(); }
			public bool LessThan           (SqlString op1, SqlString op2) { throw new InvalidOperationException(); }
			public bool LessThanOrEqual    (SqlString op1, SqlString op2) { throw new InvalidOperationException(); }
		}

		#endregion

		#region SqlByte

		private class DBU8 : IOperable<SqlByte>
		{
			public SqlByte Addition       (SqlByte op1, SqlByte op2) { return (op1 + op2); }
			public SqlByte Subtraction    (SqlByte op1, SqlByte op2) { return (op1 - op2); }
			public SqlByte Multiply       (SqlByte op1, SqlByte op2) { return (op1 * op2); }
			public SqlByte Division       (SqlByte op1, SqlByte op2) { return (op1 / op2); }
			public SqlByte Modulus        (SqlByte op1, SqlByte op2) { return (op1 % op2); }

			public SqlByte BitwiseAnd     (SqlByte op1, SqlByte op2) { return (op1 & op2); }
			public SqlByte BitwiseOr      (SqlByte op1, SqlByte op2) { return (op1 | op2); }
			public SqlByte ExclusiveOr    (SqlByte op1, SqlByte op2) { return (op1 ^ op2); }

			public SqlByte UnaryNegation  (SqlByte op)               { throw new InvalidOperationException(); }
			public SqlByte OnesComplement (SqlByte op)               { return (~op); }

			public bool Equality          (SqlByte op1, SqlByte op2) { return (op1 == op2).IsTrue; }
			public bool Inequality        (SqlByte op1, SqlByte op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan       (SqlByte op1, SqlByte op2) { return (op1 >  op2).IsTrue; }
			public bool GreaterThanOrEqual(SqlByte op1, SqlByte op2) { return (op1 >= op2).IsTrue; }
			public bool LessThan          (SqlByte op1, SqlByte op2) { return (op1 <  op2).IsTrue; }
			public bool LessThanOrEqual   (SqlByte op1, SqlByte op2) { return (op1 <= op2).IsTrue; }
		}

		#endregion

		#region Int16

		private class DBS16 : IOperable<SqlInt16>
		{
			public SqlInt16 Addition      (SqlInt16 op1, SqlInt16 op2) { return (op1 + op2); }
			public SqlInt16 Subtraction   (SqlInt16 op1, SqlInt16 op2) { return (op1 - op2); }
			public SqlInt16 Multiply      (SqlInt16 op1, SqlInt16 op2) { return (op1 * op2); }
			public SqlInt16 Division      (SqlInt16 op1, SqlInt16 op2) { return (op1 / op2); }
			public SqlInt16 Modulus       (SqlInt16 op1, SqlInt16 op2) { return (op1 % op2); }

			public SqlInt16 BitwiseAnd    (SqlInt16 op1, SqlInt16 op2) { return (op1 & op2); }
			public SqlInt16 BitwiseOr     (SqlInt16 op1, SqlInt16 op2) { return (op1 | op2); }
			public SqlInt16 ExclusiveOr   (SqlInt16 op1, SqlInt16 op2) { return (op1 ^ op2); }

			public SqlInt16 UnaryNegation (SqlInt16 op)                { return (-op); }
			public SqlInt16 OnesComplement(SqlInt16 op)                { return (~op); }

			public bool Equality          (SqlInt16 op1, SqlInt16 op2) { return (op1 == op2).IsTrue; }
			public bool Inequality        (SqlInt16 op1, SqlInt16 op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan       (SqlInt16 op1, SqlInt16 op2) { return (op1 >  op2).IsTrue; }
			public bool GreaterThanOrEqual(SqlInt16 op1, SqlInt16 op2) { return (op1 >= op2).IsTrue; }
			public bool LessThan          (SqlInt16 op1, SqlInt16 op2) { return (op1 <  op2).IsTrue; }
			public bool LessThanOrEqual   (SqlInt16 op1, SqlInt16 op2) { return (op1 <= op2).IsTrue; }
		}

		#endregion

		#region SqlInt32

		private class DBS32 : IOperable<SqlInt32>
		{
			public SqlInt32 Addition      (SqlInt32 op1, SqlInt32 op2) { return (op1 + op2); }
			public SqlInt32 Subtraction   (SqlInt32 op1, SqlInt32 op2) { return (op1 - op2); }
			public SqlInt32 Multiply      (SqlInt32 op1, SqlInt32 op2) { return (op1 * op2); }
			public SqlInt32 Division      (SqlInt32 op1, SqlInt32 op2) { return (op1 / op2); }
			public SqlInt32 Modulus       (SqlInt32 op1, SqlInt32 op2) { return (op1 % op2); }

			public SqlInt32 BitwiseAnd    (SqlInt32 op1, SqlInt32 op2) { return (op1 & op2); }
			public SqlInt32 BitwiseOr     (SqlInt32 op1, SqlInt32 op2) { return (op1 | op2); }
			public SqlInt32 ExclusiveOr   (SqlInt32 op1, SqlInt32 op2) { return (op1 ^ op2); }

			public SqlInt32 UnaryNegation (SqlInt32 op)                { return (-op); }
			public SqlInt32 OnesComplement(SqlInt32 op)                { return (~op); }

			public bool Equality          (SqlInt32 op1, SqlInt32 op2) { return (op1 == op2).IsTrue; }
			public bool Inequality        (SqlInt32 op1, SqlInt32 op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan       (SqlInt32 op1, SqlInt32 op2) { return (op1 >  op2).IsTrue; }
			public bool GreaterThanOrEqual(SqlInt32 op1, SqlInt32 op2) { return (op1 >= op2).IsTrue; }
			public bool LessThan          (SqlInt32 op1, SqlInt32 op2) { return (op1 <  op2).IsTrue; }
			public bool LessThanOrEqual   (SqlInt32 op1, SqlInt32 op2) { return (op1 <= op2).IsTrue; }
		}

		#endregion

		#region SqlInt64

		private class DBS64 : IOperable<SqlInt64>
		{
			public SqlInt64 Addition      (SqlInt64 op1, SqlInt64 op2) { return (op1 + op2); }
			public SqlInt64 Subtraction   (SqlInt64 op1, SqlInt64 op2) { return (op1 - op2); }
			public SqlInt64 Multiply      (SqlInt64 op1, SqlInt64 op2) { return (op1 * op2); }
			public SqlInt64 Division      (SqlInt64 op1, SqlInt64 op2) { return (op1 / op2); }
			public SqlInt64 Modulus       (SqlInt64 op1, SqlInt64 op2) { return (op1 % op2); }

			public SqlInt64 BitwiseAnd    (SqlInt64 op1, SqlInt64 op2) { return (op1 & op2); }
			public SqlInt64 BitwiseOr     (SqlInt64 op1, SqlInt64 op2) { return (op1 | op2); }
			public SqlInt64 ExclusiveOr   (SqlInt64 op1, SqlInt64 op2) { return (op1 ^ op2); }

			public SqlInt64 UnaryNegation (SqlInt64 op)                { return (-op); }
			public SqlInt64 OnesComplement(SqlInt64 op)                { return (~op); }

			public bool Equality          (SqlInt64 op1, SqlInt64 op2) { return (op1 == op2).IsTrue; }
			public bool Inequality        (SqlInt64 op1, SqlInt64 op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan       (SqlInt64 op1, SqlInt64 op2) { return (op1 >  op2).IsTrue; }
			public bool GreaterThanOrEqual(SqlInt64 op1, SqlInt64 op2) { return (op1 >= op2).IsTrue; }
			public bool LessThan          (SqlInt64 op1, SqlInt64 op2) { return (op1 <  op2).IsTrue; }
			public bool LessThanOrEqual   (SqlInt64 op1, SqlInt64 op2) { return (op1 <= op2).IsTrue; }
		}

		#endregion

		#region SqlBoolean

		private class DBB : IOperable<SqlBoolean>
		{
			public SqlBoolean Addition      (SqlBoolean op1, SqlBoolean op2) { throw new InvalidOperationException(); }
			public SqlBoolean Subtraction   (SqlBoolean op1, SqlBoolean op2) { throw new InvalidOperationException(); }
			public SqlBoolean Multiply      (SqlBoolean op1, SqlBoolean op2) { throw new InvalidOperationException(); }
			public SqlBoolean Division      (SqlBoolean op1, SqlBoolean op2) { throw new InvalidOperationException(); }
			public SqlBoolean Modulus       (SqlBoolean op1, SqlBoolean op2) { throw new InvalidOperationException(); }

			public SqlBoolean BitwiseAnd    (SqlBoolean op1, SqlBoolean op2) { return (op1 & op2); }
			public SqlBoolean BitwiseOr     (SqlBoolean op1, SqlBoolean op2) { return (op1 | op2); }
			public SqlBoolean ExclusiveOr   (SqlBoolean op1, SqlBoolean op2) { return (op1 ^ op2); }

			public SqlBoolean UnaryNegation (SqlBoolean op)                  { throw new InvalidOperationException(); }
			public SqlBoolean OnesComplement(SqlBoolean op)                  { return !op; }

			public bool Equality            (SqlBoolean op1, SqlBoolean op2) { return (op1 == op2).IsTrue; }
			public bool Inequality          (SqlBoolean op1, SqlBoolean op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan         (SqlBoolean op1, SqlBoolean op2) { throw new InvalidOperationException(); }
			public bool GreaterThanOrEqual  (SqlBoolean op1, SqlBoolean op2) { throw new InvalidOperationException(); }
			public bool LessThan            (SqlBoolean op1, SqlBoolean op2) { throw new InvalidOperationException(); }
			public bool LessThanOrEqual     (SqlBoolean op1, SqlBoolean op2) { throw new InvalidOperationException(); }
		}

		#endregion

		#region SqlSingle

		private class DBR4 : IOperable<SqlSingle>
		{
			public SqlSingle Addition      (SqlSingle op1, SqlSingle op2) { return (op1 + op2); }
			public SqlSingle Subtraction   (SqlSingle op1, SqlSingle op2) { return (op1 - op2); }
			public SqlSingle Multiply      (SqlSingle op1, SqlSingle op2) { return (op1 * op2); }
			public SqlSingle Division      (SqlSingle op1, SqlSingle op2) { return (op1 / op2); }
			public SqlSingle Modulus       (SqlSingle op1, SqlSingle op2) { throw new InvalidOperationException(); }

			public SqlSingle BitwiseAnd    (SqlSingle op1, SqlSingle op2) { throw new InvalidOperationException(); }
			public SqlSingle BitwiseOr     (SqlSingle op1, SqlSingle op2) { throw new InvalidOperationException(); }
			public SqlSingle ExclusiveOr   (SqlSingle op1, SqlSingle op2) { throw new InvalidOperationException(); }

			public SqlSingle UnaryNegation (SqlSingle op)                 { return (-op); }
			public SqlSingle OnesComplement(SqlSingle op)                 { throw new InvalidOperationException(); }

			public bool Equality           (SqlSingle op1, SqlSingle op2) { return (op1 == op2).IsTrue; }
			public bool Inequality         (SqlSingle op1, SqlSingle op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan        (SqlSingle op1, SqlSingle op2) { return (op1 >  op2).IsTrue; }
			public bool GreaterThanOrEqual (SqlSingle op1, SqlSingle op2) { return (op1 >= op2).IsTrue; }
			public bool LessThan           (SqlSingle op1, SqlSingle op2) { return (op1 <  op2).IsTrue; }
			public bool LessThanOrEqual    (SqlSingle op1, SqlSingle op2) { return (op1 <= op2).IsTrue; }
		}

		#endregion

		#region SqlDouble

		private class DBR8 : IOperable<SqlDouble>
		{
			public SqlDouble Addition      (SqlDouble op1, SqlDouble op2) { return (op1 + op2); }
			public SqlDouble Subtraction   (SqlDouble op1, SqlDouble op2) { return (op1 - op2); }
			public SqlDouble Multiply      (SqlDouble op1, SqlDouble op2) { return (op1 * op2); }
			public SqlDouble Division      (SqlDouble op1, SqlDouble op2) { return (op1 / op2); }
			public SqlDouble Modulus       (SqlDouble op1, SqlDouble op2) { throw new InvalidOperationException(); }

			public SqlDouble BitwiseAnd    (SqlDouble op1, SqlDouble op2) { throw new InvalidOperationException(); }
			public SqlDouble BitwiseOr     (SqlDouble op1, SqlDouble op2) { throw new InvalidOperationException(); }
			public SqlDouble ExclusiveOr   (SqlDouble op1, SqlDouble op2) { throw new InvalidOperationException(); }

			public SqlDouble UnaryNegation (SqlDouble op)                 { return (-op); }
			public SqlDouble OnesComplement(SqlDouble op)                 { throw new InvalidOperationException(); }

			public bool Equality           (SqlDouble op1, SqlDouble op2) { return (op1 == op2).IsTrue; }
			public bool Inequality         (SqlDouble op1, SqlDouble op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan        (SqlDouble op1, SqlDouble op2) { return (op1 >  op2).IsTrue; }
			public bool GreaterThanOrEqual (SqlDouble op1, SqlDouble op2) { return (op1 >= op2).IsTrue; }
			public bool LessThan           (SqlDouble op1, SqlDouble op2) { return (op1 <  op2).IsTrue; }
			public bool LessThanOrEqual    (SqlDouble op1, SqlDouble op2) { return (op1 <= op2).IsTrue; }
		}

		#endregion

		#region SqlDecimal

		private class DBD : IOperable<SqlDecimal>
		{
			public SqlDecimal Addition      (SqlDecimal op1, SqlDecimal op2) { return (op1 + op2); }
			public SqlDecimal Subtraction   (SqlDecimal op1, SqlDecimal op2) { return (op1 - op2); }
			public SqlDecimal Multiply      (SqlDecimal op1, SqlDecimal op2) { return (op1 * op2); }
			public SqlDecimal Division      (SqlDecimal op1, SqlDecimal op2) { return (op1 / op2); }
			public SqlDecimal Modulus       (SqlDecimal op1, SqlDecimal op2) { throw new InvalidOperationException(); }

			public SqlDecimal BitwiseAnd    (SqlDecimal op1, SqlDecimal op2) { throw new InvalidOperationException(); }
			public SqlDecimal BitwiseOr     (SqlDecimal op1, SqlDecimal op2) { throw new InvalidOperationException(); }
			public SqlDecimal ExclusiveOr   (SqlDecimal op1, SqlDecimal op2) { throw new InvalidOperationException(); }

			public SqlDecimal UnaryNegation (SqlDecimal op)                  { return (-op); }
			public SqlDecimal OnesComplement(SqlDecimal op)                  { throw new InvalidOperationException(); }

			public bool Equality            (SqlDecimal op1, SqlDecimal op2) { return (op1 == op2).IsTrue; }
			public bool Inequality          (SqlDecimal op1, SqlDecimal op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan         (SqlDecimal op1, SqlDecimal op2) { return (op1 >  op2).IsTrue; }
			public bool GreaterThanOrEqual  (SqlDecimal op1, SqlDecimal op2) { return (op1 >= op2).IsTrue; }
			public bool LessThan            (SqlDecimal op1, SqlDecimal op2) { return (op1 <  op2).IsTrue; }
			public bool LessThanOrEqual     (SqlDecimal op1, SqlDecimal op2) { return (op1 <= op2).IsTrue; }
		}

		#endregion

		#region SqlMoney

		private class DBM : IOperable<SqlMoney>
		{
			public SqlMoney Addition      (SqlMoney op1, SqlMoney op2) { return (op1 + op2); }
			public SqlMoney Subtraction   (SqlMoney op1, SqlMoney op2) { return (op1 - op2); }
			public SqlMoney Multiply      (SqlMoney op1, SqlMoney op2) { return (op1 * op2); }
			public SqlMoney Division      (SqlMoney op1, SqlMoney op2) { return (op1 / op2); }
			public SqlMoney Modulus       (SqlMoney op1, SqlMoney op2) { throw new InvalidOperationException(); }

			public SqlMoney BitwiseAnd    (SqlMoney op1, SqlMoney op2) { throw new InvalidOperationException(); }
			public SqlMoney BitwiseOr     (SqlMoney op1, SqlMoney op2) { throw new InvalidOperationException(); }
			public SqlMoney ExclusiveOr   (SqlMoney op1, SqlMoney op2) { throw new InvalidOperationException(); }

			public SqlMoney UnaryNegation (SqlMoney op)                { return (-op); }
			public SqlMoney OnesComplement(SqlMoney op)                { throw new InvalidOperationException(); }

			public bool Equality          (SqlMoney op1, SqlMoney op2) { return (op1 == op2).IsTrue; }
			public bool Inequality        (SqlMoney op1, SqlMoney op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan       (SqlMoney op1, SqlMoney op2) { return (op1 >  op2).IsTrue; }
			public bool GreaterThanOrEqual(SqlMoney op1, SqlMoney op2) { return (op1 >= op2).IsTrue; }
			public bool LessThan          (SqlMoney op1, SqlMoney op2) { return (op1 <  op2).IsTrue; }
			public bool LessThanOrEqual   (SqlMoney op1, SqlMoney op2) { return (op1 <= op2).IsTrue; }
		}

		#endregion

		#region SqlDateTime

		private class DBDT : IOperable<SqlDateTime>
		{
			public SqlDateTime Addition      (SqlDateTime op1, SqlDateTime op2) { throw new InvalidOperationException(); }
			public SqlDateTime Subtraction   (SqlDateTime op1, SqlDateTime op2) { throw new InvalidOperationException(); }
			public SqlDateTime Multiply      (SqlDateTime op1, SqlDateTime op2) { throw new InvalidOperationException(); }
			public SqlDateTime Division      (SqlDateTime op1, SqlDateTime op2) { throw new InvalidOperationException(); }
			public SqlDateTime Modulus       (SqlDateTime op1, SqlDateTime op2) { throw new InvalidOperationException(); }

			public SqlDateTime BitwiseAnd    (SqlDateTime op1, SqlDateTime op2) { throw new InvalidOperationException(); }
			public SqlDateTime BitwiseOr     (SqlDateTime op1, SqlDateTime op2) { throw new InvalidOperationException(); }
			public SqlDateTime ExclusiveOr   (SqlDateTime op1, SqlDateTime op2) { throw new InvalidOperationException(); }

			public SqlDateTime UnaryNegation (SqlDateTime op)                   { throw new InvalidOperationException(); }
			public SqlDateTime OnesComplement(SqlDateTime op)                   { throw new InvalidOperationException(); }

			public bool Equality             (SqlDateTime op1, SqlDateTime op2) { return (op1 == op2).IsTrue; }
			public bool Inequality           (SqlDateTime op1, SqlDateTime op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan          (SqlDateTime op1, SqlDateTime op2) { return (op1 >  op2).IsTrue; }
			public bool GreaterThanOrEqual   (SqlDateTime op1, SqlDateTime op2) { return (op1 >= op2).IsTrue; }
			public bool LessThan             (SqlDateTime op1, SqlDateTime op2) { return (op1 <  op2).IsTrue; }
			public bool LessThanOrEqual      (SqlDateTime op1, SqlDateTime op2) { return (op1 <= op2).IsTrue; }
		}

		#endregion

		#region SqlBinary

		private class DBBin : IOperable<SqlBinary>
		{
			public SqlBinary Addition      (SqlBinary op1, SqlBinary op2) { return (op1 + op2); }
			public SqlBinary Subtraction   (SqlBinary op1, SqlBinary op2) { throw new InvalidOperationException(); }
			public SqlBinary Multiply      (SqlBinary op1, SqlBinary op2) { throw new InvalidOperationException(); }
			public SqlBinary Division      (SqlBinary op1, SqlBinary op2) { throw new InvalidOperationException(); }
			public SqlBinary Modulus       (SqlBinary op1, SqlBinary op2) { throw new InvalidOperationException(); }

			public SqlBinary BitwiseAnd    (SqlBinary op1, SqlBinary op2) { throw new InvalidOperationException(); }
			public SqlBinary BitwiseOr     (SqlBinary op1, SqlBinary op2) { throw new InvalidOperationException(); }
			public SqlBinary ExclusiveOr   (SqlBinary op1, SqlBinary op2) { throw new InvalidOperationException(); }

			public SqlBinary UnaryNegation (SqlBinary op)                 { throw new InvalidOperationException(); }
			public SqlBinary OnesComplement(SqlBinary op)                 { throw new InvalidOperationException(); }

			public bool Equality           (SqlBinary op1, SqlBinary op2) { return (op1 == op2).IsTrue; }
			public bool Inequality         (SqlBinary op1, SqlBinary op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan        (SqlBinary op1, SqlBinary op2) { return (op1 >  op2).IsTrue; }
			public bool GreaterThanOrEqual (SqlBinary op1, SqlBinary op2) { return (op1 >= op2).IsTrue; }
			public bool LessThan           (SqlBinary op1, SqlBinary op2) { return (op1 <  op2).IsTrue; }
			public bool LessThanOrEqual    (SqlBinary op1, SqlBinary op2) { return (op1 <= op2).IsTrue; }
		}

		#endregion

		#region SqlGuid

		private class DBG : IOperable<SqlGuid>
		{
			public SqlGuid Addition       (SqlGuid op1, SqlGuid op2) { throw new InvalidOperationException(); }
			public SqlGuid Subtraction    (SqlGuid op1, SqlGuid op2) { throw new InvalidOperationException(); }
			public SqlGuid Multiply       (SqlGuid op1, SqlGuid op2) { throw new InvalidOperationException(); }
			public SqlGuid Division       (SqlGuid op1, SqlGuid op2) { throw new InvalidOperationException(); }
			public SqlGuid Modulus        (SqlGuid op1, SqlGuid op2) { throw new InvalidOperationException(); }

			public SqlGuid BitwiseAnd     (SqlGuid op1, SqlGuid op2) { throw new InvalidOperationException(); }
			public SqlGuid BitwiseOr      (SqlGuid op1, SqlGuid op2) { throw new InvalidOperationException(); }
			public SqlGuid ExclusiveOr    (SqlGuid op1, SqlGuid op2) { throw new InvalidOperationException(); }

			public SqlGuid UnaryNegation  (SqlGuid op)               { throw new InvalidOperationException(); }
			public SqlGuid OnesComplement (SqlGuid op)               { throw new InvalidOperationException(); }

			public bool Equality          (SqlGuid op1, SqlGuid op2) { return (op1 == op2).IsTrue; }
			public bool Inequality        (SqlGuid op1, SqlGuid op2) { return (op1 != op2).IsTrue; }
			public bool GreaterThan       (SqlGuid op1, SqlGuid op2) { return (op1 >  op2).IsTrue; }
			public bool GreaterThanOrEqual(SqlGuid op1, SqlGuid op2) { return (op1 >= op2).IsTrue; }
			public bool LessThan          (SqlGuid op1, SqlGuid op2) { return (op1 <  op2).IsTrue; }
			public bool LessThanOrEqual   (SqlGuid op1, SqlGuid op2) { return (op1 <= op2).IsTrue; }
		}

		#endregion

		#endregion
	}

	public static class Operator
	{
		public static T    Addition          <T>(T a, T b) { return Operator<T>.Op.Addition          (a, b); }
		public static T    Subtraction       <T>(T a, T b) { return Operator<T>.Op.Subtraction       (a, b); }
		public static T    Multiply          <T>(T a, T b) { return Operator<T>.Op.Multiply          (a, b); }
		public static T    Division          <T>(T a, T b) { return Operator<T>.Op.Division          (a, b); }
		public static T    Modulus           <T>(T a, T b) { return Operator<T>.Op.Modulus           (a, b); }

		public static T    BitwiseAnd        <T>(T a, T b) { return Operator<T>.Op.BitwiseAnd        (a, b); }
		public static T    BitwiseOr         <T>(T a, T b) { return Operator<T>.Op.BitwiseOr         (a, b); }
		public static T    ExclusiveOr       <T>(T a, T b) { return Operator<T>.Op.ExclusiveOr       (a, b); }

		public static T    UnaryNegation     <T>(T a)      { return Operator<T>.Op.UnaryNegation     (a);    }
		public static T    OnesComplement    <T>(T a)      { return Operator<T>.Op.OnesComplement    (a);    }

		public static bool Equality          <T>(T a, T b) { return Operator<T>.Op.Equality          (a, b); }
		public static bool Inequality        <T>(T a, T b) { return Operator<T>.Op.Inequality        (a, b); }
		public static bool GreaterThan       <T>(T a, T b) { return Operator<T>.Op.GreaterThan       (a, b); }
		public static bool GreaterThanOrEqual<T>(T a, T b) { return Operator<T>.Op.GreaterThanOrEqual(a, b); }
		public static bool LessThan          <T>(T a, T b) { return Operator<T>.Op.LessThan          (a, b); }
		public static bool LessThanOrEqual   <T>(T a, T b) { return Operator<T>.Op.LessThanOrEqual   (a, b); }
	}
}
