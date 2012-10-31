using BLToolkit.Data;

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

		[System.Diagnostics.DebuggerStepThrough]
		protected DataAccessor(DbManager dbManager, bool dispose)
			: base(dbManager, dispose)
		{
		}

		#endregion
	}
}
