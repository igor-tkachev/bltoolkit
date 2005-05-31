//
// NullableTypes.NullableTypes.Tests.NullableDecimalTest3
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 27-Jun-2003  Luca    Create     Older tests from www.go-mono.com SqlTypes
// 02-Jul-2003  Luca    Create     Minor changes and some bug fixing
// 23-Aug-2003  Luca    Upgrade    Code upgrade: Replaced new CultureInfo("") with equivalent 
//                                 CultureInfo.InvariantCulture
// 06-Oct-2003  Luca    Upgrade    Code upgrade: Replaced tabs with spaces and removed commented out code
//

namespace NullableTypes.Tests {

    using nu = NUnit.Framework;
    using nua = NUnit.Framework.Assertion;
    using sys = System;
    
    [nu.TestFixture]
    public class NullableDecimalTest3  {

        private NullableDecimal Test1;
        private NullableDecimal Test2;
        private NullableDecimal Test3;
        private NullableDecimal Test4;           


        [nu.SetUp]
        public void SetUp() {
            Test1 = new NullableDecimal(-234.254m);
            Test2 = new NullableDecimal(9256245.02347m); 
            Test3 = new NullableDecimal(9256245.02347m); 
            Test4 = new NullableDecimal(-234.254m);                 
        }


        // Test constructor
        public void TestCreate() {
            // NullableDecimal(decimal)
            NullableDecimal Test = new NullableDecimal(30.3098m);
            nua.AssertEquals ("#A01", 30.3098m, Test.Value);
                    
            try {
                NullableDecimal test = new NullableDecimal(decimal.MaxValue + 1);
                nua.Fail ("#A02");                        
            } catch (sys.Exception e) {
                nua.AssertEquals ("#A03", typeof (sys.OverflowException), e.GetType ());
            }
                    
            // NullableDecimal(double)
            Test = new NullableDecimal(10E+10d);
            nua.AssertEquals ("#A05", 100000000000m, Test.Value);
                    
            try {
                NullableDecimal test = new NullableDecimal(10E+200d);
                nua.Fail ("#A06");                        
            } catch (sys.Exception e) {
                nua.AssertEquals ("#A07", typeof (sys.OverflowException), e.GetType ());
            }
                    
            // NullableDecimal(int)
            Test = new NullableDecimal(-1);
            nua.AssertEquals ("#A08", -1m, Test.Value);
                
            // NullableDecimal(long)
            Test = new NullableDecimal((long)(-99999));
            nua.AssertEquals ("#A09", -99999m, Test.Value);
                
                    

        }

        // Test public fields
        public void TestPublicFields() {
            nua.Assert ("#B05", NullableDecimal.Null.IsNull);
            nua.Assert ("#B06", !Test1.IsNull);
        }

        // Test properties
        public void TestProperties() {
                    
                
            nua.Assert ("#C03", NullableDecimal.Null.IsNull);
            nua.Assert ("#C04", !Test1.IsPositive);
            nua.Assert ("#C05", !Test4.IsPositive);
            nua.AssertEquals ("#C08", -234.254m, Test1.Value); 
        }

        // PUBLIC METHODS

        public void TestArithmeticMethods() {

            // Abs
            nua.AssertEquals ("#D01", 234.254m, NullableDecimal.Abs (Test4).Value);
            nua.AssertEquals ("#D02", new NullableDecimal(234.254m).Value, NullableDecimal.Abs(Test1).Value);
                    
            nua.Assert ("#D03", NullableDecimal.Abs(NullableDecimal.Null).IsNull);
                    
            // Add()
            nua.AssertEquals ("#D04", Test1.Value + Test2.Value, NullableDecimal.Add (Test1, Test2).Value);

            try {
                NullableDecimal test = NullableDecimal.Add (NullableDecimal.MaxValue, NullableDecimal.MaxValue);
                nua.Fail ("#D05");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#D06", typeof (sys.OverflowException), e.GetType ());
            }
                        
            nua.AssertEquals ("#D07", -234, NullableDecimal.Ceiling(Test1).Value);
            nua.Assert ("#D08", NullableDecimal.Ceiling(NullableDecimal.Null).IsNull);
                    
            // Divide()
            nua.AssertEquals ("#D09", Test1.Value / Test4.Value, NullableDecimal.Divide (Test1, Test4).Value);
            nua.AssertEquals ("#D10", Test2.Value / Test1.Value, NullableDecimal.Divide (Test2, Test1).Value);

            try {
                NullableDecimal test = NullableDecimal.Divide(Test1, new NullableDecimal(0)).Value;
                nua.Fail ("#D11");
            } catch(sys.Exception e) {
                nua.AssertEquals ("#D12", typeof (sys.DivideByZeroException), e.GetType ());
            }

            nua.AssertEquals ("#D13", -235, NullableDecimal.Floor(Test1).Value);
                    
            // Multiply()
            nua.AssertEquals ("#D14", Test1.Value * Test2.Value, NullableDecimal.Multiply (Test1, Test2).Value);
            nua.AssertEquals ("#D15", Test1.Value * Test4.Value, NullableDecimal.Multiply (Test1, Test4).Value);

            try {
                NullableDecimal test = NullableDecimal.Multiply (NullableDecimal.MaxValue, Test1);
                nua.Fail ("#D16");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#D17", typeof (sys.OverflowException), e.GetType ());
            }
                        
                       
            // Round
            nua.AssertEquals ("#D19", decimal.Round(Test1.Value, 2), NullableDecimal.Round (Test1, 2).Value);
                    
            // Subtract()
            nua.AssertEquals ("#D20", Test1.Value - Test3.Value, NullableDecimal.Subtract(Test1, Test3).Value);

            try {
                NullableDecimal test = NullableDecimal.Subtract(NullableDecimal.MinValue, NullableDecimal.MaxValue);
                nua.Fail ("#D21");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#D22", typeof (sys.OverflowException), e.GetType ());
            }                           
                        
            nua.AssertEquals ("#D23", (NullableInt32)1, NullableDecimal.Sign (Test2));
            nua.AssertEquals ("#D24", new NullableInt32(-1), NullableDecimal.Sign (Test4));
        }

        public void TestCompareTo() {
            NullableString TestString = new NullableString ("This is a test");

            nua.Assert ("#G01", Test1.CompareTo (Test3) < 0);
            nua.Assert ("#G02", Test2.CompareTo (Test1) > 0);
            nua.Assert ("#G03", Test2.CompareTo (Test3) == 0);
            nua.Assert ("#G04", Test4.CompareTo (NullableDecimal.Null) > 0);

            try {
                Test1.CompareTo (TestString);
                nua.Fail("#G05");
            } catch(sys.Exception e) {
                nua.AssertEquals ("#G06", typeof (sys.ArgumentException), e.GetType ());
            }
        }

        public void TestEqualsMethods() {
            nua.Assert ("#H01", !Test1.Equals (Test2));

            try {
                nua.Assert ("#H02", !Test2.Equals (new NullableString ("TEST")));
                nua.Fail ("#H02");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#H02", typeof (sys.ArgumentException), e.GetType ());
            }

            nua.Assert ("#H03", Test2.Equals (Test3));

            // Static Equals()-method
            nua.Assert ("#H05", NullableDecimal.Equals (Test2, Test2).Value);
            nua.Assert ("#H06", !NullableDecimal.Equals (Test1, Test2).Value);
                    
            // NotEquals
            nua.Assert ("#H07", NullableDecimal.NotEquals (Test1, Test2).Value);
            nua.Assert ("#H08", NullableDecimal.NotEquals (Test4, Test3).Value);
            nua.Assert ("#H09", !NullableDecimal.NotEquals (Test2, Test3).Value);
            nua.Assert ("#H10", NullableDecimal.NotEquals (NullableDecimal.Null, Test3).IsNull);                 
        }


        public void TestGetType() {
            nua.AssertEquals ("#J01", "NullableTypes.NullableDecimal", 
                Test1.GetType ().ToString ());
            nua.AssertEquals ("#J02", "System.Decimal", Test1.Value.GetType ().ToString ());
        }

        public void TestGreaters() {
            // GreateThan ()
            nua.Assert ("#K01", !NullableDecimal.GreaterThan (Test1, Test2).Value);
            nua.Assert ("#K02", NullableDecimal.GreaterThan (Test2, Test1).Value);
            nua.Assert ("#K03", !NullableDecimal.GreaterThan (Test2, Test3).Value);

            // GreaterTharOrEqual ()
            nua.Assert ("#K04", !NullableDecimal.GreaterThanOrEqual (Test1, Test2).Value);
            nua.Assert ("#K05", NullableDecimal.GreaterThanOrEqual (Test2, Test1).Value);
            nua.Assert ("#K06", NullableDecimal.GreaterThanOrEqual (Test2, Test3).Value);
        }

        public void TestLessers() {
            // LessThan()
            nua.Assert ("#L01", !NullableDecimal.LessThan (Test3, Test2).Value);
            nua.Assert ("#L02", !NullableDecimal.LessThan (Test2, Test1).Value);
            nua.Assert ("#L03", NullableDecimal.LessThan (Test1, Test2).Value);

            // LessThanOrEqual ()
            nua.Assert ("#L04", NullableDecimal.LessThanOrEqual (Test1, Test2).Value);
            nua.Assert ("#L05", !NullableDecimal.LessThanOrEqual (Test2, Test1).Value);
            nua.Assert ("#L06", NullableDecimal.LessThanOrEqual (Test2, Test3).Value);
            nua.Assert ("#L07", NullableDecimal.LessThanOrEqual (Test1, NullableDecimal.Null).IsNull);
        }

        public void TestParse() {
            try {
                NullableDecimal.Parse (null);
                nua.Fail ("#m01");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#M02", typeof (sys.ArgumentNullException), e.GetType ());
            }

            try {
                NullableDecimal.Parse ("not-a-number");
                nua.Fail ("#M03");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#M04", typeof (sys.FormatException), e.GetType ());
            }

            try {
                NullableDecimal test = NullableDecimal.Parse("9e300");
                nua.Fail ("#M05");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#M06", typeof(sys.FormatException), e.GetType ());
            }

            nua.AssertEquals("#M07", 150m, NullableDecimal.Parse("150").Value);
        }

        public void TestConversions() {
            // ToDouble
            nua.AssertEquals ("N01", -234.254d, Test1.ToDouble());
                    
            // ToNullableBoolean ()
            nua.Assert ("#N02", Test1.ToNullableBoolean ().IsTrue);
                        
            NullableDecimal Test = new NullableDecimal(0);
            nua.Assert ("#N03", !Test.ToNullableBoolean ().Value);
                    
            Test = new NullableDecimal(0);
            nua.Assert ("#N04", !Test.ToNullableBoolean ().Value);
            nua.Assert ("#N05", NullableDecimal.Null.ToNullableBoolean ().IsNull);

            // ToNullableByte ()
            Test = new NullableDecimal(250);
            nua.AssertEquals ("#N06", (byte)250, Test.ToNullableByte ().Value);

            try {
                NullableByte b = (byte)Test2.ToNullableByte ();
                nua.Fail ("#N07");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#N08", typeof (sys.OverflowException), e.GetType ());
            }

            // ToNullableDouble ()
            nua.AssertEquals ("#N09", ((NullableDouble)Test1).Value, Test1.ToNullableDouble().Value);

            // ToNullableInt16 ()
            nua.AssertEquals ("#N10", (short)1, new NullableDecimal(1).ToNullableInt16 ().Value);

            try {
                NullableInt16 test = NullableDecimal.MaxValue.ToNullableInt16().Value;
                nua.Fail ("#N11");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#N12", typeof (sys.OverflowException), e.GetType ());
            }        


            nua.AssertEquals ("#N13", (int)12, new NullableDecimal(12.212m).ToNullableInt32 ().Value);
                    
            try {
                NullableInt32 test = NullableDecimal.MaxValue.ToNullableInt32 ().Value;
                nua.Fail ("#N14");
            } catch (sys.Exception e) { 
                nua.AssertEquals ("#N15", typeof (sys.OverflowException), e.GetType ());
            }

            // ToNullableInt64 ()
            nua.AssertEquals ("#N16", -234L, Test1.ToNullableInt64 ().Value);


            // ToNullableSingle ()
            nua.AssertEquals ("#N20", -234.254f, Test1.ToNullableSingle ().Value);

            System.Globalization.CultureInfo currCult = System.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                // set a culture info where decimal point is '.'
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

                // ToNullableString ()
                nua.AssertEquals ("#N21", "-234.254", Test1.ToNullableString().Value);

                // ToString ()
                nua.AssertEquals ("#N22", "-234.254", Test1.ToString ());                        
                nua.AssertEquals ("#N23", "79228162514264337593543950335", NullableDecimal.MaxValue.ToString());
            }
            finally {
                // reset culture info 
                System.Threading.Thread.CurrentThread.CurrentCulture = currCult;
            }
        }
                
        public void TestTruncate() {
            nua.AssertEquals ("#O01", -234m, NullableDecimal.Truncate (Test1).Value);
        }
                
        // OPERATORS

        public void TestArithmeticOperators() {
            // "+"-operator
            nua.AssertEquals ("#P01", Test1.Value + Test2.Value, (Test1 + Test2).Value);
     
            try {
                NullableDecimal test = NullableDecimal.MaxValue + NullableDecimal.MaxValue;
                nua.Fail ("#P02");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#P03", typeof (sys.OverflowException), e.GetType ());
            }

            // "/"-operator
            nua.AssertEquals ("#P04", Test1.Value / Test2.Value, (Test1 / Test2).Value);

            try {
                NullableDecimal test = Test3 / new NullableDecimal(0);
                nua.Fail ("#P05");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#P06", typeof (sys.DivideByZeroException), e.GetType ());
            }

            // "*"-operator
            nua.AssertEquals ("#P07", Test1.Value * Test2.Value, (Test1 * Test2).Value);

            try {
                NullableDecimal test = NullableDecimal.MaxValue * Test1;
                nua.Fail ("#P08");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#P09", typeof (sys.OverflowException), e.GetType ());
            }

            // "-"-operator
            nua.AssertEquals ("#P10", Test1.Value - Test2.Value, (Test1 - Test2).Value);

            try {
                NullableDecimal test = NullableDecimal.MinValue - NullableDecimal.MaxValue;
                nua.Fail ("#P11");
            } catch  (sys.Exception e) {
                nua.AssertEquals ("#P12", typeof (sys.OverflowException), e.GetType ());
            }
                        
            nua.Assert("#P13", (NullableDecimal.Null + Test1).IsNull);
        }

        public void TestThanOrEqualOperators() {

            // == -operator
            nua.Assert ("#Q01", (Test2 == Test3).Value);
            nua.Assert ("#Q02", !(Test1 == Test2).Value);
            nua.Assert ("#Q03", (Test1 == NullableDecimal.Null).IsNull);
                        
            // != -operator
            nua.Assert ("#Q04", !(Test2 != Test3).Value);
            nua.Assert ("#Q05", (Test1 != Test3).Value);
            nua.Assert ("#Q06", (Test4 != Test3).Value);
            nua.Assert ("#Q07", (Test1 != NullableDecimal.Null).IsNull);

            // > -operator
            nua.Assert ("#Q08", (Test2 > Test1).Value);
            nua.Assert ("#Q09", !(Test1 > Test3).Value);
            nua.Assert ("#Q10", !(Test2 > Test3).Value);
            nua.Assert ("#Q11", (Test1 > NullableDecimal.Null).IsNull);

            // >=  -operator
            nua.Assert ("#Q12", !(Test1 >= Test3).Value);
            nua.Assert ("#Q13", (Test3 >= Test1).Value);
            nua.Assert ("#Q14", (Test2 >= Test3).Value);
            nua.Assert ("#Q15", (Test1 >= NullableDecimal.Null).IsNull);

            // < -operator
            nua.Assert ("#Q16", !(Test2 < Test1).Value);
            nua.Assert ("#Q17", (Test1 < Test3).Value);
            nua.Assert ("#Q18", !(Test2 < Test3).Value);
            nua.Assert ("#Q19", (Test1 < NullableDecimal.Null).IsNull);

            // <= -operator
            nua.Assert ("#Q20", (Test1 <= Test3).Value);
            nua.Assert ("#Q21", !(Test3 <= Test1).Value);
            nua.Assert ("#Q22", (Test2 <= Test3).Value);
            nua.Assert ("#Q23", (Test1 <= NullableDecimal.Null).IsNull);
        }

        public void TestUnaryNegation() {
            nua.AssertEquals ("#R01", -(Test2.Value), (-Test2).Value);
            nua.AssertEquals ("#R02", -(Test4.Value), (-Test4).Value);
            nua.Assert ("#R03", NullableDecimal.Null.IsNull);
        }

        public void TestNullableBooleanToNullableDecimal() {
            NullableBoolean TestBoolean = new NullableBoolean (true);
            NullableDecimal Result;

            Result = (NullableDecimal)TestBoolean;

            nua.AssertEquals ("#S01", 1m, Result.Value);

            Result = (NullableDecimal)NullableBoolean.Null;
            nua.Assert ("#S02", Result.IsNull);
            nua.AssertEquals ("#S03", NullableDecimal.Null, (NullableDecimal)NullableBoolean.Null);
        }
        
        public void TestNullableDecimalToDecimal() {
            nua.AssertEquals ("#T01", -234.254m, (decimal)Test1);
        }

        public void TestNullableDoubleToNullableDecimal() {
            NullableDouble Test = new NullableDouble (12E+10);
            nua.AssertEquals ("#U01", 120000000000m, ((NullableDecimal)Test).Value);
        }
                
        public void TestNullableSingleToNullableDecimal() {
            NullableSingle Test = new NullableSingle (1E+9);
            nua.AssertEquals ("#V01", 1000000000m, ((NullableDecimal)Test).Value);
                    
            try {
                NullableDecimal test = (NullableDecimal)NullableSingle.MaxValue;
                nua.Fail ("#V02");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#V03", typeof (sys.OverflowException), e.GetType ());
            }
        }

        public void TestNullableStringToNullableDecimal() {
            NullableString TestString = new NullableString ("Test string");
            NullableString TestString100 = new NullableString ("100");

            nua.AssertEquals ("#W01", 100m, ((NullableDecimal)TestString100).Value);

            try {
                NullableDecimal test = (NullableDecimal)TestString;
                nua.Fail ("#W02");
            } catch(sys.Exception e) {
                nua.AssertEquals ("#W03", typeof (sys.FormatException), e.GetType ());
            }
                        
            try {
                NullableDecimal test = (NullableDecimal)new NullableString("9E+100");
                nua.Fail ("#W04");
            } catch (sys.Exception e) {
                nua.AssertEquals ("#W05", typeof (sys.FormatException), e.GetType());
            }
        }

        public void TestDecimalToNullableDecimal() {
            decimal d = 1000.1m;
            nua.AssertEquals ("#X01", (NullableDecimal)1000.1m, (NullableDecimal)d);        
        }
        
        public void TestByteToNullableDecimal() {                      
            nua.AssertEquals ("#Y01", 255m, ((NullableDecimal)NullableByte.MaxValue).Value);
        }
                

        public void TestNullableIntToNullableDouble() {
            NullableInt16 Test64 = new NullableInt16(64);
            NullableInt32 Test640 = new NullableInt32(640);
            NullableInt64 Test64000 = new NullableInt64(64000);
            nua.AssertEquals ("#Z01", 64m, ((NullableDecimal)Test64).Value);
            nua.AssertEquals ("#Z02", 640m,((NullableDecimal)Test640).Value);
            nua.AssertEquals ("#Z03", 64000m, ((NullableDecimal)Test64000).Value);
        }


    }
}

