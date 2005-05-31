//
// NullableTypes.Tests.NullableByteTest
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 07-Apr-2003  Luca    Create
// 13-Sep-2003  Luca    Upgrade    New test: SerializableAttribute
// 04-Oct-2003  DamienG Upgrade    New test: XmlSerializable
//                                 Code upgrade: Changed _pos to 157 to ensure unsigned tests
//                                 Code upgrade: Introduced _bigPos to replace individual _pos * 2 ones
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
    public class NullableByteTest {

        NullableByte _pos;
        NullableByte _bigPos;
        NullableByte _zero;
        NullableByte _null;
        NullableByte _min;
        NullableByte _max;

        [nu.SetUp]
        public void SetUp() {
            _pos = new NullableByte(157);
            _bigPos = new NullableByte(250);
            _zero = new NullableByte(0);
            _null = NullableByte.Null;
            _min = NullableByte.MinValue;
            _max = NullableByte.MaxValue;
        }


        #region Field Tests - A#
        [nu.Test]
        public void FieldMax() {
            nua.AssertEquals("TestA#01", byte.MaxValue, NullableByte.MaxValue.Value);
        }


        [nu.Test]
        public void FieldMin() {
            nua.AssertEquals("TestA#02", byte.MinValue, NullableByte.MinValue.Value);
        }


        [nu.Test]
        public void NullField() {
            nua.Assert("TestA#03", NullableByte.Null.IsNull);
        }


        [nu.Test]
        public void ZeroField() {
            nua.AssertEquals("TestA#04", (byte)0, NullableByte.Zero.Value);
        }
        #endregion // Field Tests - A#

        #region Constructor Tests - B#
        [nu.Test]
        public void Create() {
            NullableByte pino = new NullableByte(123);
            nua.AssertEquals("TestB#01", (byte)123, pino.Value);
        }
        #endregion // Constructor Tests - B#

        #region INullable Tests - C#
        [nu.Test]
        public void IsNullProperty() {
            nua.Assert ("TestC#01", _null.IsNull);
            nua.Assert ("TestC#02", !_pos.IsNull);
        }
        #endregion // INullable Tests - C#

        #region IComparable - Ordering Tests - D#
        [nu.Test]
        public void Compare() {
            nua.Assert("TestD#01", (((sys.IComparable)_pos).CompareTo(_null) > 0));
            nua.Assert("TestD#02", (((sys.IComparable)_pos).CompareTo(_zero) > 0));
            nua.Assert("TestD#03", (((sys.IComparable)_pos).CompareTo(_bigPos) < 0));
            nua.Assert("TestD#04", (((sys.IComparable)_pos).CompareTo(_pos) == 0));

            nua.Assert("TestD#05", (((sys.IComparable)_null).CompareTo(_pos) < 0));
            nua.Assert("TestD#06", (((sys.IComparable)_null).CompareTo(_zero) < 0));
            nua.Assert("TestD#07", (((sys.IComparable)_null).CompareTo(NullableByte.Null) == 0));
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
            NullableByte max = new NullableByte(byte.MaxValue);
            nua.AssertEquals("TestE#01", byte.MaxValue, max.Value);

            NullableByte min = new NullableByte(byte.MinValue);
            nua.AssertEquals("TestE#02", byte.MinValue, min.Value);

            byte b = 45;
            nua.AssertEquals("TestE#03", b, new NullableByte(b).Value);
        }


        [nu.Test]
        [nu.ExpectedException(typeof(NullableTypes.NullableNullValueException))]
        public void ValuePropertyNull() {
            byte bVal = _null.Value;
        }
        #endregion // Property Tests - E#

        #region Equivalence Tests - F#
        [nu.Test]
        public void StaticEqualsAndEqualityOperator() {
            
            // Case 1: either is NullableByte.Null
            nua.AssertEquals("TestF#01", NullableBoolean.Null, _null == _zero);
            nua.AssertEquals("TestF#02", NullableBoolean.Null, NullableByte.Equals(_pos, _null));
            nua.AssertEquals("TestF#03", NullableBoolean.Null, NullableByte.NotEquals(_pos, _null));

            // Case 2: both are NullableByte.Null
            nua.AssertEquals("TestF#04", NullableBoolean.Null, _null == NullableByte.Null);
            nua.AssertEquals("TestF#05", NullableBoolean.Null, NullableByte.Equals(NullableByte.Null, _null));
            nua.AssertEquals("TestF#06", NullableBoolean.Null, NullableByte.NotEquals(NullableByte.Null, _null));

            // Case 3: both are equal
            NullableByte x = _pos;
            nua.AssertEquals("TestF#07", NullableBoolean.True, x == _pos);
            nua.AssertEquals("TestF#08", NullableBoolean.True, NullableByte.Equals(_pos, x));
            nua.AssertEquals("TestF#09", NullableBoolean.False, NullableByte.NotEquals(_pos, x));

            // Case 4: inequality
            nua.AssertEquals("TestF#10", NullableBoolean.False, _zero == _pos);
            nua.AssertEquals("TestF#11", NullableBoolean.False, NullableByte.Equals(_pos, _zero));
            nua.AssertEquals("TestF#12", NullableBoolean.True, NullableByte.NotEquals(_pos, _zero));
        }


        [nu.Test]
        public void Equals() {
            
            // Case 1: either is NullableInt32.Null
            nua.Assert("TestF#21", !_null.Equals(_zero));
            nua.Assert("TestF#22", !_pos.Equals(_null));
            nua.Assert("TestF#22b", !_pos.Equals(null));

            // Case 2: both are NullableInt32.Null
            nua.Assert("TestF#23", _null.Equals(NullableByte.Null));
            nua.Assert("TestF#24", NullableByte.Null.Equals(_null));

            // Case 3: both are equal
            NullableByte x = _pos;
            nua.Assert("TestF#25", x.Equals(_pos));
            nua.Assert("TestF#26", _pos.Equals(x));

            // Case 4: inequality
            nua.Assert("TestF#27", !_zero.Equals(_pos));
            nua.Assert("TestF#28", !_pos.Equals(_zero));
        }
        #endregion // Equivalence Tests - F#

        #region Method Tests - G#

        [nu.Test]
        public void Add() {
            byte bx = 97;
            byte by = 2;
            
            NullableByte x = new NullableByte(bx);
            NullableByte y = new NullableByte(by);;

            nua.AssertEquals("TestG#01", bx + by, NullableByte.Add(x,y).Value);
        }


        [nu.Test]
        public void BitwiseAnd() {
            byte bx = 187;
            byte by = 242;
                        
            NullableByte x = new NullableByte(bx);
            NullableByte y = new NullableByte(by);            

            nua.AssertEquals("TestG#20", bx & by, NullableByte.BitwiseAnd(x ,y).Value);
        }


        [nu.Test]
        public void BitwiseOr() {
            byte bx = 187;
            byte by = 242;
                        
            NullableByte x = new NullableByte(bx);
            NullableByte y = new NullableByte(by);            

            nua.AssertEquals("TestG#30", bx | by, NullableByte.BitwiseOr(x ,y).Value);
        }


        [nu.Test]
        public void Divide() {
            byte bx = 95;
            byte by = 143;
            
            NullableByte x = new NullableByte(bx);
            NullableByte y = new NullableByte(by);

            nua.AssertEquals("TestG#40", bx / by, NullableByte.Divide(x,y).Value);
        }


        [nu.Test]
        public void GreaterThan() {
            byte bx = 3;
            byte by = 143;
            
            NullableByte x = new NullableByte(bx);
            NullableByte y = new NullableByte(by);

            nua.AssertEquals("TestG#50", bx > by, NullableByte.GreaterThan(x,y).Value);
        }


        [nu.Test]
        public void GreaterThanOrEquals() {
            byte bx = 70;
            byte by = 143;
            
            NullableByte x = new NullableByte(bx);
            NullableByte y = new NullableByte(by);

            nua.AssertEquals("TestG#61", bx >= by, NullableByte.GreaterThanOrEqual(x,y).Value);
            nua.AssertEquals("TestG#62", bx >= bx, NullableByte.GreaterThanOrEqual(x,x).Value);
        }


        [nu.Test]
        public void LessThan() {
            byte bx = 84;
            byte by = 146;
            
            NullableByte x = new NullableByte(bx);
            NullableByte y = new NullableByte(by);

            nua.AssertEquals("TestG#70", bx < by, NullableByte.LessThan(x,y).Value);
        }


        [nu.Test]
        public void LessThanOrEquals() {
            byte bx = 132;
            byte by = 143;
            
            NullableByte x = new NullableByte(bx);
            NullableByte y = new NullableByte(by);

            nua.AssertEquals("TestG#81", bx <= by, NullableByte.LessThanOrEqual(x,y).Value);
            nua.AssertEquals("TestG#82", bx <= bx, NullableByte.LessThanOrEqual(x,x).Value);
        }


        [nu.Test]
        public void Mod() {
            byte bx = 93;
            byte by = 67;
            
            NullableByte x = new NullableByte(bx);
            NullableByte y = new NullableByte(by);

            nua.AssertEquals("TestG#90", bx % by, NullableByte.Mod(x,y).Value);
        }


        [nu.Test]
        public void Multiply() {
            byte bx = 5;
            byte by = 9;
            
            NullableByte x = new NullableByte(bx);
            NullableByte y = new NullableByte(by);

            nua.AssertEquals("TestG#100", bx * by, NullableByte.Multiply(x,y).Value);
        }


        [nu.Test]
        public void OnesComplement() {
            byte bx = 85;            
            NullableByte x = new NullableByte(bx);

            nua.AssertEquals("TestG#110", (byte)(~bx & 255), NullableByte.OnesComplement(x).Value);
        }


        [nu.Test]
        public void Parse() {
            string sx = "94";
            NullableByte x = NullableByte.Parse(sx);

            nua.AssertEquals("TestG#130", byte.Parse(sx), NullableByte.Parse(sx).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ParseArgumentNullException() {
            string sx = null;            
            NullableByte.Parse(sx);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ParseFormatException() {
            string sx = "409'85";            
            NullableByte.Parse(sx);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void ParseOverflowException() {
            string sx = (byte.MaxValue + 1L).ToString();            
            NullableByte.Parse(sx);
        }


        [nu.Test]
        public void Subtract() {
            byte bx = 56;
            byte by = 28;
            
            NullableByte x = new NullableByte(bx);
            NullableByte y = new NullableByte(by);;

            nua.AssertEquals("TestG#140", bx - by, NullableByte.Subtract(x,y).Value);
        }


        [nu.Test]
        public void Xor() {
            byte bx = 239;
            byte by = 45;
            
            NullableByte x = new NullableByte(bx);
            NullableByte y = new NullableByte(by);;

            nua.AssertEquals("TestG#150", bx ^ by, NullableByte.Xor(x,y).Value);
        }
        #endregion // Method Tests - G#

        #region Operator Tests - H#
        [nu.Test]
        public void AddOperator() {
            byte bx = 203;
            byte by = 11;
            
            NullableByte nx = new NullableByte(bx);
            NullableByte ny = new NullableByte(by);
            
            // Add nullable ints
            nua.AssertEquals("TestH#01", (bx + by), (nx + ny).Value);
            nua.AssertEquals("TestH#02", bx, (nx + _zero).Value);

            // Add Nulls
            nua.Assert("TestH#03", (nx + _null).IsNull);
            nua.Assert("TestH#04", (_null + ny).IsNull);
            nua.Assert("TestH#05", (_null + NullableByte.Null).IsNull);

            // Add ints nullable ints
            nua.AssertEquals("TestH#06", (bx + by), (bx + ny).Value);
            nua.AssertEquals("TestH#07", by, (ny + 0).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void AddOperatorOverflow() {
            byte bx = byte.MaxValue;
            byte by = 1;
            
            NullableByte nx = new NullableByte(bx);
            NullableByte ny = new NullableByte(by);
            
            NullableByte nz = nx + ny;
        }


        [nu.Test]
        public void BitwiseAndOperator() {
            byte bx = 46;
            byte by = 047;
            byte bi = 255;
            
            NullableByte nx = new NullableByte(bx);
            NullableByte ny = new NullableByte(by);
            
            // & nullable ints
            nua.AssertEquals("TestH#11", (bx & by), (nx & ny).Value);
            nua.AssertEquals("TestH#12", 0, (nx & _zero).Value);
            nua.AssertEquals("TestH#13", by, (bi & ny).Value);

            // & Nulls
            nua.Assert("TestH#14", (nx & _null).IsNull);
            nua.Assert("TestH#15", (_null & ny).IsNull);
            nua.Assert("TestH#16", (_null & NullableByte.Null).IsNull);

            // & ints nullable ints
            nua.AssertEquals("TestH#17", (bx & by), (bx & ny).Value);
            nua.AssertEquals("TestH#18", 0, (ny & 0).Value);
            nua.AssertEquals("TestH#19", by, (bi & ny).Value);
        }


        [nu.Test]
        public void BitwiseOrOperator() {
            byte bx = 153;
            byte by = 52;
            byte bi = 255;
            
            NullableByte nx = new NullableByte(bx);
            NullableByte ny = new NullableByte(by);
            
            // | nullable ints
            nua.AssertEquals("TestH#21", (bx | by), (nx | ny).Value);
            nua.AssertEquals("TestH#22", bx, (nx | _zero).Value);
            nua.AssertEquals("TestH#23", bi, (bi | ny).Value);

            // | Nulls
            nua.Assert("TestH#24", (nx | _null).IsNull);
            nua.Assert("TestH#25", (_null | ny).IsNull);
            nua.Assert("TestH#26", (_null | NullableByte.Null).IsNull);

            // | ints nullable ints
            nua.AssertEquals("TestH#27", (bx | by), (bx | ny).Value);
            nua.AssertEquals("TestH#28", by, (ny | 0).Value);
            nua.AssertEquals("TestH#29", bi, (bi | ny).Value);
        }


        [nu.Test]
        public void DivideOperator() {
            byte bx = 224;
            byte by = 14;
            
            NullableByte nx = new NullableByte(bx);
            NullableByte ny = new NullableByte(by);
            
            // Divide nullable ints
            nua.AssertEquals("TestH#31", (bx / by), (nx / ny).Value);
            nua.AssertEquals("TestH#32", 0, (_zero / ny).Value);

            // Divide Nulls
            nua.Assert("TestH#33", (nx / _null).IsNull);
            nua.Assert("TestH#34", (_null / ny).IsNull);
            nua.Assert("TestH#35", (_null / NullableByte.Null).IsNull);

            // Divide ints nullable ints
            nua.AssertEquals("TestH#36", (bx / by), (bx / ny).Value);
            nua.AssertEquals("TestH#37", 0, (0 / ny).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.DivideByZeroException))]
        public void DivideOperatorDivideByZero() {                       
            NullableByte nz = _pos / _zero;
        }


        [nu.Test]
        public void ExclusiveOrOperator() {
            byte bx = 123;
            byte by = 222;
            byte bi = 255;
            
            NullableByte nx = new NullableByte(bx);
            NullableByte ny = new NullableByte(by);
            
            // ^ nullable ints
            nua.AssertEquals("TestH#41", (bx ^ by), (nx ^ ny).Value);
            nua.AssertEquals("TestH#42", bx, (nx ^ _zero).Value);
            nua.AssertEquals("TestH#43", (byte)(~by & 255), (bi ^ ny).Value);

            // ^ Nulls
            nua.Assert("TestH#44", (nx ^ _null).IsNull);
            nua.Assert("TestH#45", (_null ^ ny).IsNull);
            nua.Assert("TestH#46", (_null ^ NullableByte.Null).IsNull);

            // ^ ints nullable ints
            nua.AssertEquals("TestH#47", (bx ^ by), (bx ^ ny).Value);
            nua.AssertEquals("TestH#48", by, (ny ^ 0).Value);
            nua.AssertEquals("TestH#49", (byte)(~by & 255), (bi ^ ny).Value);
        }


        [nu.Test]
        public void GreaterThanOperator() {
            // GreaterThan nulls
            nua.Assert("TestH#51", (_pos > _null).IsNull);
            nua.Assert("TestH#52", (_null > _zero).IsNull);
            nua.Assert("TestH#53", (_null > _zero).IsNull);
            nua.Assert("TestH#54", (_null > NullableByte.Null).IsNull);

            // GreaterThan nullable ints
            nua.Assert("TestH#55", (_pos > _zero).IsTrue);
            nua.Assert("TestH#56", (_pos > _bigPos).IsFalse);
            nua.Assert("TestH#57", (_pos > _pos).IsFalse);
            nua.Assert("TestH#59", (_zero > _pos).IsFalse);
            nua.Assert("TestH#61", (_zero > _zero).IsFalse);

            // GreaterThan ints
            nua.Assert("TestH#62", (_pos > _zero.Value).IsTrue);
            nua.Assert("TestH#63", (_pos > _bigPos.Value).IsFalse);
            nua.Assert("TestH#64", (_pos > _pos.Value).IsFalse);
            nua.Assert("TestH#65", (_zero > _pos.Value).IsFalse);
            nua.Assert("TestH#67", (_zero > _zero.Value).IsFalse);
        }


        [nu.Test]
        public void GreaterThanOrEqualsOperator() {
            // GreaterThanOrEquals nulls
            nua.Assert("TestH#71", (_pos >= _null).IsNull);
            nua.Assert("TestH#72", (_null >= _zero).IsNull);
            nua.Assert("TestH#74", (_null >= NullableByte.Null).IsNull);

            // GreaterThanOrEquals nullable ints
            nua.Assert("TestH#75", (_pos >= _zero).IsTrue);
            nua.Assert("TestH#76", (_pos >= _bigPos).IsFalse);
            nua.Assert("TestH#77", (_pos >= _pos).IsTrue);
            nua.Assert("TestH#79", (_zero >= _pos).IsFalse);
            nua.Assert("TestH#81", (_zero >= _zero).IsTrue);

            // GreaterThanOrEquals ints
            nua.Assert("TestH#82", (_pos >= _zero.Value).IsTrue);
            nua.Assert("TestH#83", (_pos >= _bigPos.Value).IsFalse);
            nua.Assert("TestH#84", (_pos >= _pos.Value).IsTrue);
            nua.Assert("TestH#85", (_zero >= _pos.Value).IsFalse);
            nua.Assert("TestH#86", (_bigPos >= _zero.Value).IsTrue);
            nua.Assert("TestH#87", (_zero >= _zero.Value).IsTrue);
        }


        [nu.Test]
        public void LessThanOperator() {
            // LessThan nulls
            nua.Assert("TestH#91", (_pos < _null).IsNull);
            nua.Assert("TestH#92", (_null < _zero).IsNull);
            nua.Assert("TestH#94", (_null < NullableByte.Null).IsNull);

            // LessThan nullable ints
            nua.Assert("TestH#95", (_pos < _zero).IsFalse);
            nua.Assert("TestH#96", (_pos < _bigPos).IsTrue);
            nua.Assert("TestH#97", (_pos < _pos).IsFalse);
            nua.Assert("TestH#101", (_zero < _zero).IsFalse);

            // LessThan ints
            nua.Assert("TestH#102", (_pos < _zero.Value).IsFalse);
            nua.Assert("TestH#103", (_pos < _bigPos.Value).IsTrue);
            nua.Assert("TestH#104", (_pos < _pos.Value).IsFalse);
            nua.Assert("TestH#105", (_zero < _pos.Value).IsTrue);
            nua.Assert("TestH#106", (_pos < _zero.Value).IsFalse);
            nua.Assert("TestH#107", (_zero < _zero.Value).IsFalse);
        }


        [nu.Test]
        public void LessThanOrEqualsOperator() {
            // LessThanOrEquals nulls
            nua.Assert("TestH#111", (_pos <= _null).IsNull);
            nua.Assert("TestH#112", (_null <= _zero).IsNull);
            nua.Assert("TestH#114", (_null <= NullableByte.Null).IsNull);

            // LessThanOrEquals nullable ints
            nua.Assert("TestH#115", (_pos <= _zero).IsFalse);
            nua.Assert("TestH#116", (_pos <= _bigPos).IsTrue);
            nua.Assert("TestH#117", (_pos <= _pos).IsTrue);
            nua.Assert("TestH#119", (_zero <= _pos).IsTrue);

            // LessThanOrEquals ints
            nua.Assert("TestH#122", (_pos <= _zero.Value).IsFalse);
            nua.Assert("TestH#123", (_pos <= _bigPos.Value).IsTrue);
            nua.Assert("TestH#124", (_pos <= _pos.Value).IsTrue);
        }


        [nu.Test]
        public void ModulusOperator() {
            byte bx = 28;
            byte by = 72;
            
            NullableByte nx = new NullableByte(bx);
            NullableByte ny = new NullableByte(by);
            
            // Modulus nullable ints
            nua.AssertEquals("TestH#131", (bx % by), (nx % ny).Value);
            nua.AssertEquals("TestH#132", 0, (_zero % ny).Value);

            // Modulus Nulls
            nua.Assert("TestH#133", (nx % _null).IsNull);
            nua.Assert("TestH#134", (_null % ny).IsNull);
            nua.Assert("TestH#135", (_null % NullableByte.Null).IsNull);

            // Modulus ints nullable ints
            nua.AssertEquals("TestH#136", (bx % by), (nx % by).Value);
            nua.AssertEquals("TestH#137", 0, (0 % ny).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.DivideByZeroException))]
        public void ModulusOperatorDivideByZero() {            
            NullableByte nz = _pos % _zero;
        }


        [nu.Test]
        public void MultiplyOperator() {
            byte bx = 13;
            byte by = 17;
            
            NullableByte nx = new NullableByte(bx);
            NullableByte ny = new NullableByte(by);
            
            // Multiply nullable ints
            nua.AssertEquals("TestH#141", (bx * by), (nx * ny).Value);
            nua.AssertEquals("TestH#142", 0, (_zero * ny).Value);

            // Multiply Nulls
            nua.Assert("TestH#143", (nx * _null).IsNull);
            nua.Assert("TestH#144", (_null * ny).IsNull);
            nua.Assert("TestH#145", (_null * NullableByte.Null).IsNull);

            // Multiply ints nullable ints
            nua.AssertEquals("TestH#146", (bx * by), (bx * ny).Value);
            nua.AssertEquals("TestH#147", 0, (0 * ny).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void MultiplyOperatorOverflowException() {
            NullableByte nx = new NullableByte(byte.MaxValue);
            NullableByte nz = nx * nx;
        }


        [nu.Test]
        public void OnesComplementOperator() {
            byte bx = 12;            
            NullableByte nx = new NullableByte(bx);
            
            // OnesComplement nullable ints
            nua.AssertEquals("TestH#151",(byte)(~bx & 255), (~nx).Value);
            nua.AssertEquals("TestH#152", (byte)(255 & ~0), (~_zero).Value);

            // OnesComplement Nulls
            nua.Assert("TestH#153", (~_null).IsNull);
        }


        [nu.Test]
        public void SubtractionOperator() {
            byte bx = 85;
            byte by = 53;
            
            NullableByte nx = new NullableByte(bx);
            NullableByte ny = new NullableByte(by);
            
            // Subtraction nullable ints
            nua.AssertEquals("TestH#161", (bx - by), (nx - ny).Value);
            nua.AssertEquals("TestH#162", bx, (nx - _zero).Value);

            // Subtraction Nulls
            nua.Assert("TestH#163", (nx - _null).IsNull);
            nua.Assert("TestH#164", (_null - ny).IsNull);
            nua.Assert("TestH#165", (_null - NullableByte.Null).IsNull);

            // Subtraction ints nullable ints
            nua.AssertEquals("TestH#166", (bx - by), (bx - ny).Value);
            nua.AssertEquals("TestH#167", by, (ny - 0).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void SubtractionOperatorOverflow() {            
            NullableByte nz = _zero - _pos;
        }
        #endregion // Operator Tests - H#

        #region Conversion Operator Tests - I#
        [nu.Test]
        public void NullableBooleanConversionOperator() {
            nua.AssertEquals("TestI#01", NullableByte.Null, (NullableByte)NullableBoolean.Null);
            nua.AssertEquals("TestI#02", new NullableByte(0), (NullableByte)NullableBoolean.False);
            nua.AssertEquals("TestI#03", new NullableByte(1), (NullableByte)NullableBoolean.True);
        }


        [nu.Test]
        public void NullableByteToByteConversionOperator() {
            
            byte bx = 57;
            NullableByte nx =  new NullableByte(bx);
            nua.AssertEquals("TestI#10", bx, (byte)nx);
        }


        [nu.Test, nu.ExpectedException(typeof(NullableNullValueException))]
        public void NullableByteToByteConversionOperatorNullValueException() {
            byte b = (byte)_null;
        }


        [nu.Test]
        public void NullableDecimalConversionOperator() {
            nua.AssertEquals("TestI#11", NullableByte.Null, (NullableByte)NullableDecimal.Null);
            
            byte bx = 237;
            NullableDecimal nx =  new NullableDecimal(bx);
            nua.AssertEquals("TestI#12", bx, ((NullableByte)nx).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void NullableDecimalConversionOperatorOverflowException() {
            int i = byte.MaxValue + 1;
            NullableDecimal nx =  new NullableDecimal(i);

            NullableByte ny = (NullableByte)nx;
        }


        [nu.Test]
        public void NullableDoubleConversionOperator() {
            nua.AssertEquals("TestI#21", NullableByte.Null, (NullableByte)NullableDouble.Null);
            
            byte bx = 95;
            NullableDouble nx =  new NullableDouble(bx);
            nua.AssertEquals("TestI#22", bx, ((NullableByte)nx).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void NullableDoubleConversionOperatorOverflowException() {
            double d = double.MaxValue;
            NullableDouble nx =  new NullableDouble(d);

            NullableByte ny = (NullableByte)nx;
        }


        [nu.Test]
        public void NullableInt16ConversionOperator() {
            nua.AssertEquals("TestI#31", NullableByte.Null, (NullableByte)NullableInt16.Null);
            
            short sx = 12;
            NullableInt16 nx =  new NullableInt16(sx);
            nua.AssertEquals("TestI#32", sx, ((NullableByte)nx).Value);
        }

        
        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void NullableInt16ConversionOperatorOverflowException() {
            short s = byte.MaxValue + 1;
            NullableInt16 nx =  new NullableInt16(s);

            NullableByte ny = (NullableByte)nx;
        }


        [nu.Test]
        public void NullableInt32ConversionOperator() {
            nua.AssertEquals("TestI#41", NullableByte.Null, (NullableByte)NullableInt32.Null);
            
            int ix = 12;
            NullableInt32 nx =  new NullableInt32(ix);
            nua.AssertEquals("TestI#42", ix, ((NullableByte)nx).Value);
        }

        
        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void NullableInt32ConversionOperatorOverflowException() {
            int i = byte.MaxValue + 1;
            NullableInt32 nx =  new NullableInt32(i);

            NullableByte ny = (NullableByte)nx;
        }


        [nu.Test]
        public void NullableInt64ConversionOperator() {
            nua.AssertEquals("TestI#51", NullableByte.Null, (NullableByte)NullableInt64.Null);
            
            byte bx = 86;
            NullableInt64 nx =  new NullableInt64(bx);
            nua.AssertEquals("TestI#52", bx, ((NullableByte)nx).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void NullableInt64ConversionOperatorOverflowException() {
            long l = byte.MaxValue + 1L;
            NullableInt64 nx =  new NullableInt64(l);

            NullableByte ny = (NullableByte)nx;
        }



        [nu.Test]
        public void NullableSingleConversionOperator() {
            nua.AssertEquals("TestI#71", NullableByte.Null, (NullableByte)NullableSingle.Null);
            
            byte bx = 75;
            NullableSingle nx =  new NullableSingle(bx);
            nua.AssertEquals("TestI#72", bx, ((NullableByte)nx).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void NullableSingleConversionOperatorOverflowException() {
            float f = (float)byte.MaxValue +1f;
            NullableSingle nx =  new NullableSingle(f);

            NullableByte ny = (NullableByte)nx;
        }


        [nu.Test]
        public void NullableStringConversionOperator() {
            nua.AssertEquals("TestI#81", NullableByte.Null, (NullableByte)NullableString.Null);
            
            byte bx = 99;
            NullableString nx =  new NullableString(bx.ToString());
            nua.AssertEquals("TestI#82", bx, ((NullableByte)nx).Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.OverflowException))]
        public void NullableStringConversionOperatorOverflowException() {
            long l = byte.MaxValue + 1L;
            NullableString nx =  new NullableString(l.ToString());

            NullableByte ny = (NullableByte)nx;
        }
        
        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void NullableStringConversionOperatorFormatException() {
            string sx = "1'05";            
            NullableString nx =  new NullableString(sx);

            NullableByte ny = (NullableByte)nx;
        }


        [nu.Test]
        public void ByteToNullableByteConversionOperator() {
            byte bx = 95;
            NullableByte nx =  bx;
            nua.AssertEquals("TestI#91", bx, nx.Value);
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
        public void ToNullableDecimal() {
            decimal  d = 67;
            NullableByte n = new NullableByte((byte)d);
            nua.AssertEquals("TestJ#21", NullableDecimal.Null, _null.ToNullableDecimal());
            nua.AssertEquals("TestJ#22", d, n.ToNullableDecimal().Value);
            nua.AssertEquals("TestJ#23", new NullableDecimal(d), n.ToNullableDecimal());
        }


        [nu.Test]
        public void ToNullableDouble() {
            double  d = 193;
            NullableByte n = new NullableByte((byte)d);
            nua.AssertEquals("TestJ#31", NullableDouble.Null, _null.ToNullableDouble());
            nua.AssertEquals("TestJ#32", d, n.ToNullableDouble().Value);
            nua.AssertEquals("TestJ#33", new NullableDouble(d), n.ToNullableDouble());
        }


        [nu.Test]
        public void ToNullableInt16() {
            short s = 89;
            NullableByte n = new NullableByte((byte)s);
            nua.AssertEquals("TestJ#41", NullableInt16.Null, _null.ToNullableInt16());
            nua.AssertEquals("TestJ#42", s, n.ToNullableInt16().Value);
            nua.AssertEquals("TestJ#43", new NullableInt16(s), n.ToNullableInt16());
        }
            

        [nu.Test]
        public void ToNullableInt32() {
            int i = 89;
            NullableByte n = new NullableByte((byte)i);
            nua.AssertEquals("TestJ#51", NullableInt32.Null, _null.ToNullableInt32());
            nua.AssertEquals("TestJ#52", i, n.ToNullableInt32().Value);
            nua.AssertEquals("TestJ#53", new NullableInt32(i), n.ToNullableInt32());
        }
            

        [nu.Test]
        public void ToNullableInt64() {
            long l = 251;
            NullableByte n = new NullableByte((byte)l);
            nua.AssertEquals("TestJ#61", NullableInt64.Null, _null.ToNullableInt64());
            nua.AssertEquals("TestJ#62", l, n.ToNullableInt64().Value);
            nua.AssertEquals("TestJ#63", new NullableInt64(l), n.ToNullableInt64());
        }
            
                        
        [nu.Test]
        public void ToNullableSingle() {
            float f = 174;
            NullableByte n = new NullableByte((byte)f);
            nua.AssertEquals("TestJ#81", NullableSingle.Null, _null.ToNullableSingle());
            nua.AssertEquals("TestJ#82", f, n.ToNullableSingle().Value);
            nua.AssertEquals("TestJ#83", new NullableSingle(f), n.ToNullableSingle());
        }


        [nu.Test]
        public void ToNullableString() {
            byte i = 44;
            NullableByte n = new NullableByte(i);
            nua.AssertEquals("TestJ#91", NullableString.Null, _null.ToNullableString());
            nua.AssertEquals("TestJ#92", i.ToString(), n.ToNullableString().Value);
            nua.AssertEquals("TestJ#93", new NullableString(i.ToString()), n.ToNullableString());
        }


        [nu.Test]
        public void ToStringTest() {
            byte i = 74;
            NullableByte n = new NullableByte(i);
            nua.AssertEquals("TestJ#101", "Null", _null.ToString());
            nua.AssertEquals("TestJ#102", i.ToString(), n.ToString());
        }
        #endregion // Conversion Tests - J#

        #region Serialization - K#
        [nu.Test]
        public void SerializableAttribute() {
            NullableByte serializedDeserialized;

            serializedDeserialized = SerializeDeserialize(_null);
            nua.Assert("TestK#01", serializedDeserialized.IsNull);
            nua.Assert("TestK#02", _null.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_pos);
            nua.Assert("TestK#03", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#04", _pos.Value, serializedDeserialized.Value);
            nua.Assert("TestK#05", _pos.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_min);
            nua.Assert("TestK#06", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#07", _min.Value, serializedDeserialized.Value);
            nua.Assert("TestK#08", _min.Equals(serializedDeserialized));

            serializedDeserialized = SerializeDeserialize(_max);
            nua.Assert("TestK#09", !serializedDeserialized.IsNull);
            nua.AssertEquals("TestK#10", _max.Value, serializedDeserialized.Value);
            nua.Assert("TestK#11", _max.Equals(serializedDeserialized));
        }

        private NullableByte SerializeDeserialize(NullableByte x) {
            System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer = 
                new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableByte y = (NullableByte)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }


        [nu.Test]
        public void XmlSerializable() {
            NullableByte xmlSerializedDeserialized;

            xmlSerializedDeserialized = XmlSerializeDeserialize(_null);
            nua.Assert("TestK#20", xmlSerializedDeserialized.IsNull);
            nua.Assert("TestK#21", _null.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_pos);
            nua.Assert("TestK#22", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#23", _pos.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#24", _pos.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_min);
            nua.Assert("TestK#25", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#26", _min.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#27", _min.Equals(xmlSerializedDeserialized));

            xmlSerializedDeserialized = XmlSerializeDeserialize(_max);
            nua.Assert("TestK#28", !xmlSerializedDeserialized.IsNull);
            nua.AssertEquals("TestK#29", _max.Value, xmlSerializedDeserialized.Value);
            nua.Assert("TestK#30", _max.Equals(xmlSerializedDeserialized));
        }

        private NullableByte XmlSerializeDeserialize(NullableByte x) {
            System.Xml.Serialization.XmlSerializer serializer = 
                new System.Xml.Serialization.XmlSerializer(typeof(NullableByte));
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream())     {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableByte y = (NullableByte)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }


        [nu.Test]
        public void XmlSerializableEmptyElementNil() {
            // Bug reported by Shaun Bowe (sbowe@users.sourceforge.net)
            // http://sourceforge.net/forum/message.php?msg_id=2399265

            //<?xml version="1.0"?>
            //<NullableByte xsi:nil="true" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"></NullableByte>

            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableByte));

            using (sys.IO.MemoryStream baseStream = new sys.IO.MemoryStream()) {
                using (sys.IO.StreamWriter stream = new System.IO.StreamWriter(baseStream)) {
                    stream.WriteLine("<?xml version=\"1.0\"?>");
                    stream.WriteLine("<NullableByte xsi:nil=\"true\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></NullableByte>");
                    stream.Flush();

//                    baseStream.Position = 0;
//                    sys.IO.StreamReader streamReader = new System.IO.StreamReader(baseStream);
//                    sys.Console.WriteLine(streamReader.ReadToEnd());
                    
                    baseStream.Position = 0; // Return stream to start
                    NullableByte y = (NullableByte)serializer.Deserialize(baseStream);

                    nua.Assert(y.IsNull);

                    baseStream.Close();
                    stream.Close();
                }
            }
        }


        [nu.Test]
        public void XmlSerializableSchema() {
            sysXmlScm.XmlSchema xsd = 
                ((sysXml.Serialization.IXmlSerializable)NullableByte.Null).GetSchema();

            xsd.Compile(new sysXmlScm.ValidationEventHandler(ValidationCallBack));

            ValidateXmlAgainstXmlSchema(xsd, _null);
            ValidateXmlAgainstXmlSchema(xsd, _min);
            ValidateXmlAgainstXmlSchema(xsd, _max);

        }

        private void ValidateXmlAgainstXmlSchema(sysXmlScm.XmlSchema xsd, NullableByte x) {
            sysXml.Serialization.XmlSerializer serializer = 
                new sysXml.Serialization.XmlSerializer(typeof(NullableByte));

                
            sys.IO.MemoryStream stream = null;
            sys.Xml.XmlValidatingReader validator = null;
            try {
                // Get the serialized NullableByte instance
                stream = new sys.IO.MemoryStream();
                serializer.Serialize(stream, x);
                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start

                // Add the default namespace
                sysXml.XmlDocument doc = new sysXml.XmlDocument();
                doc.Load(stream);                    
                sysXml.XmlAttribute defaultNs = doc.CreateAttribute("xmlns");
                defaultNs.Value = "http://NullableTypes.SourceForge.Net/NullableByteXMLSchema";
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