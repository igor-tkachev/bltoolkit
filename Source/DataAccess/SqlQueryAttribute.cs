using System;

using BLToolkit.Data;

namespace BLToolkit.DataAccess
{
	[JetBrains.Annotations.BaseTypeRequired(typeof(DataAccessor))]
	[AttributeUsage(AttributeTargets.Method)]
	public class SqlQueryAttribute : Attribute
	{
		public SqlQueryAttribute()
		{
		}

		public SqlQueryAttribute(string sqlText)
		{
			_sqlText = sqlText;
		}

		private string _sqlText;
		public  string  SqlText
		{
			get { return _sqlText;  }
			set { _sqlText = value; }
		}

		private bool _isDynamic;
		public  bool  IsDynamic
		{
			get { return _isDynamic;  }
			set { _isDynamic = value; }
		}

		private int _id = int.MinValue;
		public  int  ID
		{
			get { return _id;  }
			set { _id = value; _isDynamic = value != int.MinValue; }
		}

		public virtual string GetSqlText(DataAccessor accessor, DbManager dbManager)
		{
			return _sqlText;
		}
	}
}
