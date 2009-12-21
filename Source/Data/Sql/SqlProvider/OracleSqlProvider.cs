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

	public class OracleSqlProvider : BasicSqlProvider
	{
		public OracleSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override bool IsCountSubQuerySupported { get { return false; } }

		protected override void BuildSelectClause(StringBuilder sb)
		{
			if (SqlQuery.From.Tables.Count == 0)
			{
				AppendIndent(sb).Append("SELECT").AppendLine();
				BuildColumns(sb);
				AppendIndent(sb).Append("FROM SYS.DUAL").AppendLine();
			}
			else
				base.BuildSelectClause(sb);
		}

		protected override bool BuildWhere()
		{
			return base.BuildWhere() || !NeedSkip && NeedTake && SqlQuery.OrderBy.IsEmpty && SqlQuery.Having.IsEmpty;
		}

		string _rowNumberAlias;

		protected override void BuildSql(StringBuilder sb)
		{
			bool buildRowNum = NeedSkip || NeedTake && (!SqlQuery.OrderBy.IsEmpty || !SqlQuery.Having.IsEmpty);

			string[] aliases = null;

			if (buildRowNum)
			{
				aliases = GetTempAliases(2, "t");

				if (_rowNumberAlias == null)
					_rowNumberAlias = GetTempAliases(1, "rn")[0];

				AppendIndent(sb).AppendFormat("SELECT {0}.*", aliases[1]).AppendLine();
				AppendIndent(sb).Append("FROM").    AppendLine();
				AppendIndent(sb).Append("(").       AppendLine();
				Indent++;

				AppendIndent(sb).AppendFormat("SELECT {0}.*, ROWNUM as {1}", aliases[0], _rowNumberAlias).AppendLine();
				AppendIndent(sb).Append("FROM").    AppendLine();
				AppendIndent(sb).Append("(").       AppendLine();
				Indent++;
			}

			base.BuildSql(sb);

			if (buildRowNum)
			{
				Indent--;
				AppendIndent(sb).Append(") ").Append(aliases[0]).AppendLine();
				Indent--;
				AppendIndent(sb).Append(") ").Append(aliases[1]).AppendLine();
				AppendIndent(sb).Append("WHERE").AppendLine();

				Indent++;

				if (NeedTake && NeedSkip)
				{
					AppendIndent(sb).AppendFormat("{0}.{1} BETWEEN ", aliases[1], _rowNumberAlias);
					BuildExpression(sb, Add(SqlQuery.Select.SkipValue, 1));
					sb.Append(" AND ");
					BuildExpression(sb, Add<int>(SqlQuery.Select.SkipValue, SqlQuery.Select.TakeValue));
				}
				else if (NeedTake)
				{
					AppendIndent(sb).AppendFormat("{0}.{1} <= ", aliases[1], _rowNumberAlias);
					BuildExpression(sb, Precedence.Comparison, SqlQuery.Select.TakeValue);
				}
				else
				{
					AppendIndent(sb).AppendFormat("{0}.{1} > ", aliases[1], _rowNumberAlias);
					BuildExpression(sb, Precedence.Comparison, SqlQuery.Select.SkipValue);
				}

				sb.AppendLine();
				Indent--;
			}
		}

		protected override void BuildWhereSearchCondition(StringBuilder sb, SqlQuery.SearchCondition condition)
		{
			if (NeedTake && !NeedSkip && SqlQuery.OrderBy.IsEmpty && SqlQuery.Having.IsEmpty)
			{
				BuildPredicate(
					sb,
					Precedence.LogicalConjunction,
					new SqlQuery.Predicate.ExprExpr(
						new SqlExpression(null, "ROWNUM", Precedence.Primary),
						SqlQuery.Predicate.Operator.LessOrEqual,
						SqlQuery.Select.TakeValue));

				if (base.BuildWhere())
				{
					sb.Append(" AND ");
					BuildSearchCondition(sb, Precedence.LogicalConjunction, condition);
				}
			}
			else
				BuildSearchCondition(sb, Precedence.Unknown, condition);
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "%": return new SqlFunction(be.SystemType, "MOD",    be.Expr1, be.Expr2);
					case "&": return new SqlFunction(be.SystemType, "BITAND", be.Expr1, be.Expr2);
					case "|": // (a + b) - BITAND(a, b)
						return Sub(
							Add(be.Expr1, be.Expr2, be.SystemType),
							new SqlFunction(be.SystemType, "BITAND", be.Expr1, be.Expr2),
							be.SystemType);

					case "^": // (a + b) - BITAND(a, b) * 2
						return Sub(
							Add(be.Expr1, be.Expr2, be.SystemType),
							Mul(new SqlFunction(be.SystemType, "BITAND", be.Expr1, be.Expr2), 2),
							be.SystemType);
					case "+": return be.SystemType == typeof(string)? new SqlBinaryExpression(be.SystemType, be.Expr1, "||", be.Expr2, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Coalesce"       : return new SqlFunction  (func.SystemType, "Nvl",    func.Parameters);
					case "Convert"        :
						{
							if (func.SystemType == typeof(DateTime) || func.SystemType == typeof(DateTimeOffset))
							{
								if (IsTimeDataType(func.Parameters[0]))
								{
									if (func.Parameters[1].SystemType == typeof(string))
										return func.Parameters[1];

									return new SqlFunction(func.SystemType, "To_Char", func.Parameters[1], new SqlValue("HH24:MI:SS"));
								}

								if (func.Parameters[1].SystemType == typeof(DateTime) && IsDateDataType(func.Parameters[0], "Date"))
									return new SqlFunction(func.SystemType, "Trunc", func.Parameters[1], new SqlValue("DD"));

								return new SqlFunction(func.SystemType, "To_Timestamp", func.Parameters[1], new SqlValue("YYYY-MM-DD HH24:MI:SS"));
							}

							return new SqlExpression(func.SystemType, "Cast({0} as {1})", Precedence.Primary, func.Parameters[1], func.Parameters[0]);
						}

					case "CharIndex"      :
						return func.Parameters.Length == 2?
							new SqlFunction(func.SystemType, "InStr", func.Parameters[1], func.Parameters[0]):
							new SqlFunction(func.SystemType, "InStr", func.Parameters[1], func.Parameters[0], func.Parameters[2]);
					case "AddYear"        : return new SqlFunction(func.SystemType, "Add_Months", func.Parameters[0], Mul(func.Parameters[1], 12));
					case "AddQuarter"     : return new SqlFunction(func.SystemType, "Add_Months", func.Parameters[0], Mul(func.Parameters[1],  3));
					case "AddMonth"       : return new SqlFunction(func.SystemType, "Add_Months", func.Parameters[0],     func.Parameters[1]);
					case "AddDayOfYear"   :
					case "AddWeekDay"     :
					case "AddDay"         : return Add<DateTime>(func.Parameters[0],     func.Parameters[1]);
					case "AddWeek"        : return Add<DateTime>(func.Parameters[0], Mul(func.Parameters[1], 7));
					case "AddHour"        : return Add<DateTime>(func.Parameters[0], Div(func.Parameters[1],                  24));
					case "AddMinute"      : return Add<DateTime>(func.Parameters[0], Div(func.Parameters[1],             60 * 24));
					case "AddSecond"      : return Add<DateTime>(func.Parameters[0], Div(func.Parameters[1],        60 * 60 * 24));
					case "AddMillisecond" : return Add<DateTime>(func.Parameters[0], Div(func.Parameters[1], 1000 * 60 * 60 * 24));
				}
			}
			else if (expr is SqlExpression)
			{
				SqlExpression e = (SqlExpression)expr;

				if (e.Expr.StartsWith("To_Number(To_Char(") && e.Expr.EndsWith(", 'FF'))"))
					return Div(new SqlExpression(e.SystemType, e.Expr.Replace("To_Number(To_Char(", "to_Number(To_Char("), e.Values), 1000);
			}

			return expr;
		}

		protected override void BuildDataType(StringBuilder sb, SqlDataType type)
		{
			switch (type.DbType)
			{
				case SqlDbType.BigInt     : sb.Append("Number(19)");      break;
				case SqlDbType.TinyInt    : sb.Append("Number(3)");       break;
				case SqlDbType.Money      : sb.Append("Number(19,4)");    break;
				case SqlDbType.SmallMoney : sb.Append("Number(10,4)");    break;
				case SqlDbType.VarChar    :
					sb.Append("VarChar2");
					if (type.Length > 0)
						sb.Append('(').Append(type.Length).Append(')');
					break;
				default                   : base.BuildDataType(sb, type); break;
			}
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
