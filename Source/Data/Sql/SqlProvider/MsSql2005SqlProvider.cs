using System;
using System.Data;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class MsSql2005SqlProvider : MsSqlSqlProvider
	{
		public MsSql2005SqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction)expr;

				switch (Type.GetTypeCode(func.SystemType))
				{
					case TypeCode.DateTime :

						if (func.Name == "Convert" && func.Parameters[1].SystemType == typeof(DateTime))
						{
							if (IsDateDataType(func.Parameters[0], "Datetime"))
								return new SqlExpression(
									func.SystemType, "Cast(Floor(Cast({0} as Float)) as DateTime)", Precedence.Primary, func.Parameters[1]);
						}

						break;
				}
			}

			return expr;
		}

		protected override void BuildDataType(System.Text.StringBuilder sb, SqlDataType type)
		{
			switch (type.DbType)
			{
				case SqlDbType.Date      :
				case SqlDbType.DateTime2 : sb.Append("DateTime");        break;
				default                  : base.BuildDataType(sb, type); break;
			}
		}
	}
}
