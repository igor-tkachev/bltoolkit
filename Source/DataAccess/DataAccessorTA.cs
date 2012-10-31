using BLToolkit.Data;
using BLToolkit.TypeBuilder;

namespace BLToolkit.DataAccess
{
	public abstract class DataAccessor<T,TA> : DataAccessor<T>
		where TA : DataAccessor<T>
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

		public static TA CreateInstance()
		{
			return TypeFactory.CreateInstance<TA>();
		}

		public static TA CreateInstance(DbManager dbManager)
		{
			return CreateInstance(dbManager, false);
		}

		public static TA CreateInstance(DbManager dbManager, bool dispose)
		{
			TA da = TypeFactory.CreateInstance<TA>();

			da.SetDbManager(dbManager, dispose);

			return da;
		}

		#endregion
	}
}
