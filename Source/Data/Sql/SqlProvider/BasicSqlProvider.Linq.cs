using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using Common;
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

			{ MI(() => DateTime.Parse("")             ), new F<S,D>      (p0             => Sql.Convert<DateTime,string>(p0)                ) },
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
			return Linq.Sql.Stuff(str, startLocation, length, value);
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
