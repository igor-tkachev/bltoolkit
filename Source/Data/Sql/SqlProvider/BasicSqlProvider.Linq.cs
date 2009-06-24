using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using BLToolkit.Data.Linq;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using C = Char;
	using S = String;
	using I = Int32;
	using O = Object;
	using D = DateTime;

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
			{ MI(() => "".Length              ), new F<S,I>      ( obj           => Linq.Sql.Length(obj)) },
			{ MI(() => "".Substring  (0)      ), new F<S,I,S>    ((obj,p0)       => Linq.Sql.Substring(obj, p0 + 1, obj.Length - p0)) },
			{ MI(() => "".Substring  (0,0)    ), new F<S,I,I,S>  ((obj,p0,p1)    => Linq.Sql.Substring(obj, p0 + 1, p1)) },
			{ MI(() => "".IndexOf    ("")     ), new F<S,S,I>    ((obj,p0)       => p0.Length == 0 ? 0  : (Linq.Sql.CharIndex(p0, obj)         ?? 0) - 1) },
			{ MI(() => "".IndexOf    ("",0)   ), new F<S,S,I,I>  ((obj,p0,p1)    => p0.Length == 0 && obj.Length > p1 ? p1 : (Linq.Sql.CharIndex(p0, obj, p1 + 1) ?? 0) - 1) },
			{ MI(() => "".IndexOf    ("",0,0) ), new F<S,S,I,I,I>((obj,p0,p1,p2) => p0.Length == 0 && obj.Length > p1 ? p1 : (Linq.Sql.CharIndex(p0, Linq.Sql.Left(obj, p2), p1) ?? 0) - 1) },
			{ MI(() => "".IndexOf    (' ')    ), new F<S,C,I>    ((obj,p0)       => (Linq.Sql.CharIndex(p0, obj)         ?? 0) - 1) },
			{ MI(() => "".IndexOf    (' ',0)  ), new F<S,C,I,I>  ((obj,p0,p1)    => (Linq.Sql.CharIndex(p0, obj, p1 + 1) ?? 0) - 1) },
			{ MI(() => "".IndexOf    (' ',0,0)), new F<S,C,I,I,I>((obj,p0,p1,p2) => (Linq.Sql.CharIndex(p0, Linq.Sql.Left(obj, p2), p1) ?? 0) - 1) },
			{ MI(() => "".LastIndexOf("")     ), new F<S,S,I>    ((obj,p0)       => p0.Length == 0 ? obj.Length - 1 : (Linq.Sql.CharIndex(p0, obj) ?? 0) == 0 ? -1 : obj.Length - (Linq.Sql.CharIndex(Linq.Sql.Reverse(p0), Linq.Sql.Reverse(obj)) ?? 0) - p0.Length + 1) },
			{ MI(() => "".LastIndexOf("",0)   ), new F<S,S,I,I>  ((obj,p0,p1)    => p0.Length == 0 ? p1 : (Linq.Sql.CharIndex(p0, obj,                         p1 + 1) ?? 0) == 0 ? -1 : obj.Length - (Linq.Sql.CharIndex(Linq.Sql.Reverse(p0), Linq.Sql.Reverse(obj.Substring(p1, obj.Length - p1))) ?? 0) - p0.Length + 1) },
			{ MI(() => "".LastIndexOf("",0,0) ), new F<S,S,I,I,I>((obj,p0,p1,p2) => p0.Length == 0 ? p1 : (Linq.Sql.CharIndex(p0, Linq.Sql.Left(obj, p1 + p2), p1 + 1) ?? 0) == 0 ? -1 : p1 + p2 - (Linq.Sql.CharIndex(Linq.Sql.Reverse(p0), Linq.Sql.Reverse(obj.Substring(p1, p2))) ?? 0) - p0.Length + 1) },
			{ MI(() => "".LastIndexOf(' ')    ), new F<S,C,I>    ((obj,p0)       => (Linq.Sql.CharIndex(p0, obj)         ?? 0) == 0 ? -1 : Linq.Sql.Length(obj) - (Linq.Sql.CharIndex(p0, Linq.Sql.Reverse(obj)) ?? 0)) },
			{ MI(() => "".LastIndexOf(' ',0)  ), new F<S,C,I,I>  ((obj,p0,p1)    => (Linq.Sql.CharIndex(p0, obj, p1 + 1) ?? 0) == 0 ? -1 : Linq.Sql.Length(obj) - (Linq.Sql.CharIndex(p0, Linq.Sql.Reverse(obj.Substring(p1, obj.Length - p1))) ?? 0)) },
			{ MI(() => "".LastIndexOf(' ',0,0)), new F<S,C,I,I,I>((obj,p0,p1,p2) => (Linq.Sql.CharIndex(p0, Linq.Sql.Left(obj, p1 + p2), p1 + 1) ?? 0) == 0 ? -1 : p1 + p2 - (Linq.Sql.CharIndex(p0, Linq.Sql.Reverse(obj.Substring(p1, p2))) ?? 0)) },
			{ MI(() => "".Insert     (0,"")   ), new F<S,I,S,S>  ((obj,p0,p1)    => obj.Length == p0 ? obj + p1 : Linq.Sql.Stuff(obj, p0 + 1, 0, p1)) },
			{ MI(() => "".Remove     (0)      ), new F<S,I,S>    ((obj,p0)       => Linq.Sql.Left(obj, p0)) },
			{ MI(() => "".Remove     (0,0)    ), new F<S,I,I,S>  ((obj,p0,p1)    => Linq.Sql.Stuff(obj, p0 + 1, p1, "")) },
			{ MI(() => "".PadLeft    (0)      ), new F<S,I,S>    ((obj,p0)       => Linq.Sql.PadLeft(obj, p0, ' ')) },
			{ MI(() => "".PadLeft    (0,' ')  ), new F<S,I,C,S>  ((obj,p0,p1)    => Linq.Sql.PadLeft(obj, p0, p1)) },
			{ MI(() => "".PadRight   (0)      ), new F<S,I,S>    ((obj,p0)       => Linq.Sql.PadRight(obj, p0, ' ')) },
			{ MI(() => "".PadRight   (0,' ')  ), new F<S,I,C,S>  ((obj,p0,p1)    => Linq.Sql.PadRight(obj, p0, p1)) },
			{ MI(() => "".Replace    ("","")  ), new F<S,S,S,S>  ((obj,p0,p1)    => Linq.Sql.Replace(obj, p0, p1)) },
			{ MI(() => "".Replace    (' ',' ')), new F<S,C,C,S>  ((obj,p0,p1)    => Linq.Sql.Replace(obj, p0, p1)) },
			{ MI(() => "".Trim       ()       ), new F<S,S>      ( obj           => Linq.Sql.Trim(obj)) },
			{ MI(() => "".TrimEnd    ()       ), new F<S,S>      ( obj           => Linq.Sql.TrimRight(obj)) },
			{ MI(() => "".TrimStart  ()       ), new F<S,S>      ( obj           => Linq.Sql.TrimLeft(obj)) },
			{ MI(() => "".ToLower    ()       ), new F<S,S>      ( obj           => Linq.Sql.Lower(obj)) },
			{ MI(() => "".ToUpper    ()       ), new F<S,S>      ( obj           => Linq.Sql.Upper(obj)) },
			{ MI(() => "".CompareTo  ("")     ), new F<S,S,I>    ((obj,p0)       => ConvertToCaseCompareTo(obj, p0) ) },
			{ MI(() => "".CompareTo  (1)      ), new F<S,O,I>    ((obj,p0)       => ConvertToCaseCompareTo(obj, p0.ToString()) ) },

			{ MI(() => AltStuff    ("",0,0,"")), new F<S,I,I,S,S>((p0, p1,p2,p3) => Linq.Sql.Left(p0, p1 - 1) + p3 + Linq.Sql.Right(p0, p0.Length - (p1 + p2 - 1))) },

			{ MI(() => Linq.Sql.GetDate()     ), new F<D>        (()             => Linq.Sql.CurrentTimestamp ) },
			{ MI(() => DateTime.Now           ), new F<D>        (()             => Linq.Sql.CurrentTimestamp ) },
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

		// DB2, PostgreSQL, MS SQL, SqlCe
		//
		[SqlFunction]
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
		protected static string Replicate(char ch, int count)
		{
			var sb = new StringBuilder(count);

			for (var i = 0; i < count; i++)
				sb.Append(ch);

			return sb.ToString();
		}
	}
}
