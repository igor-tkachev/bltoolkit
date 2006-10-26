using System;
using System.Collections.Generic;

using BLToolkit.Data;
using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace BLToolkit.DataAccess
{
	public abstract class DataAccessor<T,A> : DataAccessor<T>
		where A : DataAccessor<T>
	{
		#region Constructors

		[System.Diagnostics.DebuggerStepThrough]
		protected DataAccessor()
		{
		}

		[System.Diagnostics.DebuggerStepThrough]
		protected DataAccessor(DbManager dbManager)
			: base(dbManager)
		{
		}

		#endregion

		#region CreateInstance

		public static A CreateInstance()
		{
			return TypeAccessor<A>.CreateInstanceEx();
		}

		public static A CreateInstance(DbManager dbManager)
		{
			A da = TypeAccessor<A>.CreateInstanceEx();

			da.SetDbManager(dbManager);

			return da;
		}

		#endregion
	}
}
