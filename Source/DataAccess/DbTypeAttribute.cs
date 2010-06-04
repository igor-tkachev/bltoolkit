using System;
using System.Data;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false), CLSCompliant(false)]
	public sealed class DbTypeAttribute : Attribute
	{
		DbType _dbType; public DbType DbType { get { return _dbType; } set { _dbType = value; } }
		int?   _size;   public int?   Size   { get { return _size;   } set { _size   = value; } }

		public DbTypeAttribute(DbType sqlDbType)
		{
			DbType = sqlDbType;
		}

		public DbTypeAttribute(DbType sqlDbType, int size)
		{
			DbType = sqlDbType;
			Size = size;
		}
	}
}
