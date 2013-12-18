using System;
using System.Collections.Generic;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;
	using Reflection;

	public abstract class MsSqlSqlProvider : BasicSqlProvider
	{
		public override bool IsApplyJoinSupported { get { return true; } }

		protected virtual  bool BuildAlternativeSql  { get { return true; } }

		protected override string FirstFormat
		{
			get { return SqlQuery.Select.SkipValue == null ? "TOP ({0})" : null; }
		}

		protected override void BuildSql(StringBuilder sb)
		{
			if (BuildAlternativeSql)
				AlternativeBuildSql(sb, true, base.BuildSql);
			else
				base.BuildSql(sb);
		}

		protected override void BuildGetIdentity(StringBuilder sb)
		{
			sb
				.AppendLine()
				.AppendLine("SELECT SCOPE_IDENTITY()");
		}
		
		protected override void BuildSetOutput( StringBuilder sb )
	        {
	        	// TODO: set the right data type
	            sb
	                .AppendLine()
	                .Append( "declare @tabTempInsert table(TempID " ).Append( "uniqueidentifier" ).AppendLine( ")" );
	        }
	
	        protected override void BuildGetOutput( StringBuilder sb )
	        {
	            sb
	                .AppendLine()
	                .AppendLine( "select top 1 TempID from @tabTempInsert" );
	        }

		protected override void BuildOrderByClause(StringBuilder sb)
		{
			if (!BuildAlternativeSql || !NeedSkip)
				base.BuildOrderByClause(sb);
		}

		protected override IEnumerable<SqlQuery.Column> GetSelectedColumns()
		{
			if (BuildAlternativeSql && NeedSkip && !SqlQuery.OrderBy.IsEmpty)
				return AlternativeGetSelectedColumns(base.GetSelectedColumns);
			return base.GetSelectedColumns();
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			switch (expr.ElementType)
			{
				case QueryElementType.SqlBinaryExpression:
					{
						var be = (SqlBinaryExpression)expr;

						switch (be.Operation)
						{
							case "%":
								{
									var type1 = TypeHelper.GetUnderlyingType(be.Expr1.SystemType);

									if (type1 == typeof(double) || type1 == typeof(float))
									{
										return new SqlBinaryExpression(
											be.Expr2.SystemType,
											new SqlFunction(typeof(int), "Convert", SqlDataType.Int32, be.Expr1),
											be.Operation,
											be.Expr2);
									}

									break;
								}
						}

						break;
					}

				case QueryElementType.SqlFunction:
					{
						var func = (SqlFunction)expr;

						switch (func.Name)
						{
							case "Convert" :
								{
									if (TypeHelper.GetUnderlyingType(func.SystemType) == typeof(ulong) &&
										TypeHelper.IsFloatType(func.Parameters[1].SystemType))
										return new SqlFunction(
											func.SystemType,
											func.Name,
											func.Precedence,
											func.Parameters[0],
											new SqlFunction(func.SystemType, "Floor", func.Parameters[1]));

									break;
								}
						}

						break;
					}
			}

			return expr;
		}

		protected override void BuildDeleteClause(StringBuilder sb)
		{
			var table = SqlQuery.Delete.Table != null ?
				(SqlQuery.From.FindTableSource(SqlQuery.Delete.Table) ?? SqlQuery.Delete.Table) :
				SqlQuery.From.Tables[0];

			AppendIndent(sb)
				.Append("DELETE ")
				.Append(Convert(GetTableAlias(table), ConvertType.NameToQueryTableAlias))
				.AppendLine();
		}

		protected override void BuildUpdateTableName(StringBuilder sb)
		{
			var table = SqlQuery.Update.Table != null ?
				(SqlQuery.From.FindTableSource(SqlQuery.Update.Table) ?? SqlQuery.Update.Table) :
				SqlQuery.From.Tables[0];

			if (table is SqlTable)
				BuildPhysicalTable(sb, table, null);
			else
				sb.Append(Convert(GetTableAlias(table), ConvertType.NameToQueryTableAlias));
		}

		protected override void BuildString(StringBuilder sb, string value)
		{
			foreach (var ch in value)
			{
				if (ch > 127)
				{
					sb.Append("N");
					break;
				}
			}

			base.BuildString(sb, value);
		}

		protected override void BuildChar(StringBuilder sb, char value)
		{
			if (value > 127)
				sb.Append("N");

			base.BuildChar(sb, value);
		}

		protected override void BuildColumnExpression(StringBuilder sb, ISqlExpression expr, string alias, ref bool addAlias)
		{
			var wrap = false;

			if (expr.SystemType == typeof(bool))
			{
				if (expr is SqlQuery.SearchCondition)
					wrap = true;
				else
				{
					var ex = expr as SqlExpression;
					wrap = ex != null && ex.Expr == "{0}" && ex.Parameters.Length == 1 && ex.Parameters[0] is SqlQuery.SearchCondition;
				}
			}

			if (wrap) sb.Append("CASE WHEN ");
			base.BuildColumnExpression(sb, expr, alias, ref addAlias);
			if (wrap) sb.Append(" THEN 1 ELSE 0 END");
		}

		public override object Convert(object value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.NameToQueryParameter:
				case ConvertType.NameToCommandParameter:
				case ConvertType.NameToSprocParameter:
					return "@" + value;

				case ConvertType.NameToQueryField:
				case ConvertType.NameToQueryFieldAlias:
				case ConvertType.NameToQueryTableAlias:
					{
						var name = value.ToString();

						if (name.Length > 0 && name[0] == '[')
							return value;
					}

					return "[" + value + "]";

				case ConvertType.NameToDatabase:
				case ConvertType.NameToOwner:
				case ConvertType.NameToQueryTable:
					{
						var name = value.ToString();

						if (name.Length > 0 && name[0] == '[')
							return value;

						if (name.IndexOf('.') > 0)
							value = string.Join("].[", name.Split('.'));
					}

					return "[" + value + "]";

				case ConvertType.SprocParameterToName:
					if (value != null)
					{
						var str = value.ToString();
						return str.Length > 0 && str[0] == '@'? str.Substring(1): str;
					}
					break;
			}

			return value;
		}

		protected override void BuildInsertOrUpdateQuery(StringBuilder sb)
		{
			BuildInsertOrUpdateQueryAsUpdateInsert(sb);
		}

		protected override void BuildDateTime(StringBuilder sb, object value)
		{
			sb.Append(string.Format("'{0:yyyy-MM-ddTHH:mm:ss.fff}'", value));
		}

		public override void BuildValue(StringBuilder sb, object value, SqlParameter sqlParameter = null)
		{
			if      (value is sbyte)  sb.Append((byte)(sbyte)value);
			else if (value is ushort) sb.Append((short)(ushort)value);
			else if (value is uint)   sb.Append((int)(uint)value);
			else if (value is ulong)  sb.Append((long)(ulong)value);
			else base.BuildValue(sb, value);
		}
	}
}
