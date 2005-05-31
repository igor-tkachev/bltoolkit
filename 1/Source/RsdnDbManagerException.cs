/*
 * File:    RsdnDbManagerException.cs
 * Created: 10/17/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Runtime.Serialization;

namespace Rsdn.Framework.Data
{
	/// <summary>
	/// The exception that is thrown by <see cref="DbManager"/>.
	/// </summary>
	/// <remarks>
	/// <b>RsdnDbManagerException</b> is used as a wrapper of any exceptions,
	/// which may occur during the the execution of the <see cref="DbManager"/> class methods.
	/// </remarks>
	[Serializable] 
	public class RsdnDbManagerException : RsdnDataException
	{
		/// <summary>
		/// Initializes a new instance of the <b>RsdnDbManagerException</b> class.
		/// </summary>
		/// <remarks>
		/// This constructor initializes the <para>Message</para> property of the new instance 
		/// to a system-supplied message that describes the error, 
		/// such as "An Rsdn Data error has occurred."
		/// </remarks>
		public RsdnDbManagerException() 
		{
		}
        
		/// <summary>
		/// Initializes a new instance of the <b>RsdnDbManagerException</b> class 
		/// with the specified error message.
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		public RsdnDbManagerException(string message) 
			: base(message) 
		{
		}
    	
		/// <summary>
		/// Initializes a new instance of the <b>RsdnDbManagerException</b> class 
		/// with the specified error message and InnerException property.
		/// </summary>
		/// <param name="message">The message to display to the client when the exception is thrown.</param>
		/// <param name="innerException">The InnerException, if any, that threw the current exception.</param>
		public RsdnDbManagerException(string message, Exception innerException) 
			: base(message, innerException) 
		{
		}

		/// <summary>
		/// Initializes a new instance of the <b>RsdnDbManagerException</b> class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
		protected RsdnDbManagerException(SerializationInfo info, StreamingContext context) 
			: base(info,context) 
		{
		}
	}
}
