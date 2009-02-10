using System;
using System.Runtime.Serialization;

namespace BLToolkit.Data.Sql
{
	[Serializable] 
	public class SqlException : Exception
	{
		public SqlException()
			: base("A BLToolkit Sql error has occurred.")
		{
		}

		public SqlException(string message)
			: base(message) 
		{
		}

		public SqlException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public SqlException(Exception innerException)
			: base(innerException.Message, innerException)
		{
		}

		protected SqlException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}

