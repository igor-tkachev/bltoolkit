using System;
using System.Collections.Generic;
using System.Data;
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
						if (be.SystemType == typeof(string))
						{
							if (be.Expr1 is SqlFunction)
							{
								SqlFunction func = (SqlFunction)be.Expr1;

								if (func.Name == "Concat")
								{
									List<ISqlExpression> list = new List<ISqlExpression>(func.Parameters);
									list.Add(be.Expr2);
									return new SqlFunction(be.SystemType, "Concat", list.ToArray());
								}
							}

							return new SqlFunction(be.SystemType, "Concat", be.Expr1, be.Expr2);
						}

						break;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Convert" : return new SqlExpression(func.SystemType, "Cast({0} as {1})", Precedence.Primary, func.Parameters[1], func.Parameters[0]);
				}
			}
			else if (expr is SqlExpression)
			{
				SqlExpression e = (SqlExpression)expr;

				if (e.Expr.StartsWith("Extract(DayOfYear"))
					return new SqlFunction(e.SystemType, "DayOfYear", e.Values);

				if (e.Expr.StartsWith("Extract(WeekDay"))
					return Inc(
						new SqlFunction(e.SystemType,
							"WeekDay",
							new SqlFunction(
								null,
								"Date_Add",
								e.Values[0],
								new SqlExpression(null, "interval 1 day"))));
			}

			return expr;
		}

		protected override void BuildDataType(StringBuilder sb, SqlDataType type)
		{
			switch (type.DbType)
			{
				case SqlDbType.Int           :
				case SqlDbType.SmallInt      : sb.Append("Signed");        break;
				case SqlDbType.TinyInt       : sb.Append("Unsigned");      break;
				case SqlDbType.Money         : sb.Append("Decimal(19,4)"); break;
				case SqlDbType.SmallMoney    : sb.Append("Decimal(10,4)"); break;
				case SqlDbType.VarChar       : sb.Append("Char");          break;
				case SqlDbType.SmallDateTime :
				case SqlDbType.DateTime2     : sb.Append("DateTime");      break;
				case SqlDbType.Float         :
				case SqlDbType.Real          : base.BuildDataType(sb, SqlDataType.Decimal); break;
				default: base.BuildDataType(sb, type); break;
			}
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
