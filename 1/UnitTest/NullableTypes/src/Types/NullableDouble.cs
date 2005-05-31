//
// NullableTypes.NullableDouble
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author  Changes      Reasons
// 14-Apr-2003  Luca    Create
// 06-May-2003  Luca    Create       Added XML doc.
// 27-Jun-2003  Luca    Refactoring  Unified equivalent error messages  
// 30-Jun-2003  Luca    Upgrade      New requirement by Roni Burd: GetTypeCode 
// 30-Jun-2003  Luca    Upgrade      Removed NullableMoney and NullableGuid
// 07-Jul-2003  Luca    Upgrade      Applied FxCop guideline: explicited CurrentCulture in CompareTo, Parse  
//                                   and ToString code
// 13-Sep-2003  Luca    Upgrade      New requirement: the type must be sarializable
// 05-Oct-2003  DamienG Upgrade      New requirement: the type must be XmlSerializable
// 06-Oct-2003  Luca    Upgrade      Code upgrade: Replaced tabs with spaces and removed commented out code
// 06-Dic-2003  Luca    Bug Fix      Replaced Xml Schema id "NullableDoubleXmlSchema" with "NullableDouble"
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


    // Note for documenters: NullableDouble work more like System.Double so it 
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
    /// Represents a Double value that is either a double-precision floating point 
    /// number or <see cref="Null"/>.
    /// NullableDouble complies with the IEC 60559:1989 (IEEE 754) standard for 
    /// binary floating-point arithmetic as <see cref="System.Double"/>.
    /// </summary>
    [sys.Serializable]
    public struct NullableDouble : INullable, sys.IComparable, sysXmlSrl.IXmlSerializable {

        #region Fields
        double _value;
        private bool _notNull;

        /// <summary>
        /// A constant representing the largest possible value of a 
        /// <see cref="NullableDouble"/>.
        /// </summary>
        /// <remarks>
        /// The value of this constant is positive 1.79769313486232e308.
        /// </remarks>
        public static readonly NullableDouble MaxValue = new NullableDouble(double.MaxValue);

        /// <summary>
        /// A constant representing the smallest possible value of a 
        /// <see cref="NullableDouble"/>.
        /// </summary>
        /// <remarks>
        /// The value of this constant is negative 1.79769313486232e308.
        /// </remarks>
        public static readonly NullableDouble MinValue = new NullableDouble(double.MinValue);

        /// <summary>
        /// Represents a Null value that can be assigned to an instance of the 
        /// <see cref="NullableDouble"/> structure.
        /// <para>
        /// A Null is the absence of a value because missing, unknown, or inapplicable 
        /// value. A Null should not be used to imply any other value (such as zero).
        /// Also any value (such as zero) should not be used to imply the absence of a 
        /// value, that's why Null exists.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Null field is a constant of the <see cref="NullableDouble"/> structure.
        /// </remarks>
        public static readonly NullableDouble Null;

        /// <summary>
        /// Represents a zero value that can be assigned to an instance of the 
        /// <see cref="NullableDouble"/> structure.
        /// </summary>
        /// <remarks>
        /// Zero field is a constant of the <see cref="NullableDouble"/> structure.
        /// </remarks>
        public static readonly NullableDouble Zero = new NullableDouble(0);
        #endregion // Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NullableDouble"/> structure.
        /// </summary>
        /// <param name="value">
        /// A <see cref="System.Double"/> value to be stored in the 
        /// <see cref="Value"/> property of the new <see cref="NullableDouble"/> 
        /// structure. 
        /// </param>
        public NullableDouble(double value) {
            _value = value;
            _notNull = true;
        }
        #endregion // Constructors

        #region INullable
        /// <summary>
        /// Indicates whether or not the value of the <see cref="NullableDouble"/> 
        /// structure is null.
        /// </summary>
        /// <value>
        /// true if the <see cref="NullableDouble"/> structure is null, otherwise 
        /// false.
        /// </value>
        public bool IsNull { 
            get { return !_notNull; }
        }
        #endregion // INullable

        #region IComparable - Ordering
        /// <summary>
        /// Compares this <see cref="NullableDouble"/> structure to a specified 
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
        ///             (<see cref="System.Double.NaN"/>) and 
        ///             <paramref name="value"/> is a number.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>
        ///             This instance is equal to <paramref name="value"/>
        ///             <para>-or-</para>
        ///             This instance and <paramref name="value"/> are both 
        ///             <see cref="System.Double.NaN"/>, 
        ///             <see cref="System.Double.PositiveInfinity"/> or 
        ///             <see cref="System.Double.NegativeInfinity"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>A positive integer</term>
        ///             <description>
        ///             This instance is greater than <paramref name="value"/>
        ///             <para>-or-</para>
        ///             this instance is a number and <paramref name="value"/> is not 
        ///             a number (<see cref="System.Double.NaN"/>)
        ///             <para>-or-</para>
        ///             <paramref name="value"/> is a null reference 
        ///             (Nothing in Visual Basic) or <see cref="Null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <remarks>
        /// Any instance of <see cref="NullableDouble"/> , regardless of its 
        /// <paramref name="value"/>, is considered greater than a null reference 
        /// (Nothing in Visual Basic) and <see cref="Null"/>. 
        /// <para>
        /// This behavior follow CLR specifications while Operator== follows 
        /// different IEEE 754 specs.
        /// </para>
        /// </remarks>
        /// <exception cref="System.ArgumentException">
        /// The <paramref name="value"/> is neither null or of type 
        /// <see cref="NullableDouble"/>.
        /// </exception>
        public int CompareTo (object value) {
            if (value == null)
                return 1;
            
            if (!(value is NullableDouble))
                throw new sys.ArgumentException(string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText("Value is not a {0}."), "NullableTypes.NullableDouble"));

            NullableDouble sValue = (NullableDouble)value;            
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
            //            targetNamespace="http://NullableTypes.SourceForge.Net/NullableDoubleXMLSchema"
            //            xmlns="http://NullableTypes.SourceForge.Net/NullableDoubleXMLSchema"
            //            elementFormDefault="qualified">
            //        <xs:element name="NullableDouble" type="xs:double" nillable="true" />
            //    </xs:schema>

            // Element: NullableDouble
            sysXmlScm.XmlSchemaElement rootElement = new sysXmlScm.XmlSchemaElement();
            rootElement.Name = "NullableDouble";
            rootElement.SchemaTypeName = 
                new sysXml.XmlQualifiedName("double", "http://www.w3.org/2001/XMLSchema");
            rootElement.IsNillable = true;

            // Xml Schema
            sysXmlScm.XmlSchema xsd = new sysXmlScm.XmlSchema();
            xsd.Id = "NullableDouble";
            xsd.Namespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
            xsd.TargetNamespace = "http://NullableTypes.SourceForge.Net/NullableDoubleXMLSchema";
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
                _value = sysXml.XmlConvert.ToDouble(elementValue);
                _notNull = true;
            }
            else
                _notNull = false;
        }
        #endregion // IXmlSerializable

        #region Properties
        /// <summary>
        /// Gets the value of the <see cref="NullableDouble"/> structure. This 
        /// property is read-only
        /// </summary>
        /// <value>
        /// A double point value in the range -1.79769313486232e308. through 
        /// 1.79769313486232e308.
        /// </value>
        /// <exception cref="NullableNullValueException">
        /// The property is set to null.
        /// </exception>        
        public double Value { 
            get { 
                if (this.IsNull) 
                    throw new NullableNullValueException();
                else 
                    return _value; 
            }
        }
        #endregion // Properties

        #region Equivalence

        /// <summary>
        /// Compares two <see cref="NullableDouble"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="value">The object to be compared.</param>
        /// <returns>
        /// true if object is an instance of <see cref="NullableDouble"/> and the 
        /// two instances are equivalent; otherwise false.
        /// </returns>
        /// <remarks>
        /// This behavior follow CLR specifications while <see cref="operator=="/>
        /// follows different IEEE 754 specifications.
        /// </remarks>
        public override bool Equals (object value) {
            if (!(value is NullableDouble))
                return false;
            
            // Important: double.Equals has a different behavior from 
            // double.operator== with NaN:
            //    double.NaN.Equals(double.NaN) is true
            //    double.NaN == double.NaN is false (IEEE 754 specs)

            NullableDouble sValue = (NullableDouble)value;            
            return (this._notNull == sValue._notNull) && 
                (this._value.Equals(sValue._value));
        }


        /// <summary>
        /// Compares two <see cref="NullableDouble"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.False"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableDouble"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        /// <remarks>
        /// This behavior follow CLR specifications while <see cref="operator=="/> 
        /// follows different IEEE 754 specifications.
        /// </remarks>
        public static NullableBoolean Equals(NullableDouble x, NullableDouble y) {
            
            // Important: double.Equals has a different behavior from 
            // double.operator== with NaN:
            //    double.NaN.Equals(double.NaN) is true
            //    double.NaN == double.NaN is false (IEEE 754 specs)

            if (!x._notNull || !y._notNull) return NullableBoolean.Null;

            return (x.Equals(y));
        }


        /// <summary>
        /// Compares two <see cref="NullableDouble"/> structures to determine if 
        /// they are not equivalent.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.False"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.True"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableDouble"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        /// <remarks>
        /// This behavior follow CLR specifications while <see cref="operator!="/> 
        /// follows different IEEE 754 specifications.
        /// </remarks>
        public static NullableBoolean NotEquals (NullableDouble x, NullableDouble y) {

            // Important: double.Equals has a different behavior from 
            // double.operator== with NaN:
            //    double.NaN.Equals(double.NaN) is true
            //    double.NaN == double.NaN is false (IEEE 754 specs)

            if (!x._notNull || !y._notNull) return NullableBoolean.Null;

            return (!x.Equals(y));
        }

        /// <summary>
        /// Compares two instances of <see cref="NullableDouble"/> for equivalence.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.False"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableDouble"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Double.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Double.NegativeInfinity"/> is considered equal to 
        /// another <see cref="System.Double.NegativeInfinity"/>; a 
        /// <see cref="System.Double.PositiveInfinity"/> is considered equal to 
        /// another <see cref="System.Double.PositiveInfinity"/>. 
        /// </remarks>
        public static NullableBoolean operator ==(NullableDouble x, NullableDouble y) {
            if (!x._notNull || !y._notNull) return NullableBoolean.Null;

            // Important: double.Equals has a different behavior from 
            // double.operator== with NaN:
            //    double.NaN.Equals(double.NaN) is true
            //    double.NaN == double.NaN is false (IEEE 754 specs)

            return new NullableBoolean (x._value == y._value);
        }

        /// <summary>
        /// Compares two instances of <see cref="NullableDouble"/> for equivalence.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are not 
        /// equivalent or <see cref="NullableBoolean.False"/> if the two instances 
        /// are equivalent. If either instance of <see cref="NullableDouble"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Double.NaN"/> the result is true; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Double.NegativeInfinity"/> is considered equal to 
        /// another <see cref="System.Double.NegativeInfinity"/>; a 
        /// <see cref="System.Double.PositiveInfinity"/> is considered equal to 
        /// another <see cref="System.Double.PositiveInfinity"/>. 
        /// </remarks>
        public static NullableBoolean operator != (NullableDouble x, NullableDouble y) {
            if (!x._notNull || !y._notNull) return NullableBoolean.Null;
            return new NullableBoolean (!(x._value == y._value));
        }




        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode () {
            // if IsNull then _value will always be 0D
            // hash code for Null will be the hash code of 0D
            return _value.GetHashCode();
        }
        #endregion // Equivalence

        #region Methods

        /// <summary>
        /// Returns the <see cref="TypeCode"/> for value type 
        /// <see cref="NullableDouble"/>.
        /// </summary>
        /// <returns>
        /// The enumerated constant, <see cref="TypeCode.NullableDouble"/>.
        /// </returns>
        public TypeCode GetTypeCode() {
            return TypeCode.NullableDouble;
        }

        /// <summary>
        /// Computes the sum of the two specified <see cref="NullableDouble"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDouble"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDouble"/> structure 
        /// whose <see cref="Value"/> property contains the results of the addition.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableDouble"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableSingle"/>, it becomes 
        /// <see cref="System.Double.PositiveInfinity"/> or 
        /// <see cref="System.Double.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Double.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableDouble Add(NullableDouble x, NullableDouble y) {
            return (x + y);
        }

        /// <summary>
        /// Divides its first <see cref="NullableDouble"/> operand by its second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDouble"/> is 
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
        /// <see cref="NullableDouble"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableDouble"/>, it becomes 
        /// <see cref="System.Double.PositiveInfinity"/> or 
        /// <see cref="System.Double.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Double.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableDouble Divide(NullableDouble x, NullableDouble y) {
            return (x / y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDouble"/> structure to 
        /// determine if the first is greater than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDouble"/> is <see cref="NullableDouble.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Double.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Double.NegativeInfinity"/> is considered less than all 
        /// other values; a <see cref="System.Double.PositiveInfinity"/> is 
        /// considered greater than all other values. 
        /// </remarks>
        public static NullableBoolean GreaterThan(NullableDouble x, NullableDouble y) {
            return (x > y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDouble"/> structure to 
        /// determine if the first is greater than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDouble"/> is <see cref="NullableDouble.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Double.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Double.NegativeInfinity"/> is considered less than all 
        /// other values, but equal to another 
        /// <see cref="System.Double.NegativeInfinity"/>; a 
        /// <see cref="System.Double.PositiveInfinity"/> is considered greater than 
        /// all other values, but equal to another 
        /// <see cref="System.Double.PositiveInfinity"/>.
        /// </remarks>
        public static NullableBoolean GreaterThanOrEqual(NullableDouble x, NullableDouble y) {
            return (x >= y);
        }

        /// <summary>
        /// Compares two instances of <see cref="NullableDouble"/> structure to 
        /// determine if the first is less than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDouble"/> is <see cref="NullableByte.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>        
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Double.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Double.NegativeInfinity"/> is considered less than all 
        /// other values; a <see cref="System.Double.PositiveInfinity"/> is 
        /// considered greater than all other values. 
        /// </remarks>
        public static NullableBoolean LessThan(NullableDouble x, NullableDouble y) {
            return (x < y);
        }

        /// <summary>
        /// Compares two instances of <see cref="NullableDouble"/> structure to 
        /// determine if the first is less than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDouble"/> is <see cref="NullableDouble.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Double.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Double.NegativeInfinity"/> is considered less than all 
        /// other values, but equal to another 
        /// <see cref="System.Double.NegativeInfinity"/>; a 
        /// <see cref="System.Double.PositiveInfinity"/> is considered greater than 
        /// all other values, but equal to another 
        /// <see cref="System.Double.PositiveInfinity"/>.
        /// </remarks>
        public static NullableBoolean LessThanOrEqual(NullableDouble x, NullableDouble y) {
            return (x <= y);
        }

        /// <summary>
        /// Computes the product of the two specified <see cref="NullableDouble"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDouble"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDouble"/> structure 
        /// whose <see cref="Value"/> property contains  the product of the 
        /// multiplication.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableDouble"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableDouble"/>, it becomes 
        /// <see cref="System.Double.PositiveInfinity"/> or 
        /// <see cref="System.Double.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Double.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableDouble Multiply(NullableDouble x, NullableDouble y) {
            return (x * y);
        }

        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a 
        /// number value to its <see cref="NullableDouble"/> equivalent.
        /// </summary>
        /// <param name="s">The <see cref="System.String"/> to be converted.</param>
        /// <returns>
        /// A <see cref="NullableDouble"/> structure containing the parsed value.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="s"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="s"/> s is not a number in a valid format.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="s"/> represents a number less than 
        /// <see cref="System.Double.MinValue"/> or greater than 
        /// <see cref="System.Double.MaxValue"/>.
        /// </exception>
        /// <remarks>
        /// The <paramref name="s"/> parameter can contain 
        /// <see cref="System.Globalization.NumberFormatInfo.PositiveInfinitySymbol"/>, 
        /// <see cref="System.Globalization.NumberFormatInfo.NegativeInfinitySymbol"/> 
        /// and 
        /// <see cref="System.Globalization.NumberFormatInfo.NaNSymbol"/>.
        /// </remarks>
        /// <seealso cref="System.Double.Parse"/>
        public static NullableDouble Parse(string s) {
            return new NullableDouble(double.Parse(s, sysGlb.CultureInfo.CurrentCulture.NumberFormat));
        }

        /// <summary>
        /// Computes the subtraction of the two specified <see cref="NullableDouble"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDouble"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableSingle"/> structure 
        /// whose <see cref="Value"/> property contains the results of the 
        /// subtraction.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableDouble"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableDouble"/>, it becomes 
        /// <see cref="System.Double.PositiveInfinity"/> or 
        /// <see cref="System.Double.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Double.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableDouble Subtract(NullableDouble x, NullableDouble y) {
            return (x - y);
        }

        #endregion // Methods

        #region Operators

        /// <summary>
        /// Computes the sum of the two specified <see cref="NullableDouble"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDouble"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDouble"/> structure 
        /// whose <see cref="Value"/> property contains the results of the addition.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableDouble"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableDouble"/>, it becomes 
        /// <see cref="System.Double.PositiveInfinity"/> or 
        /// <see cref="System.Double.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Double.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableDouble operator + (NullableDouble x, NullableDouble y) {
            if (x.IsNull || y.IsNull)
                return NullableDouble.Null;

            double res = x.Value + y.Value;
 
            return new NullableDouble(res);
        }


        /// <summary>
        /// Divides its first <see cref="NullableDouble"/> operand by its second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDouble"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDouble"/> structure 
        /// whose <see cref="Value"/> property contains the results of the division.
        /// </returns>
        /// <exception cref="System.DivideByZeroException">
        /// <paramref name="y"/> is <see cref="Zero"/> while <paramref name="x"/> is 
        /// not <see cref="Null"/>.
        /// </exception>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableDouble"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableDouble"/>, it becomes 
        /// <see cref="System.Double.PositiveInfinity"/> or 
        /// <see cref="System.Double.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Double.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableDouble operator / (NullableDouble x, NullableDouble y) {
            if (x.IsNull || y.IsNull)
                return NullableDouble.Null;

            if (y.Value == 0)
                throw new sys.DivideByZeroException();

            double res = x.Value / y.Value;

            return new NullableDouble(res);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDouble"/> structure to 
        /// determine if the first is greater than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDouble"/> is <see cref="NullableDouble.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Double.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Double.NegativeInfinity"/> is considered less than all 
        /// other values; a <see cref="System.Double.PositiveInfinity"/> is 
        /// considered greater than all other values. 
        /// </remarks>
        public static NullableBoolean operator > (NullableDouble x, NullableDouble y) {
            if (x.IsNull || y .IsNull) return NullableBoolean.Null;

            return new NullableBoolean (x.Value > y.Value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDouble"/> structure to 
        /// determine if the first is greater than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDouble"/> is <see cref="NullableDouble.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Double.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Double.NegativeInfinity"/> is considered less than all 
        /// other values, but equal to another 
        /// <see cref="System.Double.NegativeInfinity"/>; a 
        /// <see cref="System.Double.PositiveInfinity"/> is considered greater than 
        /// all other values, but equal to another 
        /// <see cref="System.Double.PositiveInfinity"/>.
        /// </remarks>
        public static NullableBoolean operator >= (NullableDouble x, NullableDouble y) {
            if (x.IsNull || y .IsNull) return NullableBoolean.Null;
            return new NullableBoolean (x.Value >= y.Value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDouble"/> structure to 
        /// determine if the first is less than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDouble"/> is <see cref="NullableByte.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>        
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Double.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Double.NegativeInfinity"/> is considered less than all 
        /// other values; a <see cref="System.Double.PositiveInfinity"/> is 
        /// considered greater than all other values. 
        /// </remarks>
        public static NullableBoolean operator < (NullableDouble x, NullableDouble y) {
            if (x.IsNull || y .IsNull) return NullableBoolean.Null;
            return new NullableBoolean (x.Value < y.Value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDouble"/> structure to 
        /// determine if the first is less than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDouble"/> is <see cref="NullableDouble.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        /// <remarks>
        /// This operator follows IEEE 754 specifications: if either operand 
        /// is <see cref="System.Double.NaN"/> the result is false; negative and 
        /// positive zero are considered equal; a 
        /// <see cref="System.Double.NegativeInfinity"/> is considered less than all 
        /// other values, but equal to another 
        /// <see cref="System.Double.NegativeInfinity"/>; a 
        /// <see cref="System.Double.PositiveInfinity"/> is considered greater than 
        /// all other values, but equal to another 
        /// <see cref="System.Double.PositiveInfinity"/>.
        /// </remarks>
        public static NullableBoolean operator <= (NullableDouble x, NullableDouble y) {
            if (x.IsNull || y .IsNull) return NullableBoolean.Null;
            return new NullableBoolean (x.Value <= y.Value);
        }


        /// <summary>
        /// Computes the product of the two specified <see cref="NullableDouble"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDouble"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDouble"/> structure 
        /// whose <see cref="Value"/> property contains  the product of the 
        /// multiplication.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableDouble"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableDouble"/>, it becomes 
        /// <see cref="System.Double.PositiveInfinity"/> or 
        /// <see cref="System.Double.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Double.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableDouble operator * (NullableDouble x, NullableDouble y) {
            if (x.IsNull || y .IsNull) return NullableDouble.Null;

            double res = x.Value * y.Value;

            return new NullableDouble(res);
        }


        /// <summary>
        /// Computes the subtraction of the two specified <see cref="NullableDouble"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <param name="y">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDouble"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDouble"/> structure 
        /// whose <see cref="Value"/> property contains the results of the 
        /// subtraction.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If the result of the operation is too small for a 
        /// <see cref="NullableDouble"/>, it becomes positive zero or negative zero. 
        /// </para>
        /// <para>
        /// If the result of the operation is too large for a 
        /// <see cref="NullableDouble"/>, it becomes 
        /// <see cref="System.Double.PositiveInfinity"/> or 
        /// <see cref="System.Double.NegativeInfinity"/>. 
        /// </para>
        /// <para>
        /// If the operation is invalid, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// <para>
        /// If one or both operands of the operation is 
        /// <see cref="System.Double.NaN"/>, the result of the operation becomes 
        /// <see cref="System.Double.NaN"/>. 
        /// </para>
        /// </remarks>
        public static NullableDouble operator - (NullableDouble x, NullableDouble y) {
            if (x.IsNull || y .IsNull) return NullableDouble.Null;

            double res = x.Value - y.Value;

            return new NullableDouble(res);
        }


        /// <summary>
        /// Negates the <see cref="Value"/> of the specified <see cref="NullableDouble"/>
        /// structure.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/>  is <see cref="Null"/> 
        /// otherwise a <see cref="NullableDouble"/> structure containing the 
        /// negated value.
        /// </returns>
        public static NullableDouble operator - (NullableDouble x) {
            if (x.IsNull) return NullableDouble.Null;

            return new NullableDouble(-(x.Value));
        }
        #endregion // Operators

        #region Conversion Operators
        /// <summary>
        /// Converts the <see cref="NullableBoolean"/> parameter to a 
        /// <see cref="NullableDouble"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableBoolean"/> to be converted to a 
        /// <see cref="NullableDouble"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableBoolean.Null"/> otherwise a new 
        /// <see cref="NullableDouble"/> structure constructed from the 
        /// <see cref="NullableBoolean.ByteValue"/> of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableDouble(NullableBoolean x) {
            if (x.IsNull)
                return Null;
            
            return new NullableDouble((double)x.ByteValue);
        }


        /// <summary>
        /// Converts a <see cref="NullableDouble"/> to a <see cref="System.Double"/>.
        /// </summary>
        /// <param name="x">A <see cref="NullableDouble"/> to convert.</param>
        /// <returns>
        /// A <see cref="System.Double"/> set to the <see cref="Value"/> of the 
        /// <see cref="NullableDouble"/>.
        /// </returns>
        /// <exception cref="NullableNullValueException">
        /// <paramref name="x"/> is <see cref="Null"/>.
        /// </exception>
        public static explicit operator double(NullableDouble x) {
            return x.Value;
        }


        /// <summary>
        /// Converts the specified <see cref="NullableString"/> parameter to a 
        /// <see cref="NullableDouble"/> structure.
        /// </summary>
        /// <param name="x">The <see cref="NullableString"/> to be converted.</param>
        /// <returns>
        /// <see cref="Null"/> If <paramref name="x"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise a new
        /// <see cref="NullableDouble"/> structure containing the parsed value.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> is not a number in a valid format.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> represents a number less than 
        /// <see cref="System.Double.MinValue"/> or greater than 
        /// <see cref="System.Double.MaxValue"/>.
        /// </exception>
        /// <remarks>
        /// The <paramref name="x"/> parameter can contain 
        /// <see cref="System.Globalization.NumberFormatInfo.PositiveInfinitySymbol"/>, 
        /// <see cref="System.Globalization.NumberFormatInfo.NegativeInfinitySymbol"/> 
        /// and 
        /// <see cref="System.Globalization.NumberFormatInfo.NaNSymbol"/>.
        /// </remarks>
        /// <seealso cref="System.Double.Parse"/>
        public static explicit operator NullableDouble(NullableString x) {
            if (x.IsNull)
                return Null;
                
            return NullableDouble.Parse(x.Value);
        }


        /// <summary>
        /// Converts the <see cref="System.Double"/> parameter to a 
        /// <see cref="NullableDouble"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="System.Double"/> to be converted to a 
        /// <see cref="NullableDouble"/> structure. 
        /// </param>
        /// <returns>
        /// A new <see cref="NullableDouble"/> structure constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableDouble(double x) {
            return new NullableDouble(x);
        }


        /// <summary>
        /// Converts the <see cref="NullableByte"/> parameter to a 
        /// <see cref="NullableDouble"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableByte"/> to be converted to a 
        /// <see cref="NullableDouble"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableByte.Null"/> otherwise a new 
        /// <see cref="NullableDouble"/> structure constructed from the 
        /// <see cref="NullableByte.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableDouble(NullableByte x) {
            if (x.IsNull) 
                return Null;

            return new NullableDouble((double)x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableDecimal"/> parameter to a 
        /// <see cref="NullableDouble"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDecimal"/> to be converted to a 
        /// <see cref="NullableDouble"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise a new 
        /// <see cref="NullableDouble"/> structure constructed from the 
        /// <see cref="NullableDecimal.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableDouble(NullableDecimal x) {
            if (x.IsNull) 
                return Null;

            // C# Language Specification   
            // 6.2.1 Explicit numeric conversions
            // ------------------------------------
            // For a conversion from decimal to float or double, the decimal value is rounded to the nearest 
            // double or float value. While this conversion may lose precision, it never causes an exception 
            // to be thrown.             
            return new NullableDouble((double)x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableInt16"/> parameter to a 
        /// <see cref="NullableDouble"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt16"/> to be converted to a 
        /// <see cref="NullableDouble"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt16.Null"/> otherwise a new 
        /// <see cref="NullableDouble"/> structure constructed from the 
        /// <see cref="NullableInt16.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableDouble(NullableInt16 x) {
            if (x.IsNull) 
                return Null;

            return new NullableDouble((double)x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableInt32"/> parameter to a 
        /// <see cref="NullableDouble"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt32"/> to be converted to a 
        /// <see cref="NullableDouble"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt32.Null"/> otherwise a new 
        /// <see cref="NullableDouble"/> structure constructed from the 
        /// <see cref="NullableInt32.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableDouble(NullableInt32 x) {
            if (x.IsNull) 
                return Null;

            return new NullableDouble((double)x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableInt64"/> parameter to a 
        /// <see cref="NullableDouble"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt64"/> to be converted to a 
        /// <see cref="NullableDouble"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt64.Null"/> otherwise a new 
        /// <see cref="NullableDouble"/> structure constructed from the 
        /// <see cref="NullableInt64.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableDouble(NullableInt64 x) {
            if (x.IsNull) 
                return Null;

            return new NullableDouble((double)x.Value);
        }





        /// <summary>
        /// Converts the <see cref="NullableSingle"/> parameter to a 
        /// <see cref="NullableDouble"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableSingle"/> to be converted to a 
        /// <see cref="NullableDouble"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise a new 
        /// <see cref="NullableDouble"/> structure constructed from the 
        /// <see cref="NullableSingle.Value"/> of <paramref name="x"/>.
        /// </returns>
        /// <remarks>
        /// The <paramref name="x"/> <see cref="NullableSingle"/> value is rounded 
        /// to the nearest <see cref="System.Double"/> value. If the Single value is 
        /// NaN, the result is also NaN. 
        /// </remarks>
        public static explicit operator NullableDouble(NullableSingle x) {
            if (x.IsNull)
                return Null;
            
            return new NullableDouble((double)x.Value);  
        }


        #endregion // Conversion Operators

        #region Conversions

        /// <summary>
        /// Converts this <see cref="NullableDouble"/> instance to a 
        /// <see cref="NullableBoolean"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.Null"/> if this <see cref="NullableDouble"/> is 
        /// <see cref="NullableDouble.Null"/>, <see cref="NullableBoolean.False"/> if
        /// this instance <see cref="NullableDouble.Value"/> is 
        /// <see cref="NullableDouble.Zero"/> otherwise 
        /// <see cref="NullableBoolean.True"/>.
        /// </returns>
        public NullableBoolean ToNullableBoolean() {
            return ((NullableBoolean)this);
        }
        

        /// <summary>
        /// Converts this <see cref="NullableDouble"/> instance to a 
        /// <see cref="NullableByte"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableByte"/> that is 
        /// <see cref="NullableByte.Null"/> if this <see cref="NullableDouble"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise a <see cref="NullableDouble"/> 
        /// whose <see cref="NullableByte.Value"/> equals the 
        /// <see cref="NullableDouble.Value"/> of this instance.
        /// </returns>    
        /// <exception cref="System.OverflowException">
        /// This <see cref="NullableDouble"/> <see cref="Value"/> is a number that 
        /// is less than  <see cref="System.Byte.MinValue"/> or greater than 
        /// <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public NullableByte ToNullableByte() {
            return ((NullableByte)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDouble"/> instance to a 
        /// <see cref="NullableDecimal"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableDecimal"/> that is 
        /// <see cref="NullableDecimal.Null"/> if this <see cref="NullableDouble"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise a <see cref="NullableDecimal"/> 
        /// whose <see cref="NullableDecimal.Value"/> equals the 
        /// <see cref="NullableDouble.Value"/> of this instance.
        /// </returns>    
        /// <exception cref="System.OverflowException">
        /// This <see cref="NullableDouble"/> <see cref="Value"/> is a number that 
        /// is less than <see cref="NullableDecimal.MinValue"/> or greater than 
        /// <see cref="NullableDecimal.MaxValue"/>
        /// <para>-or-</para>
        /// <see cref="Value"/> is <see cref="System.Double.NaN"/>, <see cref="System.Double.PositiveInfinity"/>, or 
        /// <see cref="System.Double.NegativeInfinity"/>.
        /// </exception>
        public NullableDecimal ToNullableDecimal() {
            return ((NullableDecimal)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDouble"/> instance to a 
        /// <see cref="NullableInt16"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt16"/> that is 
        /// <see cref="NullableInt16.Null"/> if this <see cref="NullableDouble"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise a <see cref="NullableInt16"/> 
        /// whose <see cref="NullableInt16.Value"/> equals the 
        /// <see cref="NullableDouble.Value"/> of this instance.
        /// </returns>    
        /// <exception cref="System.OverflowException">
        /// This <see cref="NullableDouble"/> <see cref="Value"/> is a number that 
        /// is less than  <see cref="NullableInt16.MinValue"/> or greater than 
        /// <see cref="NullableInt16.MaxValue"/>.
        /// </exception>
        public NullableInt16 ToNullableInt16() {
            return ((NullableInt16)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDouble"/> instance to a 
        /// <see cref="NullableInt32"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt32"/> that is 
        /// <see cref="NullableInt32.Null"/> if this <see cref="NullableDouble"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise a <see cref="NullableInt32"/> 
        /// whose <see cref="NullableInt32.Value"/> equals the 
        /// <see cref="NullableDouble.Value"/> of this instance.
        /// </returns>    
        /// <exception cref="System.OverflowException">
        /// This <see cref="NullableDouble"/> <see cref="Value"/> is a number that 
        /// is less than  <see cref="NullableInt32.MinValue"/> or greater than 
        /// <see cref="NullableInt32.MaxValue"/>.
        /// </exception>
        public NullableInt32 ToNullableInt32() {
            return ((NullableInt32)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDouble"/> instance to a 
        /// <see cref="NullableInt64"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt64"/> that is 
        /// <see cref="NullableInt64.Null"/> if this <see cref="NullableDouble"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise a <see cref="NullableInt64"/> 
        /// whose <see cref="NullableInt64.Value"/> equals the 
        /// <see cref="NullableDouble.Value"/> of this instance.
        /// </returns>    
        /// <exception cref="System.OverflowException">
        /// This <see cref="NullableDouble"/> <see cref="Value"/> is a number that 
        /// is less than  <see cref="NullableInt64.MinValue"/> or greater than 
        /// <see cref="NullableInt64.MaxValue"/>.
        /// </exception>
        public NullableInt64 ToNullableInt64() {
            return ((NullableInt64)this);
        }
        



        /// <summary>
        /// Converts this <see cref="NullableDouble"/> instance to a 
        /// <see cref="NullableSingle"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableSingle"/> that is 
        /// <see cref="NullableSingle.Null"/> if this <see cref="NullableDouble"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise a <see cref="NullableSingle"/> 
        /// whose <see cref="NullableSingle.Value"/> equals the 
        /// <see cref="NullableDouble.Value"/> of this instance.
        /// </returns>    
        /// <remarks>
        /// The <see cref="System.Double"/> value is rounded to the nearest 
        /// <see cref="System.Single"/> value. If the Double value is too small to 
        /// represent as a Single, the result becomes positive zero or negative zero. 
        /// If the Double value is too large to represent as a Single, the result 
        /// becomes positive infinity or negative infinity. If the Double value is 
        /// NaN, the result is also NaN. 
        /// </remarks>
        public NullableSingle ToNullableSingle() {
            return ((NullableSingle)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDouble"/> structure to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <returns>
        /// <see cref="NullableString.Null"/> if this <see cref="NullableDouble"/>
        /// is <see cref="NullableDouble.Null"/> otherwise
        /// a new <see cref="NullableString"/> structure whose 
        /// <see cref="NullableString.Value"/> is the description of this 
        /// <see cref="NullableDouble"/> structure's <see cref="Value"/>.
        /// </returns>
        public NullableString ToNullableString () {
            return ((NullableString)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDouble"/> structure to a 
        /// <see cref="System.String"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="System.String"/> that is a description of Null if this 
        /// <see cref="NullableDouble"/> is <see cref="NullableDouble.Null"/> 
        /// otherwise a new <see cref="System.String"/> that is the description of 
        /// this <see cref="NullableDouble"/> structure's <see cref="Value"/>.
        /// </returns>
        /// <seealso cref="System.Double.ToString()"/>
        public override string ToString () {
            if (this.IsNull)
                return Locale.GetText("Null");
            
            return _value.ToString(sysGlb.CultureInfo.CurrentCulture);
        }

        #endregion // Conversions
    }
}