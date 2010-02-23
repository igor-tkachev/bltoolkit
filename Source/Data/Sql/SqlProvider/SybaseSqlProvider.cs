using System;
using System.Data;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class SybaseSqlProvider : BasicSqlProvider
	{
		public SybaseSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
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

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

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
							SqlValue value = (SqlValue)func.Parameters[3];

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

		bool _isSelect;
		bool _skipAliases;

		SybaseSqlProvider(DataProviderBase dataProvider, bool skipAliases) : base(dataProvider)
		{
			_skipAliases = skipAliases;
		}

		protected override void BuildSelectClause(StringBuilder sb)
		{
			_isSelect = true;
			base.BuildSelectClause(sb);
			_isSelect = false;
		}

		protected override void BuildColumn(StringBuilder sb, SqlQuery.Column col, ref bool addAlias)
		{
			base.BuildColumn(sb, col, ref addAlias);
			if (_skipAliases) addAlias = false;
		}

		protected override ISqlProvider CreateSqlProvider()
		{
			return new SybaseSqlProvider(DataProvider, _isSelect);
		}

		protected override void BuildDataType(StringBuilder sb, SqlDataType type)
		{
			switch (type.DbType)
			{
				case SqlDbType.DateTime2 : sb.Append("DateTime");        break;
				default                  : base.BuildDataType(sb, type); break;
			}
		}

		protected override void BuildDeleteClause(StringBuilder sb)
		{
			AppendIndent(sb);
			sb.Append("DELETE FROM ");
			BuildTableName(sb, SqlQuery.From.Tables[0], true, false);
			sb.AppendLine();
		}

		protected override void BuildUpdateTableName(StringBuilder sb)
		{
			BuildTableName(sb, SqlQuery.From.Tables[0], true, false);
		}

		protected override void BuildUnicodeString(StringBuilder sb, string value)
		{
			sb
				.Append("N\'")
				.Append(value.Replace("'", "''"))
				.Append('\'');
		}
	}
}
