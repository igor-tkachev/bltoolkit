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

	public class MySqlSqlProvider : BasicSqlProvider
	{
		public MySqlSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override string LimitFormat { get { return "LIMIT {0}"; } }

		public override bool IsNestedJoinParenthesisRequired { get { return true; } }

		protected override void BuildOffsetLimit(StringBuilder sb)
		{
			if (SqlQuery.Select.SkipValue == null)
				base.BuildOffsetLimit(sb);
			else
			{
				AppendIndent(sb).AppendFormat
				(
					SqlQuery.Select.SkipValue != null ? "LIMIT {0},{1}" : "LIMIT {1}",
					BuildExpression(new StringBuilder(), SqlQuery.Select.SkipValue),
					SqlQuery.Select.TakeValue == null?
						long.MaxValue.ToString():
						BuildExpression(new StringBuilder(), SqlQuery.Select.TakeValue).ToString()
				).AppendLine();
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
					case "+":
						if (be.Type == typeof(string))
						{
							if (be.Expr1 is SqlFunction)
							{
								SqlFunction func = (SqlFunction)be.Expr1;

								if (func.Name == "Concat")
								{
									List<ISqlExpression> list = new List<ISqlExpression>(func.Parameters);
									list.Add(be.Expr2);
									return new SqlFunction("Concat", list.ToArray());
								}
							}

							return new SqlFunction("Concat", be.Expr1, be.Expr2);
						}

						break;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Convert" : return new SqlExpression("Cast({0} as {1})", Precedence.Primary, func.Parameters[1], func.Parameters[0]);
				}
			}

			return expr;
		}

#if FW3
		protected override Dictionary<MemberInfo,BaseExpressor> GetExpressors() { return _members; }
		static    readonly Dictionary<MemberInfo,BaseExpressor> _members = new Dictionary<MemberInfo,BaseExpressor>
		{
			{ MI<S>(s => Sql.Stuff(s, 0, 0, s)), new F<S,I,I,S,S>((p0,p1,p2,p3) => AltStuff(p0, p1, p2, p3)) },
		};
#endif
	}
}
