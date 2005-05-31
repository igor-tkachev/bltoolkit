//
// NullableTypes.NullableBoolean
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Damien Guard (damienguad@users.sourceforge.net)
//
// Date         Author  Changes      Reasons
// 10-Mar-2003  Luca    Create        
// 22-Apr-2003  Luca    Create       XML documentation
// 03-Jun-2003  Luca    Upgrade      Memory consumption reduced removing _notNull field
// 27-Jun-2003  Luca    Refactoring  Unified equivalent error messages 
// 27-Jun-2003  Luca    Upgrade      New requirement by Roni Burd: GetTypeCode 
// 30-Jun-2003  Luca    Upgrade      Removed NullableMoney and NullableGuid
// 07-Jul-2003  Luca    Upgrade      Applied FxCop guideline: narrowed exceptions catch in Parse method
// 07-Jul-2003  Luca    Upgrade      Applied FxCop guideline: explicited CurrentCulture in CompareTo,
//                                   ToNullableString, Parse and ToString code
// 12-Sep-2003  Luca    Upgrade      New requirement: the type must be sarializable
// 18-Sep-2003  Luca    Upgrade      New requirement: Operators & and | and methods And and Or must 
//                                   implement the correct NULL semantic
// 05-Oct-2003  DamienG Upgrade      New requirement: the type must be XmlSerializable
// 06-Oct-2003  Luca    Upgrade      Code upgrade: Replaced tabs with spaces and removed commented out code
// 22-Oct-2003  Luca    Upgrade      New requirement: Xor and operator^ marked 'obsolete'
// 06-Dic-2003  Luca    Bug Fix      Replaced Xml Schema id "NullableBooleanXmlSchema" with "NullableBoolean"
//                                   because VSDesigner use it as type-name in the auto-generated WS client Proxy
// 06-Dic-2003  Luca    Bug Fix      Replaced Target Namespace for Xml Schema to avoid duplicate namespace with
//                                   other types in NullableTypes
// 08-Dic-2003  Luca    Bug Fix      Replaced ReadString() with ReadElementString() in IXmlSerializable.ReadXml
// 18-Feb-2004  Luca    Bug Fix      ReadXml must read nil value also with a non empty element
//

namespace NullableTypes {
    using sys = System;
    using sysGlb = System.Globalization;
    using sysXml = System.Xml;
    using sysXmlSrl = System.Xml.Serialization;
    using sysXmlScm = System.Xml.Schema;

    /// <summary>
    /// Represents a boolean value that is either <see cref="True"/>, 
    /// <see cref="False"/> or <see cref="Null"/>.
    /// </summary>
    /// <remarks>
    /// Any non-zero value is interpreted as 1 that is <see cref="True"/>; 0 is 
    /// <see cref="False"/>.
    /// </remarks>
    [sys.Serializable]
    public struct NullableBoolean : INullable, sys.IComparable, sysXmlSrl.IXmlSerializable {

        #region Fields
        private sbyte _value;        

        /// <summary>
        /// Represents a true value that can be assigned to the <see cref="Value"/> 
        /// property of an instance of the <see cref="NullableBoolean"/> structure.
        /// </summary>
        /// <remarks>
        /// The True field is a constant for the <see cref="NullableBoolean"/>
        ///  structure.
        /// </remarks>
        public static readonly NullableBoolean True = new NullableBoolean(true);

        /// <summary>
        /// Represents a false value that can be assigned to the <see cref="Value"/> property of an instance of the <see cref="NullableBoolean"/> 
        /// structure.
        /// </summary>
        /// <remarks>
        /// The False field is a constant for the <see cref="NullableBoolean"/>
        ///  structure.
        /// </remarks>
        public static readonly NullableBoolean False = new NullableBoolean(false);

        /// <summary>
        /// Represents a 1 value that can be assigned to the <see cref="Value"/> 
        /// property of an instance of the <see cref="NullableBoolean"/> structure.
        /// </summary>
        /// <remarks>
        /// The One field is a constant for the <see cref="NullableBoolean"/>
        ///  structure.
        /// </remarks>
        public static readonly NullableBoolean One = new NullableBoolean(1);

        /// <summary>
        /// Represents a 0 value that can be assigned to the <see cref="Value"/> 
        /// property of an instance of the <see cref="NullableBoolean"/> structure.
        /// </summary>
        /// <remarks>
        /// The Zero field is a constant for the <see cref="NullableBoolean"/>
        ///  structure.
        /// </remarks>
        public static readonly NullableBoolean Zero = new NullableBoolean(0);

        /// <summary>
        /// Represents a Null value that can be assigned to the <see cref="Value"/> 
        /// property of an instance of the <see cref="NullableBoolean"/> structure.
        /// <para>
        /// A Null is the absence of a value because missing, unknown, or inapplicable 
        /// value. A Null should not be used to imply any other value (such as zero).
        /// Also any value (such as zero) should not be used to imply the absence of a 
        /// value, that's why Null exists.
        /// </para>
        /// </summary>
        /// <remarks>
        /// The Null field is a constant for the <see cref="NullableBoolean"/>
        ///  structure.
        /// </remarks>
        public static readonly NullableBoolean Null;
        #endregion // Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NullableBoolean"/> 
        /// structure using the supplied boolean value.
        /// </summary>
        /// <param name="value">
        /// The value for the new NullableBoolean structure; either true or false.
        /// </param>
        public NullableBoolean(bool value) {
            _value = (sbyte)(value ? 1 : -1);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableBoolean"/> 
        /// structure using the supplied integer value.
        /// </summary>
        /// <param name="value">
        /// The integer whose value is to be used for the new 
        /// <see cref="NullableBoolean"/> structure.
        /// </param>
        public NullableBoolean(int value) {
            _value = (sbyte)(value != 0 ? 1 : -1);
        }
        #endregion // Constructors

        #region INullable
        /// <summary>
        /// Indicates whether or not the value of the <see cref="NullableBoolean"/> 
        /// structure is null.
        /// </summary>
        /// <value>
        /// true if the <see cref="NullableBoolean"/> structure is null, otherwise 
        /// false.
        /// </value>
        public bool IsNull {
            get { 
                return (_value == 0);
            }
        }
        #endregion // INullable

        #region IComparable
        /// <summary>
        /// Compares this <see cref="NullableBoolean"/> structure to a specified 
        /// object and returns an indication of their relative values.
        /// </summary>
        /// <param name="value">
        /// An object to compare, or a null reference (Nothing in Visual Basic). 
        /// </param>
        /// <returns>
        /// A signed number indicating the relative values of the instance and value.
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Value</term>
        ///             <description>Description</description>
        ///         </listheader>
        ///         <item>
        ///             <term>A negative integer</term>
        ///             <description>This instance is less than <paramref name="value"/>.</description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>This instance is equal to <paramref name="value"/>.</description>
        ///         </item>
        ///         <item>
        ///             <term>A positive integer</term>
        ///             <description>
        ///             This instance is greater than <paramref name="value"/>.
        ///             <para>-or-</para>
        ///             value is a null reference (Nothing in Visual Basic) or 
        ///             <see cref="Null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <remarks>
        /// Any instance of <see cref="NullableBoolean"/> , regardless of its <paramref name="value"/>, 
        /// is considered greater than a null reference (Nothing in Visual Basic) and
        /// <see cref="Null"/>.
        /// </remarks>
        /// <exception cref="System.ArgumentException">
        /// The <paramref name="value"/> is neither null or of type <see cref="NullableBoolean"/>.
        /// </exception>
        public int CompareTo (object value) {
            if (value == null)
                return 1;

            if (!(value is NullableBoolean))
                throw new sys.ArgumentException(string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText("Value is not a {0}."),"NullableTypes.NullableBoolean"));

            NullableBoolean iValue = (NullableBoolean)value;

            if (iValue.IsNull && this.IsNull)
                return 0;

            if (iValue.IsNull)
                return 1;
            
            if (this.IsNull) 
                return -1;

            return this.ByteValue.CompareTo(iValue.ByteValue);
        }
        #endregion // IComparable

        #region IXmlSerializable
        /// <summary>
        /// This member supports the .NET Framework infrastructure and is not intended to be used directly 
        /// from your code.
        /// </summary>
        [sys.ComponentModel.EditorBrowsable(sys.ComponentModel.EditorBrowsableState.Never)]
        sysXml.Schema.XmlSchema sysXmlSrl.IXmlSerializable.GetSchema() {
            //      <?xml version="1.0"?>
            //      <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
            //            targetNamespace="http://NullableTypes.SourceForge.Net/NullableBooleanXMLSchema"
            //            xmlns="http://NullableTypes.SourceForge.Net/NullableBooleanXMLSchema"
            //            elementFormDefault="qualified">
            //        <xs:element name="NullableBoolean" type="xs:boolean" nillable="true" />
            //    </xs:schema>

            // Element: NullableBoolean
            sysXmlScm.XmlSchemaElement rootElement = new sysXmlScm.XmlSchemaElement();
            rootElement.Name = "NullableBoolean";
            rootElement.SchemaTypeName = 
                new sysXml.XmlQualifiedName("boolean", "http://www.w3.org/2001/XMLSchema");
            rootElement.IsNillable = true;

            // Xml Schema
            sysXmlScm.XmlSchema xsd = new sysXmlScm.XmlSchema();
            xsd.Id = "NullableBoolean";
            xsd.Namespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
            xsd.TargetNamespace = "http://NullableTypes.SourceForge.Net/NullableBooleanXMLSchema";
            xsd.Items.Add(rootElement);
            xsd.ElementFormDefault = sysXmlScm.XmlSchemaForm.Qualified;

//            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
//                xsd.Write(stream);
//
//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));
//            }

            return xsd;
        }


        /// <summary>
        /// This member supports the .NET Framework infrastructure and is not intended to be used directly 
        /// from your code.
        /// </summary>
        [sys.ComponentModel.EditorBrowsable(sys.ComponentModel.EditorBrowsableState.Never)]
        void sysXmlSrl.IXmlSerializable.WriteXml(sysXml.XmlWriter writer) {
            if (!IsNull) {
                writer.WriteString(sysXml.XmlConvert.ToString(_value == 1));
            }
            else
                writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
        }


        /// <summary>
        /// This member supports the .NET Framework infrastructure and is not intended to be used directly 
        /// from your code.
        /// </summary>
        [sys.ComponentModel.EditorBrowsable(sys.ComponentModel.EditorBrowsableState.Never)]
        void sysXmlSrl.IXmlSerializable.ReadXml(sysXml.XmlReader reader) {
            string nilValue = reader["nil", "http://www.w3.org/2001/XMLSchema-instance"];
            string elementValue = reader.IsEmptyElement ? null : reader.ReadElementString();

            if (nilValue == null || nilValue == "false" || nilValue == "0") {
                _value = (sbyte)(sysXml.XmlConvert.ToBoolean(elementValue) ? 1 : -1);
            }
            else
                _value = 0;
        }
        #endregion // IXmlSerializable

        #region Properties
        /// <summary>
        /// Gets the <see cref="NullableBoolean"/> structure's value. 
        /// This property is read-only.
        /// </summary>
        /// <value>
        /// true if the <see cref="NullableBoolean"/> is True; otherwise false.
        /// </value>
        /// <exception cref="NullableNullValueException">
        /// The property is set to null.
        /// </exception>
        public bool Value {
            get { 
                if (this.IsNull)
                    throw new NullableNullValueException();
                else
                    return this.IsTrue;
            }
        }


        /// <summary>
        /// Gets the value of the <see cref="NullableBoolean"/> structure as a byte.
        /// </summary>
        /// <value>
        /// A byte representing the value of the <see cref="NullableBoolean"/> 
        /// structure.
        /// </value>
        /// <remarks>The byte value will be either 0 or 1.</remarks>
        /// <exception cref="NullableNullValueException">
        /// The property is set to null.
        /// </exception>
        public byte ByteValue {
            get {
                switch(_value) {
                    case 0:
                        throw new NullableNullValueException();
                    case 1:
                        return 1;
                    default:
                        return 0;
                }
            }
        }


        /// <summary>
        /// Indicates whether the current <see cref="Value"/> is <see cref="False"/>.
        /// </summary>
        /// <value>
        /// true if <see cref="Value"/> is <see cref="False"/>, otherwise false.
        /// </value>
        /// <remarks>
        /// If the <see cref="Value"/> is <see cref="Null"/>, this property still 
        /// will be false.
        /// </remarks>
        public bool IsFalse {
            get { 
                return (_value == -1);
            }
        }


        /// <summary>
        /// Indicates whether the current <see cref="Value"/> is <see cref="True"/>.
        /// </summary>
        /// <value>
        /// true if <see cref="Value"/> is <see cref="True"/>, otherwise false.
        /// </value>
        /// <remarks>
        /// If the <see cref="Value"/> is <see cref="Null"/>, this property still 
        /// will be false.
        /// </remarks>
        public bool IsTrue {
            get { 
                return (_value == 1);
            }
        }
        #endregion // Properties

        #region Equivalence
        /// <summary>
        /// Compares two <see cref="NullableBoolean"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="value">The object to be compared. </param>
        /// <returns>
        /// true if object is an instance of <see cref="NullableBoolean"/> and the 
        /// two are equivalent; otherwise false.
        /// </returns>
        public override bool Equals(object value) {
            if (!(value is NullableBoolean))
                return false;
            if (this.IsNull && ((NullableBoolean)value).IsNull)
                return true;
            if (((NullableBoolean)value).IsNull || this.IsNull)
                return false;

            return (bool) (this == (NullableBoolean)value);
        }


        /// <summary>
        /// Compares two <see cref="NullableBoolean"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <param name="y">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is <see cref="True"/> if the two 
        /// instances are equivalent or <see cref="False"/> if the two instances are 
        /// not equivalent. If either instance of <see cref="NullableBoolean"/> is 
        /// <see cref="Null"/>, the <see cref="Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="Null"/>.
        /// </returns>
        public static NullableBoolean Equals(NullableBoolean x, NullableBoolean y) {
            return (x == y);
        }


        /// <summary>
        /// Compares two <see cref="NullableBoolean"/> structures to determine if 
        /// they are not equivalent.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <param name="y">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is <see cref="False"/> if the two 
        /// instances are equivalent or <see cref="True"/> if the two instances are 
        /// not equivalent. If either instance of <see cref="NullableBoolean"/> is 
        /// <see cref="Null"/>, the <see cref="Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="Null"/>.
        /// </returns>
        public static NullableBoolean NotEquals(NullableBoolean x, NullableBoolean y) {
            return (x != y);
        }


        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() {
            return _value;
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableBoolean"/> for equivalence.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <param name="y">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is <see cref="True"/> if the two 
        /// instances are equivalent or <see cref="False"/> if the two instances are 
        /// not equivalent. If either instance of <see cref="NullableBoolean"/> is 
        /// <see cref="Null"/>, the <see cref="Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="Null"/>.
        /// </returns>
        public static NullableBoolean operator == (NullableBoolean x, NullableBoolean y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;
            else
                return new NullableBoolean(x.Value == y.Value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableBoolean"/> for equivalence.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <param name="y">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is <see cref="False"/> if the two 
        /// instances are equivalent or <see cref="True"/> if the two instances are 
        /// not equivalent. If either instance of <see cref="NullableBoolean"/> is 
        /// <see cref="Null"/>, the <see cref="Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="Null"/>.
        /// </returns>
        public static NullableBoolean operator != (NullableBoolean x, NullableBoolean y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;
            else
                return new NullableBoolean(x.Value != y.Value);
        }        
        #endregion // Equivalence

        #region Methods
        /// <summary>
        /// Returns the <see cref="TypeCode"/> for value type 
        /// <see cref="NullableBoolean"/>.
        /// </summary>
        /// <returns>
        /// The enumerated constant, <see cref="TypeCode.NullableBoolean"/>.
        /// </returns>
        public TypeCode GetTypeCode() {
            return TypeCode.NullableBoolean;
        }


        /// <summary>
        /// Computes the bitwise AND of two specified <see cref="NullableBoolean"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <param name="y">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// <see cref="False"/> if either instance of <see cref="NullableBoolean"/> is 
        /// <see cref="False"/>,  <see cref="True"/> if both instances are <see cref="True"/> 
        /// otherwise <see cref="Null"/>.
        /// </returns>

        public static NullableBoolean And (NullableBoolean x, NullableBoolean y) {
            return (x & y);
        }


        /// <summary>
        /// Performs a one's complement operation on the supplied 
        /// <see cref="NullableBoolean"/> structure.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is <see cref="Null"/> 
        /// otherwise the one's complement of <paramref name="x"/>.
        /// </returns>
        public static NullableBoolean OnesComplement(NullableBoolean x) {
            return ~x;
        }


        /// <summary>
        /// Performs a bitwise OR operation on the two specified SqlBoolean structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <param name="y">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// <see cref="True"/> if either instance of <see cref="NullableBoolean"/> is 
        /// <see cref="True"/>,  <see cref="False"/> if both instances are <see cref="False"/> 
        /// otherwise <see cref="Null"/> .
        /// </returns>
        public static NullableBoolean Or(NullableBoolean x, NullableBoolean y) {
            return (x | y);
        }


        /// <summary>
        /// Performs a bitwise exclusive-OR operation on the supplied parameters.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <param name="y">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableBoolean"/> is 
        /// <see cref="Null"/> otherwise the result of the logical XOR operation.
        /// </returns>
        [sys.Obsolete("Xor definition is not intuitive, is ambiguous and is not defined by the NULL semantic. " +
                  "Use explicit expression ((x || y) && !(x && y)) instead of Xor.")]
        public static NullableBoolean Xor(NullableBoolean x, NullableBoolean y) {
            return (x ^ y);
        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a 
        /// logical value to its <see cref="NullableBoolean"/> equivalent.
        /// </summary>
        /// <param name="s">The <see cref="System.String"/> to be converted.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure containing the parsed value.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="s"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="s"/>is not equivalent to <see cref="bool.TrueString"/> or 
        /// <see cref="bool.FalseString"/> or <paramref name="s"/> does not consist 
        /// solely of an optional negative sign followed by a sequence of digits 
        /// ranging from 0 to 9 or <paramref name="s"/> represents a number less than 
        /// <see cref="int.MinValue"/> or greater than <see cref="int.MaxValue"/>.
        /// </exception>
        public static NullableBoolean Parse(string s) {
            if (s == null) throw new sys.ArgumentNullException("s");

            NullableBoolean r;
            try {
                r = new NullableBoolean(int.Parse(s, sysGlb.CultureInfo.CurrentCulture));
                return r;
            }
            catch(sys.FormatException) {/* ignore */}
            catch(sys.OverflowException) {/* ignore */}

            r = new NullableBoolean(bool.Parse(s));
            return r;
        }
        #endregion // Methods

        #region Operators
        /// <summary>
        /// Computes the bitwise AND of two specified <see cref="NullableBoolean"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <param name="y">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// <see cref="False"/> if either instance of <see cref="NullableBoolean"/> is 
        /// <see cref="False"/>,  <see cref="True"/> if both instances are <see cref="True"/> 
        /// otherwise <see cref="Null"/> .
        /// </returns>
        public static NullableBoolean operator & (NullableBoolean x, NullableBoolean y) {
            if (x.IsFalse || y.IsFalse)
                return NullableBoolean.False;

            if (x.IsTrue && y.IsTrue)
                return NullableBoolean.True;

            return NullableBoolean.Null;
        }


        /// <summary>
        /// Performs a bitwise OR operation on the two specified SqlBoolean structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <param name="y">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// <see cref="True"/> if either instance of <see cref="NullableBoolean"/> is 
        /// <see cref="True"/>,  <see cref="False"/> if both instances are <see cref="False"/> 
        /// otherwise <see cref="Null"/> .
        /// </returns>
        public static NullableBoolean operator | (NullableBoolean x, NullableBoolean y) {
            if (x.IsTrue || y.IsTrue)
                return NullableBoolean.True;

            if (x.IsFalse && y.IsFalse)
                return NullableBoolean.False;

            return NullableBoolean.Null;
        }


        /// <summary>
        /// Performs a bitwise exclusive-OR operation on the supplied parameters.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <param name="y">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableBoolean"/> is 
        /// <see cref="Null"/> otherwise the result of the logical XOR operation.
        /// </returns>
        [sys.Obsolete("Operator^ is not intuitive, is ambiguous and is not defined by the NULL semantic. " +
             "Use explicit expression ((x || y) && !(x && y)) instead of operator^.", false)]
        public static NullableBoolean operator ^ (NullableBoolean x, NullableBoolean y) {
            if (x.IsNull || y.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean(x.Value ^ y.Value);
        }


        /// <summary>
        /// The false operator can be used to test the <see cref="Value"/> of the 
        /// <see cref="NullableBoolean"/> to determine whether it is false.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// true if <see cref="Value"/> is <see cref="False"/>, otherwise false.
        /// </returns>
        /// <remarks>
        /// If the <see cref="Value"/> is <see cref="Null"/>, this operator still 
        /// return false.
        /// </remarks>
        public static bool operator false (NullableBoolean x) {
            return x.IsFalse;
        }


        /// <summary>
        /// Performs a NOT operation on the supplied <see cref="NullableBoolean"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is <see cref="Null"/> 
        /// otherwise the logical NOT of <paramref name="x"/>.
        /// </returns>
        public static NullableBoolean operator ! (NullableBoolean x) {
            if (x.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean(!x.Value);
        }


        /// <summary>
        /// Performs a one's complement operation on the supplied 
        /// <see cref="NullableBoolean"/> structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is <see cref="Null"/> 
        /// otherwise the one's complement of <paramref name="x"/>.
        /// </returns>
        public static NullableBoolean operator ~ (NullableBoolean x) {
            return !x;
        }


        /// <summary>
        /// The true operator can be used to test the <see cref="Value"/> of the 
        /// <see cref="NullableBoolean"/> to determine whether it is true.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> structure.</param>
        /// <returns>
        /// true if <see cref="Value"/> is <see cref="True"/>, otherwise false.
        /// </returns>
        /// <remarks>
        /// If the <see cref="Value"/> is <see cref="Null"/>, this operator still 
        /// return false.
        /// </remarks>
        public static bool operator true (NullableBoolean x) {
            return x.IsTrue;
        }
        #endregion // Operators

        #region Conversion Operators
        /// <summary>
        /// Converts a <see cref="NullableBoolean"/> to a 
        /// <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="x">A <see cref="NullableBoolean"/> to convert.</param>
        /// <returns>
        /// A <see cref="System.Boolean"/> set to the <see cref="Value"/> of the 
        /// <see cref="NullableBoolean"/>.
        /// </returns>
        /// <exception cref="NullableNullValueException">
        /// <paramref name="x"/> is <see cref="Null"/>.
        /// </exception>
        public static explicit operator bool(NullableBoolean x) {
            return x.Value;
        }
        

        /// <summary>
        /// Converts the <see cref="NullableByte"/> parameter to a 
        /// <see cref="NullableBoolean"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableByte"/> to be converted to a 
        /// <see cref="NullableBoolean"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableByte.Null"/> otherwise a new 
        /// <see cref="NullableBoolean"/> structure constructed from the 
        /// <see cref="NullableByte.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableBoolean(NullableByte x) {
            if (x.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean(x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableDecimal"/> parameter to a 
        /// <see cref="NullableBoolean"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDecimal"/> to be converted to a 
        /// <see cref="NullableBoolean"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableDecimal.Null"/>; <see cref="False"/> if 
        /// <paramref name="x"/> <see cref="NullableDecimal.Value"/> is 
        /// <see cref="System.Decimal.Zero"/>; <see cref="True"/> otherwise.
        /// </returns>
        public static explicit operator NullableBoolean(NullableDecimal x) {
            if (x.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean(x.Value != sys.Decimal.Zero);
        }
        

        /// <summary>
        /// Converts the <see cref="NullableDouble"/> parameter to a 
        /// <see cref="NullableBoolean"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDouble"/> to be converted to a 
        /// <see cref="NullableBoolean"/> structure.
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableDouble.Null"/>; <see cref="False"/> if 
        /// <paramref name="x"/> <see cref="NullableDouble.Value"/> is 
        /// <see cref="NullableDouble.Zero"/>; <see cref="True"/> otherwise.
        /// </returns>
        public static explicit operator NullableBoolean(NullableDouble x) {
            if (x.IsNull)
                return NullableBoolean.Null;

            return x != NullableDouble.Zero;
        }


        /// <summary>
        /// Converts the <see cref="NullableInt16"/> parameter to a 
        /// <see cref="NullableBoolean"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt16"/> to be converted to a 
        /// <see cref="NullableBoolean"/> structure.
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt16.Null"/> otherwise a new 
        /// <see cref="NullableBoolean"/> structure constructed from the 
        /// <see cref="NullableInt16.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableBoolean(NullableInt16 x) {
            if (x.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean((int)x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableInt32"/> parameter to a 
        /// <see cref="NullableBoolean"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt32"/> to be converted to a 
        /// <see cref="NullableBoolean"/> structure.
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt32.Null"/> otherwise a new 
        /// <see cref="NullableBoolean"/> structure constructed from the 
        /// <see cref="NullableInt32.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableBoolean(NullableInt32 x) {
            if (x.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean(x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableInt64"/> parameter to a 
        /// <see cref="NullableBoolean"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt64"/> to be converted to a 
        /// <see cref="NullableBoolean"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt64.Null"/>; <see cref="False"/> if 
        /// <paramref name="x"/> <see cref="NullableInt64.Value"/> is 
        /// <see cref="NullableInt64.Zero"/>; <see cref="True"/> otherwise.
        /// </returns>  
        public static explicit operator NullableBoolean(NullableInt64 x) {
            if (x.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean(x.Value != 0);
        }


        /// <summary>
        /// Converts the <see cref="NullableSingle"/> parameter to a 
        /// <see cref="NullableBoolean"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableSingle"/> to be converted to a 
        /// <see cref="NullableBoolean"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableSingle.Null"/>; <see cref="False"/> if 
        /// <paramref name="x"/> <see cref="NullableSingle.Value"/> is 
        /// <see cref="NullableSingle.Zero"/>; <see cref="True"/> otherwise.
        /// </returns>  
        public static explicit operator NullableBoolean(NullableSingle x) {
            if (x.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean(x.Value != 0);
        }


        /// <summary>
        /// Converts the <see cref="NullableString"/> parameter to a 
        /// see <see cref="NullableBoolean"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableString"/> to be converted to a 
        /// <see cref="NullableBoolean"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableString.Null"/>; otherwise a 
        /// <see cref="NullableBoolean"/> structure containing the parsed value
        /// of <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// <paramref name="s"/>is not equivalent to <see cref="bool.TrueString"/> or 
        /// <see cref="bool.FalseString"/> or <paramref name="s"/> does not consist 
        /// solely of an optional negative sign followed by a sequence of digits 
        /// ranging from 0 to 9.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="s"/> represents a number less than 
        /// <see cref="int.MinValue"/> or greater than <see cref="int.MaxValue"/>.
        /// </exception>
        public static explicit operator NullableBoolean(NullableString x) {
            if (x.IsNull)
                return NullableBoolean.Null;

            return NullableBoolean.Parse(x.Value);
        }


        /// <summary>
        /// Converts the supplied <see cref="System.Boolean"/> value to a 
        /// <see cref="NullableBoolean"/>.
        /// </summary>
        /// <param name="x">
        /// A <see cref="System.Boolean"/> value to be converted to 
        /// <see cref="NullableBoolean"/>. 
        /// </param>
        /// <returns>
        /// A non null <see cref="NullableBoolean"/> having <see cref="Value"/>
        /// equal to <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableBoolean(bool x) {
            return new NullableBoolean(x);
        }
        #endregion // Conversion Operators

        #region Conversions
        /// <summary>
        /// Converts this <see cref="NullableBoolean"/> structure to a 
        /// <see cref="System.String"/>.
        /// </summary>
        /// <returns>
        /// a new <see cref="System.String"/> that is a description of Null if this 
        /// <see cref="NullableBoolean"/> is <see cref="NullableBoolean.Null"/> 
        /// otherwise a new <see cref="System.String"/> that is the description of 
        /// this <see cref="NullableBoolean"/> structure's <see cref="Value"/>.
        /// </returns>
        public override string ToString() {
            if (this.IsNull)
                return Locale.GetText("Null");

            return (this.IsTrue).ToString(sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Converts this <see cref="NullableBoolean"/> structure to a 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <returns>
        /// <see cref="NullableDecimal.Null"/> if this <see cref="NullableBoolean"/>
        /// is <see cref="NullableBoolean.Null"/> otherwise
        /// a new <see cref="NullableDecimal"/> structure whose 
        /// <see cref="NullableDecimal.Value"/> equals 1 if this 
        /// <see cref="NullableBoolean"/> structure's <see cref="Value"/> is true
        /// or equals 0 if this <see cref="NullableBoolean"/> structure's 
        /// <see cref="Value"/> is false.
        /// </returns>
        public NullableDecimal ToNullableDecimal() {
            return ((NullableDecimal)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableBoolean"/> structure to a 
        /// <see cref="NullableDouble"/> structure.
        /// </summary>
        /// <returns>
        /// <see cref="NullableDouble.Null"/> if this <see cref="NullableBoolean"/>
        /// is <see cref="NullableBoolean.Null"/> otherwise
        /// a new <see cref="NullableDouble"/> structure whose 
        /// <see cref="NullableDouble.Value"/> equals 1 if this 
        /// <see cref="NullableBoolean"/> structure's <see cref="Value"/> is true
        /// or equals 0 if this <see cref="NullableBoolean"/> structure's 
        /// <see cref="Value"/> is false.
        /// </returns>
        public NullableDouble ToNullableDouble() {
            return ((NullableDouble)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableBoolean"/> structure to a 
        /// <see cref="NullableInt16"/> structure.
        /// </summary>
        /// <returns>
        /// <see cref="NullableInt16.Null"/> if this <see cref="NullableBoolean"/>
        /// is <see cref="NullableBoolean.Null"/> otherwise
        /// a new <see cref="NullableInt16"/> structure whose 
        /// <see cref="NullableInt16.Value"/> equals 1 if this 
        /// <see cref="NullableBoolean"/> structure's <see cref="Value"/> is true
        /// or equals 0 if this <see cref="NullableBoolean"/> structure's 
        /// <see cref="Value"/> is false.
        /// </returns>
        public NullableInt16 ToNullableInt16() {
            return ((NullableInt16)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableBoolean"/> structure to a 
        /// <see cref="NullableInt32"/> structure.
        /// </summary>
        /// <returns>
        /// <see cref="NullableInt32.Null"/> if this <see cref="NullableBoolean"/>
        /// is <see cref="NullableBoolean.Null"/> otherwise
        /// a new <see cref="NullableInt32"/> structure whose 
        /// <see cref="NullableInt32.Value"/> equals 1 if this 
        /// <see cref="NullableBoolean"/> structure's <see cref="Value"/> is true
        /// or equals 0 if this <see cref="NullableBoolean"/> structure's 
        /// <see cref="Value"/> is false.
        /// </returns>
        public NullableInt32 ToNullableInt32() {
            return ((NullableInt32)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableBoolean"/> structure to a 
        /// <see cref="NullableInt64"/> structure.
        /// </summary>
        /// <returns>
        /// <see cref="NullableInt64.Null"/> if this <see cref="NullableBoolean"/>
        /// is <see cref="NullableBoolean.Null"/> otherwise
        /// a new <see cref="NullableInt64"/> structure whose 
        /// <see cref="NullableInt64.Value"/> equals 1 if this 
        /// <see cref="NullableBoolean"/> structure's <see cref="Value"/> is true
        /// or equals 0 if this <see cref="NullableBoolean"/> structure's 
        /// <see cref="Value"/> is false.
        /// </returns>
        public NullableInt64 ToNullableInt64() {
            return ((NullableInt64)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableBoolean"/> structure to a 
        /// <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <returns>
        /// <see cref="NullableSingle.Null"/> if this <see cref="NullableBoolean"/>
        /// is <see cref="NullableBoolean.Null"/> otherwise
        /// a new <see cref="NullableSingle"/> structure whose 
        /// <see cref="NullableSingle.Value"/> equals 1 if this 
        /// <see cref="NullableBoolean"/> structure's <see cref="Value"/> is true
        /// or equals 0 if this <see cref="NullableBoolean"/> structure's 
        /// <see cref="Value"/> is false.
        /// </returns>
        public NullableSingle ToNullableSingle() {
            return ((NullableSingle)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableBoolean"/> structure to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <returns>
        /// <see cref="NullableString.Null"/> if this <see cref="NullableBoolean"/>
        /// is <see cref="NullableBoolean.Null"/> otherwise
        /// a new <see cref="NullableString"/> structure whose 
        /// <see cref="NullableString.Value"/> is the description of this 
        /// <see cref="NullableBoolean"/> structure's <see cref="Value"/>.
        /// </returns>
        public NullableString ToNullableString() {
            if (this.IsNull)
                return NullableString.Null;

            return new NullableString((this.IsTrue).ToString(sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Converts this <see cref="NullableBoolean"/> structure to 
        /// <see cref="NullableByte"/>.
        /// </summary>
        /// <returns>
        /// <see cref="NullableByte.Null"/> if this instance is <see cref="Null"/>;
        /// otherwise a <see cref="NullableByte"/> structure whose 
        /// <see cref="NullableByte.Value"/> equals the 
        /// <see cref="NullableBoolean.Value"/> of this <see cref="NullableBoolean"/> 
        /// structure. If the <see cref="NullableBoolean"/> structure's 
        /// <see cref="NullableBoolean.Value"/> is true, then the 
        /// <see cref="NullableByte"/> structure's <see cref="NullableByte.Value"/>
        /// will be 1, otherwise will be 0.
        /// </returns>
        public NullableByte ToNullableByte()    {
            return ((NullableByte)this);
        }
        #endregion // Conversions
    }
}