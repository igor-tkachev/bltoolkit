//
// NullableTypes.Tests.NullableDoubleTest
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 14-Apr-2003  Luca    Create
// 23-Aug-2003  Luca    Upgrade    Code upgrade: Replaced new CultureInfo("") with equivalent 
//                                 CultureInfo.InvariantCulture
// 13-Sep-2003  Luca    Upgrade    New test: SerializableAttribute
// 15-Sep-2003  Luca    Upgrade    Code upgrade: improved test Parse to make it independent 
//                                 from user regional settings
// 05-Oct-2003  DamienG Upgrade    New test: XmlSerializable
//                                 New test: Added additional tests to SerializableAttribute
//                                 Code upgrade: Changed _pos and _neg to be double precision, added _negDec
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
    using sysThr = System.Threading;
    using sysGlb = System.Globalization;
    using sysXml = System.Xml;
    using sysXmlScm = System.Xml.Schema;
    
    [nu.TestFixture]
    public class NullableDoubleTest {

        NullableDouble _pos;
        NullableDouble _neg;
        NullableDouble _zero;
        NullableDouble _null;
        NullableDouble _notNull;

        NullableDouble _negInf;
        NullableDouble _eps;
        NullableDouble _negEps;
        NullableDouble _posInf;
        NullableDouble _nan;

        NullableDouble _negDec;
        NullableDouble _min;
        NullableDouble _max;

        [nu.SetUp]
        public void SetUp() {
            _pos = new NullableDouble(244353456.34768);
            _neg = new NullableDouble(-982343.03681);
            _negDec = new NullableDouble(-0.000000000001);
            _zero = new NullableDouble(0);
            _null = NullableDouble.Null;
            _notNull = new NullableDouble(90356);

            _negInf = new NullableDouble(double.NegativeInfinity);
            _eps = new NullableDouble(double.Epsilon);
            _negEps = new NullableDouble(-double.Epsilon);
            _posInf = new NullableDouble(double.PositiveInfinity);
            _nan = new NullableDouble(double.NaN);

            _min = NullableDouble.MinValue;
            _max = NullableDouble.MaxValue;
        }


        #region Field Tests - A#
        [nu.Test]
        public void MaxValue() {
            nua.AssertEquals("TestA#01", double.MaxValue, NullableDouble.MaxValue.Value);
        }


        [nu.Test]
        public void MinValue() {
            nua.AssertEquals("TestA#02", double.MinValue, NullableDouble.MinValue.Value);
        }


        [nu.Test]
        public void Zero() {
            nua.AssertEquals("TestA#03", 0d, NullableDouble.Zero.Value);
        }


        [nu.Test]
        public void Null() {
            nua.Assert ("TestA#04", NullableDouble.Null.IsNull);
        }
        #endregion // Field Tests - A#

        #region Constructor Tests - B#
        [nu.Test]
        public void Create() {
            double dPino = 34.87d;
            NullableDouble pino = new NullableDouble(dPino);                      
            nua.AssertEquals("TestB#01", dPino, pino.Value);

            nua.Assert("TestB#03", double.IsPositiveInfinity(new NullableDouble(double.PositiveInfinity).Value));
            nua.Assert("TestB#04", double.IsNegativeInfinity(new NullableDouble(double.NegativeInfinity).Value));
            nua.AssertEquals("TestB#05", double.Epsilon, new NullableDouble(double.Epsilon).Value);
            nua.Assert("TestB#06", double.IsNaN(new NullableDouble(double.NaN).Value));

        }


        [nu.Test]
        public void CreateFromSingle() {
            nua.Assert("TestB#11", double.IsPositiveInfinity(new NullableDouble(float.PositiveInfinity).Value));
            nua.AssertEquals("TestB#12", (double)float.MaxValue, new NullableDouble(float.MaxValue).Value);

            nua.AssertEquals("TestB#13", (double)+float.Epsilon, new NullableDouble(+float.Epsilon).Value);
            nua.AssertEquals("TestB#14", (double)-float.Epsilon, new NullableDouble(-float.Epsilon).Value);

            nua.AssertEquals("TestB#15", (double)float.MinValue, new NullableDouble(float.MinValue).Value);
            nua.Assert("TestB#16", double.IsNegativeInfinity(new NullableDouble(float.NegativeInfinity).Value));

            nua.Assert("TestB#17", double.IsNaN(new NullableDouble(float.NaN).Value));
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

            NullableDouble _bigPos = _pos * 2;
            nua.Assert("TestD#01", (((sys.IComparable)_pos).CompareTo(_null) > 0));
            nua.Assert("TestD#02", (((sys.IComparable)_pos).CompareTo(_neg) > 0));
            nua.Assert("TestD#03", (((sys.IComparable)_pos).CompareTo(_bigPos) < 0));
            nua.Assert("TestD#04", (((sys.IComparable)_pos).CompareTo(_pos) == 0));

            nua.Assert("TestD#05", (((sys.IComparable)_null).CompareTo(_pos) < 0));
            nua.Assert("TestD#06", (((sys.IComparable)_null).CompareTo(_neg) < 0));
            nua.Assert("TestD#07", (((sys.IComparable)_null).CompareTo(NullableDouble.Null) == 0));

            NullableDouble _bigNeg = _neg * 2;
            nua.Assert("TestD#08", (((sys.IComparable)_neg).CompareTo(_null) > 0));
            nua.Assert("TestD#09", (((sys.IComparable)_neg).CompareTo(_zero) < 0));
            nua.Assert("TestD#11", (((sys.IComparable)_neg).CompareTo(_bigNeg) > 0));
            nua.Assert("TestD#12", (((sys.IComparable)_neg).CompareTo(_neg) == 0));
        }


        [nu.Test]
        public void CompareNanInfEps() {
            
            nua.Assert("TestD#21", (((sys.IComparable)_negInf).CompareTo(NullableDouble.MinValue) < 0));
            nua.Assert("TestD#22", (((sys.IComparable)NullableDouble.MinValue).CompareTo(_negEps) < 0));
            nua.Assert("TestD#23", (((sys.IComparable)_negEps).CompareTo(NullableDouble.Zero) < 0));
            nua.Assert("TestD#24", (((sys.IComparable)NullableDouble.Zero).CompareTo(_eps) < 0));

            nua.Assert("TestD#25", (((sys.IComparable)_eps).CompareTo(NullableDouble.MaxValue) < 0));
            nua.Assert("TestD#26", (((sys.IComparable)NullableDouble.MaxValue).CompareTo(_posInf) < 0));

            nua.Assert("TestD#27", (((sys.IComparable)_nan).CompareTo(_eps) < 0));
            nua.Assert("TestD#28", (((sys.IComparable)_nan).CompareTo(_posInf) < 0));
            nua.Assert("TestD#29", (((sys.IComparable)_nan).CompareTo(_negInf) < 0));

            nua.Assert("TestD#30", (((sys.IComparable)_nan).CompareTo(_nan) == 0));
            nua.Assert("TestD#28", (((sys.IComparable)_posInf).CompareTo(_posInf) == 0));
            nua.Assert("TestD#29", (((sys.IComparable)_negInf).CompareTo(_negInf) == 0));
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
            NullableDouble max = new NullableDouble(double.MaxValue);
            nua.AssertEquals("TestE#01",double.MaxValue, max.Value);

            NullableDouble min = new NullableDouble(double.MinValue);
            nua.AssertEquals("TestE#02", double.MinValue, min.Value);

            double f = 89426.7634d;
            nua.AssertEquals("TestE#03", f, new NullableDouble(f).Value);

            //NullableDouble nan = new NullableDouble(double.NaN);
            //nua.AssertEquals("TestE#04", double.NaN, nan.Value);

            //NullableDouble posInf = new NullableDouble(double.PositiveInfinity);
            //nua.AssertEquals("TestE#05", double.PositiveInfinity, posInf.Value);

            //NullableDouble negInf = new NullableDouble(double.NegativeInfinity);
            //nua.AssertEquals("TestE#06", double.NegativeInfinity, negInf.Value);

            NullableDouble eps = new NullableDouble(double.Epsilon);
            nua.AssertEquals("TestE#06", double.Epsilon, eps.Value);
        }


        [nu.Test]
        [nu.ExpectedException(typeof(NullableTypes.NullableNullValueException))]
        public void ValuePropertyNull() {
            double sVal = _null.Value;
        }
        #endregion // Property Tests - E#

        #region Equivalence Tests - F#
        [nu.Test]
        public void StaticEqualsAndEqualityOperator() {
            // Case 1: either is NullableDouble.Null
            nua.AssertEquals("TestF#01", NullableBoolean.Null, _null == _zero);
            nua.AssertEquals("TestF#02", NullableBoolean.Null, NullableDouble.Equals(_neg, _null));
            nua.AssertEquals("TestF#03", NullableBoolean.Null, NullableDouble.NotEquals(_neg, _null));

            // Case 2: both are NullableDouble.Null
            nua.AssertEquals("TestF#04", NullableBoolean.Null, _null == NullableDouble.Null);
            nua.AssertEquals("TestF#05", NullableBoolean.Null, NullableDouble.Equals(NullableDouble.Null, _null));
            nua.AssertEquals("TestF#06", NullableBoolean.Null, NullableDouble.NotEquals(NullableDouble.Null, _null));

            // Case 3: both are equal
            NullableDouble x = _pos;
            nua.AssertEquals("TestF#07", NullableBoolean.True, x == _pos);
            nua.AssertEquals("TestF#08", NullableBoolean.True, NullableDouble.Equals(_pos, x));
            nua.AssertEquals("TestF#09", NullableBoolean.False, NullableDouble.NotEquals(_pos, x));

            // Case 4: inequality
            nua.AssertEquals("TestF#10", NullableBoolean.False, _zero == _neg);
            nua.AssertEquals("TestF#11", NullableBoolean.False, NullableDouble.Equals(_pos, _neg));
            nua.AssertEquals("TestF#12", NullableBoolean.True, NullableDouble.NotEquals(_pos, _neg));
        }


        [nu.Test]
        public void StaticEqualsAndEqualityOperatorNanInfEps() {
            // Case 1: both are NaN
            nua.AssertEquals("TestF#11", NullableBoolean.False, _nan == _nan);
            nua.AssertEquals("TestF#12", NullableBoolean.True, NullableDouble.Equals(_nan, _nan));
            nua.AssertEquals("TestF#13", NullableBoolean.False, NullableDouble.NotEquals(_nan, _nan));

            // Case 2: both are PositiveInfinity
            nua.AssertEquals("TestF#21", NullableBoolean.True, _posInf == _posInf);
            nua.AssertEquals("TestF#22", NullableBoolean.True, NullableDouble.Equals(_posInf, _posInf));
            nua.AssertEquals("TestF#23", NullableBoolean.False, NullableDouble.NotEquals(_posInf, _posInf));

            // Case 3: both are NegativeInfinity
            nua.AssertEquals("TestF#31", NullableBoolean.True, _negInf == _negInf);
            nua.AssertEquals("TestF#32", NullableBoolean.True, NullableDouble.Equals(_negInf, _negInf));
            nua.AssertEquals("TestF#33", NullableBoolean.False, NullableDouble.NotEquals(_negInf, _negInf));
        }


        [nu.Test]
        public void Equals() {
            // Case 1: either is NullableInt32.Null
            nua.Assert("TestF#101", !_null.Equals(_zero));
            nua.Assert("TestF#102", !_neg.Equals(_null));

            // Case 2: both are NullableInt32.Null
            nua.Assert("TestF#103", _null.Equals(NullableDouble.Null));
            nua.Assert("TestF#104", NullableDouble.Null.Equals(_null));

            // Case 3: both are equal
            NullableDouble x = _pos;
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
            double fx = -97644.444d;
            double fy = 2.234d;
            
            NullableDouble x = new NullableDouble(fx);
            NullableDouble y = new NullableDouble(fy);;

            nua.AssertEquals("TestG#01", fx + fy, NullableDouble.Add(x,y).Value);
        }


        [nu.Test]
        public void Divide() {
            double fx = -9537095.76d;
            double fy = -143.7346d;
            
            NullableDouble x = new NullableDouble(fx);
            NullableDouble y = new NullableDouble(fy);

            nua.AssertEquals("TestG#10", fx/fy, NullableDouble.Divide(x,y).Value);
        }


        [nu.Test]
        public void GreaterThan() {
            double fx = -9537095.2314d;
            double fy = 143.234d;
            
            NullableDouble x = new NullableDouble(fx);
            NullableDouble y = new NullableDouble(fy);

            nua.AssertEquals("TestG#20", fx > fy, NullableDouble.GreaterThan(x,y).Value);
        }


        [nu.Test]
        public void GreaterThanOrEquals() {
            double fx = -9537095.245d;
            double fy = 143.356d;
            
            NullableDouble x = new NullableDouble(fx);
            NullableDouble y = new NullableDouble(fy);

            nua.AssertEquals("TestG#31", fx >= fy, NullableDouble.GreaterThanOrEqual(x,y).Value);
            nua.AssertEquals("TestG#32", fx >= fx, NullableDouble.GreaterThanOrEqual(x,x).Value);
        }


        [nu.Test]
        public void LessThan() {
            double fx = 8450.234d;
            double fy = 6797346.234d;
            
            NullableDouble x = new NullableDouble(fx);
            NullableDouble y = new NullableDouble(fy);

            nua.AssertEquals("TestG#40", fx < fy, NullableDouble.LessThan(x,y).Value);
        }


        [nu.Test]
        public void LessThanOrEquals() {
            double fx = 236524.4343344d;
            double fy = 143.4d;
            
            NullableDouble x = new NullableDouble(fx);
            NullableDouble y = new NullableDouble(fy);

            nua.AssertEquals("TestG#51", fx <= fy, NullableDouble.LessThanOrEqual(x,y).Value);
            nua.AssertEquals("TestG#52", fx <= fx, NullableDouble.LessThanOrEqual(x,x).Value);
        }


        [nu.Test]
        public void Multiply() {
            double fx = 450.234424d;
            double fy = 397.232323234d;
            
            NullableDouble x = new NullableDouble(fx);
            NullableDouble y = new NullableDouble(fy);

            nua.AssertEquals("TestG#60", fx * fy, NullableDouble.Multiply(x,y).Value);
        }


        [nu.Test]
        public void Parse() {
            sysGlb.CultureInfo currCult = sys.Threading.Thread.CurrentThread.CurrentCulture;
            try {
                sysThr.Thread.CurrentThread.CurrentCulture = sysGlb.CultureInfo.InvariantCulture;
            
                string sx = "94357.2332";            
                NullableDouble x = NullableDouble.Parse(sx);

                nua.AssertEquals("TestG#70", double.Parse(sx), x.Value);
            }
            finally {
                sysThr.Thread.CurrentThread.CurrentCulture = currCult;
            }
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ParseArgumentNullException() {
            string sx = null;            
            NullableDouble.Parse(sx);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ParseFormatException() {
            string sx = "409'85";            
            NullableDouble.Parse(sx);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void ParseOverflowException() {
            // Note: ToString("G") round up to a number bigger then MaxValue
            string sx = (double.MaxValue).ToString("G"); 
            NullableDouble.Parse(sx);
        }


        [nu.Test]
        public void Subtract() {
            double fx = 239.50284d;
            double fy = 4376.9456d;
            
            NullableDouble x = new NullableDouble(fx);
            NullableDouble y = new NullableDouble(fy);;

            nua.AssertEquals("TestG#80", fx - fy, NullableDouble.Subtract(x,y).Value);
        }
        #endregion // Method Tests - G#

        #region Operator Tests - H#
        [nu.Test]
        public void AddOperator() {
            double fx = 23523.275d;
            double fy = -123.872d;
            
            NullableDouble nx = new NullableDouble(fx);
            NullableDouble ny = new NullableDouble(fy);
            NullableDouble nMax = new NullableDouble(double.MaxValue);
            
            // Add nullable doubles
            nua.AssertEquals("TestH#01", (fx + fy), (nx + ny).Value);
            nua.AssertEquals("TestH#02", fx, (nx + _zero).Value);
            nua.AssertEquals("TestH#03", (double.MaxValue + double.MaxValue), (nMax + nMax).Value);

            // Add Nulls
            nua.Assert("TestH#04", (nx + _null).IsNull);
            nua.Assert("TestH#05", (_null + ny).IsNull);
            nua.Assert("TestH#06", (_null + NullableDouble.Null).IsNull);

            // Add doubles nullable doubles
            nua.AssertEquals("TestH#07", (fx + fy), (fx + ny).Value);
            nua.AssertEquals("TestH#08", fy, (ny + 0).Value);
        }


        [nu.Test]
        public void DivideOperator() {
            double fx = 027457.65d;
            double fy = -64564.746d;
            
            NullableDouble nx = new NullableDouble(fx);
            NullableDouble ny = new NullableDouble(fy);
            NullableDouble nMax = new NullableDouble(double.MaxValue);
            
            // Divide nullable doubles
            nua.AssertEquals("TestH#11", (fx / fy), (nx / ny).Value);
            nua.AssertEquals("TestH#12", (double.MaxValue / double.Epsilon), (nMax / _eps).Value);
            nua.AssertEquals("TestH#13", 0, (_zero / ny).Value);

            // Divide Nulls
            nua.Assert("TestH#14", (nx / _null).IsNull);
            nua.Assert("TestH#15", (_null / ny).IsNull);
            nua.Assert("TestH#16", (_null / NullableDouble.Null).IsNull);

            // Divide doubles nullable doubles
            nua.AssertEquals("TestH#17", (fx / fy), (fx / ny).Value);
            nua.AssertEquals("TestH#18", 0, (0 / ny).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.DivideByZeroException))]
        public void DivideOperatorDivideByZero() {
            NullableDouble nx = new NullableDouble(82479.726);
            NullableDouble ny = new NullableDouble(0);
            
            NullableDouble nz = nx / ny;
        }


        [nu.Test]
        public void GreaterThanOperator() {
            NullableDouble _bigPos = _pos * 2;
            NullableDouble _bigNeg = _neg * 2;

            // GreaterThan nulls
            nua.Assert("TestH#21", (_pos > _null).IsNull);
            nua.Assert("TestH#22", (_null > _zero).IsNull);
            nua.Assert("TestH#23", (_null > _neg).IsNull);
            nua.Assert("TestH#24", (_null > NullableDouble.Null).IsNull);

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
            NullableDouble _bigPos = _pos * 2;
            NullableDouble _bigNeg = _neg * 2;

            // GreaterThanOrEquals nulls
            nua.Assert("TestH#41", (_pos >= _null).IsNull);
            nua.Assert("TestH#42", (_null >= _zero).IsNull);
            nua.Assert("TestH#43", (_null >= _neg).IsNull);
            nua.Assert("TestH#44", (_null >= NullableDouble.Null).IsNull);

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
            NullableDouble _bigPos = _pos * 2;
            NullableDouble _bigNeg = _neg * 2;

            // LessThan nulls
            nua.Assert("TestH#71", (_pos < _null).IsNull);
            nua.Assert("TestH#72", (_null < _zero).IsNull);
            nua.Assert("TestH#73", (_null < _neg).IsNull);
            nua.Assert("TestH#74", (_null < NullableDouble.Null).IsNull);

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
            NullableDouble _bigPos = _pos * 2;
            NullableDouble _bigNeg = _neg * 2;

            // LessThanOrEquals nulls
            nua.Assert("TestH#91", (_pos <= _null).IsNull);
            nua.Assert("TestH#92", (_null <= _zero).IsNull);
            nua.Assert("TestH#93", (_null <= _neg).IsNull);
            nua.Assert("TestH#94", (_null <= NullableDouble.Null).IsNull);

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
            double fx = 027457.2345d;
            double fy = -64564.243d;
            
            NullableDouble nx = new NullableDouble(fx);
            NullableDouble ny = new NullableDouble(fy);
            NullableDouble nMin = new NullableDouble(double.MinValue);
            NullableDouble nMax = new NullableDouble(double.MaxValue);
            
            // Multiply nullable ints
            nua.AssertEquals("TestH#121", (fx * fy), (nx * ny).Value);
            nua.AssertEquals("TestH#122", (double.MinValue * double.MaxValue), (nMin * nMax).Value);
            nua.AssertEquals("TestH#123", 0, (_zero * ny).Value);

            // Multiply Nulls
            nua.Assert("TestH#124", (nx * _null).IsNull);
            nua.Assert("TestH#125", (_null * ny).IsNull);
            nua.Assert("TestH#126", (_null * NullableDouble.Null).IsNull);

            // Multiply ints nullable ints
            nua.AssertEquals("TestH#127", (fx * fy), (fx * ny).Value);
            nua.AssertEquals("TestH#128", 0, (0 * ny).Value);
        }


        [nu.Test]
        public void SubtractionOperator() {
            double fx = 985.345d;
            double fy = -4534.234d;
            
            NullableDouble nx = new NullableDouble(fx);
            NullableDouble ny = new NullableDouble(fy);
            NullableDouble nMin = new NullableDouble(double.MinValue);
            NullableDouble nMax = new NullableDouble(double.MaxValue);
            
            // Subtraction nullable ints
            nua.AssertEquals("TestH#131", (fx - fy), (nx - ny).Value);
            nua.AssertEquals("TestH#132", (double.MinValue - double.MaxValue), (nMin - nMax).Value);
            nua.AssertEquals("TestH#133", fx, (nx - _zero).Value);

            // Subtraction Nulls
            nua.Assert("TestH#134", (nx - _null).IsNull);
            nua.Assert("TestH#135", (_null - ny).IsNull);
            nua.Assert("TestH#136", (_null - NullableDouble.Null).IsNull);

            // Subtraction ints nullable ints
            nua.AssertEquals("TestH#137", (fx - fy), (fx - ny).Value);
            nua.AssertEquals("TestH#138", fy, (ny - 0).Value);
        }



        [nu.Test]
        public void UnaryNegationOperator() {
            double dx = 905635.234d;        
            NullableDouble nx = new NullableDouble(dx);
            
            // UnaryNegation nullable ints
            nua.AssertEquals("TestH#171", -dx, (-nx).Value, 0);         
            nua.AssertEquals("TestH#172", (-0), (-_zero).Value, 0);

            // UnaryNegation Nulls
            nua.Assert("TestH#173", (-_null).IsNull);
            nua.Assert("TestH#174", (-NullableDouble.Null).IsNull);
        }
        #endregion // Operator Tests - H#

        #region Conversion Operator Tests - I#
        [nu.Test]
        public void NullableBooleanConversionOperator() {
            nua.AssertEquals("TestI#01", NullableDouble.Null, (NullableDouble)NullableBoolean.Null);
            nua.AssertEquals("TestI#02", new NullableDouble(0), (NullableDouble)NullableBoolean.False);
            nua.AssertEquals("TestI#03", new NullableDouble(1), (NullableDouble)NullableBoolean.True);
        }


        [nu.Test]
        public void NullableDoubleConversionOperator() {
            nua.AssertEquals("TestI#11", NullableDouble.Null, (NullableDouble)NullableDouble.Null);
            
            double fx = 95795.7625d;
            NullableDouble nx =  new NullableDouble(fx);
            nua.AssertEquals("TestI#12", fx, ((NullableDouble)nx).Value);
        }


        [nu.Test]
        public void NullableDoubleToSingleConversionOperator() {
            double fx = 95795.8756d;
            NullableDouble nx =  new NullableDouble(fx);
            nua.AssertEquals("TestI#21", fx, (double)nx);
        }


        [nu.Test, nu.ExpectedException(typeof(NullableNullValueException))]
        public void NullableDoubleToSingleConversionOperatorNullValueException() {
            double f = (double)_null;
        }


        [nu.Test]
        public void NullableStringConversionOperator() {
            nua.AssertEquals("TestI#31", NullableDouble.Null, (NullableDouble)NullableString.Null);
            
            double dx = 95795.23445d;
            NullableString nx =  new NullableString(dx.ToString());
            nua.AssertEquals("TestI#32", dx, ((NullableDouble)nx).Value, 0);

            NullableString ny =  new NullableString(sys.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.PositiveInfinitySymbol);
            nua.Assert("TestI#33", double.IsPositiveInfinity(((NullableDouble)ny).Value));
        
        }
  

        [nu.Test]
        public void SingleToNullableDoubleConversionOperator() {
            double fx = 95795.87256d;
            NullableDouble nx =  fx;
            nua.AssertEquals("TestI#41", fx, nx.Value);
        }


        [nu.Test]
        public void NullableByteConversionOperator() {
            nua.AssertEquals("TestI#51", NullableDouble.Null, (NullableDouble)NullableByte.Null);
            
            byte bx = 12;
            NullableByte nbx =  new NullableByte(bx);
            nua.AssertEquals("TestI#52", bx, ((NullableDouble)nbx).Value);
        }


        [nu.Test]
        public void NullableDecimalConversionOperator() {
            nua.AssertEquals("TestI#61", NullableDouble.Null, (NullableDouble)NullableDecimal.Null);
            
            double dx = 2314.2344d;
            NullableDecimal nx =  new NullableDecimal((decimal)dx);
            nua.AssertEquals("TestI#62", dx, ((NullableDouble)nx).Value);

            double dy = (double)decimal.MaxValue;
            NullableDecimal ny =  new NullableDecimal(decimal.MaxValue);
            nua.AssertEquals("TestI#63", dy, ((NullableDouble)ny).Value);
        }


        [nu.Test]
        public void NullableInt16ConversionOperator() {
            nua.AssertEquals("TestI#71", NullableDouble.Null, (NullableDouble)NullableInt16.Null);
            
            short sx = 12;
            NullableInt16 nx =  new NullableInt16(sx);
            nua.AssertEquals("TestI#72", sx, ((NullableDouble)nx).Value);
        }


        [nu.Test]
        public void NullableInt32ConversionOperator() {
            nua.AssertEquals("TestI#81", NullableDouble.Null, (NullableDouble)NullableInt32.Null);
            
            int ix = 342093;
            NullableInt32 nx =  new NullableInt32(ix);
            nua.AssertEquals("TestI#82", ix, ((NullableDouble)nx).Value);
        }


        [nu.Test]
        public void NullableInt64ConversionOperator() {
            nua.AssertEquals("TestI#91", NullableDouble.Null, (NullableDouble)NullableInt64.Null);
            
            long lx = 342093L;
            NullableInt64 nx =  new NullableInt64(lx);
            nua.AssertEquals("TestI#92", lx, ((NullableDouble)nx).Value);
        }




        [nu.Test]
        public void NullableSingleConversionOperator() {
            nua.AssertEquals("TestI#121", NullableDouble.Null, (NullableDouble)NullableSingle.Null);
            
            float fx = 95795.7625f;
            NullableDouble nx =  new NullableDouble(fx);
            nua.AssertEquals("TestI#122", fx, ((NullableSingle)nx).Value);
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
            NullableDouble n = new NullableDouble(b);
            nua.AssertEquals("TestJ#11", NullableByte.Null, _null.ToNullableByte());
            nua.AssertEquals("TestJ#12", b, n.ToNullableByte().Value);
            nua.AssertEquals("TestJ#13", new NullableByte(b), n.ToNullableByte());
        }


        [nu.Test]
        public void ToNullableDecimal() {
            decimal  d = 6474;
            NullableDouble n = new NullableDouble((int)d);
            nua.AssertEquals("TestJ#21", NullableDecimal.Null, _null.ToNullableDecimal());
            nua.AssertEquals("TestJ#22", d, n.ToNullableDecimal().Value);
            nua.AssertEquals("TestJ#23", new NullableDecimal(d), n.ToNullableDecimal());
        }


        [nu.Test]
        public void ToNullableSingle() {
            double  d = 6474;
            NullableDouble n = new NullableDouble((int)d);
            nua.AssertEquals("TestJ#31", NullableSingle.Null, _null.ToNullableSingle());
            nua.AssertEquals("TestJ#32", d, n.ToNullableSingle().Value);
            nua.AssertEquals("TestJ#33", new NullableSingle(d), n.ToNullableSingle());
        }


        [nu.Test]
        public void ToNullableInt16() {
            short s = 8923;
            NullableDouble n = new NullableDouble(s);
            nua.AssertEquals("TestJ#41", NullableInt16.Null, _null.ToNullableInt16());
            nua.AssertEquals("TestJ#42", s, n.ToNullableInt16().Value);
            nua.AssertEquals("TestJ#43", new NullableInt16(s), n.ToNullableInt16());
        }

            
        [nu.Test]
        public void ToNullableInt32() {
            int i = 8923;
            NullableDouble n = new NullableDouble(i);
            nua.AssertEquals("TestJ#41", NullableInt32.Null, _null.ToNullableInt32());
            nua.AssertEquals("TestJ#42", i, n.ToNullableInt32().Value);
            nua.AssertEquals("TestJ#43", new NullableInt32(i), n.ToNullableInt32());
        }

            
        [nu.Test]
        public void ToNullableInt64() {
            long l = 8923;
            NullableDouble n = new NullableDouble((int)l);
            nua.AssertEquals("TestJ#51", NullableInt64.Null, _null.ToNullableInt64());
            nua.AssertEquals("TestJ#52", l, n.ToNullableInt64().Value);
            nua.AssertEquals("TestJ#53", new NullableInt64(l), n.ToNullableInt64());
        }
            
            

            
        [nu.Test]
        public void ToNullableString() {
            int i = 6474;
            NullableDouble n = new NullableDouble(i);
            nua.AssertEquals("TestJ#81", NullableString.Null, _null.ToNullableString());
            nua.AssertEquals("TestJ#82", i.ToString(), n.ToNullableString().Value);
            nua.AssertEquals("TestJ#83", new NullableString(i.ToString()), n.ToNullableString());
        }


        [nu.Test]
        public void ToStringTest() {
            int i = 6474;
            NullableDouble n = new NullableDouble(i);
            nua.AssertEquals("TestJ#91", "Null", _null.ToString());
            nua.AssertEquals("TestJ#92", i.ToString(), n.ToString());
        }
        #endregion // Conversion Tests - J#

        #region Serialization - K#
        [nu.Test]
        public void SerializableAttribute() {
            NullableDouble serializedDeserialized;

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

        private NullableDouble SerializeDeserialize(NullableDouble x) {
            System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer = 
                new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableDouble y = (NullableDouble)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }


        [nu.Test]
        public void XmlSerializable() {
            NullableDouble xmlSerializedDeserialized;

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

        private NullableDouble XmlSerializeDeserialize(NullableDouble x) {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableDouble));
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));
        
                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableDouble y = (NullableDouble)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }


        [nu.Test]
        public void XmlSerializableEmptyElementNil() {
            // Bug reported by Shaun Bowe (sbowe@users.sourceforge.net)
            // http://sourceforge.net/forum/message.php?msg_id=2399265

            //<?xml version="1.0"?>
            //<NullableDouble xsi:nil="true" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"></NullableDouble>

            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableDouble));

            using (sys.IO.MemoryStream baseStream = new sys.IO.MemoryStream()) {
                using (sys.IO.StreamWriter stream = new System.IO.StreamWriter(baseStream)) {
                    stream.WriteLine("<?xml version=\"1.0\"?>");
                    stream.WriteLine("<NullableDouble xsi:nil=\"true\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></NullableDouble>");
                    stream.Flush();

//                    baseStream.Position = 0;
//                    sys.IO.StreamReader streamReader = new System.IO.StreamReader(baseStream);
//                    sys.Console.WriteLine(streamReader.ReadToEnd());
                    
                    baseStream.Position = 0; // Return stream to start
                    NullableDouble y = (NullableDouble)serializer.Deserialize(baseStream);

                    nua.Assert(y.IsNull);

                    baseStream.Close();
                    stream.Close();
                }
            }
        }


        [nu.Test]
        public void XmlSerializableSchema() {
            sysXmlScm.XmlSchema xsd = 
                ((sysXml.Serialization.IXmlSerializable)NullableDouble.Null).GetSchema();

            xsd.Compile(new sysXmlScm.ValidationEventHandler(ValidationCallBack));

            ValidateXmlAgainstXmlSchema(xsd, _null);
            ValidateXmlAgainstXmlSchema(xsd, _min);
            ValidateXmlAgainstXmlSchema(xsd, _max);

        }

        private void ValidateXmlAgainstXmlSchema(sysXmlScm.XmlSchema xsd, NullableDouble x) {
            sysXml.Serialization.XmlSerializer serializer = 
                new sysXml.Serialization.XmlSerializer(typeof(NullableDouble));

                
            sys.IO.MemoryStream stream = null;
            sys.Xml.XmlValidatingReader validator = null;
            try {
                // Get the serialized NullableDouble instance
                stream = new sys.IO.MemoryStream();
                serializer.Serialize(stream, x);
                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start

                // Add the default namespace
                sysXml.XmlDocument doc = new sysXml.XmlDocument();
                doc.Load(stream);                    
                sysXml.XmlAttribute defaultNs = doc.CreateAttribute("xmlns");
                defaultNs.Value = "http://NullableTypes.SourceForge.Net/NullableDoubleXMLSchema";
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