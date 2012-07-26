using System;
using System.Runtime.Serialization;

namespace BLToolkit.Data.Linq.Builder
{
	[Serializable]
	public class LinqBuilderException : Exception
	{
		public LinqBuilderException(Exception innerException, string testFile)
			: base(
				innerException.Message + Environment.NewLine + (
					testFile == null ?
						"To generate test code to diagnose the problem set 'BLToolkit.Common.Configuration.Linq.GenerateTestSourceOnException = true'." :
						"Test code generated. See '" + testFile + "'"),
				innerException)
		{
		}

#if !SILVERLIGHT

		protected LinqBuilderException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

#endif
	}
}

