//
// NullableTypes.Tests.NullableInt64Test
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 03-Jun-2003  Luca    Create
// 13-Sep-2003  Luca    Upgrade    New test: SerializableAttribute
// 05-Oct-2003  DamienG Upgrade    New test: XmlSerializable
//                                 New test: Added additional tests to SerializableAttribute
//                                 Code upgrade: Added min and max for serialize tests to ensure full range 
//                                 persists
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
    public class NullableInt64Test {
        NullableInt64 _pos;
        NullableInt64 _neg;
        NullableInt64 _zero;
        NullableInt64 _null;
        NullableInt64 _min;
        NullableInt64 _max;


        [nu.SetUp]
        public void SetUp() {

            _pos = new NullableInt64(8574233);
            _neg = new NullableInt64(-32234234);
            _zero = new NullableInt64(0);
            _null = NullableInt64.Null;
            _min = NullableInt64.MinValue;
            _max = NullableInt64.MaxValue;
        }


        #region Field Tests - A#
        [nu.Test]
        public void FieldMax() {
            nua.AssertEquals("TestA#01", long.MaxValue, NullableInt64.MaxValue.Value);
        }


        [nu.Test]
        public void FieldMin() {
            nua.AssertEquals("TestA#02", long.MinValue, NullableInt64.MinValue.Value);
        }


        [nu.Test]
        public void NullField() {
            nua.Assert("TestA#03", NullableInt64.Null.IsNull);
        }


        [nu.Test]
        public void ZeroField() {
            nua.AssertEquals("TestA#04", (long)0, NullableInt64.Zero.Value);
            nua.AssertEquals("TestA#05", _zero.Value, NullableInt64.Zero.Value);
        }
        #endregion // Field Tests - A#
        
        #region Constructor Tests - B#
        [nu.Test]
        public void Create() {
            NullableInt64 pino = new NullableInt64(123234);
            nua.AssertEquals("TestB#01", (long)123234, pino.Value);

            NullableInt64 gino = new NullableInt64(-123234);
            nua.AssertEquals("TestB#02", (long)-123234, gino.Value);
        }
        #endregion // Constructor Tests - B#

        #region INullable Tests - C#
        public void IsNullProperty() {
            nua.Assert ("TestC#01", _null.IsNull);
            nua.Assert ("TestC#02", !_pos.IsNull);
        }
        #endregion // INullable Tests - C#

        #region IComparable - Ordering Tests - D#
        [nu.Test]
        public void Compare() {
            NullableInt64 _bigPos = _pos * 2;
            nua.Assert("TestD#01", (((sys.IComparable)_pos).CompareTo(_null) > 0));
            nua.Assert("TestD#02", (((sys.IComparable)_pos).CompareTo(_neg) > 0));
            nua.Assert("TestD#03", (((sys.IComparable)_pos).CompareTo(_bigPos) < 0));
            nua.Assert("TestD#04", (((sys.IComparable)_pos).CompareTo(_pos) == 0));

            nua.Assert("TestD#05", (((sys.IComparable)_null).CompareTo(_pos) < 0));
            nua.Assert("TestD#06", (((sys.IComparable)_null).CompareTo(_neg) < 0));
            nua.Assert("TestD#07", (((sys.IComparable)_null).CompareTo(NullableInt64.Null) == 0));

            NullableInt64 _bigNeg = _neg * 2;
            nua.Assert("TestD#08", (((sys.IComparable)_neg).CompareTo(_null) > 0));
            nua.Assert("TestD#09", (((sys.IComparable)_neg).CompareTo(_zero) < 0));
            nua.Assert("TestD#11", (((sys.IComparable)_neg).CompareTo(_bigNeg) > 0));
            nua.Assert("TestD#12", (((sys.IComparable)_neg).CompareTo(_neg) == 0));
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
            
            NullableInt64 maxInt = new NullableInt64(long.MaxValue);
            nua.AssertEquals("TestE#01",long.MaxValue, maxInt.Value);

            NullableInt64 minInt = new NullableInt64(long.MinValue);
            nua.AssertEquals("TestE#02", long.MinValue, minInt.Value);

            long i = 8276340;
            nua.AssertEquals("TestE#03", i, new NullableInt64(i).Value);
        }


        [nu.Test]
        [nu.ExpectedException(typeof(NullableTypes.NullableNullValueException))]
        public void ValuePropertyNull() {
            long iVal = _null.Value;
        }
        #endregion // Property Tests - E#

        #region Equivalence Tests - F#
        [nu.Test]
        public void StaticEqualsAndEqualityOperator() {
            // Case 1: either is NullableInt64.Null
            nua.AssertEquals("TestF#01", NullableBoolean.Null, _null == _zero);
            nua.AssertEquals("TestF#02", NullableBoolean.Null, NullableInt64.Equals(_neg, _null));
            nua.AssertEquals("TestF#03", NullableBoolean.Null, NullableInt64.NotEquals(_neg, _null));

            // Case 2: both are NullableInt64.Null
            nua.AssertEquals("TestF#04", NullableBoolean.Null, _null == NullableInt64.Null);
            nua.AssertEquals("TestF#05", NullableBoolean.Null, NullableInt64.Equals(NullableInt64.Null, _null));
            nua.AssertEquals("TestF#06", NullableBoolean.Null, NullableInt64.NotEquals(NullableInt64.Null, _null));

            // Case 3: both are equal
            NullableInt64 x = _pos;
            nua.AssertEquals ("TestF#07", NullableBoolean.True, x == _pos);
            nua.AssertEquals ("TestF#08", NullableBoolean.True, NullableInt64.Equals(_pos, x));
            nua.AssertEquals ("TestF#09", NullableBoolean.False, NullableInt64.NotEquals(_pos, x));

            // Case 4: inequality
            nua.AssertEquals ("TestF#10", NullableBoolean.False, _zero == _neg);
            nua.AssertEquals ("TestF#11", NullableBoolean.False, NullableInt64.Equals(_pos, _neg));
            nua.AssertEquals ("TestF#12", NullableBoolean.True, NullableInt64.NotEquals(_pos, _neg));
        }


        [nu.Test]
        public void Equals() {
            // Case 1: either is NullableInt64.Null
            nua.Assert("TestF#101", !_null.Equals(_zero));
            nua.Assert("TestF#102", !_neg.Equals(_null));

            // Case 2: both are NullableInt64.Null
            nua.Assert("TestF#103", _null.Equals(NullableInt64.Null));
            nua.Assert("TestF#104", NullableInt64.Null.Equals(_null));

            // Case 3: both are equal
            NullableInt64 x = _pos;
            nua.Assert("TestF#105", x.Equals(_pos));
            nua.Assert("TestF#106", _pos.Equals(x));

            // Case 4: inequality
            nua.Assert("TestF#107", !_zero.Equals(_neg));
            nua.Assert("TestF#108", !_pos.Equals(_neg));
        }
        #endregion // Equivalence Tests - F#

        #region Method Tests - G#
        [nu.Test]
        public void Add() {
            long ix = -97643554;
            long iy = 20;
            
            NullableInt64 x = new NullableInt64(ix);
            NullableInt64 y = new NullableInt64(iy);;

            nua.AssertEquals("TestG#01", ix + iy, NullableInt64.Add(x,y).Value);
        }


        [nu.Test]
        public void BitwiseAbs() {
            long ix = -long.MaxValue;
                        
            NullableInt64 x = new NullableInt64(ix);

            nua.AssertEquals("TestG#10", sys.Math.Abs(ix), NullableInt64.Abs(x).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void BitwiseAbsOverflowException() {
            long ix = long.MinValue;
                        
            NullableInt64.Abs(new NullableInt64(ix));
        }

        [nu.Test]
        public void BitwiseAnd() {
            long ix = 618765;
            long iy = 7124233;
                        
            NullableInt64 x = new NullableInt64(ix);
            NullableInt64 y = new NullableInt64(iy);            

            nua.AssertEquals("TestG#20", ix & iy, NullableInt64.BitwiseAnd(x ,y).Value);
        }


        [nu.Test]
        public void BitwiseOr() {
            long ix = 62455;
            long iy = 23;
                        
            NullableInt64 x = new NullableInt64(ix);
            NullableInt64 y = new NullableInt64(iy);            

            nua.AssertEquals("TestG#20", ix | iy, NullableInt64.BitwiseOr(x ,y).Value);
        }


        [nu.Test]
        public void Divide() {
            long ix = -95372334095;
            long iy = 143;
            
            NullableInt64 x = new NullableInt64(ix);
            NullableInt64 y = new NullableInt64(iy);

            nua.AssertEquals ("TestG#30", ix / iy, NullableInt64.Divide(x,y).Value);
        }


        [nu.Test]
        public void GreaterThan() {
            long ix = -9537095;
            long iy = 2143;
            
            NullableInt64 x = new NullableInt64(ix);
            NullableInt64 y = new NullableInt64(iy);

            nua.AssertEquals ("TestG#40", ix > iy, NullableInt64.GreaterThan(x,y).Value);
        }


        [nu.Test]
        public void GreaterThanOrEquals() {
            long ix = -954537095;
            long iy = 143;
            
            NullableInt64 x = new NullableInt64(ix);
            NullableInt64 y = new NullableInt64(iy);

            nua.AssertEquals ("TestG#51", ix >= iy, NullableInt64.GreaterThanOrEqual(x,y).Value);
            nua.AssertEquals ("TestG#52", ix >= ix, NullableInt64.GreaterThanOrEqual(x,x).Value);
        }


        [nu.Test]
        public void LessThan() {
            long ix = 8450;
            long iy = 6797346;
            
            NullableInt64 x = new NullableInt64(ix);
            NullableInt64 y = new NullableInt64(iy);

            nua.AssertEquals ("TestG#60", ix < iy, NullableInt64.LessThan(x,y).Value);
        }


        [nu.Test]
        public void LessThanOrEquals() {
            long ix = 413236524;
            long iy = 143;
            
            NullableInt64 x = new NullableInt64(ix);
            NullableInt64 y = new NullableInt64(iy);

            nua.AssertEquals ("TestG#71", ix <= iy, NullableInt64.LessThanOrEqual(x,y).Value);
            nua.AssertEquals ("TestG#72", ix <= ix, NullableInt64.LessThanOrEqual(x,x).Value);
        }


        [nu.Test]
        public void Mod() {
            long ix = 93478450;
            long iy = 6797;
            
            NullableInt64 x = new NullableInt64(ix);
            NullableInt64 y = new NullableInt64(iy);

            nua.AssertEquals ("TestG#80", ix % iy, NullableInt64.Mod(x,y).Value);
        }


        [nu.Test]
        public void Multiply() {
            long ix = 450;
            long iy = 397;
            
            NullableInt64 x = new NullableInt64(ix);
            NullableInt64 y = new NullableInt64(iy);

            nua.AssertEquals ("TestG#90", ix * iy, NullableInt64.Multiply(x,y).Value);
        }


        [nu.Test]
        public void OnesComplement() {
            long ix = 4500985;            
            NullableInt64 x = new NullableInt64(ix);

            nua.AssertEquals ("TestG#100", ~ix, NullableInt64.OnesComplement(x).Value);
        }


        [nu.Test]
        public void Subtract() {
            long ix = 23950284;
            long iy = 43769456;
            
            NullableInt64 x = new NullableInt64(ix);
            NullableInt64 y = new NullableInt64(iy);;

            nua.AssertEquals("TestG#110", ix - iy, NullableInt64.Subtract(x,y).Value);
        }


        [nu.Test]
        public void Xor() {
            long ix = 23950284;
            long iy = 43769456;
            
            NullableInt64 x = new NullableInt64(ix);
            NullableInt64 y = new NullableInt64(iy);;

            nua.AssertEquals("TestG#120", ix ^ iy, NullableInt64.Xor(x,y).Value);
        }


        [nu.Test]
        public void Parse() {
            string sx = "94357";            
            NullableInt64 x = NullableInt64.Parse(sx);

            nua.AssertEquals ("TestG#130", long.Parse(sx), NullableInt64.Parse(sx).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ParseArgumentNullException() {
            string sx = null;            
            NullableInt64.Parse(sx);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ParseFormatException() {
            string sx = "409'85";            
            NullableInt64.Parse(sx);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void ParseOverflowException() {
            string sx = (long.MaxValue + 1M).ToString();            
            NullableInt64.Parse(sx);
        }
        #endregion // Method Tests - G#

        #region Operator Tests - H#
        [nu.Test]
        public void AddOperator() {
            long ix = 23522342343;
            long iy = -122343;
            
            NullableInt64 nx = new NullableInt64(ix);
            NullableInt64 ny = new NullableInt64(iy);
            
            // Add nullable ints
            nua.AssertEquals("TestH#01", (ix + iy), (nx + ny).Value);
            nua.AssertEquals("TestH#02", ix, (nx + _zero).Value);

            // Add Nulls
            nua.Assert("TestH#03", (nx + _null).IsNull);
            nua.Assert("TestH#04", (_null + ny).IsNull);
            nua.Assert("TestH#05", (_null + NullableInt64.Null).IsNull);

            // Add ints nullable ints
            nua.AssertEquals("TestH#06", (ix + iy), (ix + ny).Value);
            nua.AssertEquals("TestH#07", iy, (ny + 0).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void AddOperatorOverflow() {
            long ix = long.MaxValue;
            long iy = 1;
            
            NullableInt64 nx = new NullableInt64(ix);
            NullableInt64 ny = new NullableInt64(iy);
            
            NullableInt64 nz = nx + ny;
        }


        [nu.Test]
        public void BitwiseAndOperator() {
            long ix = 93462;
            long iy = 342043247;
            long ii = -1;
            
            NullableInt64 nx = new NullableInt64(ix);
            NullableInt64 ny = new NullableInt64(iy);
            
            // & nullable ints
            nua.AssertEquals("TestH#11", (ix & iy), (nx & ny).Value);
            nua.AssertEquals("TestH#12", 0, (nx & _zero).Value);
            nua.AssertEquals("TestH#13", iy, (ii & ny).Value);

            // & Nulls
            nua.Assert("TestH#14", (nx & _null).IsNull);
            nua.Assert("TestH#15", (_null & ny).IsNull);
            nua.Assert("TestH#16", (_null & NullableInt64.Null).IsNull);

            // & ints nullable ints
            nua.AssertEquals("TestH#17", (ix & iy), (ix & ny).Value);
            nua.AssertEquals("TestH#18", 0, (ny & 0).Value);
            nua.AssertEquals("TestH#19", iy, (ii & ny).Value);
        }


        [nu.Test]
        public void BitwiseOrOperator() {
            long ix = 95365;
            long iy = 67746652;
            long ii = -1;
            
            NullableInt64 nx = new NullableInt64(ix);
            NullableInt64 ny = new NullableInt64(iy);
            
            // | nullable ints
            nua.AssertEquals("TestH#21", (ix | iy), (nx | ny).Value);
            nua.AssertEquals("TestH#22", ix, (nx | _zero).Value);
            nua.AssertEquals("TestH#23", ii, (ii | ny).Value);

            // | Nulls
            nua.Assert("TestH#24", (nx | _null).IsNull);
            nua.Assert("TestH#25", (_null | ny).IsNull);
            nua.Assert("TestH#26", (_null | NullableInt64.Null).IsNull);

            // | ints nullable ints
            nua.AssertEquals("TestH#27", (ix | iy), (ix | ny).Value);
            nua.AssertEquals("TestH#28", iy, (ny | 0).Value);
            nua.AssertEquals("TestH#29", ii, (ii | ny).Value);
        }


        [nu.Test]
        public void DivideOperator() {
            long ix = 027457;
            long iy = -64564;
            
            NullableInt64 nx = new NullableInt64(ix);
            NullableInt64 ny = new NullableInt64(iy);
            
            // Divide nullable ints
            nua.AssertEquals("TestH#31", (ix / iy), (nx / ny).Value);
            nua.AssertEquals("TestH#32", 0, (_zero / ny).Value);

            // Divide Nulls
            nua.Assert("TestH#33", (nx / _null).IsNull);
            nua.Assert("TestH#34", (_null / ny).IsNull);
            nua.Assert("TestH#35", (_null / NullableInt64.Null).IsNull);

            // Divide ints nullable ints
            nua.AssertEquals("TestH#36", (ix / iy), (ix / ny).Value);
            nua.AssertEquals("TestH#37", 0, (0 / ny).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.DivideByZeroException))]
        public void DivideOperatorDivideByZero() {
            NullableInt64 nx = new NullableInt64(82479);
            NullableInt64 ny = new NullableInt64(0);
            
            NullableInt64 nz = nx / ny;
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void DivideOperatorOverflowException() {
            NullableInt64 nx = new NullableInt64(long.MinValue);
            NullableInt64 ny = new NullableInt64(-1);
            
            NullableInt64 nz = nx / ny;
        }


        [nu.Test]
        public void ExclusiveOrOperator() {
            long ix = 95334;
            long iy = 213467752;
            long ii = -1;
            
            NullableInt64 nx = new NullableInt64(ix);
            NullableInt64 ny = new NullableInt64(iy);
            
            // ^ nullable ints
            nua.AssertEquals("TestH#41", (ix ^ iy), (nx ^ ny).Value);
            nua.AssertEquals("TestH#42", ix, (nx ^ _zero).Value);
            nua.AssertEquals("TestH#43", ~iy, (ii ^ ny).Value);

            // ^ Nulls
            nua.Assert("TestH#44", (nx ^ _null).IsNull);
            nua.Assert("TestH#45", (_null ^ ny).IsNull);
            nua.Assert("TestH#46", (_null ^ NullableInt64.Null).IsNull);

            // ^ ints nullable ints
            nua.AssertEquals("TestH#47", (ix ^ iy), (ix ^ ny).Value);
            nua.AssertEquals("TestH#48", iy, (ny ^ 0).Value);
            nua.AssertEquals("TestH#49", ~iy, (ii ^ ny).Value);
        }


        [nu.Test]
        public void GreaterThanOperator() {
            NullableInt64 _bigPos = _pos * 2;
            NullableInt64 _bigNeg = _neg * 2;

            // GreaterThan nulls
            nua.Assert("TestH#51", (_pos > _null).IsNull);
            nua.Assert("TestH#52", (_null > _zero).IsNull);
            nua.Assert("TestH#53", (_null > _neg).IsNull);
            nua.Assert("TestH#54", (_null > NullableInt64.Null).IsNull);

            // GreaterThan nullable ints
            nua.Assert("TestH#55", (_pos > _neg).IsTrue);
            nua.Assert("TestH#56", (_pos > _bigPos).IsFalse);
            nua.Assert("TestH#57", (_pos > _pos).IsFalse);
            nua.Assert("TestH#59", (_neg > _zero).IsFalse);
            nua.Assert("TestH#60", (_neg > _bigNeg).IsTrue);
            nua.Assert("TestH#61", (_neg > _neg).IsFalse);

            // GreaterThan ints
            nua.Assert("TestH#62", (_pos > _neg.Value).IsTrue);
            nua.Assert("TestH#63", (_pos > _bigPos.Value).IsFalse);
            nua.Assert("TestH#64", (_pos > _pos.Value).IsFalse);
            nua.Assert("TestH#65", (_neg > _zero.Value).IsFalse);
            nua.Assert("TestH#66", (_neg > _bigNeg.Value).IsTrue);
            nua.Assert("TestH#67", (_neg > _neg.Value).IsFalse);
        }


        [nu.Test]
        public void GreaterThanOrEqualsOperator() {
            NullableInt64 _bigPos = _pos * 2;
            NullableInt64 _bigNeg = _neg * 2;

            // GreaterThanOrEquals nulls
            nua.Assert("TestH#71", (_pos >= _null).IsNull);
            nua.Assert("TestH#72", (_null >= _zero).IsNull);
            nua.Assert("TestH#73", (_null >= _neg).IsNull);
            nua.Assert("TestH#74", (_null >= NullableInt64.Null).IsNull);

            // GreaterThanOrEquals nullable ints
            nua.Assert("TestH#75", (_pos >= _neg).IsTrue);
            nua.Assert("TestH#76", (_pos >= _bigPos).IsFalse);
            nua.Assert("TestH#77", (_pos >= _pos).IsTrue);
            nua.Assert("TestH#79", (_neg >= _zero).IsFalse);
            nua.Assert("TestH#80", (_neg >= _bigNeg).IsTrue);
            nua.Assert("TestH#81", (_neg >= _neg).IsTrue);

            // GreaterThanOrEquals ints
            nua.Assert("TestH#82", (_pos >= _neg.Value).IsTrue);
            nua.Assert("TestH#83", (_pos >= _bigPos.Value).IsFalse);
            nua.Assert("TestH#84", (_pos >= _pos.Value).IsTrue);
            nua.Assert("TestH#85", (_neg >= _zero.Value).IsFalse);
            nua.Assert("TestH#86", (_neg >= _bigNeg.Value).IsTrue);
            nua.Assert("TestH#87", (_neg >= _neg.Value).IsTrue);
        }


        [nu.Test]
        public void LessThanOperator() {
            NullableInt64 _bigPos = _pos * 2;
            NullableInt64 _bigNeg = _neg * 2;

            // LessThan nulls
            nua.Assert("TestH#91", (_pos < _null).IsNull);
            nua.Assert("TestH#92", (_null < _zero).IsNull);
            nua.Assert("TestH#93", (_null < _neg).IsNull);
            nua.Assert("TestH#94", (_null < NullableInt64.Null).IsNull);

            // LessThan nullable ints
            nua.Assert("TestH#95", (_pos < _neg).IsFalse);
            nua.Assert("TestH#96", (_pos < _bigPos).IsTrue);
            nua.Assert("TestH#97", (_pos < _pos).IsFalse);
            nua.Assert("TestH#99", (_neg < _zero).IsTrue);
            nua.Assert("TestH#100", (_neg < _bigNeg).IsFalse);
            nua.Assert("TestH#101", (_neg < _neg).IsFalse);

            // LessThan ints
            nua.Assert("TestH#102", (_pos < _neg.Value).IsFalse);
            nua.Assert("TestH#103", (_pos < _bigPos.Value).IsTrue);
            nua.Assert("TestH#104", (_pos < _pos.Value).IsFalse);
            nua.Assert("TestH#105", (_neg < _zero.Value).IsTrue);
            nua.Assert("TestH#106", (_neg < _bigNeg.Value).IsFalse);
            nua.Assert("TestH#107", (_neg < _neg.Value).IsFalse);
        }


        [nu.Test]
        public void LessThanOrEqualsOperator() {
            NullableInt64 _bigPos = _pos * 2;
            NullableInt64 _bigNeg = _neg * 2;

            // LessThanOrEquals nulls
            nua.Assert("TestH#111", (_pos <= _null).IsNull);
            nua.Assert("TestH#112", (_null <= _zero).IsNull);
            nua.Assert("TestH#113", (_null <= _neg).IsNull);
            nua.Assert("TestH#114", (_null <= NullableInt64.Null).IsNull);

            // LessThanOrEquals nullable ints
            nua.Assert("TestH#115", (_pos <= _neg).IsFalse);
            nua.Assert("TestH#116", (_pos <= _bigPos).IsTrue);
            nua.Assert("TestH#117", (_pos <= _pos).IsTrue);
            nua.Assert("TestH#119", (_neg <= _zero).IsTrue);
            nua.Assert("TestH#120", (_neg <= _bigNeg).IsFalse);
            nua.Assert("TestH#121", (_neg <= _neg).IsTrue);

            // LessThanOrEquals ints
            nua.Assert("TestH#122", (_pos <= _neg.Value).IsFalse);
            nua.Assert("TestH#123", (_pos <= _bigPos.Value).IsTrue);
            nua.Assert("TestH#124", (_pos <= _pos.Value).IsTrue);
            nua.Assert("TestH#125", (_neg <= _zero.Value).IsTrue);
            nua.Assert("TestH#126", (_neg <= _bigNeg.Value).IsFalse);
            nua.Assert("TestH#127", (_neg <= _neg.Value).IsTrue);
        }


        [nu.Test]
        public void ModulusOperator() {
            long ix = -2847923;
            long iy = 67251;
            
            NullableInt64 nx = new NullableInt64(ix);
            NullableInt64 ny = new NullableInt64(iy);
            
            // Modulus nullable ints
            nua.AssertEquals("TestH#131", (ix % iy), (nx % ny).Value);
            nua.AssertEquals("TestH#132", 0, (_zero % ny).Value);

            // Modulus Nulls
            nua.Assert("TestH#133", (nx % _null).IsNull);
            nua.Assert("TestH#134", (_null % ny).IsNull);
            nua.Assert("TestH#135", (_null % NullableInt64.Null).IsNull);

            // Modulus ints nullable ints
            nua.AssertEquals("TestH#136", (ix % iy), (nx % iy).Value);
            nua.AssertEquals("TestH#137", 0, (0 % ny).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.DivideByZeroException))]
        public void ModulusOperatorDivideByZero() {
            NullableInt64 nx = new NullableInt64(82479);
            NullableInt64 ny = new NullableInt64(0);
            
            NullableInt64 nz = nx % ny;
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void ModulusOperatorOverflowException() {
            NullableInt64 nx = new NullableInt64(long.MinValue);
            NullableInt64 ny = new NullableInt64(-1);
            
            NullableInt64 nz = nx % ny;
        }


        [nu.Test]
        public void MultiplyOperator() {
            long ix = 0274579076;
            long iy = -645644;
            
            NullableInt64 nx = new NullableInt64(ix);
            NullableInt64 ny = new NullableInt64(iy);
            
            // Multiply nullable ints
            nua.AssertEquals("TestH#141", (ix * iy), (nx * ny).Value);
            nua.AssertEquals("TestH#142", 0, (_zero * ny).Value);

            // Multiply Nulls
            nua.Assert("TestH#143", (nx * _null).IsNull);
            nua.Assert("TestH#144", (_null * ny).IsNull);
            nua.Assert("TestH#145", (_null * NullableInt64.Null).IsNull);

            // Multiply ints nullable ints
            nua.AssertEquals("TestH#146", (ix * iy), (ix * ny).Value);
            nua.AssertEquals("TestH#147", 0, (0 * ny).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void MultiplyOperatorOverflowException() {
            NullableInt64 nx = new NullableInt64(long.MinValue);
            NullableInt64 ny = new NullableInt64(-1);
            
            NullableInt64 nz = nx * ny;
        }


        [nu.Test]
        public void OnesComplementOperator() {
            long ix = 92575;            
            NullableInt64 nx = new NullableInt64(ix);
            
            // OnesComplement nullable ints
            nua.AssertEquals("TestH#151", ~ix, (~nx).Value);
            nua.AssertEquals("TestH#152", ~0, (~_zero).Value);

            // OnesComplement Nulls
            nua.Assert("TestH#153", (~_null).IsNull);
        }


        [nu.Test]
        public void SubtractionOperator() {
            long ix = 985;
            long iy = -4534;
            
            NullableInt64 nx = new NullableInt64(ix);
            NullableInt64 ny = new NullableInt64(iy);
            
            // Subtraction nullable ints
            nua.AssertEquals("TestH#161", (ix - iy), (nx - ny).Value);
            nua.AssertEquals("TestH#162", ix, (nx - _zero).Value);

            // Subtraction Nulls
            nua.Assert("TestH#163", (nx - _null).IsNull);
            nua.Assert("TestH#164", (_null - ny).IsNull);
            nua.Assert("TestH#165", (_null - NullableInt64.Null).IsNull);

            // Subtraction ints nullable ints
            nua.AssertEquals("TestH#166", (ix - iy), (ix - ny).Value);
            nua.AssertEquals("TestH#167", iy, (ny - 0).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void SubtractionOperatorOverflow() {
            long ix = long.MinValue;
            long iy = 1;
            
            NullableInt64 nx = new NullableInt64(ix);
            NullableInt64 ny = new NullableInt64(iy);
            
            NullableInt64 nz = nx - ny;
        }


        [nu.Test]
        public void UnaryNegationOperator() {
            long ix = 905635;        
            NullableInt64 nx = new NullableInt64(ix);
            
            // UnaryNegation nullable ints
            nua.AssertEquals("TestH#171", (-ix), (-nx).Value);         
            nua.AssertEquals("TestH#172", (-0), (-_zero).Value);

            // UnaryNegation Nulls
            nua.Assert("TestH#173", (-_null).IsNull);
            nua.Assert("TestH#174", (-NullableInt64.Null).IsNull);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void UnaryNegationOperatorOverflow() {
            long ix = long.MinValue;            
            NullableInt64 nx = new NullableInt64(ix);
            
            NullableInt64 nz =  - nx;
        }
        #endregion // Operator Tests - H#

        #region Conversion Operator Tests - I#
        [nu.Test]
        public void NullableBooleanConversionOperator() {
            nua.AssertEquals("TestI#01", NullableInt64.Null, (NullableInt64)NullableBoolean.Null);
            nua.AssertEquals("TestI#02", new NullableInt64(0), (NullableInt64)NullableBoolean.False);
            nua.AssertEquals("TestI#03", new NullableInt64(1), (NullableInt64)NullableBoolean.True);
        }


        [nu.Test]
        public void NullableByteConversionOperator() {
            nua.AssertEquals ("TestI#11", NullableInt64.Null, (NullableInt64)NullableByte.Null);
            
            byte bx = 12;
            NullableByte nbx =  new NullableByte(bx);
            nua.AssertEquals("TestI#12", bx, ((NullableInt64)nbx).Value);
        }


        [nu.Test]
        public void NullableDecimalConversionOperator() {
            nua.AssertEquals ("TestI#11", NullableInt64.Null, (NullableInt64)NullableDecimal.Null);
            
            long ix = 94237;
            NullableDecimal nx =  new NullableDecimal(ix);
            nua.AssertEquals ("TestI#12", ix, ((NullableInt64)nx).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void NullableDecimalConversionOperatorOverflowException() {
            NullableDecimal nx =  new NullableDecimal(long.MaxValue + 1M);

            NullableInt64 ny = (NullableInt64)nx;
        }


        [nu.Test]
        public void NullableDoubleConversionOperator() {
            nua.AssertEquals ("TestI#21", NullableInt64.Null, (NullableInt64)NullableDouble.Null);
            
            long ix = 92345795;
            NullableDouble nx =  new NullableDouble(ix);
            nua.AssertEquals ("TestI#22", ix, ((NullableInt64)nx).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void NullableDoubleConversionOperatorOverflowException() {
            double d = -(double)long.MinValue;
            NullableDouble nx =  new NullableDouble(d);

            NullableInt64 ny = (NullableInt64)nx;
        }


        [nu.Test]
        public void NullableInt64ToInt64ConversionOperator() {
            long ix = 957953241;
            NullableInt64 nx =  new NullableInt64(ix);
            nua.AssertEquals("TestI#31", ix, (long)nx);
        }


        [nu.Test, nu.ExpectedException(typeof(NullableNullValueException))]
        public void NullableInt64ToInt64ConversionOperatorNullValueException() {
            long i = (long)_null;
        }

        
        [nu.Test]
        public void NullableInt32ConversionOperator() {
            nua.AssertEquals ("TestI#41", NullableInt64.Null, (NullableInt64)NullableInt32.Null);
            
            long ix = 8654;
            NullableInt32 nx =  new NullableInt32((int)ix);
            nua.AssertEquals ("TestI#42", ix, ((NullableInt64)nx).Value);
        }




        [nu.Test]
        public void NullableSingleConversionOperator() {
            nua.AssertEquals("TestI#61", NullableInt64.Null, (NullableInt64)NullableSingle.Null);
            
            long ix = 95795;
            NullableSingle nx =  new NullableSingle(ix);
            nua.AssertEquals("TestI#62", ix, ((NullableInt64)nx).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void NullableSingleConversionOperatorOverflowException() {
            float f = -(float)long.MinValue;
            NullableSingle nx =  new NullableSingle(f);

            NullableInt64 ny = (NullableInt64)nx;
        }


        [nu.Test]
        public void NullableStringConversionOperator() {
            nua.AssertEquals ("TestI#71", NullableInt64.Null, (NullableInt64)NullableString.Null);
            
            long ix = 95795;
            NullableString nx =  new NullableString(ix.ToString());
            nua.AssertEquals("TestI#72", ix, ((NullableInt64)nx).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void NullableStringConversionOperatorOverflowException() {
            NullableString nx =  new NullableString((long.MaxValue + 1M).ToString());

            NullableInt64 ny = (NullableInt64)nx;
        }
        

        [nu.Test]
        public void Int64ToNullableInt64ConversionOperator() {
            long ix = 23495795;
            NullableInt64 nx =  (NullableInt64)ix;
            nua.AssertEquals("TestI#81", ix, nx.Value);
        }


        [nu.Test]
        public void NullableInt16ConversionOperator() {
            nua.AssertEquals ("TestI#91", NullableInt64.Null, (NullableInt64)NullableInt16.Null);
            
            short sx = 12;
            NullableInt16 nx =  new NullableInt16(sx);
            nua.AssertEquals ("TestI#92", sx, ((NullableInt64)nx).Value);
        }
        #endregion // Conversion Operator Tests - I#

        #region Conversion Tests - J#
        [nu.Test]
        public void ToNullableBoolean() {
            nua.AssertEquals("TestJ#01", NullableBoolean.Null, _null.ToNullableBoolean());
            nua.AssertEquals("TestJ#02", NullableBoolean.True, _pos.ToNullableBoolean());
            nua.AssertEquals("TestJ#03", NullableBoolean.False, _zero.ToNullableBoolean());
        }


        [nu.Test]
        public void ToNullableByte() {
            byte b = 32;
            NullableInt64 n = new NullableInt64(b);
            nua.AssertEquals("TestJ#11", NullableByte.Null, _null.ToNullableByte());
            nua.AssertEquals("TestJ#12", b, n.ToNullableByte().Value);
            nua.AssertEquals("TestJ#13", new NullableByte(b), n.ToNullableByte());
        }


        [nu.Test]
        public void ToNullableDecimal() {
            decimal  d = 6474;
            NullableInt64 n = new NullableInt64((long)d);
            nua.AssertEquals("TestJ#21", NullableDecimal.Null, _null.ToNullableDecimal());
            nua.AssertEquals("TestJ#22", d, n.ToNullableDecimal().Value);
            nua.AssertEquals("TestJ#23", new NullableDecimal(d), n.ToNullableDecimal());
        }


        [nu.Test]
        public void ToNullableDouble() {
            double  d = 6474234;
            NullableInt64 n = new NullableInt64((long)d);
            nua.AssertEquals("TestJ#31", NullableDouble.Null, _null.ToNullableDouble());
            nua.AssertEquals("TestJ#32", d, n.ToNullableDouble().Value);
            nua.AssertEquals("TestJ#33", new NullableDouble(d), n.ToNullableDouble());
        }


        [nu.Test]
        public void ToNullableInt16() {
            short s = 8923;
            NullableInt64 n = new NullableInt64(s);
            nua.AssertEquals("TestJ#41", NullableInt16.Null, _null.ToNullableInt16());
            nua.AssertEquals("TestJ#42", s, n.ToNullableInt16().Value);
            nua.AssertEquals("TestJ#43", new NullableInt16(s), n.ToNullableInt16());
        }

            
        [nu.Test]
        public void ToNullableInt32() {
            int i = 8923;
            NullableInt64 n = new NullableInt64(i);
            nua.AssertEquals("TestJ#51", NullableInt32.Null, _null.ToNullableInt32());
            nua.AssertEquals("TestJ#52", i, n.ToNullableInt32().Value);
            nua.AssertEquals("TestJ#53", new NullableInt32(i), n.ToNullableInt32());
        }
            
            

           
        [nu.Test]
        public void ToNullableSingle() {
            float f = 6474;
            NullableInt64 n = new NullableInt64((long)f);
            nua.AssertEquals("TestJ#71", NullableSingle.Null, _null.ToNullableSingle());
            nua.AssertEquals("TestJ#72", f, n.ToNullableSingle().Value);
            nua.AssertEquals("TestJ#73", new NullableSingle(f), n.ToNullableSingle());
        }


        [nu.Test]
        public void ToNullableString() {
            long i = 6474;
            NullableInt64 n = new NullableInt64(i);
            nua.AssertEquals("TestJ#81", NullableString.Null, _null.ToNullableString());
            nua.AssertEquals("TestJ#82", i.ToString(), n.ToNullableString().Value);
            nua.AssertEquals("TestJ#83", new NullableString(i.ToString()), n.ToNullableString());
        }


        [nu.Test]
        public void ToStringTest() {
            long i = 6474;
            NullableInt64 n = new NullableInt64(i);
            nua.AssertEquals("TestJ#91", "Null", _null.ToString());
            nua.AssertEquals("TestJ#92", i.ToString(), n.ToString());
        }
        #endregion // Conversion Tests - J#

        #region Serialization - K#
        [nu.Test]
        public void SerializableAttribute() {
            NullableInt64 serializedDeserialized;

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

            serializedDeserialized = SerializeDeserialize(_min);
            nua.Assert("TestK#12", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#13", _min.Value, serializedDeserialized.Value);
            nua.Assert("TestK#14", _min.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_max);
            nua.Assert("TestK#15", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#16", _max.Value, serializedDeserialized.Value);
            nua.Assert("TestK#17", _max.Equals(serializedDeserialized));
        }

        private NullableInt64 SerializeDeserialize(NullableInt64 x) {
            System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer = 
                new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableInt64 y = (NullableInt64)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }

        [nu.Test]
        public void XmlSerializable() {
            NullableInt64 xmlSerializedDeserialized;

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

            xmlSerializedDeserialized = XmlSerializeDeserialize(_max);
            nua.Assert("TestK#41", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#42", _max.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#43", _max.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_min);
            nua.Assert("TestK#44", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#45", _min.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#46", _min.Equals(xmlSerializedDeserialized));
        }

        private NullableInt64 XmlSerializeDeserialize(NullableInt64 x) {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableInt64));
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));
        
                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableInt64 y = (NullableInt64)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }


        [nu.Test]
        public void XmlSerializableEmptyElementNil() {
            // Bug reported by Shaun Bowe (sbowe@users.sourceforge.net)
            // http://sourceforge.net/forum/message.php?msg_id=2399265

            //<?xml version="1.0"?>
            //<NullableInt64 xsi:nil="true" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"></NullableInt64>

            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableInt64));

            using (sys.IO.MemoryStream baseStream = new sys.IO.MemoryStream()) {
                using (sys.IO.StreamWriter stream = new System.IO.StreamWriter(baseStream)) {
                    stream.WriteLine("<?xml version=\"1.0\"?>");
                    stream.WriteLine("<NullableInt64 xsi:nil=\"true\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></NullableInt64>");
                    stream.Flush();

//                    baseStream.Position = 0;
//                    sys.IO.StreamReader streamReader = new System.IO.StreamReader(baseStream);
//                    sys.Console.WriteLine(streamReader.ReadToEnd());
                    
                    baseStream.Position = 0; // Return stream to start
                    NullableInt64 y = (NullableInt64)serializer.Deserialize(baseStream);

                    nua.Assert(y.IsNull);

                    baseStream.Close();
                    stream.Close();
                }
            }

        }


        [nu.Test]
        public void XmlSerializableSchema() {
            sysXmlScm.XmlSchema xsd = 
                ((sysXml.Serialization.IXmlSerializable)NullableInt64.Null).GetSchema();

            xsd.Compile(new sysXmlScm.ValidationEventHandler(ValidationCallBack));

            ValidateXmlAgainstXmlSchema(xsd, _null);
            ValidateXmlAgainstXmlSchema(xsd, _min);
            ValidateXmlAgainstXmlSchema(xsd, _max);

        }

        private void ValidateXmlAgainstXmlSchema(sysXmlScm.XmlSchema xsd, NullableInt64 x) {
            sysXml.Serialization.XmlSerializer serializer = 
                new sysXml.Serialization.XmlSerializer(typeof(NullableInt64));

                
            sys.IO.MemoryStream stream = null;
            sys.Xml.XmlValidatingReader validator = null;
            try {
                // Get the serialized NullableInt64 instance
                stream = new sys.IO.MemoryStream();
                serializer.Serialize(stream, x);
                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start

                // Add the default namespace
                sysXml.XmlDocument doc = new sysXml.XmlDocument();
                doc.Load(stream);                    
                sysXml.XmlAttribute defaultNs = doc.CreateAttribute("xmlns");
                defaultNs.Value = "http://NullableTypes.SourceForge.Net/NullableInt64XMLSchema";
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