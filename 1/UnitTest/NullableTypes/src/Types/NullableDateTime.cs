//
// NullableTypes.NullableDateTime
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Eric Lau (elikfunglau@users.soruceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author  Changes      Reasons
// 24-Mar-2003  Luca    Create       Declared public members
// 16-May-2003  Eric    Create       Implementation
// 20-May-2003  Eric    Create       Xml documentation done
// 27-Jun-2003  Luca    Refactoring  Unified equivalent error messages 
// 30-Jun-2003  Luca    Upgrade      New requirement by Roni Burd: GetTypeCode 
// 07-Jul-2003  Luca    Upgrade      Applied FxCop guideline: explicited CurrentCulture in CompareTo  
//                                   and ToString code
// 07-Jul-2003  Luca    Upgrade      Applied FxCop guideline: new Add method where + operator exists and
//                                   new Subtract method where - operator exists
// 19-Aug-2003  Luca    Bug Fix      Introduced a work-around for a bug of System.DateTime that throw 
//                                   ArgumentOutOfRange instead of OverflowException
// 20-Aug-2003  Luca    Bug Fix      ToString() on Null should return the "Null" string for current culture
// 13-Sep-2003  Luca    Upgrade      New requirement: the type must be sarializable
// 03-Oct-2003  DamienG Upgrade      New requirement: the type must be XmlSerializable
// 06-Oct-2003  Luca    Upgrade      Code upgrade: Replaced tabs with spaces and removed commented out code
// 06-Dic-2003  Luca    Bug Fix      Replaced Xml Schema id "NullableDateTimeXmlSchema" with "NullableDateTime"
//                                   because VSDesigner use it as type-name in the auto-generated WS client Proxy
// 06-Dic-2003  Luca    Bug Fix      Replaced Target Namespace for Xml Schema to avoid duplicate namespace with
//                                   other types in NullableTypes
// 18-Feb-2004  Luca    Bug Fix      ReadXml must read nil value also with a non empty element
//

/*
 * Note:    SqlDateTime has a domain for date and time values that's
   different from System.DateTime. NullableDateTime has the same 
   domain values as System.DateTime. - EL
 */

namespace NullableTypes {
    using sys = System;
    using sysGlb = System.Globalization;
    using sysXml = System.Xml;
    using sysXmlSrl = System.Xml.Serialization;
    using sysXmlScm = System.Xml.Schema;

    /// <summary>
    /// Represents an instant in time, but unlike System.DateTime, provides
    /// a Null value. Part of NullableTypes project.
    /// </summary>
    [sys.Serializable]
    public struct NullableDateTime : INullable, sys.IComparable, sysXmlSrl.IXmlSerializable {

        #region Fields
        private sys.DateTime _value;
        private bool _notNull;

        /// <summary>
        /// Represents the largest possible value of NullableTypes.NullableDateTime.
        /// This field is read-only.
        /// </summary>
        public static readonly NullableDateTime MaxValue = new NullableDateTime(sys.DateTime.MaxValue);
        /// <summary>
        /// Represents the smallest possible value of NullableTypes.NullableDateTime.
        /// This field is read-only.
        /// </summary>
        public static readonly NullableDateTime MinValue = new NullableDateTime(sys.DateTime.MinValue);
        /// <summary>
        /// Represents the null value of NullableTypes.NullableDateTime. 
        /// This field is read-only.
        /// <para>
        /// A Null is the absence of a value because missing, unknown, or inapplicable 
        /// value. A Null should not be used to imply any other value (such as zero).
        /// Also any value (such as zero) should not be used to imply the absence of a 
        /// value, that's why Null exists.
        /// </para>
        /// </summary>
        public static readonly NullableDateTime Null;
        /// <summary>
        /// Number of ticks per day. This field is read-only.
        /// </summary>
        /// <remarks>
        /// A tick in NullableDateTime is the same as a tick in 
        /// System.DateTime.
        /// </remarks>
        public const long TicksPerDay = 0xC92A69C000L;
        /// <summary>
        /// Number of ticks per hour. This field is read-only.
        /// </summary>
        /// <remarks>
        /// A tick in NullableDateTime is the same as a tick in 
        /// System.DateTime.
        /// </remarks>
        public const long TicksPerHour = 0x861C46800L;
        /// <summary>
        /// Number of ticks per minute. This field is read-only.
        /// </summary>
        /// <remarks>
        /// A tick in NullableDateTime is the same as a tick in 
        /// System.DateTime.
        /// </remarks>
        public const long TicksPerMinute = 0x23C34600L;
        /// <summary>
        /// Number of ticks per second. This field is read-only.
        /// </summary>
        /// <remarks>
        /// A tick in NullableDateTime is the same as a tick in 
        /// System.DateTime.
        /// </remarks>
        public const long TicksPerSecond = 0x989680;
        /// <summary>
        /// Number of ticks per millisecond. This field is read-only.
        /// </summary>
        /// <remarks>
        /// A tick in NullableDateTime is the same as a tick in 
        /// System.DateTime.
        /// </remarks>
        public const long TicksPerMillisecond = 0x2710L;
        #endregion // Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the NullableDateTime structure to the 
        /// specified System.DateTime instant.
        /// </summary>
        /// <param name="value">System.DateTime value.</param>
        public NullableDateTime(sys.DateTime value) {
            _value = value;
            _notNull = true;
        }

        /// <summary>
        /// Initializes a new instance of the DateTime structure to the specified year, month, and day.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in <paramref name="month"/>.</param>
        public NullableDateTime(int year, int month, int day) {
            _value = new sys.DateTime(year, month, day);
            _notNull = true;
        }

        /// <summary>
        /// Initializes a new instance of the DateTime structure to the specified 
        /// year, month, day, hour, minute, and second.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in <paramref name="month"/>.</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>
        public NullableDateTime(int year, int month, int day, int hour, int minute, int second) {
            _value = new sys.DateTime(year, month, day, hour, minute, second);
            _notNull = true;
        }

        /// <summary>
        /// Initializes a new instance of the DateTime structure to the specified 
        /// year, month, day, hour, minute, second, and millisecond.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in <paramref name="month"/>.</param>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        /// <param name="second">The seconds (0 through 59).</param>       
        /// <param name="millisecond">The milliseconds (0 through 999).</param>
        public NullableDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond) {
            _value = new sys.DateTime(year, month, day, hour, minute, second, millisecond);
            _notNull = true;
        }
        #endregion // Constructors

        #region INullable

        /// <summary>
        /// Gets the Null status of this instance. True if the instance
        /// is Null, false otherwise.
        /// </summary>
        /// <value>
        /// true if the value of this structure is Null, 
        /// otherwise false.
        /// </value>
        public bool IsNull { 
            get {return !_notNull;}
        }

        #endregion // INullable

        #region IComparable - Ordering

        /// <summary>
        /// Compares this instance to a specified object and 
        /// returns an indication of their relative values.
        /// </summary>
        /// <param name="value">An object to compare, or a null reference.</param>
        /// <returns>A signed number indicating the relative values of this instance and value. 
        /// See <see cref="sys.DateTime"/>
        /// </returns>
        public int CompareTo(object value) {
            if (value == null)
                return 1;
            else if (!(value is NullableDateTime))
                throw new sys.ArgumentException(string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText("Value is not a {0}."), "NullableTypes.NullableDateTime"));
            else if (((NullableDateTime)value).IsNull) {
                if (_notNull)
                    return 1;
                else
                    return 0;
            }
            else
                return _value.CompareTo(((NullableDateTime)value).Value);
        }

        #endregion // IComparable - Ordering

        #region IXmlSerializable
        /// <summary>
        /// This member supports the .NET Framework infrastructure and is not intended to be used directly 
        /// from your code.
        /// </summary>
        [sys.ComponentModel.EditorBrowsable(sys.ComponentModel.EditorBrowsableState.Never)]
        sysXml.Schema.XmlSchema sysXmlSrl.IXmlSerializable.GetSchema() {
            //
            //    <?xml version="1.0"?>
            //    <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
            //            targetNamespace="http://NullableTypes.SourceForge.Net/XMLSchema"
            //            xmlns="http://NullableTypes.SourceForge.Net/NullableDateTimeXMLSchema"
            //            elementFormDefault="qualified">
            //        <xs:element name="NullableDateTime" type="xs:dateTime" nillable = "true" />
            //    </xs:schema>
            //

            // Element: NullableDateTime
            sysXmlScm.XmlSchemaElement rootElement = new sysXmlScm.XmlSchemaElement();
            rootElement.Name = "NullableDateTime";
            rootElement.SchemaTypeName = 
                new sysXml.XmlQualifiedName("dateTime", "http://www.w3.org/2001/XMLSchema");
            rootElement.IsNillable = true;

            // Xml Schema
            sysXmlScm.XmlSchema xsd = new sysXmlScm.XmlSchema();
            xsd.Id = "NullableDateTime";
            xsd.Namespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
            xsd.TargetNamespace = "http://NullableTypes.SourceForge.Net/NullableDateTimeXMLSchema";
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
            if (_notNull) 
                writer.WriteString(sysXml.XmlConvert.ToString(_value, "yyyy-MM-ddTHH:mm:ss.fffffffzzzzzz"));
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
                _value = sys.Xml.XmlConvert.ToDateTime(elementValue);
                _notNull = true;
            }
            else {
                _notNull = false;
            }
        }
        #endregion // IXmlSerializable

        #region Equivalence
        /// <summary>
        /// Compares this instance to a specified object and 
        /// returns an boolean indicating their equality. Overridden.
        /// </summary>
        /// <param name="value">An object to compare, or null reference</param>
        /// <returns>True if the instance is equal to <paramref name="value"/>. 
        /// False otherwise.
        /// </returns>
        /// ><remarks>Unless the objects are both NullableDateTime objects
        /// and contain equal IsNull and Value properties, this method
        /// returns false.</remarks>
        public override bool Equals(object value) {
            if (!(value is NullableDateTime))
                return false;
            else if (this.IsNull && ((NullableDateTime)value).IsNull)
                return true;
            else if (this.IsNull && !((NullableDateTime)value).IsNull)
                return false;
            else if (((NullableDateTime)value).IsNull)
                return false;
            else
                return (bool) (this == (NullableDateTime)value);
        }


        /// <summary>
        /// Compare two NullableDateTime instances for equality.
        /// </summary>
        /// <param name="x">Instance to compare from.</param>
        /// <param name="y">Instance to compare to.</param>
        /// <returns>True if the two instances are equal, null value
        /// if one or both of the instances are NullableDatetime.Null, 
        /// false if the two instances are not null, but are not equal.
        /// </returns>
        public static NullableBoolean Equals(NullableDateTime x, NullableDateTime y) {
            return (x == y);
        }


        /// <summary>
        /// Compare two NullableDateTime instances for inequality.
        /// </summary>
        /// <param name="x">Instance to compare from.</param>
        /// <param name="y">Instance to compare to.</param>
        /// <returns>False if the two instances are equal, null value
        /// if one or both of the instances are NullableDatetime.Null, 
        /// true if the two instances are not null and they are not equal.
        /// </returns>
        public static NullableBoolean NotEquals(NullableDateTime x, NullableDateTime y) {
            return (x != y);
        }


        /// <summary>
        /// Compare two NullableDateTime instances for equality.
        /// </summary>
        /// <param name="x">Instance to compare from.</param>
        /// <param name="y">Instance to compare to.</param>
        /// <returns>True if the two instances are equal, null value
        /// if one or both of the instances are NullableDatetime.Null, 
        /// false if the two instances are not null, but are not equal.
        /// </returns>
        public static NullableBoolean operator == (NullableDateTime x, NullableDateTime y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;
            else
                return new NullableBoolean (x.Value == y.Value);
        }


        /// <summary>
        /// Compare two NullableDateTime instances for inequality.
        /// </summary>
        /// <param name="x">Instance to compare from.</param>
        /// <param name="y">Instance to compare to.</param>
        /// <returns>False if the two instances are equal, null value
        /// if one or both of the instances are NullableDatetime.Null, 
        /// true if the two instances are not null and they are not equal.
        /// </returns>
        public static NullableBoolean operator != (NullableDateTime x, NullableDateTime y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;
            else
                return new NullableBoolean (!(x.Value == y.Value));
        }


        /// <summary>
        /// Return hashcode. Value equal to NullableDateTime.Value's 
        /// hashcode.
        /// </summary>
        /// <returns>Hashcode value that is equal to Value 
        /// property's hashcode.</returns>
        public override int GetHashCode() {
            return _value.GetHashCode ();
        }
        #endregion // Equivalence

        #region Properties
        /// <summary>
        /// Gets the number of ticks. Uses same tick mechanism as <see cref="System.DateTime"/>.
        /// </summary>
        /// <value>
        /// The number of ticks that represent the date and time of this instance. 
        /// </value>
        public long Ticks {
            get {
                if (this.IsNull)
                    throw new NullableNullValueException();
                else
                    return _value.Ticks;
            }
        }


        /// <summary>
        /// Gets the System.DateTime value represented by the instance.
        /// </summary>
        /// <value>The value of the <see cref="NullableDateTime"/> structure.</value>
        /// <exception cref="NullableNullValueException">
        /// The property is set to null.
        /// </exception>
        public sys.DateTime Value {
            get { 
                if (this.IsNull) 
                    throw new NullableNullValueException();
                else 
                    return _value; 
            }
        }
        #endregion // Properties

        #region Methods
        /// <summary>
        /// Returns the <see cref="TypeCode"/> for value type 
        /// <see cref="NullableDateTime"/>.
        /// </summary>
        /// <returns>
        /// The enumerated constant, <see cref="TypeCode.NullableDateTime"/>.
        /// </returns>
        public TypeCode GetTypeCode() {
            return TypeCode.NullableDateTime;
        }

        /// <summary>
        /// Adds a specified time interval to this structure. 
        /// </summary>
        /// <param name="t">A System.TimeSpan</param>
        public void Add(sys.TimeSpan t) {
            if (_notNull)
                _value += t;
        }

        /// <summary>
        /// Subtracts a specified time interval to this structure. 
        /// </summary>
        /// <param name="t">A System.TimeSpan</param>
        /// <returns>
        /// A NullableDateTime that is the difference of this structure and <paramref name="t"/>.
        /// </returns>
        /// <remarks>
        /// This method does not change the value of this NullableDateTime. Instead, a new NullableDateTime
        ///  is returned whose value is the result of this operation.
        /// </remarks>
        public NullableDateTime Subtract(sys.TimeSpan t) {
            if (IsNull)
                return NullableDateTime.Null;
            return new NullableDateTime(_value - t);
        }

        /// <summary>
        /// Compare two NullableDateTime instances.
        /// </summary>
        /// <param name="x">Instance to compare from.</param>
        /// <param name="y">Instance to compare to.</param>
        /// <returns>True if <paramref name="x"/> is greater than <paramref name="y"/>, 
        /// null value if one or both of the instances are NullableDatetime.Null, 
        /// false if <paramref name="x"/> is less than or equal to <paramref name="y"/>.
        /// </returns>
        public static NullableBoolean GreaterThan(NullableDateTime x, NullableDateTime y) {
            return (x > y);
        }

        /// <summary>
        /// Compare two NullableDateTime instances.
        /// </summary>
        /// <param name="x">Instance to compare from.</param>
        /// <param name="y">Instance to compare to.</param>
        /// <returns>True if <paramref name="x"/> is greater than or equal to <paramref name="y"/>, 
        /// null value if one or both of the instances are NullableDatetime.Null, 
        /// false if <paramref name="x"/> is less than <paramref name="y"/>.
        /// </returns>
        public static NullableBoolean GreaterThanOrEqual(NullableDateTime x, NullableDateTime y) {
            return (x >= y);
        }

        /// <summary>
        /// Compare two NullableDateTime instances.
        /// </summary>
        /// <param name="x">Instance to compare from.</param>
        /// <param name="y">Instance to compare to.</param>
        /// <returns>True if <paramref name="x"/> is less than <paramref name="y"/>, 
        /// null value if one or both of the instances are NullableDatetime.Null, 
        /// false if <paramref name="x"/> is greater than or equal to <paramref name="y"/>.
        /// </returns>
        public static NullableBoolean LessThan(NullableDateTime x, NullableDateTime y) {
            return (x < y);
        }

        /// <summary>
        /// Compare two NullableDateTime instances.
        /// </summary>
        /// <param name="x">Instance to compare from.</param>
        /// <param name="y">Instance to compare to.</param>
        /// <returns>True if <paramref name="x"/> is less than or equal to <paramref name="y"/>, 
        /// null value if one or both of the instances are NullableDatetime.Null, 
        /// false if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        public static NullableBoolean LessThanOrEqual(NullableDateTime x, NullableDateTime y) {
            return (x <= y);
        }

        // Note: MSDN recommends that you use Convert.ToDateTime rather than
        // DateTime.Parse for parsing. This is true for all basic value types.
        // The end result should be exactly the same. - EL
        /// <summary>
        /// Parses a string expression of an instant in time.
        /// </summary>
        /// <param name="s">string instance</param>
        /// <returns>NullableDateTime instance containing 
        /// a value representative of the instant in time.</returns>
        /// <exception cref="sys.ArgumentNullException">
        /// <paramref name="s"/> is null.</exception>
        /// <exception cref="sys.FormatException">
        /// <paramref name="s"/> is not a properly formatted date and time.
        /// </exception>
        /// <exception cref="sys.OverflowException">
        /// <paramref name="s"/> represents a time instant less than MinValue or
        /// greater than MaxValue.
        /// </exception>
        public static NullableDateTime Parse(string s) {
            if( s == null )
                throw new sys.ArgumentNullException("s");
            else {
                sys.DateTime d;
                try {
                    d = sys.Convert.ToDateTime(s, sysGlb.CultureInfo.CurrentCulture);
                }
                catch(sys.ArgumentOutOfRangeException) {
                    // this is a bug of the System.DateTime.Parse method that throw ArgumentOutOfRangeException 
                    // instead of OverflowException; this bug is in .NET Framework 1.0 and is fixed in 
                    // .NET Framework 1.1
                    throw new sys.OverflowException();
                }
                return new NullableDateTime(d);
            }
        }
        #endregion // Methods

        #region Operators
        /// <summary>
        /// Adds a specified time interval to a specified date and time, 
        /// yielding a new date and time.
        /// </summary>
        /// <param name="x">A NullableDateTime</param>
        /// <param name="t">A System.TimeSpan</param>
        /// <returns>
        /// A NullableDateTime that is the sum of <paramref name="x"/> and <paramref name="t"/>.
        /// </returns>
        public static NullableDateTime operator + (NullableDateTime x, sys.TimeSpan t) {
            if (x.IsNull)
                return NullableDateTime.Null;
            
            return new NullableDateTime(x.Value + t);
        }


        /// <summary>
        /// Compare two NullableDateTime instances.
        /// </summary>
        /// <param name="x">Instance to compare from.</param>
        /// <param name="y">Instance to compare to.</param>
        /// <returns>True if <paramref name="x"/> is greater than <paramref name="y"/>, 
        /// null value if one or both of the instances are NullableDatetime.Null, 
        /// false if <paramref name="x"/> is less than or equal to <paramref name="y"/>.
        /// </returns>
        public static NullableBoolean operator > (NullableDateTime x, NullableDateTime y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;
            else
                return new NullableBoolean(x.Value > y.Value);
        }


        /// <summary>
        /// Compare two NullableDateTime instances.
        /// </summary>
        /// <param name="x">Instance to compare from.</param>
        /// <param name="y">Instance to compare to.</param>
        /// <returns>True if <paramref name="x"/> is greater than or equal to <paramref name="y"/>, 
        /// null value if one or both of the instances are NullableDatetime.Null, 
        /// false if <paramref name="x"/> is less than <paramref name="y"/>.
        /// </returns>
        public static NullableBoolean operator >= (NullableDateTime x, NullableDateTime y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;
            else
                return new NullableBoolean(x.Value >= y.Value);
        }


        /// <summary>
        /// Compare two NullableDateTime instances.
        /// </summary>
        /// <param name="x">Instance to compare from.</param>
        /// <param name="y">Instance to compare to.</param>
        /// <returns>True if <paramref name="x"/> is less than <paramref name="y"/>, 
        /// null value if one or both of the instances are NullableDatetime.Null, 
        /// false if <paramref name="x"/> is greater than or equal to <paramref name="y"/>.
        /// </returns>
        public static NullableBoolean operator < (NullableDateTime x, NullableDateTime y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;
            else
                return new NullableBoolean(x.Value < y.Value);
        }

        /// <summary>
        /// Compare two NullableDateTime instances.
        /// </summary>
        /// <param name="x">Instance to compare from.</param>
        /// <param name="y">Instance to compare to.</param>
        /// <returns>True if <paramref name="x"/> is less than or equal to <paramref name="y"/>, 
        /// null value if one or both of the instances are NullableDatetime.Null, 
        /// false if <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        public static NullableBoolean operator <= (NullableDateTime x, NullableDateTime y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;
            else
                return new NullableBoolean(x.Value <= y.Value);
        }

        /// <summary>
        /// Subtracts a specified time interval to a specified date and time, 
        /// yielding a new date and time.
        /// </summary>
        /// <param name="x">A NullableDateTime</param>
        /// <param name="t">A System.TimeSpan</param>
        /// <returns>
        /// A NullableDateTime that is the difference of <paramref name="x"/> and <paramref name="t"/>.
        /// </returns>
        public static NullableDateTime operator - (NullableDateTime x, sys.TimeSpan t) {
            if (x.IsNull)
                return NullableDateTime.Null;
            return new NullableDateTime(x.Value - t);
        }
        #endregion // Operators

        #region Conversion Operators
        /// <summary>
        /// Converts a NullableDateTime instance to a System.DateTime instance.
        /// </summary>
        /// <param name="x">NullableDateTime instance</param>
        /// <returns>The Value property of NullableDateTime instance</returns>
        public static explicit operator sys.DateTime (NullableDateTime x) {
            return x.Value;
        }


        /// <summary>
        /// Converts a NullableString instance to a NullableDateTime instance.
        /// </summary>
        /// <param name="x">NullableString instance</param>
        /// <returns>The NullableDateTime equivalent of the value indicated by
        /// the NullableDateTime instance.</returns>
        public static explicit operator NullableDateTime (NullableString x) {
            return NullableDateTime.Parse(x.Value);
        }


        /// <summary>
        /// Converts a System.DateTime instance to a NullableDateTime instance.
        /// </summary>
        /// <param name="x">System.DateTime instance</param>
        /// <returns>NullableDateTime instance containing the value indicated by 
        /// x.</returns>
        public static implicit operator NullableDateTime (sys.DateTime x) {
            return new NullableDateTime(x);
        }
        #endregion // Conversion Operators

        #region Conversions
        /// <summary>
        /// Converts to a NullableString representing the time instance.
        /// </summary>
        /// <returns>A NullableString representative of the NullableDatetime time instance.</returns>
        public NullableString ToNullableString () {
            return ((NullableString)this);
        }


        /// <summary>
        /// Converts the NullableDateTime value to a string.
        /// </summary>
        /// <returns>A string representative of the NullableDateTime time instance.</returns>
        public override string ToString () {    
            if (this.IsNull)
                return Locale.GetText("Null");
            else
                return _value.ToString(sysGlb.CultureInfo.CurrentCulture);
        }
        #endregion // Conversions

    }
}