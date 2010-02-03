using System;
using System.Data;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

#if FW3
	using Linq;
#endif

	public class FirebirdSqlProvider : BasicSqlProvider
	{
		public FirebirdSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		protected override void BuildSelectClause(StringBuilder sb)
		{
			if (SqlQuery.From.Tables.Count == 0)
			{
				AppendIndent(sb);
				sb.Append("SELECT").AppendLine();
				BuildColumns(sb);
				AppendIndent(sb);
				sb.Append("FROM rdb$database").AppendLine();
			}
			else
				base.BuildSelectClause(sb);
		}

		protected override bool   SkipFirst   { get { return false;       } }
		protected override string SkipFormat  { get { return "SKIP {0}";  } }
		protected override string FirstFormat { get { return "FIRST {0}"; } }

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "%": return new SqlFunction(be.SystemType, "Mod",     be.Expr1, be.Expr2);
					case "&": return new SqlFunction(be.SystemType, "Bin_And", be.Expr1, be.Expr2);
					case "|": return new SqlFunction(be.SystemType, "Bin_Or",  be.Expr1, be.Expr2);
					case "^": return new SqlFunction(be.SystemType, "Bin_Xor", be.Expr1, be.Expr2);
					case "+": return be.SystemType == typeof(string)? new SqlBinaryExpression(be.SystemType, be.Expr1, "||", be.Expr2, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction)expr;

				switch (func.Name)
				{
					case "Convert" :
						if (func.SystemType == typeof(bool))
						{
							ISqlExpression ex = AlternativeConvertToBoolean(func, 1);
							if (ex != null)
								return ex;
						}

						return new SqlExpression(func.SystemType, "Cast({0} as {1})", Precedence.Primary, FloorBeforeConvert(func), func.Parameters[0]);

#if FW3
					case "DateAdd" :
						switch ((Sql.DateParts)((SqlValue)func.Parameters[0]).Value)
						{
							case Sql.DateParts.Quarter  :
								return new SqlFunction(func.SystemType, func.Name, new SqlValue(Sql.DateParts.Month), Mul(func.Parameters[1], 3), func.Parameters[2]);
							case Sql.DateParts.DayOfYear:
							case Sql.DateParts.WeekDay:
								return new SqlFunction(func.SystemType, func.Name, new SqlValue(Sql.DateParts.Day),   func.Parameters[1],         func.Parameters[2]);
							case Sql.DateParts.Week     :
								return new SqlFunction(func.SystemType, func.Name, new SqlValue(Sql.DateParts.Day),   Mul(func.Parameters[1], 7), func.Parameters[2]);
						}

						break;
#endif

				}
			}
			else if (expr is SqlExpression)
			{
				SqlExpression e = (SqlExpression)expr;

				if (e.Expr.StartsWith("Extract(Quarter"))
					return Inc(Div(Dec(new SqlExpression(e.SystemType, "Extract(Month from {0})", e.Parameters)), 3));

				if (e.Expr.StartsWith("Extract(YearDay"))
					return Inc(new SqlExpression(e.SystemType, e.Expr.Replace("Extract(YearDay", "Extract(yearDay"), e.Parameters));

				if (e.Expr.StartsWith("Extract(WeekDay"))
					return Inc(new SqlExpression(e.SystemType, e.Expr.Replace("Extract(WeekDay", "Extract(weekDay"), e.Parameters));
			}

			return expr;
		}

		protected override void BuildDataType(StringBuilder sb, SqlDataType type)
		{
			switch (type.DbType)
			{
				case SqlDbType.Decimal       :
					if (type == SqlDataType.DbDecimal || type == SqlDataType.UInt64)
						base.BuildDataType(sb, new SqlDataType(type.DbType, type.Type, 18, type.Scale));
					else
						base.BuildDataType(sb, type);
					break;
				case SqlDbType.TinyInt       : sb.Append("SmallInt");        break;
				case SqlDbType.Money         : sb.Append("Decimal(18,4)");   break;
				case SqlDbType.SmallMoney    : sb.Append("Decimal(10,4)");   break;
				case SqlDbType.SmallDateTime :
				case SqlDbType.DateTime      :
				case SqlDbType.DateTime2     : sb.Append("TimeStamp");       break;
				case SqlDbType.NVarChar      :
					sb.Append("VarChar");
					if (type.Length > 0)
						sb.Append('(').Append(type.Length).Append(')');
					break;
				default                      : base.BuildDataType(sb, type); break;
			}
		}

		public override SqlQuery Finalize(SqlQuery sqlQuery)
		{
			sqlQuery = base.Finalize(sqlQuery);

			return
				sqlQuery.IsDelete ?
					GetAlternativeDelete(sqlQuery) :
				sqlQuery.IsUpdate ?
					GetAlternativeUpdate(sqlQuery) :
					sqlQuery;
		}

		protected override void BuildFromClause(StringBuilder sb)
		{
			if (!SqlQuery.IsUpdate)
				base.BuildFromClause(sb);
		}
	}
}
