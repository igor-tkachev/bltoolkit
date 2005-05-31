//
// NullableTypes.NullableSingle
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author  Changes      Reasons
// 14-Apr-2003  Luca    Create
// 06-May-2003  Luca    Create       Added XML documentation
// 27-Jun-2003  Luca    Refactoring  Unified equivalent error messages 
// 30-Jun-2003  Luca    Upgrade      New requirement by Roni Burd: GetTypeCode
// 30-Jun-2003  Luca    Upgrade      Removed NullableMoney and NullableGuid
// 07-Jul-2003  Luca    Upgrade      Applied FxCop guideline: explicited CurrentCulture in CompareTo, Parse  
//                                   and ToString code
// 13-Sep-2003  Luca    Upgrade      New requirement: the type must be sarializable
// 05-Oct-2003  DamienG Upgrade      New requirement: the type must be XmlSerializable
// 06-Oct-2003  Luca    Upgrade      Code upgrade: Replaced tabs with spaces and removed commented out code
// 06-Dic-2003  Luca    Bug Fix      Replaced Xml Schema id "NullableSingleXmlSchema" with "NullableSingle"
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


    // Note for documenters: NullableSingle work more like System.Single so it 
    // accept Nan and Infinite values. Instead SqlSingle don't due the fact that 
    // they can't be persisted to Db. 

    // Note for implementer:
    // Look C# Language Specification at 4.1.5 Floating point types
    //  - Positive zero and negative zero. In most situations, positive zero and 
    //    negative zero behave identically as the simple value zero, but certain 
    //    operations distinguish between the two (Section 7.7.2). 
    //  - Positive infinity and negative infinity. Infinities are produced by such 
    //    operations as dividing a non-zero number by zero. For example, 1.0 / 0.0 
    //    yields positive infinity, and –1.0 / 0.0 yields negative infinity. 
    //  - The Not-a-Number value, often abbreviated NaN. NaN’s are produced by 
    //    invalid floating-point operations, such as dividing zero by zero. 
    // Look C# Language Specification at 7.7.2 Division operator

    // Note for implementer and tester: (from C# language specification
    // 6.2.1 Explicit numeric conversions) For a conversion from double to float, 
    // the double value is rounded to the nearest float value. If the double value
    // is too small to represent as a float, the result becomes positive zero or 
    // negative zero. If the double value is too large to represent as a float, the 
    // result becomes positive infinity or negative infinity. If the double value is 
    // NaN, the result is also NaN. 

    /// <summary>
    /// Represents a Single value that is either a single-precision floating point 
    /// number or <see cref="Null"/>.
    /// NullableSingle complies with the IEC 60559:1989 (IEEE 754) standard for binary 
    /// floating-point arithmetic as <see cref="System.Single"/>.
    /// </summary>
    [sys.Serializable]
    public struct NullableSingle : INullable, sys.IComparable, sysXmlSrl.IXmlSerializable {

        #region Fields
        float _value;
        private bool _notNull;

        /// <summary>
        /// A constant representing the largest possible value of a 
        /// <see cref="NullableSingle"/>.
        /// </summary>
        /// <remarks>
        /// The value of this constant is positive 3.402823e38.
        /// </remarks>
        public static readonly NullableSingle MaxValue = new NullableSingle(float.MaxValue);

        /// <summary>
        /// A constant representing the smallest possible value of a 
        /// <see cref="NullableSingle"/>.
        /// </summary>
        /// <remarks>
        /// The value of this constant is negative 3.402823e38.
        /// </remarks>
        public static readonly NullableSingle MinValue = new NullableSingle(float.MinValue);

        /// <summary>
        /// Represents a Null value that can be assigned to an instance of the 
        /// <see cref="NullableSingle"/> structure.
        /// <para>
        /// A Null is the absence of a value because missing, unknown, or inapplicable 
        /// value. A Null should not be used to imply any other value (such as zero).
        /// Also any value (such as zero) should not be used to imply the absence of a 
        /// value, that's why Null exists.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Null field is a constant of the <see cref="NullableSingle"/> structure.
        /// </remarks>
        public static readonly NullableSingle Null;

        /// <summary>
        /// Represents a zero value that can be assigned to an instance of the 
        /// <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <remarks>
        /// Zero field is a constant of the <see cref="NullableSingle"/> structure.
        /// </remarks>
        public static readonly NullableSingle Zero = new NullableSingle(0);
        #endregion // Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <param name="value">
        /// A <see cref="System.Double"/> value to be stored in the 
        /// <see cref="Value"/> property of the new <see cref="NullableSingle"/> 
        /// structure. 
        /// </param>
        /// <remarks>
        /// The <see cref="System.Double"/> <paramref name="value"/> is rounded 
        /// to the nearest <see cref="System.Single"/> value. If the Double value
        /// is too small to represent as a Single, the result becomes positive zero 
        /// or negative zero. If the Double value is too large to represent as a 
        /// Single, the result becomes positive infinity or negative infinity. If the 
        /// Double value is NaN, the result is also NaN. 
        /// </remarks>
        public NullableSingle(double value) {
            _value = (float)value;
            _notNull = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <param name="value">
        /// A <see cref="System.Single"/> value to be stored in the 
        /// <see cref="Value"/> property of the new <see cref="NullableSingle"/> 
        /// structure. 
        /// </param>        
        public NullableSingle(float value) {
            _value = value;
            _notNull = true;
        }
        #endregion // Constructors

        #region INullable
        /// <summary>
        /// Indicates whether or not the value of the <see cref="NullableSingle"/> 
        /// structure is null.
        /// </summary>
        /// <value>
        /// true if the <see cref="NullableSingle"/> structure is null, otherwise 
        /// false.
        /// </value>
        public bool IsNull { 
            get { return !_notNull; }
        }
        #endregion // INullable

        #region IComparable - Ordering
        /// <summary>
        /// Compares this <see cref="NullableSingle"/> structure to a specified 
        /// object and returns an indication of their relative values.
        /// </summary>
        /// <param name="value">
        /// An object to compare, or a null reference (Nothing in Visual Basic). 
        /// </param>
        /// <returns>
        /// A signed number indicating the relative values of the instance and 
        /// value.
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Value</term>
        ///             <description>Description</description>
        ///         </listheader>
        ///         <item>
        ///             <term>A negative integer</term>
        ///             <description>
        ///             This instance is less than <paramref name="value"/>
        ///             <para>-or-</para>
        ///             This instance is not a number 
        ///             (<see cref="System.Single.NaN"/>) and 
        ///             <paramref name="value"/> is a number.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>
        ///             This instance is equal to <paramref name="value"/>
        ///             <para>-or-</para>
        ///             This instance and <paramref name="value"/> are both 
        ///             <see cref="System.Single.NaN"/>, 
        ///             <see cref="System.Single.PositiveInfinity"/> or 
        ///             <see cref="System.Single.NegativeInfinity"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>A positive integer</term>
        ///             <description>
        ///             This instance is greater than <paramref name="value"/>
        ///             <para>-or-</para>
        ///             this instance is a number and <paramref name="value"/> is not 
        ///             a number (<see cref="System.Single.NaN"/>)
        ///             <para>-or-</para>
        ///             <paramref name="value"/> is a null reference (Nothing)or
        ///             <see cref="Null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <remarks>
        /// Any instance of <see cref="NullableSingle"/> , regardless of its 
        /// <paramref name="value"/>, is considered greater than a null reference
        /// (Nothing in Visual Basic) and <see cref="Null"/>.
        /// <para>
        /// This behavior follow CLR specifications while Operator== follows 
        /// different IEEE 754 specs.
        /// </para>
        /// </remarks>
        /// <exception cref="System.ArgumentException">
        /// The <paramref name="value"/> is neither null or of type 
        /// <see cref="NullableSingle"/>.
        /// </exception>
        public int CompareTo (object value) {
            if (value == null)
                return 1;
            
            if (!(value is NullableSingle))
                throw new sys.ArgumentException(string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText("Value is not a {0}."), "NullableTypes.NullableSingle"));

            NullableSingle sValue = (NullableSingle)value;            
            if (sValue.IsNull && this.IsNull)
                return 0;

            if (sValue.IsNull)
                return 1;

            if (this.IsNull)
                return -1;
            
            return _value.CompareTo(sValue._value);
        }
        #endregion // IComparable - Ordering

        #region IXmlSerializable
        /// <summary>
        /// This member supports the .NET Framework infrastructure and is not intended to be used directly 
        /// from your code.
        /// </summary>
        [sys.ComponentModel.EditorBrowsable(sys.ComponentModel.EditorBrowsableState.Never)]
        sysXml.Schema.XmlSchema sysXmlSrl.IXmlSerializable.GetSchema() {
            //    <?xml version="1.0"?>
            //    <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
            //            targetNamespace="http://NullableTypes.SourceForge.Net/NullableSingleXMLSchema"
            //            xmlns="http://NullableTypes.SourceForge.Net/NullableSingleXMLSchema"
            //            elementFormDefault="qualified">
            //        <xs:element name="NullableSingle" type="xs:float" nillable="true" />
            //    </xs:schema>

            // Element: NullableSingle
            sysXmlScm.XmlSchemaElement rootElement = new sysXmlScm.XmlSchemaElement();
            rootElement.Name = "NullableSingle";
            rootElement.SchemaTypeName = 
                new sysXml.XmlQualifiedName("float", "http://www.w3.org/2001/XMLSchema");
            rootElement.IsNillable = true;

            // Xml Schema
            sysXmlScm.XmlSchema xsd = new sysXmlScm.XmlSchema();
            xsd.Id = "NullableSingle";
            xsd.Namespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
            xsd.TargetNamespace = "http://NullableTypes.SourceForge.Net/NullableSingleXMLSchema";
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
                _value = sysXml.XmlConvert.ToSingle(elementValue);
                _notNull = true;
            }
            else
                _notNull = false;
        }
        #endregion // IXmlSerializable

        #region Properties
        /// <summary>
        /// Gets the value of the <see cref="NullableSingle"/> structure. This 
        /// property is read-only
        /// </summary>
        /// <value>
        /// A floating point value in the range -3.40E+38 through 3.40E+38.
        /// </value>
        /// <exception cref="NullableNullValueException">
        /// The property is set to null.
        /// </exception>
        public float Value { 
            get { 
                if (this.IsNull) 
                    throw new NullableNullValueException();

                return _value; 
            }
        }
        #endregion // Properties

        #region Equivalence
        /// <summary>
        /// Compares two <see cref="NullableSingle"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="value">The object to be compared. </param>
        /// <returns>
        /// true if object is an instance of <see cref="NullableSingle"/> and the 
        /// two instances are equivalent; otherwise false.
        /// </returns>
        /// <remarks>
        /// This behavior follow CLR specifications while <see cref="operator=="/>
        /// follows different IEEE 754 specifications.
        /// </remarks>
        public override bool Equals (object value) {
            if (!(value is NullableSingle))
                return false;
            
            // Important: float.Equals has a different behavior from 
            // float.operator== with NaN:
            //    float.NaN.Equals(float.NaN) is true 
            //    float.NaN == float.NaN is false (IEEE 754 specs)

            NullableSingle sValue = (NullableSingle)value;            
            return (this._notNull == sValue._notNull) && 
                (this._value.Equals(sValue._value)); 
        }


        /// <summary>
        /// Compares two <see cref="NullableSingle"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.False"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableSingle"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        /// <remarks>
        /// This behavior follow CLR specifications while <see cref="operator=="/> 
        /// follows different IEEE 754 specifications.
        /// </remarks>
        public static NullableBoolean Equals(NullableSingle x, NullableSingle y) {

            // Important: float.Equals has a different behavior from 
            // float.operator== with NaN:
            //    float.NaN.Equals(float.NaN) is true
            //    float.NaN == float.NaN is false (IEEE 754 specs)
            
            if (!x._notNull || !y._notNull) return NullableBoolean.Null;

            return (x.Equals(y));
        }

        /// <summary>
        /// Compares two <see cref="NullableSingle"/> structures to determine if 
        /// they are not equivalent.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.False"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.True"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableSingle"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        /// <remarks>
        /// This behavior follow CLR specifications while <see cref="operator!="/> 
        /// follows different IEEE 754 specifications.
        /// </remarks>
        public static NullableBoolean NotEquals(NullableSingle x, NullableSingle y) {

            // Important: float.Equals has a different behavior from 
            // float.operator== with NaN:
            //    float.NaN.Equals(float.NaN) is true
            //    float.NaN == float.NaN is false (IEEE 754 specs)

            if (!x._notNull || !y._notNull) return NullableBoolean.Null;
            
            return (!x.Equals(y));
        }

        /// <summary>
        /// Compares two instances of <see cref="NullableSingle"/> for equivalence.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.False"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableSingle"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Single.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Single.NegativeInfinity"/> is considered equal to 
        /// another <see cref="System.Single.NegativeInfinity"/>; a 
        /// <see cref="System.Single.PositiveInfinity"/> is considered equal to 
        /// another <see cref="System.Single.PositiveInfinity"/>. 
        /// </remarks>
        public static NullableBoolean operator ==(NullableSingle x, NullableSingle y) {
            if (!x._notNull || !y._notNull) return NullableBoolean.Null;

            // Important: float.Equals has a different behavior from 
            // float.operator== with NaN:
            //    float.NaN.Equals(float.NaN) is true
            //    float.NaN == float.NaN is false (IEEE 754 specs)
            
            return new NullableBoolean(x._value == y._value);
        }

        /// <summary>
        /// Compares two instances of <see cref="NullableSingle"/> for equivalence.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are not 
        /// equivalent or <see cref="NullableBoolean.False"/> if the two instances 
        /// are equivalent. If either instance of <see cref="NullableSingle"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Single.NaN"/> the result is true; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Single.NegativeInfinity"/> is considered equal to 
        /// another <see cref="System.Single.NegativeInfinity"/>; a 
        /// <see cref="System.Single.PositiveInfinity"/> is considered equal to 
        /// another <see cref="System.Single.PositiveInfinity"/>. 
        /// </remarks>
        public static NullableBoolean operator !=(NullableSingle x, NullableSingle y) {
            if (!x._notNull || !y._notNull) return NullableBoolean.Null;
            return new NullableBoolean(!(x._value == y._value));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() {
            // if IsNull then _value will always be 0f
            // hash code for Null will be the hash code of 0f
            return _value.GetHashCode();
        }
        #endregion // Equivalence

        #region Methods
        /// <summary>
        /// Returns the <see cref="TypeCode"/> for value type 
        /// <see cref="NullableSingle"/>.
        /// </summary>
        /// <returns>
        /// The enumerated constant, <see cref="TypeCode.NullableSingle"/>.
        /// </returns>
        public TypeCode GetTypeCode() {
            return TypeCode.NullableSingle;
        }


        /// <summary>
        /// Computes the sum of the two specified <see cref="NullableSingle"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableSingle"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableSingle"/> structure 
        /// whose <see cref="Value"/> property contains the results of the addition.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableSingle"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableSingle"/>, it becomes 
        /// <see cref="System.Single.PositiveInfinity"/> or 
        /// <see cref="System.Single.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Single.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableSingle Add(NullableSingle x, NullableSingle y) {
            return (x + y);
        }


        /// <summary>
        /// Divides its first <see cref="NullableSingle"/> operand by its second.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableSingle"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableSingle"/> structure 
        /// whose <see cref="Value"/> property contains the results of the division.
        /// </returns>
        /// <exception cref="System.DivideByZeroException">
        /// <paramref name="y"/> is <see cref="Zero"/> while <paramref name="x"/> is 
        /// not <see cref="Null"/>.
        /// </exception>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableSingle"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableSingle"/>, it becomes 
        /// <see cref="System.Single.PositiveInfinity"/> or 
        /// <see cref="System.Single.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Single.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableSingle Divide(NullableSingle x, NullableSingle y) {
            return (x / y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableSingle"/> structure to 
        /// determine if the first is greater than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableSingle"/> is <see cref="NullableSingle.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Single.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Single.NegativeInfinity"/> is considered less than all 
        /// other values; a <see cref="System.Single.PositiveInfinity"/> is 
        /// considered greater than all other values. 
        /// </remarks>
        public static NullableBoolean GreaterThan(NullableSingle x, NullableSingle y) {
            return (x > y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableSingle"/> structure to 
        /// determine if the first is greater than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableSingle"/> is <see cref="NullableSingle.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Single.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Single.NegativeInfinity"/> is considered less than all 
        /// other values, but equal to another 
        /// <see cref="System.Single.NegativeInfinity"/>; a 
        /// <see cref="System.Single.PositiveInfinity"/> is considered greater than 
        /// all other values, but equal to another 
        /// <see cref="System.Single.PositiveInfinity"/>.
        /// </remarks>
        public static NullableBoolean GreaterThanOrEqual(NullableSingle x, NullableSingle y) {
            return (x >= y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableSingle"/> structure to 
        /// determine if the first is less than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableSingle"/> is <see cref="NullableByte.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>        
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Single.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Single.NegativeInfinity"/> is considered less than all 
        /// other values; a <see cref="System.Single.PositiveInfinity"/> is 
        /// considered greater than all other values. 
        /// </remarks>
        public static NullableBoolean LessThan(NullableSingle x, NullableSingle y) {
            return (x < y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableSingle"/> structure to 
        /// determine if the first is less than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableSingle"/> is <see cref="NullableSingle.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Single.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Single.NegativeInfinity"/> is considered less than all 
        /// other values, but equal to another 
        /// <see cref="System.Single.NegativeInfinity"/>; a 
        /// <see cref="System.Single.PositiveInfinity"/> is considered greater than 
        /// all other values, but equal to another 
        /// <see cref="System.Single.PositiveInfinity"/>.
        /// </remarks>
        public static NullableBoolean LessThanOrEqual(NullableSingle x, NullableSingle y) {
            return (x <= y);
        }


        /// <summary>
        /// Computes the product of the two specified <see cref="NullableSingle"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableSingle"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableSingle"/> structure 
        /// whose <see cref="Value"/> property contains  the product of the 
        /// multiplication.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableSingle"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableSingle"/>, it becomes 
        /// <see cref="System.Single.PositiveInfinity"/> or 
        /// <see cref="System.Single.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Single.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableSingle Multiply(NullableSingle x, NullableSingle y) {
            return (x * y);
        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a 
        /// number to its <see cref="NullableSingle"/> equivalent.
        /// </summary>
        /// <param name="s">The <see cref="System.String"/> to be converted.</param>
        /// <returns>
        /// A <see cref="NullableSingle"/> structure containing the parsed value.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="s"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="s"/> s is not a number in a valid format.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="s"/> represents a number less than 
        /// <see cref="System.Single.MinValue"/> or greater than 
        /// <see cref="System.Single.MaxValue"/>.
        /// </exception>
        /// <remarks>
        /// The <paramref name="s"/> parameter can contain 
        /// <see cref="System.Globalization.NumberFormatInfo.PositiveInfinitySymbol"/>, 
        /// <see cref="System.Globalization.NumberFormatInfo.NegativeInfinitySymbol"/> 
        /// and 
        /// <see cref="System.Globalization.NumberFormatInfo.NaNSymbol"/>.
        /// </remarks>
        /// <seealso cref="System.Single.Parse"/>
        public static NullableSingle Parse (string s) {
            return new NullableSingle(float.Parse(s, sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Computes the subtraction of the two specified <see cref="NullableSingle"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableSingle"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableSingle"/> structure 
        /// whose <see cref="Value"/> property contains the results of the 
        /// subtraction.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableSingle"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableSingle"/>, it becomes 
        /// <see cref="System.Single.PositiveInfinity"/> or 
        /// <see cref="System.Single.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Single.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableSingle Subtract(NullableSingle x, NullableSingle y) {
            return (x - y);
        }
        #endregion // Methods

        #region Operators
        /// <summary>
        /// Computes the sum of the two specified <see cref="NullableSingle"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableSingle"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableSingle"/> structure 
        /// whose <see cref="Value"/> property contains the results of the addition.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableSingle"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableSingle"/>, it becomes 
        /// <see cref="System.Single.PositiveInfinity"/> or 
        /// <see cref="System.Single.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Single.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableSingle operator + (NullableSingle x, NullableSingle y) {
            if (x.IsNull || y.IsNull)
                return NullableSingle.Null;

            float res = x.Value + y.Value;

            return new NullableSingle(res);
        }


        /// <summary>
        /// Divides its first <see cref="NullableSingle"/> operand by its second.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableSingle"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableSingle"/> structure 
        /// whose <see cref="Value"/> property contains the results of the division.
        /// </returns>
        /// <exception cref="System.DivideByZeroException">
        /// <paramref name="y"/> is <see cref="Zero"/> while <paramref name="x"/> is 
        /// not <see cref="Null"/>.
        /// </exception>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableSingle"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableSingle"/>, it becomes 
        /// <see cref="System.Single.PositiveInfinity"/> or 
        /// <see cref="System.Single.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Single.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableSingle operator / (NullableSingle x, NullableSingle y) {
            if (x.IsNull || y.IsNull)
                return NullableSingle.Null;

            if (y.Value == 0)
                throw new sys.DivideByZeroException();

            float res = x.Value / y.Value;

            return new NullableSingle(res);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableSingle"/> structure to 
        /// determine if the first is greater than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableSingle"/> is <see cref="NullableSingle.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Single.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Single.NegativeInfinity"/> is considered less than all 
        /// other values; a <see cref="System.Single.PositiveInfinity"/> is 
        /// considered greater than all other values. 
        /// </remarks>
        public static NullableBoolean operator > (NullableSingle x, NullableSingle y) {
            if (x.IsNull || y .IsNull) return NullableBoolean.Null;

            return new NullableBoolean(x.Value > y.Value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableSingle"/> structure to 
        /// determine if the first is greater than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableSingle"/> is <see cref="NullableSingle.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Single.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Single.NegativeInfinity"/> is considered less than all 
        /// other values, but equal to another 
        /// <see cref="System.Single.NegativeInfinity"/>; a 
        /// <see cref="System.Single.PositiveInfinity"/> is considered greater than 
        /// all other values, but equal to another 
        /// <see cref="System.Single.PositiveInfinity"/>.
        /// </remarks>
        public static NullableBoolean operator >= (NullableSingle x, NullableSingle y) {
            if (x.IsNull || y .IsNull) return NullableBoolean.Null;
            return new NullableBoolean(x.Value >= y.Value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableSingle"/> structure to 
        /// determine if the first is less than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableSingle"/> is <see cref="NullableByte.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>        
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Single.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Single.NegativeInfinity"/> is considered less than all 
        /// other values; a <see cref="System.Single.PositiveInfinity"/> is 
        /// considered greater than all other values. 
        /// </remarks>
        public static NullableBoolean operator < (NullableSingle x, NullableSingle y) {
            if (x.IsNull || y .IsNull) return NullableBoolean.Null;
            return new NullableBoolean(x.Value < y.Value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableSingle"/> structure to 
        /// determine if the first is less than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableSingle"/> is <see cref="NullableSingle.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Single.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Single.NegativeInfinity"/> is considered less than all 
        /// other values, but equal to another 
        /// <see cref="System.Single.NegativeInfinity"/>; a 
        /// <see cref="System.Single.PositiveInfinity"/> is considered greater than 
        /// all other values, but equal to another 
        /// <see cref="System.Single.PositiveInfinity"/>.
        /// </remarks>
        public static NullableBoolean operator <= (NullableSingle x, NullableSingle y) {
            if (x.IsNull || y .IsNull) return NullableBoolean.Null;
            return new NullableBoolean(x.Value <= y.Value);
        }


        /// <summary>
        /// Computes the product of the two specified <see cref="NullableSingle"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableSingle"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableSingle"/> structure 
        /// whose <see cref="Value"/> property contains  the product of the 
        /// multiplication.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableSingle"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableSingle"/>, it becomes 
        /// <see cref="System.Single.PositiveInfinity"/> or 
        /// <see cref="System.Single.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Single.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableSingle operator * (NullableSingle x, NullableSingle y) {
            if (x.IsNull || y .IsNull) return NullableSingle.Null;

            float res = x.Value * y.Value;

            return new NullableSingle(res);
        }


        /// <summary>
        /// Computes the subtraction of the two specified <see cref="NullableSingle"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <param name="y">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableSingle"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableSingle"/> structure 
        /// whose <see cref="Value"/> property contains the results of the 
        /// subtraction.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableSingle"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableSingle"/>, it becomes 
        /// <see cref="System.Single.PositiveInfinity"/> or 
        /// <see cref="System.Single.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Single.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Single.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableSingle operator - (NullableSingle x, NullableSingle y) {
            if (x.IsNull || y .IsNull) return NullableSingle.Null;

            float res = x.Value - y.Value;

            return new NullableSingle(res);
        }


        /// <summary>
        /// Negates the <see cref="Value"/> of the specified <see cref="NullableSingle"/>
        /// structure.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/>  is <see cref="Null"/> 
        /// otherwise a <see cref="NullableSingle"/> structure containing the 
        /// negated value.
        /// </returns>
        public static NullableSingle operator - (NullableSingle x) {
            if (x.IsNull) return NullableSingle.Null;

            return new NullableSingle(-(x.Value));
        }
        #endregion // Operators

        #region Conversion Operators
        /// <summary>
        /// Converts the <see cref="NullableBoolean"/> parameter to a 
        /// <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableBoolean"/> to be converted to a 
        /// <see cref="NullableSingle"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableBoolean.Null"/> otherwise a new 
        /// <see cref="NullableSingle"/> structure constructed from the 
        /// <see cref="NullableBoolean.ByteValue"/> of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableSingle(NullableBoolean x) {
            if (x.IsNull)
                return Null;
            
            return new NullableSingle((float)x.ByteValue);
        }


        /// <summary>
        /// Converts the <see cref="NullableDouble"/> parameter to a 
        /// <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDouble"/> to be converted to a 
        /// <see cref="NullableSingle"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise a new 
        /// <see cref="NullableSingle"/> structure constructed from the 
        /// <see cref="NullableDouble.Value"/> of <paramref name="x"/>.
        /// </returns>
        /// <remarks>
        /// The <paramref name="x"/> <see cref="NullableDouble"/> value is rounded 
        /// to the nearest <see cref="System.Single"/> value. If the Double value
        /// is too small to represent as a Single, the result becomes positive zero 
        /// or negative zero. If the Double value is too large to represent as a 
        /// Single, the result becomes positive infinity or negative infinity. If the 
        /// Double value is NaN, the result is also NaN. 
        /// </remarks>

        public static explicit operator NullableSingle(NullableDouble x) {
            if (x.IsNull)
                return Null;

            return new NullableSingle((float)x.Value);  
        }


        /// <summary>
        /// Converts a <see cref="NullableSingle"/> to a <see cref="System.Single"/>.
        /// </summary>
        /// <param name="x">A <see cref="NullableSingle"/> to convert.</param>
        /// <returns>
        /// A <see cref="System.Single"/> set to the <see cref="Value"/> of the 
        /// <see cref="NullableSingle"/>.
        /// </returns>
        /// <exception cref="NullableNullValueException">
        /// <paramref name="x"/> is <see cref="Null"/>.
        /// </exception>
        public static explicit operator float(NullableSingle x) {
            return x.Value;
        }


        /// <summary>
        /// Converts the specified <see cref="NullableString"/> parameter to a 
        /// <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <param name="x">The <see cref="NullableString"/> to be converted.</param>
        /// <returns>
        /// <see cref="Null"/> If <paramref name="x"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise a new
        /// <see cref="NullableSingle"/> structure containing the parsed value.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> is not a number in a valid format.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> represents a number less than 
        /// <see cref="System.Single.MinValue"/> or greater than 
        /// <see cref="System.Single.MaxValue"/>.
        /// </exception>
        /// <remarks>
        /// The <paramref name="x"/> parameter can contain 
        /// <see cref="System.Globalization.NumberFormatInfo.PositiveInfinitySymbol"/>, 
        /// <see cref="System.Globalization.NumberFormatInfo.NegativeInfinitySymbol"/> 
        /// and 
        /// <see cref="System.Globalization.NumberFormatInfo.NaNSymbol"/>.
        /// </remarks>
        /// <seealso cref="System.Single.Parse"/>
        public static explicit operator NullableSingle(NullableString x) {
            if (x.IsNull)
                return Null;
                
            return NullableSingle.Parse(x.Value);
        }


        /// <summary>
        /// Converts the <see cref="System.Single"/> parameter to a 
        /// <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="System.Single"/> to be converted to a 
        /// <see cref="NullableSingle"/> structure. 
        /// </param>
        /// <returns>
        /// A new <see cref="NullableSingle"/> structure constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableSingle(float x) {
            return new NullableSingle(x);
        }


        /// <summary>
        /// Converts the <see cref="NullableByte"/> parameter to a 
        /// <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableByte"/> to be converted to a 
        /// <see cref="NullableSingle"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableByte.Null"/> otherwise a new 
        /// <see cref="NullableSingle"/> structure constructed from the 
        /// <see cref="NullableByte.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableSingle(NullableByte x) {
            if (x.IsNull) 
                return Null;

            return new NullableSingle((float)x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableDecimal"/> parameter to a 
        /// <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDecimal"/> to be converted to a 
        /// <see cref="NullableSingle"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise a new 
        /// <see cref="NullableSingle"/> structure constructed from the 
        /// <see cref="NullableDecimal.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableSingle(NullableDecimal x) {
            if (x.IsNull) 
                return Null;

            // C# Language Specification   
            // 6.2.1 Explicit numeric conversions
            // ------------------------------------
            // For a conversion from decimal to float or double, the decimal value is rounded to the nearest 
            // double or float value. While this conversion may lose precision, it never causes an exception 
            // to be thrown. 
            return new NullableSingle((float)x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableInt16"/> parameter to a 
        /// <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt16"/> to be converted to a 
        /// <see cref="NullableSingle"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt16.Null"/> otherwise a new 
        /// <see cref="NullableSingle"/> structure constructed from the 
        /// <see cref="NullableInt16.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableSingle(NullableInt16 x) {
            if (x.IsNull) 
                return Null;

            return new NullableSingle((float)x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableInt32"/> parameter to a 
        /// <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt32"/> to be converted to a 
        /// <see cref="NullableSingle"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt32.Null"/> otherwise a new 
        /// <see cref="NullableSingle"/> structure constructed from the 
        /// <see cref="NullableInt32.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableSingle(NullableInt32 x) {
            if (x.IsNull) 
                return Null;

            return new NullableSingle((float)x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableInt64"/> parameter to a 
        /// <see cref="NullableSingle"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt64"/> to be converted to a 
        /// <see cref="NullableSingle"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt64.Null"/> otherwise a new 
        /// <see cref="NullableSingle"/> structure constructed from the 
        /// <see cref="NullableInt64.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableSingle(NullableInt64 x) {
            if (x.IsNull) 
                return Null;

            return new NullableSingle((float)x.Value);
        }
        #endregion // Conversion Operators

        #region Conversions
        /// <summary>
        /// Converts this <see cref="NullableSingle"/> instance to a 
        /// <see cref="NullableBoolean"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.Null"/> if this <see cref="NullableSingle"/> is 
        /// <see cref="NullableSingle.Null"/>, <see cref="NullableBoolean.False"/> if
        /// this instance <see cref="NullableSingle.Value"/> is 
        /// <see cref="NullableSingle.Zero"/> otherwise 
        /// <see cref="NullableBoolean.True"/>.
        /// </returns>
        public NullableBoolean ToNullableBoolean() {
            return ((NullableBoolean)this);
        }
        

        /// <summary>
        /// Converts this <see cref="NullableSingle"/> instance to a 
        /// <see cref="NullableByte"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableByte"/> that is 
        /// <see cref="NullableByte.Null"/> if this <see cref="NullableSingle"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise a <see cref="NullableByte"/> 
        /// whose <see cref="NullableByte.Value"/> equals the 
        /// <see cref="NullableSingle.Value"/> of this instance.
        /// </returns>    
        /// <exception cref="System.OverflowException">
        /// This <see cref="NullableSingle"/> <see cref="Value"/> is a number that 
        /// is less than  <see cref="System.Byte.MinValue"/> or greater than 
        /// <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public NullableByte ToNullableByte() {
            return ((NullableByte)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableSingle"/> instance to a 
        /// <see cref="NullableDecimal"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableDecimal"/> that is 
        /// <see cref="NullableDecimal.Null"/> if this <see cref="NullableSingle"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise a <see cref="NullableDecimal"/> 
        /// whose <see cref="NullableDecimal.Value"/> equals the 
        /// <see cref="NullableSingle.Value"/> of this instance.
        /// </returns>    
        /// <exception cref="System.OverflowException">
        /// This <see cref="NullableSingle"/> <see cref="Value"/> is a number that 
        /// is less than <see cref="NullableDecimal.MinValue"/> or greater than 
        /// <see cref="NullableDecimal.MaxValue"/>
        /// <para>-or-</para>
        /// <see cref="Value"/> is <see cref="System.Single.NaN"/>, <see cref="System.Single.PositiveInfinity"/>, or 
        /// <see cref="System.Single.NegativeInfinity"/>.
        /// </exception>
        public NullableDecimal ToNullableDecimal() {
            return ((NullableDecimal)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableSingle"/> instance to a 
        /// <see cref="NullableDouble"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableDouble"/> that is 
        /// <see cref="NullableDouble.Null"/> if this <see cref="NullableSingle"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise a <see cref="NullableDouble"/> 
        /// whose <see cref="NullableDouble.Value"/> equals the 
        /// <see cref="NullableSingle.Value"/> of this instance.
        /// </returns>    
        public NullableDouble ToNullableDouble() {
            return ((NullableDouble)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableSingle"/> instance to a 
        /// <see cref="NullableInt16"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt16"/> that is 
        /// <see cref="NullableInt16.Null"/> if this <see cref="NullableSingle"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise a <see cref="NullableInt16"/> 
        /// whose <see cref="NullableInt16.Value"/> equals the 
        /// <see cref="NullableSingle.Value"/> of this instance.
        /// </returns>    
        /// <exception cref="System.OverflowException">
        /// This <see cref="NullableSingle"/> <see cref="Value"/> is a number that 
        /// is less than  <see cref="NullableInt16.MinValue"/> or greater than 
        /// <see cref="NullableInt16.MaxValue"/>.
        /// </exception>
        public NullableInt16 ToNullableInt16() {
            return ((NullableInt16)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableSingle"/> instance to a 
        /// <see cref="NullableInt32"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt32"/> that is 
        /// <see cref="NullableInt32.Null"/> if this <see cref="NullableSingle"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise a <see cref="NullableInt32"/> 
        /// whose <see cref="NullableInt32.Value"/> equals the 
        /// <see cref="NullableSingle.Value"/> of this instance.
        /// </returns>    
        /// <exception cref="System.OverflowException">
        /// This <see cref="NullableSingle"/> <see cref="Value"/> is a number that 
        /// is less than  <see cref="NullableInt32.MinValue"/> or greater than 
        /// <see cref="NullableInt32.MaxValue"/>.
        /// </exception>
        public NullableInt32 ToNullableInt32() {
            return ((NullableInt32)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableSingle"/> instance to a 
        /// <see cref="NullableInt64"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt64"/> that is 
        /// <see cref="NullableInt64.Null"/> if this <see cref="NullableSingle"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise a <see cref="NullableInt64"/> 
        /// whose <see cref="NullableInt64.Value"/> equals the 
        /// <see cref="NullableSingle.Value"/> of this instance.
        /// </returns>    
        /// <exception cref="System.OverflowException">
        /// This <see cref="NullableSingle"/> <see cref="Value"/> is a number that 
        /// is less than  <see cref="NullableInt64.MinValue"/> or greater than 
        /// <see cref="NullableInt64.MaxValue"/>.
        /// </exception>
        public NullableInt64 ToNullableInt64() {
            return ((NullableInt64)this);
        }



        /// <summary>
        /// Converts this <see cref="NullableSingle"/> structure to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <returns>
        /// <see cref="NullableString.Null"/> if this <see cref="NullableSingle"/>
        /// is <see cref="NullableSingle.Null"/> otherwise
        /// a new <see cref="NullableString"/> structure whose 
        /// <see cref="NullableString.Value"/> is the description of this 
        /// <see cref="NullableSingle"/> structure's <see cref="Value"/>.
        /// </returns>
        public NullableString ToNullableString() {
            return ((NullableString)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableSingle"/> structure to a 
        /// <see cref="System.String"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="System.String"/> that is a description of Null if this 
        /// <see cref="NullableSingle"/> is <see cref="NullableSingle.Null"/> 
        /// otherwise a new <see cref="System.String"/> that is the description of 
        /// this <see cref="NullableSingle"/> structure's <see cref="Value"/>.
        /// </returns>
        /// <seealso cref="System.Single.ToString()"/>
        public override string ToString() {
            if (this.IsNull)
                return Locale.GetText("Null");
            
            return _value.ToString(sysGlb.CultureInfo.CurrentCulture);
        }
        #endregion // Conversions
    }
}