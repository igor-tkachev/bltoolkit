using System;

using JetBrains.Annotations;

namespace BLToolkit.Data
{
	public class DbManagerTransaction : IDisposable
	{
		public DbManagerTransaction([NotNull] DbManager dbManager)
		{
			if (dbManager == null) throw new ArgumentNullException("dbManager");

			DbManager           = dbManager;
		}

		public DbManager DbManager           { get; private set; }

		bool _disposeTransaction = true;

		public void Commit()
		{
			DbManager.CommitTransaction();
			_disposeTransaction = false;
		}

		public void Rollback()
		{
			DbManager.RollbackTransaction();
			_disposeTransaction = false;
		}

		public void Dispose()
		{
			if (_disposeTransaction)
				DbManager.RollbackTransaction();
		}
	}
}
