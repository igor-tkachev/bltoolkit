using System;
using System.Collections.Generic;

using BLToolkit.Data;
using BLToolkit.Aspects;

namespace BLToolkit.DataAccess
{
	public abstract class DataAccessor<T> : DataAccessor
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
	}
}
