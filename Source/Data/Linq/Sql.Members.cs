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
				ex = ((UnaryExpression)ex).Operand;

			return
				ex is MemberExpression     ? ((MemberExpression)    ex).Member :
				ex is MethodCallExpression ? ((MethodCallExpression)ex).Method :
				                 (MemberInfo)((NewExpression)       ex).Constructor;
		}

		static MemberInfo M(Expression<Func<object>> func)
		{
			return ReflectionHelper.MemeberInfo(func);
		}

		static LambdaExpression L<TR>                   (Expression<Func<TR>>                   func) { return func; }
		static LambdaExpression L<T1,TR>                (Expression<Func<T1,TR>>                func) { return func; }
		static LambdaExpression L<T1,T2,TR>             (Expression<Func<T1,T2,TR>>             func) { return func; }
		static LambdaExpression L<T1,T2,T3,TR>          (Expression<Func<T1,T2,T3,TR>>          func) { return func; }
		static LambdaExpression L<T1,T2,T3,T4,TR>       (Expression<Func<T1,T2,T3,T4,TR>>       func) { return func; }
		static LambdaExpression L<T1,T2,T3,T4,T5,TR>    (Expression<Func<T1,T2,T3,T4,T5,TR>>    func) { return func; }
		static LambdaExpression L<T1,T2,T3,T4,T5,T6,TR> (Expression<Func<T1,T2,T3,T4,T5,T6,TR>> func) { return func; }

		static public   Dictionary<string,Dictionary<MemberInfo,LambdaExpression>>  Members { get { return _members; } }
		static readonly Dictionary<string,Dictionary<MemberInfo,LambdaExpression>> _members = new Dictionary<string,Dictionary<MemberInfo,LambdaExpression>>
		{
			{ "", new Dictionary<MemberInfo,LambdaExpression> {

				#region String

				{ M(() => "".Length                      ), L<S,I>      ( obj           => Length(obj).Value) },
				{ M(() => "".Substring  (0)              ), L<S,I,S>    ((obj,p0)       => Substring(obj, p0 + 1, obj.Length - p0)) },
				{ M(() => "".Substring  (0,0)            ), L<S,I,I,S>  ((obj,p0,p1)    => Substring(obj, p0 + 1, p1)) },
				{ M(() => "".IndexOf    ("")             ), L<S,S,I>    ((obj,p0)       => p0.Length == 0                    ? 0  : (CharIndex(p0, obj)                  .Value) - 1) },
				{ M(() => "".IndexOf    ("",0)           ), L<S,S,I,I>  ((obj,p0,p1)    => p0.Length == 0 && obj.Length > p1 ? p1 : (CharIndex(p0, obj,           p1 + 1).Value) - 1) },
				{ M(() => "".IndexOf    ("",0,0)         ), L<S,S,I,I,I>((obj,p0,p1,p2) => p0.Length == 0 && obj.Length > p1 ? p1 : (CharIndex(p0, Left(obj, p2), p1)    .Value) - 1) },
				{ M(() => "".IndexOf    (' ')            ), L<S,C,I>    ((obj,p0)       =>                                          (CharIndex(p0, obj)                  .Value) - 1) },
				{ M(() => "".IndexOf    (' ',0)          ), L<S,C,I,I>  ((obj,p0,p1)    =>                                          (CharIndex(p0, obj,           p1 + 1).Value) - 1) },
				{ M(() => "".IndexOf    (' ',0,0)        ), L<S,C,I,I,I>((obj,p0,p1,p2) =>                                          (CharIndex(p0, Left(obj, p2), p1)     ?? 0) - 1) },
				{ M(() => "".LastIndexOf("")             ), L<S,S,I>    ((obj,p0)       => p0.Length == 0 ? obj.Length - 1 : (CharIndex(p0, obj)                       .Value) == 0 ? -1 : obj.Length - (CharIndex(Reverse(p0), Reverse(obj))                               .Value) - p0.Length + 1) },
				{ M(() => "".LastIndexOf("",0)           ), L<S,S,I,I>  ((obj,p0,p1)    => p0.Length == 0 ? p1             : (CharIndex(p0, obj,                p1 + 1).Value) == 0 ? -1 : obj.Length - (CharIndex(Reverse(p0), Reverse(obj.Substring(p1, obj.Length - p1))).Value) - p0.Length + 1) },
				{ M(() => "".LastIndexOf("",0,0)         ), L<S,S,I,I,I>((obj,p0,p1,p2) => p0.Length == 0 ? p1             : (CharIndex(p0, Left(obj, p1 + p2), p1 + 1).Value) == 0 ? -1 :    p1 + p2 - (CharIndex(Reverse(p0), Reverse(obj.Substring(p1, p2)))             .Value) - p0.Length + 1) },
				{ M(() => "".LastIndexOf(' ')            ), L<S,C,I>    ((obj,p0)       => (CharIndex(p0, obj)                       .Value) == 0 ? -1 : obj.Length - (CharIndex(p0, Reverse(obj))                               .Value)) },
				{ M(() => "".LastIndexOf(' ',0)          ), L<S,C,I,I>  ((obj,p0,p1)    => (CharIndex(p0, obj, p1 + 1)               .Value) == 0 ? -1 : obj.Length - (CharIndex(p0, Reverse(obj.Substring(p1, obj.Length - p1))).Value)) },
				{ M(() => "".LastIndexOf(' ',0,0)        ), L<S,C,I,I,I>((obj,p0,p1,p2) => (CharIndex(p0, Left(obj, p1 + p2), p1 + 1).Value) == 0 ? -1 : p1 + p2    - (CharIndex(p0, Reverse(obj.Substring(p1, p2)))             .Value)) },
				{ M(() => "".Insert     (0,"")           ), L<S,I,S,S>  ((obj,p0,p1)    => obj.Length == p0 ? obj + p1 : Stuff(obj, p0 + 1, 0, p1)) },
				{ M(() => "".Remove     (0)              ), L<S,I,S>    ((obj,p0)       => Left     (obj, p0)) },
				{ M(() => "".Remove     (0,0)            ), L<S,I,I,S>  ((obj,p0,p1)    => Stuff    (obj, p0 + 1, p1, "")) },
				{ M(() => "".PadLeft    (0)              ), L<S,I,S>    ((obj,p0)       => PadLeft  (obj, p0, ' ')) },
				{ M(() => "".PadLeft    (0,' ')          ), L<S,I,C,S>  ((obj,p0,p1)    => PadLeft  (obj, p0, p1)) },
				{ M(() => "".PadRight   (0)              ), L<S,I,S>    ((obj,p0)       => PadRight (obj, p0, ' ')) },
				{ M(() => "".PadRight   (0,' ')          ), L<S,I,C,S>  ((obj,p0,p1)    => PadRight (obj, p0, p1)) },
				{ M(() => "".Replace    ("","")          ), L<S,S,S,S>  ((obj,p0,p1)    => Replace  (obj, p0, p1)) },
				{ M(() => "".Replace    (' ',' ')        ), L<S,C,C,S>  ((obj,p0,p1)    => Replace  (obj, p0, p1)) },
				{ M(() => "".Trim       ()               ), L<S,S>      ( obj           => Trim     (obj)) },
				{ M(() => "".TrimEnd    ()               ), L<S,S>      ( obj           => TrimRight(obj)) },
				{ M(() => "".TrimStart  ()               ), L<S,S>      ( obj           => TrimLeft(obj)) },
				{ M(() => "".ToLower    ()               ), L<S,S>      ( obj           => Lower(obj)) },
				{ M(() => "".ToUpper    ()               ), L<S,S>      ( obj           => Upper(obj)) },
				{ M(() => "".CompareTo  ("")             ), L<S,S,I>    ((obj,p0)       => ConvertToCaseCompareTo(obj, p0).Value ) },
				{ M(() => "".CompareTo  (1)              ), L<S,O,I>    ((obj,p0)       => ConvertToCaseCompareTo(obj, p0.ToString()).Value ) },
				{ M(() => string.IsNullOrEmpty("")       ), L<S,B>      (     p0        => p0 == null || p0.Length == 0 ) },

				{ M(() => AltStuff("",0,0,"")), L<S,I?,I?,S,S>((p0, p1,p2,p3) => Left(p0, p1 - 1) + p3 + Right(p0, p0.Length - (p1 + p2 - 1))) },

				#endregion

				#region DateTime

				{ M(() => GetDate()                      ), L<D>      (()       => CurrentTimestamp2 ) },
				{ M(() => DateTime.Now                   ), L<D>      (()       => CurrentTimestamp2 ) },
				{ M(() => DateTime.Now.Year              ), L<D,I>    (obj      => DatePart(DateParts.Year,        obj).Value     ) },
				{ M(() => DateTime.Now.Month             ), L<D,I>    (obj      => DatePart(DateParts.Month,       obj).Value     ) },
				{ M(() => DateTime.Now.DayOfYear         ), L<D,I>    (obj      => DatePart(DateParts.DayOfYear,   obj).Value     ) },
				{ M(() => DateTime.Now.Day               ), L<D,I>    (obj      => DatePart(DateParts.Day,         obj).Value     ) },
				{ M(() => DateTime.Now.DayOfWeek         ), L<D,I>    (obj      => DatePart(DateParts.WeekDay,     obj).Value - 1 ) },
				{ M(() => DateTime.Now.Hour              ), L<D,I>    (obj      => DatePart(DateParts.Hour,        obj).Value     ) },
				{ M(() => DateTime.Now.Minute            ), L<D,I>    (obj      => DatePart(DateParts.Minute,      obj).Value     ) },
				{ M(() => DateTime.Now.Second            ), L<D,I>    (obj      => DatePart(DateParts.Second,      obj).Value     ) },
				{ M(() => DateTime.Now.Millisecond       ), L<D,I>    (obj      => DatePart(DateParts.Millisecond, obj).Value     ) },
				{ M(() => DateTime.Now.Date              ), L<D,D>    (obj      => Convert2(Date,                  obj)     ) },
				{ M(() => DateTime.Now.TimeOfDay         ), L<D,T>    (obj      => DateToTime(Convert2(Time,       obj)).Value    ) },
				{ M(() => DateTime.Now.AddYears       (0)), L<D,I,D>  ((obj,p0) => DateAdd (DateParts.Year,        p0, obj).Value ) },
				{ M(() => DateTime.Now.AddMonths      (0)), L<D,I,D>  ((obj,p0) => DateAdd (DateParts.Month,       p0, obj).Value ) },
				{ M(() => DateTime.Now.AddDays        (0)), L<D,F,D>  ((obj,p0) => DateAdd (DateParts.Day,         p0, obj).Value ) },
				{ M(() => DateTime.Now.AddHours       (0)), L<D,F,D>  ((obj,p0) => DateAdd (DateParts.Hour,        p0, obj).Value ) },
				{ M(() => DateTime.Now.AddMinutes     (0)), L<D,F,D>  ((obj,p0) => DateAdd (DateParts.Minute,      p0, obj).Value ) },
				{ M(() => DateTime.Now.AddSeconds     (0)), L<D,F,D>  ((obj,p0) => DateAdd (DateParts.Second,      p0, obj).Value ) },
				{ M(() => DateTime.Now.AddMilliseconds(0)), L<D,F,D>  ((obj,p0) => DateAdd (DateParts.Millisecond, p0, obj).Value ) },
				{ M(() => new DateTime(0, 0, 0)          ), L<I,I,I,D>((y,m,d)  => MakeDateTime(y, m, d).Value                    ) },

				{ M(() => MakeDateTime(0, 0, 0)          ), L<I?,I?,I?,D?>         ((y,m,d)        => Convert(Date, y.ToString() + "-" + m.ToString() + "-" + d.ToString())) },
				{ M(() => new DateTime(0, 0, 0, 0, 0, 0) ), L<I,I,I,I,I,I,D>       ((y,m,d,h,mm,s) => MakeDateTime(y, m, d, h, mm, s).Value ) },
				{ M(() => MakeDateTime(0, 0, 0, 0, 0, 0) ), L<I?,I?,I?,I?,I?,I?,D?>((y,m,d,h,mm,s) => Convert(DateTime2,
					y.ToString() + "-" + m.ToString() + "-" + d.ToString() + " " +
					h.ToString() + ":" + mm.ToString() + ":" + s.ToString())) },

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

				#region ToString

				{ M(() => ((Boolean)true).ToString()), L<Boolean, String>(p0 => ConvertTo<string>.From(p0) ) },
				{ M(() => ((Byte)    0)  .ToString()), L<Byte,    String>(p0 => ConvertTo<string>.From(p0) ) },
				{ M(() => ((Char)   '0') .ToString()), L<Char,    String>(p0 => ConvertTo<string>.From(p0) ) },
				{ M(() => ((Decimal) 0)  .ToString()), L<Decimal, String>(p0 => ConvertTo<string>.From(p0) ) },
				{ M(() => ((Double)  0)  .ToString()), L<Double,  String>(p0 => ConvertTo<string>.From(p0) ) },
				{ M(() => ((Int16)   0)  .ToString()), L<Int16,   String>(p0 => ConvertTo<string>.From(p0) ) },
				{ M(() => ((Int32)   0)  .ToString()), L<Int32,   String>(p0 => ConvertTo<string>.From(p0) ) },
				{ M(() => ((Int64)   0)  .ToString()), L<Int64,   String>(p0 => ConvertTo<string>.From(p0) ) },
				{ M(() => ((SByte)   0)  .ToString()), L<SByte,   String>(p0 => ConvertTo<string>.From(p0) ) },
				{ M(() => ((Single)  0)  .ToString()), L<Single,  String>(p0 => ConvertTo<string>.From(p0) ) },
				{ M(() => ((String) "0") .ToString()), L<String,  String>(p0 => p0            ) },
				{ M(() => ((UInt16)  0)  .ToString()), L<UInt16,  String>(p0 => ConvertTo<string>.From(p0) ) },
				{ M(() => ((UInt32)  0)  .ToString()), L<UInt32,  String>(p0 => ConvertTo<string>.From(p0) ) },
				{ M(() => ((UInt64)  0)  .ToString()), L<UInt64,  String>(p0 => ConvertTo<string>.From(p0) ) },

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
				{ M(() => System.Convert.ToDateTime(DateTime.Now) ), L<DateTime,DateTime>(p0 => p0                           ) },
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

				#region ToString

				{ M(() => System.Convert.ToString((Boolean)true)), L<Boolean, String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((Byte)    0)  ), L<Byte,    String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((Char)   '0') ), L<Char,    String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString(DateTime.Now) ), L<DateTime,String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((Decimal) 0)  ), L<Decimal, String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((Double)  0)  ), L<Double,  String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((Int16)   0)  ), L<Int16,   String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((Int32)   0)  ), L<Int32,   String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((Int64)   0)  ), L<Int64,   String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((Object)  0)  ), L<Object,  String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((SByte)   0)  ), L<SByte,   String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((Single)  0)  ), L<Single,  String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((String) "0") ), L<String,  String>(p0 => p0                         ) },
				{ M(() => System.Convert.ToString((UInt16)  0)  ), L<UInt16,  String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((UInt32)  0)  ), L<UInt32,  String>(p0 => ConvertTo<String>.From(p0) ) },
				{ M(() => System.Convert.ToString((UInt64)  0)  ), L<UInt64,  String>(p0 => ConvertTo<String>.From(p0) ) },

				#endregion

				#endregion

				#region Math

				{ M(() => Math.Abs    ((Decimal)0)), L<Decimal,Decimal>(p => Abs    (p).Value ) },
				{ M(() => Math.Abs    ((Double) 0)), L<Double, Double> (p => Abs    (p).Value ) },
				{ M(() => Math.Abs    ((Int16)  0)), L<Int16,  Int16>  (p => Abs    (p).Value ) },
				{ M(() => Math.Abs    ((Int32)  0)), L<Int32,  Int32>  (p => Abs    (p).Value ) },
				{ M(() => Math.Abs    ((Int64)  0)), L<Int64,  Int64>  (p => Abs    (p).Value ) },
				{ M(() => Math.Abs    ((SByte)  0)), L<SByte,  SByte>  (p => Abs    (p).Value ) },
				{ M(() => Math.Abs    ((Single) 0)), L<Single, Single> (p => Abs    (p).Value ) },

				{ M(() => Math.Acos   (0)  ), L<F,F>   ( p    => Acos (p)   .Value ) },
				{ M(() => Math.Asin   (0)  ), L<F,F>   ( p    => Asin (p)   .Value ) },
				{ M(() => Math.Atan   (0)  ), L<F,F>   ( p    => Atan (p)   .Value ) },
				{ M(() => Math.Atan2  (0,0)), L<F,F,F> ((x,y) => Atan2(x, y).Value ) },

				{ M(() => Math.Ceiling((Decimal)0)), L<Decimal,Decimal>(p => Ceiling(p).Value ) },
				{ M(() => Math.Ceiling((Double) 0)), L<Double, Double> (p => Ceiling(p).Value ) },

				{ M(() => Math.Cos    (0)  ), L<F,F>   ( p    => Cos  (p).Value ) },
				{ M(() => Math.Sin    (0)  ), L<F,F>   ( p    => Sin  (p).Value ) },

				{ M(() => Math.Floor  ((Decimal)0)), L<Decimal,Decimal>(p => Floor(p).Value ) },
				{ M(() => Math.Floor  ((Double) 0)), L<Double, Double> (p => Floor(p).Value ) },

				#endregion
			}},

			#region MsSql2008

			{ "MsSql2008", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => PadRight("",0,' ')),  L<S,I?,C,S>   ((p0,p1,p2) => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
				{ M(() => PadLeft ("",0,' ')),  L<S,I?,C,S>   ((p0,p1,p2) => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
				{ M(() => Trim    ("")      ),  L<S,S>        ( p0        => TrimLeft(TrimRight(p0))) },
				{ M(() => MakeDateTime(0,0,0)), L<I?,I?,I?,D?>((y,m,d)    => DateAdd(DateParts.Month, (y.Value - 1900) * 12 + m.Value - 1, d.Value - 1)) },
			}},

			#endregion

			#region MsSql2005

			{ "MsSql2005", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => PadRight("",0,' ')),  L<S,I?,C,S>   ((p0,p1,p2) => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
				{ M(() => PadLeft ("",0,' ')),  L<S,I?,C,S>   ((p0,p1,p2) => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
				{ M(() => Trim    ("")      ),  L<S,S>        ( p0        => TrimLeft(TrimRight(p0))) },
				{ M(() => MakeDateTime(0,0,0)), L<I?,I?,I?,D?>((y,m,d)    => DateAdd(DateParts.Month, (y.Value - 1900) * 12 + m.Value - 1, d.Value - 1)) },
			}},

			#endregion

			#region SqlCe

			{ "SqlCe", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Left    ("",0)    ), L<S,I?,S>   ((p0,p1)    => Substring(p0, 1, p1)) },
				{ M(() => Right   ("",0)    ), L<S,I?,S>   ((p0,p1)    => Substring(p0, p0.Length - p1 + 1, p1)) },
				{ M(() => PadRight("",0,' ')), L<S,I?,C?,S>((p0,p1,p2) => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
				{ M(() => PadLeft ("",0,' ')), L<S,I?,C?,S>((p0,p1,p2) => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
				{ M(() => Trim    ("")      ), L<S,S>      ( p0        => TrimLeft(TrimRight(p0))) },
			}},

			#endregion

			#region DB2

			{ "DB2", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Space   (0)        ), L<I?,S>       ( p0           => Convert(VarChar(1000), Replicate(" ", p0))) },
				{ M(() => Stuff   ("",0,0,"")), L<S,I?,I?,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
				{ M(() => PadRight("",0,' ') ), L<S,I?,C?,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : p0 + VarChar(Replicate(p2, p1 - p0.Length), 1000)) },
				{ M(() => PadLeft ("",0,' ') ), L<S,I?,C?,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : VarChar(Replicate(p2, p1 - p0.Length), 1000) + p0) },

				{ M(() => ConvertTo<String>.From((Decimal)0)), L<Decimal,S>(p => TrimLeft(Convert<string,Decimal>(p), '0')) },
				{ M(() => ConvertTo<String>.From(Guid.Empty)), L<Guid,   S>(p => Lower(
					Substring(Hex(p),  7,  2) + Substring(Hex(p),  5, 2) + Substring(Hex(p), 3, 2) + Substring(Hex(p), 1, 2) + "-" +
					Substring(Hex(p), 11,  2) + Substring(Hex(p),  9, 2) + "-" +
					Substring(Hex(p), 15,  2) + Substring(Hex(p), 13, 2) + "-" +
					Substring(Hex(p), 17,  4) + "-" +
					Substring(Hex(p), 21, 12))) },
			}},

			#endregion

			#region Informix

			{ "Informix", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Left ("",0)     ), L<S,I?,S>     ((p0,p1)       => Substring(p0, 1, p1)) },
				{ M(() => Right("",0)     ), L<S,I?,S>     ((p0,p1)       => Substring(p0, p0.Length - p1 + 1, p1)) },
				{ M(() => Stuff("",0,0,"")), L<S,I?,I?,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
				{ M(() => Space(0)        ), L<I?,S>       ( p0           => PadRight(" ", p0, ' ')) },
			}},

			#endregion

			#region Oracle

			{ "Oracle", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Left ("",0)     ), L<S,I?,S>     ((p0,p1)       => Substring(p0, 1, p1)) },
				{ M(() => Right("",0)     ), L<S,I?,S>     ((p0,p1)       => Substring(p0, p0.Length - p1 + 1, p1)) },
				{ M(() => Stuff("",0,0,"")), L<S,I?,I?,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
				{ M(() => Space(0)        ), L<I?,S>       ( p0           => PadRight(" ", p0, ' ')) },

				{ M(() => ConvertTo<String>.From(Guid.Empty)), L<Guid,S>(p => Lower(
					Substring(Convert2(Char(36), p),  7,  2) + Substring(Convert2(Char(36), p),  5, 2) + Substring(Convert2(Char(36), p), 3, 2) + Substring(Convert2(Char(36), p), 1, 2) + "-" +
					Substring(Convert2(Char(36), p), 11,  2) + Substring(Convert2(Char(36), p),  9, 2) + "-" +
					Substring(Convert2(Char(36), p), 15,  2) + Substring(Convert2(Char(36), p), 13, 2) + "-" +
					Substring(Convert2(Char(36), p), 17,  4) + "-" +
					Substring(Convert2(Char(36), p), 21, 12))) },
			}},

			#endregion

			#region Firebird

			{ "Firebird", new Dictionary<MemberInfo,LambdaExpression> {
				{ M<S>(_  => Space(0         )),   L<I?,S>       ( p0           => PadRight(" ", p0, ' ')) },
				{ M<S>(s  => Stuff(s, 0, 0, s)),   L<S,I?,I?,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
			}},

			#endregion

			#region MySql

			{ "MySql", new Dictionary<MemberInfo,LambdaExpression> {
				{ M<S>(s => Stuff(s, 0, 0, s)), L<S,I?,I?,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
			}},

			#endregion

			#region PostgreSQL

			{ "PostgreSQL", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Left ("",0)     ), L<S,I?,S>     ((p0,p1)       => Substring(p0, 1, p1)) },
				{ M(() => Right("",0)     ), L<S,I?,S>     ((p0,p1)       => Substring(p0, p0.Length - p1 + 1, p1)) },
				{ M(() => Stuff("",0,0,"")), L<S,I?,I?,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
				{ M(() => Space(0)        ), L<I?,S>       ( p0           => Replicate(" ", p0)) },
			}},

			#endregion

			#region SQLite

			{ "SQLite", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Stuff   ("",0,0,"")), L<S,I?,I?,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
				{ M(() => PadRight("",0,' ') ), L<S,I?,C?,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
				{ M(() => PadLeft ("",0,' ') ), L<S,I?,C?,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },

				{ M(() => MakeDateTime(0, 0, 0)), L<I?,I?,I?,D?>((y,m,d) => Convert(Date,
					y.ToString() + "-" +
					(m.ToString().Length == 1 ? "0" + m.ToString() : m.ToString()) + "-" +
					(d.ToString().Length == 1 ? "0" + d.ToString() : d.ToString()))) },

				{ M(() => MakeDateTime(0, 0, 0, 0, 0, 0)), L<I?,I?,I?,I?,I?,I?,D?>((y,m,d,h,i,s) => Convert(DateTime2,
					y.ToString() + "-" +
					(m.ToString().Length == 1 ? "0" + m.ToString() : m.ToString()) + "-" +
					(d.ToString().Length == 1 ? "0" + d.ToString() : d.ToString()) + " " +
					(h.ToString().Length == 1 ? "0" + h.ToString() : h.ToString()) + ":" +
					(i.ToString().Length == 1 ? "0" + i.ToString() : i.ToString()) + ":" +
					(s.ToString().Length == 1 ? "0" + s.ToString() : s.ToString()))) },

				{ M(() => ConvertTo<String>.From(Guid.Empty)), L<Guid,S>(p => Lower(
					Substring(Hex(p),  7,  2) + Substring(Hex(p),  5, 2) + Substring(Hex(p), 3, 2) + Substring(Hex(p), 1, 2) + "-" +
					Substring(Hex(p), 11,  2) + Substring(Hex(p),  9, 2) + "-" +
					Substring(Hex(p), 15,  2) + Substring(Hex(p), 13, 2) + "-" +
					Substring(Hex(p), 17,  4) + "-" +
					Substring(Hex(p), 21, 12))) },
			}},

			#endregion

			#region Sybase

			{ "Sybase", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => PadRight("",0,' ')),  L<S,I?,C?,S>((p0,p1,p2) => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
				{ M(() => PadLeft ("",0,' ')),  L<S,I?,C?,S>((p0,p1,p2) => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
				{ M(() => Trim    ("")      ),  L<S,S>      ( p0        => TrimLeft(TrimRight(p0))) },
			}},

			#endregion

			#region Access

			{ "Access", new Dictionary<MemberInfo,LambdaExpression> {
				{ M(() => Stuff   ("",0,0,"")), L<S,I?,I?,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
				{ M(() => PadRight("",0,' ') ), L<S,I?,C?,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
				{ M(() => PadLeft ("",0,' ') ), L<S,I?,C?,S>  ((p0,p1,p2)    => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
				{ M(() => MakeDateTime(0,0,0)), L<I?,I?,I?,D?>((y,m,d)       => MakeDateTime2(y, m, d))                                   },

				{ M(() => ConvertTo<String>.From(Guid.Empty)), L<Guid,S>(p => Lower(Substring(p.ToString(), 2, 36))) },

				{ M(() => Math.Ceiling((Decimal)0)), L<Decimal,Decimal>(p => -Floor(-p).Value ) },
				{ M(() => Math.Ceiling((Double) 0)), L<Double, Double> (p => -Floor(-p).Value ) },

				{ M(() => Math.Floor  ((Decimal)0)), L<Decimal,Decimal>(p => Floor(p).Value ) },
				{ M(() => Math.Floor  ((Double) 0)), L<Double, Double> (p => Floor(p).Value ) },
			}},

			#endregion
		};

		[SqlFunction]
		static int? ConvertToCaseCompareTo(string str, string value)
		{
			return str == null || value == null ? (int?)null : str.CompareTo(value);
		}

		// Access, DB2, Firebird, Informix, MySql, Oracle, PostgreSQL, SQLite
		//
		[SqlFunction]
		static string AltStuff(string str, int? startLocation, int? length, string value)
		{
			return Stuff(str, startLocation, length, value);
		}

		// DB2
		//
		[SqlFunction]
		static string VarChar(object obj, int? size)
		{
			return obj.ToString();
		}

		// DB2
		//
		[SqlFunction]
		static string Hex(Guid? guid)
		{
			return guid == null ? null : guid.ToString();
		}

#pragma warning disable 3019

		// DB2, PostgreSQL, Access, MS SQL, SqlCe
		//
		[CLSCompliant(false)]
		[SqlFunction]
		[SqlFunction("DB2",        "Repeat")]
		[SqlFunction("PostgreSQL", "Repeat")]
		[SqlFunction("Access",     "String", 1, 0)]
		static string Replicate(string str, int? count)
		{
			if (str == null || count == null)
				return null;

			var sb = new StringBuilder(str.Length * count.Value);

			for (var i = 0; i < count; i++)
				sb.Append(str);

			return sb.ToString();
		}

		[CLSCompliant(false)]
		[SqlFunction]
		[SqlFunction("DB2",        "Repeat")]
		[SqlFunction("PostgreSQL", "Repeat")]
		[SqlFunction("Access",     "String", 1, 0)]
		static string Replicate(char? ch, int? count)
		{
			if (ch == null || count == null)
				return null;

			var sb = new StringBuilder(count.Value);

			for (var i = 0; i < count; i++)
				sb.Append(ch);

			return sb.ToString();
		}

		// MSSQL
		//
		[SqlFunction]
		static DateTime? DateAdd(DateParts part, int? number, int? days)
		{
			return days == null ? null : DateAdd(part, number, new DateTime(1900, 1, days.Value + 1));
		}

		// Access
		//
		[SqlFunction("Access", "DateSerial")]
		static DateTime? MakeDateTime2(int? year, int? month, int? day)
		{
			return year == null || month == null || day == null?
				(DateTime?)null :
				new DateTime(year.Value, month.Value, day.Value);
		}
	}
}
