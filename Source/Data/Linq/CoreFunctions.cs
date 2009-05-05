using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq
{
	using Sql;
	using Sql.SqlProvider;

	using IExpr = Sql.ISqlExpression;

	class CoreFunctions : ReflectionHelper
	{
		public static MemberMaker GetMember(MemberInfo mi)
		{
			MemberMaker maker;
			return _members.TryGetValue(mi, out maker) ? maker : null;
		}

		static MemberInfo Function<T>(Expression<Func<T,object>> func)
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

		static readonly Dictionary<MemberInfo,MemberMaker> _members = new Dictionary<MemberInfo,MemberMaker>
		{
			{ Function<string>(s => s.Length                ), (s,p) => Length     (s, p[0])                        },
			{ Function<string>(s => s.CharIndex  (""       )), (s,p) => CharIndex  (s, p[0], p[1])                  },
			{ Function<string>(s => s.CharIndex  ("",  0   )), (s,p) => CharIndex  (s, p[0], p[1],      p[2])       },
			{ Function<string>(s => s.CharIndex  (' '      )), (s,p) => CharIndex  (s, p[0], p[1])                  },
			{ Function<string>(s => s.CharIndex  (' ', 0   )), (s,p) => CharIndex  (s, p[0], p[1],      p[2])       },
			{ Function<string>(s => s.Left       (0        )), (s,p) => Left       (s, p[0], p[1])                  },
			{ Function<string>(s => s.Right      (0        )), (s,p) => Right      (s, p[0], p[1])                  },
			//{ Function<string>(s => s.Substring  (0        )), (s,p) => Substring  (s, p[0], Inc(s, p[1]), Sub(s, Length(s, p[0]), p[1])) },
			{ Function<string>(s => s.Substring  (0,   0   )), (s,p) => Substring  (s, p[0], Inc(s, p[1]), p[2])    },
			{ Function<string>(s => s.Reverse    (         )), (s,p) => Reverse    (s, p[0])                        },
		};

		static ISqlExpression Add(ISqlProvider p, ISqlExpression expr1, ISqlExpression expr2) { return p.ConvertExpression(new SqlBinaryExpression(expr1, "+", expr2, typeof (int), Precedence.Additive)); }
		static ISqlExpression Inc(ISqlProvider p, ISqlExpression expr)                        { return Add(p, expr, new SqlValue(1)); }
		static ISqlExpression Sub(ISqlProvider p, ISqlExpression expr1, ISqlExpression expr2) { return p.ConvertExpression(new SqlBinaryExpression(expr1, "-", expr2, typeof (int), Precedence.Subtraction)); }
	}
}
