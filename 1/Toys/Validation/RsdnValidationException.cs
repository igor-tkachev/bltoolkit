using System;
using System.Runtime.Serialization;

namespace Rsdn.Framework.Validation
{
	[Serializable] 
	public class RsdnValidationException : Exception
	{
		public RsdnValidationException() 
		{
		}

		public RsdnValidationException(string message) 
			: base(message) 
		{
		}

		public RsdnValidationException(string message, Exception innerException) 
			: base(message, innerException) 
		{
		}

		protected RsdnValidationException(SerializationInfo info, StreamingContext context) 
			: base(info, context) 
		{
		}
	}
}
