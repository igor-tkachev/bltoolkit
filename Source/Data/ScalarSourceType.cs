using System;

namespace BLToolkit.Data
{
	/// <summary>
	/// Defines the method how a scalar value is returned from the server.
	/// </summary>
	public enum ScalarSourceType
	{
		DataReader,
		OutputParameter,
		ReturnValue,
		AffectedRows,
	}
}
