using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq
{
	using C = Char;
	using S = String;
	using I = Int32;

	class ExtendedFunctions : ReflectionHelper
	{
		public static Expression GetExpr(MemberInfo mi)
		{
			FBase expr;
			return _members.TryGetValue(mi, out expr) ? expr.Expr : null;
		}

		static MemberInfo MI<T>(Expression<Func<T,object>> func)
		{
			var ex = func.Body;

			if (ex is UnaryExpression)
				ex = ((UnaryExpression)func.Body).Operand;

			return ex is MemberExpression?
				((MemberExpression)    ex).Member:
				((MethodCallExpression)ex).Method;
		}

		abstract class FBase
		{
			public Expression Expr;

			protected void Set(LambdaExpression func)
			{
				Expr = func.Body;

				if (Expr is UnaryExpression)
					Expr = ((UnaryExpression)func.Body).Operand;
			}
		}

		class F<TR>             : FBase { public F(Expression<Func<TR>>             func) { Set(func); } }
		class F<T1,TR>          : FBase { public F(Expression<Func<T1,TR>>          func) { Set(func); } }
		class F<T1,T2,TR>       : FBase { public F(Expression<Func<T1,T2,TR>>       func) { Set(func); } }
		class F<T1,T2,T3,TR>    : FBase { public F(Expression<Func<T1,T2,T3,TR>>    func) { Set(func); } }
		class F<T1,T2,T3,T4,TR> : FBase { public F(Expression<Func<T1,T2,T3,T4,TR>> func) { Set(func); } }

		static readonly Dictionary<MemberInfo,FBase> _members = new Dictionary<MemberInfo,FBase>
		{
			{ MI<S>(s => s.Substring  (0        )), new F<S,I,S>    ((obj,p0)       => obj.Substring(p0, obj.Length - p0)) },
			{ MI<S>(s => s.IndexOf    (""       )), new F<S,S,I>    ((obj,p0)       => p0.Length == 0                    ? 0  : (obj.CharIndex(p0)         ?? 0) - 1) },
			{ MI<S>(s => s.IndexOf    ("",  0   )), new F<S,S,I,I>  ((obj,p0,p1)    => p0.Length == 0 && obj.Length > p1 ? p1 : (obj.CharIndex(p0, p1 + 1) ?? 0) - 1) },
			{ MI<S>(s => s.IndexOf    ("",  0, 0)), new F<S,S,I,I,I>((obj,p0,p1,p2) => p0.Length == 0 && obj.Length > p1 ? p1 : (obj.Left(p2).CharIndex(p0, p1) ?? 0) - 1) },
			{ MI<S>(s => s.IndexOf    (' '      )), new F<S,C,I>    ((obj,p0)       => (obj.CharIndex(p0)         ?? 0) - 1) },
			{ MI<S>(s => s.IndexOf    (' ', 0   )), new F<S,C,I,I>  ((obj,p0,p1)    => (obj.CharIndex(p0, p1 + 1) ?? 0) - 1) },
			{ MI<S>(s => s.IndexOf    (' ', 0, 0)), new F<S,C,I,I,I>((obj,p0,p1,p2) => (obj.Left(p2).CharIndex(p0, p1) ?? 0) - 1) },
			{ MI<S>(s => s.LastIndexOf(""       )), new F<S,S,I>    ((obj,p0)       => p0.Length == 0 ? obj.Length - 1 : (obj.CharIndex(p0) ?? 0) == 0 ? -1 : obj.Length - (obj.Reverse().CharIndex(p0.Reverse()) ?? 0) - p0.Length + 1) },
			{ MI<S>(s => s.LastIndexOf("",  0   )), new F<S,S,I,I>  ((obj,p0,p1)    => p0.Length == 0 ? p1 : (obj.CharIndex(p0, p1 + 1) ?? 0) == 0 ? -1 : obj.Length - (obj.Substring(p1, obj.Length - p1).Reverse().CharIndex(p0.Reverse()) ?? 0) - p0.Length + 1) },
			{ MI<S>(s => s.LastIndexOf("",  0, 0)), new F<S,S,I,I,I>((obj,p0,p1,p2) => p0.Length == 0 ? p1 : (obj.Left(p1 + p2).CharIndex(p0, p1 + 1) ?? 0) == 0 ? -1 : p1 + p2 - (obj.Substring(p1, p2).Reverse().CharIndex(p0.Reverse()) ?? 0) - p0.Length + 1) },
			{ MI<S>(s => s.LastIndexOf(' '      )), new F<S,C,I>    ((obj,p0)       => (obj.CharIndex(p0)         ?? 0) == 0 ? -1 : obj.Length - (obj.Reverse().CharIndex(p0) ?? 0)) },
			{ MI<S>(s => s.LastIndexOf(' ', 0   )), new F<S,C,I,I>  ((obj,p0,p1)    => (obj.CharIndex(p0, p1 + 1) ?? 0) == 0 ? -1 : obj.Length - (obj.Substring(p1, obj.Length - p1).Reverse().CharIndex(p0) ?? 0)) },
			{ MI<S>(s => s.LastIndexOf(' ', 0, 0)), new F<S,C,I,I,I>((obj,p0,p1,p2) => (obj.Left(p1 + p2).CharIndex(p0, p1 + 1) ?? 0) == 0 ? -1 : p1 + p2 - (obj.Substring(p1, p2).Reverse().CharIndex(p0) ?? 0)) },
			{ MI<S>(s => s.Insert     (0  , ""  )), new F<S,I,S,S>  ((obj,p0,p1)    => obj.Length == p0 ? obj + p1 : obj.Stuff(p0 + 1, 0, p1)) },
			{ MI<S>(s => s.Remove     (0        )), new F<S,I,S>    ((obj,p0)       => obj.Left(p0)) },
			{ MI<S>(s => s.Remove     (0  , 0   )), new F<S,I,I,S>  ((obj,p0,p1)    => obj.Stuff(p0 + 1, p1, "")) },
		};
	}
}
