//
// NullableTypes.Tests.NullableDecimalTest
// 
// Authors: Bill Hwang   (mrsolo@users.sourceforge.net)
//          Luca Minudel (lukadotnet@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 22-Apr-2003  Bill    Create     Copy from NullableDouble
// 12-Giu-2003  Luca    Upgrade    Adapted to NullableDecimal
// 23-Aug-2003  Luca    Bug Fix    Replaced CultureInfo EN-us with InvariantCulture that do not change wher user
//                                 customize international settings
// 13-Sep-2003  Luca    Upgrade    New test: SerializableAttribute
// 05-Oct-2003  DamienG Upgrade    New test: XmlSerializable
//                                 New test: Added additional tests to SerializableAttribute
//                                 Code upgrade: Changed _pos and _neg to be decimals, added _negDec
//                                 Code upgrade: Added min and max for serialize tests to ensure full 
//                                 range persists
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
    using sysGlb = System.Globalization;
    using sysXml = System.Xml;
    using sysXmlScm = System.Xml.Schema;
    
    [nu.TestFixture]
    public class NullableDecimalTest {

        NullableDecimal _pos;
        NullableDecimal _neg;
        NullableDecimal _zero;
        NullableDecimal _null;
        NullableDecimal _notNull; 
        NullableDecimal _negDec;
        NullableDecimal _min;
        NullableDecimal _max;

        [nu.SetUp]
        public void SetUp() {
            _pos = new NullableDecimal(246.789);
            _neg = new NullableDecimal(-9823.61);
            _zero = new NullableDecimal(0);
            _null = NullableDecimal.Null;
            _notNull = new NullableDecimal(90356);
            _negDec = new NullableDecimal(-0.0001);
            _min = NullableDecimal.MinValue;
            _max = NullableDecimal.MaxValue;
        }


        #region Field Tests - A#
        [nu.Test]
        public void MaxValue() {
            nua.AssertEquals("TestA#01", decimal.MaxValue, NullableDecimal.MaxValue.Value);
        }


        [nu.Test]
        public void MinValue() {
            nua.AssertEquals("TestA#02", decimal.MinValue, NullableDecimal.MinValue.Value);
        }


        [nu.Test]
        public void Zero() {
            nua.AssertEquals("TestA#03", decimal.Zero, NullableDecimal.Zero.Value);
        }


        [nu.Test]
        public void Null() {
            nua.Assert ("TestA#04", NullableDecimal.Null.IsNull);
        }


        [nu.Test]
        public void One() {
            nua.AssertEquals("TestA#05", decimal.One, NullableDecimal.One.Value);
        }


        [nu.Test]
        public void MinusOne() {
            nua.AssertEquals("TestA#06", decimal.MinusOne, NullableDecimal.MinusOne.Value);
        }
        #endregion // Field Tests - A#

        #region Constructor Tests - B#
        [nu.Test]
        public void Create() {
            nua.AssertEquals("TestB#01", decimal.MinValue , new NullableDecimal(decimal.MinValue).Value);
            nua.AssertEquals("TestB#02", decimal.Zero, new NullableDecimal(decimal.Zero).Value);
            nua.AssertEquals("TestB#03", decimal.MaxValue , new NullableDecimal(decimal.MaxValue).Value);
        }


        [nu.Test]
        public void CreateFromDouble() {
            double hi = (double)decimal.MaxValue / 2;
            nua.AssertEquals("TestB#10", (decimal)hi, new NullableDecimal(hi).Value); 

            double low = (double)decimal.MinValue / 2;
            nua.AssertEquals("TestB#11", (decimal)low, new NullableDecimal(low).Value); 
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void CreateFromDoubleMaxOverflowException() {
            double d = 2d * (double)decimal.MaxValue;
            new NullableDecimal(d);
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void CreateFromDoubleMinOverflowException() {
            double d = 2d * (double)decimal.MinValue;
            new NullableDecimal(d);
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void CreateFromDoubleNaNOverflowException() {            
            new NullableDecimal(double.NaN);
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void CreateFromDoublePosInfOverflowException() {            
            new NullableDecimal(double.PositiveInfinity);
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void CreateFromDoubleNegInfOverflowException() {            
            new NullableDecimal(double.NegativeInfinity);
        }


        [nu.Test]
        public void CreateFromInt() {
            nua.AssertEquals("TestB#21", (decimal)int.MinValue, new NullableDecimal(int.MinValue).Value); 
            nua.AssertEquals("TestB#22", (decimal)int.MaxValue, new NullableDecimal(int.MaxValue).Value); 
        }

        
        [nu.Test]
        public void CreateFromIntArray() {
            nua.AssertEquals("TestB#31", decimal.MaxValue,
                new NullableDecimal(new int[] {-1, -1, -1, 0}).Value); 

            nua.AssertEquals("TestB#32", decimal.Zero,
                new NullableDecimal(new int[] {0, 0, 0, 0}).Value); 

            nua.AssertEquals("TestB#33", decimal.MinValue,
                new NullableDecimal(new int[] {-1, -1, -1, -2147483648}).Value); 
        }


        [nu.Test, nu.ExpectedException(typeof(System.ArgumentNullException))]
        public void CreateFromIntArrayArgumentNullException() { 
            int[] nullArray = null;
            new NullableDecimal(nullArray); 
        }


        [nu.Test, nu.ExpectedException(typeof(System.ArgumentException))]
        public void CreateFromIntArrayArgumentExceptionA() { 
            // Creo da un Array di zero elementi, non 4
            new NullableDecimal(new int[] {}); 
        }


        [nu.Test, nu.ExpectedException(typeof(System.ArgumentException))]
        public void CreateFromIntArrayArgumentExceptionB() { 
            // Array contenente una rappresentazione NON valida
            new NullableDecimal(new int[] {int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue}); 
        }


        [nu.Test]
        public void CreateFromLong() {
            nua.AssertEquals("TestB#41", (decimal)long.MinValue, new NullableDecimal(long.MinValue).Value); 
            nua.AssertEquals("TestB#42", (decimal)long.MaxValue, new NullableDecimal(long.MaxValue).Value); 
        }


        [nu.Test]
        public void CreateFromFloat() {
            float hi = (float)decimal.MaxValue / 2;
            nua.AssertEquals("TestB#51", (decimal)hi, new NullableDecimal(hi).Value); 

            float low = (float)decimal.MinValue / 2;
            nua.AssertEquals("TestB#52", (decimal)low, new NullableDecimal(low).Value); 
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void CreateFromFloatMaxOverflowException() {
            float f = 2f * (float)decimal.MaxValue;
            new NullableDecimal(f);
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void CreateFromFloatMinOverflowException() {
            float f = 2f * (float)decimal.MinValue;
            new NullableDecimal(f);
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void CreateFromFloatNaNOverflowException() {            
            new NullableDecimal(float.NaN);
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void CreateFromFloatPosInfOverflowException() {            
            new NullableDecimal(float.PositiveInfinity);
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void CreateFromFloatNegInfOverflowException() {            
            new NullableDecimal(float.NegativeInfinity);
        }


        [nu.Test]
        public void CreateFromUInt() {
            nua.AssertEquals("TestB#61", (decimal)uint.MinValue, new NullableDecimal(uint.MinValue).Value); 
            nua.AssertEquals("TestB#62", (decimal)uint.MaxValue, new NullableDecimal(uint.MaxValue).Value); 
        }


        [nu.Test]
        public void CreateFromULong() {
            nua.AssertEquals("TestB#71", (decimal)ulong.MinValue, new NullableDecimal(ulong.MinValue).Value); 
            nua.AssertEquals("TestB#72", (decimal)ulong.MaxValue, new NullableDecimal(ulong.MaxValue).Value); 
        }


        [nu.Test]
        public void CreateFromScale96Bits1Sign() {
            
            nua.AssertEquals("TestB#81", decimal.MaxValue,
                new NullableDecimal(-1, -1, -1, false, 0).Value); 

            nua.AssertEquals("TestB#82", decimal.Zero,
                new NullableDecimal(0, 0, 0, true, 0).Value); 

            nua.AssertEquals("TestB#83", decimal.MinValue,
                new NullableDecimal(-1, -1, -1, true, 0).Value); 

        }


        [nu.Test, nu.ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void CreateFromScale96Bits1SignArgumentOutOfRangeException() {
            new NullableDecimal(0, 0, 0, true, 29);
        }
        #endregion // Constructor Tests - B#

        #region INullable Tests - C#
        [nu.Test]
        public void IsNullProperty() {
            nua.Assert("TestC#01", _null.IsNull);
            nua.Assert("TestC#02", !_notNull.IsNull);
        }        
        #endregion // INullable Tests - C#

        #region IComparable - Ordering Tests - D#
        [nu.Test]
        public void Compare() {
            NullableDecimal _bigPos = new NullableDecimal(_pos.Value * 2);
            nua.Assert("TestD#01", (((sys.IComparable)_pos).CompareTo(_null) > 0));
            nua.Assert("TestD#02", (((sys.IComparable)_pos).CompareTo(_neg) > 0));
            nua.Assert("TestD#03", (((sys.IComparable)_pos).CompareTo(_bigPos) < 0));
            nua.Assert("TestD#04", (((sys.IComparable)_pos).CompareTo(_pos) == 0));

            nua.Assert("TestD#05", (((sys.IComparable)_null).CompareTo(_pos) < 0));
            nua.Assert("TestD#06", (((sys.IComparable)_null).CompareTo(_neg) < 0));
            nua.Assert("TestD#07", (((sys.IComparable)_null).CompareTo(NullableDecimal.Null) == 0));

            NullableDecimal _bigNeg = new NullableDecimal(_neg.Value * 2);
            nua.Assert("TestD#08", (((sys.IComparable)_neg).CompareTo(_null) > 0));
            nua.Assert("TestD#09", (((sys.IComparable)_neg).CompareTo(_zero) < 0));
            nua.Assert("TestD#11", (((sys.IComparable)_neg).CompareTo(_bigNeg) > 0));
            nua.Assert("TestD#12", (((sys.IComparable)_neg).CompareTo(_neg) == 0));

            nua.Assert("TestD#13", (((sys.IComparable)_negDec).CompareTo(_zero) < 0));
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
            
            NullableDecimal max = new NullableDecimal(decimal.MaxValue);
            nua.AssertEquals("TestE#01",decimal.MaxValue, max.Value);

            NullableDecimal min = new NullableDecimal(decimal.MinValue);
            nua.AssertEquals("TestE#02", decimal.MinValue, min.Value); 

        }

        [nu.Test]
        [nu.ExpectedException(typeof(NullableTypes.NullableNullValueException))]
        public void ValuePropertyNull() {
            decimal sVal = _null.Value;
        }

        // IsPositive property
        [nu.Test]
        public void IsPositive() {            
            nua.AssertEquals("TestE#11", true, _pos.IsPositive);
            nua.AssertEquals("TestE#12", false, _neg.IsPositive);
            nua.AssertEquals("TestE#13", false, _zero.IsPositive);
        } 

        [nu.Test]
        [nu.ExpectedException(typeof(NullableTypes.NullableNullValueException))]
        public void IsPositiveNull() {
            bool res =_null.IsPositive;
        }
        #endregion // Property Tests - E#

        #region Equivalence Tests - F#
        [nu.Test]
        public void StaticEqualsAndEqualityOperator() {
            // Case 1: either is NullableDecimal.Null
            nua.AssertEquals("TestF#01", NullableBoolean.Null, _null == _zero);
            nua.AssertEquals("TestF#02", NullableBoolean.Null, _null != _zero);
            nua.AssertEquals("TestF#03", NullableBoolean.Null, NullableDecimal.Equals(_neg, _null));
            nua.AssertEquals("TestF#04", NullableBoolean.Null, NullableDecimal.NotEquals(_neg, _null));

            // Case 2: both are NullableDecimal.Null
            nua.AssertEquals("TestF#05", NullableBoolean.Null, _null == NullableDecimal.Null);
            nua.AssertEquals("TestF#06", NullableBoolean.Null, _null != NullableDecimal.Null);
            nua.AssertEquals("TestF#07", NullableBoolean.Null, NullableDecimal.Equals(NullableDecimal.Null, _null));
            nua.AssertEquals("TestF#08", NullableBoolean.Null, NullableDecimal.NotEquals(NullableDecimal.Null, _null));

            // Case 3: both are equal
            NullableDecimal x = _pos;
            nua.AssertEquals("TestF#09", NullableBoolean.True, x == _pos);
            nua.AssertEquals("TestF#10", NullableBoolean.False, x != _pos);
            nua.AssertEquals("TestF#11", NullableBoolean.True, NullableDecimal.Equals(_pos, x));
            nua.AssertEquals("TestF#12", NullableBoolean.False, NullableDecimal.NotEquals(_pos, x));

            // Case 4: inequality
            nua.AssertEquals("TestF#13", NullableBoolean.False, _zero == _neg);
            nua.AssertEquals("TestF#14", NullableBoolean.True, _zero != _neg);
            nua.AssertEquals("TestF#15", NullableBoolean.False, NullableDecimal.Equals(_pos, _neg));
            nua.AssertEquals("TestF#16", NullableBoolean.True, NullableDecimal.NotEquals(_pos, _neg));
        } 


        [nu.Test]
        public void Equals() {
            // Case 1: either is NullableInt32.Null
            nua.Assert("TestF#21", !_null.Equals(_zero));
            nua.Assert("TestF#22", !_neg.Equals(_null));

            // Case 2: both are NullableInt32.Null
            nua.Assert("TestF#23", _null.Equals(NullableDecimal.Null));
            nua.Assert("TestF#24", NullableDecimal.Null.Equals(_null));

            // Case 3: both are equal
            NullableDecimal x = _pos;
            nua.Assert("TestF#25", x.Equals(_pos));
            nua.Assert("TestF#26", _pos.Equals(x));

            // Case 4: inequality
            nua.Assert("TestF#27", !_zero.Equals(_neg));
            nua.Assert("TestF#28", !_pos.Equals(_neg));
        }
        #endregion // Equivalence Tests - F#

        #region Method Tests - G#
        [nu.Test]
        public void Abs() {
            int ix = -int.MaxValue;                        
            NullableDecimal x = new NullableDecimal(ix);
            nua.AssertEquals("TestG#01", sys.Math.Abs(ix), NullableDecimal.Abs(x).Value);
            nua.AssertEquals("TestG#02", 0, NullableDecimal.Abs(NullableDecimal.Zero).Value);
            nua.Assert("TestG#03", NullableDecimal.Abs(NullableDecimal.Null).IsNull);
        }


        [nu.Test]
        public void Add() {
            decimal dx = -2345M;
            decimal dy = 4567M;
            
            NullableDecimal x = new NullableDecimal(dx);
            NullableDecimal y = new NullableDecimal(dy);;

            nua.AssertEquals("TestG#10", dx + dy, NullableDecimal.Add(x,y).Value);
        }


        [nu.Test]
        public void Divide() { 
            decimal dx = -2345.432545M;
            decimal dy = 423423.4567M;

            NullableDecimal x = new NullableDecimal(dx);
            NullableDecimal y = new NullableDecimal(dy);

            nua.AssertEquals("TestG#20", dx/dy, NullableDecimal.Divide(x,y).Value);
        }


        [nu.Test]
        public void GreaterThan() { 
            decimal dx = 25423235.2345M;
            decimal dy = -432423.3424567M;
            
            NullableDecimal x = new NullableDecimal(dx);
            NullableDecimal y = new NullableDecimal(dy);

            nua.AssertEquals("TestG#30", dx > dy, NullableDecimal.GreaterThan(x,y).Value);
        }

        [nu.Test]
        public void GreaterThanOrEquals() { 
            decimal dx = 9067985.9234742387M;
            decimal dy = 8242453.234432423M;
            
            NullableDecimal x = new NullableDecimal(dx);
            NullableDecimal y = new NullableDecimal(dy);

            nua.AssertEquals("TestG#41", dx >= dy, NullableDecimal.GreaterThanOrEqual(x,y).Value);
            nua.AssertEquals("TestG#42", dx >= dx, NullableDecimal.GreaterThanOrEqual(x,x).Value);
        }


        [nu.Test]
        public void LessThan() { 
            decimal dx = 4370452.2868246M;
            decimal dy = -65893.92346M;
            
            NullableDecimal x = new NullableDecimal(dx);
            NullableDecimal y = new NullableDecimal(dy);

            nua.AssertEquals("TestG#50", dx < dy, NullableDecimal.LessThan(x,y).Value);
        }


        [nu.Test]
        public void LessThanOrEquals() {
            decimal fx = - 236524.4343344M;
            decimal fy = - 2134143.193741249054M;
            
            NullableDecimal x = new NullableDecimal(fx);
            NullableDecimal y = new NullableDecimal(fy);

            nua.AssertEquals("TestG#61", fx <= fy, NullableDecimal.LessThanOrEqual(x,y).Value);
            nua.AssertEquals("TestG#62", fx <= fx, NullableDecimal.LessThanOrEqual(x,x).Value);
        }


        [nu.Test]
        public void Multiply() {
            decimal dx = 450.234424M;
            decimal dy = 397.232323234M;
            
            NullableDecimal x = new NullableDecimal(dx);
            NullableDecimal y = new NullableDecimal(dy);

            nua.AssertEquals("TestG#70", dx * dy, NullableDecimal.Multiply(x,y).Value);
        }


        [nu.Test]
        public void Parse() {
            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                // set a culture info where decimal point is ','
                sys.Threading.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;

                string sx = "-94357,2332";            
                NullableDecimal x = NullableDecimal.Parse(sx);

                nua.AssertEquals("TestG#80", decimal.Parse(sx), x.Value);
            }
            finally {
                // reset culture info 
                sys.Threading.Thread.CurrentThread.CurrentCulture = currCult;
            }
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ParseArgumentNullException() {
            string sx = null;            
            NullableDecimal.Parse(sx);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ParseFormatException() {
            string sx = "409'???'85";            
            NullableDecimal.Parse(sx);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void ParseOverflowException() {
            string sx = "9" + (decimal.MaxValue).ToString();            
            NullableDecimal.Parse(sx);
        }


        [nu.Test]
        public void Subtract() {
            decimal dx = 24354339.50284M;
            decimal dy = 4375336.9456M;
            
            NullableDecimal x = new NullableDecimal(dx);
            NullableDecimal y = new NullableDecimal(dy);;

            nua.AssertEquals("TestG#90", dx - dy, NullableDecimal.Subtract(x,y).Value);
        }


        [nu.Test]
        public void Ceiling() {
            decimal dx = +24354339.10284M;
            decimal dy = +24354340M;
            decimal dz = -24354339M;

            NullableDecimal nx = new NullableDecimal(dx);
            NullableDecimal ny = new NullableDecimal(dy);


            nua.AssertEquals("TestG#100", dy , NullableDecimal.Ceiling(nx).Value);
            nua.AssertEquals("TestG#100b", dz , NullableDecimal.Ceiling(-nx).Value);
            nua.AssertEquals("TestG#100c", dy , NullableDecimal.Ceiling(ny).Value);
        }


        [nu.Test]
        public void Floor() {
            decimal dx = +24354339.90284M;
            decimal dy = +24354339M;
            decimal dz = -24354340M;


            NullableDecimal nx = new NullableDecimal(dx);
            NullableDecimal ny = new NullableDecimal(dy);

            nua.AssertEquals("TestG#101", sys.Decimal.Floor(dx), NullableDecimal.Floor(nx).Value);
            nua.AssertEquals("TestG#102", sys.Decimal.Floor(dz), NullableDecimal.Floor(-nx).Value);
            nua.AssertEquals("TestG#103", sys.Decimal.Floor(dy), NullableDecimal.Floor(ny).Value);

        }


        [nu.Test]
        public void Round() {
            decimal dx = 24354339.50284M;
            
            NullableDecimal nx = new NullableDecimal(dx);

            nua.Assert("TestG#111", NullableDecimal.Round(_null, 0).IsNull);
            nua.AssertEquals("TestG#112", sys.Decimal.Round(dx, 0), NullableDecimal.Round(nx, 0).Value);
            nua.AssertEquals("TestG#113", sys.Decimal.Round(dx, 3), NullableDecimal.Round(nx, 3).Value);
            nua.AssertEquals("TestG#114", sys.Decimal.Round(dx, 28), NullableDecimal.Round(nx, 28).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentOutOfRangeException))]
        public void RoundArgumentOutOfRangeException() {            
            NullableDecimal.Round(_notNull, 44);
        }


        [nu.Test]
        public void Truncate() {
            decimal dx = 24354339.50284M;
            decimal dy = -24354339.50284M;
            
            NullableDecimal nx = new NullableDecimal(dx);
            NullableDecimal ny = new NullableDecimal(dy);

            nua.AssertEquals("TestG#121", sys.Decimal.Truncate(dx), NullableDecimal.Truncate(nx).Value);
            nua.AssertEquals("TestG#122", sys.Decimal.Truncate(dy), NullableDecimal.Truncate(ny).Value);
        }


        [nu.Test]
        public void Sign() {
            decimal dx = 24354339.50284M;
            
            NullableDecimal nx = new NullableDecimal(dx);

            nua.Assert("TestG#131", NullableDecimal.Sign(_null).IsNull);
            nua.AssertEquals("TestG#132", NullableInt32.Zero, NullableDecimal.Sign(_zero));
            nua.AssertEquals("TestG#133", new NullableInt32(+1), NullableDecimal.Sign(_pos));
            nua.AssertEquals("TestG#134", new NullableInt32(-1), NullableDecimal.Sign(_neg));
        }
        #endregion // Method Tests - G#

        #region Operator Tests - H#
        [nu.Test]
        public void AddOperator() {
            decimal dx = 23535623.272545M;
            decimal dy = -12335343.87254M;
            
            NullableDecimal nx = new NullableDecimal(dx);
            NullableDecimal ny = new NullableDecimal(dy);
            
            // Add nullable doubles
            nua.AssertEquals("TestH#01", (dx + dy), (nx + ny).Value);
            nua.AssertEquals("TestH#02", dx, (nx + _zero).Value);

            // Add Nulls
            nua.Assert("TestH#04", (nx + _null).IsNull);
            nua.Assert("TestH#05", (_null + ny).IsNull);
            nua.Assert("TestH#06", (_null + NullableDecimal.Null).IsNull);

            // Add doubles nullable doubles
            nua.AssertEquals("TestH#07", (dx + dy), (dx + ny).Value);
            nua.AssertEquals("TestH#08", dy, (ny + 0).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void AddOperatorOverflowException() {
            NullableDecimal res = NullableDecimal.MaxValue + NullableDecimal.MaxValue;
        }


        [nu.Test]
        public void DivideOperator() {
            decimal dx = 027457.65346653M;
            decimal dy = -64564.74634554M;
            
            NullableDecimal nx = new NullableDecimal(dx);
            NullableDecimal ny = new NullableDecimal(dy);
            
            // Divide nullable doubles
            nua.AssertEquals("TestH#11", (dx / dy), (nx / ny).Value);             
            nua.AssertEquals("TestH#12", 0, (_zero / ny).Value);

            // Divide Nulls
            nua.Assert("TestH#14", (nx / _null).IsNull);
            nua.Assert("TestH#15", (_null / ny).IsNull);
            nua.Assert("TestH#16", (_null / NullableDecimal.Null).IsNull);

            // Divide doubles nullable doubles
            nua.AssertEquals("TestH#17", (dx / dy), (dx / ny).Value);
            nua.AssertEquals("TestH#18", 0, (0 / ny).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.DivideByZeroException))]
        public void DivideOperatorDivideByZero() {
            NullableDecimal nz = _notNull / NullableDecimal.Zero;
        }


        [nu.Test]
        public void GreaterThanOperator() {
            NullableDecimal _bigPos = new NullableDecimal(_pos.Value * 2);
            NullableDecimal _bigNeg = new NullableDecimal(_neg.Value * 2);

            // GreaterThan nulls
            nua.Assert("TestH#21", (_pos > _null).IsNull);
            nua.Assert("TestH#22", (_null > _zero).IsNull);
            nua.Assert("TestH#23", (_null > _neg).IsNull);
            nua.Assert("TestH#24", (_null > NullableDecimal.Null).IsNull);

            // GreaterThan nullable ints
            nua.Assert("TestH#25", (_pos > _neg).IsTrue);
            nua.Assert("TestH#26", (_pos > _bigPos).IsFalse);
            nua.Assert("TestH#27", (_pos > _pos).IsFalse);
            nua.Assert("TestH#29", (_neg > _zero).IsFalse);
            nua.Assert("TestH#30", (_neg > _bigNeg).IsTrue);
            nua.Assert("TestH#31", (_neg > _neg).IsFalse);

            // GreaterThan ints
            nua.Assert("TestH#32", (_pos > _neg.Value).IsTrue);
            nua.Assert("TestH#33", (_pos > _bigPos.Value).IsFalse);
            nua.Assert("TestH#34", (_pos > _pos.Value).IsFalse);
            nua.Assert("TestH#35", (_neg > _zero.Value).IsFalse);
            nua.Assert("TestH#36", (_neg > _bigNeg.Value).IsTrue);
            nua.Assert("TestH#37", (_neg > _neg.Value).IsFalse);
        }


        [nu.Test]
        public void GreaterThanOrEqualsOperator() {
            NullableDecimal _bigPos = _pos * 2;
            NullableDecimal _bigNeg = _neg * 2;

            // GreaterThanOrEquals nulls
            nua.Assert("TestH#41", (_pos >= _null).IsNull);
            nua.Assert("TestH#42", (_null >= _zero).IsNull);
            nua.Assert("TestH#43", (_null >= _neg).IsNull);
            nua.Assert("TestH#44", (_null >= NullableDecimal.Null).IsNull);

            // GreaterThanOrEquals nullable ints
            nua.Assert("TestH#45", (_pos >= _neg).IsTrue);
            nua.Assert("TestH#46", (_pos >= _bigPos).IsFalse);
            nua.Assert("TestH#47", (_pos >= _pos).IsTrue);
            nua.Assert("TestH#49", (_neg >= _zero).IsFalse);
            nua.Assert("TestH#50", (_neg >= _bigNeg).IsTrue);
            nua.Assert("TestH#51", (_neg >= _neg).IsTrue);

            // GreaterThanOrEquals ints
            nua.Assert("TestH#52", (_pos >= _neg.Value).IsTrue);
            nua.Assert("TestH#53", (_pos >= _bigPos.Value).IsFalse);
            nua.Assert("TestH#54", (_pos >= _pos.Value).IsTrue);
            nua.Assert("TestH#55", (_neg >= _zero.Value).IsFalse);
            nua.Assert("TestH#56", (_neg >= _bigNeg.Value).IsTrue);
            nua.Assert("TestH#57", (_neg >= _neg.Value).IsTrue);

        }


        [nu.Test]
        public void LessThanOperator() {
            NullableDecimal _bigPos = _pos * 2;
            NullableDecimal _bigNeg = _neg * 2;

            // LessThan nulls
            nua.Assert("TestH#71", (_pos < _null).IsNull);
            nua.Assert("TestH#72", (_null < _zero).IsNull);
            nua.Assert("TestH#73", (_null < _neg).IsNull);
            nua.Assert("TestH#74", (_null < NullableDecimal.Null).IsNull);

            // LessThan nullable ints
            nua.Assert("TestH#75", (_pos < _neg).IsFalse);
            nua.Assert("TestH#76", (_pos < _bigPos).IsTrue);
            nua.Assert("TestH#77", (_pos < _pos).IsFalse);
            nua.Assert("TestH#79", (_neg < _zero).IsTrue);
            nua.Assert("TestH#80", (_neg < _bigNeg).IsFalse);
            nua.Assert("TestH#81", (_neg < _neg).IsFalse);

            // LessThan ints
            nua.Assert("TestH#82", (_pos < _neg.Value).IsFalse);
            nua.Assert("TestH#83", (_pos < _bigPos.Value).IsTrue);
            nua.Assert("TestH#84", (_pos < _pos.Value).IsFalse);
            nua.Assert("TestH#85", (_neg < _zero.Value).IsTrue);
            nua.Assert("TestH#86", (_neg < _bigNeg.Value).IsFalse);
            nua.Assert("TestH#87", (_neg < _neg.Value).IsFalse);
        }


        [nu.Test]
        public void LessThanOrEqualsOperator() {
            NullableDecimal _bigPos = _pos * 2;
            NullableDecimal _bigNeg = _neg * 2;

            // LessThanOrEquals nulls
            nua.Assert("TestH#91", (_pos <= _null).IsNull);
            nua.Assert("TestH#92", (_null <= _zero).IsNull);
            nua.Assert("TestH#93", (_null <= _neg).IsNull);
            nua.Assert("TestH#94", (_null <= NullableDecimal.Null).IsNull);

            // LessThanOrEquals nullable ints
            nua.Assert("TestH#95", (_pos <= _neg).IsFalse);
            nua.Assert("TestH#96", (_pos <= _bigPos).IsTrue);
            nua.Assert("TestH#97", (_pos <= _pos).IsTrue);
            nua.Assert("TestH#99", (_neg <= _zero).IsTrue);
            nua.Assert("TestH#100", (_neg <= _bigNeg).IsFalse);
            nua.Assert("TestH#101", (_neg <= _neg).IsTrue);

            // LessThanOrEquals ints
            nua.Assert("TestH#102", (_pos <= _neg.Value).IsFalse);
            nua.Assert("TestH#103", (_pos <= _bigPos.Value).IsTrue);
            nua.Assert("TestH#104", (_pos <= _pos.Value).IsTrue);
            nua.Assert("TestH#105", (_neg <= _zero.Value).IsTrue);
            nua.Assert("TestH#106", (_neg <= _bigNeg.Value).IsFalse);
            nua.Assert("TestH#107", (_neg <= _neg.Value).IsTrue);
        }


        [nu.Test]
        public void MultiplyOperator() {
            decimal dx = 7674.45453547M;
            decimal dy = -35656.573835M;
            
            NullableDecimal nx = new NullableDecimal(dx);
            NullableDecimal ny = new NullableDecimal(dy);
            
            // Multiply nullable ints
            nua.AssertEquals("TestH#121", (dx * dy), (nx * ny).Value);
            nua.AssertEquals("TestH#123", 0, (_zero * ny).Value);

            // Multiply Nulls
            nua.Assert("TestH#124", (nx * _null).IsNull);
            nua.Assert("TestH#125", (_null * ny).IsNull);
            nua.Assert("TestH#126", (_null * NullableDecimal.Null).IsNull);

            // Multiply ints nullable ints
            nua.AssertEquals("TestH#127", (dx * dy), (dx * ny).Value);
            nua.AssertEquals("TestH#128", 0, (0 * ny).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void MultiplyOperatorOverflowException() {
            NullableDecimal res = NullableDecimal.MaxValue * 3;
        }


        [nu.Test]
        public void SubtractionOperator() {
            decimal dx = 98345345.66565655M;
            decimal dy = -45.65656534M;
            
            NullableDecimal nx = new NullableDecimal(dx);
            NullableDecimal ny = new NullableDecimal(dy);
            
            // Subtraction nullable ints
            nua.AssertEquals("TestH#131", (dx - dy), (nx - ny).Value);
            nua.AssertEquals("TestH#133", dx, (nx - _zero).Value);

            // Subtraction Nulls
            nua.Assert("TestH#134", (nx - _null).IsNull);
            nua.Assert("TestH#135", (_null - ny).IsNull);
            nua.Assert("TestH#136", (_null - NullableDecimal.Null).IsNull);

            // Subtraction ints nullable ints
            nua.AssertEquals("TestH#137", (dx - dy), (dx - ny).Value);
            nua.AssertEquals("TestH#138", dy, (ny - 0).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void SubtractionOperatorOverflowException() {
            NullableDecimal res = NullableDecimal.MinValue - 3;
        }


        [nu.Test]
        public void UnaryNegationOperator() {
            decimal dx = 905635M;        
            NullableDecimal nx = new NullableDecimal(dx);
            
            // UnaryNegation nullable ints
            nua.AssertEquals("TestH#171", -dx, (-nx).Value);         
            nua.AssertEquals("TestH#172", -0, (-_zero).Value);

            // UnaryNegation Nulls
            nua.Assert("TestH#173", (-_null).IsNull);
            nua.Assert("TestH#174", (-NullableDecimal.Null).IsNull);
            
            // Min e Max
            nua.AssertEquals("TestH#175", -(NullableDecimal.MinValue.Value), (-NullableDecimal.MinValue).Value);
            nua.AssertEquals("TestH#176", -(NullableDecimal.MaxValue.Value), (-NullableDecimal.MaxValue).Value);
        }
        #endregion // Operator Tests - H#

        #region Conversion Operator Tests - I#
        [nu.Test]
        public void NullableBooleanToNullableDecimal() {
            nua.AssertEquals("TestI#01", NullableDecimal.Null, (NullableDecimal)NullableBoolean.Null);
            nua.AssertEquals("TestI#02", new NullableDecimal(0), (NullableDecimal)NullableBoolean.False);
            nua.AssertEquals("TestI#03", new NullableDecimal(1), (NullableDecimal)NullableBoolean.True);
        }


        public void NullableDecimalToDecimal() {
            decimal dx = 248695.9872M;
            NullableDecimal nx = new NullableDecimal(dx);
            nua.AssertEquals("TestI#11", dx, (decimal)nx);

            nua.AssertEquals("TestI#12", decimal.MaxValue, NullableDecimal.MaxValue);
            nua.AssertEquals("TestI#13", decimal.MinValue, NullableDecimal.MinValue);
        }


        [nu.Test, nu.ExpectedException(typeof(NullableNullValueException))]
        public void NullableDecimalToDecimalNullableNullValueException() {
            decimal res = (decimal)NullableDecimal.Null;            
        }


        [nu.Test]
        public void NullableSingleToNullableDecimal() {
            
            float fx = 95795.8756f;
            decimal dx = (decimal)fx;
            NullableSingle nfx = new NullableSingle(fx);
            NullableDecimal ndx =  new NullableDecimal(dx);
            nua.AssertEquals("TestI#21", ndx, (NullableDecimal)nfx);

            nua.AssertEquals("TestI#22", _null, (NullableDecimal)NullableSingle.Null);
        }


        [nu.Test]
        public void NullableStringToNullableDecimal() {
            nua.AssertEquals("TestI#31", NullableDecimal.Null, (NullableDecimal)NullableString.Null);
            
            decimal dx = 95795.98264M;
            NullableString nx =  new NullableString(dx.ToString());
            nua.AssertEquals("TestI#32", dx, ((NullableDecimal)nx).Value);

            NullableString ny =  new NullableString(decimal.MaxValue.ToString());                        
            nua.AssertEquals("TestI#33", NullableDecimal.MaxValue, (NullableDecimal)ny);

            NullableString nz =  new NullableString(decimal.MinValue.ToString());                        
            nua.AssertEquals("TestI#33", NullableDecimal.MinValue, (NullableDecimal)nz);
        }
  

        [nu.Test]
        public void NullableDoubleToNullableDecimal() {
            double dx = 92346.8723D;
            decimal x = (decimal)dx;
            NullableDouble ndx = new NullableDouble(dx);
            NullableDecimal nx =  new NullableDecimal(x);
            nua.AssertEquals("TestI#41", nx, (NullableDecimal)ndx);

            nua.AssertEquals("TestI#42", _null, (NullableDecimal)NullableDouble.Null);
        }


        [nu.Test]
        public void DecimalToNullableDecimal() {
            nua.AssertEquals("TestI#51", NullableDecimal.MaxValue, (NullableDecimal)decimal.MaxValue);
            nua.AssertEquals("TestI#53", NullableDecimal.Zero, (NullableDecimal)decimal.Zero);
            nua.AssertEquals("TestI#53", NullableDecimal.MinValue, (NullableDecimal)decimal.MinValue);
        }


        [nu.Test]
        public void NullableByteToNullableDecimal() {
            nua.AssertEquals("TestI#61", NullableDecimal.Null, (NullableDecimal)NullableByte.Null);

            byte bx = 100;
            decimal dx = (decimal)bx;
            NullableDecimal ndx = new NullableDecimal(dx);
            NullableByte nbx = new NullableByte(bx);
            nua.AssertEquals("TestI#62", ndx, (NullableDecimal)nbx);
            
        }


        [nu.Test]
        public void NullableInt16ToNullableDecimal() {
            nua.AssertEquals("TestI#71", NullableDecimal.Null, (NullableDecimal)NullableInt16.Null);
            
            short ix = 100;
            decimal dx = (decimal)ix;
            NullableDecimal ndx = new NullableDecimal(dx);
            NullableInt16 nix = new NullableInt16(ix);
            nua.AssertEquals("TestI#72", ndx, (NullableDecimal)nix);
        }


        [nu.Test]
        public void NullableInt32ToNullableDecimal() {
            nua.AssertEquals("TestI#81", NullableDecimal.Null, (NullableDecimal)NullableInt32.Null);
            
            int ix = 1000;
            decimal dx = (decimal)ix;
            NullableDecimal ndx = new NullableDecimal(dx);
            NullableInt32 nix = new NullableInt32(ix);
            nua.AssertEquals("TestI#82", ndx, (NullableDecimal)nix);
        }


        [nu.Test]
        public void NullableInt64ToNullableDecimal() {
            nua.AssertEquals("TestI#91", NullableDecimal.Null, (NullableDecimal)NullableInt64.Null);
            
            long ix = 100000;
            decimal dx = (decimal)ix;
            NullableDecimal ndx = new NullableDecimal(dx);
            NullableInt64 nix = new NullableInt64(ix);
            nua.AssertEquals("TestI#92", ndx, (NullableDecimal)nix);
        }
        #endregion // Conversion Operator Tests - I#

        #region Conversion Tests - J#
        [nu.Test]
        public void ToDouble() {
            decimal d = 91238679.1343124M;
            double db = (double)d;
            NullableDecimal nd = new NullableDecimal(d);

            nua.AssertEquals("TestJ#01", db, nd.ToDouble());
        }


        [nu.Test, nu.ExpectedException(typeof(NullableNullValueException))]
        public void ToDoubleNullableNullValueException() {
            double res = _null.ToDouble();
        }


        [nu.Test]
        public void ToNullableBoolean() {
            nua.AssertEquals("TestJ#11", NullableBoolean.Null.IsNull, _null.ToNullableBoolean().IsNull);
            nua.Assert("TestJ#12", _pos.ToNullableBoolean().IsTrue);
            nua.Assert("TestJ#13", _zero.ToNullableBoolean().IsFalse);
        }


        [nu.Test]
        public void ToNullableByte() {
            nua.Assert("TestJ#21", _null.ToNullableByte().IsNull);

            decimal d = 32;
            NullableDecimal n = new NullableDecimal(d);
            byte b = (byte)d;
            nua.AssertEquals("TestJ#22", b, n.ToNullableByte().Value);
            nua.AssertEquals("TestJ#23", new NullableByte(b).Value, n.ToNullableByte().Value);
        }


        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void ToNullableByteOverflowException() {
            NullableByte res = NullableDecimal.MaxValue.ToNullableByte();
        }

 
        [nu.Test]
        public void ToNullableDouble() {
            nua.Assert("TestJ#31", _null.ToNullableDouble().IsNull);

            decimal d = 29486.8264M;
            double  db = (double)d;
            NullableDecimal n = new NullableDecimal(d);
            nua.AssertEquals("TestJ#32", db, n.ToNullableDouble().Value);
            nua.AssertEquals("TestJ#33", new NullableDouble(db).Value, n.ToNullableDouble().Value);
        }


        [nu.Test]
        public void ToNullableInt16() {
            nua.Assert("TestJ#41", _null.ToNullableInt16().IsNull);

            decimal d = 9247;
            short s = (short)d;
            NullableDecimal n = new NullableDecimal(d);
            nua.AssertEquals("TestJ#42", s, n.ToNullableInt16().Value);
            nua.AssertEquals("TestJ#43", new NullableInt16(s).Value, n.ToNullableInt16().Value);
        }

            
        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void ToNullableInt16OverflowException() {
            NullableInt16 res = NullableDecimal.MaxValue.ToNullableInt16();
        }


        [nu.Test]
        public void ToNullableInt32() {
            nua.Assert("TestJ#51", _null.ToNullableInt32().IsNull);

            decimal d = 39247;
            int i = (int)d;
            NullableDecimal n = new NullableDecimal(d);
            nua.AssertEquals("TestJ#52", i, n.ToNullableInt32().Value);
            nua.AssertEquals("TestJ#53", new NullableInt32(i).Value, n.ToNullableInt32().Value);
        }

            
        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void ToNullableInt32OverflowException() {
            NullableInt32 res = NullableDecimal.MaxValue.ToNullableInt32();
        }


        [nu.Test]
        public void ToNullableInt64() {
            nua.Assert("TestJ#61", _null.ToNullableInt64().IsNull);

            decimal d = 3924723;
            long l = (long)d;
            NullableDecimal n = new NullableDecimal(d);
            nua.AssertEquals("TestJ#62", l, n.ToNullableInt64().Value);
            nua.AssertEquals("TestJ#63", new NullableInt64(l).Value, n.ToNullableInt64().Value);
        }

            
        [nu.Test, nu.ExpectedException(typeof(System.OverflowException))]
        public void ToNullableInt64OverflowException() {
            NullableInt64 res = NullableDecimal.MaxValue.ToNullableInt64();
        }


        [nu.Test]
        public void ToNullableSingle() {
            nua.Assert("TestJ#71", _null.ToNullableSingle().IsNull);

            decimal d = 29486.8264M;
            float f = (float)d;
            NullableDecimal n = new NullableDecimal(d);
            nua.AssertEquals("TestJ#72", f, n.ToNullableSingle().Value);
            nua.AssertEquals("TestJ#73", new NullableSingle(f).Value, n.ToNullableSingle().Value);
        }

            
        [nu.Test]
        public void ToNullableString() {
            nua.Assert("TestJ#81", _null.ToNullableString().IsNull);

            NullableDecimal n = new NullableDecimal(1234);
            nua.AssertEquals("TestJ#82", "1234", n.ToNullableString().Value);
            nua.AssertEquals("TestJ#83", new NullableString("1234").Value, n.ToNullableString().Value);
        }


        [nu.Test]
        public void ToStringTest() {
            nua.AssertEquals("TestJ#91", "Null", _null.ToString());

            int i = 6474;
            NullableDecimal n = new NullableDecimal(i);
            nua.AssertEquals("TestJ#92", i.ToString(), n.ToString());
        }
        #endregion // Conversion Tests - J#

        #region Serialization - K#
        [nu.Test]
        public void SerializableAttribute() {
            NullableDecimal serializedDeserialized;

            serializedDeserialized = SerializeDeserialize(_null);
            nua.Assert("TestK#01", serializedDeserialized.IsNull);
            nua.Assert("TestK#02", _null.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_zero);
            nua.Assert("TestK#03", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#04", _zero.Value, serializedDeserialized.Value);
            nua.Assert("TestK#05", _zero.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_pos);
            nua.Assert("TestK#06", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#07", _pos.Value, serializedDeserialized.Value);
            nua.Assert("TestK#08", _pos.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_neg);
            nua.Assert("TestK#09", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#10", _neg.Value, serializedDeserialized.Value);
            nua.Assert("TestK#11", _neg.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_negDec);
            nua.Assert("TestK#12", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#13", _negDec.Value, serializedDeserialized.Value);
            nua.Assert("TestK#14", _negDec.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_min);
            nua.Assert("TestK#15", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#16", _min.Value, serializedDeserialized.Value);
            nua.Assert("TestK#17", _min.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_max);
            nua.Assert("TestK#18", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#19", _max.Value, serializedDeserialized.Value);
            nua.Assert("TestK#20", _max.Equals(serializedDeserialized));
        }

        private NullableDecimal SerializeDeserialize(NullableDecimal x) {
            System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer = 
                new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableDecimal y = (NullableDecimal)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }


        [nu.Test]
        public void XmlSerializable() {
            NullableDecimal xmlSerializedDeserialized;

            xmlSerializedDeserialized = XmlSerializeDeserialize(_null);
            nua.Assert("TestK#30", xmlSerializedDeserialized.IsNull);
            nua.Assert("TestK#31", _null.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_zero);
            nua.Assert("TestK#32", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#33", _zero.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#34", _zero.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_pos);
            nua.Assert("TestK#35", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#36", _pos.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#37", _pos.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_neg);
            nua.Assert("TestK#38", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#39", _neg.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#40", _neg.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_negDec);
            nua.Assert("TestK#41", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#42", _negDec.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#43", _negDec.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_max);
            nua.Assert("TestK#44", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#45", _max.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#46", _max.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_min);
            nua.Assert("TestK#47", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#48", _min.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#49", _min.Equals(xmlSerializedDeserialized));
        }

        private NullableDecimal XmlSerializeDeserialize(NullableDecimal x) {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableDecimal));
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));
        
                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableDecimal y = (NullableDecimal)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }


        [nu.Test]
        public void XmlSerializableEmptyElementNil() {
            // Bug reported by Shaun Bowe (sbowe@users.sourceforge.net)
            // http://sourceforge.net/forum/message.php?msg_id=2399265

            //<?xml version="1.0"?>
            //<NullableDecimal xsi:nil="true" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"></NullableDecimal>

            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableDecimal));

            using (sys.IO.MemoryStream baseStream = new sys.IO.MemoryStream()) {
                using (sys.IO.StreamWriter stream = new System.IO.StreamWriter(baseStream)) {
                    stream.WriteLine("<?xml version=\"1.0\"?>");
                    stream.WriteLine("<NullableDecimal xsi:nil=\"true\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></NullableDecimal>");
                    stream.Flush();

//                    baseStream.Position = 0;
//                    sys.IO.StreamReader streamReader = new System.IO.StreamReader(baseStream);
//                    sys.Console.WriteLine(streamReader.ReadToEnd());
                    
                    baseStream.Position = 0; // Return stream to start
                    NullableDecimal y = (NullableDecimal)serializer.Deserialize(baseStream);

                    nua.Assert(y.IsNull);

                    baseStream.Close();
                    stream.Close();
                }
            }
        }


        [nu.Test]
        public void XmlSerializableSchema() {
            sysXmlScm.XmlSchema xsd = 
                ((sysXml.Serialization.IXmlSerializable)NullableDecimal.Null).GetSchema();

            xsd.Compile(new sysXmlScm.ValidationEventHandler(ValidationCallBack));

            ValidateXmlAgainstXmlSchema(xsd, _null);
            ValidateXmlAgainstXmlSchema(xsd, _min);
            ValidateXmlAgainstXmlSchema(xsd, _max);

        }

        private void ValidateXmlAgainstXmlSchema(sysXmlScm.XmlSchema xsd, NullableDecimal x) {
            sysXml.Serialization.XmlSerializer serializer = 
                new sysXml.Serialization.XmlSerializer(typeof(NullableDecimal));

                
            sys.IO.MemoryStream stream = null;
            sys.Xml.XmlValidatingReader validator = null;
            try {
                // Get the serialized NullableDecimal instance
                stream = new sys.IO.MemoryStream();
                serializer.Serialize(stream, x);
                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start

                // Add the default namespace
                sysXml.XmlDocument doc = new sysXml.XmlDocument();
                doc.Load(stream);                    
                sysXml.XmlAttribute defaultNs = doc.CreateAttribute("xmlns");
                defaultNs.Value = "http://NullableTypes.SourceForge.Net/NullableDecimalXMLSchema";
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