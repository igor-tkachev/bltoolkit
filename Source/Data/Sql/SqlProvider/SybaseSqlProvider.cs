﻿using System;
using System.Data;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class SybaseSqlProvider : BasicSqlProvider
	{
		public SybaseSqlProvider()
		{
		}

		protected override void BuildGetIdentity(StringBuilder sb)
		{
			sb
				.AppendLine()
				.AppendLine("SELECT @@IDENTITY");
		}

		protected override string FirstFormat              { get { return "TOP {0}"; } }

		public    override bool   IsSkipSupported          { get { return false;     } }
		public    override bool   TakeAcceptsParameter     { get { return false;     } }
		public    override bool   IsSubQueryTakeSupported  { get { return false;     } }
		public    override bool   IsCountSubQuerySupported { get { return false;     } }
		public    override bool   CanCombineParameters     { get { return false;     } }

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlFunction)
			{
				var func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "CharIndex" :
						if (func.Parameters.Length == 3)
							return Add<int>(
								ConvertExpression(new SqlFunction(func.SystemType, "CharIndex",
									func.Parameters[0],
									ConvertExpression(new SqlFunction(typeof(string), "Substring",
										func.Parameters[1],
										func.Parameters[2], new SqlFunction(typeof(int), "Len", func.Parameters[1]))))),
								Sub(func.Parameters[2], 1));
						break;

					case "Stuff"     :
						if (func.Parameters[3] is SqlValue)
						{
							var value = (SqlValue)func.Parameters[3];

							if (value.Value is string && string.IsNullOrEmpty((string)value.Value))
								return new SqlFunction(
									func.SystemType,
									func.Name,
									func.Precedence,
									func.Parameters[0],
									func.Parameters[1],
									func.Parameters[1],
									new SqlValue(null));
						}

						break;
				}
			}

			return expr;
		}

		protected override void BuildFunction(StringBuilder sb, SqlFunction func)
		{
			func = ConvertFunctionParameters(func);
			base.BuildFunction(sb, func);
		}

		private  bool _isSelect;
		readonly bool _skipAliases;

		SybaseSqlProvider(bool skipAliases)
		{
			_skipAliases = skipAliases;
		}

		protected override void BuildSelectClause(StringBuilder sb)
		{
			_isSelect = true;
			base.BuildSelectClause(sb);
			_isSelect = false;
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

			if (_skipAliases) addAlias = false;
		}

		protected override ISqlProvider CreateSqlProvider()
		{
			return new SybaseSqlProvider(_isSelect);
		}

		protected override void BuildDataType(StringBuilder sb, SqlDataType type)
		{
			switch (type.SqlDbType)
			{
#if !MONO
				case SqlDbType.DateTime2 : sb.Append("DateTime");        break;
#endif
				default                  : base.BuildDataType(sb, type); break;
			}
		}

		protected override void BuildDeleteClause(StringBuilder sb)
		{
			AppendIndent(sb);
			sb.Append("DELETE FROM ");
//			BuildTableName(sb, SqlQuery.From.Tables[0], true, false);

			ISqlTableSource table;
			ISqlTableSource source;

			if (SqlQuery.Delete.Table != null)
				table = source = SqlQuery.Delete.Table;
			else
			{
				table  = SqlQuery.From.Tables[0];
				source = SqlQuery.From.Tables[0].Source;
			}

			var alias = GetTableAlias(table);
			BuildPhysicalTable(sb, source, alias);

			sb.AppendLine();
		}

		protected override void BuildUpdateTableName(StringBuilder sb)
		{
			if (SqlQuery.Update.Table != null && SqlQuery.Update.Table != SqlQuery.From.Tables[0].Source)
				BuildPhysicalTable(sb, SqlQuery.Update.Table, null);
			else
				BuildTableName(sb, SqlQuery.From.Tables[0], true, false);
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

		public override object Convert(object value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.NameToQueryParameter:
				case ConvertType.NameToCommandParameter:
				case ConvertType.NameToSprocParameter:
					{
						var name = "@" + value;

						if (name.Length > 27)
							name = name.Substring(0, 27);

						return name;
					}

				case ConvertType.NameToQueryField:
				case ConvertType.NameToQueryFieldAlias:
				case ConvertType.NameToQueryTableAlias:
					{
						var name = value.ToString();

						if (name.Length > 28 || name.Length > 0 && name[0] == '[')
							return value;
					}

					return "[" + value + "]";

				case ConvertType.NameToDatabase:
				case ConvertType.NameToOwner:
				case ConvertType.NameToQueryTable:
					{
						var name = value.ToString();

						if (name.Length > 28 || name.Length > 0 && (name[0] == '[' || name[0] == '#'))
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

		protected override void BuildEmptyInsert(StringBuilder sb)
		{
			sb.AppendLine("VALUES ()");
		}
	}
}
