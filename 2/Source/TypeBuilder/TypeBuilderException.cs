using System;
using System.Runtime.Serialization;

namespace BLToolkit.TypeBuilder
{
	/// <summary>
	/// Defines the base class for the namespace exceptions.
	/// </summary>
	/// <remarks>
	/// This class is the base class for exceptions that may occur during execution of the namespace members.
	/// </remarks>
	[Serializable] 
	public class TypeBuilderException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <b>TypeBuilderException</b> class.
		/// </summary>
		/// <remarks>
		/// This constructor initializes the <para>Message</para> property of the new instance 
		/// to a system-supplied message that describes the error, 
		/// such as "An Rsdn Data error has occurred."
		/// </remarks>
		public TypeBuilderException() 
			: base("A Build Type exception has occurred.")
		{
		}
        
		/// <summary>
		/// Initializes a new instance of the <b>TypeBuilderException</b> class 
		/// with the specified error message.
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		public TypeBuilderException(string message) 
			: base(message) 
		{
		}
    	
		/// <summary>
		/// Initializes a new instance of the <b>TypeBuilderException</b> class 
		/// with the specified error message and InnerException property.
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		/// <param name="innerException">The InnerException, if any, that threw the current exception.</param>
		public TypeBuilderException(string message, Exception innerException) 
			: base(message, innerException) 
		{
		}

		/// <summary>
		/// Initializes a new instance of the <b>TypeBuilderException</b> class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
		protected TypeBuilderException(SerializationInfo info, StreamingContext context) 
			: base(info, context) 
		{
		}
	}
}
