using System;
using System.Runtime.Serialization;

namespace Rsdn.Framework.DataAccess
{
	[Serializable]
	public class RsdnDataAccessException : Exception
	{
		public RsdnDataAccessException()
		{
		}

		public RsdnDataAccessException(string message)
			: base(message)
		{
		}

		public RsdnDataAccessException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected RsdnDataAccessException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
