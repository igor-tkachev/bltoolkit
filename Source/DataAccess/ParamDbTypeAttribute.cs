using System;
using System.Data;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class ParamDbTypeAttribute : Attribute
	{
		public ParamDbTypeAttribute(DbType dbType)
		{
			_dbType = dbType;
		}

		private readonly DbType _dbType;
		public           DbType  DbType
		{
			get { return _dbType; }
		}
	}
}
