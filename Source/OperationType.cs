using System;

namespace Rsdn.Framework.Data
{
	/// <summary>
	/// Type of an operation being performed.
	/// </summary>
	public enum OperationType
	{
		/// <summary>
		/// 
		/// </summary>
		OpenConnection,

		/// <summary>
		/// 
		/// </summary>
		CloseConnection,

		/// <summary>
		/// 
		/// </summary>
		BeginTransaction,

		/// <summary>
		/// 
		/// </summary>
		CommitTransaction,

		/// <summary>
		/// 
		/// </summary>
		DisposeTransaction,

		/// <summary>
		/// 
		/// </summary>
		DeriveParameters,

		/// <summary>
		/// 
		/// </summary>
		PrepareCommand,

		/// <summary>
		/// 
		/// </summary>
		ExecuteNonQuery,

		/// <summary>
		/// 
		/// </summary>
		ExecuteScalar,

		/// <summary>
		/// 
		/// </summary>
		ExecuteReader,

		/// <summary>
		/// 
		/// </summary>
		Fill,

		/// <summary>
		/// 
		/// </summary>
		Update
	}
}
