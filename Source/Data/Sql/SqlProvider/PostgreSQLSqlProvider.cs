using System;
using System.Data;
using System.Text;
using System.Linq;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;
	using Reflection;

	public class PostgreSQLSqlProvider : BasicSqlProvider
	{
		public override bool IsInsertOrUpdateSupported { get { return false; } }

		public override int CommandCount(SqlQuery sqlQuery)
		{
			return sqlQuery.IsInsert && sqlQuery.Insert.WithIdentity ? 2 : 1;
		}

		protected override void BuildCommand(int commandNumber, StringBuilder sb)
		{
			var into = SqlQuery.Insert.Into;
			var attr = GetSequenceNameAttribute(into, false);
			var name =
				attr != null ?
					attr.SequenceName :
					Convert(
						string.Format("{0}_{1}_seq", into.PhysicalName, into.GetIdentityField().PhysicalName),
						ConvertType.NameToQueryField);

			AppendIndent(sb)
				.Append("SELECT currval('")
				.Append(name)
				.AppendLine("')");
		}

		protected override ISqlProvider CreateSqlProvider()
		{
			return new PostgreSQLSqlProvider();
		}

		protected override string LimitFormat  { get { return "LIMIT {0}";   } }
		protected override string OffsetFormat { get { return "OFFSET {0} "; } }

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				var be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "^": return new SqlBinaryExpression(be.SystemType, be.Expr1, "#", be.Expr2);
					case "+": return be.SystemType == typeof(string)? new SqlBinaryExpression(be.SystemType, be.Expr1, "||", be.Expr2, be.Precedence): expr;
				}
			}
			else if (expr is SqlFunction)
			{
				var func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Convert"   :
						if (TypeHelper.GetUnderlyingType(func.SystemType) == typeof(bool))
						{
							var ex = AlternativeConvertToBoolean(func, 1);
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
				var e = (SqlExpression)expr;

				if (e.Expr.StartsWith("Extract(DOW"))
					return Inc(new SqlExpression(expr.SystemType, e.Expr.Replace("Extract(DOW", "Extract(Dow"), e.Parameters));

				if (e.Expr.StartsWith("Extract(Millisecond"))
					return new SqlExpression(expr.SystemType, "Cast(To_Char({0}, 'MS') as int)", e.Parameters);
			}

			return expr;
		}

		public override void BuildValue(StringBuilder sb, object value)
		{
			if (value is bool)
				sb.Append(value);
			else
				base.BuildValue(sb, value);
		}

		protected override void BuildDataType(StringBuilder sb, SqlDataType type)
		{
			switch (type.SqlDbType)
			{
				case SqlDbType.TinyInt       : sb.Append("SmallInt");        break;
				case SqlDbType.Money         : sb.Append("Decimal(19,4)");   break;
				case SqlDbType.SmallMoney    : sb.Append("Decimal(10,4)");   break;
#if !MONO
				case SqlDbType.DateTime2     :
#endif
				case SqlDbType.SmallDateTime :
				case SqlDbType.DateTime      : sb.Append("TimeStamp");       break;
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
			CheckAliases(sqlQuery, int.MaxValue);

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
			if (!SqlQuery.IsUpdate)
				base.BuildFromClause(sb);
		}

		public static bool QuoteIdentifiers;

		public override object Convert(object value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.NameToQueryField:
				case ConvertType.NameToQueryFieldAlias:
				case ConvertType.NameToQueryTable:
				case ConvertType.NameToQueryTableAlias:
                case ConvertType.NameToOwner:
					if (QuoteIdentifiers)
					{
						var name = value.ToString();

						if (name.Length > 0 && name[0] == '"')
							return value;

						return '"' + name + '"';
					}

					break;

				case ConvertType.NameToQueryParameter:
				case ConvertType.NameToCommandParameter:
				case ConvertType.NameToSprocParameter:
					return ":" + value.ToString().Replace(" ", string.Empty);

				case ConvertType.SprocParameterToName:
					if (value != null)
					{
						var str = value.ToString();
						return (str.Length > 0 && str[0] == ':')? str.Substring(1): str;
					}

					break;
			}

			return value;
		}

		public override ISqlExpression GetIdentityExpression(SqlTable table, SqlField identityField, bool forReturning)
		{
			if (table.SequenceAttributes != null)
			{
				var attr = GetSequenceNameAttribute(table, false);
	
				if (attr != null)
					return new SqlExpression("nextval('" + attr.SequenceName+"')", Precedence.Primary);
			}

			return base.GetIdentityExpression(table, identityField, forReturning);
		}

        protected override void BuildInsertClause( StringBuilder sb, string insertText, bool appendTableName )
        {
            AppendIndent(sb).Append(insertText);

			if (appendTableName)
				BuildPhysicalTable(sb, SqlQuery.Insert.Into, null);

			if (SqlQuery.Insert.Items.Count == 0)
			{
				sb.Append(' ');
				BuildEmptyInsert(sb);
			}
			else
			{
				sb.AppendLine();

				AppendIndent(sb).AppendLine("(");

				Indent++;

				var first = true;

				foreach (var expr in SqlQuery.Insert.Items)
				{
					if (!first)
						sb.Append(',').AppendLine();
					first = false;

					AppendIndent(sb);
					BuildExpression(sb, expr.Column, false, true);
				}

				Indent--;

				sb.AppendLine();
				AppendIndent(sb).AppendLine(")");

				if (SqlQuery.QueryType == QueryType.InsertOrUpdate || SqlQuery.From.Tables.Count == 0)
				{
					AppendIndent(sb).AppendLine("VALUES");
					AppendIndent(sb).AppendLine("(");

					Indent++;

					first = true;

					foreach (var expr in SqlQuery.Insert.Items)
					{
						if (!first)
							sb.Append(',').AppendLine();
						first = false;

						AppendIndent(sb);
						BuildExpression(sb, expr.Expression);
					}

					Indent--;

					sb.AppendLine();
                    AppendIndent( sb ).AppendLine( ")" );

                    if ( SqlQuery.Insert.WithOutput )
                    {
                        var pkField = SqlQuery.Insert.Into.Fields.FirstOrDefault( x => x.Value.IsIdentity && x.Value.IsPrimaryKey );

                        var name = pkField.Value.Name;

                        if ( QuoteIdentifiers && ( name.Length > 0 && name[0] != '"' ) )
                            name = '"' + name + '"';

                        sb.Append( string.Format( "RETURNING {0} as id", name ) );
                    }
				}
			}
        }

        protected override void BuildInsertQuery( StringBuilder sb )
        {
            if (SqlQuery.Insert.WithOutput)
                sb.Append( "WITH taboutput AS (" );

            base.BuildInsertQuery( sb );

            if (SqlQuery.Insert.WithOutput)
                sb.AppendLine().Append( ")SELECT id FROM taboutput LIMIT 1" );
        }

		//protected override void BuildInsertOrUpdateQuery(StringBuilder sb)
		//{
		//	BuildInsertOrUpdateQueryAsMerge(sb, null);
		//}
	}
}
