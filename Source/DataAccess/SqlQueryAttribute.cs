using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Method)]
	public class SqlQueryAttribute : Attribute
	{
		public SqlQueryAttribute(string sqlText)
		{
			_sqlText = sqlText;
		}

		private readonly string _sqlText;
		public           string  SqlText
		{
			get { return _sqlText; }
		}
	}
}
