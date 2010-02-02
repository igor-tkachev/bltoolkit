using System;
using System.Text;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class DB2SqlProvider : BasicSqlProvider
	{
		public DB2SqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override bool TakeAcceptsParameter { get { return SqlQuery.Select.SkipValue != null; } }

		protected override void BuildSql(StringBuilder sb)
		{
			AlternativeBuildSql(sb, false, base.BuildSql);
		}

		protected override void BuildSelectClause(StringBuilder sb)
		{
			if (SqlQuery.From.Tables.Count == 0)
			{
				AppendIndent(sb).Append("SELECT").AppendLine();
				BuildColumns(sb);
				AppendIndent(sb).Append("FROM SYSIBM.SYSDUMMY1 FETCH FIRST 1 ROW ONLY").AppendLine();
			}
			else
				base.BuildSelectClause(sb);
		}

		protected override string LimitFormat
		{
			get { return SqlQuery.Select.SkipValue == null ? "FETCH FIRST {0} ROWS ONLY" : null; }
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "%":
						{
							ISqlExpression expr1 = !TypeHelper.IsIntegerType(be.Expr1.SystemType) ? new SqlFunction(typeof(int), "Int", be.Expr1) : be.Expr1;
							return new SqlFunction(be.SystemType, "Mod", expr1, be.Expr2);
						}
					case "&": return new SqlFunction(be.SystemType, "BitAnd", be.Expr1, be.Expr2);
					case "|": return new SqlFunction(be.SystemType, "BitOr",  be.Expr1, be.Expr2);
					case "^": return new SqlFunction(be.SystemType, "BitXor", be.Expr1, be.Expr2);
					case "+": return be.SystemType == typeof(string)? new SqlBinaryExpression(be.SystemType, be.Expr1, "||", be.Expr2, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Convert"    :
						if (func.SystemType == typeof(bool))
						{
							ISqlExpression ex = AlternativeConvertToBoolean(func, 1);
							if (ex != null)
								return ex;
						}

						if (func.Parameters[0] is SqlDataType)
						{
							SqlDataType type = (SqlDataType)func.Parameters[0];

							if (type.Type == typeof(string) && func.Parameters[1].SystemType != typeof(string))
								return new SqlFunction(func.SystemType, "RTrim", new SqlFunction(typeof(string), "Char", func.Parameters[1]));

							if (type.Length > 0)
								return new SqlFunction(func.SystemType, type.DbType.ToString(), func.Parameters[1], new SqlValue(type.Length));

							if (type.Precision > 0)
								return new SqlFunction(func.SystemType, type.DbType.ToString(), func.Parameters[1], new SqlValue(type.Precision), new SqlValue(type.Scale));

							return new SqlFunction(func.SystemType, type.DbType.ToString(), func.Parameters[1]);
						}

						if (func.Parameters[0] is SqlFunction)
						{
							SqlFunction f = (SqlFunction)func.Parameters[0];

							return
								f.Name == "Char" ?
									new SqlFunction(func.SystemType, f.Name, func.Parameters[1]) :
								f.Parameters.Length == 1 ?
									new SqlFunction(func.SystemType, f.Name, func.Parameters[1], f.Parameters[0]) :
									new SqlFunction(func.SystemType, f.Name, func.Parameters[1], f.Parameters[0], f.Parameters[1]);
						}

						{
							SqlExpression e = (SqlExpression)func.Parameters[0];
							return new SqlFunction(func.SystemType, e.Expr, func.Parameters[1]);
						}

					case "Millisecond"   : return Div(new SqlFunction(func.SystemType, "Microsecond", func.Parameters), 1000);
					case "SmallDateTime" :
					case "DateTime"      :
					case "DateTime2"     : return new SqlFunction  (func.SystemType, "TimeStamp", func.Parameters);
					case "TinyInt"       : return new SqlFunction  (func.SystemType, "SmallInt",  func.Parameters);
					case "Money"         : return new SqlFunction  (func.SystemType, "Decimal",   func.Parameters[0], new SqlValue(19), new SqlValue(4));
					case "SmallMoney"    : return new SqlFunction  (func.SystemType, "Decimal",   func.Parameters[0], new SqlValue(10), new SqlValue(4));
					case "VarChar"       :
						if (func.Parameters[0].SystemType == typeof(decimal))
							return new SqlFunction(func.SystemType, "Char", func.Parameters[0]);
						break;
					case "NChar"         :
					case "NVarChar"      : return new SqlFunction  (func.SystemType, "Char",      func.Parameters);
				}
			}

			return expr;
		}

		public override SqlQuery Finalize(SqlQuery sqlQuery)
		{
			return GetAlternativeDelete(base.Finalize(sqlQuery));
		}
	}
}
