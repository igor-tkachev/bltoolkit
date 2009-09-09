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

		protected override void BuildOffsetLimit(StringBuilder sb)
		{
			if (SqlBuilder.Select.SkipValue == null)
				base.BuildOffsetLimit(sb);
			else
			{
				AppendIndent(sb).AppendFormat
				(
					SqlBuilder.Select.SkipValue != null ? "LIMIT {0},{1}" : "LIMIT {1}",
					BuildExpression(new StringBuilder(), SqlBuilder.Select.SkipValue),
					SqlBuilder.Select.TakeValue == null?
						long.MaxValue.ToString():
						BuildExpression(new StringBuilder(), SqlBuilder.Select.TakeValue).ToString()
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
					case "CharIndex" : return new SqlFunction("Locate", func.Parameters);
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
