using System;

namespace BLToolkit.Data.Sql.SqlProvider
{
	[AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class PostgreSQLSequenceNameAttribute : Attribute
	{
		public PostgreSQLSequenceNameAttribute(string sequenceName)
		{
			SequenceName = sequenceName;
		}

		private string _sequenceName;
		public  string  SequenceName
		{
			get { return _sequenceName;  }
			set { _sequenceName = value; }
		}
	}
}
