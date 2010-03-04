using System;
using System.Data;
using System.Text;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class InformixSqlProvider : BasicSqlProvider
	{
		public InformixSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override int CommandCount(SqlQuery sqlQuery)
		{
			return sqlQuery.QueryType == QueryType.Insert && sqlQuery.Set.WithIdentity ? 2 : 1;
		}

		protected override void BuildCommand(int commandNumber, StringBuilder sb)
		{
			sb.AppendLine("SELECT DBINFO('sqlca.sqlerrd1') FROM systables where tabid = 1");
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
					case "%": return new SqlFunction(be.SystemType, "Mod",    be.Expr1, be.Expr2);
					case "&": return new SqlFunction(be.SystemType, "BitAnd", be.Expr1, be.Expr2);
					case "|": return new SqlFunction(be.SystemType, "BitOr",  be.Expr1, be.Expr2);
					case "^": return new SqlFunction(be.SystemType, "BitXor", be.Expr1, be.Expr2);
					case "+": return be.SystemType == typeof(string)? new SqlBinaryExpression(be.SystemType, be.Expr1, "||", be.Expr2, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction)expr;

				switch (func.Name)
				{
					case "Coalesce" : return new SqlFunction(func.SystemType, "Nvl", func.Parameters);
					case "Convert"  :
						{
							ISqlExpression par0 = func.Parameters[0];
							ISqlExpression par1 = func.Parameters[1];

							switch (Type.GetTypeCode(TypeHelper.GetUnderlyingType(func.SystemType)))
							{
								case TypeCode.String   : return new SqlFunction(func.SystemType, "To_Char", func.Parameters[1]);
								case TypeCode.Boolean  :
									{
										ISqlExpression ex = AlternativeConvertToBoolean(func, 1);
										if (ex != null)
											return ex;
										break;
									}

								case TypeCode.UInt64:
									if (TypeHelper.IsFloatType(func.Parameters[1].SystemType))
										par1 = new SqlFunction(func.SystemType, "Floor", func.Parameters[1]);
									break;

								case TypeCode.DateTime :
									if (IsDateDataType(func.Parameters[0], "Date"))
									{
										if (func.Parameters[1].SystemType == typeof(string))
										{
											return new SqlFunction(
												func.SystemType,
												"Date",
												new SqlFunction(func.SystemType, "To_Date", func.Parameters[1], new SqlValue("%Y-%m-%d")));
										}

										return new SqlFunction(func.SystemType, "Date", func.Parameters[1]);
									}

									if (IsTimeDataType(func.Parameters[0]))
										return new SqlExpression(func.SystemType, "Cast(Extend({0}, hour to second) as Char(8))", Precedence.Primary, func.Parameters[1]);

									return new SqlFunction(func.SystemType, "To_Date", func.Parameters[1]);

								default:
									if (TypeHelper.GetUnderlyingType(func.SystemType) == typeof(DateTimeOffset))
										goto case TypeCode.DateTime;
									break;
							}

							return new SqlExpression(func.SystemType, "Cast({0} as {1})", Precedence.Primary, par1, par0);
						}

					case "Quarter"  : return Inc(Div(Dec(new SqlFunction(func.SystemType, "Month", func.Parameters)), 3));
					case "WeekDay"  : return Inc(new SqlFunction(func.SystemType, "weekDay", func.Parameters));
					case "DayOfYear":
						return
							Inc(Sub<int>(
								new SqlFunction(null, "Mdy",
									new SqlFunction(null, "Month", func.Parameters),
									new SqlFunction(null, "Day",   func.Parameters),
									new SqlFunction(null, "Year",  func.Parameters)),
								new SqlFunction(null, "Mdy",
									new SqlValue(1),
									new SqlValue(1),
									new SqlFunction(null, "Year", func.Parameters))));
					case "Week"     :
						return
							new SqlExpression(
								func.SystemType,
								"((Extend({0}, year to day) - (Mdy(12, 31 - WeekDay(Mdy(1, 1, year({0}))), Year({0}) - 1) + Interval(1) day to day)) / 7 + Interval(1) day to day)::char(10)::int",
								func.Parameters);
					case "Hour"     :
					case "Minute"   :
					case "Second"   : return new SqlExpression(func.SystemType, string.Format("({{0}}::datetime {0} to {0})::char(3)::int", func.Name), func.Parameters);
				}
			}

			return expr;
		}

		protected override void BuildValue(StringBuilder sb, object value)
		{
			if (value is bool || value is bool?)
				sb.Append((bool)value ? "'t'" : "'f'");
			else
				base.BuildValue(sb, value);
		}

		protected override void BuildDataType(StringBuilder sb, SqlDataType type)
		{
			switch (type.DbType)
			{
				case SqlDbType.TinyInt    : sb.Append("SmallInt");        break;
				case SqlDbType.SmallMoney : sb.Append("Decimal(10,4)");   break;
				default                   : base.BuildDataType(sb, type); break;
			}
		}

		public override SqlQuery Finalize(SqlQuery sqlQuery)
		{
			CheckAliases(sqlQuery);

			new QueryVisitor().Visit(sqlQuery.Select, delegate(IQueryElement element)
			{
				if (element.ElementType == QueryElementType.SqlParameter)
					((SqlParameter)element).IsQueryParameter = false;
			});

			sqlQuery = base.Finalize(sqlQuery);

			switch (sqlQuery.QueryType)
			{
				case QueryType.Delete :
					sqlQuery = GetAlternativeDelete(sqlQuery);
					sqlQuery.From.Tables[0].Alias = "$";
					break;

				case QueryType.Update:
					sqlQuery = GetAlternativeUpdate(sqlQuery);
					break;
			}

			return sqlQuery;
		}

		protected override void BuildFromClause(StringBuilder sb)
		{
			if (SqlQuery.QueryType != QueryType.Update)
				base.BuildFromClause(sb);
		}
	}
}
