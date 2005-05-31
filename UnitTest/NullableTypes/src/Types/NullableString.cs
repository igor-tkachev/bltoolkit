//
// NullableTypes.NullableString
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          NiceGuyUK    (niceguyuk@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date        Author    Changes     Reasons
// 14-Apr-2003 Luca      Create      Declared public members
// 30-Apr-2003 NiceGuyUK Create      Started implementation (fields, constructors, INullable, IComparable)
// 17-Jun-2003 Luca      Create      Added XML documentation
// 24-Jun-2003 Luca      Create      Implementation completed 
// 27-Jun-2003 Luca      Refactoring Unified equivalent error messages 
// 30-Jun-2003 Luca      Upgrade     New requirement by Roni Burd: GetTypeCode
// 30-Jun-2003 Luca      Upgrade     Removed NullableMoney and NullableGuid
// 07-Jul-2003 Luca      Upgrade     Applied FxCop guideline: explicited CurrentCulture in Compare,
//                                   explicit conv. operators and CompareTo code
// 13-Sep-2003 Luca      Upgrade     New requirement: the type must be sarializable
// 05-Oct-2003 DamienG   Upgrade     New requirement: the type must be XmlSerializable
// 06-Oct-2003 Luca      Upgrade     Code upgrade: Replaced tabs with spaces and removed commented out code
// 06-Dic-2003 Luca      Bug Fix     Replaced Xml Schema id "NullableStringXmlSchema" with "NullableString"
//                                   because VSDesigner use it as type-name in the auto-generated WS client Proxy
// 06-Dic-2003  Luca     Bug Fix     Replaced Target Namespace for Xml Schema to avoid duplicate namespace with
//                                   other types in NullableTypes
// 18-Feb-2004  Luca    Bug Fix      ReadXml must read nil value also with a non empty element
//

namespace NullableTypes {
    using sys = System;
    using sysGlb = System.Globalization;
    using sysXml = System.Xml;
    using sysXmlSrl = System.Xml.Serialization;
    using sysXmlScm = System.Xml.Schema;

    // Note for the implementer
    // --------------------------------------------------------------------------
    // These string methods make culture aware comparison:
    // - CompareTo (use current thread CultureInfo)
    // - StartWith (idem)
    // - EndWith   (idem)
    // - Compare   (accept CultureInfo as parameter; programmer should
    //              use a InvariantCulture to compare string that are not 
    //              for the user as path names, XML tags, Reflection strings, 
    //              etc.)
    // These string methods make culture agnostic comparison:
    // - Operator ==
    // - Operator !=
    // - Equals
    // - CompareOrdinal
    //
    // Operator == calls Equals that calls CompareOrdinal
    // Operator ==   ->    Equals    ->    CompareOrdinal
    // 
    // Performance comparison benchmark:
    // (source http://www.ugidotnet.org/images/articles/stringhe/perf.gif)
    // -   141 msec: ReferenceEquals  (strings not interned may result different)
    // -   703 msec: CompareOrdinal 
    // -   730 msec: Equals
    // - 15000 msec: Compare
    // --------------------------------------------------------------------------




    /// <summary>
    /// Represents a String that is either a variable-length stream of characters
    /// or <see cref="Null"/>.
    /// </summary>
    /// <remarks>
    /// A NullableString type can sound very strange because System.String is a Reference-Type 
    /// and it can already be null (Nothing in Visual Basic).
    /// Anyway a NullableString parameter (or field or property) explicity states that a Null 
    /// value is welcome: you wont get an ArgumentNullException passing Null to a NullableString 
    /// method parameter (or field or property).
    /// Moreover NullableString permits to treat Null values for a String like for any other
    /// type of NullableTypes.
    /// </remarks>
    [sys.Serializable]
    public struct NullableString : INullable, sys.IComparable, sysXmlSrl.IXmlSerializable {

        #region Fields
        private string _value;

        /// <summary>
        /// Represents a Null value that can be assigned to an instance of the 
        /// <see cref="NullableString"/> structure.
        /// <para>
        /// A Null is the absence of a value because missing, unknown, or inapplicable 
        /// value. A Null should not be used to imply any other value (such as zero).
        /// Also any value (such as zero) should not be used to imply the absence of a 
        /// value, that's why Null exists.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Null field is a constant of the <see cref="NullableString"/> structure.
        /// </remarks>
        public static readonly NullableString Null;

        /// <summary>
        /// Represents the empty string. This field is read-only.
        /// </summary>
        public static readonly NullableString Empty = new NullableString(string.Empty);
        #endregion // Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="NullableString"/> to the value 
        /// indicated by an array of Unicode characters.
        /// </summary>
        /// <param name="value">An array of Unicode characters.</param>
        /// <remarks>
        /// If <paramref name="value"/> is a null reference (Nothing in Visual Basic) or 
        /// contains no element, an <see cref="Empty"/> instance is initialized.
        /// </remarks>
        public NullableString(char[] value) {
            _value = new string(value);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableString"/> class to 
        /// the value indicated by a specified Unicode character repeated a specified 
        /// number of times.
        /// </summary>
        /// <param name="c">A Unicode character.</param>
        /// <param name="count">The number of times c occurs.</param>
        public NullableString(char c, int count) {
            //built string from char[] of count occurences of c
            _value = new string(c, count);
        }


        /// <summary>
        /// Initializes a new instance of the String class to the value indicated by 
        /// an array of Unicode characters, a starting character position within that 
        /// array, and a length.
        /// </summary>
        /// <param name="value">An array of Unicode characters.</param>
        /// <param name="startIndex">
        /// The starting position within <paramref name="value"/>.
        /// </param>
        /// <param name="length">
        /// The number of characters within <paramref name="value"/> to use.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="startIndex"/> or <paramref name="length"/> is less than 
        /// zero. 
        /// <para>- or - </para>
        /// The sum of paramref name="startIndex"/ and <paramref name="length"/> is 
        /// greater than the number of elements in <paramref name="value"/>.
        /// </exception>
        public NullableString(char[] value, int startIndex, int length) {
            if (value == null)
                throw new System.ArgumentNullException("value");

            if (startIndex < 0 || (value.Length > 0 && value.Length-1 < startIndex))
                throw new sys.ArgumentOutOfRangeException("startIndex");

            if (length < 0 || value.Length  < startIndex + length)
                throw new sys.ArgumentOutOfRangeException("length");

            _value = new string(value, startIndex, length);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NullableString"/> structure 
        /// using the specified string.
        /// </summary>
        /// <param name="value">The string to store.</param>
        /// <remarks>
        /// If <paramref name="value"/> is null (Nothing in Visual Basic) the new 
        /// instance will be <see cref="NullableString.Null"/>.
        /// </remarks>
        public NullableString(string value) {
            _value = value;
        }

        #endregion // Constructors

        #region INullable
        /// <summary>
        /// Indicates whether or not the value of the <see cref="NullableString"/> 
        /// structure is null.
        /// </summary>
        /// <value>
        /// true if the <see cref="NullableString"/> structure is null, otherwise 
        /// false.
        /// </value>
        public bool IsNull {
            get { 
                return (_value == null); 
            }
        }
        #endregion // INullable

        #region IComparable - Ordering
        /// <summary>
        /// Compares this <see cref="NullableString"/> structure to a specified 
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
        /// Any instance of <see cref="NullableString"/> including the 
        /// <see cref="Empty"/> string, regardless of its <paramref name="value"/>, 
        /// is considered greater than a null reference (Nothing in Visual Basic) 
        /// and <see cref="Null"/>; and two <see cref="Null"/> compare equal to each 
        /// other.
        /// </remarks>
        /// <exception cref="System.ArgumentException">
        /// The <paramref name="value"/> is neither null or of type 
        /// <see cref="NullableInt32"/>.
        /// </exception>
        public int CompareTo(object value) {
            if (value == null)
                return 1;
            
            if (!(value is NullableString))
                throw new sys.ArgumentException(string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText("Value is not a {0}."), "NullableTypes.NullableString"));
            
            NullableString strValue = (NullableString)value;            
            return this.CompareTo(strValue);
        }


        /// <summary>
        /// Compares this <see cref="NullableString"/> structure to a specified 
        /// object and returns an indication of their relative values.
        /// </summary>
        /// <param name="value">
        /// A NullableString to compare. 
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
        ///             value is <see cref="Null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <remarks>
        /// Any instance of <see cref="NullableString"/> including the 
        /// <see cref="Empty"/> string, regardless of its <paramref name="value"/>, 
        /// is considered greater than <see cref="Null"/>; and two <see cref="Null"/> 
        /// compare equal to each other.
        /// </remarks>
        /// <exception cref="System.ArgumentException">
        /// The <paramref name="value"/> is not of type <see cref="NullableString"/>.
        /// </exception>
        public int CompareTo(NullableString value) {
                       
            if (value.IsNull && this.IsNull)
                return 0;

            if (value.IsNull)
                return 1;

            if (this.IsNull)
                return -1;
            
            return _value.CompareTo(value._value);
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
            //            targetNamespace="http://NullableTypes.SourceForge.Net/NullableStringXMLSchema"
            //            xmlns="http://NullableTypes.SourceForge.Net/NullableStringXMLSchema"
            //            elementFormDefault="qualified">
            //        <xs:element name="NullableSingle" type="xs:string" nillable="true" />
            //    </xs:schema>

            // Element: NullableSingle
            sysXmlScm.XmlSchemaElement rootElement = new sysXmlScm.XmlSchemaElement();
            rootElement.Name = "NullableString";
            rootElement.SchemaTypeName = 
                new sysXml.XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
            rootElement.IsNillable = true;

            // Xml Schema
            sysXmlScm.XmlSchema xsd = new sysXmlScm.XmlSchema();
            xsd.Id = "NullableString";
            xsd.Namespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
            xsd.TargetNamespace = "http://NullableTypes.SourceForge.Net/NullableStringXMLSchema";
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
                writer.WriteString(_value);
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

            if (nilValue == null || nilValue == "false" || nilValue == "0") 
                _value = elementValue;
            else
                _value = null;
        }
        #endregion // IXmlSerializable

        #region Properties
        /// <summary>
        /// Gets the value of the <see cref="NullableString"/> structure. This 
        /// property is read-only
        /// </summary>
        /// <value>The value of the <see cref="NullableString"/> structure.</value>
        /// <exception cref="NullableNullValueException">
        /// The property is set to null.
        /// </exception> 
        public string Value { 
            get { 
                if (IsNull) 
                    throw new NullableNullValueException();

                return _value; 
            }
        }


        /// <summary>
        /// Indicates whether or not the value of the <see cref="NullableString"/> 
        /// structure is <see cref="Empty"/>.
        /// </summary>
        /// <value>
        /// true if the <see cref="NullableString"/> structure is 
        /// <see cref="Empty"/>, otherwise false.
        /// </value>
        /// <exception cref="NullableNullValueException">
        /// This instance is set to <see cref="Null"/>.
        /// </exception> 
        public bool IsEmpty {
            get {
                if (IsNull) 
                    throw new NullableNullValueException();

                return (_value.Length == 0);
            }
        }


        /// <summary>
        /// Gets the character at a specified character position in this instance.
        /// In C#, this property is the indexer for the <see cref="NullableString"/> 
        /// class.
        /// </summary>
        /// <param name="index">
        /// A character position in this instance.
        /// </param>
        /// <value>
        /// A Unicode character.
        /// </value>
        /// <exception cref="System.IndexOutOfRangeException">
        /// <partamref nale="index"/> is greater than or equal to the length of this object or less 
        /// than zero.
        /// </exception>
        /// <exception cref="NullableNullValueException">
        /// This instance is set to <see cref="Null"/>.
        /// </exception> 
        [System.Runtime.CompilerServices.IndexerName("Chars")]
        public char this[int index] {
            get {
                if (_value == null) 
                    throw new NullableNullValueException();

                return _value[index];
            }
        }


        /// <summary>
        /// Gets the number of characters in this instance.
        /// </summary>
        /// <value>The number of characters in this instance.</value>
        /// <exception cref="NullableNullValueException">
        /// This instance is set to <see cref="Null"/>.
        /// </exception>         
        public int Length {
            get {
                if (_value == null) 
                    throw new NullableNullValueException();

                return _value.Length;
            }
        }
        #endregion // Properties        
    
        #region Equivalence
        /// <summary>
        /// Compares two <see cref="NullableString"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="value">The object to be compared. </param>
        /// <returns>
        /// true if object is an instance of <see cref="NullableString"/> and the 
        /// two instances are equivalent; otherwise false.
        /// </returns>
        public override bool Equals (object value) {
            if (!(value is NullableString))
                return false;

            NullableString sValue = (NullableString)value;
            return (this._value == sValue._value);
        }


        /// <summary>
        /// Compares two <see cref="NullableString"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="value">The value to be compared.</param>
        /// <returns>
        /// true if two values are equivalent; otherwise false.
        /// </returns>
        public bool Equals (NullableString value) {
            return (this._value == value._value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableString"/> for equivalence.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.False"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableString"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>        
        public static NullableBoolean operator ==(NullableString a, NullableString b) {
            if (a.IsNull || b.IsNull) 
                return NullableBoolean.Null;
            
            return new NullableBoolean(a._value == b._value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableString"/> for equivalence.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.False"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.True"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableString"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean operator !=(NullableString a, NullableString b) {
            if (a.IsNull || b.IsNull) 
                return NullableBoolean.Null;

            return new NullableBoolean(a._value != b._value);
        }


        /// <summary>
        /// Compares two <see cref="NullableString"/> structures to determine if 
        /// they are equivalent.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.False"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableString"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean Equals(NullableString a, NullableString b) {
            if (a.IsNull || b.IsNull) 
                return NullableBoolean.Null;
            
            return new NullableBoolean(a._value == b._value);
        }


        /// <summary>
        /// Compares two <see cref="NullableString"/> structures to determine if 
        /// they are not equivalent.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.False"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.True"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableString"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean NotEquals(NullableString a, NullableString b) {
            if (a.IsNull || b.IsNull) 
                return NullableBoolean.Null;
            
            return new NullableBoolean(a._value != b._value);        
        }


        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() {
            string tmp = (_value == null) ? string.Empty : _value;

            return tmp.GetHashCode();
        }
        #endregion // Equivalence

        #region Methods
        /// <summary>
        /// Returns the <see cref="TypeCode"/> for value type 
        /// <see cref="NullableString"/>.
        /// </summary>
        /// <returns>
        /// The enumerated constant, <see cref="TypeCode.NullableString"/>.
        /// </returns>
        public TypeCode GetTypeCode() {
            return TypeCode.NullableString;
        }


        /// <summary>
        /// Creates a copy of this <see cref="NullableString"/> structures.
        /// </summary>
        /// <returns>
        /// A new <see cref="NullableString"/> object in which all property values 
        /// are the same as the original.
        /// </returns>
        public NullableString Clone() {
            return this;
        }


        /// <summary>
        /// Compares two specified <see cref="NullableString"/> structures.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <returns>
        /// A 32-bit signed integer indicating the lexical relationship between the 
        /// two comparands.
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Value</term>
        ///             <description>Description</description>
        ///         </listheader>
        ///         <item>
        ///             <term>A negative integer</term>
        ///             <description>
        ///             <paramref name="a"/> is less than <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>
        ///             <paramref name="a"/> is equal to <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>A positive integer</term>
        ///             <description>
        ///             <paramref name="a"/> is greater than <paramref name="b"/>.
        ///             <para>-or-</para>
        ///             <paramref name="b"/> is <see cref="Null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <remarks>
        /// By definition, any <see cref="NullableString"/>, including the 
        /// <see cref="Empty"/> string, compares greater than a <see cref="Null"/>; 
        /// and two <see cref="Null"/> compare equal to each other.
        /// This method performs a case-sensitive operation. The method uses the 
        /// current culture to determine the ordering of individual characters. 
        /// Uppercase letters evaluate greater than their lowercase equivalents. 
        /// The two strings are compared on a character-by-character basis.
        /// </remarks>
        public static int Compare(NullableString a, NullableString b) {
            return string.Compare(a._value, b._value, false, sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Compares two specified <see cref="NullableString"/> structures, ignoring 
        /// or honoring their case.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <param name="ignoreCase">
        /// A <see cref="System.Boolean"/> indicating a case-sensitive or 
        /// insensitive comparison. (true indicates a case-insensitive comparison).
        /// </param>
        /// <returns>
        /// A 32-bit signed integer indicating the lexical relationship between the 
        /// two comparands.
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Value</term>
        ///             <description>Description</description>
        ///         </listheader>
        ///         <item>
        ///             <term>A negative integer</term>
        ///             <description>
        ///             <paramref name="a"/> is less than <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>
        ///             <paramref name="a"/> is equal to <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>A positive integer</term>
        ///             <description>
        ///             <paramref name="a"/> is greater than <paramref name="b"/>.
        ///             <para>-or-</para>
        ///             <paramref name="b"/> is <see cref="Null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <remarks>
        /// By definition, any <see cref="NullableString"/>, including the 
        /// <see cref="Empty"/> string, compares greater than a <see cref="Null"/>; 
        /// and two <see cref="Null"/> compare equal to each other.
        /// This method performs a case-sensitive operation. The method uses the 
        /// current culture to determine the ordering of individual characters.  
        /// The two strings are compared on a character-by-character basis.
        /// </remarks>
        public static int Compare(NullableString a, NullableString b, bool ignoreCase) {
            return string.Compare(a._value, b._value, ignoreCase, sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Compares two specified <see cref="NullableString"/> structures, ignoring 
        /// or honoring their case and honoring culture-specific information about 
        /// their formatting.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <param name="ignoreCase">
        /// A <see cref="System.Boolean"/> indicating a case-sensitive or 
        /// insensitive comparison. (true indicates a case-insensitive comparison).
        /// </param>
        /// <param name="culture">
        /// A <see cref="System.Globalization.CultureInfo"/> object that supplies 
        /// culture-specific formatting information. 
        /// </param>
        /// <returns>
        /// A 32-bit signed integer indicating the lexical relationship between the 
        /// two comparands.
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Value</term>
        ///             <description>Description</description>
        ///         </listheader>
        ///         <item>
        ///             <term>A negative integer</term>
        ///             <description>
        ///             <paramref name="a"/> is less than <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>
        ///             <paramref name="a"/> is equal to <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>A positive integer</term>
        ///             <description>
        ///             <paramref name="a"/> is greater than <paramref name="b"/>.
        ///             <para>-or-</para>
        ///             <paramref name="b"/> is <see cref="Null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="culture"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <remarks>
        /// culture specifies a <see cref="System.Globalization.CultureInfo"/>
        /// object, which provides culture-specific information that can affect the 
        /// comparison.
        /// By definition, any <see cref="NullableString"/>, including the 
        /// <see cref="Empty"/> string, compares greater than a <see cref="Null"/>; 
        /// and two <see cref="Null"/> compare equal to each other.
        /// </remarks>
        public static int Compare(NullableString a, NullableString b, bool ignoreCase, sysGlb.CultureInfo culture) {
            return string.Compare(a._value, b._value, ignoreCase, culture);
        }


        /// <summary>
        /// Compares substrings of the two specified <see cref="NullableString"/> 
        /// structures.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="indexA">
        /// The position of the substring within <paramref name="a"/>.
        /// </param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <param name="indexB">
        /// The position of the substring within <paramref name="b"/>.
        /// </param>
        /// <param name="length">
        /// The maximum number of characters in the substrings to compare.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer indicating the lexical relationship between the 
        /// two comparands.
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Value</term>
        ///             <description>Description</description>
        ///         </listheader>
        ///         <item>
        ///             <term>A negative integer</term>
        ///             <description>
        ///             the substring in <paramref name="a"/> is less than the 
        ///             substring in <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>
        ///             the substring in <paramref name="a"/> is equal to 
        ///             the substring in <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>A positive integer</term>
        ///             <description>
        ///             the substring in <paramref name="a"/> is greater than the 
        ///             substring in <paramref name="b"/>.
        ///             <para>-or-</para>
        ///             <paramref name="b"/> is <see cref="Null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The sum of <paramref name="indexA"/> and <paramref name="length"/> is 
        /// greater than <paramref name="a"/>. 
        /// <para>-or-</para>
        /// The sum of <paramref name="indexB"/> and <paramref name="length"/> is 
        /// greater than <paramref name="b"/>.
        /// <para>-or-</para>
        /// <paramref name="indexA"/>, <paramref name="indexB"/> or 
        /// <paramref name="length"/> is negative.
        /// </exception>
        /// <remarks>
        /// <paramref name="indexA"/> and <paramref name="indexB"/> are zero-based.
        /// <paramref name="length"/> cannot be negative. If 
        /// <paramref name="length"/> is zero, then zero is returned.
        /// The number of characters compared is the lesser of the 
        /// <paramref name="length"/> of <paramref name="a"/> less 
        /// <paramref name="indexA"/>, the <paramref name="length"/> of 
        /// <paramref name="b"/> less <paramref name="indexB"/>, and 
        /// <paramref name="length"/>.
        /// By definition, any <see cref="NullableString"/>, including the 
        /// <see cref="Empty"/> string, compares greater than <see cref="Null"/>; 
        /// and two <see cref="Null"/> compare equal to each other.
        /// This method performs a case-sensitive operation. The method uses the 
        /// current culture to determine the ordering of individual characters. 
        /// Uppercase letters evaluate greater than their lowercase equivalents. 
        /// The two <see cref="NullableString"/> are compared on a 
        /// character-by-character basis.
        /// </remarks>
        public static int Compare(NullableString a, int indexA, NullableString b, int indexB, int length) {
            return string.Compare(a._value, indexA, b._value, indexB, length, false, sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Compares substrings of the two specified <see cref="NullableString"/> 
        /// structures.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="indexA">
        /// The position of the substring within <paramref name="a"/>.
        /// </param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <param name="indexB">
        /// The position of the substring within <paramref name="b"/>.
        /// </param>
        /// <param name="length">
        /// The maximum number of characters in the substrings to compare.
        /// </param>
        /// <param name="ignoreCase">
        /// A <see cref="System.Boolean"/> indicating a case-sensitive or 
        /// insensitive comparison. (true indicates a case-insensitive comparison).
        /// </param>
        /// <returns>
        /// A 32-bit signed integer indicating the lexical relationship between the 
        /// two comparands.
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Value</term>
        ///             <description>Description</description>
        ///         </listheader>
        ///         <item>
        ///             <term>A negative integer</term>
        ///             <description>
        ///             the substring in <paramref name="a"/> is less than the 
        ///             substring in <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>
        ///             the substring in <paramref name="a"/> is equal to 
        ///             the substring in <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>A positive integer</term>
        ///             <description>
        ///             the substring in <paramref name="a"/> is greater than the 
        ///             substring in <paramref name="b"/>.
        ///             <para>-or-</para>
        ///             <paramref name="b"/> is <see cref="Null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The sum of <paramref name="indexA"/> and <paramref name="length"/> is 
        /// greater than <paramref name="a"/>. 
        /// <para>-or-</para>
        /// The sum of <paramref name="indexB"/> and <paramref name="length"/> is 
        /// greater than <paramref name="b"/>.
        /// <para>-or-</para>
        /// <paramref name="indexA"/>, <paramref name="indexB"/> or 
        /// <paramref name="length"/> is negative.
        /// </exception>
        /// <remarks>
        /// <paramref name="indexA"/> and <paramref name="indexB"/> are zero-based.
        /// <paramref name="length"/> cannot be negative. If 
        /// <paramref name="length"/> is zero, then zero is returned.
        /// The number of characters compared is the lesser of the 
        /// <paramref name="length"/> of <paramref name="a"/> less 
        /// <paramref name="indexA"/>, the <paramref name="length"/> of 
        /// <paramref name="b"/> less <paramref name="indexB"/>, and 
        /// <paramref name="length"/>.
        /// By definition, any <see cref="NullableString"/>, including the 
        /// <see cref="Empty"/> string, compares greater than <see cref="Null"/>; 
        /// and two <see cref="Null"/> compare equal to each other.
        /// This method performs a case-sensitive operation. The method uses the 
        /// current culture to determine the ordering of individual characters. 
        /// Uppercase letters evaluate greater than their lowercase equivalents. 
        /// The two <see cref="NullableString"/> are compared on a 
        /// character-by-character basis.
        /// </remarks>
        public static int Compare(NullableString a, int indexA, NullableString b, int indexB, int length, bool ignoreCase) {
            return string.Compare(a._value, indexA, b._value, indexB, length, ignoreCase, sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Compares substrings of the two specified <see cref="NullableString"/> 
        /// structures.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="indexA">
        /// The position of the substring within <paramref name="a"/>.
        /// </param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <param name="indexB">
        /// The position of the substring within <paramref name="b"/>.
        /// </param>
        /// <param name="length">
        /// The maximum number of characters in the substrings to compare.
        /// </param>
        /// <param name="ignoreCase">
        /// A <see cref="System.Boolean"/> indicating a case-sensitive or 
        /// insensitive comparison. (true indicates a case-insensitive comparison).
        /// </param>
        /// <param name="culture">
        /// A <see cref="System.Globalization.CultureInfo"/> object that supplies 
        /// culture-specific formatting information. 
        /// </param>
        /// <returns>
        /// A 32-bit signed integer indicating the lexical relationship between the 
        /// two comparands.
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Value</term>
        ///             <description>Description</description>
        ///         </listheader>
        ///         <item>
        ///             <term>A negative integer</term>
        ///             <description>
        ///             the substring in <paramref name="a"/> is less than the 
        ///             substring in <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>
        ///             the substring in <paramref name="a"/> is equal to 
        ///             the substring in <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>A positive integer</term>
        ///             <description>
        ///             the substring in <paramref name="a"/> is greater than the 
        ///             substring in <paramref name="b"/>.
        ///             <para>-or-</para>
        ///             <paramref name="b"/> is <see cref="Null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="culture"/> is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The sum of <paramref name="indexA"/> and <paramref name="length"/> is 
        /// greater than <paramref name="a"/>. 
        /// <para>-or-</para>
        /// The sum of <paramref name="indexB"/> and <paramref name="length"/> is 
        /// greater than <paramref name="b"/>.
        /// <para>-or-</para>
        /// <paramref name="indexA"/>, <paramref name="indexB"/> or 
        /// <paramref name="length"/> is negative.
        /// </exception>
        /// <remarks>
        /// <paramref name="culture"/> specifies a <see cref="System.Globalization.CultureInfo"/>
        /// object, which provides culture-specific information that can affect the 
        /// comparison.        
        /// <paramref name="indexA"/> and <paramref name="indexB"/> are zero-based.
        /// <paramref name="length"/> cannot be negative. If 
        /// <paramref name="length"/> is zero, then zero is returned.
        /// The number of characters compared is the lesser of the 
        /// <paramref name="length"/> of <paramref name="a"/> less 
        /// <paramref name="indexA"/>, the <paramref name="length"/> of 
        /// <paramref name="b"/> less <paramref name="indexB"/>, and 
        /// <paramref name="length"/>.
        /// By definition, any <see cref="NullableString"/>, including the 
        /// <see cref="Empty"/> string, compares greater than <see cref="Null"/>; 
        /// and two <see cref="Null"/> compare equal to each other. 
        /// The two <see cref="NullableString"/> are compared on a 
        /// character-by-character basis.
        /// </remarks>
        public static int Compare(NullableString a, int indexA, NullableString b, int indexB, int length, bool ignoreCase, sysGlb.CultureInfo culture) {
            return string.Compare(a._value, indexA, b._value, indexB, length, ignoreCase, culture);
        }


        /// <summary>
        /// Compares two specified <see cref="NullableString"/> structures without 
        /// considering the local national language or culture.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <returns>
        /// A 32-bit signed integer indicating the lexical relationship between the 
        /// two comparands.
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Value</term>
        ///             <description>Description</description>
        ///         </listheader>
        ///         <item>
        ///             <term>A negative integer</term>
        ///             <description>
        ///             <paramref name="a"/> is less than <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>
        ///             <paramref name="a"/> is equal to <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>A positive integer</term>
        ///             <description>
        ///             <paramref name="a"/> is greater than <paramref name="b"/>.
        ///             <para>-or-</para>
        ///             <paramref name="b"/> is <see cref="Null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <remarks>
        /// By definition, any <see cref="NullableString"/>, including the 
        /// <see cref="Empty"/> string, compares greater than a <see cref="Null"/>; 
        /// and two <see cref="Null"/> compare equal to each other.
        /// </remarks>
        public static int CompareOrdinal(NullableString a, NullableString b) {
            return string.CompareOrdinal(a._value, b._value);
        }


        /// <summary>
        /// Compares substrings of the two specified <see cref="NullableString"/> 
        /// structures ,without considering the local national language or culture.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="indexA">
        /// The position of the substring within <paramref name="a"/>.
        /// </param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <param name="indexB">
        /// The position of the substring within <paramref name="b"/>.
        /// </param>
        /// <param name="length">
        /// The maximum number of characters in the substrings to compare.
        /// </param>
        /// <returns>
        /// A 32-bit signed integer indicating the lexical relationship between the 
        /// two comparands.
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Value</term>
        ///             <description>Description</description>
        ///         </listheader>
        ///         <item>
        ///             <term>A negative integer</term>
        ///             <description>
        ///             the substring in <paramref name="a"/> is less than the 
        ///             substring in <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>
        ///             the substring in <paramref name="a"/> is equal to 
        ///             the substring in <paramref name="b"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>A positive integer</term>
        ///             <description>
        ///             the substring in <paramref name="a"/> is greater than the 
        ///             substring in <paramref name="b"/>.
        ///             <para>-or-</para>
        ///             <paramref name="b"/> is <see cref="Null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The sum of <paramref name="indexA"/> and <paramref name="length"/> is 
        /// greater than <paramref name="a"/>. 
        /// <para>-or-</para>
        /// The sum of <paramref name="indexB"/> and <paramref name="length"/> is 
        /// greater than <paramref name="b"/>.
        /// <para>-or-</para>
        /// <paramref name="indexA"/>, <paramref name="indexB"/> or 
        /// <paramref name="length"/> is negative.
        /// </exception>
        /// <remarks>
        /// By definition, any <see cref="NullableString"/>, including the 
        /// <see cref="Empty"/> string, compares greater than <see cref="Null"/>; 
        /// and two <see cref="Null"/> compare equal to each other.
        /// <paramref name="indexA"/> and <paramref name="indexB"/> are zero-based.
        /// <paramref name="length"/> cannot be negative. If 
        /// <paramref name="length"/> is zero, then zero is returned.
        /// The number of characters compared is the lesser of the 
        /// <paramref name="length"/> of <paramref name="a"/> less 
        /// <paramref name="indexA"/>, the <paramref name="length"/> of 
        /// <paramref name="b"/> less <paramref name="indexB"/>, and 
        /// <paramref name="length"/>.
        /// </remarks>
        public static int CompareOrdinal(NullableString a, int indexA, NullableString b, int indexB, int length) {
            return string.CompareOrdinal(a._value, indexA, b._value, indexB, length);
        }


        /// <summary>
        /// Concatenates two specified instances of <see cref="NullableString"/>.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <returns>The concatenation of str0 and str1.</returns>
        /// <remarks>
        /// An <see cref="Empty"/> string is used in place of any null argument.
        /// </remarks>
        public static NullableString Concat(NullableString a, NullableString b) {
            return a + b;
        }        
        #endregion // Methods 

        #region Operators
        /// <summary>
        /// Concatenates the two specified <see cref="NullableString"/> structures.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> structure.</param>
        /// <param name="b">A <see cref="NullableString"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableString"/> containing the newly concatenated value 
        /// representing the contents of the two <see cref="NullableString"/>
        /// parameters.
        /// </returns>
        public static NullableString operator + (NullableString a, NullableString b) {
            if (a._value == null || b._value == null) 
                return (NullableString.Null);

            return new NullableString(a._value + b._value);
        }
        #endregion // Operators

        #region Conversion Operators
        /// <summary>
        /// Converts the <see cref="NullableBoolean"/> parameter to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableBoolean"/> to be converted to a 
        /// <see cref="NullableString"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableBoolean.Null"/> otherwise a new 
        /// <see cref="NullableString"/> the string representation of 
        /// <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableString(NullableBoolean x) {
            if (x.IsNull)
                return NullableString.Null;

            return new NullableString(x.Value.ToString(sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Converts the <see cref="NullableByte"/> parameter to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableByte"/> to be converted to a 
        /// <see cref="NullableString"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableByte.Null"/> otherwise a new 
        /// <see cref="NullableString"/> that's the string representation of 
        /// <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableString(NullableByte x) {
            if (x.IsNull)
                return NullableString.Null;

            return new NullableString(x.Value.ToString(sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Converts the <see cref="NullableDateTime"/> parameter to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDateTime"/> to be converted to a 
        /// <see cref="NullableString"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableDateTime.Null"/> otherwise a new 
        /// <see cref="NullableString"/> that's the string representation of 
        /// <paramref name="x"/>.
        /// </returns>    
        public static explicit operator NullableString(NullableDateTime x) {
            if (x.IsNull)
                return NullableString.Null;

            return new NullableString(x.Value.ToString(sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Converts the <see cref="NullableDecimal"/> parameter to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDecimal"/> to be converted to a 
        /// <see cref="NullableString"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise a new 
        /// <see cref="NullableString"/> structure that's the string representation 
        /// of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableString(NullableDecimal x) {
            if (x.IsNull)
                return NullableString.Null;

            return new NullableString(x.Value.ToString(sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Converts the <see cref="NullableDouble"/> parameter to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDouble"/> to be converted to a 
        /// <see cref="NullableString"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise a new 
        /// <see cref="NullableString"/> structure that's the string representation 
        /// of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableString(NullableDouble x) {
            if (x.IsNull)
                return NullableString.Null;

            return new NullableString(x.Value.ToString(sysGlb.CultureInfo.CurrentCulture));
        }



        /// <summary>
        /// Converts the <see cref="NullableInt16"/> parameter to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt16"/> to be converted to a 
        /// <see cref="NullableString"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt16.Null"/> otherwise a new 
        /// <see cref="NullableString"/> structure that's the string representation 
        /// of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableString(NullableInt16 x) {
            if (x.IsNull)
                return NullableString.Null;

            return new NullableString(x.Value.ToString(sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Converts the <see cref="NullableInt32"/> parameter to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt32"/> to be converted to a 
        /// <see cref="NullableString"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt32.Null"/> otherwise a new 
        /// <see cref="NullableString"/> structure that's the string representation 
        /// of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableString(NullableInt32 x) {
            if (x.IsNull)
                return NullableString.Null;

            return new NullableString(x.Value.ToString(sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Converts the <see cref="NullableInt64"/> parameter to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt64"/> to be converted to a 
        /// <see cref="NullableString"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt64.Null"/> otherwise a new 
        /// <see cref="NullableString"/> structure that's the string representation 
        /// of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableString(NullableInt64 x) {
            if (x.IsNull)
                return NullableString.Null;

            return new NullableString(x.Value.ToString(sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Converts the <see cref="NullableSingle"/> parameter to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableSingle"/> to be converted to a 
        /// <see cref="NullableString"/> structure. 
        /// </param>
        /// <returns>
        /// <see cref="Null"/> if <paramref name="x"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise a new 
        /// <see cref="NullableString"/> structure that's the string representation 
        /// of <paramref name="x"/>.
        /// </returns>
        public static explicit operator NullableString(NullableSingle x) {
            if (x.IsNull)
                return NullableString.Null;

            return new NullableString(x.Value.ToString(sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// Converts a <see cref="NullableString"/> to a <see cref="System.String"/>.
        /// </summary>
        /// <param name="a">A <see cref="NullableString"/> to convert.</param>
        /// <returns>
        /// A <see cref="System.String"/> set to the <see cref="Value"/> of the 
        /// <see cref="NullableString"/>.
        /// </returns>
        /// <exception cref="NullableNullValueException">
        /// <paramref name="a"/> is <see cref="Null"/>.
        /// </exception>
        public static explicit operator string(NullableString a) {
            return a.Value;
        }


        /// <summary>
        /// Converts the <see cref="System.String"/> parameter to a 
        /// <see cref="NullableString"/> structure.
        /// </summary>
        /// <param name="a">
        /// A <see cref="System.String"/> to be converted to a 
        /// <see cref="NullableString"/> structure. 
        /// </param>
        /// <returns>
        /// A new <see cref="NullableString"/> structure constructed from 
        /// <paramref name="a"/>.
        /// </returns>
        public static implicit operator NullableString(string a) {
            return new NullableString(a);
        }
        #endregion // Conversion Operators

        #region Conversions
        /// <summary>
        /// Converts this <see cref="NullableString"/> instance to a 
        /// <see cref="NullableBoolean"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.Null"/> if this <see cref="NullableString"/> is 
        /// <see cref="NullableString.Null"/>, <see cref="NullableBoolean.True"/> if
        /// this instance <see cref="NullableString.Value"/> is 
        /// equal to <see cref="System.Boolean.TrueString"/> or "1" otherwise 
        /// <see cref="NullableBoolean.False"/>.
        /// </returns>
        public NullableBoolean ToNullableBoolean() {
            return (NullableBoolean)this;
        }

        /// <summary>
        /// Converts this <see cref="NullableString"/> instance to a 
        /// <see cref="NullableByte"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableByte"/> that is 
        /// <see cref="NullableByte.Null"/> if this <see cref="NullableString"/> is 
        /// <see cref="NullableString.Null"/> otherwise a <see cref="NullableByte"/> 
        /// whose <see cref="NullableByte.Value"/> equals the 
        /// the number represented by this <see cref="NullableString"/>.
        /// </returns>    
        /// <exception cref="System.OverflowException">
        /// This <see cref="NullableString"/> represent a number that is less than 
        /// <see cref="System.Byte.MinValue"/> or greater than 
        /// <see cref="System.Byte.MaxValue"/>.
        /// </exception>
        public NullableByte ToNullableByte() {
            return (NullableByte)this;
        }

        /// <summary>
        /// Converts this <see cref="NullableString"/> structure to 
        /// <see cref="NullableDateTime"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="NullableDateTime"/> structure containing the date value 
        /// represented by this <see cref="NullableString"/> structure.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// this <see cref="NullableString"/> structure does not contain a valid 
        /// string representation of a date and time.
        /// <seealso cref="System.DateTime.Parse"/>
        /// </exception>
        /// <remarks>
        /// This <see cref="NullableString"/> structure is parsed using the 
        /// formatting information in a DateTimeFormatInfo initialized for the 
        /// current culture.
        /// This method attempts to parse this <see cref="NullableString"/> 
        /// completely and avoid throwing FormatException. It ignores unrecognized 
        /// data if possible and fills in missing month, day, and year information 
        /// with the current time. If s contains only a date and no time, this method 
        /// assumes 12 A.M. Any leading, inner, or trailing white space character 
        /// is ignored.                                                                                                                                                                                                                                                                                                                                                  Parameter s must contain the representation of a date and time in one of the formats described in the DateTimeFormatInfo topic.
        /// </remarks>
        public NullableDateTime ToNullableDateTime() {
            return (NullableDateTime)this;
        }

        /// <summary>
        /// Converts this <see cref="NullableString"/> instance to a 
        /// <see cref="NullableDecimal"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableDecimal"/> that is 
        /// <see cref="NullableDecimal.Null"/> if this <see cref="NullableString"/> 
        /// is <see cref="NullableString.Null"/> otherwise a 
        /// <see cref="NullableDecimal"/> whose <see cref="NullableDecimal.Value"/> 
        /// equals the number represented by this <see cref="NullableString"/>.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// this <see cref="NullableString"/> instance is not of the correct format.
        /// <seealso cref="System.DateTime.Parse"/>
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// this <see cref="NullableString"/> instance represents a number less than 
        /// <see cref="System.DateTime.MinValue"/> or greater than 
        /// <see cref="System.DateTime.MaxValue"/>.
        /// </exception>
        public NullableDecimal ToNullableDecimal() {
            return (NullableDecimal)this;
        }

        /// <summary>
        /// Converts this <see cref="NullableString"/> instance to a 
        /// <see cref="NullableDouble"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableDouble"/> that is 
        /// <see cref="NullableDouble.Null"/> if this <see cref="NullableString"/> is 
        /// <see cref="NullableString.Null"/> otherwise a <see cref="NullableDouble"/> 
        /// whose <see cref="NullableDouble.Value"/> equals the number represented 
        /// by this <see cref="NullableString"/> instance.
        /// </returns>        
        /// <exception cref="System.FormatException">
        /// this <see cref="NullableString"/> instance is not of the correct format.
        /// <seealso cref="System.Double.Parse"/>
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// this <see cref="NullableString"/> instance represents a number less than 
        /// <see cref="System.Double.MinValue"/> or greater than 
        /// <see cref="System.Double.MaxValue"/>.
        /// </exception>
        public NullableDouble ToNullableDouble() {
            return (NullableDouble)this;
        }


        /// <summary>
        /// Converts this <see cref="NullableString"/> instance to a 
        /// <see cref="NullableInt16"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt16"/> that is 
        /// <see cref="NullableInt16.Null"/> if this <see cref="NullableString"/> is 
        /// <see cref="NullableString.Null"/> otherwise a <see cref="NullableInt16"/> 
        /// whose <see cref="NullableInt16.Value"/> equals the number represented by
        /// this <see cref="NullableString"/>.
        /// </returns>        
        /// <exception cref="System.FormatException">
        /// this <see cref="NullableString"/> instance is not of the correct format.
        /// <seealso cref="System.Int16.Parse"/>
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value represent a number that is less than 
        /// <see cref="System.Int16.MinValue"/> or greater than 
        /// <see cref="System.Int16.MaxValue"/>.
        /// </exception>
        public NullableInt16 ToNullableInt16() {
            return (NullableInt16)this;
        }
        
        /// <summary>
        /// Converts this <see cref="NullableString"/> instance to a 
        /// <see cref="NullableInt32"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt32"/> that is 
        /// <see cref="NullableInt32.Null"/> if this <see cref="NullableString"/> is 
        /// <see cref="NullableString.Null"/> otherwise a <see cref="NullableInt32"/> 
        /// whose <see cref="NullableInt32.Value"/> equals the number represented by
        /// this <see cref="NullableString"/>.
        /// </returns>        
        /// <exception cref="System.FormatException">
        /// this <see cref="NullableString"/> instance is not of the correct format.
        /// <seealso cref="System.Int32.Parse"/>
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value represent a number that is less than 
        /// <see cref="System.Int32.MinValue"/> or greater than 
        /// <see cref="System.Int32.MaxValue"/>.
        /// </exception>
        public NullableInt32 ToNullableInt32() {
            return (NullableInt32)this;
        }

        /// <summary>
        /// Converts this <see cref="NullableString"/> instance to a 
        /// <see cref="NullableInt64"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableInt64"/> that is 
        /// <see cref="NullableInt64.Null"/> if this <see cref="NullableString"/> is 
        /// <see cref="NullableString.Null"/> otherwise a <see cref="NullableInt64"/> 
        /// whose <see cref="NullableInt64.Value"/> equals the number represented by
        /// this <see cref="NullableString"/>.
        /// </returns>        
        /// <exception cref="System.FormatException">
        /// this <see cref="NullableString"/> instance is not of the correct format.
        /// <seealso cref="System.Int64.Parse"/>
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value represent a number that is less than 
        /// <see cref="System.Int64.MinValue"/> or greater than 
        /// <see cref="System.Int64.MaxValue"/>.
        /// </exception>        
        public NullableInt64 ToNullableInt64() {
            return (NullableInt64)this;
        }
        
        /// <summary>
        /// Converts this <see cref="NullableString"/> instance to a 
        /// <see cref="NullableSingle"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="NullableSingle"/> that is 
        /// <see cref="NullableSingle.Null"/> if this <see cref="NullableString"/> is 
        /// <see cref="NullableString.Null"/> otherwise a <see cref="NullableSingle"/> 
        /// whose <see cref="NullableSingle.Value"/> equals the number represented 
        /// by this <see cref="NullableString"/> instance.
        /// </returns>        
        /// <exception cref="System.FormatException">
        /// this <see cref="NullableString"/> instance is not of the correct format.
        /// <seealso cref="System.Single.Parse"/>
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// this <see cref="NullableString"/> instance represents a number less than 
        /// <see cref="System.Single.MinValue"/> or greater than 
        /// <see cref="System.Single.MaxValue"/>.
        /// </exception>
        public NullableSingle ToNullableSingle() {
            return (NullableSingle)this;
        }

        /// <summary>
        /// Converts this <see cref="NullableString"/> structure to a 
        /// <see cref="System.String"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="System.String"/> that is a description of Null if this 
        /// <see cref="NullableString"/> is <see cref="NullableString.Null"/> 
        /// otherwise a new <see cref="System.String"/> that is equal to
        /// this instance <see cref="Value"/>.
        /// </returns>        
        public override string ToString() {
            if (this.IsNull)
                return Locale.GetText("Null");
            
            return _value;        
        }
        

        #endregion Conversions
    }
}