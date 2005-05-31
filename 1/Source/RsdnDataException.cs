/*
 * File:    RsdnDataException.cs
 * Created: 12/27/2002
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Runtime.Serialization;

namespace Rsdn.Framework.Data
{
	/// <summary>
	/// Defines the base class for the namespace exceptions.
	/// </summary>
	/// <remarks>
	/// This class is the base class for exceptions that may occur during execution of the namespace members.
	/// </remarks>
	[Serializable] 
	public abstract class RsdnDataException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <b>RsdnDataException</b> class.
		/// </summary>
		/// <remarks>
		/// This constructor initializes the <para>Message</para> property of the new instance 
		/// to a system-supplied message that describes the error, 
		/// such as "An Rsdn Data error has occurred."
		/// </remarks>
		public RsdnDataException() 
			: base("An Rsdn Data error has occurred.")
		{
		}
        
		/// <summary>
		/// Initializes a new instance of the <b>RsdnDataException</b> class 
		/// with the specified error message.
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		public RsdnDataException(string message) 
			: base(message) 
		{
		}
    	
		/// <summary>
		/// Initializes a new instance of the <b>RsdnDataException</b> class 
		/// with the specified error message and InnerException property.
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		/// <param name="innerException">The InnerException, if any, that threw the current exception.</param>
		public RsdnDataException(string message, Exception innerException) 
			: base(message, innerException) 
		{
		}

		/// <summary>
		/// Initializes a new instance of the <b>RsdnDataException</b> class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
		protected RsdnDataException(SerializationInfo info, StreamingContext context) 
			: base(info,context) 
		{
		}
	}
}
