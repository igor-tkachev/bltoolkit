//
// NullableTypes.HelperFunctions.Tests.NullConvertTest
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 19-Aug-2003  Luca    Create
// 23-Aug-2003  Luca    Upgrade    New tests for methods: string From(NullableXxx, string, string).
// 15-Sep-2003  Luca    Upgrade    Code upgrade: improved tests A#204 and A#205 to make them independent 
//                                 from user regional settings
// 06-Oct-2003  Luca    Upgrade    Code upgrade: Replaced tabs with spaces
//

namespace NullableTypes.HelperFunctions.Tests
{

    using nu = NUnit.Framework;
    using nua = NUnit.Framework.Assertion;
    using sys = System;
    using sysDat = System.Data;
    using sysGlb = System.Globalization;
    using sysThr = System.Threading;
    
    [nu.TestFixture]
    public class NullConvertTest {
        string _s ;
        NullableString _ns;
        sys.DateTime _zero;

        [nu.SetUp]
        public void SetUp() {
            _s = "a non null string";
            _ns = new NullableString(_s);
            _zero = new sys.DateTime(1, 1, 1);

        }


        #region Non nullable built-in types to NullableTypes - A#

        [nu.Test]
        public void ToNullableBoolean() {
            nua.AssertEquals("TestA#01", NullableBoolean.Null, NullConvert.ToNullableBoolean(true, true));
            nua.AssertEquals("TestA#02", NullableBoolean.True, NullConvert.ToNullableBoolean(true, false));

            nua.AssertEquals("TestA#03", NullableBoolean.False, NullConvert.ToNullableBoolean(false, true));
            nua.AssertEquals("TestA#04", NullableBoolean.Null, NullConvert.ToNullableBoolean(false, false));            
        }


        [nu.Test]
        public void ToNullableBooleanFromInt32() {
            nua.AssertEquals("TestA#11", NullableBoolean.Null, NullConvert.ToNullableBoolean(int.MaxValue, int.MaxValue));
            nua.AssertEquals("TestA#12", NullableBoolean.True, NullConvert.ToNullableBoolean(1, int.MaxValue));
            nua.AssertEquals("TestA#13", NullableBoolean.False, NullConvert.ToNullableBoolean(0, int.MaxValue));

        }


        [nu.Test]
        public void ToNullableBooleanFromString() {
            nua.AssertEquals("TestA#21", NullableBoolean.Null, NullConvert.ToNullableBoolean("gino", "gino"));
            nua.AssertEquals("TestA#22", NullableBoolean.Null, NullConvert.ToNullableBoolean(null, null));

            nua.AssertEquals("TestA#23", NullableBoolean.True, NullConvert.ToNullableBoolean(bool.TrueString,   "gino"));
            nua.AssertEquals("TestA#24", NullableBoolean.True, NullConvert.ToNullableBoolean(bool.TrueString, null));
            nua.AssertEquals("TestA#25", NullableBoolean.False, NullConvert.ToNullableBoolean(bool.FalseString, "gino"));
            nua.AssertEquals("TestA#26", NullableBoolean.False, NullConvert.ToNullableBoolean(bool.FalseString, null));

        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ToNullableBooleanFromStringFormatException() {
            NullConvert.ToNullableBoolean("xxx", "gino");
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableBooleanFromStringArgumentNullException() {            
            NullConvert.ToNullableBoolean(null, string.Empty);
        }

       
        [nu.Test]
        public void ToNullableString() {
            nua.AssertEquals("TestA#31", NullableString.Null, NullConvert.ToNullableString(string.Empty, string.Empty));
            nua.AssertEquals("TestA#32", NullableString.Null, NullConvert.ToNullableString("<null>", "<null>"));
            nua.AssertEquals("TestA#33", NullableString.Null, NullConvert.ToNullableString(null, null));

            nua.AssertEquals("TestA#32", _ns, NullConvert.ToNullableString(_s,   string.Empty));
            nua.AssertEquals("TestA#33", _ns, NullConvert.ToNullableString(_s, "<null>"));
            nua.AssertEquals("TestA#34", _ns, NullConvert.ToNullableString(_s, null));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableStringArgumentNullException() {            
            NullConvert.ToNullableString(null, "n.a.");
        }


        [nu.Test]
        public void ToNullableByte() {
            nua.AssertEquals("TestA#41", NullableByte.Null, NullConvert.ToNullableByte(byte.MinValue, byte.MinValue));
            nua.AssertEquals("TestA#42", NullableByte.Null, NullConvert.ToNullableByte(125, 125));
            nua.AssertEquals("TestA#43", NullableByte.Null, NullConvert.ToNullableByte(byte.MaxValue, byte.MaxValue));

            nua.AssertEquals("TestA#44", NullableByte.MinValue, NullConvert.ToNullableByte(0, byte.MaxValue));
            nua.AssertEquals("TestA#45", new NullableByte(100), NullConvert.ToNullableByte(100, byte.MaxValue));
            nua.AssertEquals("TestA#46", NullableByte.MaxValue, NullConvert.ToNullableByte(255, byte.MinValue));
        }        


        [nu.Test]
        public void ToNullableByteFromString() {
            nua.AssertEquals("TestA#51", NullableByte.Null, NullConvert.ToNullableByte(string.Empty, string.Empty));
            nua.AssertEquals("TestA#52", NullableByte.Null, NullConvert.ToNullableByte("pino", "pino"));
            nua.AssertEquals("TestA#53", NullableByte.Null, NullConvert.ToNullableByte(null, null));

            nua.AssertEquals("TestA#54", NullableByte.MinValue, NullConvert.ToNullableByte("0", string.Empty));
            nua.AssertEquals("TestA#55", new NullableByte(100), NullConvert.ToNullableByte("100", string.Empty));
            nua.AssertEquals("TestA#56", NullableByte.MaxValue, NullConvert.ToNullableByte("255", string.Empty));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ToNullableByteFromStringFormatException() {
            NullConvert.ToNullableByte("xxx", "gino");
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableByteFromStringArgumentNullException() {            
            NullConvert.ToNullableByte(null, string.Empty);
        }


        [nu.Test]
        public void ToNullableInt16() {
            nua.AssertEquals("TestA#61", NullableInt16.Null, NullConvert.ToNullableInt16(short.MinValue, short.MinValue));
            nua.AssertEquals("TestA#62", NullableInt16.Null, NullConvert.ToNullableInt16(0, 0));
            nua.AssertEquals("TestA#63", NullableInt16.Null, NullConvert.ToNullableInt16(short.MaxValue, short.MaxValue));

            nua.AssertEquals("TestA#64", NullableInt16.MinValue, NullConvert.ToNullableInt16(short.MinValue, short.MaxValue));
            nua.AssertEquals("TestA#65", NullableInt16.Zero, NullConvert.ToNullableInt16(0, short.MaxValue));
            nua.AssertEquals("TestA#66", NullableInt16.MaxValue, NullConvert.ToNullableInt16(short.MaxValue, short.MinValue));
        }        


        [nu.Test]
        public void ToNullableInt16FromString() {
            nua.AssertEquals("TestA#71", NullableInt16.Null, NullConvert.ToNullableInt16(string.Empty, string.Empty));
            nua.AssertEquals("TestA#72", NullableInt16.Null, NullConvert.ToNullableInt16("pino", "pino"));
            nua.AssertEquals("TestA#73", NullableInt16.Null, NullConvert.ToNullableInt16(null, null));

            nua.AssertEquals("TestA#74", NullableInt16.MinValue, NullConvert.ToNullableInt16(short.MinValue.ToString(), string.Empty));
            nua.AssertEquals("TestA#75", NullableInt16.Zero, NullConvert.ToNullableInt16("0", string.Empty));
            nua.AssertEquals("TestA#76", NullableInt16.MaxValue, NullConvert.ToNullableInt16(short.MaxValue.ToString(), string.Empty));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ToNullableInt16FromStringFormatException() {
            NullConvert.ToNullableInt16("xxx", "gino");
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableInt16FromStringArgumentNullException() {            
            NullConvert.ToNullableInt16(null, string.Empty);
        }


        [nu.Test]
        public void ToNullableInt32() {
            nua.AssertEquals("TestA#81", NullableInt32.Null, NullConvert.ToNullableInt32(int.MinValue, int.MinValue));
            nua.AssertEquals("TestA#82", NullableInt32.Null, NullConvert.ToNullableInt32(0, 0));
            nua.AssertEquals("TestA#83", NullableInt32.Null, NullConvert.ToNullableInt32(int.MaxValue, int.MaxValue));

            nua.AssertEquals("TestA#84", NullableInt32.MinValue, NullConvert.ToNullableInt32(int.MinValue, int.MaxValue));
            nua.AssertEquals("TestA#85", NullableInt32.Zero, NullConvert.ToNullableInt32(0, int.MaxValue));
            nua.AssertEquals("TestA#86", NullableInt32.MaxValue, NullConvert.ToNullableInt32(int.MaxValue, int.MinValue));
        }        


        [nu.Test]
        public void ToNullableInt32FromString() {
            nua.AssertEquals("TestA#91", NullableInt32.Null, NullConvert.ToNullableInt32(string.Empty, string.Empty));
            nua.AssertEquals("TestA#92", NullableInt32.Null, NullConvert.ToNullableInt32("pino", "pino"));
            nua.AssertEquals("TestA#93", NullableInt32.Null, NullConvert.ToNullableInt32(null, null));

            nua.AssertEquals("TestA#94", NullableInt32.MinValue, NullConvert.ToNullableInt32(int.MinValue.ToString(), string.Empty));
            nua.AssertEquals("TestA#95", NullableInt32.Zero, NullConvert.ToNullableInt32("0", string.Empty));
            nua.AssertEquals("TestA#96", NullableInt32.MaxValue, NullConvert.ToNullableInt32(int.MaxValue.ToString(), string.Empty));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ToNullableInt32FromStringFormatException() {
            NullConvert.ToNullableInt32("xxx", "gino");
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableInt32FromStringArgumentNullException() {            
            NullConvert.ToNullableInt32(null, string.Empty);
        }


        [nu.Test]
        public void ToNullableInt64() {
            nua.AssertEquals("TestA#101", NullableInt64.Null, NullConvert.ToNullableInt64(long.MinValue, long.MinValue));
            nua.AssertEquals("TestA#102", NullableInt64.Null, NullConvert.ToNullableInt64(0, 0));
            nua.AssertEquals("TestA#103", NullableInt64.Null, NullConvert.ToNullableInt64(long.MaxValue, long.MaxValue));

            nua.AssertEquals("TestA#104", NullableInt64.MinValue, NullConvert.ToNullableInt64(long.MinValue, long.MaxValue));
            nua.AssertEquals("TestA#105", NullableInt64.Zero, NullConvert.ToNullableInt64(0, long.MaxValue));
            nua.AssertEquals("TestA#106", NullableInt64.MaxValue, NullConvert.ToNullableInt64(long.MaxValue, long.MinValue));
        }        


        [nu.Test]
        public void ToNullableInt64FromString() {
            nua.AssertEquals("TestA#111", NullableInt64.Null, NullConvert.ToNullableInt64(string.Empty, string.Empty));
            nua.AssertEquals("TestA#112", NullableInt64.Null, NullConvert.ToNullableInt64("pino", "pino"));
            nua.AssertEquals("TestA#113", NullableInt64.Null, NullConvert.ToNullableInt64(null, null));

            nua.AssertEquals("TestA#114", NullableInt64.MinValue, NullConvert.ToNullableInt64(long.MinValue.ToString(), string.Empty));
            nua.AssertEquals("TestA#115", NullableInt64.Zero, NullConvert.ToNullableInt64("0", string.Empty));
            nua.AssertEquals("TestA#116", NullableInt64.MaxValue, NullConvert.ToNullableInt64(long.MaxValue.ToString(), string.Empty));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ToNullableInt64FromStringFormatException() {
            NullConvert.ToNullableInt64("xxx", "gino");
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableInt64FromStringArgumentNullException() {            
            NullConvert.ToNullableInt64(null, string.Empty);
        }


        [nu.Test]
        public void ToNullableSingle() {
            nua.AssertEquals("TestA#121", NullableSingle.Null, NullConvert.ToNullableSingle(float.MinValue, float.MinValue));
            nua.AssertEquals("TestA#122", NullableSingle.Null, NullConvert.ToNullableSingle(0, 0));
            nua.AssertEquals("TestA#123", NullableSingle.Null, NullConvert.ToNullableSingle(float.Epsilon, float.Epsilon));
            nua.AssertEquals("TestA#124", NullableSingle.Null, NullConvert.ToNullableSingle(float.NaN, float.NaN));
            nua.AssertEquals("TestA#125", NullableSingle.Null, NullConvert.ToNullableSingle(float.PositiveInfinity, float.PositiveInfinity));
            nua.AssertEquals("TestA#126", NullableSingle.Null, NullConvert.ToNullableSingle(float.NegativeInfinity, float.NegativeInfinity));
            nua.AssertEquals("TestA#127", NullableSingle.Null, NullConvert.ToNullableSingle(float.MaxValue, float.MaxValue));

            nua.AssertEquals("TestA#128", NullableSingle.MinValue, NullConvert.ToNullableSingle(float.MinValue, float.MaxValue));
            nua.AssertEquals("TestA#129", NullableSingle.Zero, NullConvert.ToNullableSingle(0, float.Epsilon));
            nua.AssertEquals("TestA#130", NullableSingle.Zero, NullConvert.ToNullableSingle(0, float.NaN));
            nua.AssertEquals("TestA#131", NullableSingle.Zero, NullConvert.ToNullableSingle(0, float.PositiveInfinity));
            nua.AssertEquals("TestA#132", NullableSingle.Zero, NullConvert.ToNullableSingle(0, float.NegativeInfinity));
            nua.AssertEquals("TestA#133", NullableSingle.MaxValue, NullConvert.ToNullableSingle(float.MaxValue, float.MinValue));
        }        


        [nu.Test]
        public void ToNullableSingleFromString() {
            nua.AssertEquals("TestA#141", NullableSingle.Null, NullConvert.ToNullableSingle(string.Empty, string.Empty));
            nua.AssertEquals("TestA#142", NullableSingle.Null, NullConvert.ToNullableSingle("pino", "pino"));
            nua.AssertEquals("TestA#143", NullableSingle.Null, NullConvert.ToNullableSingle(null, null));
            nua.AssertEquals("TestA#144", NullableSingle.MinValue, NullConvert.ToNullableSingle(float.MinValue.ToString("R"), string.Empty));
            nua.AssertEquals("TestA#145", NullableSingle.Zero, NullConvert.ToNullableSingle("0", string.Empty));
            nua.AssertEquals("TestA#146", NullableSingle.Zero, NullConvert.ToNullableSingle("0", System.Globalization.NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol));
            nua.AssertEquals("TestA#147", NullableSingle.Zero, NullConvert.ToNullableSingle("0", System.Globalization.NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol));
            nua.AssertEquals("TestA#148", NullableSingle.Zero, NullConvert.ToNullableSingle("0", System.Globalization.NumberFormatInfo.CurrentInfo.NaNSymbol));
            nua.AssertEquals("TestA#149", NullableSingle.MaxValue, NullConvert.ToNullableSingle(float.MaxValue.ToString("R"), string.Empty));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ToNullableSingleFromStringFormatException() {
            NullConvert.ToNullableSingle("xxx", "gino");
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableSingleFromStringArgumentNullException() {            
            NullConvert.ToNullableSingle(null, string.Empty);
        }

        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void ToNullableSingleFromStringOverflowException() {            
            NullConvert.ToNullableSingle(double.MaxValue.ToString("R"), string.Empty);
        }

        [nu.Test]
        public void ToNullableDouble() {
            nua.AssertEquals("TestA#121", NullableDouble.Null, NullConvert.ToNullableDouble(double.MinValue, double.MinValue));
            nua.AssertEquals("TestA#122", NullableDouble.Null, NullConvert.ToNullableDouble(0, 0));
            nua.AssertEquals("TestA#123", NullableDouble.Null, NullConvert.ToNullableDouble(double.Epsilon, double.Epsilon));
            nua.AssertEquals("TestA#124", NullableDouble.Null, NullConvert.ToNullableDouble(double.NaN, double.NaN));
            nua.AssertEquals("TestA#125", NullableDouble.Null, NullConvert.ToNullableDouble(double.PositiveInfinity, double.PositiveInfinity));
            nua.AssertEquals("TestA#126", NullableDouble.Null, NullConvert.ToNullableDouble(double.NegativeInfinity, double.NegativeInfinity));
            nua.AssertEquals("TestA#127", NullableDouble.Null, NullConvert.ToNullableDouble(double.MaxValue, double.MaxValue));

            nua.AssertEquals("TestA#128", NullableDouble.MinValue, NullConvert.ToNullableDouble(double.MinValue, double.MaxValue));
            nua.AssertEquals("TestA#129", NullableDouble.Zero, NullConvert.ToNullableDouble(0, double.Epsilon));
            nua.AssertEquals("TestA#130", NullableDouble.Zero, NullConvert.ToNullableDouble(0, double.NaN));
            nua.AssertEquals("TestA#131", NullableDouble.Zero, NullConvert.ToNullableDouble(0, double.PositiveInfinity));
            nua.AssertEquals("TestA#132", NullableDouble.Zero, NullConvert.ToNullableDouble(0, double.NegativeInfinity));
            nua.AssertEquals("TestA#133", NullableDouble.MaxValue, NullConvert.ToNullableDouble(double.MaxValue, double.MinValue));
        }        


        [nu.Test]
        public void ToNullableDoubleFromString() {
            nua.AssertEquals("TestA#141", NullableDouble.Null, NullConvert.ToNullableDouble(string.Empty, string.Empty));
            nua.AssertEquals("TestA#142", NullableDouble.Null, NullConvert.ToNullableDouble("pino", "pino"));
            nua.AssertEquals("TestA#143", NullableDouble.Null, NullConvert.ToNullableDouble(null, null));
            nua.AssertEquals("TestA#144", NullableDouble.MinValue, NullConvert.ToNullableDouble(double.MinValue.ToString("R"), string.Empty));
            nua.AssertEquals("TestA#145", NullableDouble.Zero, NullConvert.ToNullableDouble("0", string.Empty));
            nua.AssertEquals("TestA#146", NullableDouble.Zero, NullConvert.ToNullableDouble("0", System.Globalization.NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol));
            nua.AssertEquals("TestA#147", NullableDouble.Zero, NullConvert.ToNullableDouble("0", System.Globalization.NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol));
            nua.AssertEquals("TestA#148", NullableDouble.Zero, NullConvert.ToNullableDouble("0", System.Globalization.NumberFormatInfo.CurrentInfo.NaNSymbol));
            nua.AssertEquals("TestA#149", NullableDouble.MaxValue, NullConvert.ToNullableDouble(double.MaxValue.ToString("R"), string.Empty));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ToNullableDoubleFromStringFormatException() {
            NullConvert.ToNullableDouble("xxx", "gino");
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableDoubleFromStringArgumentNullException() {            
            NullConvert.ToNullableDouble(null, string.Empty);
        }

        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void ToNullableDoubleFromStringOverflowException() {            
            // Note: ToString("G") round up to a number bigger then MaxValue
            NullConvert.ToNullableDouble(double.MaxValue.ToString("G"), string.Empty);
        }


        [nu.Test]
        public void ToNullableDecimal() {
            nua.AssertEquals("TestA#161", NullableDecimal.Null, NullConvert.ToNullableDecimal(decimal.MinValue, decimal.MinValue));
            nua.AssertEquals("TestA#162", NullableDecimal.Null, NullConvert.ToNullableDecimal(0, 0));
            nua.AssertEquals("TestA#163", NullableDecimal.Null, NullConvert.ToNullableDecimal(decimal.MaxValue, decimal.MaxValue));

            nua.AssertEquals("TestA#164", NullableDecimal.MinValue, NullConvert.ToNullableDecimal(decimal.MinValue, decimal.MaxValue));
            nua.AssertEquals("TestA#165", NullableDecimal.MaxValue, NullConvert.ToNullableDecimal(decimal.MaxValue, decimal.MinValue));
        }        


        [nu.Test]
        public void ToNullableDecimalFromString() {
            nua.AssertEquals("TestA#181", NullableDecimal.Null, NullConvert.ToNullableDecimal(string.Empty, string.Empty));
            nua.AssertEquals("TestA#182", NullableDecimal.Null, NullConvert.ToNullableDecimal("pino", "pino"));
            nua.AssertEquals("TestA#183", NullableDecimal.Null, NullConvert.ToNullableDecimal(null, null));
            nua.AssertEquals("TestA#184", NullableDecimal.MinValue, NullConvert.ToNullableDecimal(decimal.MinValue.ToString(), string.Empty));
            nua.AssertEquals("TestA#185", NullableDecimal.Zero, NullConvert.ToNullableDecimal("0", string.Empty));
            nua.AssertEquals("TestA#186", NullableDecimal.MaxValue, NullConvert.ToNullableDecimal(decimal.MaxValue.ToString(), string.Empty));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ToNullableDecimalFromStringFormatException() {
            NullConvert.ToNullableDecimal("xxx", "gino");
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableDecimalFromStringArgumentNullException() {            
            NullConvert.ToNullableDecimal(null, string.Empty);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void ToNullableDecimalFromStringOverflowException() {            
            NullConvert.ToNullableDecimal("9" + (decimal.MaxValue).ToString(), string.Empty);
        }



        [nu.Test]
        public void ToNullableDateTime() {
            nua.AssertEquals("TestA#191", NullableDateTime.Null, NullConvert.ToNullableDateTime(sys.DateTime.MinValue, sys.DateTime.MinValue));
            nua.AssertEquals("TestA#192", NullableDateTime.Null, NullConvert.ToNullableDateTime(_zero, _zero));
            nua.AssertEquals("TestA#193", NullableDateTime.Null, NullConvert.ToNullableDateTime(sys.DateTime.MaxValue, sys.DateTime.MaxValue));

            nua.AssertEquals("TestA#194", NullableDateTime.MinValue, NullConvert.ToNullableDateTime(sys.DateTime.MinValue, sys.DateTime.MaxValue));
            nua.AssertEquals("TestA#195", NullableDateTime.MaxValue, NullConvert.ToNullableDateTime(sys.DateTime.MaxValue, sys.DateTime.MinValue));
        }        


        [nu.Test]
        public void ToNullableDateTimeFromString() {
            nua.AssertEquals("TestA#201", NullableDateTime.Null, NullConvert.ToNullableDateTime(string.Empty, string.Empty));
            nua.AssertEquals("TestA#202", NullableDateTime.Null, NullConvert.ToNullableDateTime("pino", "pino"));
            nua.AssertEquals("TestA#203", NullableDateTime.Null, NullConvert.ToNullableDateTime(null, null));

            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;
//                sys.Console.WriteLine();
//                sys.Console.WriteLine("Extra check");
//                sys.Console.WriteLine("at NullableTypes.HelperFunctions.Tests.NullConvertTest.ToNullableDateTimeFromString()");
//                sys.Console.WriteLine("{0} {1}  -  {2} {3}", 
//                    sysGlb.DateTimeFormatInfo.CurrentInfo.ShortDatePattern,
//                    sysGlb.DateTimeFormatInfo.CurrentInfo.ShortTimePattern,
//                    sysGlb.DateTimeFormatInfo.CurrentInfo.LongDatePattern,
//                    sysGlb.DateTimeFormatInfo.CurrentInfo.LongTimePattern);
//                sys.Console.WriteLine("for TestA#204");
                string fmt = "yyyy/MMM/dd HH:mm:ss.fffffff";
//                sys.Console.WriteLine("{0}\t]{1}[", NullableDateTime.MinValue.Value.Ticks, NullableDateTime.MinValue.Value.ToString(fmt));
//                sys.Console.WriteLine("{0}\t]{1}[", NullConvert.ToNullableDateTime(sys.DateTime.MinValue.ToString(fmt), string.Empty).Value.Ticks, NullConvert.ToNullableDateTime(sys.DateTime.MinValue.ToString(fmt), string.Empty).Value.ToString(fmt));
                nua.AssertEquals("TestA#204", NullableDateTime.MinValue, NullConvert.ToNullableDateTime(sys.DateTime.MinValue.ToString(fmt), string.Empty));
//                sys.Console.WriteLine("for TestA#205");
//                sys.Console.WriteLine("{0}\t]{1}[", NullableDateTime.MaxValue.Value.Ticks, NullableDateTime.MaxValue.Value.ToString(fmt));
//                sys.Console.WriteLine("{0}\t]{1}[", NullConvert.ToNullableDateTime(sys.DateTime.MaxValue.ToString(fmt), string.Empty).Value.Ticks, NullConvert.ToNullableDateTime(sys.DateTime.MaxValue.ToString(fmt), string.Empty).Value.ToString(fmt));
                nua.AssertEquals("TestA#205", NullableDateTime.MaxValue, NullConvert.ToNullableDateTime(sys.DateTime.MaxValue.ToString(fmt), string.Empty));
            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ToNullableDateTimeFromStringFormatException() {
            NullConvert.ToNullableDateTime("xxx", "gino");
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableDateTimeFromStringArgumentNullException() {            
            NullConvert.ToNullableDateTime(null, string.Empty);
        }

        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void ToNullableDateTimeFromStringOverflowException() {            
            NullConvert.ToNullableDateTime("9" + sys.DateTime.MaxValue.ToString("yyyy-MMM-dd"), string.Empty);
        }
        #endregion // Non nullable built-in types to NullableTypes

        #region NullableTypes to non nullable built-in types - B#

        [nu.Test]
        public void FromNullableBoolean() {
            nua.AssertEquals("TestB#001", true, NullConvert.From(NullableBoolean.Null, true));
            nua.AssertEquals("TestB#002", false, NullConvert.From(NullableBoolean.Null, false));

            nua.AssertEquals("TestB#003", true, NullConvert.From(NullableBoolean.True, true));
            nua.AssertEquals("TestB#004", true, NullConvert.From(NullableBoolean.True, false));

            nua.AssertEquals("TestB#005", false, NullConvert.From(NullableBoolean.False, true));
            nua.AssertEquals("TestB#006", false, NullConvert.From(NullableBoolean.False, false));
        }


        [nu.Test]
        public void FromNullableString() {
            nua.AssertEquals("TestB#011", string.Empty, NullConvert.From(NullableString.Null, string.Empty));
            nua.AssertEquals("TestB#012", "<null>", NullConvert.From(NullableString.Null, "<null>"));
            nua.AssertEquals("TestB#013", null, NullConvert.From(NullableString.Null, null));

            nua.AssertEquals("TestB#014", _s, NullConvert.From(_ns, string.Empty));
            nua.AssertEquals("TestB#015", _s, NullConvert.From(_ns, "<null>"));
            nua.AssertEquals("TestB#016", _s, NullConvert.From(_ns, null));

        }


        [nu.Test]
        public void FromNullableByte() {
            nua.AssertEquals("TestB#021", byte.MinValue, NullConvert.From(NullableByte.Null, byte.MinValue));
            nua.AssertEquals("TestB#022", 100, NullConvert.From(NullableByte.Null, (byte)100));
            nua.AssertEquals("TestB#023", byte.MaxValue, NullConvert.From(NullableByte.Null, byte.MaxValue));

            nua.AssertEquals("TestB#024", byte.MinValue, NullConvert.From(NullableByte.MinValue, (byte)99));
            nua.AssertEquals("TestB#025", 125 , NullConvert.From(new NullableByte(125), (byte)99));
            nua.AssertEquals("TestB#026", byte.MaxValue, NullConvert.From(NullableByte.MaxValue, (byte)99));
        }


        [nu.Test]
        public void FromNullableInt16() {
            nua.AssertEquals("TestB#031", short.MinValue, NullConvert.From(NullableInt16.Null, short.MinValue));
            nua.AssertEquals("TestB#032", 100, NullConvert.From(NullableInt16.Null, (short)100));
            nua.AssertEquals("TestB#033", short.MaxValue, NullConvert.From(NullableInt16.Null, short.MaxValue));

            nua.AssertEquals("TestB#034", short.MinValue, NullConvert.From(NullableInt16.MinValue, (short)99));
            nua.AssertEquals("TestB#035", 125 , NullConvert.From(new NullableInt16(125), (short)99));
            nua.AssertEquals("TestB#036", short.MaxValue, NullConvert.From(NullableInt16.MaxValue, (short)99));
        }


        [nu.Test]
        public void FromNullableInt32() {
            nua.AssertEquals("TestB#031", int.MinValue, NullConvert.From(NullableInt32.Null, int.MinValue));
            nua.AssertEquals("TestB#032", 0, NullConvert.From(NullableInt32.Null, 0));
            nua.AssertEquals("TestB#033", int.MaxValue, NullConvert.From(NullableInt32.Null, int.MaxValue));

            nua.AssertEquals("TestB#034", int.MinValue, NullConvert.From(NullableInt32.MinValue, (int)99));
            nua.AssertEquals("TestB#035", 0 , NullConvert.From(NullableInt32.Zero, (int)99));
            nua.AssertEquals("TestB#036", int.MaxValue, NullConvert.From(NullableInt32.MaxValue, (int)99));
        }


        [nu.Test]
        public void FromNullableInt64() {
            nua.AssertEquals("TestB#031", long.MinValue, NullConvert.From(NullableInt64.Null, long.MinValue));
            nua.AssertEquals("TestB#032", 0, NullConvert.From(NullableInt64.Null, 0));
            nua.AssertEquals("TestB#033", long.MaxValue, NullConvert.From(NullableInt64.Null, long.MaxValue));

            nua.AssertEquals("TestB#034", long.MinValue, NullConvert.From(NullableInt64.MinValue, (long)99));
            nua.AssertEquals("TestB#035", 0 , NullConvert.From(NullableInt64.Zero, (long)99));
            nua.AssertEquals("TestB#036", long.MaxValue, NullConvert.From(NullableInt64.MaxValue, (long)99));
        }


        [nu.Test]
        public void FromNullableSingle() {
            nua.AssertEquals("TestB#041", float.MinValue, NullConvert.From(NullableSingle.Null, float.MinValue));
            nua.AssertEquals("TestB#042", 0, NullConvert.From(NullableSingle.Null, 0));
            nua.AssertEquals("TestB#043", float.Epsilon, NullConvert.From(NullableSingle.Null, float.Epsilon));
            nua.AssertEquals("TestB#044", float.NaN, NullConvert.From(NullableSingle.Null, float.NaN));
            nua.AssertEquals("TestB#045", float.NegativeInfinity, NullConvert.From(NullableSingle.Null, float.NegativeInfinity));
            nua.AssertEquals("TestB#046", float.PositiveInfinity, NullConvert.From(NullableSingle.Null, float.PositiveInfinity));
            nua.AssertEquals("TestB#047", float.MaxValue, NullConvert.From(NullableSingle.Null, float.MaxValue));

            nua.AssertEquals("TestB#048", float.MinValue, NullConvert.From(NullableSingle.MinValue, 99F));
            nua.AssertEquals("TestB#049", 0, NullConvert.From(NullableSingle.Zero, 99F));
            nua.AssertEquals("TestB#050", float.Epsilon, NullConvert.From(new NullableSingle(float.Epsilon), 99F));
            nua.AssertEquals("TestB#051", float.NaN, NullConvert.From(new NullableSingle(float.NaN), 99F));
            nua.AssertEquals("TestB#052", float.PositiveInfinity, NullConvert.From(new NullableSingle(float.PositiveInfinity), 99F));
            nua.AssertEquals("TestB#053", float.NegativeInfinity, NullConvert.From(new NullableSingle(float.NegativeInfinity), 99F));
            nua.AssertEquals("TestB#054", float.MaxValue, NullConvert.From(NullableSingle.MaxValue, 99F));
        }

        [nu.Test]
        public void FromNullableDouble() {
            nua.AssertEquals("TestB#061", double.MinValue, NullConvert.From(NullableDouble.Null, double.MinValue));
            nua.AssertEquals("TestB#062", 0, NullConvert.From(NullableDouble.Null, 0));
            nua.AssertEquals("TestB#063", double.Epsilon, NullConvert.From(NullableDouble.Null, double.Epsilon));
            nua.AssertEquals("TestB#064", double.NaN, NullConvert.From(NullableDouble.Null, double.NaN));
            nua.AssertEquals("TestB#065", double.NegativeInfinity, NullConvert.From(NullableDouble.Null, double.NegativeInfinity));
            nua.AssertEquals("TestB#066", double.PositiveInfinity, NullConvert.From(NullableDouble.Null, double.PositiveInfinity));
            nua.AssertEquals("TestB#067", double.MaxValue, NullConvert.From(NullableDouble.Null, double.MaxValue));

            nua.AssertEquals("TestB#068", double.MinValue, NullConvert.From(NullableDouble.MinValue, 99F));
            nua.AssertEquals("TestB#069", 0, NullConvert.From(NullableDouble.Zero, 99F));
            nua.AssertEquals("TestB#070", double.Epsilon, NullConvert.From(new NullableDouble(double.Epsilon), 99F));
            nua.AssertEquals("TestB#071", double.NaN, NullConvert.From(new NullableDouble(double.NaN), 99F));
            nua.AssertEquals("TestB#072", double.PositiveInfinity, NullConvert.From(new NullableDouble(double.PositiveInfinity), 99F));
            nua.AssertEquals("TestB#073", double.NegativeInfinity, NullConvert.From(new NullableDouble(double.NegativeInfinity), 99F));
            nua.AssertEquals("TestB#074", double.MaxValue, NullConvert.From(NullableDouble.MaxValue, 99F));
        }

        [nu.Test]
        public void FromNullableDecimal() {
            nua.AssertEquals("TestB#081", decimal.MinValue, NullConvert.From(NullableDecimal.Null, decimal.MinValue));
            nua.AssertEquals("TestB#082", 0, NullConvert.From(NullableDecimal.Null, 0));
            nua.AssertEquals("TestB#083", decimal.MaxValue, NullConvert.From(NullableDecimal.Null, decimal.MaxValue));

            nua.AssertEquals("TestB#084", decimal.MinValue, NullConvert.From(NullableDecimal.MinValue, 99M));
            nua.AssertEquals("TestB#085", 0, NullConvert.From(NullableDecimal.Zero, 99M));
            nua.AssertEquals("TestB#086", decimal.MaxValue, NullConvert.From(NullableDecimal.MaxValue, 99M));
        }

        [nu.Test]
        public void FromNullableDateTime() {
            nua.AssertEquals("TestB#091", sys.DateTime.MinValue, NullConvert.From(NullableDateTime.Null, sys.DateTime.MinValue));
            nua.AssertEquals("TestB#092", _zero, NullConvert.From(NullableDateTime.Null, _zero));
            nua.AssertEquals("TestB#093", sys.DateTime.MaxValue, NullConvert.From(NullableDateTime.Null, sys.DateTime.MaxValue));

            sys.DateTime one = new sys.DateTime(1111, 11, 11, 11, 11, 11, 11);
            nua.AssertEquals("TestB#094", sys.DateTime.MinValue, NullConvert.From(NullableDateTime.MinValue, one));
            nua.AssertEquals("TestB#095", _zero, NullConvert.From(_zero, one));
            nua.AssertEquals("TestB#096", sys.DateTime.MaxValue, NullConvert.From(NullableDateTime.MaxValue, one));
        }

        #endregion // NullableTypes to non nullable built-in types

        #region NullableTypes to string representation - C#
        [nu.Test]
        public void FromNullableBooleanToString() {
            nua.AssertEquals("TestC#001", null, NullConvert.From(NullableBoolean.Null, null));
            nua.AssertEquals("TestC#002", string.Empty, NullConvert.From(NullableBoolean.Null, string.Empty));
            nua.AssertEquals("TestC#003", "unknown", NullConvert.From(NullableBoolean.Null, "unknown"));

            nua.AssertEquals("TestC#004", bool.TrueString, NullConvert.From(NullableBoolean.True, "unknown"));
            nua.AssertEquals("TestC#005", bool.FalseString, NullConvert.From(NullableBoolean.False, "unknown"));
        }


        [nu.Test]
        public void FromNullableByteToString() {
            nua.AssertEquals("TestC#021", null, NullConvert.From(NullableByte.Null, null, null));
            nua.AssertEquals("TestC#022", string.Empty, NullConvert.From(NullableByte.Null,string.Empty, null));
            nua.AssertEquals("TestC#023", "unknown", NullConvert.From(NullableByte.Null, "unknown", null));

            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                nua.AssertEquals("TestC#024", "¤125.00", NullConvert.From(new NullableByte(125), string.Empty, "C"));
                nua.AssertEquals("TestC#025", "125.00", NullConvert.From(new NullableByte(125), string.Empty, "F"));
                nua.AssertEquals("TestC#026", "7D", NullConvert.From(new NullableByte(125), string.Empty, "X"));


            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

            // invalid format specified
            nua.AssertEquals("TestC#030", " ", NullConvert.From(new NullableByte(125), "null", " "));
            nua.AssertEquals("TestC#031", "not a valid format!!!", NullConvert.From(new NullableByte(125), "null", "not a valid format!!!"));

        }


        [nu.Test, nu.ExpectedException(typeof(System.FormatException))]
        public void FromNullableByteToStringFormatException() {
            NullConvert.From(new NullableByte(1), "null", "R");
        }


        [nu.Test]
        public void FromNullableInt16ToString() {
            nua.AssertEquals("TestC#041", null, NullConvert.From(NullableInt16.Null, null, null));
            nua.AssertEquals("TestC#042", string.Empty, NullConvert.From(NullableInt16.Null,string.Empty, null));
            nua.AssertEquals("TestC#043", "unknown", NullConvert.From(NullableInt16.Null, "unknown", null));

            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                nua.AssertEquals("TestC#044", "(¤32,768.00)", 
                    NullConvert.From(new NullableInt16(-32768), string.Empty, "C"));
                nua.AssertEquals("TestC#045", "-32768.00", 
                    NullConvert.From(new NullableInt16(-32768), string.Empty, "F"));
                nua.AssertEquals("TestC#046", "8000", 
                    NullConvert.From(new NullableInt16(-32768), string.Empty, "X"));

            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

            // invalid format specified
            nua.AssertEquals("TestC#050", " ", NullConvert.From(new NullableInt16(1), "null", " "));
            nua.AssertEquals("TestC#051", "not a valid format!!!", NullConvert.From(new NullableInt16(1), "null", "not a valid format!!!"));

        }


        [nu.Test, nu.ExpectedException(typeof(System.FormatException))]
        public void FromNullableInt16ToStringFormatException() {
            NullConvert.From(new NullableInt16(1), "null", "R");
        }


        [nu.Test]
        public void FromNullableInt32ToString() {
            nua.AssertEquals("TestC#061", null, NullConvert.From(NullableInt32.Null, null, null));
            nua.AssertEquals("TestC#062", string.Empty, NullConvert.From(NullableInt32.Null,string.Empty, null));
            nua.AssertEquals("TestC#063", "unknown", NullConvert.From(NullableInt32.Null, "unknown", null));

            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                nua.AssertEquals("TestC#064", "(¤2,147,483,648.00)", 
                    NullConvert.From(new NullableInt32(-2147483648), string.Empty, "C"));
                nua.AssertEquals("TestC#065", "-2147483648.00", 
                    NullConvert.From(new NullableInt32(-2147483648), string.Empty, "F"));
                nua.AssertEquals("TestC#066", "80000000", 
                    NullConvert.From(new NullableInt32(-2147483648), string.Empty, "X"));

            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

            // invalid format specified
            nua.AssertEquals("TestC#070", " ", 
                NullConvert.From(new NullableInt32(1), "null", " "));
            nua.AssertEquals("TestC#071", "not a valid format!!!", 
                NullConvert.From(new NullableInt32(1), "null", "not a valid format!!!"));

        }


        [nu.Test, nu.ExpectedException(typeof(System.FormatException))]
        public void FromNullableInt32ToStringFormatException() {
            NullConvert.From(new NullableInt32(1), "null", "R");
        }


        [nu.Test]
        public void FromNullableInt64ToString() {
            nua.AssertEquals("TestC#081", null, NullConvert.From(NullableInt64.Null, null, null));
            nua.AssertEquals("TestC#082", string.Empty, NullConvert.From(NullableInt64.Null,string.Empty, null));
            nua.AssertEquals("TestC#083", "unknown", NullConvert.From(NullableInt64.Null, "unknown", null));

            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                nua.AssertEquals("TestC#084", "(¤9,223,372,036,854,775,808.00)", 
                    NullConvert.From(new NullableInt64(-9223372036854775808), string.Empty, "C"));
                nua.AssertEquals("TestC#085", "-9223372036854775808.00", 
                    NullConvert.From(new NullableInt64(-9223372036854775808), string.Empty, "F"));
                nua.AssertEquals("TestC#086", "8000000000000000", 
                    NullConvert.From(new NullableInt64(-9223372036854775808), string.Empty, "X"));

            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

            // invalid format specified
            nua.AssertEquals("TestC#090", " ", 
                NullConvert.From(new NullableInt64(1), "null", " "));
            nua.AssertEquals("TestC#091", "not a valid format!!!", 
                NullConvert.From(new NullableInt64(1), "null", "not a valid format!!!"));

        }


        [nu.Test, nu.ExpectedException(typeof(System.FormatException))]
        public void FromNullableInt64ToStringFormatException() {
            NullConvert.From(new NullableInt64(1), "null", "R");
        }


        [nu.Test]
        public void FromNullableSingleToString() {
            nua.AssertEquals("TestC#101", null, NullConvert.From(NullableSingle.Null, null, null));
            nua.AssertEquals("TestC#102", string.Empty, NullConvert.From(NullableSingle.Null,string.Empty, null));
            nua.AssertEquals("TestC#103", "unknown", NullConvert.From(NullableSingle.Null, "unknown", null));

            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                nua.AssertEquals("TestC#104", "(¤340,282,300,000,000,000,000,000,000,000,000,000,000.00)", 
                    NullConvert.From(new NullableSingle(-3.402823e38), string.Empty, "C"));
                nua.AssertEquals("TestC#105", "-340282300000000000000000000000000000000.00", 
                    NullConvert.From(new NullableSingle(-3.402823e38), string.Empty, "F"));
                nua.AssertEquals("TestC#106", "-3.402823E+038", 
                    NullConvert.From(new NullableSingle(-3.402823e38), string.Empty, "E"));

                nua.AssertEquals("TestC#107", "NaN", 
                    NullConvert.From(new NullableSingle(float.NaN), string.Empty, "G"));

            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

            // invalid format specified
            nua.AssertEquals("TestC#112", " ", 
                NullConvert.From(new NullableSingle(1), "null", " "));
            nua.AssertEquals("TestC#113", "not a valid format!!!", 
                NullConvert.From(new NullableSingle(1), "null", "not a valid format!!!"));

        }


        [nu.Test, nu.ExpectedException(typeof(System.FormatException))]
        public void FromNullableSingleToStringFormatException() {
            NullConvert.From(new NullableSingle(1), "null", "X");
        }


        [nu.Test]
        public void FromNullableDoubleToString() {
            nua.AssertEquals("TestC#121", null, NullConvert.From(NullableDouble.Null, null, null));
            nua.AssertEquals("TestC#122", string.Empty, NullConvert.From(NullableDouble.Null,string.Empty, null));
            nua.AssertEquals("TestC#123", "unknown", NullConvert.From(NullableDouble.Null, "unknown", null));

            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                nua.AssertEquals("TestC#124", "(¤940,282,300,000,000,000,000,000,000,000,000,000,000.00)", 
                    NullConvert.From(new NullableDouble(-9.402823e38), string.Empty, "C"));
                nua.AssertEquals("TestC#125", "-940282300000000000000000000000000000000.00", 
                    NullConvert.From(new NullableDouble(-9.402823e38), string.Empty, "F"));
                nua.AssertEquals("TestC#126", "-9.402823E+038", 
                    NullConvert.From(new NullableDouble(-9.402823e38), string.Empty, "E"));

                nua.AssertEquals("TestC#127", "-Infinity", 
                    NullConvert.From(new NullableDouble(float.NegativeInfinity), string.Empty, "G"));

            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

            // invalid format specified
            nua.AssertEquals("TestC#132", " ", 
                NullConvert.From(new NullableDouble(1), "null", " "));
            nua.AssertEquals("TestC#133", "not a valid format!!!", 
                NullConvert.From(new NullableDouble(1), "null", "not a valid format!!!"));

        }


        [nu.Test, nu.ExpectedException(typeof(System.FormatException))]
        public void FromNullableDoubleToStringFormatException() {
            NullConvert.From(new NullableDouble(1), "null", "X");
        }


        [nu.Test]
        public void FromNullableDecimalToString() {
            nua.AssertEquals("TestC#141", null, NullConvert.From(NullableDecimal.Null, null, null));
            nua.AssertEquals("TestC#142", string.Empty, NullConvert.From(NullableDecimal.Null,string.Empty, null));
            nua.AssertEquals("TestC#143", "unknown", NullConvert.From(NullableDecimal.Null, "unknown", null));

            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                nua.AssertEquals("TestC#144", "(¤79,228,162,514,264,337,593,543,950,335.00)", 
                    NullConvert.From(new NullableDecimal(-79228162514264337593543950335M), string.Empty, "C"));
                nua.AssertEquals("TestC#145", "-79228162514264337593543950335.00", 
                    NullConvert.From(new NullableDecimal(-79228162514264337593543950335M), string.Empty, "F"));
                nua.AssertEquals("TestC#146", "-79,228,162,514,264,337,593,543,950,335.00", 
                    NullConvert.From(new NullableDecimal(-79228162514264337593543950335M), string.Empty, "N"));

            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

            // invalid format specified
            nua.AssertEquals("TestC#152", " ", 
                NullConvert.From(new NullableDecimal(1), "null", " "));
            nua.AssertEquals("TestC#153", "not a valid format!!!", 
                NullConvert.From(new NullableDecimal(1), "null", "not a valid format!!!"));

        }


        [nu.Test, nu.ExpectedException(typeof(System.FormatException))]
        public void FromNullableDecimalToStringFormatException() {
            NullConvert.From(new NullableDecimal(1), "null", "R");
        }

        [nu.Test]
        public void FromNullableDateTimeToString() {
            nua.AssertEquals("TestC#161", null, NullConvert.From(NullableDateTime.Null, null, null));
            nua.AssertEquals("TestC#162", string.Empty, NullConvert.From(NullableDateTime.Null,string.Empty, null));
            nua.AssertEquals("TestC#163", "unknown", NullConvert.From(NullableDateTime.Null, "unknown", null));

            NullableDateTime d = new NullableDateTime(2003, 8, 22, 14, 41, 23, 234);

            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                nua.AssertEquals("TestC#164", "08/22/2003", NullConvert.From(d, string.Empty, "d"));
                nua.AssertEquals("TestC#165", "14:41", NullConvert.From(d, string.Empty, "t"));
                nua.AssertEquals("TestC#166", "2003-08-22 14:41:23Z", NullConvert.From(d, string.Empty, "u"));

            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }

            // invalid format specified
            nua.AssertEquals("TestC#171", "nooooooooooooo!!!", NullConvert.From(d, "null", "nooooooooooooo!!!"));

        }


        [nu.Test, nu.ExpectedException(typeof(System.FormatException))]
        public void FromNullableDateTimeToStringFormatException() {
            NullConvert.From(_zero, "null", " ");
        }
        #endregion // NullableTypes to string representation - C#
    }
}
