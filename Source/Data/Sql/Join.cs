using System;
using System.Collections.Generic;

using BLToolkit.Reflection.Extension;

namespace BLToolkit.Data.Sql
{
	public class Join
	{
		public Join()
		{
		}

		public Join(string tableName, params JoinOn[] joinOns)
			: this(tableName, null, joinOns)
		{
		}

		public Join(string tableName, string alias, params JoinOn[] joinOns)
		{
			_tableName = tableName;
			_alias     = alias;
			_joinOns.AddRange(joinOns);
		}

		public Join(AttributeExtension ext)
		{
			_tableName = (string)ext["TableName"];
			_alias     = (string)ext["Alias"];

			AttributeExtensionCollection col = ext.Attributes["On"];

			foreach (AttributeExtension ae in col)
				_joinOns.Add(new JoinOn(ae));
		}

		private string _tableName;
		public  string  TableName { get { return _tableName; } set { _tableName = value; } }

		private string _alias;
		public  string  Alias { get { return _alias; } set { _alias = value; } }

		private List<JoinOn> _joinOns = new List<JoinOn>();
		public  List<JoinOn>  JoinOns
		{
			get { return _joinOns; }
		}

		public Join Clone()
		{
			Join join = new Join(_tableName, _alias);

			foreach (JoinOn on in JoinOns)
				join.JoinOns.Add(new JoinOn(on.Field, on.OtherField, on.Expression));

			return join;
		}
	}
}
