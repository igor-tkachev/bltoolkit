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
		static IExpr Reverse  (ISqlProvider p, IExpr s)                   { return MakeFunc(p, "Reverse",   s      ); }

		static IExpr IndexOf(ISqlProvider p, IExpr s, IExpr a)
		{
			return MakeFunc(p, "CASE",
				Condition .Expr(Length(p, a)). Equal .Value(0).ToExpr(),   Value(0),
				Dec(CharIndex(p, s, a)));
		}

		static IExpr IndexOf(ISqlProvider p, IExpr s, IExpr a, IExpr b)
		{
			return MakeFunc(p, "CASE",
				Condition .Expr(Length(p, a)). Equal .Value(0). And .Expr(Length(p, s)). GreaterOrEqual .Expr(Inc(b)).ToExpr(),   b,
				Dec(CharIndex(p, s, a, Inc(b))));
		}

		static IExpr IndexOf(ISqlProvider p, IExpr s, IExpr a, IExpr b, IExpr c)
		{
			return MakeFunc(p, "CASE",
				Condition .Expr(Length(p, a)). Equal .Value(0). And .Expr(Length(p, s)). GreaterOrEqual .Expr(Inc(b)).ToExpr(),   b,
				Dec(CharIndex(p, Left(p, s, Add(b, c)), a, Inc(b))));
		}

		static IExpr LastIndexOf(ISqlProvider p, IExpr s, IExpr a)
		{
			return MakeFunc(p, "CASE",
				Condition .Expr(Length(p, a)).       Equal .Value(0).ToExpr(),   Dec(Length(p, s)),
				Condition .Expr(CharIndex(p, s, a)). Equal .Value(0).ToExpr(),   Value(-1),
				Inc(Sub(Length(p, s), Add(CharIndex(p, Reverse(p, s), Reverse(p, a)), Length(p, a)))));
		}

		static IExpr LastIndexOf(ISqlProvider p, IExpr s, IExpr a, IExpr b)
		{
			return MakeFunc(p, "CASE",
				Condition .Expr(Length(p, a)). Equal .Value(0). And .Expr(Length(p, s)). GreaterOrEqual .Expr(Inc(b)).ToExpr(), b,
				Condition .Expr(CharIndex(p, Left(p, s, Inc(b)), a)). Equal .Value(0).ToExpr(), Value(-1),
				Inc(Sub(Length(p, s), Add(CharIndex(p, Reverse(p, Left(p, s, Inc(b))), Reverse(p, a)), Length(p, a)))));
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
			{ Function<string>(s => s.LastIndexOf("",  0   )), (s,p) => LastIndexOf(s, p[0], p[1], p[2])            },
			{ Function<string>(s => s.Left       (0        )), (s,p) => Left       (s, p[0], p[1])                  },
			{ Function<string>(s => s.Substring  (0        )), (s,p) => Substring  (s, p[0], Inc(p[1]), Sub(Length(s, p[0]), p[1])) },
			{ Function<string>(s => s.Substring  (0,   0   )), (s,p) => Substring  (s, p[0], Inc(p[1]), p[2])       },
			{ Function<string>(s => s.Reverse    (         )), (s,p) => Reverse    (s, p[0])                        },
		};

		static SqlBuilder.SearchCondition Condition { get { return new SqlBuilder.SearchCondition(); } }
		static ISqlExpression Value(object o) { return new SqlValue(o); }

		static ISqlExpression Inc(ISqlExpression expr)                        { return BasicSqlProvider.Add(expr,  1);     }
		static ISqlExpression Dec(ISqlExpression expr)                        { return BasicSqlProvider.Sub(expr,  1);     }
		static ISqlExpression Add(ISqlExpression expr1, ISqlExpression expr2) { return BasicSqlProvider.Add<int>(expr1, expr2); }
		//static ISqlExpression Add(ISqlExpression expr1, int value)            { return BasicSqlProvider.Add(expr1, value); }
		static ISqlExpression Sub(ISqlExpression expr1, ISqlExpression expr2) { return BasicSqlProvider.Sub<int>(expr1, expr2); }
		//static ISqlExpression Sub(ISqlExpression expr1, int value)            { return BasicSqlProvider.Sub(expr1, value); }
		//static ISqlExpression Mul(ISqlExpression expr1, ISqlExpression expr2) { return BasicSqlProvider.Mul(expr1, expr2); }
		//static ISqlExpression Mul(ISqlExpression expr1, int value)            { return BasicSqlProvider.Mul(expr1, value); }
	}
}
