//
// NullableTypes.NullableInt16
// 
// Author: Abhijeet Dev (abhijeet_dev@users.sourceforge.net)
//         Luca Minudel (lukadotnet@users.sourceforge.net)
//         Damien Guard (damienguard@users.sourceforge.net)
//         Partha Choudhury (parthachoudhury@users.sourceforge.net)
//
// Date         Author   Changes     Reasons
// 03-Jun-2003  Abhijeet Create
// 24-Jun-2003  Luca     Create      Added XML documentation
// 27-Jun-2003  Luca     Refactoring Unified equivalent error messages  
// 30-Jun-2003  Luca     Upgrade     New requirement by Roni Burd: GetTypeCode
// 30-Jun-2003  Luca     Upgrade     Removed NullableMoney and NullableGuid
// 07-Jul-2003  Luca     Upgrade     Applied FxCop guideline: explicited CurrentCulture in CompareTo, Parse  
//                                   and ToString code
// 13-Sep-2003  Luca     Upgrade     New requirement: the type must be sarializable
// 05-Oct-2003  DamienG  Upgrade     New requirement: the type must be XmlSerializable
// 06-Oct-2003  Luca     Upgrade     Code upgrade: Replaced tabs with spaces and removed commented out code
// 06-Dic-2003  Luca     Bug Fix     Replaced Xml Schema id "NullableInt16XmlSchema" with "NullableInt16"
//                                   because VSDesigner use it as type-name in the auto-generated WS client Proxy
// 06-Dic-2003  Luca     Bug Fix     Replaced Target Namespace for Xml Schema to avoid duplicate namespace with
//                                   other types in NullableTypes
// 30-Dec-2003  Partha   Upgrade     Replaced hardcoded MInValue and MaxValue values with Int16.MinValue/MaxValue
// 18-Feb-2004  Luca    Bug Fix      ReadXml must read nil value also with a non empty element
//

namespace NullableTypes {
    using sys = System;
    using sysGlb = System.Globalization;
    using sysXml = System.Xml;
    using sysXmlSrl = System.Xml.Serialization;
    using sysXmlScm = System.Xml.Schema;

    /// <summary>
    /// Represents an Int16 that is either a 16-bit signed integer or 
    /// <see cref="Null"/>.
    /// </summary>
    [sys.Serializable]
    public struct NullableInt16 : NullableTypes.INullable, sys.IComparable, sysXmlSrl.IXmlSerializable {
        
        #region Fields
        /// <summary>
        /// Value of this object, if it's not null
        /// </summary>
        short _value;
        /// <summary>
        /// Determines if this opject is null or not
        /// </summary>
        private bool _notNull;
        /// <summary>
        /// Maximum value of this type of object
        /// </summary>
        public static readonly NullableInt16 MaxValue = new NullableInt16 (sys.Int16.MaxValue);
        /// <summary>
        /// Minimum value of this type of object
        /// </summary>
        public static readonly NullableInt16 MinValue = new NullableInt16 (sys.Int16.MinValue);
        /// <summary>
        /// Null initializer for this object.
        /// <para>
        /// A Null is the absence of a value because missing, unknown, or inapplicable 
        /// value. A Null should not be used to imply any other value (such as zero).
        /// Also any value (such as zero) should not be used to imply the absence of a 
        /// value, that's why Null exists.
        /// </para>
        /// </summary>
        public static readonly NullableInt16 Null;
        /// <summary>
        /// Zero value Nullable16 object
        /// </summary>
        public static readonly NullableInt16 Zero = new NullableInt16 (0);
        #endregion //Fields
        
        #region Constructors
        /// <summary>
        /// Constructor for NullableInt16 type objects
        /// </summary>
        /// <param name="value">value that it should contain, if constructed.</param>
        public NullableInt16(short value) {
            _value = value;
            _notNull = true;
        }
        #endregion // Constructors

        #region INullable
        /// <summary>
        /// Indicates whether or not the value of the <see cref="NullableInt32"/> 
        /// structure is Null.
        /// </summary>
        /// <value>
        /// true if the <see cref="NullableInt16"/> structure is Null, otherwise 
        /// false.
        /// </value>
        public bool IsNull {
            get { return !_notNull; }
        }
        #endregion // INullable

        #region IComparable - Ordering
        /// <summary>
        /// Implementation for IComparable interface. 
        /// </summary>
        /// <param name="value">object that it has to be compared to.</param>
        /// <returns>if this is not null, and argument is not null - CompareTo of short values
        /// <BR>If both null - 0</BR>
        /// If argument is null - 1
        /// <BR>If this is null -1</BR>
        /// </returns>
        public int CompareTo(object value) {
            if (value == null)
                return 1;
            
            if (!(value is NullableInt16))
                throw new sys.ArgumentException(string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText("Value is not a {0}."), "NullableTypes.NullableInt16"));
            
            NullableInt16 iValue = (NullableInt16)value;
            if (iValue.IsNull && this.IsNull)
                return 0;

            if (iValue.IsNull)
                return 1;

            if (this.IsNull)
                return -1;
            
            return _value.CompareTo(iValue.Value);
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
            //            targetNamespace="http://NullableTypes.SourceForge.Net/NullableInt16XMLSchema"
            //            xmlns="http://NullableTypes.SourceForge.Net/NullableInt16XMLSchema"
            //            elementFormDefault="qualified">
            //        <xs:element name="NullableInt16" type="xs:short" nillable="true" />
            //    </xs:schema>

            // Element: NullableInt16
            sysXmlScm.XmlSchemaElement rootElement = new sysXmlScm.XmlSchemaElement();
            rootElement.Name = "NullableInt16";
            rootElement.SchemaTypeName = 
                new sysXml.XmlQualifiedName("short", "http://www.w3.org/2001/XMLSchema");
            rootElement.IsNillable = true;

            // Xml Schema
            sysXmlScm.XmlSchema xsd = new sysXmlScm.XmlSchema();
            xsd.Id = "NullableInt16";
            xsd.Namespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
            xsd.TargetNamespace = "http://NullableTypes.SourceForge.Net/NullableInt16XMLSchema";
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
                _value = sysXml.XmlConvert.ToInt16(elementValue);
                _notNull = true;
            }
            else
                _notNull = false;
        }
        #endregion // IXmlSerializable

        #region Properties
        /// <summary>
        /// Gets the value of the <see cref="NullableInt16"/> structure. This 
        /// property is read-only
        /// </summary>
        /// <value>The value of the <see cref="NullableInt16"/> structure.</value>
        /// <exception cref="NullableNullValueException">
        /// The property is set to Null.
        /// </exception> 
        public short Value {
            get {
                if (this.IsNull) throw new NullableNullValueException(); 
                return _value;
            }
        }
        #endregion // Properties
    
        #region Equivalence
        /// <summary>
        /// Determines if this NullableInt16 is equal to the specified NullableInt16
        /// </summary>
        /// <param name="value">value to be compared to</param>
        /// <returns>whether this NullableInt16 is equal to specified NullableInt16</returns>
        public override bool Equals(object value) {
            if (!(value is NullableInt16))
                return false;

            NullableInt16 iValue = (NullableInt16)value;

            return (this._value == iValue._value) && 
                (this._notNull == iValue._notNull);
        }


        /// <summary>
        /// Determines whether given NullableInt16 values are equal or not.
        /// </summary>
        /// <param name="x">First NullableInt16 value</param>
        /// <param name="y">Second NullableInt16 value</param>
        /// <returns>true if specified NullableInt16 value are not equal.</returns>
        public static NullableBoolean NotEquals(NullableInt16 x, NullableInt16 y) {
            return (x != y);
        }


        /// <summary>
        /// Determines hashcode of this NullableInt16
        /// </summary>
        /// <returns>HashCode of this NullableInt16</returns>
        public override int GetHashCode() {
            // if IsNull then _value will always be 0
            // hash code for Null will be 0
            return _value;
        }


        /// <summary>
        /// Determines if two NullableInt16 values are equal or not
        /// </summary>
        /// <param name="x">A <see cref="NullableInt16"/> structure.</param>
        /// <param name="y">A <see cref="NullableInt16"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.False"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableInt16"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean Equals(NullableInt16 x, NullableInt16 y) {
            return (x == y);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableInt16"/> for equivalence.
        /// </summary>
        /// <param name="x">A <see cref="NullableInt16"/> structure.</param>
        /// <param name="y">A <see cref="NullableInt16"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.True"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.False"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableInt16"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean operator == (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;

            return new NullableBoolean (x.Value == y.Value);
        }


        /// <summary>
        /// Compares two instances of <see cref="NullableInt16"/> for equivalence.
        /// </summary>
        /// <param name="x">A <see cref="NullableInt16"/> structure.</param>
        /// <param name="y">A <see cref="NullableInt16"/> structure.</param>
        /// <returns>
        /// A <see cref="NullableBoolean"/> that is 
        /// <see cref="NullableBoolean.False"/> if the two instances are equivalent 
        /// or <see cref="NullableBoolean.True"/> if the two instances are not 
        /// equivalent. If either instance of <see cref="NullableInt16"/> is 
        /// <see cref="Null"/>, the <see cref="NullableBoolean.Value"/> of the 
        /// <see cref="NullableBoolean"/> will be <see cref="NullableBoolean.Null"/>.
        /// </returns>
        public static NullableBoolean operator != (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;

            return new NullableBoolean (x.Value != y.Value);
        }
        #endregion // Equivalence
        
        #region Methods
        /// <summary>
        /// Returns the <see cref="TypeCode"/> for value type 
        /// <see cref="NullableInt16"/>.
        /// </summary>
        /// <returns>
        /// The enumerated constant, <see cref="TypeCode.NullableInt16"/>.
        /// </returns>
        public TypeCode GetTypeCode() {
            return TypeCode.NullableInt16;
        }


        /// <summary>
        /// Returns the absolute value of a Nullable Int16
        /// </summary>
        /// <param name="x">A number in range -32768 to 32767</param>
        /// <returns>A NullableInt16 number y such that, y is in between 0 and 32767(both included) </returns>
        public static NullableInt16 Abs(NullableInt16 x) {
            if (x.IsNull || x.Value > 0)
                return x;
            return new NullableInt16(sys.Math.Abs(x.Value));
        }


        /// <summary>
        /// Returns the result of adding two NullableInt16
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <param name="y">NullableInt16 value</param>
        /// <returns>result of addition operation</returns>
        public static NullableInt16 Add (NullableInt16 x, NullableInt16 y) {
            return (x + y);
        }


        /// <summary>
        /// returns the result of bitwise And operation on NullableInt16 values
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <param name="y">NullableInt16 value</param>
        /// <returns>result of 'bitwise And' operation</returns>
        public static NullableInt16 BitwiseAnd(NullableInt16 x, NullableInt16 y) {
            return (x & y);
        }
        

        /// <summary>
        /// returns the result of bitwise 'Or' operation on NullableInt16 values
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <param name="y">NullableInt16 value</param>
        /// <returns>result of 'bitwise Or' operation</returns>
        public static NullableInt16 BitwiseOr(NullableInt16 x, NullableInt16 y) {
            return (x | y);
        }

        
        /// <summary>
        /// Returns the result of division performed on NullableInt16 values
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <param name="y">NullableInt16 value</param>
        /// <returns>result of division operation</returns>
        public static NullableInt16 Divide(NullableInt16 x, NullableInt16 y) {
            return (x / y);
        }


        /// <summary>
        /// returns the result of greater than comparision between NullableInt16 values
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <param name="y">NullableInt16 value</param>
        /// <returns>true if x>y, false otherwise</returns>
        public static NullableBoolean GreaterThan (NullableInt16 x, NullableInt16 y) {
            return (x > y);
        }


        /// <summary>
        /// returns the result of 'greater than or equals' comparision between NullableInt16 values
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <param name="y">NullableInt16 value</param>
        /// <returns>true id x>=y false otherwise</returns>
        public static NullableBoolean GreaterThanOrEqual (NullableInt16 x, NullableInt16 y) {
            return (x >= y);
        }
                

        /// <summary>
        /// returns the result of less than comparision between NullableInt16 values
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <param name="y">NullableInt16 value</param>
        /// <returns>true if x is less then y, false otherwise</returns>
        public static NullableBoolean LessThan(NullableInt16 x, NullableInt16 y) {
            return (x < y);
        }


        /// <summary>
        /// returns the result of 'less than or equals' comparision between NullableInt16 values
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <param name="y">NullableInt16 value</param>
        /// <returns>true if x is less then y, false otherwise</returns>
        public static NullableBoolean LessThanOrEqual(NullableInt16 x, NullableInt16 y) {
            return (x <= y);
        }


        /// <summary>
        /// returns modulo for NullableInt16 values
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <param name="y">NullableInt16 value</param>
        /// <returns>modulo of x, base y</returns>
        public static NullableInt16 Mod(NullableInt16 x, NullableInt16 y) {
            return (x % y);
        }


        /// <summary>
        /// returns the result of multiplication of two NullableInt16 values
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <param name="y">NullableInt16 value</param>
        /// <returns>x multiplied by y</returns>
        public static NullableInt16 Multiply(NullableInt16 x, NullableInt16 y) {
            return (x * y);
        }
    

        /// <summary>
        /// 1's complement of a NullableInt16 value
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <returns>1's complement of x</returns>
        public static NullableInt16 OnesComplement(NullableInt16 x) {
            return ~x;
        }


        /// <summary>
        /// parse NullableInt16 value from a string
        /// </summary>
        /// <param name="s">string of NullableInt16 value</param>
        /// <returns>NullableInt16 value: result of parsing the string s</returns>
        public static NullableInt16 Parse(string s) {
            return new NullableInt16 (short.Parse(s, sysGlb.CultureInfo.CurrentCulture));
        }


        /// <summary>
        /// returns the result of subtraction of a NullableInt16 value from another.
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <param name="y">NullableInt16 value</param>
        /// <returns>y subtracted from x</returns>
        public static NullableInt16 Subtract(NullableInt16 x, NullableInt16 y) {
            return (x - y);
        }
        #endregion // Methods

        #region Operators
        /// <summary>
        /// Returns result of adding the operands
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <param name="y">Nullable Int16 value</param>
        /// <returns>result of addition</returns>
        public static NullableInt16 operator + (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull)
                return NullableInt16.Null;

            checked {
                return new NullableInt16((short)(x.Value + y.Value));
            }
        }


        /// <summary>
        /// Bitwise AND operator
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <param name="y">Nullable Int16 value</param>
        /// <returns>result of bitwise 'and' operation</returns>
        public static NullableInt16 operator & (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull)
                return NullableInt16.Null;

            return new NullableInt16((short)(x.Value & y.Value));
        }


        /// <summary>
        /// Bitwise OR operator
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <param name="y">Nullable Int16 value</param>
        /// <returns>result of bitwise 'or' operation</returns>
        public static NullableInt16 operator | (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull)
                return NullableInt16.Null;

            unchecked {
                return new NullableInt16((short)((int)(ushort)x.Value | (int)(ushort) y.Value));
            }
        }


        /// <summary>
        /// Division operation
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <param name="y">Nullable Int16 value</param>
        /// <returns>result of division operation</returns>
        public static NullableInt16 operator / (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull)
                return NullableInt16.Null;
            
            checked {
                return new NullableInt16 ((short)(x.Value / y.Value));
            }
        }


        /// <summary>
        /// Bitwise exclusive OR operator
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <param name="y">Nullable Int16 value</param>
        /// <returns>result of bitwise exclusive 'or' operation</returns>
        public static NullableInt16 operator ^ (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull)
                return NullableInt16.Null;

            return new NullableInt16 ((short)(x.Value ^ y.Value));
        }


        /// <summary>
        /// Comparison: greater than
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <param name="y">Nullable Int16 value</param>
        /// <returns>result of 'greater than' comparison</returns>
        public static NullableBoolean operator >(NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;

            return new NullableBoolean (x.Value > y.Value);
        }


        /// <summary>
        /// Comparison: greater than or equals
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <param name="y">Nullable Int16 value</param>
        /// <returns>result of greater than or equals comparison</returns>
        public static NullableBoolean operator >= (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;

            return new NullableBoolean (x.Value >= y.Value);
        }


        /// <summary>
        /// Comparison: Less than
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <param name="y">Nullable Int16 value</param>
        /// <returns>result of less than comparison</returns>
        public static NullableBoolean operator < (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;

            return new NullableBoolean (x.Value < y.Value);
        }


        /// <summary>
        /// Comparison: less than or equals
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <param name="y">Nullable Int16 value</param>
        /// <returns>result of less than or equals operation</returns>
        public static NullableBoolean operator <= (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull) 
                return NullableBoolean.Null;

            return new NullableBoolean (x.Value <= y.Value);
        }


        /// <summary>
        /// Modulus operation
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <param name="y">Nullable Int16 value</param>
        /// <returns>result of modulus operation</returns>
        public static NullableInt16 operator % (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull) 
                return NullableInt16.Null;
            // negative of MinValue (-32768) is more than MaxValue (32767)
            if ((x.Value==NullableInt16.MinValue.Value)&&(y.Value==-1))
                throw new sys.OverflowException();
            if (y.Value!=0)
                return new NullableInt16((short)(x.Value % y.Value));
            throw new sys.DivideByZeroException();
        }


        /// <summary>
        /// Multiplication operation
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <param name="y">Nullable Int16 value</param>
        /// <returns>result of multiplication operation</returns>
        public static NullableInt16 operator * (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull) 
                return NullableInt16.Null;

            checked {
                return new NullableInt16 ((short)(x.Value * y.Value));
            }
        }


        /// <summary>
        /// finds out one's compliment of a NullableInt16 value
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <returns>One's complement of a Nullable Int16 value</returns>
        public static NullableInt16 operator ~ (NullableInt16 x) {
            if (x.IsNull) 
                return NullableInt16.Null;

            return new NullableInt16 ((short)(~x.Value));
        }


        /// <summary>
        /// Computes the subtraction of the two specified <see cref="NullableInt16"/> 
        /// structures.
        /// </summary>
        /// <param name="x">A <see cref="NullableInt16"/> structure.</param>
        /// <param name="y">A <see cref="NullableInt16"/> structure.</param>
        /// <returns>
        /// <see cref="Null"/> if either instance of <see cref="NullableInt16"/> is 
        /// <see cref="Null"/> otherwise a <see cref="NullableInt16"/> structure 
        /// whose <see cref="Value"/> property contains the results of the 
        /// subtraction.
        /// </returns>
        /// <exception cref="System.OverflowException">
        /// The subtraction compute a number that is less than 
        /// <see cref="System.Int16.MinValue"/> or greater than 
        /// <see cref="System.Int16.MaxValue"/>.
        /// </exception>
        public static NullableInt16 operator - (NullableInt16 x, NullableInt16 y) {
            if (x.IsNull || y.IsNull) 
                return NullableInt16.Null;

            checked {
                return new NullableInt16 ((short)(x.Value - y.Value));
            }
        }


        /// <summary>
        /// Negates a Nullable Int16 value
        /// </summary>
        /// <param name="x">Nullable Int16 value</param>
        /// <returns>negative of the operand</returns>
        public static NullableInt16 operator - (NullableInt16 x) {
            if (x.IsNull)
                return NullableInt32.Null;

            checked {
                return new NullableInt16 ((short)(-x.Value));
            }
        }
        #endregion // Operators

        #region Conversion Operators
        /// <summary>
        /// Converts NullableBoolean to NullableInt16
        /// </summary>
        /// <param name="x">NullableBoolean value</param>
        /// <returns>equivalent NullableInt16</returns>
        public static explicit operator NullableInt16(NullableBoolean x) {
            if (x.IsNull) 
                return NullableInt16.Null;

            return new NullableInt16((short)x.ByteValue);
        }


        /// <summary>
        /// converts NullableDecimal value to NullableInt16 value
        /// </summary>
        /// <param name="x">NullableDecimal value</param>
        /// <returns>equivalent NullableInt16</returns>
        public static explicit operator NullableInt16 (NullableDecimal x) {
            if (x.IsNull) 
                return NullableInt16.Null;

            return new NullableInt16(sys.Decimal.ToInt16(x.Value));
        }


        /// <summary>
        /// Converts NullableDouble to NullableInt16 value
        /// </summary>
        /// <param name="x">NullableDouble value</param>
        /// <returns>equivalent NullableInt16</returns>
        public static explicit operator NullableInt16 (NullableDouble x) {
            if (x.IsNull) 
                return NullableInt16.Null;

            checked { 
                return new NullableInt16((short)x.Value);
            }
        }
        

        /// <summary>
        /// Converts NullableInt16 to short
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <returns>equivalent short value</returns>
        public static explicit operator short (NullableInt16 x) {
            return x.Value;
        }


        /// <summary>
        /// Converts NullableInt16 to int
        /// </summary>
        /// <param name="x">NullableInt16 value</param>
        /// <returns>equivalent int value</returns>
        public static explicit operator int (NullableInt16 x) {
            return (int)x.Value;
        }


        /// <summary>
        /// Converts NullableInt64 to NullableInt16 value
        /// </summary>
        /// <param name="x">NullableInt64 value</param>
        /// <returns>equivalent NullableInt16</returns>
        public static explicit operator NullableInt16 (NullableInt64 x) {
            if (x.IsNull) 
                return NullableInt16.Null;

            checked { 
                return new NullableInt16((short)x.Value);
            }
        }
        




        /// <summary>
        /// Converts NullableSingle to NullableInt16
        /// </summary>
        /// <param name="x">Nullable single value</param>
        /// <returns>equivalent NullableInt16</returns>
        public static explicit operator NullableInt16(NullableSingle x) {
            if (x.IsNull) 
                return NullableInt16.Null;

            checked { 
                return new NullableInt32((short)x.Value);
            }
        }


        /// <summary>
        /// Converts NullableString to NullableInt16 value
        /// </summary>
        /// <param name="x">Nullable string value</param>
        /// <returns>equivalent NullableInt16</returns>
        public static explicit operator NullableInt16(NullableString x) {
            if (x.IsNull) 
                return NullableInt16.Null;

            return NullableInt16.Parse(x.Value);
        }


        /// <summary>
        /// Converts short to to NullableInt16 value
        /// </summary>
        /// <param name="x">short value</param>
        /// <returns>equivalent NullableInt16</returns>
        public static implicit operator NullableInt16(short x) {
            return new NullableInt16 (x);
        }


        /// <summary>
        /// Converts NullableByte to NullableInt16 value
        /// </summary>
        /// <param name="x">NullableByte value</param>
        /// <returns>equivalent NullableInt16</returns>
        public static implicit operator NullableInt16(NullableByte x) {
            if (x.IsNull) 
                return NullableInt16.Null;

            return new NullableInt16 ((short)x.Value);
        }


        /// <summary>
        /// Converts NullableInt32 to NullableInt16 value
        /// </summary>
        /// <param name="x">NullableInt32 value</param>
        /// <returns>equivalent NullableInt16</returns>
        public static implicit operator NullableInt16(NullableInt32 x) {
            if (x.IsNull) 
                return NullableInt16.Null;

            return new NullableInt16 ((short)x.Value);
        }
        #endregion // Conversion Operators

        #region Conversions
        /// <summary>
        /// Converts the value of this instance to a NullableBoolean value
        /// </summary>
        /// <returns>NullableBoolean equivalent</returns>
        public NullableBoolean ToNullableBoolean() {
            return ((NullableBoolean)this);
        }


        /// <summary>
        /// Converts the value of this instance to a NullableDecimal value
        /// </summary>
        /// <returns>NullableDecimal equivalent</returns>
        public NullableByte ToNullableByte() {
            return ((NullableByte)this);
        }


        /// <summary>
        /// Converts the value of this instance to a NullableDecimal value
        /// </summary>
        /// <returns>NullableDecimal equivalent</returns>
        public NullableDecimal ToNullableDecimal() {
            return ((NullableDecimal)this);
        }


        /// <summary>
        /// Converts the value of this instance to a NullableDouble value
        /// </summary>
        /// <returns>NullableDouble equivalent</returns>
        public NullableDouble ToNullableDouble() {
            return ((NullableDouble)this);
        }


        /// <summary>
        /// Converts the value of this instance to a ToNullableInt64 value
        /// </summary>
        /// <returns>NullableInt64 equivalent</returns>
        public NullableInt32 ToNullableInt32() {
            return ((NullableInt16)this);
        }


        /// <summary>
        /// Converts the value of this instance to a ToNullableInt64 value
        /// </summary>
        /// <returns>NullableInt64 equivalent</returns>
        public NullableInt64 ToNullableInt64() {
            return ((NullableInt64)this);
        }




        /// <summary>
        /// Converts the value of this instance to a NullableSingle value
        /// </summary>
        /// <returns>NullableSingle equivalent</returns>
        public NullableSingle ToNullableSingle() {
            return ((NullableSingle)this);
        }


        /// <summary>
        /// Converts the value of this instance to a NullableString value
        /// </summary>
        /// <returns>NullableString equivalent</returns>
        public NullableString ToNullableString () {
            return ((NullableString)this);
        }


        /// <summary>
        /// Converts the value of this instance to a string value
        /// </summary>
        /// <returns>string</returns>
        public override string ToString() {
            if (this.IsNull)
                return Locale.GetText("Null");

            return _value.ToString(sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Calculates exclusive or of two NullableInt16 values
        /// </summary>
        /// <param name="x">a NullableType value</param>
        /// <param name="y">a nullableType value</param>
        /// <returns>result of the XOR operation</returns>
        public static NullableInt16 Xor(NullableInt16 x, NullableInt16 y) {
            return (x ^ y);
        }
        #endregion // Conversions
    }
}