//
// NullableTypes.Tests.NullableDateTimeTest
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Eric Lau (elikfunglau@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 14-Apr-2003  Luca    Create
// 16-May-2003  Eric    Create     Test suit completed
// 13-Sep-2003  Luca    Upgrade    New test: SerializableAttribute
// 05-Oct-2003  DamienG Upgrade    New test: XmlSerializable
//                                 Code upgrade: Added min and max for serialize tests to ensure 
//                                 full range persists
//                                 Code upgrade: Tidy up source
// 06-Ott-2003  Luca    Upgrade    New test: XmlSerializableSchema
// 06-Oct-2003  Luca    Upgrade    Code upgrade: Replaced tabs with spaces and removed commented out code
// 06-Dic-2003  Luca    Bug Fix    Replaced Target Namespace for Xml Schema to reflect changes in the target type
// 18-Feb-2004  Luca    Upgrade    New test: XmlSerializableEmptyElementNil for xml deserialization of a nil 
//                                 value with a non empty element
//

namespace NullableTypes.Tests {
    using nu = NUnit.Framework;
    using nua = NUnit.Framework.Assertion;
    using sys = System;
    using sysXml = System.Xml;
    using sysXmlScm = System.Xml.Schema;

    [nu.TestFixture]
    public class NullableDateTimeTest {
        NullableDateTime _past;
        NullableDateTime _future;
        NullableDateTime _null;
        NullableDateTime _min;
        NullableDateTime _max;


        [nu.SetUp]
        public void SetUp() {
            _past = new NullableDateTime(new sys.DateTime(2000, 1, 1));
            _future = new NullableDateTime(2025, 5, 5);
            _null = NullableDateTime.Null;
            _min = NullableDateTime.MinValue;
            _max = NullableDateTime.MaxValue;
        }

        #region Field Tests - A#
        [nu.Test]
        public void FieldMax() {
            nua.AssertEquals("TestA#01", sys.DateTime.MaxValue, 
                NullableDateTime.MaxValue.Value);
        }

        [nu.Test]
        public void FieldMin() {
            nua.AssertEquals("TestA#02", sys.DateTime.MinValue, 
                NullableDateTime.MinValue.Value);
        }

        [nu.Test]
        public void NullField() {
            nua.Assert("TestA#03", NullableDateTime.Null.IsNull);
        }
        #endregion // Field Tests - A#

        #region Constructor Tests - B#
        [nu.Test]
        public void Create() {
            NullableDateTime dtCtor1 = new NullableDateTime(2012, 01, 22, 5, 43, 34, 434);
            nua.AssertEquals ("TestB#01", 
                new sys.DateTime(2012, 01, 22, 5, 43, 34, 434), 
                dtCtor1.Value);

            NullableDateTime dtCtor2 = new NullableDateTime(1980, 07, 28);
            nua.AssertEquals ("TestB#02", new sys.DateTime(1980, 7, 28),
                dtCtor2.Value);

            sys.DateTime temp = new sys.DateTime(5989, 2, 29, new sys.Globalization.HebrewCalendar());
            NullableDateTime dtCtor3 = new NullableDateTime(temp);
            nua.AssertEquals ("TestB#03", temp, dtCtor3.Value);
        }
        #endregion // Constructor Tests - B#

        #region INullable Tests - C#
        [nu.Test]
        public void IsNullProperty() {
            nua.Assert ("TestC#01", _null.IsNull);
            nua.Assert ("TestC#02", !_past.IsNull);
        }
        #endregion // INullable Tests - C#

        #region IComparable - Ordering Tests - D#
        [nu.Test]
        public void Compare() {
            NullableDateTime _bigFuture = _future + new sys.TimeSpan(3, 3, 3);
            nua.Assert("TestD#01", (((sys.IComparable)_future).CompareTo(_null) > 0));
            nua.Assert("TestD#02", (((sys.IComparable)_future).CompareTo(_past) > 0));
            nua.Assert("TestD#03", (((sys.IComparable)_future).CompareTo(_bigFuture) < 0));
            nua.Assert("TestD#04", (((sys.IComparable)_future).CompareTo(_future) == 0));

            nua.Assert("TestD#05", (((sys.IComparable)_null).CompareTo(_future) < 0));
            nua.Assert("TestD#06", (((sys.IComparable)_null).CompareTo(_past) < 0));
            nua.Assert("TestD#07", (((sys.IComparable)_null).CompareTo(NullableDateTime.Null) == 0));

            NullableDateTime _bigPast = _past - new sys.TimeSpan(23, 3, 45, 55, 545);
            nua.Assert("TestD#08", (((sys.IComparable)_past).CompareTo(_null) > 0));
            nua.Assert("TestD#09", (((sys.IComparable)_past).CompareTo(_future) < 0));
            nua.Assert("TestD#11", (((sys.IComparable)_past).CompareTo(_bigPast) > 0));
            nua.Assert("TestD#12", (((sys.IComparable)_past).CompareTo(_past) == 0));
        }

        [nu.Test]
        [nu.ExpectedException(typeof(sys.ArgumentException))]
        public void CompareToWrongType() {
            ((sys.IComparable)_null).CompareTo(1);
        }
        #endregion // IComparable - Ordering Tests - D#

        #region Property Tests - E#
        // Value property
        [nu.Test]
        public void ValueProperty() {
            sys.DateTime _testPast = new sys.DateTime(2000, 1, 1);
            nua.AssertEquals("TestE#01", _testPast, _past.Value);

            sys.DateTime _testFuture = new sys.DateTime(2025, 5, 5);
            nua.AssertEquals("TestE#02", _testFuture.Ticks, _future.Ticks);

            nua.Assert("TestE#03", _null.IsNull);
        }


        [nu.Test]
        [nu.ExpectedException(typeof(NullableTypes.NullableNullValueException))]
        public void ValuePropertyNull() {
            sys.DateTime dtVal = _null.Value;
        }
        #endregion // Property Tests - E#

        #region Equivalence Tests - F#
        [nu.Test]
        public void StaticEqualsAndEqualityOperator() {            
            // Case 1: either is NullableDateTime.Null
            nua.AssertEquals("TestF#01", NullableBoolean.Null, _null == _past);
            nua.AssertEquals("TestF#02", NullableBoolean.Null, NullableDateTime.Equals(_past, _null));
            nua.AssertEquals("TestF#03", NullableBoolean.Null, NullableDateTime.NotEquals(_future, _null));

            // Case 2: both are NullableDateTime.Null
            nua.AssertEquals("TestF#04", NullableBoolean.Null, _null == NullableDateTime.Null);
            nua.AssertEquals("TestF#05", NullableBoolean.Null, NullableDateTime.Equals(NullableDateTime.Null, _null));
            nua.AssertEquals("TestF#06", NullableBoolean.Null, NullableDateTime.NotEquals(NullableDateTime.Null, _null));

            // Case 3: both are equal
            NullableDateTime x = _future;
            nua.AssertEquals ("TestF#07", NullableBoolean.True, x == _future);
            nua.AssertEquals ("TestF#08", NullableBoolean.True, NullableDateTime.Equals(_future, x));
            nua.AssertEquals ("TestF#09", NullableBoolean.False, NullableDateTime.NotEquals(_future, x));

            // Case 4: inequality
            nua.AssertEquals ("TestF#10", NullableBoolean.False, _past == _future);
            nua.AssertEquals ("TestF#11", NullableBoolean.False, NullableDateTime.Equals(_past, _future));
            nua.AssertEquals ("TestF#12", NullableBoolean.True, NullableDateTime.NotEquals(_future, _past));
        }


        [nu.Test]
        public void Equals() {    
            // Case 1: either is NullableDateTime.Null
            nua.Assert("TestF#101", !_null.Equals(_past));
            nua.Assert("TestF#102", !_past.Equals(_null));

            // Case 2: both are NullableDateTime.Null
            nua.Assert("TestF#103", _null.Equals(NullableDateTime.Null));
            nua.Assert("TestF#104", NullableDateTime.Null.Equals(_null));

            // Case 3: both are equal
            NullableDateTime x = _future;
            nua.Assert("TestF#105", x.Equals(_future));
            nua.Assert("TestF#106", _future.Equals(x));

            // Case 4: inequality
            nua.Assert("TestF#107", !_future.Equals(_past));
            nua.Assert("TestF#108", !_past.Equals(_future));
        }
        #endregion // Equivalence Tests - F#

        #region Method Tests - G#
        [nu.Test]
        public void GreaterThan() {
            sys.DateTime dtx = new sys.DateTime(1944, 12, 11);
            sys.DateTime dty = new sys.DateTime(2111, 1, 21);
            
            NullableDateTime x = new NullableDateTime (dtx);
            NullableDateTime y = new NullableDateTime (dty);

            nua.Assert("TestG#01", (bool)NullableDateTime.GreaterThan(y, x));
        }


        [nu.Test]
        public void GreaterThanOrEqual() {
            sys.DateTime dtx = new sys.DateTime(1944, 12, 11);
            sys.DateTime dty = new sys.DateTime(2111, 1, 21);
            sys.DateTime dtz = new sys.DateTime(1944, 12, 11);
            
            NullableDateTime x = new NullableDateTime (dtx);
            NullableDateTime y = new NullableDateTime (dty);
            NullableDateTime z = new NullableDateTime (dtz);

            nua.Assert("TestG#02", (bool)NullableDateTime.GreaterThanOrEqual(y, x));
            nua.Assert("TestG#03", (bool)NullableDateTime.GreaterThanOrEqual(y, z));
            nua.Assert("TestG#04", (bool)NullableDateTime.GreaterThanOrEqual(z, x));
        }


        [nu.Test]
        public void LessThan() {
            sys.DateTime dtx = new sys.DateTime(1834, 4, 4);
            sys.DateTime dty = new sys.DateTime(1999, 08, 15);
            
            NullableDateTime x = new NullableDateTime(dtx);
            NullableDateTime y = new NullableDateTime(dty);

            nua.AssertEquals ("TestG#05", dtx < dty, NullableDateTime.LessThan(x,y).Value);
        }


        [nu.Test]
        public void LessThanOrEquals() {
            sys.DateTime dtx = new sys.DateTime(2014, 8, 13);
            sys.DateTime dty = new sys.DateTime(2111, 1, 21);
            sys.DateTime dtz = new sys.DateTime(2014, 8, 13);
            
            NullableDateTime x = new NullableDateTime (dtx);
            NullableDateTime y = new NullableDateTime (dty);
            NullableDateTime z = new NullableDateTime (dtz);

            nua.AssertEquals ("TestG#06", dtx <= dty, NullableDateTime.LessThanOrEqual(x,y).Value);
            nua.AssertEquals ("TestG#07", dtx <= dtz, NullableDateTime.LessThanOrEqual(x,z).Value);
        }


        [nu.Test]
        public void Parse() {
            string sx = "2005-07-27";            
            NullableDateTime x = NullableDateTime.Parse(sx);

            nua.AssertEquals ("TestG#130", sys.Convert.ToDateTime(sx), 
                NullableDateTime.Parse(sx).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ParseNull() {            
            NullableDateTime temp = NullableDateTime.Parse(null);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ParseFormatException() {
            string sx = "409'85";            
            NullableDateTime.Parse(sx);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void OverflowException() {
            string sx = "9" + sys.DateTime.MaxValue.ToString("yyyy-MMM-dd");            
            NullableDateTime.Parse(sx);
        }
        #endregion // Method Tests - G#
        
        #region Operator Tests - H#
        [nu.Test]
        public void AddOperator() {
            sys.DateTime dtx = new sys.DateTime(1978, 8, 13);
            sys.TimeSpan dtdx = new sys.TimeSpan(12, 0, 0, 0);
            sys.DateTime dty = dtx + dtdx;

            NullableDateTime nx = new NullableDateTime(dtx);
            NullableDateTime ny = new NullableDateTime(dty);
            
            // Add nullable datetimes
            nua.AssertEquals("TestH#01", (dtx + dtdx), (nx + dtdx).Value);
            nua.AssertEquals("TestH#02", dtx, (nx + new sys.TimeSpan(0, 0, 0, 0)).Value);

            // Addition Nulls
            nua.Assert("TestH#02-a", (_null + dtdx).IsNull);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentOutOfRangeException))]
        public void AddOperatorOverflow() {
            sys.DateTime dtx = sys.DateTime.MaxValue;
            sys.TimeSpan dx = new sys.TimeSpan(12, 2, 3, 4);
            
            NullableDateTime nx = new NullableDateTime (dtx);
            
            NullableDateTime nz = nx + dx;
        }

        [nu.Test]
        public void GreaterThanOperator() {
            sys.TimeSpan dx = new sys.TimeSpan(1, 1, 1, 1);
            NullableDateTime _bigFuture = _future + dx;
            NullableDateTime _bigPast = _past - dx;

            // GreaterThan nulls
            nua.Assert("TestH#03", (_future > _null).IsNull);
            nua.Assert("TestH#04", (_null > _past).IsNull);
            nua.Assert("TestH#05", (_null > NullableDateTime.Null).IsNull);

            // GreaterThan nullable ints
            nua.Assert("TestH#06", (_future > _past).IsTrue);
            nua.Assert("TestH#07", (_future > _bigFuture).IsFalse);
            nua.Assert("TestH#08", (_future > _future).IsFalse);
            nua.Assert("TestH#09", (_past > _bigPast).IsTrue);
            nua.Assert("TestH#10", (_past > _past).IsFalse);

            // GreaterThan ints
            nua.Assert("TestH#11", (_future > _past.Value).IsTrue);
            nua.Assert("TestH#12", (_future > _bigFuture.Value).IsFalse);
            nua.Assert("TestH#13", (_future > _future.Value).IsFalse);
            nua.Assert("TestH#14", (_past > _bigPast.Value).IsTrue);
            nua.Assert("TestH#15", (_past > _past.Value).IsFalse);
        }


        [nu.Test]
        public void GreaterThanOrEqualsOperator() {
            sys.TimeSpan dx = new sys.TimeSpan(1, 1, 1, 1);
            NullableDateTime _bigFuture = _future + dx;
            NullableDateTime _bigPast = _past - dx;

            // GreaterThanOrEquals nulls
            nua.Assert("TestH#16", (_future >= _null).IsNull);
            nua.Assert("TestH#17", (_null >= _past).IsNull);
            nua.Assert("TestH#18", (_null >= NullableDateTime.Null).IsNull);

            // GreaterThanOrEquals nullable ints
            nua.Assert("TestH#19", (_future >= _past).IsTrue);
            nua.Assert("TestH#20", (_future >= _bigFuture).IsFalse);
            nua.Assert("TestH#21", (_future >= _future).IsTrue);
            nua.Assert("TestH#23", (_past >= _bigPast).IsTrue);
            nua.Assert("TestH#24", (_past >= _past).IsTrue);

            // GreaterThanOrEquals ints
            nua.Assert("TestH#25", (_future >= _past.Value).IsTrue);
            nua.Assert("TestH#26", (_future >= _bigFuture.Value).IsFalse);
            nua.Assert("TestH#27", (_future >= _future.Value).IsTrue);
            nua.Assert("TestH#29", (_past >= _bigPast.Value).IsTrue);
            nua.Assert("TestH#30", (_past >= _past.Value).IsTrue);
        }


        [nu.Test]
        public void LessThanOperator() {
            sys.TimeSpan dx = new sys.TimeSpan(0, 1, 1, 1);
            NullableDateTime _bigFuture = _future + dx;
            NullableDateTime _bigPast = _past - dx;

            // LessThan nulls
            nua.Assert("TestH#31", (_future < _null).IsNull);
            nua.Assert("TestH#32", (_null < _past).IsNull);
            nua.Assert("TestH#33", (_null < NullableDateTime.Null).IsNull);

            // LessThan nullable ints
            nua.Assert("TestH#34", (_future < _past).IsFalse);
            nua.Assert("TestH#35", (_future < _bigFuture).IsTrue);
            nua.Assert("TestH#36", (_future < _future).IsFalse);
            nua.Assert("TestH#37", (_past < _bigPast).IsFalse);
            nua.Assert("TestH#38", (_past < _past).IsFalse);

            // LessThan ints
            nua.Assert("TestH#39", (_future < _past.Value).IsFalse);
            nua.Assert("TestH#40", (_future < _bigFuture.Value).IsTrue);
            nua.Assert("TestH#41", (_future < _future.Value).IsFalse);
            nua.Assert("TestH#42", (_past < _bigPast.Value).IsFalse);
            nua.Assert("TestH#43", (_past < _past.Value).IsFalse);
        }


        [nu.Test]
        public void LessThanOrEqualsOperator() {
            sys.TimeSpan dx = new sys.TimeSpan(0, 1, 1, 1);
            NullableDateTime _bigFuture = _future + dx;
            NullableDateTime _bigPast = _past - dx;

            // LessThanOrEquals nulls
            nua.Assert("TestH#44", (_future <= _null).IsNull);
            nua.Assert("TestH#45", (_null <= _past).IsNull);
            nua.Assert("TestH#46", (_null <= NullableDateTime.Null).IsNull);

            // LessThanOrEquals nullable ints
            nua.Assert("TestH#47", (_future <= _past).IsFalse);
            nua.Assert("TestH#48", (_future <= _bigFuture).IsTrue);
            nua.Assert("TestH#49", (_future <= _future).IsTrue);
            nua.Assert("TestH#50", (_past <= _bigPast).IsFalse);
            nua.Assert("TestH#51", (_past <= _past).IsTrue);

            // LessThanOrEquals ints
            nua.Assert("TestH#52", (_future <= _past.Value).IsFalse);
            nua.Assert("TestH#53", (_future <= _bigFuture.Value).IsTrue);
            nua.Assert("TestH#54", (_future <= _future.Value).IsTrue);
            nua.Assert("TestH#55", (_past <= _bigPast.Value).IsFalse);
            nua.Assert("TestH#56", (_past <= _past.Value).IsTrue);
        }


        [nu.Test]
        public void SubtractionOperator() {
            sys.DateTime dtx = new sys.DateTime(1978, 8, 13);
            sys.TimeSpan dtdx = new sys.TimeSpan(12, 0, 0, 0);
            sys.DateTime dty = dtx - dtdx;
            
            NullableDateTime nx = new NullableDateTime(dtx);
            NullableDateTime ny = new NullableDateTime(dty);
            
            // Subtraction nullable ints
            nua.AssertEquals("TestH#57", (dtx - dtdx), (nx - dtdx).Value);
            nua.AssertEquals("TestH#58", dtx, (nx - new sys.TimeSpan(0, 0, 0, 0)).Value);

            // Subtraction Nulls
            nua.Assert("TestH#59", (_null - dtdx).IsNull);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentOutOfRangeException))]
        public void SubtractionOperatorOverflow() {
            sys.DateTime dtx = sys.DateTime.MinValue;
            sys.TimeSpan dx = new sys.TimeSpan(1, 0, 1, 1);
            
            NullableDateTime nx = new NullableDateTime(dtx);
            
            NullableDateTime nz = nx - dx;
        }
        #endregion // Operators Tests - H#

        #region Conversion Operators Tests - I#
        [nu.Test]
        public void DateTimeConversionOperator() {
            sys.DateTime x = new sys.DateTime(2002, 2, 22);
            NullableDateTime nx = new NullableDateTime(x);

            nua.AssertEquals("TestI#01", x, (sys.DateTime)nx);
        }


        [nu.Test]
        public void NullableDateTimeConversionOperatorFromNullableString() {
            NullableString x = new NullableString("2002-2-22");
            sys.DateTime tx = new sys.DateTime(2002, 2, 22);

            nua.AssertEquals("TestI#02", new NullableDateTime(tx), 
                (NullableDateTime)x);
        }


        [nu.Test]
        public void NullableDateTimeConversionOperatorFromDateTime() {
            sys.DateTime dtx = new sys.DateTime(2010, 12, 12);
            NullableDateTime nx = new NullableDateTime(dtx);

            nua.AssertEquals("TestI#03", nx, (NullableDateTime)dtx);
        }
        #endregion // Conversion Operator Tests - I#

        #region Serialization - K#
        [nu.Test]
        public void SerializableAttribute() {
            NullableDateTime serializedDeserialized;

            serializedDeserialized = SerializeDeserialize(_null);
            nua.Assert("TestK#01", serializedDeserialized.IsNull);
            nua.Assert("TestK#02", _null.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_past);
            nua.Assert("TestK#03", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#04", _past.Value, serializedDeserialized.Value);
            nua.Assert("TestK#05", _past.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_min);
            nua.Assert("TestK#06", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#07", _min.Value, serializedDeserialized.Value);
            nua.Assert("TestK#08", _min.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_max);
            nua.Assert("TestK#09", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#10", _max.Value, serializedDeserialized.Value);
            nua.Assert("TestK#11", _max.Equals(serializedDeserialized));
        }

        private NullableDateTime SerializeDeserialize(NullableDateTime x) {
            System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer = 
                new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableDateTime y = (NullableDateTime)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }


        [nu.Test]
        public void XmlSerializable() {
            NullableDateTime xmlSerializedDeserialized;

            xmlSerializedDeserialized = XmlSerializeDeserialize(_null);
            nua.Assert("TestK#20", xmlSerializedDeserialized.IsNull);
            nua.Assert("TestK#21", _null.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_past);
            nua.Assert("TestK#22", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#23", _past.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#24", _past.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_min);
            nua.Assert("TestK#25", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#26", _min.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#27", _min.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_max);
            nua.Assert("TestK#28", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#29", _max.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#30", _max.Equals(xmlSerializedDeserialized));
        }

        private NullableDateTime XmlSerializeDeserialize(NullableDateTime x) {
            System.Xml.Serialization.XmlSerializer serializer = 
                new System.Xml.Serialization.XmlSerializer(typeof(NullableDateTime));
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableDateTime y = (NullableDateTime)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }


        [nu.Test]
        public void XmlSerializableEmptyElementNil() {
            // Bug reported by Shaun Bowe (sbowe@users.sourceforge.net)
            // http://sourceforge.net/forum/message.php?msg_id=2399265

            //<?xml version="1.0"?>
            //<NullableDateTime xsi:nil="true" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"></NullableDateTime>

            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableDateTime));

            using (sys.IO.MemoryStream baseStream = new sys.IO.MemoryStream()) {
                using (sys.IO.StreamWriter stream = new System.IO.StreamWriter(baseStream)) {
                    stream.WriteLine("<?xml version=\"1.0\"?>");
                    stream.WriteLine("<NullableDateTime xsi:nil=\"true\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></NullableDateTime>");
                    stream.Flush();

//                    baseStream.Position = 0;
//                    sys.IO.StreamReader streamReader = new System.IO.StreamReader(baseStream);
//                    sys.Console.WriteLine(streamReader.ReadToEnd());
                    
                    baseStream.Position = 0; // Return stream to start
                    NullableDateTime y = (NullableDateTime)serializer.Deserialize(baseStream);

                    nua.Assert(y.IsNull);

                    baseStream.Close();
                    stream.Close();
                }
            }
        }

        [nu.Test]
        public void XmlSerializableSchema() {
            sysXmlScm.XmlSchema xsd = 
                ((sysXml.Serialization.IXmlSerializable)NullableDateTime.Null).GetSchema();

            xsd.Compile(new sysXmlScm.ValidationEventHandler(ValidationCallBack));

            ValidateXmlAgainstXmlSchema(xsd, _null);
            ValidateXmlAgainstXmlSchema(xsd, _min);
            ValidateXmlAgainstXmlSchema(xsd, _max);

        }

        private void ValidateXmlAgainstXmlSchema(sysXmlScm.XmlSchema xsd, NullableDateTime x) {
            sysXml.Serialization.XmlSerializer serializer = 
                new sysXml.Serialization.XmlSerializer(typeof(NullableDateTime));

                
            sys.IO.MemoryStream stream = null;
            sys.Xml.XmlValidatingReader validator = null;
            try {
                // Get the serialized NullableDateTime instance
                stream = new sys.IO.MemoryStream();
                serializer.Serialize(stream, x);
                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start

                // Add the default namespace
                sysXml.XmlDocument doc = new sysXml.XmlDocument();
                doc.Load(stream);                    
                sysXml.XmlAttribute defaultNs = doc.CreateAttribute("xmlns");
                defaultNs.Value = "http://NullableTypes.SourceForge.Net/NullableDateTimeXMLSchema";
                doc.DocumentElement.Attributes.Append(defaultNs);

                // Validate
                validator = new sysXml.XmlValidatingReader(doc.OuterXml, sysXml.XmlNodeType.Document, null);
                validator.ValidationType = sys.Xml.ValidationType.Schema;                    
                validator.Schemas.Add(xsd);
                validator.ValidationEventHandler += new sys.Xml.Schema.ValidationEventHandler(ValidationCallBack);
                while(validator.Read());

            }
            finally {
                if (validator != null)
                    validator.Close();

                if (stream != null)
                    ((sys.IDisposable)stream).Dispose();
            }
        }

        private static void ValidationCallBack(object sender, sysXmlScm.ValidationEventArgs args)    {
            throw args.Exception;
        }

        #endregion // Serialization
    }
}