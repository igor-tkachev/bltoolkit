using System;
using System.Collections.Generic;

using BLToolkit.Data.Sql;

namespace BLToolkit.Data.Linq
{
	class SelectInfo
	{
		public SelectInfo(Type type, SqlTable table)
		{
			_type = type;

			foreach (var field in table.Fields.Values)
				_columns.Add(field.Name, field);
		}

		Type                              _type;
		Dictionary<string,ISqlExpression> _columns = new Dictionary<string,ISqlExpression>();
	}
}
