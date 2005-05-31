//
// NullableTypes.NullableDecimal
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Abhijeet Dev (abhijeet_dev@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author   Changes     Reasons
// 11-Apr-2003  Luca     Create      Declared public members
// 17-Jun-2003  Luca     Create      Added XML documentation
// 24-Jun-2003  Abhijeet Create      Added method definitions.
// 27-Jun-2003  Luca     Refactoring Unified equivalent error messages 
// 30-Jun-2003  Luca     Upgrade     New requirement by Roni Burd: GetTypeCode 
// 02-Jul-2003  Luca     Bug Fix     Removed bugs to pass tests
// 07-Jul-2003  Luca     Upgrade     Applied FxCop guideline: explicited CurrentCulture in CompareTo,
//                                   ToString, Parse, NullableString conv. and Equals code
// 13-Sep-2003  Luca     Upgrade     New requirement: the type must be sarializable
// 05-Oct-2003  DamienG  Upgrade     New requirement: the type must be XmlSerializable
// 06-Oct-2003  Luca     Upgrade     Code upgrade: Replaced tabs with spaces and removed commented out code
// 06-Dic-2003  Luca     Bug Fix     Replaced Xml Schema id "NullableDecimalXmlSchema" with "NullableDecimal"
//                                   because VSDesigner use it as type-name in the auto-generated WS client Proxy
// 06-Dic-2003  Luca     Bug Fix     Replaced Target Namespace for Xml Schema to avoid duplicate namespace with
//                                   other types in NullableTypes
// 18-Feb-2004  Luca     Bug Fix     ReadXml must read nil value also with a non empty element
//

namespace NullableTypes {
    using sys = System;
    using sysGlb = System.Globalization;
    using sysXml = System.Xml;
    using sysXmlSrl = System.Xml.Serialization;
    using sysXmlScm = System.Xml.Schema;


    // Note for the implementer: NullableDecimal is more similar to System.Decimal then SqlDecimal
    // because SqlDecimal have a wider value domain and it explicity manage precision and scale.
    // Instead we want NullableDecimal do be compatible to System.Decimal without the needs for 
    // narrowing conversions.

    // Note for the implementer: System.Decimal is not a primitive type for the CLR (the CLR do
    // not have IL instructions for it) so it must provide its operators overload. Besides this 
    // CHECKED and UNCHECKED statements, operators or compiler command-line options have no 
    // effect for operations on System.Decimal

    /// <summary>
    /// Represents a Decimal that is either a decimal number value or 
    /// <see cref="Null"/>.
    /// </summary>
    [sys.Serializable]
    public struct NullableDecimal : INullable, sys.IComparable, sysXmlSrl.IXmlSerializable {

        #region Fields
        private decimal _value;
        private bool _notNull;

        /// <summary>
        /// Represents a Null value that can be assigned to an instance of the 
        /// <see cref="NullableDecimal"/> structure.
        /// <para>
        /// A Null is the absence of a value because missing, unknown, or inapplicable 
        /// value. A Null should not be used to imply any other value (such as zero).
        /// Also any value (such as zero) should not be used to imply the absence of a 
        /// value, that's why Null exists.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Null field is a constant of the <see cref="NullableDecimal"/> structure.
        /// </remarks>
        public static readonly NullableDecimal Null;

        /// <summary>
        /// A constant representing the largest possible value of a 
        /// <see cref="NullableDecimal"/>.
        /// </summary>
        /// <remarks>
        /// The value for this constant is positive 
        /// 79.228.162.514.264.337.593.543.950.335.
        /// </remarks>
        public static readonly NullableDecimal MaxValue = new NullableDecimal(decimal.MaxValue);

        /// <summary>
        /// A constant representing the smallest possible value of a 
        /// <see cref="NullableDecimal"/>.
        /// </summary>
        /// <remarks>
        /// The value for this constant is negative 
        /// 79.228.162.514.264.337.593.543.950.335.
        /// </remarks>
        public static readonly NullableDecimal MinValue = new NullableDecimal(decimal.MinValue);

        /// <summary>
        /// Represents the number negative one (-1) that can be assigned to an 
        /// instance of the <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <remarks>
        /// Zero field is a constant of the <see cref="NullableDecimal"/> structure.
        /// </remarks>
        public static readonly NullableDecimal MinusOne = new NullableDecimal(decimal.MinusOne);

        /// <summary>
        /// Represents the number positive one (1) that can be assigned to an 
        /// instance of the <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <remarks>
        /// Zero field is a constant of the <see cref="NullableDecimal"/> structure.
        /// </remarks>        
        public static readonly NullableDecimal One = new NullableDecimal(decimal.One);

        /// <summary>
        /// Represents a zero value that can be assigned to an instance of the 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <remarks>
        /// Zero field is a constant of the <see cref="NullableDecimal"/> structure.
        /// </remarks>
        public static readonly NullableDecimal Zero = new NullableDecimal(decimal.Zero);
        #endregion // Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NullableDecimal"/> structure 
        /// using the specified decimal value.
        /// </summary>
        /// <param name="value">
        /// A decimal value to be stored in the <see cref="Value"/> property of the 
        /// new <see cref="NullableDecimal"/> structure. 
        /// </param>
        public NullableDecimal(decimal value) {
            _value = value;
            _notNull = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableDecimal"/> structure 
        /// using the specified double-precision floating-point number.
        /// </summary>
        /// <param name="value">
        /// A double-precision floating-point number value to be stored in the 
        /// <see cref="Value"/> property of the new <see cref="NullableDecimal"/> 
        /// structure. 
        /// </param>
        /// <exception cref="System.OverflowException">
        /// <paramref name="value"/> is greater than <see cref="MaxValue"/> or 
        /// less than <see cref="MinValue"/>
        /// <para>-or-</para>
        /// <paramref name="value"/> is <see cref="System.Double.NaN"/>, 
        /// <see cref="System.Double.PositiveInfinity"/>, or 
        /// <see cref="System.Double.NegativeInfinity"/>.
        /// </exception>        
        public NullableDecimal(double value) {
            _value = new decimal(value);
            _notNull = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableDecimal"/> structure 
        /// using the supplied integer value.
        /// </summary>
        /// <param name="value">
        /// An integer value to be stored in the <see cref="Value"/> property of the 
        /// new <see cref="NullableDecimal"/> structure. 
        /// </param>
        public NullableDecimal(int value) {
            _value = new decimal(value);
            _notNull = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableDecimal"/> structure 
        /// using a decimal value represented in binary and contained in a 
        /// specified array.
        /// </summary>
        /// <param name="bits">
        /// An array representing in binary a decimal value to be stored in the 
        /// <see cref="Value"/> property of the new <see cref="NullableDecimal"/> 
        /// structure.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="bits"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The length of the <paramref name="bits"/> is not 4
        /// <para>-or-</para>
        /// the representation of the decimal value in <paramref name="bits"/> is 
        /// not valid.
        /// </exception>
        public NullableDecimal(int[] bits) {
            _value = new decimal(bits);            
            _notNull = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableDecimal"/> structure 
        /// using the of the specified 64-bit signed integer.
        /// </summary>
        /// <param name="value">
        /// A 64-bit signed integer value to be stored in the <see cref="Value"/> 
        /// property of the new <see cref="NullableDecimal"/> structure. 
        /// </param>
        public NullableDecimal(long value) {
            _value = new decimal(value);
            _notNull = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableDecimal"/> structure 
        /// using the specified floating-point number.
        /// </summary>
        /// <param name="value">
        /// A floating-point number value to be stored in the <see cref="Value"/> 
        /// property of the new <see cref="NullableDecimal"/> 
        /// structure. 
        /// </param>
        /// <exception cref="System.OverflowException"> 
        /// <paramref name="value"/> is greater than <see cref="MaxValue"/> or less 
        /// than <see cref="MinValue"/>
        /// <para>-or-</para>
        /// <paramref name="value"/> is <see cref="System.Single.NaN"/>, 
        /// <see cref="System.Single.PositiveInfinity"/>, or 
        /// <see cref="System.Single.NegativeInfinity"/>.
        /// </exception>
        public NullableDecimal(float value) {
            _value = new decimal(value);
            _notNull = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableDecimal"/> structure 
        /// using the specified 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">
        /// A 32-bit unsigned integer value to be stored in the <see cref="Value"/> 
        /// property of the new <see cref="NullableDecimal"/> structure. 
        /// </param>
        [sys.CLSCompliant(false)]
        public NullableDecimal(uint value) {
            _value = new decimal(value);
            _notNull = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableDecimal"/> structure 
        /// using the of the specified 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">
        /// A 64-bit unsigned integer value to be stored in the <see cref="Value"/> 
        /// property of the new <see cref="NullableDecimal"/> structure. 
        /// </param>
        [sys.CLSCompliant(false)]
        public NullableDecimal(ulong value) {
            _value = new decimal(value);
            _notNull = true;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableDecimal"/> structure 
        /// from parameters specifying the instance's constituent parts.
        /// </summary>
        /// <param name="lo">The low 32 bits of a 96-bit integer.</param>
        /// <param name="mid">The middle 32 bits of a 96-bit integer.</param>
        /// <param name="hi">The high 32 bits of a 96-bit integer.</param>
        /// <param name="isNegative">
        /// The sign of the number; true is negative, false is positive.
        /// </param>
        /// <param name="scale">A power of 10 ranging from 0 to 28.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="scale"/> is greater than 28.
        /// </exception>
        /// <remarks>
        /// The binary representation of a <see cref="System.Decimal"/> number 
        /// consists of a 1-bit sign, a 96-bit integer number, and a scaling factor 
        /// used to divide the integer number and specify what portion of it is a 
        /// decimal fraction. The scaling factor is implicitly the number 10 raised 
        /// to an exponent ranging from 0 to 28.
        /// </remarks>
        public NullableDecimal(int lo, int mid, int hi, bool isNegative, byte scale) {
            _value = new decimal(lo, mid, hi, isNegative, scale);
            _notNull = true;
        }
        #endregion // Constructors

        #region INullable
        /// <summary>
        /// Indicates whether or not the value of the <see cref="NullableDecimal"/> 
        /// structure is null.
        /// </summary>
        /// <value>
        /// true if the <see cref="NullableDecimal"/> structure is null, otherwise 
        /// false.
        /// </value>
        public bool IsNull {
            get { return !_notNull; }
        }
        #endregion // INullable

        #region IComparable - Ordering
        /// <summary>
        /// Compares this <see cref="NullableDecimal"/> structure to a specified 
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
        /// Any instance of <see cref="NullableDecimal"/> , regardless of its 
        /// <paramref name="value"/>, is considered greater than a null reference 
        /// (Nothing in Visual Basic) and <see cref="Null"/>. 
        /// </remarks>
        /// <exception cref="System.ArgumentException">
        /// The <paramref name="value"/> is neither null or of type 
        /// <see cref="NullableDecimal"/>.
        /// </exception>
        public int CompareTo(object value) {
            if (value == null)
                return 1;
            
            if (!(value is NullableDecimal))
                throw new sys.ArgumentException(string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText("Value is not a {0}."), "NullableTypes.NullableDecimal"));
            
            NullableDecimal dValue = (NullableDecimal)value;
            if (dValue.IsNull && this.IsNull)
                return 0;

            if (dValue.IsNull)
                return 1;

            if (this.IsNull)
                return -1;
            
            return _value.CompareTo(dValue.Value);
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
            //            targetNamespace="http://NullableTypes.SourceForge.Net/NullableDecimalXMLSchema"
            //            xmlns="http://NullableTypes.SourceForge.Net/NullableDecimalXMLSchema"
            //            elementFormDefault="qualified">
            //        <xs:element name="NullableDecimal" type="xs:decimal" nillable="true" />
            //    </xs:schema>

            // Element: NullableDecimal
            sysXmlScm.XmlSchemaElement rootElement = new sysXmlScm.XmlSchemaElement();
            rootElement.Name = "NullableDecimal";
            rootElement.SchemaTypeName = 
                new sysXml.XmlQualifiedName("decimal", "http://www.w3.org/2001/XMLSchema");
            rootElement.IsNillable = true;

            // Xml Schema
            sysXmlScm.XmlSchema xsd = new sysXmlScm.XmlSchema();
            xsd.Id = "NullableDecimal";
            xsd.Namespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
            xsd.TargetNamespace = "http://NullableTypes.SourceForge.Net/NullableDecimalXMLSchema";
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
                _value = sysXml.XmlConvert.ToDecimal(elementValue);
                _notNull = true;
            }
            else
                _notNull = false;
        }
        #endregion // IXmlSerializable

        #region Properties
        /// <summary>
        /// Gets the value of the <see cref="NullableDecimal"/> structure. This 
        /// property is read-only
        /// </summary>
        /// <value>The value of the <see cref="NullableDecimal"/> structure.</value>
        /// <exception cref="NullableNullValueException">
        /// The property is set to null.
        /// </exception>   
        public decimal Value {
            get { 
                if (this.IsNull) 
                    throw new NullableNullValueException();

                return _value; 
            }
        }

        /// <summary>
        /// Indicates whether or not the value of the <see cref="NullableDecimal"/> 
        /// structure is greater than zero.
        /// </summary>
        /// <value>
        /// true if the <see cref="NullableDecimal"/> structure is greater than zero,
        /// otherwise false.
        /// </value>
        /// <exception cref="NullableNullValueException">
        /// The property is set to null.
        /// </exception>  
        public bool IsPositive {
            get { 
                if (this.IsNull) 
                    throw new NullableNullValueException();

                return (_value > 0M); 
            }
        }
        #endregion // Properties

        #region Equivalence
        /// <summary>
        /// Compares two <see cref="NullableDecimal"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="value">The object to be compared. </param>
        /// <returns>
        /// true if object is an instance of <see cref="NullableDecimal"/> and the 
        /// two instances are equivalent; otherwise false.
        /// </returns>
        public override bool Equals(object value) {            

            if (!(value is NullableDecimal))
                throw new sys.ArgumentException(string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText("Value is not a {0}."), "NullableTypes.NullableDecimal"));
            NullableDecimal val = (NullableDecimal)value;
            if (val.IsNull) {
                if (this.IsNull) return true;
                return false;
            }
            if (this.IsNull) return false;
            return val.Value.Equals(this.Value);
            
        }


        /// <summary>
        /// Compares two <see cref="NullableDecimal"/> structures to determine if 
        /// they are not equivalent.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.False"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.True"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean NotEquals(NullableDecimal x, NullableDecimal y) {
            return (x != y);
        }


        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() {
            // if IsNull then _value will always be 0
            // hash code for Null will be (0M).GetHashCode()
            return _value.GetHashCode();
        }


        /// <summary>
        /// Compares two <see cref="NullableDecimal"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.False"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>        
        public static NullableBoolean Equals(NullableDecimal x, NullableDecimal y) {
            return (x == y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDecimal"/> for equivalence.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.False"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean operator == (NullableDecimal x, NullableDecimal y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;

            return new NullableBoolean(x.Value == y.Value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDecimal"/> for equivalence.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.False"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.True"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean operator != (NullableDecimal x, NullableDecimal y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;

            return new NullableBoolean(x.Value != y.Value);
        }
        #endregion // Equivalence

        #region Methods
        /// <summary>
        /// Returns the <see cref="TypeCode"/> for value type 
        /// <see cref="NullableDecimal"/>.
        /// </summary>
        /// <returns>
        /// The enumerated constant, <see cref="TypeCode.NullableDecimal"/>.
        /// </returns>
        public TypeCode GetTypeCode() {
            return TypeCode.NullableDecimal;
        }


        /// <summary>
        /// Computes absolute value of the specified <see cref="NullableDecimal"/> 
        /// structure.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDecimal"/> structure 
        /// whose <see cref="Value"/> property contains the absolute value.
        /// </returns>        
        public static NullableDecimal Abs(NullableDecimal x) {
            // if IsNull x._value will be 0
            if (x._value < 0M)
                x._value = sys.Decimal.Negate(x._value);
            return x;
        }


        /// <summary>
        /// Computes the sum of the two specified <see cref="NullableDecimal"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDecimal"/> structure 
        /// whose <see cref="Value"/> property contains the results of the addition.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The addition compute a number that is less than 
        /// <see cref="NullableDecimal.MinValue"/> or greater than 
        /// <see cref="NullableDecimal.MaxValue"/>.
        /// </exception>
        public static NullableDecimal Add(NullableDecimal x, NullableDecimal y) {
            return (x+y);
        }


        /// <summary>
        /// Divides its first <see cref="NullableDecimal"/> operand by its second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDecimal"/> structure 
        /// whose <see cref="Value"/> property contains the results of the division.
        /// </returns>
        /// <exception cref="System.DivideByZeroException">
        /// <paramref name="y"/> is <see cref="Zero"/> while <paramref name="x"/> is 
        /// not <see cref="Null"/>.
        /// </exception>
        public static NullableDecimal Divide(NullableDecimal x, NullableDecimal y) {
            return (x/y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDecimal"/> structure to 
        /// determine if the first is greater than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDecimal"/> is <see cref="NullableDecimal.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean GreaterThan (NullableDecimal x, NullableDecimal y) {
            return (x > y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDecimal"/> structure to 
        /// determine if the first is greater than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDecimal"/> is <see cref="NullableDecimal.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean GreaterThanOrEqual (NullableDecimal x, NullableDecimal y) {
            return (x >= y);
        }
                

        /// <summary>
        /// Compares two instances of <see cref="NullableDecimal"/> structure to 
        /// determine if the first is less than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDecimal"/> is <see cref="NullableDecimal.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>        
        public static NullableBoolean LessThan(NullableDecimal x, NullableDecimal y) {
            return (x < y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDecimal"/> structure to 
        /// determine if the first is less than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDecimal"/> is <see cref="NullableDecimal.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean LessThanOrEqual(NullableDecimal x, NullableDecimal y) {
            return (x <= y);
        }


        /// <summary>
        /// Computes the product of the two specified <see cref="NullableDecimal"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDecimal"/> structure 
        /// whose <see cref="Value"/> property contains  the product of the 
        /// multiplication.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The product  compute a number that is less than 
        /// <see cref="NullableDecimal.MinValue"/> or greater than 
        /// <see cref="NullableDecimal.MaxValue"/>.
        /// </exception>        
        public static NullableDecimal Multiply(NullableDecimal x, NullableDecimal y) {
            return (x * y);
        }
                

        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a 
        /// number to its <see cref="NullableDecimal"/> equivalent.
        /// </summary>
        /// <param name="s">The <see cref="System.String"/> to be converted.</param>
        /// <returns>
        /// A <see cref="NullableDecimal"/> structure containing the parsed value.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="s"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="s"/> does not consist solely of an optional sign followed 
        /// by a sequence of digits ranging from 0 to 9 and optionally a 
        /// culture-specific decimal point symbol followed by a sequence of digits 
        /// ranging from 0 to 9.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="s"/> represents a number less than 
        /// <see cref="NullableDecimal.MinValue"/> or greater than <see cref="NullableDecimal.MaxValue"/>.
        /// </exception>
        public static NullableDecimal Parse(string s) {
            return new NullableDecimal(sys.Decimal.Parse(s, sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Computes the subtraction of the two specified <see cref="NullableDecimal"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableInt32"/> structure 
        /// whose <see cref="Value"/> property contains the results of the 
        /// subtraction.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The subtraction compute a number that is less than 
        /// <see cref="NullableDecimal.MinValue"/> or greater than 
        /// <see cref="NullableDecimal.MaxValue"/>.
        /// </exception>
        public static NullableDecimal Subtract(NullableDecimal x, NullableDecimal y) {
            return (x-y);
        }
                

        /// <summary>
        /// Computes the smallest whole number greater than or equal to the specified 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is <see cref="Null"/> 
        /// otherwise the smallest whole number greater than or equal to 
        /// <paramref name="x"/>.
        /// </returns>         
        public static NullableDecimal Ceiling(NullableDecimal x) {
            if (x.IsNull) return NullableDecimal.Null;
            if (x._value == decimal.Truncate(x._value)) return x;
            return new NullableDecimal(decimal.Floor(x.Value) + 1);
        }


        /// <summary>
        /// Rounds the specified <see cref="NullableDecimal"/> to the closest integer 
        /// toward negative infinity.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is <see cref="Null"/> 
        /// otherwise if <paramref namne="x"/> has a fractional part, the next whole 
        /// number toward negative infinity that is less than <paramref name="x"/>
        /// <papa>-or-</papa>
        /// if <paramref namne="x"/> doesn't have a fractional part, 
        /// <paramref namne="x"/> is returned unchanged.
        /// </returns>   
        public static NullableDecimal Floor(NullableDecimal x) {
            if (x.IsNull) return NullableDecimal.Null;
            else return new NullableDecimal(decimal.Floor(x.Value));
        }


        /// <summary>
        /// Rounds a <see cref="NullableDecimal"/> value to a specified 
        /// number of decimal places.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="decimals">
        /// A value from 0 to 28 that specifies the number of decimal places to 
        /// round to. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is <see cref="Null"/> 
        /// otherwise a number equivalent to <paramref namne="x"/> rounded to 
        /// <paramref nane="decimals"/> number of decimal places.
        /// </returns>   
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref nane="decimals"/> is not a value from 0 to 28.
        /// </exception>
        public static NullableDecimal Round(NullableDecimal x, int decimals) {
            if (x.IsNull) return NullableDecimal.Null;
            else return new NullableDecimal(decimal.Round(x.Value, decimals));
        }


        /// <summary>
        /// Returns the integral digits of the specified 
        /// <see cref="NullableDecimal"/> value to a specified 
        /// while any fractional digits are discarded
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is <see cref="Null"/> 
        /// otherwise the result of <paramref name="x"/> rounded toward zero, to the 
        /// </returns>   
        /// <remarks>
        /// This method rounds <paramref name="x"/> discarding any digits after 
        /// the decimal point.
        /// </remarks>
        public static NullableDecimal Truncate(NullableDecimal x) {
            if (x.IsNull) return NullableDecimal.Null;
            else return new NullableDecimal(decimal.Truncate(x.Value));
        }


        /// <summary>
        /// Gets a value indicating the sign of a specified 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="NullableInt32.Null"/> if <paramref name="x"/> is 
        /// <see cref="Null"/>, <see cref="NullableInt32.Zero"/> if 
        /// <paramref name="x"/> is <see cref="NullableDecimal.Zero"/>
        /// otherwise +1 <paramref name="x"/> is greater then zero or -1 if 
        /// <paramref name="x"/> is less then zero.
        /// </returns>   
        public static NullableInt32 Sign(NullableDecimal x) {
            if (x.IsNull) return NullableInt32.Null;
            if (x._value == decimal.Zero) return NullableInt32.Zero;
            if (x._value > 0M) return new NullableInt32(1);
            else return new NullableInt32(-1);
        }
        #endregion // Methods

        #region Operators
        /// <summary>
        /// Computes the sum of the two specified <see cref="NullableDecimal"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDecimal"/> structure 
        /// whose <see cref="Value"/> property contains the results of the addition.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The addition compute a number that is less than 
        /// <see cref="NullableDecimal.MinValue"/> or greater than
        /// <see cref="NullableDecimal.MaxValue"/>.
        /// </exception>
        public static NullableDecimal operator + (NullableDecimal x, NullableDecimal y) {
            if (x.IsNull || y.IsNull)
                return NullableDecimal.Null;

            decimal res = decimal.Add(x.Value, y.Value);
 
            return new NullableDecimal(res);        
        }


        /// <summary>
        /// Divides its first <see cref="NullableDecimal"/> operand by its second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDecimal"/> structure 
        /// whose <see cref="Value"/> property contains the results of the division.
        /// </returns>
        /// <exception cref="System.DivideByZeroException">
        /// <paramref name="y"/> is <see cref="Zero"/> while <paramref name="x"/> is 
        /// not <see cref="Null"/>.
        /// </exception>
        public static NullableDecimal operator / (NullableDecimal x, NullableDecimal y) {
            if (x.IsNull || y.IsNull)
                return NullableDecimal.Null;

            decimal res = decimal.Divide(x._value, y._value);
 
            return new NullableDecimal(res);        
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDecimal"/> structure to 
        /// determine if the first is greater than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDecimal"/> is <see cref="NullableDecimal.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean operator > (NullableDecimal x, NullableDecimal y) {
            if (x.IsNull || y.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean(decimal.Compare(x._value, y._value) > 0);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDecimal"/> structure to 
        /// determine if the first is greater than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDecimal"/> is <see cref="NullableDecimal.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is greater 
        /// than or equal to the second instance, otherwise 
        /// <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean operator >= (NullableDecimal x, NullableDecimal y) {
            if (x.IsNull || y.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean(!(decimal.Compare(x._value, y._value) < 0));
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDecimal"/> structure to 
        /// determine if the first is less than the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDecimal"/> is <see cref="NullableDecimal.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less 
        /// than the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean operator < (NullableDecimal x, NullableDecimal y) {
            if (x.IsNull || y.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean(decimal.Compare(x._value, y._value) < 0);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableDecimal"/> structure to 
        /// determine if the first is less than or equal to the second.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> structure that is 
        /// <see cref="NullableBoolean.Null"/> if either instance of 
        /// <see cref="NullableDecimal"/> is <see cref="NullableDecimal.Null"/>, 
        /// <see cref="NullableBoolean.True"/> if the first instance is less than or 
        /// equal to the second instance, otherwise <see cref="NullableBoolean.False"/>. 
        /// </returns>
        public static NullableBoolean operator <= (NullableDecimal x, NullableDecimal y) {
            if (x.IsNull || y.IsNull)
                return NullableBoolean.Null;

            return new NullableBoolean(!(decimal.Compare(x._value, y._value) > 0));
        }


        /// <summary>
        /// Computes the product of the two specified <see cref="NullableDecimal"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDecimal"/> structure 
        /// whose <see cref="Value"/> property contains  the product of the 
        /// multiplication.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The product  compute a number that is less than 
        /// <see cref="NullableDecimal.MinValue"/> or greater than 
        /// <see cref="NullableDecimal.MaxValue"/>.
        /// </exception>
        public static NullableDecimal operator * (NullableDecimal x, NullableDecimal y) {
            if (x.IsNull || y.IsNull)
                return NullableDecimal.Null;

            decimal res = decimal.Multiply(x._value, y._value);
 
            return new NullableDecimal(res);        
        }


        /// <summary>
        /// Computes the subtraction of the two specified <see cref="NullableDecimal"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <param name="y">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableDecimal"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableDecimal"/> structure 
        /// whose <see cref="Value"/> property contains the results of the 
        /// subtraction.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The subtraction compute a number that is less than 
        /// <see cref="NullableDecimal.MinValue"/> or greater than 
        /// <see cref="NullableDecimal.MaxValue"/>.
        /// </exception>
        public static NullableDecimal operator - (NullableDecimal x, NullableDecimal y) {
            if (x.IsNull || y.IsNull)
                return NullableDecimal.Null;

            decimal res = decimal.Subtract(x._value, y._value);
 
            return new NullableDecimal(res);        
        }


        /// <summary>
        /// Negates the value of the specified <see cref="NullableDecimal"/> 
        /// operand.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is <see cref="Null"/> 
        /// otherwise the value of <paramref name="x"/> multiplied by negative one.
        /// </returns>
        public static NullableDecimal operator - (NullableDecimal x) {
            if (x.IsNull)
                return NullableDecimal.Null;

            decimal res = decimal.Negate(x._value);
 
            return new NullableDecimal(res); 
        }
        #endregion // Operators

        #region Conversion Operators
        /// <summary>
        /// Converts the <see cref="NullableBoolean"/> parameter to a 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableBoolean"/> to be converted to a 
        /// <see cref="NullableDecimal"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableBoolean.Null"/> otherwise a new 
        /// <see cref="NullableDecimal"/> structure constructed from the 
        /// <see cref="NullableBoolean.ByteValue"/> of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableDecimal(NullableBoolean x) {
            if (x.IsNull) return NullableDecimal.Null;
            return new NullableDecimal(x.ByteValue);
        }


        /// <summary>
        /// Converts a <see cref="NullableDecimal"/> to a <see cref="System.Decimal"/>.
        /// </summary>
        /// <param name="x">A <see cref="NullableDecimal"/> to convert.</param>
        /// <returns>
        /// A <see cref="System.Decimal"/> set to the <see cref="Value"/> of the 
        /// <see cref="NullableInt32"/>.
        /// </returns>
        /// <exception cref="NullableNullValueException">
        /// <paramref name="x"/> is <see cref="Null"/>.
        /// </exception>
        public static explicit operator decimal(NullableDecimal x) {
            return x.Value;
        }


        /// <summary>
        /// Converts the <see cref="NullableSingle"/> parameter to a 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableSingle"/> to be converted to a 
        /// <see cref="NullableDecimal"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise a new 
        /// <see cref="NullableDecimal"/> structure constructed from the 
        /// <see cref="NullableSingle.Value"/> of <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is a number that is less than 
        /// <see cref="NullableDecimal.MinValue"/> or greater than 
        /// <see cref="NullableDecimal.MaxValue"/>
        /// <para>-or-</para>
        /// value is <see cref="System.Single.NaN"/>, <see cref="System.Single.PositiveInfinity"/>, or 
        /// <see cref="System.Single.NegativeInfinity"/>.
        /// </exception>
        public static explicit operator NullableDecimal(NullableSingle x) {
            if (x.IsNull) return NullableDecimal.Null;
            return new NullableDecimal((sys.Decimal)(x.Value));
        }


        /// <summary>
        /// Converts the specified <see cref="NullableString"/> parameter to a 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <param name="x">The <see cref="NullableString"/> to be converted.</param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableString.Null"/> otherwise a new 
        /// <see cref="NullableDecimal"/> structure containing the parsed value.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> does not consist solely of an optional sign followed 
        /// by a sequence of digits ranging from 0 to 9 and optionally a 
        /// culture-specific decimal point symbol followed by a sequence of digits 
        /// ranging from 0 to 9.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> represents a number less than 
        /// <see cref="NullableDecimal.MinValue"/> or greater than 
        /// <see cref="NullableDecimal.MaxValue"/>.
        /// </exception>
        public static explicit operator NullableDecimal(NullableString x) {
            if (x.IsNull) return NullableDecimal.Null;
            return new NullableDecimal(sys.Decimal.Parse(x.Value, sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Converts the <see cref="NullableDouble"/> parameter to a 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDouble"/> to be converted to a 
        /// <see cref="NullableDecimal"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise a new 
        /// <see cref="NullableDecimal"/> structure constructed from the 
        /// <see cref="NullableDouble.Value"/> of <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is a number that is less than 
        /// <see cref="NullableDecimal.MinValue"/> or greater than 
        /// <see cref="NullableDecimal.MaxValue"/>
        /// <para>-or-</para>
        /// value is <see cref="System.Double.NaN"/>, <see cref="System.Double.PositiveInfinity"/>, or 
        /// <see cref="System.Double.NegativeInfinity"/>.
        /// </exception>
        public static explicit operator NullableDecimal(NullableDouble x) {
            if (x.IsNull) return NullableDecimal.Null;
            return new NullableDecimal((sys.Decimal)(x.Value));
        }


        /// <summary>
        /// Converts the <see cref="System.Decimal"/> parameter to a 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="System.Decimal"/> to be converted to a 
        /// <see cref="NullableDecimal"/> structure. 
        /// </param>
        /// <returns>
        /// A new <see cref="NullableDecimal"/> structure constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableDecimal(decimal x) {
            return new NullableDecimal(x);
        }


        /// <summary>
        /// Converts the <see cref="NullableByte"/> parameter to a 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableByte"/> to be converted to a 
        /// <see cref="NullableDecimal"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableByte.Null"/> otherwise a new 
        /// <see cref="NullableDecimal"/> structure constructed from the 
        /// <see cref="NullableByte.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableDecimal(NullableByte x) {
            if (x.IsNull) return NullableDecimal.Null;
            return new NullableDecimal((int)x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableInt16"/> parameter to a 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt16"/> to be converted to a 
        /// <see cref="NullableDecimal"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt16.Null"/> otherwise a new 
        /// <see cref="NullableDecimal"/> structure constructed from the 
        /// <see cref="NullableInt16.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableDecimal(NullableInt16 x) {
            if (x.IsNull) return NullableDecimal.Null;
            return new NullableDecimal((int)(x.Value));
        }


        /// <summary>
        /// Converts the <see cref="NullableInt32"/> parameter to a 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt32"/> to be converted to a 
        /// <see cref="NullableDecimal"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt32.Null"/> otherwise a new 
        /// <see cref="NullableDecimal"/> structure constructed from the 
        /// <see cref="NullableInt32.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableDecimal(NullableInt32 x) {
            if (x.IsNull) return NullableDecimal.Null;
            return new NullableDecimal(x.Value);
        }


        /// <summary>
        /// Converts the <see cref="NullableInt64"/> parameter to a 
        /// <see cref="NullableDecimal"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt64"/> to be converted to a 
        /// <see cref="NullableDecimal"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt64.Null"/> otherwise a new 
        /// <see cref="NullableDecimal"/> structure constructed from the 
        /// <see cref="NullableInt64.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static implicit operator NullableDecimal(NullableInt64 x) {
            if (x.IsNull) return NullableDecimal.Null;
            return new NullableDecimal(x.Value);
        }
        #endregion // Conversion Operators

        #region Conversions
        /// <summary>
        /// Converts this <see cref="NullableDecimal"/> instance to a 
        /// <see cref="System.Double"/>.
        /// </summary>
        /// <returns>
        /// A  <see cref="System.Double"/> whose value equals the 
        /// <see cref="NullableDecimal.Value"/> of this instance.
        /// </returns>    
        /// <exception cref="NullableNullValueException">
        /// The property is set to null.
        /// </exception> 
        public double ToDouble() {
            return (double)this.Value;
        }


        /// <summary>
        /// Converts this <see cref="NullableDecimal"/> instance to a 
        /// <see cref="NullableBoolean"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.Null"/> if this <see cref="NullableDecimal"/> is 
        /// <see cref="NullableDecimal.Null"/>, <see cref="NullableBoolean.False"/> if
        /// this instance <see cref="NullableDecimal.Value"/> is 
        /// <see cref="NullableDecimal.Zero"/> otherwise 
        /// <see cref="NullableBoolean.True"/>.
        /// </returns>
        public NullableBoolean ToNullableBoolean() {
            return ((NullableBoolean)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDecimal"/> instance to a 
        /// <see cref="NullableByte"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableByte"/> that is 
        /// <see cref="NullableByte.Null"/> if this <see cref="NullableDecimal"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise a <see cref="NullableByte"/> 
        /// whose <see cref="NullableByte.Value"/> equals the 
        /// <see cref="NullableDecimal.Value"/> of this instance.
        /// </returns>    
        /// <exception cref="System.OverflowException">
        /// This <see cref="NullableDecimal"/> value is a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than 
        /// <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public NullableByte ToNullableByte() {
            return ((NullableByte)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDecimal"/> instance to a 
        /// <see cref="NullableDouble"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableDouble"/> that is 
        /// <see cref="NullableDouble.Null"/> if this <see cref="NullableDecimal"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise a <see cref="NullableDouble"/> 
        /// whose <see cref="NullableDouble.Value"/> equals the 
        /// <see cref="NullableDecimal.Value"/> of this instance.
        /// </returns>        
        public NullableDouble ToNullableDouble() {
            return ((NullableDouble)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDecimal"/> instance to a 
        /// <see cref="NullableInt16"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt16"/> that is 
        /// <see cref="NullableInt16.Null"/> if this <see cref="NullableDecimal"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise a <see cref="NullableInt16"/> 
        /// whose <see cref="NullableInt16.Value"/> equals the 
        /// <see cref="NullableDecimal.Value"/> of this instance.
        /// </returns>        
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is a number that is less than 
        /// <see cref="System.Int16.MinValue"/> or greater than 
        /// <see cref="System.Int16.MaxValue"/>.
        /// </exception>
        public NullableInt16 ToNullableInt16() {
            return ((NullableInt16)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDecimal"/> instance to a 
        /// <see cref="NullableInt32"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt32"/> that is 
        /// <see cref="NullableInt32.Null"/> if this <see cref="NullableDecimal"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise a <see cref="NullableInt32"/> 
        /// whose <see cref="NullableInt32.Value"/> equals the 
        /// <see cref="NullableDecimal.Value"/> of this instance.
        /// </returns>        
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is a number that is less than 
        /// <see cref="System.Int32.MinValue"/> or greater than 
        /// <see cref="System.Int32.MaxValue"/>.
        /// </exception>
        public NullableInt32 ToNullableInt32() {
            return ((NullableInt32)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDecimal"/> instance to a 
        /// <see cref="NullableInt64"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt64"/> that is 
        /// <see cref="NullableInt64.Null"/> if this <see cref="NullableDecimal"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise a <see cref="NullableInt64"/> 
        /// whose <see cref="NullableInt64.Value"/> equals the 
        /// <see cref="NullableDecimal.Value"/> of this instance.
        /// </returns>        
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is a number that is less than 
        /// <see cref="System.Int64.MinValue"/> or greater than 
        /// <see cref="System.Int64.MaxValue"/>.
        /// </exception>
        public NullableInt64 ToNullableInt64() {
            return ((NullableInt64)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDecimal"/> instance to a 
        /// <see cref="NullableSingle"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableSingle"/> that is 
        /// <see cref="NullableSingle.Null"/> if this <see cref="NullableDecimal"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise a <see cref="NullableSingle"/> 
        /// whose <see cref="NullableSingle.Value"/> equals the 
        /// <see cref="NullableDecimal.Value"/> of this instance.
        /// </returns>    
        public NullableSingle ToNullableSingle() {
            return ((NullableSingle)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDecimal"/> structure to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <returns>
        /// <see cref="NullableString.Null"/> if this <see cref="NullableDecimal"/>
        /// is <see cref="NullableDecimal.Null"/> otherwise
        /// a new <see cref="NullableString"/> structure whose 
        /// <see cref="NullableString.Value"/> is the description of this 
        /// <see cref="NullableDecimal"/> structure's <see cref="Value"/>.
        /// </returns>
        public NullableString ToNullableString() {
            return ((NullableString)this);
        }


        /// <summary>
        /// Converts this <see cref="NullableDecimal"/> structure to a 
        /// <see cref="System.String"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="System.String"/> that is a description of Null if this 
        /// <see cref="NullableDecimal"/> is <see cref="NullableDecimal.Null"/> 
        /// otherwise a new <see cref="System.String"/> that is the description of 
        /// this <see cref="NullableDecimal"/> structure's <see cref="Value"/>.
        /// </returns>
        public override string ToString() {
            if (this.IsNull)
                return Locale.GetText("Null");

            return _value.ToString(sysGlb.CultureInfo.CurrentCulture);
        }
        #endregion // Conversions
    }
}