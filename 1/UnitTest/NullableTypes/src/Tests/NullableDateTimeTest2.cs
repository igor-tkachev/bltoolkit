//
// NullableTypes.Tests.NullableDateTimeTest
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 24-Mar-2003  Luca    Create     Older tests from www.go-mono.com SqlTypes
// 20-Aug-2003  Luca    Upgrade    New test: test for ToString() on NullableDateTime.Null 
// 23-Aug-2003  Luca    Upgrade    Code upgrade: Replaced new CultureInfo("") with equivalent 
//                                 CultureInfo.InvariantCulture
// 15-Sep-2003  Luca    Upgrade    Code upgrade: improved test TestParse to make it independent 
//                                 from user regional settings
// 06-Oct-2003  Luca    Upgrade    Code upgrade: Replaced tabs with spaces and removed commented out code
//


namespace NullableTypes.Tests {

    using nu = NUnit.Framework;
    using nua = NUnit.Framework.Assertion;
    using sys = System;
    using sysGlb = System.Globalization;
    using sysThr = System.Threading;
    
    [nu.TestFixture]
    public class NullableDateTimeTest2  {

        private long[] _myTicks = {
                                     631501920000000000L,    // 25 Feb 2002 - 00:00:00
                                     631502475130080000L,    // 25 Feb 2002 - 15:25:13,8
                                     631502115130080000L,    // 25 Feb 2002 - 05:25:13,8
                                     631502115000000000L,    // 25 Feb 2002 - 05:25:00
                                     631502115130000000L,    // 25 Feb 2002 - 05:25:13
                                     631502079130000000L,    // 25 Feb 2002 - 04:25:13
                                     629197085770000000L    // 06 Nov 1994 - 08:49:37 
                                 };

        private NullableDateTime _test1;
        private NullableDateTime _test2;
        private NullableDateTime _test3;



        [nu.SetUp]
        public void SetUp() {
            _myTicks = new long[] {
                                    631501920000000000L,    // 25 Feb 2002 - 00:00:00
                                    631502475130080000L,    // 25 Feb 2002 - 15:25:13,8
                                    631502115130080000L,    // 25 Feb 2002 - 05:25:13,8
                                    631502115000000000L,    // 25 Feb 2002 - 05:25:00
                                    631502115130000000L,    // 25 Feb 2002 - 05:25:13
                                    631502079130000000L,    // 25 Feb 2002 - 04:25:13
                                    629197085770000000L        // 06 Nov 1994 - 08:49:37 
                                  };

            _test1 = new NullableDateTime(2002, 10, 19, 9, 40, 0);
            _test2 = new NullableDateTime(2003, 11, 20,10, 50, 1);
            _test3 = new NullableDateTime(2003, 11, 20, 10, 50, 1);
        }


        // Test constructor
        public void TestCreate() {
            // NullableDateTime (DateTime)
            NullableDateTime CTest = new NullableDateTime (
                new sys.DateTime (2002, 5, 19, 3, 34, 0));
            nua.AssertEquals ("#A01", 2002, CTest.Value.Year);
                
            // NullableDateTime (int, int)
            CTest = new NullableDateTime(sys.DateTime.MinValue.Year, 
                                         sys.DateTime.MinValue.Month,
                                         sys.DateTime.MinValue.Day);
            
            // NullableDateTime (int, int, int)
            nua.AssertEquals ("#A02", sys.DateTime.MinValue.Year, CTest.Value.Year);
            nua.AssertEquals ("#A03", sys.DateTime.MinValue.Month, CTest.Value.Month);
            nua.AssertEquals ("#A04", sys.DateTime.MinValue.Day, CTest.Value.Day);
            nua.AssertEquals ("#A05", 0, CTest.Value.Hour);

            // NullableDateTime (int, int, int, int, int, int)
            CTest = new NullableDateTime (5000, 12, 31);
            nua.AssertEquals ("#A06", 5000, CTest.Value.Year);
            nua.AssertEquals ("#A07", 12, CTest.Value.Month);
            nua.AssertEquals ("#A08", 31, CTest.Value.Day);

            // NullableDateTime (int, int, int, int, int, int, double)
            CTest = new NullableDateTime (1978, 5, 19, 3, 34, 0);
            nua.AssertEquals ("#A09", 1978, CTest.Value.Year);
            nua.AssertEquals ("#A10", 5, CTest.Value.Month);
            nua.AssertEquals ("#A11", 19, CTest.Value.Day);
            nua.AssertEquals ("#A12", 3, CTest.Value.Hour);
            nua.AssertEquals ("#A13", 34, CTest.Value.Minute);
            nua.AssertEquals ("#A14", 0, CTest.Value.Second);
            
            try {
                CTest = new NullableDateTime (10000, 12, 31);
                nua.Fail ("#A15");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#A16", typeof (sys.ArgumentOutOfRangeException),
                    e.GetType ());
            }
            
            // NullableDateTime (int, int, int, int, int, int, int)
            CTest = new NullableDateTime (1978, 5, 19, 3, 34, 0, 12);
            nua.AssertEquals ("#A17", 1978, CTest.Value.Year);
            nua.AssertEquals ("#A18", 5, CTest.Value.Month);
            nua.AssertEquals ("#A19", 19, CTest.Value.Day);
            nua.AssertEquals ("#A20", 3, CTest.Value.Hour);
            nua.AssertEquals ("#A21", 34, CTest.Value.Minute);
            nua.AssertEquals ("#A22", 0, CTest.Value.Second);
            nua.AssertEquals ("#A23", 12, CTest.Value.Millisecond);
        }

        // Test public fields
        public void TestPublicFields() {
            // MaxValue
            nua.AssertEquals ("#B01", 9999, NullableDateTime.MaxValue.Value.Year);
            nua.AssertEquals ("#B02", 12, NullableDateTime.MaxValue.Value.Month);
            nua.AssertEquals ("#B03", 31, NullableDateTime.MaxValue.Value.Day);
            nua.AssertEquals ("#B04", 23, NullableDateTime.MaxValue.Value.Hour);
            nua.AssertEquals ("#B05", 59, NullableDateTime.MaxValue.Value.Minute);
            nua.AssertEquals ("#B06", 59, NullableDateTime.MaxValue.Value.Second);

            // MinValue
            nua.AssertEquals ("#B07", sys.DateTime.MinValue.Year, NullableDateTime.MinValue.Value.Year);
            nua.AssertEquals ("#B08", sys.DateTime.MinValue.Month, NullableDateTime.MinValue.Value.Month);
            nua.AssertEquals ("#B09", sys.DateTime.MinValue.Day, NullableDateTime.MinValue.Value.Day);
            nua.AssertEquals ("#B10", sys.DateTime.MinValue.Hour, NullableDateTime.MinValue.Value.Hour);
            nua.AssertEquals ("#B11", sys.DateTime.MinValue.Minute, NullableDateTime.MinValue.Value.Minute);
            nua.AssertEquals ("#B12", sys.DateTime.MinValue.Second, NullableDateTime.MinValue.Value.Second);

            // Null
            nua.Assert ("#B13", NullableDateTime.Null.IsNull);

            // TicksPerHour
            nua.AssertEquals ("#B14", 36000000000, NullableDateTime.TicksPerHour);

            // TicksPerMinute
            nua.AssertEquals ("#B15", 600000000, NullableDateTime.TicksPerMinute);

            // TicksPerSecond
            nua.AssertEquals ("#B16", 10000000, NullableDateTime.TicksPerSecond);
        }

        // Test properties
        public void TestProperties() {
                
            // IsNull
            nua.Assert ("#C04", NullableDateTime.Null.IsNull);
            nua.Assert ("#C05", !_test2.IsNull);


            // Value
            nua.AssertEquals ("#C09", 2003, _test2.Value.Year);
            nua.AssertEquals ("#C10", 2002, _test1.Value.Year);            
        }

        // PUBLIC METHODS

        public void TestCompareTo() {
            NullableString TestString = new NullableString ("This is a test");

            nua.Assert ("#D01", _test1.CompareTo (_test3) < 0);
            nua.Assert ("#D02", _test2.CompareTo (_test1) > 0);
            nua.Assert ("#D03", _test2.CompareTo (_test3) == 0);
            nua.Assert ("#D04", _test1.CompareTo (NullableDateTime.Null) > 0);

            try {
                _test1.CompareTo (TestString);
                nua.Fail("#D05");
            } catch(sys.Exception e) {
                nua.AssertEquals ("#D06", typeof (sys.ArgumentException), e.GetType ());
            }
        }

        public void TestEqualsMethods() {
            nua.Assert ("#E01", !_test1.Equals (_test2));
            nua.Assert ("#E04", _test2.Equals (_test3));

            // Static Equals()-method
            nua.Assert ("#E05", NullableDateTime.Equals (_test2, _test3).Value);
            nua.Assert ("#E06", !NullableDateTime.Equals (_test1, _test2).Value);
        }

        public void TestEqualsMethods2() {
            nua.Assert ("#E03", !_test2.Equals (new NullableString ("TEST")));
        }

        public void TestGetHashCode() {
            // FIXME: Better way to test HashCode
            nua.AssertEquals ("#F01", _test1.GetHashCode (), _test1.GetHashCode ());
            nua.Assert ("#F02", _test2.GetHashCode () != _test1.GetHashCode ());
        }

        public void TestGetType() {
            nua.AssertEquals ("#G01", "NullableTypes.NullableDateTime", 
                _test1.GetType ().ToString ());
            nua.AssertEquals ("#G02", "System.DateTime", 
                _test1.Value.GetType ().ToString ());
        }

        public void TestGreaters() {
            // GreateThan ()
            nua.Assert ("#H01", !NullableDateTime.GreaterThan (_test1, _test2).Value);
            nua.Assert ("#H02", NullableDateTime.GreaterThan (_test2, _test1).Value);
            nua.Assert ("#H03", !NullableDateTime.GreaterThan (_test2, _test3).Value);

            // GreaterTharOrEqual ()
            nua.Assert ("#H04", !NullableDateTime.GreaterThanOrEqual (_test1, _test2).Value);
            nua.Assert ("#H05", NullableDateTime.GreaterThanOrEqual (_test2, _test1).Value);
            nua.Assert ("#H06", NullableDateTime.GreaterThanOrEqual (_test2, _test3).Value);
        }

        public void TestLessers() {
            // LessThan()
            nua.Assert ("#I01", !NullableDateTime.LessThan (_test2, _test3).Value);
            nua.Assert ("#I02", !NullableDateTime.LessThan (_test2, _test1).Value);
            nua.Assert ("#I03", NullableDateTime.LessThan (_test1, _test3).Value);

            // LessThanOrEqual ()
            nua.Assert ("#I04", NullableDateTime.LessThanOrEqual (_test1, _test2).Value);
            nua.Assert ("#I05", !NullableDateTime.LessThanOrEqual (_test2, _test1).Value);
            nua.Assert ("#I06", NullableDateTime.LessThanOrEqual (_test3, _test2).Value);
            nua.Assert ("#I07", NullableDateTime.LessThanOrEqual (_test1, NullableDateTime.Null).IsNull);
        }

        public void TestNotEquals() {
            nua.Assert ("#J01", NullableDateTime.NotEquals (_test1, _test2).Value);
            nua.Assert ("#J02", NullableDateTime.NotEquals (_test3, _test1).Value);
            nua.Assert ("#J03", !NullableDateTime.NotEquals (_test2, _test3).Value);
            nua.Assert ("#J04", NullableDateTime.NotEquals (NullableDateTime.Null, _test2).IsNull);
        }

        [nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void TestParseArgumentNullException() {
            NullableDateTime.Parse(null);
        }

        [nu.ExpectedException(typeof(sys.FormatException))]
        public void TestParseFormatException() {
            NullableDateTime.Parse ("not-a-number");
        }


        public void TestParse() {

            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                NullableDateTime t1 = NullableDateTime.Parse(new sys.DateTime(2002,2,25).ToShortDateString());
                nua.AssertEquals ("#K05", _myTicks[0], t1.Value.Ticks);

                try {
                    t1 = NullableDateTime.Parse ("2002-02-25");
                } catch (sys.Exception e) {
                    nua.Fail ("#K06 " + e);
                }

                nua.AssertEquals ("#K07", _myTicks[0], t1.Value.Ticks);
                t1 = NullableDateTime.Parse ("Monday, 25 February 2002");
                nua.AssertEquals ("#K08", _myTicks[0], t1.Value.Ticks);
                t1 = NullableDateTime.Parse ("Monday, 25 February 2002 05:25");
                nua.AssertEquals ("#K09", _myTicks[3], t1.Value.Ticks);
                t1 = NullableDateTime.Parse ("Monday, 25 February 2002 05:25:13");
                nua.AssertEquals ("#K10", _myTicks[4], t1.Value.Ticks);
                t1 = NullableDateTime.Parse (new sys.DateTime(2002,2,25,05,25,0).ToString("g"));
                nua.AssertEquals ("#K11", _myTicks[3], t1.Value.Ticks);
                t1 = NullableDateTime.Parse (new sys.DateTime(2002,2,25,05,25,13).ToString("G"));
                nua.AssertEquals ("#K12", _myTicks[4], t1.Value.Ticks);

            
                NullableDateTime t2 = new NullableDateTime (sys.DateTime.Today.Year, 2, 25);
                t1 = NullableDateTime.Parse ("February 25");
                nua.AssertEquals ("#K19", t2.Value.Ticks, t1.Value.Ticks);
            
                t2 = new NullableDateTime (sys.DateTime.Today.Year, 2, 8);
                t1 = NullableDateTime.Parse ("February 08");
                nua.AssertEquals ("#K20", t2.Value.Ticks, t1.Value.Ticks);

                t1 = NullableDateTime.Parse ("Mon, 25 Feb 2002 04:25:13 GMT");
                t1 = sys.TimeZone.CurrentTimeZone.ToUniversalTime(t1.Value);
                nua.AssertEquals ("#K21", 2002, t1.Value.Year);
                nua.AssertEquals ("#K22", 02, t1.Value.Month);
                nua.AssertEquals ("#K23", 25, t1.Value.Day);
                nua.AssertEquals ("#K24", 04, t1.Value.Hour);
                nua.AssertEquals ("#K25", 25, t1.Value.Minute);
                nua.AssertEquals ("#K26", 13, t1.Value.Second);

                t1 = NullableDateTime.Parse ("2002-02-25T05:25:13");
                nua.AssertEquals ("#K27", _myTicks[4], t1.Value.Ticks);

                t2 = sys.DateTime.Today + new sys.TimeSpan (5,25,0);
                t1 = NullableDateTime.Parse ("05:25");
                nua.AssertEquals("#K28", t2.Value.Ticks, t1.Value.Ticks);

                t2 = sys.DateTime.Today + new sys.TimeSpan (5,25,13);
                t1 = NullableDateTime.Parse ("05:25:13");
                nua.AssertEquals("#K29", t2.Value.Ticks, t1.Value.Ticks);

                t2 = new NullableDateTime (2002, 2, 1);
                t1 = NullableDateTime.Parse ("2002 February");
                nua.AssertEquals ("#K30", t2.Value.Ticks, t1.Value.Ticks);
            
                t2 = new NullableDateTime (2002, 2, 1);
                t1 = NullableDateTime.Parse ("2002 February");
                nua.AssertEquals ("#K31", t2.Value.Ticks, t1.Value.Ticks);
            
                t2 = new NullableDateTime (sys.DateTime.Today.Year, 2, 8);
                t1 = NullableDateTime.Parse ("February 8");
            
                nua.AssertEquals ("#K32", t2.Value.Ticks, t1.Value.Ticks);
            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

        }

        public void TestToString() {
            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;
            
                NullableDateTime t1 = new NullableDateTime(2002, 2, 25, 5, 25, 13);
                NullableDateTime t2 = new NullableDateTime(2002, 2, 25, 15, 25, 13);
            
                // Standard patterns
                nua.AssertEquals("L01", "02/25/2002 05:25:13", t1.ToString());
                nua.AssertEquals("L02", (NullableString)"02/25/2002 05:25:13", t1.ToNullableString ());
            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

            nua.AssertEquals("L03", "Null", NullableDateTime.Null.ToString());

        }


        // OPERATORS

        public void TestArithmeticOperators() {
            sys.TimeSpan TestSpan = new sys.TimeSpan (20, 1, 20, 20);
            NullableDateTime ResultDateTime;

            // "+"-operator
            ResultDateTime = _test1 + TestSpan;
            nua.AssertEquals ("#M01", 2002, ResultDateTime.Value.Year);
            nua.AssertEquals ("#M02", 8, ResultDateTime.Value.Day);
            nua.AssertEquals ("#M03", 11, ResultDateTime.Value.Hour);
            nua.AssertEquals ("#M04", 0, ResultDateTime.Value.Minute);
            nua.AssertEquals ("#M05", 20, ResultDateTime.Value.Second);     
            nua.Assert ("#M06", (NullableDateTime.Null + TestSpan).IsNull);

            try {
                ResultDateTime = NullableDateTime.MaxValue + TestSpan;
                nua.Fail ("#M07");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#M08", typeof (sys.ArgumentOutOfRangeException), e.GetType ());
            }

            // "-"-operator
            ResultDateTime = _test1 - TestSpan;
            nua.AssertEquals ("#M09", 2002, ResultDateTime.Value.Year);
            nua.AssertEquals ("#M10", 29, ResultDateTime.Value.Day);
            nua.AssertEquals ("#M11", 8, ResultDateTime.Value.Hour);
            nua.AssertEquals ("#M12", 19, ResultDateTime.Value.Minute);
            nua.AssertEquals ("#M13", 40, ResultDateTime.Value.Second);     
            nua.Assert ("#M14", (NullableDateTime.Null - TestSpan).IsNull);
            
            try {
                ResultDateTime = NullableDateTime.MinValue - TestSpan;
                nua.Fail ("#M15");
            } catch  (sys.Exception e) {
                nua.AssertEquals ("#M16", typeof (sys.ArgumentOutOfRangeException), e.GetType ());
            }
        }

        public void TestThanOrEqualOperators() {
            // == -operator
            nua.Assert ("#N01", (_test2 == _test3).Value);
            nua.Assert ("#N02", !(_test1 == _test2).Value);
            nua.Assert ("#N03", (_test1 == NullableDateTime.Null).IsNull);
                        
            // != -operator
            nua.Assert ("#N04", !(_test2 != _test3).Value);
            nua.Assert ("#N05", (_test1 != _test3).Value);
            nua.Assert ("#N06", (_test1 != NullableDateTime.Null).IsNull);

            // > -operator
            nua.Assert ("#N07", (_test2 > _test1).Value);
            nua.Assert ("#N08", !(_test3 > _test2).Value);
            nua.Assert ("#N09", (_test1 > NullableDateTime.Null).IsNull);

            // >=  -operator
            nua.Assert ("#N10", !(_test1 >= _test3).Value);
            nua.Assert ("#N11", (_test3 >= _test1).Value);
            nua.Assert ("#N12", (_test2 >= _test3).Value);
            nua.Assert ("#N13", (_test1 >= NullableDateTime.Null).IsNull);

            // < -operator
            nua.Assert ("#N14", !(_test2 < _test1).Value);
            nua.Assert ("#N15", (_test1 < _test3).Value);
            nua.Assert ("#N16", !(_test2 < _test3).Value);
            nua.Assert ("#N17", (_test1 < NullableDateTime.Null).IsNull);

            // <= -operator
            nua.Assert ("#N18", (_test1 <= _test3).Value);
            nua.Assert ("#N19", !(_test3 <= _test1).Value);
            nua.Assert ("#N20", (_test2 <= _test3).Value);
            nua.Assert ("#N21", (_test1 <= NullableDateTime.Null).IsNull);
        }

        public void TestNullableDateTimeToDateTime() {
            nua.AssertEquals ("O01", 2002, ((sys.DateTime)_test1).Year);
            nua.AssertEquals ("O03", 2003, ((sys.DateTime)_test2).Year);
            nua.AssertEquals ("O04", 10, ((sys.DateTime)_test1).Month);
            nua.AssertEquals ("O05", 19, ((sys.DateTime)_test1).Day);
            nua.AssertEquals ("O06", 9, ((sys.DateTime)_test1).Hour);
            nua.AssertEquals ("O07", 40, ((sys.DateTime)_test1).Minute);
            nua.AssertEquals ("O08", 0, ((sys.DateTime)_test1).Second);
        }

        public void TestNullableStringToNullableDateTime() {
            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;
            
                NullableString TestString = new NullableString ("02/25/2002");
                NullableDateTime t1 = (NullableDateTime)TestString;

                nua.AssertEquals ("#P01", _myTicks[0], t1.Value.Ticks);

                nua.AssertEquals ("#P02", _myTicks[0], t1.Value.Ticks);
                t1 = (NullableDateTime) new NullableString ("Monday, 25 February 2002");
                nua.AssertEquals ("#P04", _myTicks[0], t1.Value.Ticks);
                t1 = (NullableDateTime) new NullableString ("Monday, 25 February 2002 05:25");
                nua.AssertEquals ("#P05", _myTicks[3], t1.Value.Ticks);
                t1 = (NullableDateTime) new NullableString ("Monday, 25 February 2002 05:25:13");
                nua.AssertEquals ("#P05", _myTicks[4], t1.Value.Ticks);
                t1 = (NullableDateTime) new NullableString ("02/25/2002 05:25");
                nua.AssertEquals ("#P06", _myTicks[3], t1.Value.Ticks);
                t1 = (NullableDateTime) new NullableString ("02/25/2002 05:25:13");
                nua.AssertEquals ("#P07", _myTicks[4], t1.Value.Ticks);
                t1 = (NullableDateTime) new NullableString ("2002-02-25 04:25:13Z");
                t1 = sys.TimeZone.CurrentTimeZone.ToUniversalTime(t1.Value);
                nua.AssertEquals ("#P08", 2002, t1.Value.Year);
                nua.AssertEquals ("#P09", 02, t1.Value.Month);
                nua.AssertEquals ("#P10", 25, t1.Value.Day);
                nua.AssertEquals ("#P11", 04, t1.Value.Hour);
                nua.AssertEquals ("#P12", 25, t1.Value.Minute);
                nua.AssertEquals ("#P13", 13, t1.Value.Second);
            
                NullableDateTime t2 = new NullableDateTime (sys.DateTime.Today.Year, 2, 25);
                t1 = (NullableDateTime) new NullableString ("February 25");
                nua.AssertEquals ("#P14", t2.Value.Ticks, t1.Value.Ticks);
            
                t2 = new NullableDateTime (sys.DateTime.Today.Year, 2, 8);
                t1 = (NullableDateTime) new NullableString ("February 08");
                nua.AssertEquals ("#P15", t2.Value.Ticks, t1.Value.Ticks);

                t1 = (NullableDateTime) new NullableString ("Mon, 25 Feb 2002 04:25:13 GMT");
                t1 = sys.TimeZone.CurrentTimeZone.ToUniversalTime(t1.Value);
                nua.AssertEquals ("#P16", 2002, t1.Value.Year);
                nua.AssertEquals ("#P17", 02, t1.Value.Month);
                nua.AssertEquals ("#P18", 25, t1.Value.Day);
                nua.AssertEquals ("#P19", 04, t1.Value.Hour);
                nua.AssertEquals ("#P20", 25, t1.Value.Minute);
                nua.AssertEquals ("#P21", 13, t1.Value.Second);

                t1 = (NullableDateTime) new NullableString ("2002-02-25T05:25:13");
                nua.AssertEquals ("#P22", _myTicks[4], t1.Value.Ticks);

                t2 = sys.DateTime.Today + new sys.TimeSpan (5,25,0);
                t1 = (NullableDateTime) new NullableString ("05:25");
                nua.AssertEquals("#P23", t2.Value.Ticks, t1.Value.Ticks);

                t2 = sys.DateTime.Today + new sys.TimeSpan (5,25,13);
                t1 = (NullableDateTime) new NullableString ("05:25:13");
                nua.AssertEquals("#P24", t2.Value.Ticks, t1.Value.Ticks);

                t2 = new NullableDateTime (2002, 2, 1);
                t1 = (NullableDateTime) new NullableString ("2002 February");
                nua.AssertEquals ("#P25", t2.Value.Ticks, t1.Value.Ticks);
            
                t2 = new NullableDateTime (2002, 2, 1);
                t1 = (NullableDateTime) new NullableString ("2002 February");
                nua.AssertEquals ("#P26", t2.Value.Ticks, t1.Value.Ticks);
            
                t2 = new NullableDateTime (sys.DateTime.Today.Year, 2, 8);
                t1 = (NullableDateTime) new NullableString ("February 8");
            
                nua.AssertEquals ("#P27", t2.Value.Ticks, t1.Value.Ticks);
            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }
        }

        public void TestDateTimeToNullableDateTime() {
            sys.DateTime DateTimeTest = new sys.DateTime(2002, 10, 19, 11, 53, 4);
            NullableDateTime Result = (NullableDateTime)DateTimeTest;
            nua.AssertEquals ("#Q01", 2002, Result.Value.Year);
            nua.AssertEquals ("#Q02", 10, Result.Value.Month);
            nua.AssertEquals ("#Q03", 19, Result.Value.Day);
            nua.AssertEquals ("#Q04", 11, Result.Value.Hour);
            nua.AssertEquals ("#Q05", 53, Result.Value.Minute);
            nua.AssertEquals ("#Q06", 4, Result.Value.Second);
        }
    }
}

