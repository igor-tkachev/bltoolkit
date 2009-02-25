using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BLToolkit.Data.Sql;

namespace BLToolkit.Data.Linq
{
	class QueryInfo
	{
		public QueryInfo(ParseInfo<Expression> parseInfo, SqlTable table)
		{
			ParseInfo = parseInfo;

			foreach (var field in table.Fields.Values)
				_columns.Add(field.Name, field);
		}

		Dictionary<string,ISqlExpression> _columns = new Dictionary<string,ISqlExpression>();

		public ParseInfo<Expression> ParseInfo;

		public void SetAlias(string alias, SqlBuilder sql)
		{
			foreach (var item in _columns.Values)
			{
				var field = item as SqlField;

				if (field != null)
				{
					var source = sql.From[field.Table];

					if (source.Alias == null)
						source.Alias = alias;
				}

				break;
			}
		}

		public void BuildSelect(SqlBuilder sql)
		{
			sql.Select.Columns.Clear();

			foreach (var c in _columns)
				sql.Select.Expr(c.Value);
		}
	}
}
