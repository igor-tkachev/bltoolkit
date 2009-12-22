using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Linq
{
	using B = Boolean;
	using C = Char;
	using S = String;
	using I = Int32;
	using O = Object;
	using D = DateTime;
	using T = TimeSpan;
	using F = Double;

	public static partial class Sql
	{
		#region AddMember

		public static void AddMember(MemberInfo memberInfo, LambdaExpression expression)
		{
			AddMember("", memberInfo, expression);
		}

		public static void AddMember(string providerName, MemberInfo memberInfo, LambdaExpression expression)
		{
			Dictionary<MemberInfo,LambdaExpression> dic;

			if (!_members.TryGetValue(providerName, out dic))
				_members.Add(providerName, dic = new Dictionary<MemberInfo,LambdaExpression>());

			dic[memberInfo] = expression;
		}

		public static void AddMember(Expression<Func<object>> memberInfo, LambdaExpression expression)
		{
			AddMember("", M(memberInfo), expression);
		}

		public static void AddMember(string providerName, Expression<Func<object>> memberInfo, LambdaExpression expression)
		{
			AddMember(providerName, M(memberInfo), expression);
		}

		public static void AddMember<T>(Expression<Func<T,object>> memberInfo, LambdaExpression expression)
		{
			AddMember("", M(memberInfo), expression);
		}

		public static void AddMember<T>(string providerName, Expression<Func<T,object>> memberInfo, LambdaExpression expression)
		{
			AddMember(providerName, M(memberInfo), expression);
		}

		#endregion

		static MemberInfo M<T>(Expression<Func<T,object>> func)
		{
			var ex = func.Body;

			if (ex is UnaryExpression)
				ex = ((UnaryExpression)func.Body).Operand;

			return ex is MemberExpression?
				((MemberExpression)    ex).Member:
				((MethodCallExpression)ex).Method;
		}

		static MemberInfo M(Expression<Func<object>> func)
		{
			return ReflectionHelper.MemeberInfo(func);
		}

		static LambdaExpression L<TR>             (Expression<Func<TR>>             func) { return func; }
		static LambdaExpression L<T1,TR>          (Expression<Func<T1,TR>>          func) { return func; }
		static LambdaExpression L<T1,T2,TR>       (Expression<Func<T1,T2,TR>>       func) { return func; }
		static LambdaExpression L<T1,T2,T3,TR>    (Expression<Func<T1,T2,T3,TR>>    func) { return func; }
		static LambdaExpression L<T1,T2,T3,T4,TR> (Expression<Func<T1,T2,T3,T4,TR>> func) { return func; }

		static public   Dictionary<string,Dictionary<MemberInfo,LambdaExpression>>  Members { get { return _members; } }
		static readonly Dictionary<string,Dictionary<MemberInfo,LambdaExpression>> _members = new Dictionary<string,Dictionary<MemberInfo,LambdaExpression>>
		{
			{ "", new Dictionary<MemberInfo,LambdaExpression> {

				#region string

				{ M(() => "".Length                      ), L<S,I>      ( obj           => Length(obj)) },
				{ M(() => "".Substring  (0)              ), L<S,I,S>    ((obj,p0)       => Substring(obj, p0 + 1, obj.Length - p0)) },
				{ M(() => "".Substring  (0,0)            ), L<S,I,I,S>  ((obj,p0,p1)    => Substring(obj, p0 + 1, p1)) },
				{ M(() => "".IndexOf    ("")             ), L<S,S,I>    ((obj,p0)       => p0.Length == 0                    ? 0  : (CharIndex(p0, obj)                   ?? 0) - 1) },
				{ M(() => "".IndexOf    ("",0)           ), L<S,S,I,I>  ((obj,p0,p1)    => p0.Length == 0 && obj.Length > p1 ? p1 : (CharIndex(p0, obj,           p1 + 1) ?? 0) - 1) },
				{ M(() => "".IndexOf    ("",0,0)         ), L<S,S,I,I,I>((obj,p0,p1,p2) => p0.Length == 0 && obj.Length > p1 ? p1 : (CharIndex(p0, Left(obj, p2), p1)     ?? 0) - 1) },
				{ M(() => "".IndexOf    (' ')            ), L<S,C,I>    ((obj,p0)       =>                                          (CharIndex(p0, obj)                   ?? 0) - 1) },
				{ M(() => "".IndexOf    (' ',0)          ), L<S,C,I,I>  ((obj,p0,p1)    =>                                          (CharIndex(p0, obj,           p1 + 1) ?? 0) - 1) },
				{ M(() => "".IndexOf    (' ',0,0)        ), L<S,C,I,I,I>((obj,p0,p1,p2) =>                                          (CharIndex(p0, Left(obj, p2), p1)     ?? 0) - 1) },
				{ M(() => "".LastIndexOf("")             ), L<S,S,I>    ((obj,p0)       => p0.Length == 0 ? obj.Length - 1 : (CharIndex(p0, obj)                        ?? 0) == 0 ? -1 : obj.Length - (CharIndex(Reverse(p0), Reverse(obj))                                ?? 0) - p0.Length + 1) },
				{ M(() => "".LastIndexOf("",0)           ), L<S,S,I,I>  ((obj,p0,p1)    => p0.Length == 0 ? p1             : (CharIndex(p0, obj,                p1 + 1) ?? 0) == 0 ? -1 : obj.Length - (CharIndex(Reverse(p0), Reverse(obj.Substring(p1, obj.Length - p1))) ?? 0) - p0.Length + 1) },
				{ M(() => "".LastIndexOf("",0,0)         ), L<S,S,I,I,I>((obj,p0,p1,p2) => p0.Length == 0 ? p1             : (CharIndex(p0, Left(obj, p1 + p2), p1 + 1) ?? 0) == 0 ? -1 :    p1 + p2 - (CharIndex(Reverse(p0), Reverse(obj.Substring(p1, p2)))              ?? 0) - p0.Length + 1) },
				{ M(() => "".LastIndexOf(' ')            ), L<S,C,I>    ((obj,p0)       => (CharIndex(p0, obj)         ?? 0)                    == 0 ? -1 : Length(obj) - (CharIndex(p0, Reverse(obj))                                ?? 0)) },
				{ M(() => "".LastIndexOf(' ',0)          ), L<S,C,I,I>  ((obj,p0,p1)    => (CharIndex(p0, obj, p1 + 1) ?? 0)                    == 0 ? -1 : Length(obj) - (CharIndex(p0, Reverse(obj.Substring(p1, obj.Length - p1))) ?? 0)) },
				{ M(() => "".LastIndexOf(' ',0,0)        ), L<S,C,I,I,I>((obj,p0,p1,p2) => (CharIndex(p0, Left(obj, p1 + p2), p1 + 1) ?? 0) == 0 ? -1 : p1 + p2         - (CharIndex(p0, Reverse(obj.Substring(p1, p2)))              ?? 0)) },
				{ M(() => "".Insert     (0,"")           ), L<S,I,S,S>  ((obj,p0,p1)    => obj.Length == p0 ? obj + p1 : Stuff(obj, p0 + 1, 0, p1)) },
				{ M(() => "".Remove     (0)              ), L<S,I,S>    ((obj,p0)       => Left(obj, p0)) },
				{ M(() => "".Remove     (0,0)            ), L<S,I,I,S>  ((obj,p0,p1)    => Stuff(obj, p0 + 1, p1, "")) },
				{ M(() => "".PadLeft    (0)              ), L<S,I,S>    ((obj,p0)       => PadLeft(obj, p0, ' ')) },
				{ M(() => "".PadLeft    (0,' ')          ), L<S,I,C,S>  ((obj,p0,p1)    => PadLeft(obj, p0, p1)) },
				{ M(() => "".PadRight   (0)              ), L<S,I,S>    ((obj,p0)       => PadRight(obj, p0, ' ')) },
				{ M(() => "".PadRight   (0,' ')          ), L<S,I,C,S>  ((obj,p0,p1)    => PadRight(obj, p0, p1)) },
				{ M(() => "".Replace    ("","")          ), L<S,S,S,S>  ((obj,p0,p1)    => Replace(obj, p0, p1)) },
				{ M(() => "".Replace    (' ',' ')        ), L<S,C,C,S>  ((obj,p0,p1)    => Replace(obj, p0, p1)) },
				{ M(() => "".Trim       ()               ), L<S,S>      ( obj           => Trim(obj)) },
				{ M(() => "".TrimEnd    ()               ), L<S,S>      ( obj           => TrimRight(obj)) },
				{ M(() => "".TrimStart  ()               ), L<S,S>      ( obj           => TrimLeft(obj)) },
				{ M(() => "".ToLower    ()               ), L<S,S>      ( obj           => Lower(obj)) },
				{ M(() => "".ToUpper    ()               ), L<S,S>      ( obj           => Upper(obj)) },
				{ M(() => "".CompareTo  ("")             ), L<S,S,I>    ((obj,p0)       => ConvertToCaseCompareTo(obj, p0) ) },
				{ M(() => "".CompareTo  (1)              ), L<S,O,I>    ((obj,p0)       => ConvertToCaseCompareTo(obj, p0.ToString()) ) },
				{ M(() => string.IsNullOrEmpty("")       ), L<S,B>      (     p0        => p0 == null || p0.Length == 0 ) },

				{ M(() => AltStuff      ("",0,0,"")),       L<S,I,I,S,S>((p0, p1,p2,p3) => Left(p0, p1 - 1) + p3 + Right(p0, p0.Length - (p1 + p2 - 1))) },

				#endregion

				#region DateTime

				{ M(() => GetDate()                      ), L<D>        (()             => CurrentTimestamp2 ) },
				{ M(() => DateTime.Now                   ), L<D>        (()             => CurrentTimestamp2 ) },
				{ M(() => DateTime.Now.Year              ), L<D,I>      (obj            => DatePart(DateParts.Year,        obj)     ) },
				{ M(() => DateTime.Now.Month             ), L<D,I>      (obj            => DatePart(DateParts.Month,       obj)     ) },
				{ M(() => DateTime.Now.DayOfYear         ), L<D,I>      (obj            => DatePart(DateParts.DayOfYear,   obj)     ) },
				{ M(() => DateTime.Now.Day               ), L<D,I>      (obj            => DatePart(DateParts.Day,         obj)     ) },
				{ M(() => DateTime.Now.DayOfWeek         ), L<D,I>      (obj            => DatePart(DateParts.WeekDay,     obj) - 1 ) },
				{ M(() => DateTime.Now.Hour              ), L<D,I>      (obj            => DatePart(DateParts.Hour,        obj)     ) },
				{ M(() => DateTime.Now.Minute            ), L<D,I>      (obj            => DatePart(DateParts.Minute,      obj)     ) },
				{ M(() => DateTime.Now.Second            ), L<D,I>      (obj            => DatePart(DateParts.Second,      obj)     ) },
				{ M(() => DateTime.Now.Millisecond       ), L<D,I>      (obj            => DatePart(DateParts.Millisecond, obj)     ) },
				{ M(() => DateTime.Now.Date              ), L<D,D>      (obj            => Convert2(Date,                  obj)     ) },
				{ M(() => DateTime.Now.TimeOfDay         ), L<D,T>      (obj            => DateToTime(Convert2(Time,       obj))    ) },
				{ M(() => DateTime.Now.AddYears       (0)), L<D,I,D>    ((obj,p0)       => DateAdd (DateParts.Year,        p0, obj) ) },
				{ M(() => DateTime.Now.AddMonths      (0)), L<D,I,D>    ((obj,p0)       => DateAdd (DateParts.Month,       p0, obj) ) },
				{ M(() => DateTime.Now.AddDays        (0)), L<D,F,D>    ((obj,p0)       => DateAdd (DateParts.Day,         p0, obj) ) },
				{ M(() => DateTime.Now.AddHours       (0)), L<D,F,D>    ((obj,p0)       => DateAdd (DateParts.Hour,        p0, obj) ) },
				{ M(() => DateTime.Now.AddMinutes     (0)), L<D,F,D>    ((obj,p0)       => DateAdd (DateParts.Minute,      p0, obj) ) },
				{ M(() => DateTime.Now.AddSeconds     (0)), L<D,F,D>    ((obj,p0)       => DateAdd (DateParts.Second,      p0, obj) ) },
				{ M(() => DateTime.Now.AddMilliseconds(0)), L<D,F,D>    ((obj,p0)       => DateAdd (DateParts.Millisecond, p0, obj) ) },

				#endregion

				#region Parse

				{ M(() =>        Boolean. Parse("")), L<String,Boolean> (p0 => ConvertTo<Boolean>. From(p0) ) },
				{ M(() =>        Byte.    Parse("")), L<String,Byte>    (p0 => ConvertTo<Byte>.    From(p0) ) },
				{ M(() => System.Char.    Parse("")), L<String,Char>    (p0 => ConvertTo<Char>.    From(p0) ) },
				{ M(() =>        DateTime.Parse("")), L<String,DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Decimal. Parse("")), L<String,Decimal> (p0 => ConvertTo<Decimal>. From(p0) ) },
				{ M(() =>        Double.  Parse("")), L<String,Double>  (p0 => ConvertTo<Double>.  From(p0) ) },
				{ M(() =>        Int16.   Parse("")), L<String,Int16>   (p0 => ConvertTo<Int16>.   From(p0) ) },
				{ M(() =>        Int32.   Parse("")), L<String,Int32>   (p0 => ConvertTo<Int32>.   From(p0) ) },
				{ M(() =>        Int64.   Parse("")), L<String,Int64>   (p0 => ConvertTo<Int64>.   From(p0) ) },
				{ M(() =>        SByte.   Parse("")), L<String,SByte>   (p0 => ConvertTo<SByte>.   From(p0) ) },
				{ M(() =>        Single.  Parse("")), L<String,Single>  (p0 => ConvertTo<Single>.  From(p0) ) },
				{ M(() =>        UInt16.  Parse("")), L<String,UInt16>  (p0 => ConvertTo<UInt16>.  From(p0) ) },
				{ M(() =>        UInt32.  Parse("")), L<String,UInt32>  (p0 => ConvertTo<UInt32>.  From(p0) ) },
				{ M(() =>        UInt64.  Parse("")), L<String,UInt64>  (p0 => ConvertTo<UInt64>.  From(p0) ) },

				#endregion

				#region Convert

				#region ToInt64

				{ M(() => System.Convert.ToInt64((Boolean)true)), L<Boolean, Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((Byte)    0)  ), L<Byte,    Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((Char)   '0') ), L<Char,    Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64(DateTime.Now) ), L<DateTime,Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((Decimal) 0)  ), L<Decimal, Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((Double)  0)  ), L<Double,  Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((Int16)   0)  ), L<Int16,   Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((Int32)   0)  ), L<Int32,   Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((Int64)   0)  ), L<Int64,   Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((Object)  0)  ), L<Object,  Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((SByte)   0)  ), L<SByte,   Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((Single)  0)  ), L<Single,  Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((String) "0") ), L<String,  Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((UInt16)  0)  ), L<UInt16,  Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((UInt32)  0)  ), L<UInt32,  Int64>(p0 => ConvertTo<Int64>.From(p0) ) },
				{ M(() => System.Convert.ToInt64((UInt64)  0)  ), L<UInt64,  Int64>(p0 => ConvertTo<Int64>.From(p0) ) },

				#endregion

				#region ToInt32

				{ M(() => System.Convert.ToInt32((Boolean)true)), L<Boolean, Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((Byte)    0)  ), L<Byte,    Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((Char)   '0') ), L<Char,    Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32(DateTime.Now) ), L<DateTime,Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((Decimal) 0)  ), L<Decimal, Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((Double)  0)  ), L<Double,  Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((Int16)   0)  ), L<Int16,   Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((Int32)   0)  ), L<Int32,   Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((Int64)   0)  ), L<Int64,   Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((Object)  0)  ), L<Object,  Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((SByte)   0)  ), L<SByte,   Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((Single)  0)  ), L<Single,  Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((String) "0") ), L<String,  Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((UInt16)  0)  ), L<UInt16,  Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((UInt32)  0)  ), L<UInt32,  Int32>(p0 => ConvertTo<Int32>.From(p0) ) },
				{ M(() => System.Convert.ToInt32((UInt64)  0)  ), L<UInt64,  Int32>(p0 => ConvertTo<Int32>.From(p0) ) },

				#endregion

				#region ToInt16

				{ M(() => System.Convert.ToInt16((Boolean)true)), L<Boolean, Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((Byte)    0)  ), L<Byte,    Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((Char)   '0') ), L<Char,    Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16(DateTime.Now) ), L<DateTime,Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((Decimal) 0)  ), L<Decimal, Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((Double)  0)  ), L<Double,  Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((Int16)   0)  ), L<Int16,   Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((Int32)   0)  ), L<Int32,   Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((Int64)   0)  ), L<Int64,   Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((Object)  0)  ), L<Object,  Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((SByte)   0)  ), L<SByte,   Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((Single)  0)  ), L<Single,  Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((String) "0") ), L<String,  Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((UInt16)  0)  ), L<UInt16,  Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((UInt32)  0)  ), L<UInt32,  Int16>(p0 => ConvertTo<Int16>.From(p0) ) },
				{ M(() => System.Convert.ToInt16((UInt64)  0)  ), L<UInt64,  Int16>(p0 => ConvertTo<Int16>.From(p0) ) },

				#endregion

				#region ToByte

				{ M(() => System.Convert.ToByte((Boolean)true)), L<Boolean, Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((Byte)    0)  ), L<Byte,    Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((Char)   '0') ), L<Char,    Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte(DateTime.Now) ), L<DateTime,Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((Decimal) 0)  ), L<Decimal, Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((Double)  0)  ), L<Double,  Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((Int16)   0)  ), L<Int16,   Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((Int32)   0)  ), L<Int32,   Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((Int64)   0)  ), L<Int64,   Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((Object)  0)  ), L<Object,  Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((SByte)   0)  ), L<SByte,   Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((Single)  0)  ), L<Single,  Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((String) "0") ), L<String,  Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((UInt16)  0)  ), L<UInt16,  Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((UInt32)  0)  ), L<UInt32,  Byte>(p0 => ConvertTo<Byte>.From(p0) ) },
				{ M(() => System.Convert.ToByte((UInt64)  0)  ), L<UInt64,  Byte>(p0 => ConvertTo<Byte>.From(p0) ) },

				#endregion

				#region ToDecimal

				{ M(() => System.Convert.ToDecimal((Boolean)true)), L<Boolean, Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((Byte)    0)  ), L<Byte,    Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((Char)   '0') ), L<Char,    Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal(DateTime.Now) ), L<DateTime,Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((Decimal) 0)  ), L<Decimal, Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((Double)  0)  ), L<Double,  Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((Int16)   0)  ), L<Int16,   Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((Int32)   0)  ), L<Int32,   Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((Int64)   0)  ), L<Int64,   Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((Object)  0)  ), L<Object,  Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((SByte)   0)  ), L<SByte,   Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((Single)  0)  ), L<Single,  Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((String) "0") ), L<String,  Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((UInt16)  0)  ), L<UInt16,  Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((UInt32)  0)  ), L<UInt32,  Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },
				{ M(() => System.Convert.ToDecimal((UInt64)  0)  ), L<UInt64,  Decimal>(p0 => ConvertTo<Decimal>.From(p0) ) },

				#endregion

				#region ToDouble

				{ M(() => System.Convert.ToDouble((Boolean)true)), L<Boolean, Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((Byte)    0)  ), L<Byte,    Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((Char)   '0') ), L<Char,    Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble(DateTime.Now) ), L<DateTime,Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((Decimal) 0)  ), L<Decimal, Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((Double)  0)  ), L<Double,  Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((Int16)   0)  ), L<Int16,   Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((Int32)   0)  ), L<Int32,   Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((Int64)   0)  ), L<Int64,   Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((Object)  0)  ), L<Object,  Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((SByte)   0)  ), L<SByte,   Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((Single)  0)  ), L<Single,  Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((String) "0") ), L<String,  Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((UInt16)  0)  ), L<UInt16,  Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((UInt32)  0)  ), L<UInt32,  Double>(p0 => ConvertTo<Double>.From(p0) ) },
				{ M(() => System.Convert.ToDouble((UInt64)  0)  ), L<UInt64,  Double>(p0 => ConvertTo<Double>.From(p0) ) },

				#endregion

				#region ToSingle

				{ M(() => System.Convert.ToSingle((Boolean)true)), L<Boolean, Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((Byte)    0)  ), L<Byte,    Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((Char)   '0') ), L<Char,    Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle(DateTime.Now) ), L<DateTime,Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((Decimal) 0)  ), L<Decimal, Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((Double)  0)  ), L<Double,  Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((Int16)   0)  ), L<Int16,   Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((Int32)   0)  ), L<Int32,   Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((Int64)   0)  ), L<Int64,   Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((Object)  0)  ), L<Object,  Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((SByte)   0)  ), L<SByte,   Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((Single)  0)  ), L<Single,  Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((String) "0") ), L<String,  Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((UInt16)  0)  ), L<UInt16,  Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((UInt32)  0)  ), L<UInt32,  Single>(p0 => ConvertTo<Single>.From(p0) ) },
				{ M(() => System.Convert.ToSingle((UInt64)  0)  ), L<UInt64,  Single>(p0 => ConvertTo<Single>.From(p0) ) },

				#endregion

				#region ToDateTime

				{ M(() => System.Convert.ToDateTime((Boolean)true)), L<Boolean, DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((Byte)    0)  ), L<Byte,    DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((Char)   '0') ), L<Char,    DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime(DateTime.Now) ), L<DateTime,DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((Decimal) 0)  ), L<Decimal, DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((Double)  0)  ), L<Double,  DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((Int16)   0)  ), L<Int16,   DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((Int32)   0)  ), L<Int32,   DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((Int64)   0)  ), L<Int64,   DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((Object)  0)  ), L<Object,  DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((SByte)   0)  ), L<SByte,   DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((Single)  0)  ), L<Single,  DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((String) "0") ), L<String,  DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((UInt16)  0)  ), L<UInt16,  DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((UInt32)  0)  ), L<UInt32,  DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },
				{ M(() => System.Convert.ToDateTime((UInt64)  0)  ), L<UInt64,  DateTime>(p0 => ConvertTo<DateTime>.From(p0) ) },

				#endregion

				#endregion
			}},

			#region MsSql2008

			{ "MsSql2008", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => PadRight("",0,' ')), L<S,I,C,S>((p0,p1,p2) => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
				{ M(() => PadLeft ("",0,' ')), L<S,I,C,S>((p0,p1,p2) => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
				{ M(() => Trim    ("")      ), L<S,S>    ( p0        => TrimLeft(TrimRight(p0))) },
			}},

			#endregion

			#region MsSql2005

			{ "MsSql2005", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => PadRight("",0,' ')), L<S,I,C,S>((p0,p1,p2) => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
				{ M(() => PadLeft ("",0,' ')), L<S,I,C,S>((p0,p1,p2) => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
				{ M(() => Trim    ("")      ), L<S,S>    ( p0        => TrimLeft(TrimRight(p0))) },
			}},

			#endregion

			#region SqlCe

			{ "SqlCe", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Left    ("",0)    ), L<S,I,S>  ((p0,p1)    => Substring(p0, 1, p1)) },
				{ M(() => Right   ("",0)    ), L<S,I,S>  ((p0,p1)    => Substring(p0, p0.Length - p1 + 1, p1)) },
				{ M(() => PadRight("",0,' ')), L<S,I,C,S>((p0,p1,p2) => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
				{ M(() => PadLeft ("",0,' ')), L<S,I,C,S>((p0,p1,p2) => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
				{ M(() => Trim    ("")      ), L<S,S>    ( p0        => TrimLeft(TrimRight(p0))) },
			}},

			#endregion

			#region DB2

			{ "DB2", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Space   (0)        ), L<I,S>      ( p0           => Convert(VarChar(1000), Replicate(" ", p0))) },
				{ M(() => Stuff   ("",0,0,"")), L<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
				{ M(() => PadRight("",0,' ') ), L<S,I,C,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : p0 + VarChar(Replicate(p2, p1 - p0.Length), 1000)) },
				{ M(() => PadLeft ("",0,' ') ), L<S,I,C,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : VarChar(Replicate(p2, p1 - p0.Length), 1000) + p0) },
			}},

			#endregion

			#region Informix

			{ "Informix", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Left ("",0)     ), L<S,I,S>    ((p0,p1)       => Substring(p0, 1, p1)) },
				{ M(() => Right("",0)     ), L<S,I,S>    ((p0,p1)       => Substring(p0, p0.Length - p1 + 1, p1)) },
				{ M(() => Stuff("",0,0,"")), L<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
				{ M(() => Space(0)        ), L<I,S>      ( p0           => PadRight(" ", p0, ' ')) },
			}},

			#endregion

			#region Oracle

			{ "Oracle", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Left ("",0)     ), L<S,I,S>    ((p0,p1)       => Substring(p0, 1, p1)) },
				{ M(() => Right("",0)     ), L<S,I,S>    ((p0,p1)       => Substring(p0, p0.Length - p1 + 1, p1)) },
				{ M(() => Stuff("",0,0,"")), L<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
				{ M(() => Space(0)        ), L<I,S>      ( p0           => PadRight(" ", p0, ' ')) },
			}},

			#endregion

			#region Firebird

			{ "Firebird", new Dictionary<MemberInfo,LambdaExpression> {
				{ M<S>(_ => Space(0         )), L<I,S>      ( p0           => PadRight(" ", p0, ' ')) },
				{ M<S>(s => Stuff(s, 0, 0, s)), L<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
			}},

			#endregion

			#region MySql

			{ "MySql", new Dictionary<MemberInfo,LambdaExpression> {
				{ M<S>(s => Stuff(s, 0, 0, s)), L<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
			}},

			#endregion

			#region PostgreSQL

			{ "PostgreSQL", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Left ("",0)     ), L<S,I,S>    ((p0,p1)       => Substring(p0, 1, p1)) },
				{ M(() => Right("",0)     ), L<S,I,S>    ((p0,p1)       => Substring(p0, p0.Length - p1 + 1, p1)) },
				{ M(() => Stuff("",0,0,"")), L<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
				{ M(() => Space(0)        ), L<I,S>      ( p0           => Replicate(" ", p0)) },
			}},

			#endregion

			#region SQLite

			{ "SQLite", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Stuff   ("",0,0,"")), L<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
				{ M(() => PadRight("",0,' ') ), L<S,I,C,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
				{ M(() => PadLeft ("",0,' ') ), L<S,I,C,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
			}},

			#endregion

			#region Sybase

			{ "Sybase", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => PadRight("",0,' ')), L<S,I,C,S>((p0,p1,p2) => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
				{ M(() => PadLeft ("",0,' ')), L<S,I,C,S>((p0,p1,p2) => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
				{ M(() => Trim    ("")      ), L<S,S>    ( p0        => TrimLeft(TrimRight(p0))) },
			}},

			#endregion

			#region Access

			{ "Access", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Stuff   ("",0,0,"")), L<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
				{ M(() => PadRight("",0,' ') ), L<S,I,C,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
				{ M(() => PadLeft ("",0,' ') ), L<S,I,C,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
			}},

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
		static string AltStuff(string str, int startLocation, int length, string value)
		{
			return Stuff(str, startLocation, length, value);
		}

		// DB2
		//
		[SqlFunction]
		static string VarChar(object obj, int size)
		{
			return obj.ToString();
		}

#pragma warning disable 3019

		// DB2, PostgreSQL, Access, MS SQL, SqlCe
		//
		[CLSCompliant(false)]
		[SqlFunction]
		[SqlFunction("DB2",        "Repeat")]
		[SqlFunction("PostgreSQL", "Repeat")]
		[SqlFunction("Access",     "String", 1, 0)]
		static string Replicate(string str, int count)
		{
			if (str == null)
				return null;

			var sb = new StringBuilder(str.Length * count);

			for (var i = 0; i < count; i++)
				sb.Append(str);

			return sb.ToString();
		}

		[CLSCompliant(false)]
		[SqlFunction]
		[SqlFunction("DB2",        "Repeat")]
		[SqlFunction("PostgreSQL", "Repeat")]
		[SqlFunction("Access",     "String", 1, 0)]
		static string Replicate(char ch, int count)
		{
			var sb = new StringBuilder(count);

			for (var i = 0; i < count; i++)
				sb.Append(ch);

			return sb.ToString();
		}
	}
}
