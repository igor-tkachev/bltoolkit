using System;

namespace BLToolkit.Data
{
	/// <summary>
	/// Type of an operation being performed.
	/// </summary>
	public enum OperationType
	{
		OpenConnection,
		CloseConnection,
		BeginTransaction,
		CommitTransaction,
		RollbackTransaction,
		DisposeTransaction,
		DeriveParameters,
		PrepareCommand,
		ExecuteNonQuery,
		[Obsolete]
		ExecuteScalar,
		ExecuteReader,
		Fill,
		Update,
		Read
	}
}
