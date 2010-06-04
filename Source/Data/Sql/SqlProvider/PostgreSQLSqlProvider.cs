using System;
using System.Data;
using System.Text;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class PostgreSQLSqlProvider : BasicSqlProvider
	{
		public PostgreSQLSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override int CommandCount(SqlQuery sqlQuery)
		{
			return sqlQuery.QueryType == QueryType.Insert && sqlQuery.Set.WithIdentity ? 2 : 1;
		}

		protected override void BuildCommand(int commandNumber, StringBuilder sb)
		{
			SequenceNameAttribute attr = GetSequenceNameAttribute();

			AppendIndent(sb)
				.Append("SELECT currval('")
				.Append(attr.SequenceName)
				.AppendLine("')");
		}

		protected override string LimitFormat  { get { return "LIMIT {0}"; } }
		protected override string OffsetFormat { get { return "OFFSET {0} "; } }

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "^": return new SqlBinaryExpression(be.SystemType, be.Expr1, "#", be.Expr2);
					case "+": return be.SystemType == typeof(string)? new SqlBinaryExpression(be.SystemType, be.Expr1, "||", be.Expr2, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Convert"   :
						if (TypeHelper.GetUnderlyingType(func.SystemType) == typeof(bool))
						{
							ISqlExpression ex = AlternativeConvertToBoolean(func, 1);
							if (ex != null)
								return ex;
						}

						return new SqlExpression(func.SystemType, "Cast({0} as {1})", Precedence.Primary, FloorBeforeConvert(func), func.Parameters[0]);

					case "CharIndex" :
						return func.Parameters.Length == 2?
							new SqlExpression(func.SystemType, "Position({0} in {1})", Precedence.Primary, func.Parameters[0], func.Parameters[1]):
							Add<int>(
								new SqlExpression(func.SystemType, "Position({0} in {1})", Precedence.Primary, func.Parameters[0],
									ConvertExpression(new SqlFunction(typeof(string), "Substring",
										func.Parameters[1],
										func.Parameters[2],
										Sub<int>(ConvertExpression(new SqlFunction(typeof(int), "Length", func.Parameters[1])), func.Parameters[2])))),
								Sub(func.Parameters[2], 1));
				}
			}
			else if (expr is SqlExpression)
			{
				SqlExpression e = (SqlExpression)expr;

				if (e.Expr.StartsWith("Extract(DOW"))
					return Inc(new SqlExpression(expr.SystemType, e.Expr.Replace("Extract(DOW", "Extract(Dow"), e.Parameters));

				if (e.Expr.StartsWith("Extract(Millisecond"))
					return new SqlExpression(expr.SystemType, "Cast(To_Char(t.DateTimeValue, 'MS') as int)", e.Parameters);
			}

			return expr;
		}

		protected override void BuildValue(StringBuilder sb, object value)
		{
			if (value is bool)
				sb.Append(value);
			else
				base.BuildValue(sb, value);
		}

		protected override void BuildDataType(StringBuilder sb, SqlDataType type)
		{
			switch (type.DbType)
			{
				case SqlDbType.TinyInt       : sb.Append("SmallInt");        break;
				case SqlDbType.Money         : sb.Append("Decimal(19,4)");   break;
				case SqlDbType.SmallMoney    : sb.Append("Decimal(10,4)");   break;
				case SqlDbType.SmallDateTime :
				case SqlDbType.DateTime      :
				case SqlDbType.DateTime2     : sb.Append("TimeStamp");       break;
				case SqlDbType.Bit           : sb.Append("Boolean");         break;
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
			CheckAliases(sqlQuery);

			sqlQuery = base.Finalize(sqlQuery);

			switch (sqlQuery.QueryType)
			{
				case QueryType.Delete : return GetAlternativeDelete(sqlQuery);
				case QueryType.Update : return GetAlternativeUpdate(sqlQuery);
				default               : return sqlQuery;
			}
		}

		protected override void BuildFromClause(StringBuilder sb)
		{
			if (SqlQuery.QueryType != QueryType.Update)
				base.BuildFromClause(sb);
		}
	}
}
