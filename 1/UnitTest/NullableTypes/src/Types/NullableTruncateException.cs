//
// NullableTypes.NullableTruncateException
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes      Reasons
// 17-Feb-2003  Luca    Created
// 08-May-2003  Luca    New feature  Missing Serialization
//

namespace NullableTypes {

    using sys = System;
    using sysSrl = System.Runtime.Serialization;

	/// <summary>
	/// The exception that is thrown when setting a value into a NullableTypes 
	/// structure would truncate that value.
	/// </summary>
	[sys.Serializable]
	public class NullableTruncateException : NullableTypeException, sysSrl.ISerializable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NullableTruncateException"/>
		/// class with default properties.
		/// </summary>
		public NullableTruncateException()
			: base (Locale.GetText("Numeric arithmetic causes truncation.")) {}


		/// <summary>
		/// Initializes a new instance of the <see cref="NullableTruncateException"/> 
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">
		/// The error message that explains the reason for the exception.
		/// </param>
		public NullableTruncateException(string message) : base (message) {}


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableTruncateException"/> 
        /// class with a specified error message and a reference to the inner 
        /// exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the 
        /// exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception. If the 
        /// <paramref name="innerException"/> parameter is not a null reference 
        /// (Nothing in Visual Basic), the current exception is raised in a catch 
        /// block that handles the inner exception. 
        /// </param>
        public NullableTruncateException(string message, sys.Exception innerException) 
            : base (message, innerException) {}


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableTruncateException"/> 
        /// class using the specified serialization information and streaming 
        /// context.
        /// </summary>
        /// <param name="info">A SerializationInfo structure.</param>
        /// <param name="context">A StreamingContext structure.</param>
        protected NullableTruncateException(sysSrl.SerializationInfo info, sysSrl.StreamingContext context)
            : base(info, context) {}


        #region ISerializable
        /// <summary>
        /// When overridden in a derived class, sets the 
        /// <see cref="System.Runtime.Serialization.SerializationInfo"/> with 
        /// information about the exception.
        /// </summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo"/> that 
        /// holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext"/> that 
        /// contains contextual information about the source or destination. 
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is a null reference 
        /// (Nothing in Visual Basic).
        /// </exception>
        /// <remarks>
        /// GetObjectData sets a 
        /// <see cref="System.Runtime.Serialization.SerializationInfo"/> with all 
        /// the exception object data targeted for serialization. During 
        /// deserialization, the exception is reconstituted from the 
        /// <see cref="System.Runtime.Serialization.SerializationInfo"/> transmitted 
        /// over the stream.
        /// </remarks>
        void sysSrl.ISerializable.GetObjectData(sysSrl.SerializationInfo info, sysSrl.StreamingContext context) {
            base.GetObjectData(info, context);
        }
        #endregion
	}
}
