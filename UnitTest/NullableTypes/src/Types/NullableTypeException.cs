//
// NullableTypes.NullableTypeException
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes      Reasons
// 17-Feb-2003  Luca    Create
// 08-May-2003  Luca    Upgrade      New requirement: exception serialization
// 27-Jun-2003  Luca    Refactoring  Unified equivalent error messages 
// 07-Jul-2003  Luca    Upgrade      Applied FxCop guideline: add default constructor
// 06-Oct-2003  Luca    Upgrade      Code upgrade: Replaced tabs with spaces
//

namespace NullableTypes {

    using sys = System;
    using sysSrl = System.Runtime.Serialization;

    /// <summary>
    /// The base exception class for the <see cref="NullableTypes"/> exceptions.
    /// </summary>
    [sys.Serializable]            
    public class NullableTypeException : sys.SystemException, sysSrl.ISerializable
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableTypeException"/> 
        /// class with a specified error message.
        /// </summary>
        public NullableTypeException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableTypeException"/> 
        /// class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the 
        /// exception.</param>
        public NullableTypeException(string message) : base (message) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableTypeException"/> 
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
        public NullableTypeException(string message, sys.Exception innerException) 
            : base (message, innerException) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableTypeException"/> 
        /// class using the specified serialization information and streaming 
        /// context.
        /// </summary>
        /// <param name="info">A SerializationInfo structure.</param>
        /// <param name="context">A StreamingContext structure.</param>
        protected NullableTypeException(sysSrl.SerializationInfo info, sysSrl.StreamingContext context)
             : base(info, context)  {}


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
