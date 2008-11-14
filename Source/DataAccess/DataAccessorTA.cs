using BLToolkit.Data;
using BLToolkit.TypeBuilder;

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
			return TypeFactory.CreateInstance<A>();
		}

		public static A CreateInstance(DbManager dbManager)
		{
			return CreateInstance(dbManager, false);
		}

		public static A CreateInstance(DbManager dbManager, bool dispose)
		{
			A da = TypeFactory.CreateInstance<A>();

			da.SetDbManager(dbManager, dispose);

			return da;
		}

		#endregion
	}
}
