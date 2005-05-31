//
// NullableTypes.TypeCode
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes      Reasons
// 27-Jun-2003  Luca    Create       New requirements by Roni Burd
//

namespace NullableTypes {
        /// <summary>
        /// Specifies the type of a NullableTypes structure.
        /// </summary>
        /// <remarks>
        /// Call the GetType method on a NullableTypes structure to obtain the 
        /// structure type code.
        /// </remarks>
        [System.Serializable]
        public enum TypeCode {

            ///<summary>
            ///Represents a boolean value that is either 
            ///<see cref="NullableTypes.NullableBoolean.True"/>, 
            ///<see cref="NullableTypes.NullableBoolean.False"/> or 
            ///<see cref="NullableTypes.NullableBoolean.Null"/>.
            ///</summary>
            NullableBoolean = 3,        // Boolean

            /// <summary>
            /// Represents a byte value that is either an 8-bit unsigned integer in the 
            /// range of 0 through 255 or <see cref="NullableTypes.NullableByte.Null"/>.
            /// </summary>
            NullableByte = 6,           // Byte

            /// <summary>
            /// Represents an Int16 that is either a 16-bit signed integer or 
            /// <see cref="NullableTypes.NullableInt16.Null"/>.
            /// </summary>
            NullableInt16 = 7,          // Signed 16-bit integer

            /// <summary>
            /// Represents an Int32 that is either a 32-bit signed integer or 
            /// <see cref="NullableTypes.NullableInt32.Null"/>.
            /// </summary>           
            NullableInt32 = 9,          // Signed 32-bit integer

            /// <summary>
            /// Represents an Int64 that is either a 64-bit signed integer or 
            /// <see cref="NullableTypes.NullableInt64.Null"/>.
            /// </summary>
            NullableInt64 = 11,         // Signed 64-bit integer

            /// <summary>
            /// Represents a Single value that is either a single-precision floating point 
            /// number or <see cref="NullableTypes.NullableSingle.Null"/>.
            /// </summary>
            NullableSingle = 13,        // IEEE 32-bit float

            /// <summary>
            /// Represents a Double value that is either a double-precision floating point 
            /// number or <see cref="NullableTypes.NullableDouble.Null"/>.
            /// </summary>
            NullableDouble = 14,        // IEEE 64-bit double

            /// <summary>
            /// Represents a Decimal that is either a decimal number value or 
            /// <see cref="NullableTypes.NullableDecimal.Null"/>.
            /// </summary>
            NullableDecimal = 15,       // Decimal
            
            /// <summary>
            /// Represents a DateTime that is either a date and time data or 
            /// <see cref="NullableTypes.NullableDateTime.Null"/>.
            /// </summary>
            NullableDateTime = 16,      // DateTime

            /// <summary>
            /// Represents a String that is either a variable-length stream of characters
            /// or <see cref="NullableTypes.NullableString.Null"/>.
            /// </summary>
            NullableString = 18        // Unicode character string
        }
    }

