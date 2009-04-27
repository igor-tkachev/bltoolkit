using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using BLToolkit.Data.Sql;
using BLToolkit.Data.Sql.SqlProvider;

namespace BLToolkit.Data.Linq
{
	using IExpr = ISqlExpression;

	class Members : ReflectionHelper
	{
		public static MemberMaker GetMember(MemberInfo mi)
		{
			MemberMaker maker;
			return _members.TryGetValue(mi, out maker) ? maker : null;
		}

		static MemberInfo Function<T1>(Expression<Func<T1,object>> func)
		{
			var ex = func.Body;

			if (ex is UnaryExpression)
				ex = ((UnaryExpression)func.Body).Operand;

			return ex is MemberExpression?
				((MemberExpression)    ex).Member:
				((MethodCallExpression)ex).Method;
		}

		static ISqlExpression MakeFunc(ISqlProvider sqlProvider, string name, params ISqlExpression[] parameters)
		{
			return sqlProvider.ConvertExpression(new SqlFunction(name, parameters));
		}

		public delegate ISqlExpression MemberMaker (ISqlProvider sqlProvider, params ISqlExpression[] exprs);

		static IExpr Length   (ISqlProvider p, IExpr s)                   { return MakeFunc(p, "Length",    s      ); }
		static IExpr CharIndex(ISqlProvider p, IExpr s, IExpr a)          { return MakeFunc(p, "CharIndex", a, s   ); }
		static IExpr CharIndex(ISqlProvider p, IExpr s, IExpr a, IExpr b) { return MakeFunc(p, "CharIndex", a, s, b); }
		static IExpr Substring(ISqlProvider p, IExpr s, IExpr a, IExpr b) { return MakeFunc(p, "Substring", s, a, b); }
		static IExpr Left     (ISqlProvider p, IExpr s, IExpr a)          { return MakeFunc(p, "Left",      s, a   ); }
		static IExpr Right    (ISqlProvider p, IExpr s, IExpr a)          { return MakeFunc(p, "Right",     s, a   ); }
		static IExpr Reverse  (ISqlProvider p, IExpr s)                   { return MakeFunc(p, "Reverse",   s      ); }

		static IExpr IndexOf(ISqlProvider p, IExpr s, IExpr a)
		{
			return MakeFunc(p, "CASE",
				Condition .Expr(Length(p, a)). Equal .Value(0).ToExpr(),   Value(0),
				Dec(p, CharIndex(p, s, a)));
		}

		static IExpr IndexOf(ISqlProvider p, IExpr s, IExpr a, IExpr b)
		{
			return MakeFunc(p, "CASE",
				Condition .Expr(Length(p, a)). Equal .Value(0). And .Expr(Length(p, s)). GreaterOrEqual .Expr(Inc(p, b)).ToExpr(),   b,
				Dec(p, CharIndex(p, s, a, Inc(p, b))));
		}

		static IExpr IndexOf(ISqlProvider p, IExpr s, IExpr a, IExpr b, IExpr c)
		{
			return MakeFunc(p, "CASE",
				Condition .Expr(Length(p, a)). Equal .Value(0). And .Expr(Length(p, s)). GreaterOrEqual .Expr(Inc(p, b)).ToExpr(),   b,
				Dec(p, CharIndex(p, Left(p, s, Add(p, b, c)), a, Inc(p, b))));
		}

		static IExpr LastIndexOf(ISqlProvider p, IExpr s, IExpr a)
		{
			return MakeFunc(p, "CASE",
				Condition .Expr(Length(p, a)).       Equal .Value(0).ToExpr(),   Dec(p, Length(p, s)),
				Condition .Expr(CharIndex(p, s, a)). Equal .Value(0).ToExpr(),   Value(-1),
				Inc(p, Sub(p, Length(p, s), Add(p, CharIndex(p, Reverse(p, s), Reverse(p, a)), Length(p, a)))));
		}

		static IExpr LastIndexOf(ISqlProvider p, IExpr s, IExpr a, IExpr b)
		{
			return MakeFunc(p, "CASE",
				Condition .Expr(Length(p, a)). Equal .Value(0). And .Expr(Length(p, s)). GreaterOrEqual .Expr(Inc(p, b)).ToExpr(), b,
				Condition .Expr(CharIndex(p, Left(p, s, Inc(p, b)), a)). Equal .Value(0).ToExpr(), Value(-1),
				Inc(p, Sub(p, Length(p, s), Add(p, CharIndex(p, Reverse(p, Left(p, s, Inc(p, b))), Reverse(p, a)), Length(p, a)))));
		}

		static IExpr LastIndexOf(ISqlProvider p, IExpr s, IExpr a, IExpr b, IExpr c)
		{
			return MakeFunc(p, "CASE",
				Condition .Expr(Length(p, a)). Equal .Value(0). And .Expr(Length(p, s)). GreaterOrEqual .Expr(Inc(p, b)).ToExpr(), b,
				Condition
					.Expr(CharIndex(p, Left(p, s, Inc(p, b)), a)). Equal .Value(0). Or
					.Expr(Inc(p, Sub(p, Length(p, s), Add(p, CharIndex(p, Reverse(p, Left(p, s, Inc(p, b))), Reverse(p, a)), Length(p, a))))). LessOrEqual .Expr(Sub(p, b, c)).ToExpr(),   Value(-1),
					      Inc(p, Sub(p, Length(p, s), Add(p, CharIndex(p, Reverse(p, Left(p, s, Inc(p, b))), Reverse(p, a)), Length(p, a)))));
		}

		static readonly Dictionary<MemberInfo,MemberMaker> _members = new Dictionary<MemberInfo,MemberMaker>
		{
			{ Function<string>(s => s.Length                ), (s,p) => Length     (s, p[0])                        },
			{ Function<string>(s => s.CharIndex  (""       )), (s,p) => CharIndex  (s, p[0], p[1])                  },
			{ Function<string>(s => s.CharIndex  ("",  0   )), (s,p) => CharIndex  (s, p[0], p[1],      p[2])       },
			{ Function<string>(s => s.IndexOf    (""       )), (s,p) => IndexOf    (s, p[0], p[1])                  },
			{ Function<string>(s => s.IndexOf    ("",  0   )), (s,p) => IndexOf    (s, p[0], p[1],      p[2])       },
			{ Function<string>(s => s.IndexOf    ("",  0, 0)), (s,p) => IndexOf    (s, p[0], p[1],      p[2], p[3]) },
			{ Function<string>(s => s.IndexOf    (' '      )), (s,p) => IndexOf    (s, p[0], p[1])                  },
			{ Function<string>(s => s.IndexOf    (' ', 0   )), (s,p) => IndexOf    (s, p[0], p[1],      p[2])       },
			{ Function<string>(s => s.IndexOf    (' ', 0, 0)), (s,p) => IndexOf    (s, p[0], p[1],      p[2], p[3]) },
			{ Function<string>(s => s.LastIndexOf(""       )), (s,p) => LastIndexOf(s, p[0], p[1])                  },
			{ Function<string>(s => s.LastIndexOf(' '      )), (s,p) => LastIndexOf(s, p[0], p[1])                  },
			{ Function<string>(s => s.LastIndexOf("",  0   )), (s,p) => LastIndexOf(s, p[0], p[1],      p[2])       },
			{ Function<string>(s => s.LastIndexOf("",  0, 0)), (s,p) => LastIndexOf(s, p[0], p[1],      p[2], p[3]) },
			{ Function<string>(s => s.Left       (0        )), (s,p) => Left       (s, p[0], p[1])                  },
			{ Function<string>(s => s.Right      (0        )), (s,p) => Right      (s, p[0], p[1])                  },
			{ Function<string>(s => s.Substring  (0        )), (s,p) => Substring  (s, p[0], Inc(s, p[1]), Sub(s, Length(s, p[0]), p[1])) },
			{ Function<string>(s => s.Substring  (0,   0   )), (s,p) => Substring  (s, p[0], Inc(s, p[1]), p[2])       },
			{ Function<string>(s => s.Reverse    (         )), (s,p) => Reverse    (s, p[0])                        },
		};

		static SqlBuilder.SearchCondition Condition { get { return new SqlBuilder.SearchCondition(); } }
		static ISqlExpression Value(object o) { return new SqlValue(o); }

		static ISqlExpression Add(ISqlProvider p, ISqlExpression expr1, ISqlExpression expr2) { return p.ConvertExpression(new SqlBinaryExpression(expr1, "+", expr2, typeof (int), Precedence.Additive)); }
		static ISqlExpression Inc(ISqlProvider p, ISqlExpression expr)                        { return Add(p, expr, new SqlValue(1)); }
		static ISqlExpression Sub(ISqlProvider p, ISqlExpression expr1, ISqlExpression expr2) { return p.ConvertExpression(new SqlBinaryExpression(expr1, "-", expr2, typeof (int), Precedence.Subtraction)); }
		static ISqlExpression Dec(ISqlProvider p, ISqlExpression expr)                        { return Sub(p, expr, new SqlValue(1)); }
	}
}
