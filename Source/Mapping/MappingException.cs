using System;
using System.Runtime.Serialization;

namespace BLToolkit.Mapping
{
	/// <summary>
	/// The exception that is thrown by <see cref="Map"/>.
	/// </summary>
	/// <remarks>
	/// <b>MappingException</b> is used as a wrapper of any exceptions,
	/// which may occur during the the execution of the <see cref="Map"/> class methods.
	/// </remarks>
	[Serializable] 
	public class MappingException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <b>MappingException</b> class.
		/// </summary>
		/// <remarks>
		/// This constructor initializes the <para>Message</para> property of the new instance 
		/// to a system-supplied message that describes the error, 
		/// such as "A mapping exception has occurred."
		/// </remarks>
		public MappingException() 
			: base("A mapping exception has occurred.")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <b>MappingException</b> class 
		/// with the specified error message.
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		public MappingException(string message) 
			: base(message) 
		{
		}

		/// <summary>
		/// Initializes a new instance of the <b>MappingException</b> class 
		/// with the specified error message and InnerException property.
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		/// <param name="innerException">The InnerException, if any, that threw the current exception.</param>
		public MappingException(string message, Exception innerException) 
			: base(message, innerException) 
		{
		}

		/// <summary>
		/// Initializes a new instance of the <b>MappingException</b> class 
		/// with inner InvalidCastException with the specified types.
		/// </summary>
		/// <param name="valueType">A <see cref="System.Type"/> used as source type.</param>
		/// <param name="conversionType">A <see cref="System.Type"/> used as destination type.</param>
		public MappingException(Type valueType, Type conversionType)
			: base("A mapping exception has occurred.", new InvalidCastException(
				string.Format("Can not convert type '{0}' to '{1}'.",
					valueType.FullName, conversionType.FullName)))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <b>MappingException</b> class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
		protected MappingException(SerializationInfo info, StreamingContext context) 
			: base(info,context) 
		{
		}
	}
}
