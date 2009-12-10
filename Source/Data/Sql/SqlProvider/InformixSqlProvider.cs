using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

#if FW3
	using Linq;

	using C = Char;
	using S = String;
	using I = Int32;
#endif

	public class InformixSqlProvider : BasicSqlProvider
	{
		public InformixSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override void BuildSelectClause(StringBuilder sb)
		{
			if (SqlQuery.From.Tables.Count == 0)
			{
				AppendIndent(sb).Append("SELECT FIRST 1").AppendLine();
				BuildColumns(sb);
				AppendIndent(sb).Append("FROM SYSTABLES").AppendLine();
			}
			else
				base.BuildSelectClause(sb);
		}

		public override bool IsSubQueryTakeSupported { get { return false; } }

		protected override string FirstFormat { get { return "FIRST {0}"; } }
		protected override string SkipFormat  { get { return "SKIP {0}";  } }

		protected override void BuildLikePredicate(StringBuilder sb, SqlQuery.Predicate.Like predicate)
		{
			if (predicate.IsNot)
				sb.Append("NOT ");

			int precedence = GetPrecedence(predicate);

			BuildExpression(sb, precedence, predicate.Expr1);
			sb.Append(" LIKE ");
			BuildExpression(sb, precedence, predicate.Expr2);

			if (predicate.Escape != null)
			{
				sb.Append(" ESCAPE ");
				BuildExpression(sb, precedence, predicate.Escape);
			}
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "%": return new SqlFunction("Mod",    be.Expr1, be.Expr2);
					case "&": return new SqlFunction("BitAnd", be.Expr1, be.Expr2);
					case "|": return new SqlFunction("BitOr",  be.Expr1, be.Expr2);
					case "^": return new SqlFunction("BitXor", be.Expr1, be.Expr2);
					case "+": return be.Type == typeof(string)? new SqlBinaryExpression(be.Expr1, "||", be.Expr2, be.Type, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction)expr;

				switch (func.Name)
				{
					case "Coalesce" : return new SqlFunction("Nvl", func.Parameters);
					case "Convert"  : return new SqlExpression("Cast({0} as {1})", Precedence.Primary, func.Parameters[1], func.Parameters[0]);
					case "Quarter"  : return Inc(Div(Dec(new SqlFunction("Month", func.Parameters)), 3));
					case "WeekDay"  : return Inc(new SqlFunction("weekDay", func.Parameters));
					case "DayOfYear":
						return
							Inc(Sub<int>(
								new SqlFunction("Mdy",
									new SqlFunction("Month", func.Parameters),
									new SqlFunction("Day",   func.Parameters),
									new SqlFunction("Year",  func.Parameters)),
								new SqlFunction("Mdy",
									new SqlValue(1),
									new SqlValue(1),
									new SqlFunction("Year", func.Parameters))));
					case "Week"     :
						return
							new SqlExpression(
								"((Extend({0}, year to day) - (Mdy(12, 31 - WeekDay(Mdy(1, 1, year(t.DateTimeValue))), Year({0}) - 1) + Interval(1) day to day)) / 7 + Interval(1) day to day)::char(10)::int",
								func.Parameters);
					case "Hour"     :
					case "Minute"   :
					case "Second"   : return new SqlExpression(string.Format("({{0}}::datetime {0} to {0})::char(3)::int", func.Name), func.Parameters);
				}
			}

			return expr;
		}

#if FW3
		protected override Dictionary<MemberInfo,BaseExpressor> GetExpressors() { return _members; }
		static    readonly Dictionary<MemberInfo,BaseExpressor> _members = new Dictionary<MemberInfo,BaseExpressor>
		{
			{ MI(() => Sql.Left ("",0)     ), new F<S,I,S>    ((p0,p1)       => Sql.Substring(p0, 1, p1)) },
			{ MI(() => Sql.Right("",0)     ), new F<S,I,S>    ((p0,p1)       => Sql.Substring(p0, p0.Length - p1 + 1, p1)) },
			{ MI(() => Sql.Stuff("",0,0,"")), new F<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
			{ MI(() => Sql.Space(0)        ), new F<I,S>      ( p0           => Sql.PadRight(" ", p0, ' ')) },
		};
#endif
	}
}
