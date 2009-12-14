using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using Linq;

	using B = Boolean;
	using C = Char;
	using S = String;
	using I = Int32;
	using O = Object;
	using D = DateTime;
	using F = Double;

	partial class BasicSqlProvider
	{
		protected virtual Dictionary<MemberInfo,BaseExpressor> GetExpressors()
		{
			return null;
		}

		public virtual Expression ConvertMember(MemberInfo mi)
		{
			BaseExpressor expr;

			var expressors = GetExpressors();

			if (expressors != null)
				if (expressors.TryGetValue(mi, out expr))
					return expr.Expr;

			return _expressors.TryGetValue(mi, out expr) ? expr.Expr : null;
		}

		protected static MemberInfo MI<T>(Expression<Func<T,object>> func)
		{
			var ex = func.Body;

			if (ex is UnaryExpression)
				ex = ((UnaryExpression)func.Body).Operand;

			return ex is MemberExpression?
				((MemberExpression)    ex).Member:
				((MethodCallExpression)ex).Method;
		}

		protected static MemberInfo MI(Expression<Func<object>> func)
		{
			return ReflectionHelper.MemeberInfo(func);
		}

		protected abstract class BaseExpressor
		{
			public Expression Expr;

			protected void Set(LambdaExpression func)
			{
				Expr = func.Body;

				if (Expr is UnaryExpression)
					Expr = ((UnaryExpression)func.Body).Operand;
			}
		}

		protected class F<TR>             : BaseExpressor { public F(Expression<Func<TR>>             func) { Set(func); } }
		protected class F<T1,TR>          : BaseExpressor { public F(Expression<Func<T1,TR>>          func) { Set(func); } }
		protected class F<T1,T2,TR>       : BaseExpressor { public F(Expression<Func<T1,T2,TR>>       func) { Set(func); } }
		protected class F<T1,T2,T3,TR>    : BaseExpressor { public F(Expression<Func<T1,T2,T3,TR>>    func) { Set(func); } }
		protected class F<T1,T2,T3,T4,TR> : BaseExpressor { public F(Expression<Func<T1,T2,T3,T4,TR>> func) { Set(func); } }

		static readonly Dictionary<MemberInfo,BaseExpressor> _expressors = new Dictionary<MemberInfo,BaseExpressor>
		{
			{ MI(() => "".Length                      ), new F<S,I>      ( obj           => Sql.Length(obj)) },
			{ MI(() => "".Substring  (0)              ), new F<S,I,S>    ((obj,p0)       => Sql.Substring(obj, p0 + 1, obj.Length - p0)) },
			{ MI(() => "".Substring  (0,0)            ), new F<S,I,I,S>  ((obj,p0,p1)    => Sql.Substring(obj, p0 + 1, p1)) },
			{ MI(() => "".IndexOf    ("")             ), new F<S,S,I>    ((obj,p0)       => p0.Length == 0 ? 0  : (Sql.CharIndex(p0, obj)         ?? 0) - 1) },
			{ MI(() => "".IndexOf    ("",0)           ), new F<S,S,I,I>  ((obj,p0,p1)    => p0.Length == 0 && obj.Length > p1 ? p1 : (Sql.CharIndex(p0, obj, p1 + 1) ?? 0) - 1) },
			{ MI(() => "".IndexOf    ("",0,0)         ), new F<S,S,I,I,I>((obj,p0,p1,p2) => p0.Length == 0 && obj.Length > p1 ? p1 : (Sql.CharIndex(p0, Linq.Sql.Left(obj, p2), p1) ?? 0) - 1) },
			{ MI(() => "".IndexOf    (' ')            ), new F<S,C,I>    ((obj,p0)       => (Sql.CharIndex(p0, obj)         ?? 0) - 1) },
			{ MI(() => "".IndexOf    (' ',0)          ), new F<S,C,I,I>  ((obj,p0,p1)    => (Sql.CharIndex(p0, obj, p1 + 1) ?? 0) - 1) },
			{ MI(() => "".IndexOf    (' ',0,0)        ), new F<S,C,I,I,I>((obj,p0,p1,p2) => (Sql.CharIndex(p0, Sql.Left(obj, p2), p1) ?? 0) - 1) },
			{ MI(() => "".LastIndexOf("")             ), new F<S,S,I>    ((obj,p0)       => p0.Length == 0 ? obj.Length - 1 : (Sql.CharIndex(p0, obj)                            ?? 0) == 0 ? -1 : obj.Length - (Sql.CharIndex(Sql.Reverse(p0), Sql.Reverse(obj))                                ?? 0) - p0.Length + 1) },
			{ MI(() => "".LastIndexOf("",0)           ), new F<S,S,I,I>  ((obj,p0,p1)    => p0.Length == 0 ? p1             : (Sql.CharIndex(p0, obj,                    p1 + 1) ?? 0) == 0 ? -1 : obj.Length - (Sql.CharIndex(Sql.Reverse(p0), Sql.Reverse(obj.Substring(p1, obj.Length - p1))) ?? 0) - p0.Length + 1) },
			{ MI(() => "".LastIndexOf("",0,0)         ), new F<S,S,I,I,I>((obj,p0,p1,p2) => p0.Length == 0 ? p1             : (Sql.CharIndex(p0, Sql.Left(obj, p1 + p2), p1 + 1) ?? 0) == 0 ? -1 :    p1 + p2 - (Sql.CharIndex(Sql.Reverse(p0), Sql.Reverse(obj.Substring(p1, p2)))              ?? 0) - p0.Length + 1) },
			{ MI(() => "".LastIndexOf(' ')            ), new F<S,C,I>    ((obj,p0)       => (Sql.CharIndex(p0, obj)         ?? 0)                    == 0 ? -1 : Sql.Length(obj) - (Sql.CharIndex(p0, Sql.Reverse(obj))                                ?? 0)) },
			{ MI(() => "".LastIndexOf(' ',0)          ), new F<S,C,I,I>  ((obj,p0,p1)    => (Sql.CharIndex(p0, obj, p1 + 1) ?? 0)                    == 0 ? -1 : Sql.Length(obj) - (Sql.CharIndex(p0, Sql.Reverse(obj.Substring(p1, obj.Length - p1))) ?? 0)) },
			{ MI(() => "".LastIndexOf(' ',0,0)        ), new F<S,C,I,I,I>((obj,p0,p1,p2) => (Sql.CharIndex(p0, Sql.Left(obj, p1 + p2), p1 + 1) ?? 0) == 0 ? -1 : p1 + p2         - (Sql.CharIndex(p0, Sql.Reverse(obj.Substring(p1, p2)))              ?? 0)) },
			{ MI(() => "".Insert     (0,"")           ), new F<S,I,S,S>  ((obj,p0,p1)    => obj.Length == p0 ? obj + p1 : Sql.Stuff(obj, p0 + 1, 0, p1)) },
			{ MI(() => "".Remove     (0)              ), new F<S,I,S>    ((obj,p0)       => Sql.Left(obj, p0)) },
			{ MI(() => "".Remove     (0,0)            ), new F<S,I,I,S>  ((obj,p0,p1)    => Sql.Stuff(obj, p0 + 1, p1, "")) },
			{ MI(() => "".PadLeft    (0)              ), new F<S,I,S>    ((obj,p0)       => Sql.PadLeft(obj, p0, ' ')) },
			{ MI(() => "".PadLeft    (0,' ')          ), new F<S,I,C,S>  ((obj,p0,p1)    => Sql.PadLeft(obj, p0, p1)) },
			{ MI(() => "".PadRight   (0)              ), new F<S,I,S>    ((obj,p0)       => Sql.PadRight(obj, p0, ' ')) },
			{ MI(() => "".PadRight   (0,' ')          ), new F<S,I,C,S>  ((obj,p0,p1)    => Sql.PadRight(obj, p0, p1)) },
			{ MI(() => "".Replace    ("","")          ), new F<S,S,S,S>  ((obj,p0,p1)    => Sql.Replace(obj, p0, p1)) },
			{ MI(() => "".Replace    (' ',' ')        ), new F<S,C,C,S>  ((obj,p0,p1)    => Sql.Replace(obj, p0, p1)) },
			{ MI(() => "".Trim       ()               ), new F<S,S>      ( obj           => Sql.Trim(obj)) },
			{ MI(() => "".TrimEnd    ()               ), new F<S,S>      ( obj           => Sql.TrimRight(obj)) },
			{ MI(() => "".TrimStart  ()               ), new F<S,S>      ( obj           => Sql.TrimLeft(obj)) },
			{ MI(() => "".ToLower    ()               ), new F<S,S>      ( obj           => Sql.Lower(obj)) },
			{ MI(() => "".ToUpper    ()               ), new F<S,S>      ( obj           => Sql.Upper(obj)) },
			{ MI(() => "".CompareTo  ("")             ), new F<S,S,I>    ((obj,p0)       => ConvertToCaseCompareTo(obj, p0) ) },
			{ MI(() => "".CompareTo  (1)              ), new F<S,O,I>    ((obj,p0)       => ConvertToCaseCompareTo(obj, p0.ToString()) ) },
			{ MI(() => string.IsNullOrEmpty("")       ), new F<S,S,B>    ((obj,p0)       => p0 == null || p0.Length == 0 ) },

			{ MI(() => AltStuff      ("",0,0,"")),       new F<S,I,I,S,S>((p0, p1,p2,p3) => Sql.Left(p0, p1 - 1) + p3 + Sql.Right(p0, p0.Length - (p1 + p2 - 1))) },

			{ MI(() => Sql.GetDate()                  ), new F<D>        (()             => Sql.CurrentTimestamp2 ) },
			{ MI(() => DateTime.Now                   ), new F<D>        (()             => Sql.CurrentTimestamp2 ) },
			{ MI(() => DateTime.Now.Year              ), new F<D,I>      (obj            => Sql.DatePart(Sql.DateParts.Year,        obj)     ) },
			{ MI(() => DateTime.Now.Month             ), new F<D,I>      (obj            => Sql.DatePart(Sql.DateParts.Month,       obj)     ) },
			{ MI(() => DateTime.Now.DayOfYear         ), new F<D,I>      (obj            => Sql.DatePart(Sql.DateParts.DayOfYear,   obj)     ) },
			{ MI(() => DateTime.Now.Day               ), new F<D,I>      (obj            => Sql.DatePart(Sql.DateParts.Day,         obj)     ) },
			{ MI(() => DateTime.Now.DayOfWeek         ), new F<D,I>      (obj            => Sql.DatePart(Sql.DateParts.WeekDay,     obj) - 1 ) },
			{ MI(() => DateTime.Now.Hour              ), new F<D,I>      (obj            => Sql.DatePart(Sql.DateParts.Hour,        obj)     ) },
			{ MI(() => DateTime.Now.Minute            ), new F<D,I>      (obj            => Sql.DatePart(Sql.DateParts.Minute,      obj)     ) },
			{ MI(() => DateTime.Now.Second            ), new F<D,I>      (obj            => Sql.DatePart(Sql.DateParts.Second,      obj)     ) },
			{ MI(() => DateTime.Now.Millisecond       ), new F<D,I>      (obj            => Sql.DatePart(Sql.DateParts.Millisecond, obj)     ) },
			{ MI(() => DateTime.Now.AddYears       (0)), new F<D,I,D>    ((obj,p0)       => Sql.DateAdd (Sql.DateParts.Year,        p0, obj) ) },
			{ MI(() => DateTime.Now.AddMonths      (0)), new F<D,I,D>    ((obj,p0)       => Sql.DateAdd (Sql.DateParts.Month,       p0, obj) ) },
			{ MI(() => DateTime.Now.AddDays        (0)), new F<D,F,D>    ((obj,p0)       => Sql.DateAdd (Sql.DateParts.Day,         p0, obj) ) },
			{ MI(() => DateTime.Now.AddHours       (0)), new F<D,F,D>    ((obj,p0)       => Sql.DateAdd (Sql.DateParts.Hour,        p0, obj) ) },
			{ MI(() => DateTime.Now.AddMinutes     (0)), new F<D,F,D>    ((obj,p0)       => Sql.DateAdd (Sql.DateParts.Minute,      p0, obj) ) },
			{ MI(() => DateTime.Now.AddSeconds     (0)), new F<D,F,D>    ((obj,p0)       => Sql.DateAdd (Sql.DateParts.Second,      p0, obj) ) },
			{ MI(() => DateTime.Now.AddMilliseconds(0)), new F<D,F,D>    ((obj,p0)       => Sql.DateAdd (Sql.DateParts.Millisecond, p0, obj) ) },

			{ MI(() => DateTime.Parse("")             ), new F<S,D>      (p0             => Sql.ConvertTo<DateTime>.From(p0)                 ) },

			#region Convert

			#region ToInt64

			{ MI(() => Convert.ToInt64((Boolean)true)), new F<Boolean, Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((Byte)    0)  ), new F<Byte,    Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((Char)   '0') ), new F<Char,    Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64(DateTime.Now) ), new F<DateTime,Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((Decimal) 0)  ), new F<Decimal, Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((Double)  0)  ), new F<Double,  Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((Int16)   0)  ), new F<Int16,   Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((Int32)   0)  ), new F<Int32,   Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((Int64)   0)  ), new F<Int64,   Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((Object)  0)  ), new F<Object,  Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((SByte)   0)  ), new F<SByte,   Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((Single)  0)  ), new F<Single,  Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((String) "0") ), new F<String,  Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((UInt16)  0)  ), new F<UInt16,  Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((UInt32)  0)  ), new F<UInt32,  Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },
			{ MI(() => Convert.ToInt64((UInt64)  0)  ), new F<UInt64,  Int64>(p0 => Sql.ConvertTo<Int64>.From(p0) ) },

			#endregion

			#region ToInt32

			{ MI(() => Convert.ToInt32((Boolean)true)), new F<Boolean, Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((Byte)    0)  ), new F<Byte,    Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((Char)   '0') ), new F<Char,    Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32(DateTime.Now) ), new F<DateTime,Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((Decimal) 0)  ), new F<Decimal, Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((Double)  0)  ), new F<Double,  Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((Int16)   0)  ), new F<Int16,   Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((Int32)   0)  ), new F<Int32,   Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((Int64)   0)  ), new F<Int64,   Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((Object)  0)  ), new F<Object,  Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((SByte)   0)  ), new F<SByte,   Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((Single)  0)  ), new F<Single,  Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((String) "0") ), new F<String,  Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((UInt16)  0)  ), new F<UInt16,  Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((UInt32)  0)  ), new F<UInt32,  Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },
			{ MI(() => Convert.ToInt32((UInt64)  0)  ), new F<UInt64,  Int32>(p0 => Sql.ConvertTo<Int32>.From(p0) ) },

			#endregion

			#region ToInt16

			{ MI(() => Convert.ToInt16((Boolean)true)), new F<Boolean, Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((Byte)    0)  ), new F<Byte,    Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((Char)   '0') ), new F<Char,    Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16(DateTime.Now) ), new F<DateTime,Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((Decimal) 0)  ), new F<Decimal, Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((Double)  0)  ), new F<Double,  Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((Int16)   0)  ), new F<Int16,   Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((Int32)   0)  ), new F<Int32,   Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((Int64)   0)  ), new F<Int64,   Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((Object)  0)  ), new F<Object,  Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((SByte)   0)  ), new F<SByte,   Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((Single)  0)  ), new F<Single,  Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((String) "0") ), new F<String,  Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((UInt16)  0)  ), new F<UInt16,  Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((UInt32)  0)  ), new F<UInt32,  Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },
			{ MI(() => Convert.ToInt16((UInt64)  0)  ), new F<UInt64,  Int16>(p0 => Sql.ConvertTo<Int16>.From(p0) ) },

			#endregion

			#region ToByte

			{ MI(() => Convert.ToByte((Boolean)true)), new F<Boolean, Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((Byte)    0)  ), new F<Byte,    Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((Char)   '0') ), new F<Char,    Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte(DateTime.Now) ), new F<DateTime,Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((Decimal) 0)  ), new F<Decimal, Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((Double)  0)  ), new F<Double,  Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((Int16)   0)  ), new F<Int16,   Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((Int32)   0)  ), new F<Int32,   Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((Int64)   0)  ), new F<Int64,   Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((Object)  0)  ), new F<Object,  Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((SByte)   0)  ), new F<SByte,   Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((Single)  0)  ), new F<Single,  Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((String) "0") ), new F<String,  Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((UInt16)  0)  ), new F<UInt16,  Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((UInt32)  0)  ), new F<UInt32,  Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },
			{ MI(() => Convert.ToByte((UInt64)  0)  ), new F<UInt64,  Byte>(p0 => Sql.ConvertTo<Byte>.From(p0) ) },

			#endregion

			#region ToDecimal

			{ MI(() => Convert.ToDecimal((Boolean)true)), new F<Boolean, Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((Byte)    0)  ), new F<Byte,    Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((Char)   '0') ), new F<Char,    Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal(DateTime.Now) ), new F<DateTime,Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((Decimal) 0)  ), new F<Decimal, Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((Double)  0)  ), new F<Double,  Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((Int16)   0)  ), new F<Int16,   Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((Int32)   0)  ), new F<Int32,   Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((Int64)   0)  ), new F<Int64,   Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((Object)  0)  ), new F<Object,  Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((SByte)   0)  ), new F<SByte,   Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((Single)  0)  ), new F<Single,  Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((String) "0") ), new F<String,  Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((UInt16)  0)  ), new F<UInt16,  Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((UInt32)  0)  ), new F<UInt32,  Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },
			{ MI(() => Convert.ToDecimal((UInt64)  0)  ), new F<UInt64,  Decimal>(p0 => Sql.ConvertTo<Decimal>.From(p0) ) },

			#endregion

			#region ToDouble

			{ MI(() => Convert.ToDouble((Boolean)true)), new F<Boolean, Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((Byte)    0)  ), new F<Byte,    Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((Char)   '0') ), new F<Char,    Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble(DateTime.Now) ), new F<DateTime,Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((Decimal) 0)  ), new F<Decimal, Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((Double)  0)  ), new F<Double,  Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((Int16)   0)  ), new F<Int16,   Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((Int32)   0)  ), new F<Int32,   Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((Int64)   0)  ), new F<Int64,   Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((Object)  0)  ), new F<Object,  Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((SByte)   0)  ), new F<SByte,   Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((Single)  0)  ), new F<Single,  Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((String) "0") ), new F<String,  Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((UInt16)  0)  ), new F<UInt16,  Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((UInt32)  0)  ), new F<UInt32,  Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },
			{ MI(() => Convert.ToDouble((UInt64)  0)  ), new F<UInt64,  Double>(p0 => Sql.ConvertTo<Double>.From(p0) ) },

			#endregion

			#region ToSingle

			{ MI(() => Convert.ToSingle((Boolean)true)), new F<Boolean, Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((Byte)    0)  ), new F<Byte,    Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((Char)   '0') ), new F<Char,    Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle(DateTime.Now) ), new F<DateTime,Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((Decimal) 0)  ), new F<Decimal, Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((Double)  0)  ), new F<Double,  Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((Int16)   0)  ), new F<Int16,   Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((Int32)   0)  ), new F<Int32,   Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((Int64)   0)  ), new F<Int64,   Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((Object)  0)  ), new F<Object,  Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((SByte)   0)  ), new F<SByte,   Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((Single)  0)  ), new F<Single,  Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((String) "0") ), new F<String,  Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((UInt16)  0)  ), new F<UInt16,  Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((UInt32)  0)  ), new F<UInt32,  Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },
			{ MI(() => Convert.ToSingle((UInt64)  0)  ), new F<UInt64,  Single>(p0 => Sql.ConvertTo<Single>.From(p0) ) },

			#endregion

			#endregion
		};

		[SqlFunction]
		static int ConvertToCaseCompareTo(string str, string value)
		{
			return str.CompareTo(value);
		}

		// Access, DB2, Firebird, Informix, MySql, Oracle, PostgreSQL, SQLite
		//
		[SqlFunction]
		protected static string AltStuff(string str, int startLocation, int length, string value)
		{
			return Sql.Stuff(str, startLocation, length, value);
		}

		// DB2
		//
		[SqlFunction]
		protected static string VarChar(object obj, int size)
		{
			return obj.ToString();
		}

		// DB2, PostgreSQL, Access, MS SQL, SqlCe
		//
		[SqlFunction]
		[SqlFunction("DB2",        "Repeat")]
		[SqlFunction("PostgreSQL", "Repeat")]
		[SqlFunction("Access",     "String", 1, 0)]
		[CLSCompliant(false)]
		protected static string Replicate(string str, int count)
		{
			if (str == null)
				return null;

			var sb = new StringBuilder(str.Length * count);

			for (var i = 0; i < count; i++)
				sb.Append(str);

			return sb.ToString();
		}

		[SqlFunction]
		[SqlFunction("DB2",        "Repeat")]
		[SqlFunction("PostgreSQL", "Repeat")]
		[SqlFunction("Access",     "String", 1, 0)]
		[CLSCompliant(false)]
		protected static string Replicate(char ch, int count)
		{
			var sb = new StringBuilder(count);

			for (var i = 0; i < count; i++)
				sb.Append(ch);

			return sb.ToString();
		}
	}
}
