namespace BLToolkit.Data.DataProvider
{
	public enum ConvertType
	{
		/// <summary>
		/// Provided name should be converted to query parameter name.
		/// For example:
		///     firstName -> @firstName
		/// for the following query:
		///     SELECT * FROM Person WHERE FirstName = @firstName
		///                                            ^ here
		/// </summary>
		NameToQueryParameter,

		/// <summary>
		/// Provided name should be converted to command parameter name.
		/// For example:
		///     firstName -> @firstName
		/// for the following query:
		///     db.Parameter("@firstName") = "John";
		///                   ^ here
		/// </summary>
		NameToParameter,

		/// <summary>
		/// Provided name should be converted to query field name.
		/// For example:
		///     firstName -> @firstName
		/// for the following query:
		///     SELECT [FirstName] FROM Person WHERE ID = 1
		///            ^   and   ^
		/// </summary>
		NameToQueryField,

		/// <summary>
		/// Provided name should be converted to query table name.
		/// For example:
		///     firstName -> @firstName
		/// for the following query:
		///     SELECT * FROM [Person]
		///                   ^ and  ^
		/// </summary>
		NameToQueryTable,

		/// <summary>
		/// Provided command parameter name should be converted to name.
		/// For example:
		///     @firstName -> firstName
		/// for the following query:
		///     db.Parameter("@firstName") = "John";
		///                   ^ at has to be removed
		/// </summary>
		ParameterToName,

		/// <summary>
		/// Gets error number from a native exception.
		/// For example:
		///     SqlException -> SqlException.Number,
		///   OleDbException -> OleDbException.Errors[0].NativeError
		/// </summary>
		ExceptionToErrorNumber,
	}
}
