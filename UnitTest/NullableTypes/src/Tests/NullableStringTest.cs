//
// NullableTypes.Tests.NullableStringTest2
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 24-May-2003  Luca    Create
// 23-Aug-2003  Luca    Upgrade    Code upgrade: Replaced new CultureInfo("") with equivalent 
//                                 CultureInfo.InvariantCulture
// 13-Sep-2003  Luca    Upgrade    New test: SerializableAttribute
// 15-Sep-2003  Luca    Upgrade    Code upgrade: Improved test CompareTo to make it independent 
//                                 from user regional settings 
// 05-Oct-2003  DamienG Upgrade    New test: XmlSerializable
//                                 Code upgrade: Tidy up source
// 06-Ott-2003  Luca    Upgrade    New test: XmlSerializableSchema
//                                 Code upgrade: Replaced tabs with spaces and removed commented out code
// 06-Dic-2003  Luca    Bug Fix    Replaced Target Namespace for Xml Schema to reflect changes in the target type
// 18-Feb-2004  Luca    Upgrade    New test: XmlSerializableEmptyElementNil for xml deserialization of a nil 
//                                 value with a non empty element
//


namespace NullableTypes.Tests {

    using nu = NUnit.Framework;
    using nua = NUnit.Framework.Assertion;
    using sys = System;
    using sysThr = System.Threading;
    using sysGlb = System.Globalization;
    using sysXml = System.Xml;
    using sysXmlScm = System.Xml.Schema;

    [nu.TestFixture]
    public class NullableStringTest2 {

        private NullableString Test1;
        private NullableString Test2;
        private NullableString Test3;

        [nu.SetUp]
        public void SetUp() {
            Test1 = new NullableString("First TestString");
            Test2 = new NullableString("This is just a test NullableString");
            Test3 = new NullableString("This is just another test NullableString");
            //sysThr.Thread.CurrentThread.CurrentCulture = new sysGlb.CultureInfo("en-AU");
        }


        // Test constructor
        [nu.Test]
        public void Create() {
            
            // NullableString (String)
            NullableString  TestString = new NullableString ("Test");
            nua.AssertEquals("#A01", "Test", TestString.Value);

            char[] x = new char[]{'G', 'i', 'n', 'o'};
            TestString = new NullableString(x, 0, 4);
            nua.AssertEquals("#A02", "Gino", TestString.Value);

            TestString = new NullableString(x, 3, 1);
            nua.AssertEquals("#A03", "o", TestString.Value);

            TestString = new NullableString(x, 0, 0);
            nua.AssertEquals("#A04", string.Empty, TestString.Value);

            TestString = new NullableString(x, 3, 0);
            nua.AssertEquals("#A05", string.Empty, TestString.Value);

            string s = new string(new char[0], 0, 0);
            nua.AssertEquals("#A06", string.Empty, s);
            TestString = new NullableString(new char[0], 0, 0);
            nua.AssertEquals("#A06", string.Empty, TestString.Value);
            
            
            // NullableString (String, int)
            //TestString = new NullableString ("Test", 2057);
            //nua.AssertEquals("#A02", 2057, TestString.LCID);

            // NullableString (int, NullableCompareOptions, byte[])
            //            TestString = new NullableString (2057,
            //                NullableCompareOptions.BinarySort|NullableCompareOptions.IgnoreCase,
            //                new byte [2] {123, 221});
            //            nua.AssertEquals("#A03", 2057, TestString.CompareInfo.LCID);
                        
            // NullableString(string, int, NullableCompareOptions)
            //            TestString = new NullableString ("Test", 2057, NullableCompareOptions.IgnoreNonSpace);
            //            nua.Assert("#A04", !TestString.IsNull);
                        
            // NullableString (int, NullableCompareOptions, byte[], bool)
            //            TestString = new NullableString (2057, NullableCompareOptions.BinarySort, new byte [4] {100, 100, 200, 45}, true);
            //            nua.AssertEquals("#A05", (byte)63, TestString.GetNonUnicodeBytes () [0]);
            //            TestString = new NullableString (2057, NullableCompareOptions.BinarySort, new byte [2] {113, 100}, false);
            //            nua.AssertEquals("#A06", (String)"qd", TestString.Value);
                        
            // NullableString (int, NullableCompareOptions, byte[], int, int)
            //            TestString = new NullableString (2057, NullableCompareOptions.BinarySort, new byte [2] {113, 100}, 0, 2);
            //            nua.Assert("#A07", !TestString.IsNull);

            //            try {
            //                TestString = new NullableString (2057, NullableCompareOptions.BinarySort, new byte [2] {113, 100}, 2, 1);
            //                nua.Fail ("#A07b");
            //            } catch (sys.Exception e) {
            //                nua.AssertEquals("#A07c", typeof (ArgumentOutOfRangeException), e.GetType ());
            //            }

            //            try {
            //                TestString = new NullableString (2057, NullableCompareOptions.BinarySort, new byte [2] {113, 100}, 0, 4);
            //                nua.Fail ("#A07d");
            //            } catch (sys.Exception e) {
            //                nua.AssertEquals("#A07e", typeof (ArgumentOutOfRangeException), e.GetType ());
            //            }

            // NullableString (int, NullableCompareOptions, byte[], int, int, bool)
            //            TestString = new NullableString (2057, NullableCompareOptions.IgnoreCase, new byte [3] {100, 111, 50}, 1, 2, false);
            //            nua.AssertEquals("#A08", "o2", TestString.Value);
            //            TestString = new NullableString (2057, NullableCompareOptions.IgnoreCase, new byte [3] {123, 111, 222}, 1, 2, true);
            //            nua.Assert("#A09", !TestString.IsNull);                        
        }


        [nu.Test, nu.ExpectedException(typeof(System.ArgumentNullException))]
        public void CreateArgumentNullException() {
            char[] x = null;
            NullableString s = new NullableString (x, 0, 0);
        }


        [nu.Test, nu.ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CreateArgumentOutOfRangeException1() {
            char[] x = new char[]{'G', 'i', 'n', 'o'};
            NullableString TestString = new NullableString(x, 0, 5);
        }


        [nu.Test, nu.ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CreateArgumentOutOfRangeException2() {
            char[] x = new char[]{'G', 'i', 'n', 'o'};
            NullableString TestString = new NullableString(x, 3, 2);
        }


        [nu.Test, nu.ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CreateArgumentOutOfRangeException3() {
            char[] x = new char[]{'G', 'i', 'n', 'o'};
            NullableString TestString = new NullableString(x, 4, 0);
        }


        [nu.Test, nu.ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CreateArgumentOutOfRangeException4() {
            char[] x = new char[]{'G', 'i', 'n', 'o'};
            NullableString TestString = new NullableString(x, 3, 2);
        }


        [nu.Test, nu.ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CreateArgumentOutOfRangeException5() {
            char[] x = new char[]{'G', 'i', 'n', 'o'};
            NullableString TestString = new NullableString(x, 2, -1);
        }


        [nu.Test, nu.ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CreateArgumentOutOfRangeException6() {
            char[] x = new char[]{'G', 'i', 'n', 'o'};
            NullableString TestString = new NullableString(x, -1, 0);
        }


        // Test public fields
        [nu.Test]
        public void PublicFields() {
            //            // BinarySort
            //            nua.AssertEquals("#B01", 32768, NullableString.BinarySort);
            //                        
            //            // IgnoreCase
            //            nua.AssertEquals("#B02", 1, NullableString.IgnoreCase);
            //                                      
            //            // IgnoreKanaType
            //            nua.AssertEquals("#B03", 8, NullableString.IgnoreKanaType);
            //
            //            // IgnoreNonSpace
            //            nua.AssertEquals("#B04", 2, NullableString.IgnoreNonSpace);
            //                        
            //            // IgnoreWidth
            //            nua.AssertEquals("#B05", 16, NullableString.IgnoreWidth);
                        
            // Null
            nua.Assert("#B06", NullableString.Null.IsNull);
        }


        // Test properties
        [nu.Test]
        public void Properties() {
            //            // CompareInfo
            //            nua.AssertEquals("#C01", 3081, Test1.CompareInfo.LCID);
            //
            //            // CultureInfo
            //            nua.AssertEquals("#C02", 3081, Test1.CultureInfo.LCID);                
                        
            // IsNull
            nua.Assert("#C03", !Test1.IsNull);
            nua.Assert("#C04", NullableString.Null.IsNull);
                        
            //            // LCID
            //            nua.AssertEquals("#C05", 3081, Test1.LCID);
            //                        
            //            // NullableCompareOptions
            //            nua.AssertEquals("#C06", "IgnoreCase, IgnoreKanaType, IgnoreWidth", 
            //                Test1.NullableCompareOptions.ToString ());

            // Value
            nua.AssertEquals("#C07", "First TestString", Test1.Value);
        }


        // PUBLIC METHODS

        [nu.Test]
        public void CompareTo() {

            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = new sysGlb.CultureInfo("it-IT", false);

                NullableString _null = NullableString.Null;
                NullableString _zz  = new NullableString("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
                NullableString _kk  = new NullableString("KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK");
                NullableString _aa  = new NullableString("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                NullableString _zzzz = _zz + _zz;
                NullableString _a = new NullableString("aaaaaaaaaaaaaaaaaaa");

                nua.Assert("TestD#01", (((sys.IComparable)_zz).CompareTo(_null) > 0));
                nua.Assert("TestD#02", (((sys.IComparable)_zz).CompareTo(_aa) > 0));
                nua.Assert("TestD#03", (((sys.IComparable)_zz).CompareTo(_zzzz) < 0));
                nua.Assert("TestD#04", (((sys.IComparable)_zz).CompareTo(_zz) == 0));

                nua.Assert("TestD#05", (((sys.IComparable)_null).CompareTo(_zz) < 0));
                nua.Assert("TestD#06", (((sys.IComparable)_null).CompareTo(_aa) < 0));
                nua.Assert("TestD#07", (((sys.IComparable)_null).CompareTo(NullableString.Null) == 0));

                nua.Assert("TestD#08", (((sys.IComparable)_aa).CompareTo(_null) > 0));
                nua.Assert("TestD#09", (((sys.IComparable)_aa).CompareTo(_kk) < 0));
                nua.Assert("TestD#11", (((sys.IComparable)_aa).CompareTo(_a) > 0));
                nua.Assert("TestD#12", (((sys.IComparable)_aa).CompareTo(_aa) == 0));
            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

        }


        [nu.Test]
        [nu.ExpectedException(typeof(sys.ArgumentException))]
        public void CompareToWrongType() {
            ((sys.IComparable)NullableString.Empty).CompareTo(1);
        }

        //        public void CompareTo() {
        //            NullableByte Test = new NullableByte (1);
        //
        //            nua.Assert("#D01", Test1.CompareTo (Test3) < 0);
        //            nua.Assert("#D02", Test2.CompareTo (Test1) > 0);
        //            nua.Assert("#D03", Test2.CompareTo (Test3) == 0);
        //            nua.Assert("#D04", Test3.CompareTo (NullableString.Null) > 0);
        //
        //            try {
        //                Test1.CompareTo (Test);
        //                nua.Fail("#D05");
        //            } catch(sys.Exception e) {                        
        //                nua.AssertEquals("#D06", typeof (sys.ArgumentException), e.GetType ());
        //            }
        //                        
        //            NullableString T1 = new NullableString ("test", 2057, NullableCompareOptions.IgnoreCase);
        //            NullableString T2 = new NullableString ("TEST", 2057, NullableCompareOptions.None);
                
        //            try {
        //                T1.CompareTo (T2);
        //                nua.Fail ("#D07");
        //            } catch (sys.Exception e) {
        //                nua.AssertEquals("#D08", typeof (NullableTypeException), e.GetType ());
        //            }
                
        //            // IgnoreCase
        //            T1 = new NullableString ("test", 2057, NullableCompareOptions.IgnoreCase);
        //            T2 = new NullableString ("TEST", 2057, NullableCompareOptions.IgnoreCase);
        //            nua.Assert("#D09", T2.CompareTo (T1) == 0);
        //                
        //            T1 = new NullableString ("test", 2057);
        //            T2 = new NullableString ("TEST", 2057);
        //            nua.Assert("#D10", T2.CompareTo (T1) == 0);
        //
        //            T1 = new NullableString ("test", 2057, NullableCompareOptions.None);
        //            T2 = new NullableString ("TEST", 2057, NullableCompareOptions.None);
        //            nua.Assert("#D11", T2.CompareTo (T1) != 0);
        //
        //            // IgnoreNonSpace
        //            T1 = new NullableString ("TESTñ", 2057, NullableCompareOptions.IgnoreNonSpace);
        //            T2 = new NullableString ("TESTn", 2057, NullableCompareOptions.IgnoreNonSpace);
        //            nua.Assert("#D12", T2.CompareTo (T1) == 0);
        //                
        //            T1 = new NullableString ("TESTñ", 2057, NullableCompareOptions.None);
        //            T2 = new NullableString ("TESTn", 2057, NullableCompareOptions.None);
        //            nua.Assert("#D13", T2.CompareTo (T1) != 0);
        //
        //            // BinarySort
        //            T1 = new NullableString ("01_", 2057, NullableCompareOptions.BinarySort);
        //            T2 = new NullableString ("_01", 2057, NullableCompareOptions.BinarySort);
        //            nua.Assert("#D14", T1.CompareTo (T2) < 0);
        //                    
        //            T1 = new NullableString ("01_", 2057, NullableCompareOptions.None);
        //            T2 = new NullableString ("_01", 2057, NullableCompareOptions.None);
        //            nua.Assert("#D15", T1.CompareTo (T2) > 0);            
        //        }


        [nu.Test]
        public void EqualsMethods() {
            NullableString t2 = new NullableString(Test2.Value);
            nua.Assert("#E01", !Test1.Equals(Test2));
            nua.Assert("#E02", !Test3.Equals (Test1));
            nua.Assert("#E03", !Test2.Equals (new NullableString ("TEST")));
            nua.Assert("#E04", Test2.Equals(t2));

            // Static Equals()-method
            nua.Assert("#E05", NullableString.Equals(Test2, t2).Value);
            nua.Assert("#E06", !NullableString.Equals (Test1, Test3).Value);
        }


        [nu.Test]
        public void GetHashCodeTest() {
            // FIXME: Better way to test HashCode
            nua.AssertEquals("#F01", Test1.GetHashCode (), 
                Test1.GetHashCode ());
            nua.Assert("#F02", Test1.GetHashCode () != Test2.GetHashCode ());
            nua.Assert("#F03", Test2.GetHashCode () == Test2.GetHashCode ());
        }


        [nu.Test]
        public void GetTypeTest() {
            nua.AssertEquals("#G01", "NullableTypes.NullableString", 
                Test1.GetType ().ToString ());
            nua.AssertEquals("#G02", "System.String", 
                Test1.Value.GetType ().ToString ());
        }


        [nu.Test]
        public void NotEquals() {
            NullableString t2 = new NullableString(Test2.Value);

            nua.Assert("#J01", NullableString.NotEquals (Test1, Test2).Value);
            nua.Assert("#J02", NullableString.NotEquals (Test2, Test1).Value);
            nua.Assert("#J03", NullableString.NotEquals (Test3, Test1).Value);
            nua.Assert("#J04", !NullableString.NotEquals (Test2, t2).Value);

            nua.Assert("#J05", NullableString.NotEquals (NullableString.Null, Test3).IsNull);
        }


        [nu.Test]
        public void Concat() {
            Test1 = new NullableString ("First TestString");
            Test2 = new NullableString ("This is just a test NullableString");
            Test3 = new NullableString ("This is just a test NullableString");

            nua.AssertEquals("#K01", 
                (NullableString)"First TestStringThis is just a test NullableString", 
                NullableString.Concat (Test1, Test2));

            nua.AssertEquals("#K02", NullableString.Null, 
                NullableString.Concat (Test1, NullableString.Null));
        }


        [nu.Test]
        public void Clone() {
            NullableString TestNullableString  = Test1.Clone ();
            nua.AssertEquals("#L01", Test1, TestNullableString);
        }


        [nu.Test]
        public void CompareOptionsFromNullableCompareOptions() {
            //            nua.AssertEquals("#M01", CompareOptions.IgnoreCase,
            //                NullableString.CompareOptionsFromNullableCompareOptions (
            //                NullableCompareOptions.IgnoreCase));
            //            nua.AssertEquals("#M02", CompareOptions.IgnoreCase,
            //                NullableString.CompareOptionsFromNullableCompareOptions (
            //                NullableCompareOptions.IgnoreCase));
            //            try {
            //                                
            //                CompareOptions test = NullableString.CompareOptionsFromNullableCompareOptions (
            //                    NullableCompareOptions.BinarySort);
            //                nua.Fail ("#M03");
            //            } catch (sys.Exception e) {
            //                nua.AssertEquals("#M04", typeof (ArgumentOutOfRangeException), e.GetType ());
            //            }
        }


        [nu.Test]
        public void UnicodeBytes() {
            //            nua.AssertEquals("#N01", (byte)105, Test1.GetNonUnicodeBytes () [1]);
            //            nua.AssertEquals("#N02", (byte)32, Test1.GetNonUnicodeBytes () [5]);
            //
            //            nua.AssertEquals("#N03", (byte)70, Test1.GetUnicodeBytes () [0]);
            //            nua.AssertEquals("#N03b", (byte)70, Test1.GetNonUnicodeBytes () [0]);
            //            nua.AssertEquals("#N03c", (byte)0, Test1.GetUnicodeBytes () [1]);
            //            nua.AssertEquals("#N03d", (byte)105, Test1.GetNonUnicodeBytes () [1]);
            //            nua.AssertEquals("#N03e", (byte)105, Test1.GetUnicodeBytes () [2]);
            //            nua.AssertEquals("#N03f", (byte)114, Test1.GetNonUnicodeBytes () [2]);
            //            nua.AssertEquals("#N03g", (byte)0, Test1.GetUnicodeBytes () [3]);
            //            nua.AssertEquals("#N03h", (byte)115, Test1.GetNonUnicodeBytes () [3]);
            //            nua.AssertEquals("#N03i", (byte)114, Test1.GetUnicodeBytes () [4]);
            //            nua.AssertEquals("#N03j", (byte)116, Test1.GetNonUnicodeBytes () [4]);
            //
            //            nua.AssertEquals("#N04", (byte)105, Test1.GetUnicodeBytes () [2]);
            //
            //            try {
            //                byte test = Test1.GetUnicodeBytes () [105];
            //                nua.Fail ("#N05");
            //            } catch (sys.Exception e) {
            //                nua.AssertEquals("#N06", typeof (IndexOutOfRangeException), e.GetType());                                
            //            }
        }
                      

        [nu.Test]          
        public void Conversions() {

            NullableString String250 = new NullableString ("250");
            NullableString String9E300 = new NullableString ("9E+300");

            // ToNullableBoolean ()        
            try {
                bool test = Test1.ToNullableBoolean ().Value;                              
                nua.Fail ("#01");
            } catch (sys.Exception e) {
                nua.AssertEquals("#01.5", typeof (sys.FormatException), e.GetType());                                                                
            }
                        
            nua.Assert("#O02", (new NullableString("1")).ToNullableBoolean ().Value);
            nua.Assert("#O03", !(new NullableString("0")).ToNullableBoolean ().Value);
            nua.Assert("#O04", (new NullableString("True")).ToNullableBoolean ().Value);
            nua.Assert("#O05", !(new NullableString("FALSE")).ToNullableBoolean ().Value);
            nua.Assert("#O06", NullableString.Null.ToNullableBoolean ().IsNull);

            // ToNullableByte ()
            try {
                byte test = Test1.ToNullableByte ().Value;
                nua.Fail ("#07");
            } catch (sys.Exception e) {
                nua.AssertEquals("#O07.5", typeof (sys.FormatException), e.GetType());    
            }

            nua.AssertEquals("#O08", (byte)250, String250.ToNullableByte ().Value);    
            try {
                NullableByte b = (byte)(new NullableString ("2500")).ToNullableByte ();
                nua.Fail ("#O09");
            } catch (sys.Exception e) {
                nua.AssertEquals("#O10", typeof (sys.OverflowException), e.GetType ());
            }

            // ToNullableDateTime
            nua.AssertEquals("#O11", 10, 
                (new NullableString ("2002-10-10")).ToNullableDateTime ().Value.Day);
                        

            // ToNullableDouble
            nua.AssertEquals("#O19", (NullableDouble)9E+300, String9E300.ToNullableDouble ());

            try {
                NullableDouble test = (new NullableString ("4e400")).ToNullableDouble ();
                nua.Fail ("#O20");
            } catch (sys.Exception e) {
                nua.AssertEquals("#O21", typeof (sys.OverflowException), e.GetType ());
            }

                        
            // ToNullableInt16 ()
            nua.AssertEquals("#O24", (short)250, String250.ToNullableInt16 ().Value);

            try {
                NullableInt16 test = String9E300.ToNullableInt16().Value;
                nua.Fail ("#O25");
            } catch (sys.Exception e) {
                nua.AssertEquals("#O26", typeof (sys.FormatException), e.GetType ());
            }        

            // ToNullableInt32 ()
            nua.AssertEquals("#O27", (int)250, String250.ToNullableInt32 ().Value);

            try {
                NullableInt32 test = String9E300.ToNullableInt32 ().Value;
                nua.Fail ("#O28");
            } catch (sys.Exception e) { 
                nua.AssertEquals("#O29", typeof (sys.FormatException), e.GetType ());
            }

            try {
                NullableInt32 test = Test1.ToNullableInt32 ().Value;
                nua.Fail ("#O30");
            } catch (sys.Exception e) { 
                nua.AssertEquals("#O31", typeof (sys.FormatException), e.GetType ());
            }

            // ToNullableInt64 ()
            nua.AssertEquals("#O32", (long)250, String250.ToNullableInt64 ().Value);

            try {        
                NullableInt64 test = String9E300.ToNullableInt64 ().Value;
                nua.Fail ("#O33");
            } catch (sys.Exception e) {
                nua.AssertEquals("#O34", typeof (sys.FormatException), e.GetType ());
            }        


            // ToNullableSingle ()
            nua.AssertEquals("#O38", (float)250, String250.ToNullableSingle ().Value);

            try {
                NullableSingle test = String9E300.ToNullableSingle().Value;
                nua.Fail ("#O39");
            } catch (sys.Exception e) {
                nua.AssertEquals("#O40", typeof (sys.OverflowException), e.GetType ());
            }        

            // ToString ()
            nua.AssertEquals("#O41", "First TestString", Test1.ToString ());

            // ToNullableDecimal ()
            try {
                nua.AssertEquals("#O13", (decimal)250, Test1.ToNullableDecimal ().Value);
                nua.Fail ("#O14");
            } catch (sys.Exception e) {
                nua.AssertEquals("#O15", typeof (sys.FormatException), e.GetType ());
            }

            nua.AssertEquals("#O16", (decimal)250, String250.ToNullableDecimal ().Value);

            try {
                NullableDecimal test = String9E300.ToNullableDecimal ().Value;
                nua.Fail ("#O17");
            } catch (sys.Exception e) {
                nua.AssertEquals("#O18", typeof (sys.FormatException), e.GetType ());
            }      

        }


        // OPERATORS

        [nu.Test]
        public void ArithmeticOperators() {
            NullableString TestString = new NullableString ("...Testing...");
            nua.AssertEquals("#P01", (NullableString)"First TestString...Testing...",
                Test1 + TestString);
            nua.AssertEquals("#P02", NullableString.Null,
                Test1 + NullableString.Null);
        }


        [nu.Test]
        public void ThanOrEqualOperators() {
            NullableString t2 = Test2.Clone();
            // == -operator
            nua.Assert("#Q01", (Test2 == t2).Value);
            nua.Assert("#Q02", !(Test1 == Test2).Value);
            nua.Assert("#Q03", (Test1 == NullableString.Null).IsNull);
                        
            // != -operator
            nua.Assert("#Q04", !(t2 != Test2).Value);
            nua.Assert("#Q05", !(Test2 != t2).Value);
            nua.Assert("#Q06", (Test1 != Test3).Value);
            nua.Assert("#Q07", (Test1 != NullableString.Null).IsNull);
        }


        [nu.Test]
        public void NullableBooleanToNullableString() {
            NullableBoolean TestBoolean = new NullableBoolean (true);
            NullableBoolean TestBoolean2 = new NullableBoolean (false);
            NullableString Result;

            Result = (NullableString)TestBoolean;
            nua.AssertEquals("#R01", "True", Result.Value);
                        
            Result = (NullableString)TestBoolean2;
            nua.AssertEquals("#R02", "False", Result.Value);
                        
            Result = (NullableString)NullableBoolean.Null;
            nua.Assert("#R03", Result.IsNull);
        }


        [nu.Test]
        public void NullableByteToBoolean() {
            NullableByte TestByte = new NullableByte (250);
            nua.AssertEquals("#S01", "250", ((NullableString)TestByte).Value);
            try {
                NullableString test = ((NullableString)NullableByte.Null).Value;
                nua.Fail ("#S02");
            } catch (sys.Exception e) {
                nua.AssertEquals("#S03", typeof (NullableNullValueException), e.GetType ());
            }
        }


        [nu.Test]
        public void NullableDateTimeToNullableString() {                        
            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                NullableDateTime TestTime = new NullableDateTime(2002, 10, 22, 9, 52, 30);
                nua.AssertEquals("#T01", "10/22/2002 09:52:30", ((NullableString)TestTime).Value);                        
            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }
        }
                

        [nu.Test]
        public void NullableDecimalToNullableString() {
            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                NullableDecimal TestDecimal = new NullableDecimal (1000.2345);
                nua.AssertEquals("#U01", "1000.2345", ((NullableString)TestDecimal).Value);                  
            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

        }
                

        [nu.Test]
        public void NullableDoubleToNullableString() {
            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                NullableDouble TestDouble = new NullableDouble(64E+64);
                nua.AssertEquals("#V01", "6.4E+65", ((NullableString)TestDouble).Value);
            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

        }

                
        [nu.Test]
        public void NullableInt16ToNullableString() {
            NullableInt16 TestInt = new NullableInt16(20012);
            nua.AssertEquals("#X01", "20012", ((NullableString)TestInt).Value);
            try {
                NullableString test = ((NullableString)NullableInt16.Null).Value;
                nua.Fail ("#X02");
            } catch (sys.Exception e) {
                nua.AssertEquals("#X03", typeof (NullableNullValueException), e.GetType ());                                
            }
        }
                

        [nu.Test]
        public void NullableInt32ToNullableString() {
            NullableInt32 TestInt = new NullableInt32(-12456);
            nua.AssertEquals("#Y01", "-12456", ((NullableString)TestInt).Value);
            try {
                NullableString test = ((NullableString)NullableInt32.Null).Value;
                nua.Fail ("#Y02");
            } catch (sys.Exception e) {
                nua.AssertEquals("#Y03", typeof (NullableNullValueException), e.GetType ());                                
            }
        }
                

        [nu.Test]
        public void NullableInt64ToNullableString() {
            NullableInt64 TestInt = new NullableInt64(10101010);
            nua.AssertEquals("#Z01", "10101010", ((NullableString)TestInt).Value);
        }
                
                
        [nu.Test]
        public void NullableSingleToNullableString() {
            NullableSingle TestSingle = new NullableSingle (3E+20);
            nua.AssertEquals("#AB01", "3E+20", ((NullableString)TestSingle).Value);
        }
                      

        [nu.Test]                        
        public void NullableStringToString() {
            nua.AssertEquals("#AC01", "First TestString",(string)Test1);                        
        }


        [nu.Test]
        public void StringToNullableString() {
            string TestString = "Test String";
            nua.AssertEquals("#AD01", "Test String", ((NullableString)TestString).Value);                        
        }                


        #region Serialization - K#
        [nu.Test]
        public void SerializableAttribute() {
            NullableString serializedDeserialized;

            serializedDeserialized = SerializeDeserialize(NullableString.Null);
            nua.Assert("TestK#01", serializedDeserialized.IsNull);
            nua.Assert("TestK#02", NullableString.Null.Equals(serializedDeserialized));

            NullableString ns = new NullableString("Pino");
            serializedDeserialized = SerializeDeserialize(ns);
            nua.Assert("TestK#03", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#04", ns.Value, serializedDeserialized.Value);
            nua.Assert("TestK#05", ns.Equals(serializedDeserialized));

        }

        private NullableString SerializeDeserialize(NullableString x) {
            System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer = 
                new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableString y = (NullableString)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }

        }


        public void XmlSerializable() {
            NullableString serializedDeserialized;

            serializedDeserialized = SerializeDeserialize(NullableString.Null);
            nua.Assert("TestK#01", serializedDeserialized.IsNull);
            nua.Assert("TestK#02", NullableString.Null.Equals(serializedDeserialized));

            NullableString ns = new NullableString("Pino");
            serializedDeserialized = SerializeDeserialize(ns);
            nua.Assert("TestK#03", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#04", ns.Value, serializedDeserialized.Value);
            nua.Assert("TestK#05", ns.Equals(serializedDeserialized));

        }

        private NullableString XmlSerializeDeserialize(NullableString x) {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableString));
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableString y = (NullableString)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }

        }
 

        [nu.Test]
        public void XmlSerializableEmptyElementNil() {
            // Bug reported by Shaun Bowe (sbowe@users.sourceforge.net)
            // http://sourceforge.net/forum/message.php?msg_id=2399265

            //<?xml version="1.0"?>
            //<NullableString xsi:nil="true" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"></NullableString>

            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableString));

            using (sys.IO.MemoryStream baseStream = new sys.IO.MemoryStream()) {
                using (sys.IO.StreamWriter stream = new System.IO.StreamWriter(baseStream)) {
                    stream.WriteLine("<?xml version=\"1.0\"?>");
                    stream.WriteLine("<NullableString xsi:nil=\"true\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></NullableString>");
                    stream.Flush();

//                    baseStream.Position = 0;
//                    sys.IO.StreamReader streamReader = new System.IO.StreamReader(baseStream);
//                    sys.Console.WriteLine(streamReader.ReadToEnd());
                    
                    baseStream.Position = 0; // Return stream to start
                    NullableString y = (NullableString)serializer.Deserialize(baseStream);

                    nua.Assert(y.IsNull);

                    baseStream.Close();
                    stream.Close();
                }
            }
        }


        [nu.Test]
        public void XmlSerializableSchema() {
            sysXmlScm.XmlSchema xsd = 
                ((sysXml.Serialization.IXmlSerializable)NullableString.Null).GetSchema();

            xsd.Compile(new sysXmlScm.ValidationEventHandler(ValidationCallBack));

            ValidateXmlAgainstXmlSchema(xsd, NullableString.Null);
            ValidateXmlAgainstXmlSchema(xsd, NullableString.Empty);
            ValidateXmlAgainstXmlSchema(xsd, new NullableString("gnirts A"));

        }

        private void ValidateXmlAgainstXmlSchema(sysXmlScm.XmlSchema xsd, NullableString x) {
            sysXml.Serialization.XmlSerializer serializer = 
                new sysXml.Serialization.XmlSerializer(typeof(NullableString));

                
            sys.IO.MemoryStream stream = null;
            sys.Xml.XmlValidatingReader validator = null;
            try {
                // Get the serialized NullableString instance
                stream = new sys.IO.MemoryStream();
                serializer.Serialize(stream, x);
                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start

                // Add the default namespace
                sysXml.XmlDocument doc = new sysXml.XmlDocument();
                doc.Load(stream);                    
                sysXml.XmlAttribute defaultNs = doc.CreateAttribute("xmlns");
                defaultNs.Value = "http://NullableTypes.SourceForge.Net/NullableStringXMLSchema";
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
        #endregion //Serialization
    }
}