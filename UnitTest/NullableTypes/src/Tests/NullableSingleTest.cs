//
// NullableTypes.Tests.NullableSingleTest
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 14-Apr-2003  Luca    Create
// 13-Sep-2003  Luca    Upgrade    New test: SerializableAttribute
// 15-Sep-2003  Luca    Upgrade    Code upgrade: improved test Parse to make it independent 
//                                 from user regional settings
// 05-Oct-2003  DamienG Upgrade    New test: XmlSerializable
//                                 New test: Added additional tests to SerializableAttribute
//                                 Code upgrade: Added min and max for serialize tests to ensure full range 
//                                 persists
//                                 Code upgrade: Change _pos and _neg to be decimals not integers
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
    public class NullableSingleTest {

        NullableSingle _pos;
        NullableSingle _neg;
        NullableSingle _zero;
        NullableSingle _null;
        NullableSingle _notNull;

        NullableSingle _negInf;
        NullableSingle _eps;
        NullableSingle _negEps;
        NullableSingle _posInf;
        NullableSingle _nan;

        NullableSingle _negDec;
        NullableSingle _min;
        NullableSingle _max;

        [nu.SetUp]
        public void SetUp() {
            _pos = new NullableSingle(246.234);
            _neg = new NullableSingle(-9823.198);
            _negDec = new NullableSingle(-0.000001);
            _zero = new NullableSingle(0);
            _null = NullableSingle.Null;
            _notNull = new NullableSingle(90356);

            _negInf = new NullableSingle(float.NegativeInfinity);
            _eps = new NullableSingle(float.Epsilon);
            _negEps = new NullableSingle(-float.Epsilon);
            _posInf = new NullableSingle(float.PositiveInfinity);
            _nan = new NullableSingle(float.NaN);

            _min = NullableSingle.MinValue;
            _max = NullableSingle.MaxValue;
        }


        #region Field Tests - A#
        [nu.Test]
        public void MaxValue() {
            nua.AssertEquals("TestA#01", 3.40282346638528859E+38f, NullableSingle.MaxValue.Value);
        }


        [nu.Test]
        public void MinValue() {
            nua.AssertEquals("TestA#02", -3.40282346638528859E+38f, NullableSingle.MinValue.Value);
        }


        [nu.Test]
        public void Zero() {
            nua.AssertEquals("TestA#03", 0f, NullableSingle.Zero.Value);
        }


        [nu.Test]
        public void Null() {
            nua.Assert ("TestA#04", NullableSingle.Null.IsNull);
        }
        #endregion // Field Tests - A#

        #region Constructor Tests - B#
        [nu.Test]
        public void Create() {
            float fPino = 34.87f;
            NullableSingle pino = new NullableSingle(fPino);                      
            nua.AssertEquals("TestB#01", fPino, pino.Value);

            float fGino = -9000.6543f;
            double dGino = fGino;
            NullableSingle gino = new NullableSingle(dGino);                      
            nua.AssertEquals("TestB#02", fGino, gino.Value);

            nua.Assert("TestB#03", float.IsPositiveInfinity(new NullableSingle(float.PositiveInfinity).Value));
            nua.Assert("TestB#04", float.IsNegativeInfinity(new NullableSingle(float.NegativeInfinity).Value));
            nua.AssertEquals("TestB#05", float.Epsilon, new NullableSingle(float.Epsilon).Value);
            nua.Assert("TestB#06", float.IsNaN(new NullableSingle(float.NaN).Value));
        }


        [nu.Test]
        public void CreateFromDouble() {
            nua.Assert("TestB#11", float.IsPositiveInfinity(new NullableSingle(double.PositiveInfinity).Value));
            nua.Assert("TestB#12", float.IsPositiveInfinity(new NullableSingle(double.MaxValue).Value));

            nua.AssertEquals("TestB#13", 0f, new NullableSingle(+double.Epsilon).Value);
            nua.AssertEquals("TestB#14", 0f, new NullableSingle(-double.Epsilon).Value);

            nua.Assert("TestB#15", float.IsNegativeInfinity(new NullableSingle(double.MinValue).Value));
            nua.Assert("TestB#16", float.IsNegativeInfinity(new NullableSingle(double.NegativeInfinity).Value));

            nua.Assert("TestB#17", float.IsNaN(new NullableSingle(double.NaN).Value));
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
            NullableSingle _bigPos = _pos * 2;
            nua.Assert("TestD#01", (((sys.IComparable)_pos).CompareTo(_null) > 0));
            nua.Assert("TestD#02", (((sys.IComparable)_pos).CompareTo(_neg) > 0));
            nua.Assert("TestD#03", (((sys.IComparable)_pos).CompareTo(_bigPos) < 0));
            nua.Assert("TestD#04", (((sys.IComparable)_pos).CompareTo(_pos) == 0));

            nua.Assert("TestD#05", (((sys.IComparable)_null).CompareTo(_pos) < 0));
            nua.Assert("TestD#06", (((sys.IComparable)_null).CompareTo(_neg) < 0));
            nua.Assert("TestD#07", (((sys.IComparable)_null).CompareTo(NullableSingle.Null) == 0));

            NullableSingle _bigNeg = _neg * 2;
            nua.Assert("TestD#08", (((sys.IComparable)_neg).CompareTo(_null) > 0));
            nua.Assert("TestD#09", (((sys.IComparable)_neg).CompareTo(_zero) < 0));
            nua.Assert("TestD#11", (((sys.IComparable)_neg).CompareTo(_bigNeg) > 0));
            nua.Assert("TestD#12", (((sys.IComparable)_neg).CompareTo(_neg) == 0));
        }


        [nu.Test]
        public void CompareNanInfEps() {
            nua.Assert("TestD#21", (((sys.IComparable)_negInf).CompareTo(NullableSingle.MinValue) < 0));
            nua.Assert("TestD#22", (((sys.IComparable)NullableSingle.MinValue).CompareTo(_negEps) < 0));
            nua.Assert("TestD#23", (((sys.IComparable)_negEps).CompareTo(_zero) < 0));
            nua.Assert("TestD#24", (((sys.IComparable)_zero).CompareTo(_eps) < 0));

            nua.Assert("TestD#25", (((sys.IComparable)_eps).CompareTo(NullableSingle.MaxValue) < 0));
            nua.Assert("TestD#26", (((sys.IComparable)NullableSingle.MaxValue).CompareTo(_posInf) < 0));

            nua.Assert("TestD#27", (((sys.IComparable)_nan).CompareTo(_eps) < 0));
            nua.Assert("TestD#28", (((sys.IComparable)_nan).CompareTo(_posInf) < 0));
            nua.Assert("TestD#29", (((sys.IComparable)_nan).CompareTo(_negInf) < 0));

            nua.Assert("TestD#30", (((sys.IComparable)_nan).CompareTo(_nan) == 0));
            nua.Assert("TestD#31", (((sys.IComparable)_posInf).CompareTo(_posInf) == 0));
            nua.Assert("TestD#32", (((sys.IComparable)_negInf).CompareTo(_negInf) == 0));
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
            NullableSingle max = new NullableSingle(float.MaxValue);
            nua.AssertEquals("TestE#01",float.MaxValue, max.Value);

            NullableSingle min = new NullableSingle(float.MinValue);
            nua.AssertEquals("TestE#02", float.MinValue, min.Value);

            float f = 89426.7634f;
            nua.AssertEquals("TestE#03", f, new NullableSingle(f).Value);

            NullableSingle _nan = new NullableSingle(float.NaN);
            nua.AssertEquals("TestE#04", float.NaN, _nan.Value);

            NullableSingle _posInf = new NullableSingle(float.PositiveInfinity);
            nua.AssertEquals("TestE#05", float.PositiveInfinity, _posInf.Value);

            NullableSingle _negInf = new NullableSingle(float.NegativeInfinity);
            nua.AssertEquals("TestE#06", float.NegativeInfinity, _negInf.Value);

            NullableSingle eps = new NullableSingle(float.Epsilon);
            nua.AssertEquals("TestE#06", float.Epsilon, eps.Value);
        }


        [nu.Test]
        [nu.ExpectedException(typeof(NullableTypes.NullableNullValueException))]
        public void ValuePropertyNull() {
            float sVal = _null.Value;
        }
        #endregion // Property Tests - E#

        #region Equivalence Tests - F#
        [nu.Test]
        public void StaticEqualsAndEqualityOperator() {
            // Case 1: either is NullableSingle.Null
            nua.AssertEquals("TestF#01", NullableBoolean.Null, _null == _zero);
            nua.AssertEquals("TestF#02", NullableBoolean.Null, NullableSingle.Equals(_neg, _null));
            nua.AssertEquals("TestF#03", NullableBoolean.Null, NullableSingle.NotEquals(_neg, _null));

            // Case 2: both are NullableSingle.Null
            nua.AssertEquals("TestF#04", NullableBoolean.Null, _null == NullableSingle.Null);
            nua.AssertEquals("TestF#05", NullableBoolean.Null, NullableSingle.Equals(NullableSingle.Null, _null));
            nua.AssertEquals("TestF#06", NullableBoolean.Null, NullableSingle.NotEquals(NullableSingle.Null, _null));

            // Case 3: both are equal
            NullableSingle x = _pos;
            nua.AssertEquals("TestF#07", NullableBoolean.True, x == _pos);
            nua.AssertEquals("TestF#08", NullableBoolean.True, NullableSingle.Equals(_pos, x));
            nua.AssertEquals("TestF#09", NullableBoolean.False, NullableSingle.NotEquals(_pos, x));

            // Case 4: inequality
            nua.AssertEquals("TestF#10", NullableBoolean.False, _zero == _neg);
            nua.AssertEquals("TestF#11", NullableBoolean.False, NullableSingle.Equals(_pos, _neg));
            nua.AssertEquals("TestF#12", NullableBoolean.True, NullableSingle.NotEquals(_pos, _neg));
        }


        [nu.Test]
        public void StaticEqualsAndEqualityOperatorNanInfEps() {
            // Case 1: both are NaN
            nua.AssertEquals("TestF#11", NullableBoolean.False, _nan == _nan);
            nua.AssertEquals("TestF#12", NullableBoolean.True, NullableSingle.Equals(_nan, _nan));
            nua.AssertEquals("TestF#13", NullableBoolean.False, NullableSingle.NotEquals(_nan, _nan));

            // Case 2: both are PositiveInfinity
            nua.AssertEquals("TestF#21", NullableBoolean.True, _posInf == _posInf);
            nua.AssertEquals("TestF#22", NullableBoolean.True, NullableSingle.Equals(_posInf, _posInf));
            nua.AssertEquals("TestF#23", NullableBoolean.False, NullableSingle.NotEquals(_posInf, _posInf));

            // Case 3: both are NegativeInfinity
            nua.AssertEquals("TestF#31", NullableBoolean.True, _negInf == _negInf);
            nua.AssertEquals("TestF#32", NullableBoolean.True, NullableSingle.Equals(_negInf, _negInf));
            nua.AssertEquals("TestF#33", NullableBoolean.False, NullableSingle.NotEquals(_negInf, _negInf));
        }


        [nu.Test]
        public void Equals() {
            // Case 1: either is NullableInt32.Null
            nua.Assert("TestF#101", !_null.Equals(_zero));
            nua.Assert("TestF#102", !_neg.Equals(_null));

            // Case 2: both are NullableInt32.Null
            nua.Assert("TestF#103", _null.Equals(NullableSingle.Null));
            nua.Assert("TestF#104", NullableSingle.Null.Equals(_null));

            // Case 3: both are equal
            NullableSingle x = _pos;
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
            float fx = -97644.444f;
            float fy = 2.234f;
            
            NullableSingle x = new NullableSingle(fx);
            NullableSingle y = new NullableSingle(fy);;

            nua.AssertEquals("TestG#01", fx + fy, NullableSingle.Add(x,y).Value);
        }


        [nu.Test]
        public void Divide() {
            float fx = -9537095.76f;
            float fy = -143.7346f;
            
            NullableSingle x = new NullableSingle(fx);
            NullableSingle y = new NullableSingle(fy);

            nua.AssertEquals("TestG#10", fx/fy, NullableSingle.Divide(x,y).Value);
        }


        [nu.Test]
        public void GreaterThan() {
            float fx = -9537095.2314f;
            float fy = 143.234f;
            
            NullableSingle x = new NullableSingle(fx);
            NullableSingle y = new NullableSingle(fy);

            nua.AssertEquals("TestG#20", fx > fy, NullableSingle.GreaterThan(x,y).Value);
        }


        [nu.Test]
        public void GreaterThanOrEquals() {
            float fx = -9537095.245f;
            float fy = 143.356f;
            
            NullableSingle x = new NullableSingle(fx);
            NullableSingle y = new NullableSingle(fy);

            nua.AssertEquals("TestG#31", fx >= fy, NullableSingle.GreaterThanOrEqual(x,y).Value);
            nua.AssertEquals("TestG#32", fx >= fx, NullableSingle.GreaterThanOrEqual(x,x).Value);
        }


        [nu.Test]
        public void LessThan() {
            float fx = 8450.234f;
            float fy = 6797346.234f;
            
            NullableSingle x = new NullableSingle(fx);
            NullableSingle y = new NullableSingle(fy);

            nua.AssertEquals("TestG#40", fx < fy, NullableSingle.LessThan(x,y).Value);
        }


        [nu.Test]
        public void LessThanOrEquals() {
            float fx = 236524.4343344f;
            float fy = 143.4f;
            
            NullableSingle x = new NullableSingle(fx);
            NullableSingle y = new NullableSingle(fy);

            nua.AssertEquals("TestG#51", fx <= fy, NullableSingle.LessThanOrEqual(x,y).Value);
            nua.AssertEquals("TestG#52", fx <= fx, NullableSingle.LessThanOrEqual(x,x).Value);
        }


        [nu.Test]
        public void Multiply() {
            float fx = 450.234424f;
            float fy = 397.232323234f;
            
            NullableSingle x = new NullableSingle(fx);
            NullableSingle y = new NullableSingle(fy);

            nua.AssertEquals("TestG#60", fx * fy, NullableSingle.Multiply(x,y).Value);
        }


        [nu.Test]
        public void Parse() {
            string sx = (94357.2332f).ToString("R");            
            NullableSingle x = NullableSingle.Parse(sx);

            nua.AssertEquals("TestG#70", float.Parse(sx), x.Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ParseArgumentNullException() {
            string sx = null;            
            NullableSingle.Parse(sx);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ParseFormatException() {
            string sx = "409'85";            
            NullableSingle.Parse(sx);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void ParseOverflowException() {
            string sx = (double.MaxValue).ToString();            
            NullableSingle.Parse(sx);
        }


        [nu.Test]
        public void Subtract() {
            float fx = 239.50284f;
            float fy = 4376.9456f;
            
            NullableSingle x = new NullableSingle(fx);
            NullableSingle y = new NullableSingle(fy);;

            nua.AssertEquals("TestG#80", fx - fy, NullableSingle.Subtract(x,y).Value);
        }
        #endregion // Method Tests - G#

        #region Operator Tests - H#
        [nu.Test]
        public void AddOperator() {
            float fx = 23523.275f;
            float fy = -123.872f;
            
            NullableSingle nx = new NullableSingle(fx);
            NullableSingle ny = new NullableSingle(fy);
            NullableSingle nMax = new NullableSingle(float.MaxValue);
            
            // Add nullable floats
            nua.AssertEquals("TestH#01", (fx + fy), (nx + ny).Value);
            nua.AssertEquals("TestH#02", fx, (nx + _zero).Value);
            nua.AssertEquals("TestH#03", (fx + float.MaxValue), (nx + nMax).Value);

            // Add Nulls
            nua.Assert("TestH#04", (nx + _null).IsNull);
            nua.Assert("TestH#05", (_null + ny).IsNull);
            nua.Assert("TestH#06", (_null + NullableSingle.Null).IsNull);

            // Add floats nullable floats
            nua.AssertEquals("TestH#07", (fx + fy), (fx + ny).Value);
            nua.AssertEquals("TestH#08", fy, (ny + 0).Value);
        }


        [nu.Test]
        public void DivideOperator() {
            float fx = 027457.65f;
            float fy = -64564.746f;
            
            NullableSingle nx = new NullableSingle(fx);
            NullableSingle ny = new NullableSingle(fy);
            NullableSingle nMax = new NullableSingle(float.MaxValue);
                        
            NullableSingle nz = nx / ny;
            
            // Divide nullable floats
            nua.AssertEquals("TestH#11", (fx / fy), (nx / ny).Value);
            nua.AssertEquals("TestH#12", (float.MaxValue / float.Epsilon), (nMax / _eps).Value);
            nua.AssertEquals("TestH#13", 0, (_zero / ny).Value);

            // Divide Nulls
            nua.Assert("TestH#14", (nx / _null).IsNull);
            nua.Assert("TestH#15", (_null / ny).IsNull);
            nua.Assert("TestH#16", (_null / NullableSingle.Null).IsNull);

            // Divide floats nullable floats
            nua.AssertEquals("TestH#17", (fx / fy), (fx / ny).Value);
            nua.AssertEquals("TestH#18", 0, (0 / ny).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.DivideByZeroException))]
        public void DivideOperatorDivideByZero() {
            NullableSingle nx = new NullableSingle(82479.726);
            NullableSingle ny = new NullableSingle(0);
            
            NullableSingle nz = nx / ny;
        }


        [nu.Test]
        public void GreaterThanOperator() {
            NullableSingle _bigPos = _pos * 2;
            NullableSingle _bigNeg = _neg * 2;

            // GreaterThan nulls
            nua.Assert("TestH#21", (_pos > _null).IsNull);
            nua.Assert("TestH#22", (_null > _zero).IsNull);
            nua.Assert("TestH#23", (_null > _neg).IsNull);
            nua.Assert("TestH#24", (_null > NullableSingle.Null).IsNull);

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
            NullableSingle _bigPos = _pos * 2;
            NullableSingle _bigNeg = _neg * 2;

            // GreaterThanOrEquals nulls
            nua.Assert("TestH#41", (_pos >= _null).IsNull);
            nua.Assert("TestH#42", (_null >= _zero).IsNull);
            nua.Assert("TestH#43", (_null >= _neg).IsNull);
            nua.Assert("TestH#44", (_null >= NullableSingle.Null).IsNull);

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
            NullableSingle _bigPos = _pos * 2;
            NullableSingle _bigNeg = _neg * 2;

            // LessThan nulls
            nua.Assert("TestH#71", (_pos < _null).IsNull);
            nua.Assert("TestH#72", (_null < _zero).IsNull);
            nua.Assert("TestH#73", (_null < _neg).IsNull);
            nua.Assert("TestH#74", (_null < NullableSingle.Null).IsNull);

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
            NullableSingle _bigPos = _pos * 2;
            NullableSingle _bigNeg = _neg * 2;

            // LessThanOrEquals nulls
            nua.Assert("TestH#91", (_pos <= _null).IsNull);
            nua.Assert("TestH#92", (_null <= _zero).IsNull);
            nua.Assert("TestH#93", (_null <= _neg).IsNull);
            nua.Assert("TestH#94", (_null <= NullableSingle.Null).IsNull);

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
            float fx = 027457.2345f;
            float fy = -64564.243f;
            
            NullableSingle nx = new NullableSingle(fx);
            NullableSingle ny = new NullableSingle(fy);
            NullableSingle nMin = new NullableSingle(float.MinValue);
            NullableSingle nMax = new NullableSingle(float.MaxValue);
                      
            // Multiply nullable floats
            nua.AssertEquals("TestH#121", (fx * fy), (nx * ny).Value);
            nua.AssertEquals("TestH#122", (float.MinValue * float.MaxValue), (nMin * nMax).Value);
            nua.AssertEquals("TestH#123", 0, (_zero * ny).Value);

            // Multiply Nulls
            nua.Assert("TestH#124", (nx * _null).IsNull);
            nua.Assert("TestH#125", (_null * ny).IsNull);
            nua.Assert("TestH#126", (_null * NullableSingle.Null).IsNull);

            // Multiply ints nullable floats
            nua.AssertEquals("TestH#127", (fx * fy), (fx * ny).Value);
            nua.AssertEquals("TestH#128", 0, (0 * ny).Value);
        }


        [nu.Test]
        public void SubtractionOperator() {
            float fx = 985.345f;
            float fy = -4534.234f;
            
            NullableSingle nx = new NullableSingle(fx);
            NullableSingle ny = new NullableSingle(fy);
            NullableSingle nMin = new NullableSingle(float.MinValue);
            NullableSingle nMax = new NullableSingle(float.MaxValue);
            
            // Subtraction nullable ints
            nua.AssertEquals("TestH#131", (fx - fy), (nx - ny).Value);
            nua.AssertEquals("TestH#132", (float.MinValue - float.MaxValue), (nMin - nMax).Value);
            nua.AssertEquals("TestH#133", fx, (nx - _zero).Value);

            // Subtraction Nulls
            nua.Assert("TestH#134", (nx - _null).IsNull);
            nua.Assert("TestH#135", (_null - ny).IsNull);
            nua.Assert("TestH#136", (_null - NullableSingle.Null).IsNull);

            // Subtraction ints nullable ints
            nua.AssertEquals("TestH#137", (fx - fy), (fx - ny).Value);
            nua.AssertEquals("TestH#138", fy, (ny - 0).Value);
        }


        [nu.Test]
        public void UnaryNegationOperator() {
            float fx = 905635.234f;        
            NullableSingle nx = new NullableSingle(fx);
            
            // UnaryNegation nullable ints
            nua.AssertEquals("TestH#171", (-fx), (-nx).Value);         
            nua.AssertEquals("TestH#172", (-0), (-_zero).Value);

            // UnaryNegation Nulls
            nua.Assert("TestH#173", (-_null).IsNull);
            nua.Assert("TestH#174", (-NullableSingle.Null).IsNull);
        }
        #endregion // Operator Tests - H#

        #region Conversion Operator Tests - I#
        [nu.Test]
        public void NullableBooleanConversionOperator() {
            nua.AssertEquals("TestI#01", NullableSingle.Null, (NullableSingle)NullableBoolean.Null);
            nua.AssertEquals("TestI#02", new NullableSingle(0), (NullableSingle)NullableBoolean.False);
            nua.AssertEquals("TestI#03", new NullableSingle(1), (NullableSingle)NullableBoolean.True);
        }


        [nu.Test]
        public void NullableSingleToSingleConversionOperator() {
            float fx = 95795.8756f;
            NullableSingle nx =  new NullableSingle(fx);
            nua.AssertEquals("TestI#21", fx, (float)nx);
        }


        [nu.Test, nu.ExpectedException(typeof(NullableNullValueException))]
        public void NullableSingleToSingleConversionOperatorNullValueException() {
            float f = (float)_null;
        }


        [nu.Test]
        public void NullableStringConversionOperator() {
            nua.AssertEquals("TestI#31", NullableSingle.Null, (NullableSingle)NullableString.Null);
            
            float fx = 95795.23445f;
            NullableString nx =  new NullableString(fx.ToString());
            nua.AssertEquals("TestI#32", fx, ((NullableSingle)nx).Value);

            NullableString nMax =  new NullableString(sys.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.PositiveInfinitySymbol);
            nua.Assert("TestI#33", float.IsPositiveInfinity(((NullableSingle)nMax).Value));
        }
        

        [nu.Test]
        public void SingleToNullableSingleConversionOperator() {
            float fx = 95795.87256f;
            NullableSingle nx =  fx;
            nua.AssertEquals("TestI#41", fx, nx.Value);
        }


        [nu.Test]
        public void NullableByteConversionOperator() {
            nua.AssertEquals("TestI#51", NullableSingle.Null, (NullableSingle)NullableByte.Null);
            
            byte bx = 12;
            NullableByte nbx =  new NullableByte(bx);
            nua.AssertEquals("TestI#52", bx, ((NullableSingle)nbx).Value);
        }


        [nu.Test]
        public void NullableDecimalConversionOperator() {
            nua.AssertEquals("TestI#61", NullableSingle.Null, (NullableSingle)NullableDecimal.Null);
            
            float fx = 2314.2344f;
            NullableDecimal nx =  new NullableDecimal((decimal)fx);
            nua.AssertEquals("TestI#62", fx, ((NullableSingle)nx).Value);

            float fy = (float)decimal.MaxValue;
            NullableDecimal ny =  new NullableDecimal(decimal.MaxValue);
            nua.AssertEquals("TestI#63", fy, ((NullableSingle)ny).Value);

        }


        [nu.Test]
        public void NullableInt16ConversionOperator() {
            nua.AssertEquals("TestI#71", NullableSingle.Null, (NullableSingle)NullableInt16.Null);
            
            short sx = 12;
            NullableInt16 nx =  new NullableInt16(sx);
            nua.AssertEquals("TestI#72", sx, ((NullableSingle)nx).Value);
        }


        [nu.Test]
        public void NullableInt32ConversionOperator() {
            nua.AssertEquals("TestI#81", NullableSingle.Null, (NullableSingle)NullableInt32.Null);
            
            int ix = 342093;
            NullableInt32 nx =  new NullableInt32(ix);
            nua.AssertEquals("TestI#82", ix, ((NullableSingle)nx).Value);
        }


        [nu.Test]
        public void NullableInt64ConversionOperator() {
            nua.AssertEquals("TestI#91", NullableSingle.Null, (NullableSingle)NullableInt64.Null);
            
            long lx = 342093L;
            NullableInt64 nx =  new NullableInt64(lx);
            nua.AssertEquals("TestI#92", lx, ((NullableSingle)nx).Value);
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
            NullableSingle n = new NullableSingle(b);
            nua.AssertEquals("TestJ#11", NullableByte.Null, _null.ToNullableByte());
            nua.AssertEquals("TestJ#12", b, n.ToNullableByte().Value);
            nua.AssertEquals("TestJ#13", new NullableByte(b), n.ToNullableByte());
        }


        [nu.Test]
        public void ToNullableDecimal() {
            decimal  d = 6474;
            NullableSingle n = new NullableSingle((int)d);
            nua.AssertEquals("TestJ#21", NullableDecimal.Null, _null.ToNullableDecimal());
            nua.AssertEquals("TestJ#22", d, n.ToNullableDecimal().Value);
            nua.AssertEquals("TestJ#23", new NullableDecimal(d), n.ToNullableDecimal());
        }


        [nu.Test]
        public void ToNullableDouble() {
            double  d = 6474;
            NullableSingle n = new NullableSingle((int)d);
            nua.AssertEquals("TestJ#31", NullableDouble.Null, _null.ToNullableDouble());
            nua.AssertEquals("TestJ#32", d, n.ToNullableDouble().Value);
            nua.AssertEquals("TestJ#33", new NullableDouble(d), n.ToNullableDouble());
        }


        [nu.Test]
        public void ToNullableInt16() {
            short s = 8923;
            NullableSingle n = new NullableSingle(s);
            nua.AssertEquals("TestJ#41", NullableInt16.Null, _null.ToNullableInt16());
            nua.AssertEquals("TestJ#42", s, n.ToNullableInt16().Value);
            nua.AssertEquals("TestJ#43", new NullableInt16(s), n.ToNullableInt16());
        }
    
        
        [nu.Test]
        public void ToNullableInt32() {
            int i = 8923;
            NullableSingle n = new NullableSingle(i);
            nua.AssertEquals("TestJ#41", NullableInt32.Null, _null.ToNullableInt32());
            nua.AssertEquals("TestJ#42", i, n.ToNullableInt32().Value);
            nua.AssertEquals("TestJ#43", new NullableInt32(i), n.ToNullableInt32());
        }
 
           
        [nu.Test]
        public void ToNullableInt64() {
            long l = 8923;
            NullableSingle n = new NullableSingle((int)l);
            nua.AssertEquals("TestJ#51", NullableInt64.Null, _null.ToNullableInt64());
            nua.AssertEquals("TestJ#52", l, n.ToNullableInt64().Value);
            nua.AssertEquals("TestJ#53", new NullableInt64(l), n.ToNullableInt64());
        }
            
            

           
        [nu.Test]
        public void ToNullableString() {
            int i = 6474;
            NullableSingle n = new NullableSingle(i);
            nua.AssertEquals("TestJ#81", NullableString.Null, _null.ToNullableString());
            nua.AssertEquals("TestJ#82", i.ToString(), n.ToNullableString().Value);
            nua.AssertEquals("TestJ#83", new NullableString(i.ToString()), n.ToNullableString());
        }


        [nu.Test]
        public void ToStringTest() {
            int i = 6474;
            NullableSingle n = new NullableSingle(i);
            nua.AssertEquals("TestJ#91", "Null", _null.ToString());
            nua.AssertEquals("TestJ#92", i.ToString(), n.ToString());
        }
        #endregion // Conversion Tests - J#

        #region Serialization - K#
        [nu.Test]
        public void SerializableAttribute() {
            NullableSingle serializedDeserialized;

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

        private NullableSingle SerializeDeserialize(NullableSingle x) {
            System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer = 
                new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableSingle y = (NullableSingle)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }


        [nu.Test]
        public void XmlSerializable() {
            NullableSingle xmlSerializedDeserialized;

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

        private NullableSingle XmlSerializeDeserialize(NullableSingle x) {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableSingle));
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));
        
                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableSingle y = (NullableSingle)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }

        
        [nu.Test]
        public void XmlSerializableEmptyElementNil() {
            // Bug reported by Shaun Bowe (sbowe@users.sourceforge.net)
            // http://sourceforge.net/forum/message.php?msg_id=2399265

            //<?xml version="1.0"?>
            //<NullableSingle xsi:nil="true" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"></NullableSingle>

            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableSingle));

            using (sys.IO.MemoryStream baseStream = new sys.IO.MemoryStream()) {
                using (sys.IO.StreamWriter stream = new System.IO.StreamWriter(baseStream)) {
                    stream.WriteLine("<?xml version=\"1.0\"?>");
                    stream.WriteLine("<NullableSingle xsi:nil=\"true\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></NullableSingle>");
                    stream.Flush();

//                    baseStream.Position = 0;
//                    sys.IO.StreamReader streamReader = new System.IO.StreamReader(baseStream);
//                    sys.Console.WriteLine(streamReader.ReadToEnd());
                    
                    baseStream.Position = 0; // Return stream to start
                    NullableSingle y = (NullableSingle)serializer.Deserialize(baseStream);

                    nua.Assert(y.IsNull);

                    baseStream.Close();
                    stream.Close();
                }
            }
        }

        [nu.Test]
        public void XmlSerializableSchema() {
            sysXmlScm.XmlSchema xsd = 
                ((sysXml.Serialization.IXmlSerializable)NullableSingle.Null).GetSchema();

            xsd.Compile(new sysXmlScm.ValidationEventHandler(ValidationCallBack));

            ValidateXmlAgainstXmlSchema(xsd, _null);
            ValidateXmlAgainstXmlSchema(xsd, _min);
            ValidateXmlAgainstXmlSchema(xsd, _max);

        }

        private void ValidateXmlAgainstXmlSchema(sysXmlScm.XmlSchema xsd, NullableSingle x) {
            sysXml.Serialization.XmlSerializer serializer = 
                new sysXml.Serialization.XmlSerializer(typeof(NullableSingle));

                
            sys.IO.MemoryStream stream = null;
            sys.Xml.XmlValidatingReader validator = null;
            try {
                // Get the serialized NullableSingle instance
                stream = new sys.IO.MemoryStream();
                serializer.Serialize(stream, x);
                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start

                // Add the default namespace
                sysXml.XmlDocument doc = new sysXml.XmlDocument();
                doc.Load(stream);                    
                sysXml.XmlAttribute defaultNs = doc.CreateAttribute("xmlns");
                defaultNs.Value = "http://NullableTypes.SourceForge.Net/NullableSingleXMLSchema";
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