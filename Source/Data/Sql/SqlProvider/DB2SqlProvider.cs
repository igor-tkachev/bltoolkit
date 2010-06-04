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

		SqlField _identityField;

		public override int CommandCount(SqlQuery sqlQuery)
		{
			if (sqlQuery.QueryType == QueryType.Insert && sqlQuery.Set.WithIdentity)
			{
				_identityField = GetIdentityField(sqlQuery.Set.Into);

				if (_identityField == null)
					return 2;
			}

			return 1;
		}

		public override int BuildSql(int commandNumber, SqlQuery sqlQuery, StringBuilder sb, int indent, int nesting, bool skipAlias)
		{
			if (_identityField != null)
			{
				indent += 2;

				AppendIndent(sb).AppendLine("SELECT");
				AppendIndent(sb).Append("\t");
				BuildExpression(sb, _identityField, false, true);
				sb.AppendLine();
				AppendIndent(sb).AppendLine("FROM");
				AppendIndent(sb).AppendLine("\tNEW TABLE");
				AppendIndent(sb).AppendLine("\t(");
			}

			int ret = base.BuildSql(commandNumber, sqlQuery, sb, indent, nesting, skipAlias);

			if (_identityField != null)
				sb.AppendLine("\t)");

			return ret;
		}

		protected override void BuildCommand(int commandNumber, StringBuilder sb)
		{
			sb.AppendLine("SELECT identity_val_local() FROM SYSIBM.SYSDUMMY1");
		}

		protected override void BuildSql(StringBuilder sb)
		{
			AlternativeBuildSql(sb, false, base.BuildSql);
		}

		protected override void BuildSelectClause(StringBuilder sb)
		{
			if (SqlQuery.From.Tables.Count == 0)
			{
				AppendIndent(sb).AppendLine("SELECT");
				BuildColumns(sb);
				AppendIndent(sb).AppendLine("FROM SYSIBM.SYSDUMMY1 FETCH FIRST 1 ROW ONLY");
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
						if (TypeHelper.GetUnderlyingType(func.SystemType) == typeof(bool))
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
					case "DateTime2"     : return new SqlFunction(func.SystemType, "TimeStamp", func.Parameters);
					case "TinyInt"       : return new SqlFunction(func.SystemType, "SmallInt",  func.Parameters);
					case "Money"         : return new SqlFunction(func.SystemType, "Decimal",   func.Parameters[0], new SqlValue(19), new SqlValue(4));
					case "SmallMoney"    : return new SqlFunction(func.SystemType, "Decimal",   func.Parameters[0], new SqlValue(10), new SqlValue(4));
					case "VarChar"       :
						if (TypeHelper.GetUnderlyingType(func.Parameters[0].SystemType) == typeof(decimal))
							return new SqlFunction(func.SystemType, "Char", func.Parameters[0]);
						break;
					case "NChar"         :
					case "NVarChar"      : return new SqlFunction(func.SystemType, "Char",      func.Parameters);
				}
			}

			return expr;
		}

		public override SqlQuery Finalize(SqlQuery sqlQuery)
		{
			new QueryVisitor().Visit(sqlQuery.Select, delegate(IQueryElement element)
			{
				if (element.ElementType == QueryElementType.SqlParameter)
					((SqlParameter)element).IsQueryParameter = false;
			});

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

		protected override void BuildValue(StringBuilder sb, object value)
		{
			if (value is Guid)
			{
				string s = ((Guid)value).ToString("N");

				sb
					.Append("Cast(x'")
					.Append(s.Substring( 6,  2))
					.Append(s.Substring( 4,  2))
					.Append(s.Substring( 2,  2))
					.Append(s.Substring( 0,  2))
					.Append(s.Substring(10,  2))
					.Append(s.Substring( 8,  2))
					.Append(s.Substring(14,  2))
					.Append(s.Substring(12,  2))
					.Append(s.Substring(16, 16))
					.Append("' as char(16) for bit data)");
			}
			else
				base.BuildValue(sb, value);
		}
	}
}
