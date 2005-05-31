//
// NullableTypes.NullableByte
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author  Changes      Reasons
// 07-Apr-2003  Luca    Create
// 24-Apr-2003  Luca    Create       Added XML documentation
// 27-Jun-2003  Luca    Refactoring  Unified equivalent error messages 
// 30-Jun-2003  Luca    Upgrade      New requirement by Roni Burd: GetTypeCode 
// 30-Jun-2003  Luca    Upgrade      Removed NullableMoney and NullableGuid
// 07-Jul-2003  Luca    Upgrade      Applied FxCop guideline: explicited CurrentCulture in CompareTo, 
//                                   Parse and ToString code
// 12-Sep-2003  Luca    Upgrade      New requirement: the type must be sarializable
// 05-Oct-2003  DamienG Upgrade      New requirement: the type must be XmlSerializable
// 06-Oct-2003  Luca    Upgrade      Code upgrade: Replaced tabs with spaces and removed commented out code
// 07-Ott-2003  Luca    Upgrade      Applied FxCop guideline: in XmlWriter the call to ToString() changed 
//                                   a call to XmlConvert.ToString() and made symmetric call in XmlRead
// 06-Dic-2003  Luca    Bug Fix      Replaced Xml Schema id "NullableByteXmlSchema" with "NullableByte"
//                                   because VSDesigner use it as type-name in the auto-generated WS client Proxy
// 06-Dic-2003  Luca    Bug Fix      Replaced Target Namespace for Xml Schema to avoid duplicate namespace with
//                                   other types in NullableTypes
// 18-Feb-2004  Luca    Bug Fix      ReadXml must read nil value also with a non empty element
//

namespace NullableTypes {
    using sys = System;
    using sysGlb = System.Globalization;
    using sysXml = System.Xml;
    using sysXmlSrl = System.Xml.Serialization;
    using sysXmlScm = System.Xml.Schema;

    /// <summary>
    /// Represents a byte value that is either an 8-bit unsigned integer in the 
    /// range of 0 through 255 or <see cref="Null"/>.
    /// </summary>
    [sys.Serializable]
    public struct NullableByte: INullable, sys.IComparable,  sysXmlSrl.IXmlSerializable {

        #region Fields
        byte _value;
        private bool _notNull;

        /// <summary>
        /// A constant representing the largest possible value of a 
        /// <see cref="NullableByte"/>.
        /// </summary>
        /// <remarks>
        /// The value of this constant is 255 or, hexadecimal 0xFF.
        /// </remarks>
        public static readonly NullableByte MaxValue = new NullableByte(byte.MaxValue);

        /// <summary>
        /// A constant representing the smallest possible value of a 
        /// <see cref="NullableByte"/>.
        /// </summary>
        /// <remarks>The value of this constant is 0.</remarks>
        public static readonly NullableByte MinValue = new NullableByte(byte.MinValue);

        /// <summary>
        /// Represents a Null value that can be assigned to an instance of the 
        /// <see cref="NullableByte"/> structure.
        /// <para>
        /// A Null is the absence of a value because missing, unknown, or inapplicable 
        /// value. A Null should not be used to imply any other value (such as zero).
        /// Also any value (such as zero) should not be used to imply the absence of a 
        /// value, that's why Null exists.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Null field is a constant of the <see cref="NullableByte"/> structure.
        /// </remarks>
        public static readonly NullableByte Null;

        /// <summary>
        /// Represents a zero value that can be assigned to an instance of the 
        /// <see cref="NullableByte"/> structure.
        /// </summary>
        /// <remarks>
        /// Zero field is a constant of the <see cref="NullableByte"/> structure.
        /// </remarks>
        public static readonly NullableByte Zero = new NullableByte(0);
        #endregion // Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NullableByte"/> structure 
        /// using the specified byte value.
        /// </summary>
        /// <param name="value">
        /// A byte value to be stored in the <see cref="Value"/> property of the new 
        /// <see cref="NullableByte"/> structure. 
        /// </param>
        public NullableByte(byte value) {
            _value = value;
            _notNull = true;
        }
        #endregion // Constructors

        #region INullable
        /// <summary>
        /// Indicates whether or not the value of the <see cref="NullableByte"/> 
        /// structure is null.
        /// </summary>
        /// <value>
        /// true if the <see cref="NullableByte"/> structure is null, otherwise 
        /// false.
        /// </value>
        public bool IsNull {
            get {return !_notNull;}
        }
        #endregion // INullable

        #region IComparable Ordering
        /// <summary>
        /// Compares this <see cref="NullableByte"/> structure to a specified 
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
        ///             <description>
        ///             This instance is less than <paramref name="value"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>
        ///             This instance is equal to <paramref name="value"/>.
        ///             </description>
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
        /// Any instance of <see cref="NullableByte"/> , regardless of its 
        /// <paramref name="value"/>, is considered greater than a null reference 
        /// (Nothing  in Visual Basic) <see cref="Null"/>. 
        /// </remarks>
        /// <exception cref="System.ArgumentException">
        /// The <paramref name="value"/> is neither null or of type 
        /// <see cref="NullableByte"/>.
        /// </exception>
        public int CompareTo (object value) {
            if (value == null)
                return 1;
            
            if (!(value is NullableByte))
                throw new sys.ArgumentException(string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText("Value is not a {0}."), "NullableTypes.NullableByte"));
            
            NullableByte bValue = (NullableByte)value;
            if (bValue.IsNull && this.IsNull)
                return 0;

            if (bValue.IsNull)
                return 1;

            if (this.IsNull)
                return -1;
            
            return _value.CompareTo(bValue.Value);
        }
        #endregion // IComparable Ordering

        #region IXmlSerializable
        /// <summary>
        /// This member supports the .NET Framework infrastructure and is not intended to be used directly 
        /// from your code.
        /// </summary>
        [sys.ComponentModel.EditorBrowsable(sys.ComponentModel.EditorBrowsableState.Never)]
        sysXml.Schema.XmlSchema sysXmlSrl.IXmlSerializable.GetSchema() {
            //    <?xml version="1.0"?>
            //    <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
            //            targetNamespace="http://NullableTypes.SourceForge.Net/NullableByteXMLSchema"
            //            xmlns="http://NullableTypes.SourceForge.Net/NullableByteXMLSchema"
            //            elementFormDefault="qualified">
            //        <xs:element name="NullableByte" type="xs:unsignedByte" nillable="true" />
            //    </xs:schema>

            // Element: NullableByte
            sysXmlScm.XmlSchemaElement rootElement = new sysXmlScm.XmlSchemaElement();
            rootElement.Name = "NullableByte";
            rootElement.SchemaTypeName = 
                new sysXml.XmlQualifiedName("unsignedByte", "http://www.w3.org/2001/XMLSchema");
            rootElement.IsNillable = true;

            // Xml Schema
            sysXmlScm.XmlSchema xsd = new sysXmlScm.XmlSchema();
            xsd.Id = "NullableByte";
            xsd.Namespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
            xsd.TargetNamespace = "http://NullableTypes.SourceForge.Net/NullableByteXMLSchema";
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
            if (!IsNull) 
                writer.WriteString(sysXml.XmlConvert.ToString(_value));
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
                _value = sysXml.XmlConvert.ToByte(elementValue);
                _notNull = true;
            }
            else
                _notNull = false;
        }
        #endregion // IXmlSerializable

        #region Properties
        /// <summary>
        /// Gets the value of the <see cref="NullableByte"/> structure. This 
        /// property is read-only
        /// </summary>
        /// <value>The value of the <see cref="NullableByte"/> structure.</value>
        /// <exception cref="NullableNullValueException">
        /// The property is set to null.
        /// </exception>
        public byte Value { 
            get { 
                if (this.IsNull) 
                    throw new NullableNullValueException();

                return _value; 
            }
        }
        #endregion // Properties

        #region Equivalence
        /// <summary>
        /// Compares two <see cref="NullableByte"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="value">The object to be compared. </param>
        /// <returns>
        /// true if object is an instance of <see cref="NullableByte"/> and the 
        /// two instances are equivalent; otherwise false.
        /// </returns>
        public override bool Equals (object value) {
            if (!(value is NullableByte))
                return false;

            NullableByte bValue = (NullableByte)value;

            return (this._value == bValue._value) && 
                (this._notNull == bValue._notNull);
        }


        /// <summary>
        /// Compares two <see cref="NullableByte"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.False"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean Equals (NullableByte x, NullableByte y) {
            return (x == y);
        }

        /// <summary>
        /// Compares two <see cref="NullableByte"/> structures to determine if 
        /// they are not equivalent.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.False"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.True"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean NotEquals (NullableByte x, NullableByte y) {
            return (x != y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableByte"/> for equivalence.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.False"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.True"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean operator != (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;
            else
                return new NullableBoolean (!(x.Value == y.Value));
        }

        /// <summary>
        /// Compares two instances of <see cref="NullableByte"/> for equivalence.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.False"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean operator == (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;
            else
                return new NullableBoolean (x.Value == y.Value);
        }


        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode () {
            return (int)_value;
        }
        #endregion // Equivalence

        #region Methods
        /// <summary>
        /// Returns the <see cref="TypeCode"/> for value type 
        /// <see cref="NullableByte"/>.
        /// </summary>
        /// <returns>
        /// The enumerated constant, <see cref="TypeCode.NullableByte"/>.
        /// </returns>
        public TypeCode GetTypeCode() {
            return TypeCode.NullableByte;
        }

        /// <summary>
        /// Computes the sum of the two specified <see cref="NullableByte"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the results of the addition.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The addition compute a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static NullableByte Add(NullableByte x, NullableByte y) {
            return (x + y);
        }


        /// <summary>
        /// Computes the bitwise AND of the two specified <see cref="NullableByte"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the results of the bitwise 
        /// AND operation.
        /// </returns>
        public static NullableByte BitwiseAnd(NullableByte x, NullableByte y) {
            return (x & y);
        }


        /// <summary>
        /// Computes the bitwise OR of the two specified <see cref="NullableByte"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the results of the bitwise 
        /// OR operation.
        /// </returns>
        public static NullableByte BitwiseOr(NullableByte x, NullableByte y) {
            return (x | y);
        }


        /// <summary>
        /// Divides its first <see cref="NullableByte"/> operand by its second.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the results of the division.
        /// </returns>
        /// <exception cref="System.DivideByZeroException">
        /// <paramref name="y"/> is <see cref="Zero"/> while <paramref name="x"/> is 
        /// not <see cref="Null"/>.
        /// </exception>
        public static NullableByte Divide(NullableByte x, NullableByte y) {
            return (x / y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableByte"/> structure to 
        /// determine if the first is greater than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableByte"/> is <see cref="NullableByte.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean GreaterThan(NullableByte x, NullableByte y) {
            return (x > y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableByte"/> structure to 
        /// determine if the first is greater than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableByte"/> is <see cref="NullableByte.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean GreaterThanOrEqual(NullableByte x, NullableByte y) {
            return (x >= y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableByte"/> structure to 
        /// determine if the first is less than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableByte"/> is <see cref="NullableByte.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean LessThan(NullableByte x, NullableByte y) {
            return (x < y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableByte"/> structure to 
        /// determine if the first is less than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableByte"/> is <see cref="NullableByte.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean LessThanOrEqual(NullableByte x, NullableByte y) {
            return (x <= y);
        }


        /// <summary>
        /// Computes the remainder for the division of the two specified 
        /// <see cref="NullableByte"/> structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the remainder.
        /// </returns>
        public static NullableByte Mod(NullableByte x, NullableByte y) {
            return (x % y);
        }


        /// <summary>
        /// Computes the product of the two specified <see cref="NullableByte"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains  the product of the 
        /// multiplication.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The product  compute a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static NullableByte Multiply(NullableByte x, NullableByte y) {
            return (x * y);
        }


        /// <summary>
        /// Performs a one's complement operation on the supplied 
        /// <see cref="NullableByte"/> structure.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is <see cref="Null"/> 
        /// otherwise the one's complement of <paramref name="x"/>.
        /// </returns>
        public static NullableByte OnesComplement(NullableByte x) {
            return ~x;
        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a 
        /// byte to its <see cref="NullableByte"/> equivalent.
        /// </summary>
        /// <param name="s">The <see cref="System.String"/> to be converted.</param>
        /// <returns>
        /// A <see cref="NullableByte"/> structure containing the parsed value.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="s"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="s"/> does not consist solely of an optional sign followed 
        /// by a sequence of digits ranging from 0 to 9.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="s"/> represents a number less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static NullableByte Parse(string s) {
            return new NullableByte(byte.Parse(s, sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Computes the subtraction of the two specified <see cref="NullableByte"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the results of the 
        /// subtraction.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The subtraction compute a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static NullableByte Subtract(NullableByte x, NullableByte y) {
            return (x - y);
        }


        /// <summary>
        /// Computes the bitwise exclusive-OR of the two specified 
        /// <see cref="NullableByte"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the results of the bitwise 
        /// XOR operation.
        /// </returns>
        public static NullableByte Xor(NullableByte x, NullableByte y) {
            return (x ^ y);
        }
        #endregion // Methods

        #region Operators
        /// <summary>
        /// Computes the sum of the two specified <see cref="NullableByte"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the results of the addition.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The addition compute a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static NullableByte operator + (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull)
                return NullableByte.Null;

            checked {
                return new NullableByte((byte) (x.Value + y.Value));
            }
        }


        /// <summary>
        /// Computes the bitwise AND of the two specified <see cref="NullableByte"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the results of the bitwise 
        /// AND operation.
        /// </returns>
        public static NullableByte operator & (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull)
                return NullableByte.Null;

            return new NullableByte((byte)(x.Value & y.Value));
        }


        /// <summary>
        /// Computes the bitwise OR of the two specified <see cref="NullableByte"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the results of the bitwise 
        /// OR operation.
        /// </returns>
        public static NullableByte operator | (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull)
                return NullableByte.Null;

            return new NullableByte((byte)(x.Value | y.Value));
        }


        /// <summary>
        /// Divides its first <see cref="NullableByte"/> operand by its second.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the results of the division.
        /// </returns>
        /// <exception cref="System.DivideByZeroException">
        /// <paramref name="y"/> is <see cref="Zero"/> while <paramref name="x"/> is 
        /// not <see cref="Null"/>.
        /// </exception>
        public static NullableByte operator / (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull)
                return NullableByte.Null;

            return new NullableByte((byte) (x.Value / y.Value));
        }


        /// <summary>
        /// Computes the bitwise exclusive-OR of the two specified 
        /// <see cref="NullableByte"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the results of the bitwise 
        /// XOR operation.
        /// </returns>
        public static NullableByte operator ^ (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull)
                return NullableByte.Null;

            return new NullableByte((byte) (x.Value ^ y.Value));
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableByte"/> structure to 
        /// determine if the first is greater than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableByte"/> is <see cref="NullableByte.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean operator > (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean (x.Value > y.Value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableByte"/> structure to 
        /// determine if the first is greater than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableByte"/> is <see cref="NullableByte.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean operator >= (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;

            return new NullableBoolean (x.Value >= y.Value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableByte"/> structure to 
        /// determine if the first is less than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableByte"/> is <see cref="NullableByte.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean operator < (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;

            return new NullableBoolean (x.Value < y.Value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableByte"/> structure to 
        /// determine if the first is less than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableByte"/> is <see cref="NullableByte.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean operator <= (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;

            return new NullableBoolean (x.Value <= y.Value);
        }


        /// <summary>
        /// Computes the remainder for the division of the two specified 
        /// <see cref="NullableByte"/> structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the remainder.
        /// </returns>
        public static NullableByte operator % (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull)
                return NullableByte.Null;

            return new NullableByte((byte)(x.Value % y.Value));
        }


        /// <summary>
        /// Computes the product of the two specified <see cref="NullableByte"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains  the product of the 
        /// multiplication.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The product  compute a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static NullableByte operator * (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull)
                return NullableByte.Null;

            checked {
                return new NullableByte((byte) (x.Value * y.Value));
            }
        }


        /// <summary>
        /// Performs a one's complement operation on the supplied 
        /// <see cref="NullableByte"/> structure.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is <see cref="Null"/> 
        /// otherwise the one's complement of <paramref name="x"/>.
        /// </returns>
        public static NullableByte operator ~(NullableByte x) {
            if (x.IsNull)
                return NullableByte.Null;

            return new NullableByte((byte)(255 & ~x.Value));
        }


        /// <summary>
        /// Computes the subtraction of the two specified <see cref="NullableByte"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> structure.</param>
        /// <param name="y">A <see cref="NullableByte"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableByte"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableByte"/> structure 
        /// whose <see cref="Value"/> property contains the results of the 
        /// subtraction.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The subtraction compute a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static NullableByte operator - (NullableByte x, NullableByte y) {
            if (x.IsNull || y.IsNull)
                return NullableByte.Null;

            checked {
                return new NullableByte((byte) (x.Value - y.Value));
            }
        }
        #endregion // Operators

        #region Conversion Operators
        /// <summary>
        /// Converts the <see cref="NullableBoolean"/> parameter to a 
        /// <see cref="NullableByte"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableBoolean"/> to be converted to a 
        /// <see cref="NullableByte"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableBoolean.Null"/> otherwise a new 
        /// <see cref="NullableByte"/> structure constructed from the 
        /// <see cref="NullableBoolean.ByteValue"/> of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableByte(NullableBoolean x) {
            if (x.IsNull)
                return Null;

            return new NullableByte(x.ByteValue);
        }


        /// <summary>
        /// Converts a <see cref="NullableByte"/> to a <see cref="System.Byte"/>.
        /// </summary>
        /// <param name="x">A <see cref="NullableByte"/> to convert.</param>
        /// <returns>
        /// A <see cref="System.Byte"/> set to the <see cref="Value"/> of the 
        /// <see cref="NullableByte"/>.
        /// </returns>
        /// <exception cref="NullableNullValueException">
        /// <paramref name="x"/> is <see cref="Null"/>.
        /// </exception>
        public static explicit operator byte(NullableByte x) {            
            return x.Value;
        }


        /// <summary>
        /// Converts the <see cref="NullableDecimal"/> parameter to a 
        /// <see cref="NullableByte"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDecimal"/> to be converted to a 
        /// <see cref="NullableByte"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise a new 
        /// <see cref="NullableByte"/> structure constructed from the 
        /// <see cref="NullableDecimal.Value"/> of <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static explicit operator NullableByte(NullableDecimal x) {
            if (x.IsNull)
                return Null;

            checked {
                return new NullableByte((byte)x.Value);
            }
        }

        
        /// <summary>
        /// Converts the <see cref="NullableDouble"/> parameter to a 
        /// <see cref="NullableByte"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDouble"/> to be converted to a 
        /// <see cref="NullableByte"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise a new 
        /// <see cref="NullableByte"/> structure constructed from the 
        /// <see cref="NullableDouble.Value"/> of <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static explicit operator NullableByte(NullableDouble x) {
            if (x.IsNull)
                return Null;

            checked {
                return new NullableByte((byte)x.Value);
            }
        }


        /// <summary>
        /// Converts the <see cref="NullableInt16"/> parameter to a 
        /// <see cref="NullableByte"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt16"/> to be converted to a 
        /// <see cref="NullableByte"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt16.Null"/> otherwise a new 
        /// <see cref="NullableByte"/> structure constructed from the 
        /// <see cref="NullableInt16.Value"/> of <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static explicit operator NullableByte(NullableInt16 x) {
            if (x.IsNull)
                return NullableByte.Null;

            checked {
                return new NullableByte((byte)x.Value);
            }
        }


        /// <summary>
        /// Converts the <see cref="NullableInt32"/> parameter to a 
        /// <see cref="NullableByte"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt32"/> to be converted to a 
        /// <see cref="NullableByte"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt32.Null"/> otherwise a new 
        /// <see cref="NullableByte"/> structure constructed from the 
        /// <see cref="NullableInt32.Value"/> of <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static explicit operator NullableByte(NullableInt32 x) {
            if (x.IsNull)
                return NullableByte.Null;

            checked {
                return new NullableByte((byte)x.Value);
            }
        }


        /// <summary>
        /// Converts the <see cref="NullableInt64"/> parameter to a 
        /// <see cref="NullableByte"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt64"/> to be converted to a 
        /// <see cref="NullableByte"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt64.Null"/> otherwise a new 
        /// <see cref="NullableByte"/> structure constructed from the 
        /// <see cref="NullableInt64.Value"/> of <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static explicit operator NullableByte(NullableInt64 x) {
            if (x.IsNull)
                return NullableByte.Null;

            checked {
                return new NullableByte((byte)x.Value);
            }
        }


        /// <summary>
        /// Converts the <see cref="NullableSingle"/> parameter to a 
        /// <see cref="NullableByte"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableSingle"/> to be converted to a 
        /// <see cref="NullableByte"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise a new 
        /// <see cref="NullableByte"/> structure constructed from the 
        /// <see cref="NullableSingle.Value"/> of <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static explicit operator NullableByte(NullableSingle x) {
            if (x.IsNull)
                return NullableByte.Null;

            checked {
                return new NullableByte((byte)x.Value);
            }
        }


        /// <summary>
        /// Converts the specified <see cref="NullableString"/> parameter to a 
        /// <see cref="NullableByte"/> structure.
        /// </summary>
        /// <param name="x">The <see cref="NullableString"/> to be converted.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableString.Null"/> otherwise a new 
        /// <see cref="NullableByte"/> structure containing the parsed value.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> does not consist solely of an optional sign followed 
        /// by a sequence of digits ranging from 0 to 9.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> represents a number less than 
        /// <see cref="System.Byte.MinValue"/> or greater than <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public static explicit operator NullableByte(NullableString x) {            
            if (x.IsNull)
                return NullableByte.Null;

            return NullableByte.Parse(x.Value);
        }


        /// <summary>
        /// Converts the <see cref="System.Byte"/> parameter to a 
        /// <see cref="NullableByte"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="System.Byte"/> to be converted to a 
        /// <see cref="NullableByte"/> structure. 
        /// </param>
        /// <returns>
        /// A new <see cref="NullableByte"/> structure constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableByte(byte x) {
            return new NullableByte(x);
        }
        #endregion // Conversion Operators

        #region Conversions
        /// <summary>
        /// Converts this <see cref="NullableByte"/> instance to a 
        /// <see cref="NullableBoolean"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.Null"/> if this <see cref="NullableByte"/> is 
        /// <see cref="NullableByte.Null"/>, <see cref="NullableBoolean.False"/> if
        /// this instance <see cref="NullableByte.Value"/> is 
        /// <see cref="NullableByte.Zero"/> otherwise 
        /// <see cref="NullableBoolean.True"/>.
        /// </returns>
        public NullableBoolean ToNullableBoolean () {
            return ((NullableBoolean)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableByte"/> instance to a 
        /// <see cref="NullableDecimal"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableDecimal"/> that is 
        /// <see cref="NullableDecimal.Null"/> if this <see cref="NullableByte"/> is 
        /// <see cref="NullableByte.Null"/> otherwise a <see cref="NullableDecimal"/> 
        /// whose <see cref="NullableDecimal.Value"/> equals the 
        /// <see cref="NullableByte.Value"/> of this instance.
        /// </returns>        
        public NullableDecimal ToNullableDecimal () {
            return ((NullableDecimal)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableByte"/> instance to a 
        /// <see cref="NullableDouble"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableDouble"/> that is 
        /// <see cref="NullableDouble.Null"/> if this <see cref="NullableByte"/> is 
        /// <see cref="NullableByte.Null"/> otherwise a <see cref="NullableDouble"/> 
        /// whose <see cref="NullableDouble.Value"/> equals the 
        /// <see cref="NullableByte.Value"/> of this instance.
        /// </returns>        
        public NullableDouble ToNullableDouble () {
            return ((NullableDouble)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableByte"/> instance to a 
        /// <see cref="NullableInt16"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt16"/> that is 
        /// <see cref="NullableInt16.Null"/> if this <see cref="NullableByte"/> is 
        /// <see cref="NullableByte.Null"/> otherwise a <see cref="NullableInt16"/> 
        /// whose <see cref="NullableInt16.Value"/> equals the 
        /// <see cref="NullableByte.Value"/> of this instance.
        /// </returns>        
        public NullableInt16 ToNullableInt16 () {
            return ((NullableInt16)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableByte"/> instance to a 
        /// <see cref="NullableInt32"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt32"/> that is 
        /// <see cref="NullableInt32.Null"/> if this <see cref="NullableByte"/> is 
        /// <see cref="NullableByte.Null"/> otherwise a <see cref="NullableInt32"/> 
        /// whose <see cref="NullableInt32.Value"/> equals the 
        /// <see cref="NullableByte.Value"/> of this instance.
        /// </returns>        
        public NullableInt32 ToNullableInt32 () {
            return ((NullableInt32)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableByte"/> instance to a 
        /// <see cref="NullableInt64"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt64"/> that is 
        /// <see cref="NullableInt64.Null"/> if this <see cref="NullableByte"/> is 
        /// <see cref="NullableByte.Null"/> otherwise a <see cref="NullableInt64"/> 
        /// whose <see cref="NullableInt64.Value"/> equals the 
        /// <see cref="NullableByte.Value"/> of this instance.
        /// </returns>    
        public NullableInt64 ToNullableInt64 () {
            return ((NullableInt64)this);
        }



        /// <summary>
        /// Converts this <see cref="NullableByte"/> instance to a 
        /// <see cref="NullableSingle"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableSingle"/> that is 
        /// <see cref="NullableSingle.Null"/> if this <see cref="NullableByte"/> is 
        /// <see cref="NullableByte.Null"/> otherwise a <see cref="NullableSingle"/> 
        /// whose <see cref="NullableSingle.Value"/> equals the 
        /// <see cref="NullableByte.Value"/> of this instance.
        /// </returns>    
        public NullableSingle ToNullableSingle () {
            return ((NullableSingle)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableByte"/> structure to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <returns>
        /// <see cref="NullableString.Null"/> if this <see cref="NullableByte"/>
        /// is <see cref="NullableByte.Null"/> otherwise
        /// a new <see cref="NullableString"/> structure whose 
        /// <see cref="NullableString.Value"/> is the description of this 
        /// <see cref="NullableByte"/> structure's <see cref="Value"/>.
        /// </returns>
        public NullableString ToNullableString () {
            return ((NullableString)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableByte"/> structure to a 
        /// <see cref="System.String"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="System.String"/> that is a description of Null if this 
        /// <see cref="NullableByte"/> is <see cref="NullableByte.Null"/> 
        /// otherwise a new <see cref="System.String"/> that is the description of 
        /// this <see cref="NullableByte"/> structure's <see cref="Value"/>.
        /// </returns>
        public override string ToString () {
            if (this.IsNull)
                return Locale.GetText("Null");

            return _value.ToString(sysGlb.CultureInfo.CurrentCulture);
        }
        #endregion // Conversions
    }
}