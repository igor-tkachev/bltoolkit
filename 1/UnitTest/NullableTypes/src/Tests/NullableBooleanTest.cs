//
// NullableTypes.Tests.NullableBooleanTest
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//          Damien Guard (damienguard@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 10-Mar-2003  Luca    Create     Created from older tests by www.go-mono.com SqlTypes
// 09-Aug-2003  Luca    Upgrade    New test: ParseFormatException2
// 12-Sep-2003  Luca    Upgrade    New test: SerializableAttribute
// 18-Sep-2003  Luca    Upgrade    Changed test: to reflect new requirements And, BitwiseAndOperator, Or, 
//                                 BitwiseOrOperator changed; Xor, ExlusiveOrOperator removed
// 04-Oct-2003  DamienG Upgrade    New test: XmlSerializable
//                                 Code upgrade: Tidy up of error messages (const string error...)
//                                 Code upgrade: Fixed incorrect "And" error messages in "Or" and "Xor"
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
    public class NullableBooleanTest {
        
        private NullableBoolean _nullableTrue;
        private NullableBoolean _nullableFalse;
        private NullableBoolean _nullableNull;

        [nu.SetUp]
        public void SetUp() {
            _nullableTrue = new NullableBoolean(true);
            _nullableFalse = new NullableBoolean(false);
            _nullableNull = NullableBoolean.Null;
        }


        #region Field Tests
        [nu.Test]
        public void FalseField() {
            nua.Assert("False field does not work correctly",
                !NullableBoolean.False.Value);
        }


        [nu.Test]
        public void NullField() {
            nua.Assert("Null field does not work correctly",
                NullableBoolean.Null.IsNull);
        }


        [nu.Test]
        public void OneField() {
            nua.AssertEquals("One field does not work correctly",
                (byte)1, NullableBoolean.One.ByteValue);
        }


        [nu.Test]
        public void TrueField() {
            nua.Assert("True field does not work correctly",
                NullableBoolean.True.Value);
        }


        [nu.Test]
        public void ZeroField() {
            nua.AssertEquals("Zero field does not work correctly",
                (byte)0, NullableBoolean.Zero.ByteValue);
        }
        #endregion // Field Tests

        #region Constructor Tests
        [nu.Test]
        public void Create () {
            const string error = "Creation of NullableBoolean failed";
            NullableBoolean true2 = new NullableBoolean(1);
            NullableBoolean false2 = new NullableBoolean(0);

            nua.Assert(error, _nullableTrue.Value);
            nua.Assert(error, true2.Value);
            nua.Assert(error, !_nullableFalse.Value);
            nua.Assert(error, !false2.Value);
        }
        #endregion // Constructor Tests

        #region INullable Tests
        // IsNull property
        [nu.Test]
        public void IsNullProperty() {
            const string error = "IsNull property does not work correctly ";

            nua.Assert(error + "(true)", !((NullableTypes.INullable)_nullableTrue).IsNull);
            nua.Assert(error + "(false)", !((NullableTypes.INullable)_nullableFalse).IsNull);
            nua.Assert(error + "(Null)", ((NullableTypes.INullable)NullableBoolean.Null).IsNull);
        }
        #endregion // INullable Tests

        #region IComparer Tests
        // CompareTo
        [nu.Test]
        public void CompareTo() {
            const string error = "CompareTo method does not work correctly";

            nua.Assert(error, (((sys.IComparable)_nullableTrue).CompareTo(NullableBoolean.Null) > 0));
            nua.Assert(error, (((sys.IComparable)_nullableFalse).CompareTo(NullableBoolean.Null) > 0));
            nua.Assert(error, (((sys.IComparable)_nullableTrue).CompareTo(_nullableTrue) == 0));
            nua.Assert(error, (((sys.IComparable)_nullableTrue).CompareTo(_nullableFalse) > 0));
            nua.Assert(error, (((sys.IComparable)_nullableFalse).CompareTo(_nullableTrue) < 0));
            nua.Assert(error, (((sys.IComparable)_nullableFalse).CompareTo(_nullableFalse) == 0));
            nua.Assert(error, (((sys.IComparable)_nullableNull).CompareTo(_nullableFalse) < 0));
            nua.Assert(error, (((sys.IComparable)_nullableNull).CompareTo(_nullableNull) == 0));
        }


        [nu.Test]
        [nu.ExpectedException(typeof(sys.ArgumentException))]
        public void CompareToWrongType() {
            ((sys.IComparable)_nullableTrue).CompareTo(1);
        }
        #endregion // IComparer Tests

        #region Property Tests

        // Value property
        [nu.Test]
        public void ValueProperty() {
            const string error = "Value property does not work correctly ";

            nua.Assert(error + "(true)", _nullableTrue.Value);
            nua.Assert(error + "(false)", !_nullableFalse.Value);
        }


        [nu.Test]
        [nu.ExpectedException(typeof(NullableTypes.NullableNullValueException))]
        public void ValuePropertyNull() {
            bool result =_nullableNull.Value;
        }


        // ByteValue property
        [nu.Test]
        public void ByteValueProperty() {
            const string error = "ByteValue property does not work correctly ";

            nua.AssertEquals(error + "(true)", (byte)1, _nullableTrue.ByteValue);
            nua.AssertEquals(error + "(false)", (byte)0, _nullableFalse.ByteValue);
        }


        [nu.Test]
        [nu.ExpectedException(typeof(NullableTypes.NullableNullValueException))]
        public void ByteValuePropertyNull() {
            byte result =_nullableNull.ByteValue;
        }


        // IsFalse property
        [nu.Test]
        public void IsFalseProperty() {
            const string error = "IsFalse property does not work correctly ";

            nua.Assert(error + "(true)", !_nullableTrue.IsFalse);
            nua.Assert(error + "(false)", _nullableFalse.IsFalse);
            nua.Assert(error + "(null)", !_nullableNull.IsFalse);
        }


        // IsTrue property
        [nu.Test]
        public void IsTrueProperty() {
            const string error = "IsTrue property does not work correctly ";

            nua.Assert(error + "(true)", _nullableTrue.IsTrue);
            nua.Assert(error + "(false)", !_nullableFalse.IsTrue);
            nua.Assert(error + "(null)", !_nullableNull.IsTrue);
        }

        #endregion // Property Tests

        #region Equivalence Tests
        // static Equals
        [nu.Test]
        public void StaticEquals() {
            const string error = "Static Equals method does not work correctly ";

            NullableBoolean nullableTrue2 = new NullableBoolean(true);
            NullableBoolean nullableFalse2 = new NullableBoolean(false);

            nua.Assert(error +  "(true == true)", NullableBoolean.Equals(_nullableTrue, nullableTrue2).Value);
            nua.Assert(error +  "(false == false)", NullableBoolean.Equals(_nullableFalse, nullableFalse2).Value);

            nua.Assert(error +  "(true == false)", !NullableBoolean.Equals(_nullableTrue, _nullableFalse).Value);
            nua.Assert(error +  "(false == true)", !NullableBoolean.Equals(_nullableFalse, _nullableTrue).Value);

            nua.AssertEquals(error +  "(null == false)", NullableBoolean.Null, NullableBoolean.Equals(NullableBoolean.Null, _nullableFalse));
            nua.AssertEquals(error +  "(true == null)", NullableBoolean.Null, NullableBoolean.Equals(_nullableTrue, NullableBoolean.Null));

            nua.AssertEquals(error +  "(null == null)", NullableBoolean.Null, NullableBoolean.Equals(NullableBoolean.Null, NullableBoolean.Null));
        }


        // Equals
        [nu.Test]
        public void Equals() {
            const string error = "Equals method does not work correctly ";

            NullableBoolean nullableTrue2 = new NullableBoolean(true);
            NullableBoolean nullableFalse2 = new NullableBoolean(false);

            nua.Assert(error + "(true == true)", _nullableTrue.Equals(nullableTrue2));
            nua.Assert(error + "(false == false)", _nullableFalse.Equals(nullableFalse2));

            nua.Assert(error + "(true == false)", !_nullableTrue.Equals(_nullableFalse));
            nua.Assert(error + "(false == true)", !_nullableFalse.Equals(_nullableTrue));

            nua.Assert(error + "(true == null)", !_nullableTrue.Equals(_nullableNull));
            nua.Assert(error + "(null == false)", !_nullableNull.Equals(_nullableFalse));
            nua.Assert(error + "(null == null)", _nullableNull.Equals(_nullableNull));
        }


        // Equality operator
        [nu.Test]
        public void EqualityOperator() {
            const string error = "Equality operator does not work correctly ";

            NullableBoolean nullableTrue2 = new NullableBoolean(true);
            NullableBoolean nullableFalse2 = new NullableBoolean(false);

            NullableBoolean nullableResult;

            nullableResult = _nullableTrue == _nullableFalse;
            nua.Assert(error + "(true == false)", !nullableResult.Value);
            nullableResult = _nullableFalse == _nullableTrue;
            nua.Assert(error + "(false == true)", !nullableResult.Value);

            nullableResult = _nullableTrue == nullableTrue2;
            nua.Assert(error + "(true == true)", nullableResult.Value);
            nullableResult = _nullableFalse == nullableFalse2;
            nua.Assert(error + "(false == false)", nullableResult.Value);

            nullableResult = _nullableFalse == NullableBoolean.Null;
            nua.Assert(error + "(false == Null)", nullableResult.IsNull);
            nullableResult = NullableBoolean.Null == _nullableTrue;
            nua.Assert(error + "(null == true)", nullableResult.IsNull);
            nullableResult = NullableBoolean.Null == _nullableNull;
            nua.Assert(error + "(null == null)", nullableResult.IsNull);
        }


        // NotEquals
        [nu.Test]
        public void NotEquals() {
            const string error = "NotEquals method does not work correctly ";
            
            NullableBoolean nullableTrue2 = new NullableBoolean(true);
            NullableBoolean nullableFalse2 = new NullableBoolean(false);

            NullableBoolean nullableResult;

            // true != false
            nullableResult = NullableBoolean.NotEquals(_nullableTrue, _nullableFalse);
            nua.Assert(error + "(true != false)", nullableResult.Value);
            nullableResult = NullableBoolean.NotEquals(_nullableFalse, _nullableTrue);
            nua.Assert(error + "(false != true)", nullableResult.Value);

            // true != true
            nullableResult = NullableBoolean.NotEquals(_nullableTrue, _nullableTrue);
            nua.Assert(error + "(true != true)", !nullableResult.Value);
            nullableResult = NullableBoolean.NotEquals(_nullableTrue, nullableTrue2);
            nua.Assert(error + "(true != true2)", !nullableResult.Value);

            // false != false
            nullableResult = NullableBoolean.NotEquals(_nullableFalse, _nullableFalse);
            nua.Assert(error + "(false != false)", !nullableResult.Value);
            nullableResult = NullableBoolean.NotEquals(_nullableTrue, nullableTrue2);
            nua.Assert(error + "(false != false2)", !nullableResult.Value);

            // If either instance of NullableBoolean is null, the Value of the NullableBoolean will be Null.
            nullableResult = NullableBoolean.NotEquals(NullableBoolean.Null, _nullableFalse);
            nua.Assert(error + "(Null != false)", nullableResult.IsNull);
            nullableResult = NullableBoolean.NotEquals(_nullableTrue, NullableBoolean.Null);
            nua.Assert(error + "(true != Null)", nullableResult.IsNull);
            nullableResult = NullableBoolean.NotEquals(_nullableNull, NullableBoolean.Null);
            nua.Assert(error + "(null != null)", nullableResult.IsNull);
        }


        [nu.Test]
        public void GetHashCodeTest() {
            const string error = "GetHashCode method does not work correctly";

            nua.Assert(error, _nullableTrue.GetHashCode() != _nullableFalse.GetHashCode());
            nua.Assert(error, _nullableTrue.GetHashCode() != _nullableNull.GetHashCode());
            nua.Assert(error, _nullableNull.GetHashCode() != _nullableFalse.GetHashCode());
        }
        #endregion // Equivalence Tests

        #region Method Tests
        //  Parse
        [nu.Test]
        public void Parse() {
            const string error = "Parse method does not work correctly ";
                                                                         
            nua.Assert(error + "(\"True\")", NullableBoolean.Parse("True").Value);
            nua.Assert(error + "(\" True\")", NullableBoolean.Parse(" True").Value);
            nua.Assert(error + "(\"True \")", NullableBoolean.Parse("True ").Value);
            nua.Assert(error + "(\"tRue\")", NullableBoolean.Parse("tRuE").Value);
            nua.Assert(error + "(\"False\")", !NullableBoolean.Parse("False").Value);
            nua.Assert(error + "(\" False\")", !NullableBoolean.Parse(" False").Value);
            nua.Assert(error + "(\"False \")", !NullableBoolean.Parse("False ").Value);
            nua.Assert(error + "(\"fAlSe\")", !NullableBoolean.Parse("fAlSe").Value);

            nua.Assert(error + "(1)", NullableBoolean.Parse("1").Value);
            nua.Assert(error + "(-1)", NullableBoolean.Parse("-1").Value);
            nua.Assert(error + "(0)", !NullableBoolean.Parse("0").Value);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ParseFormatException() {
            NullableBoolean.Parse("Gino");
        }


        [nu.Test, nu.ExpectedException(typeof(sys.FormatException))]
        public void ParseFormatException2() {
            NullableBoolean.Parse(long.MaxValue.ToString());
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ParseArgumentNullException() {
            NullableBoolean.Parse(null);
        }
 

        // And
        [nu.Test]
        public void And() {
            const string error = "And method does not work correctly ";
            
            NullableBoolean nullableTrue2 = new NullableBoolean(true);
            NullableBoolean nullableFalse2 = new NullableBoolean(false);

            // One result value
            NullableBoolean nullableResult;

            // true && false
            nullableResult = NullableBoolean.And(_nullableTrue, _nullableFalse);
            nua.Assert(error + "(true && false)", !nullableResult.Value);
            nullableResult = NullableBoolean.And(_nullableFalse, _nullableTrue);
            nua.Assert(error + "(false && true)", !nullableResult.Value);

            // true && true
            nullableResult = NullableBoolean.And(_nullableTrue, nullableTrue2);
            nua.Assert(error + "(true && true2)", nullableResult.Value);
            nullableResult = NullableBoolean.And(_nullableTrue, _nullableTrue);
            nua.Assert(error + "(true && true)", nullableResult.Value);

            // false && false
            nullableResult = NullableBoolean.And(_nullableFalse, nullableFalse2);
            nua.Assert(error + "(false && false2)", !nullableResult.Value);
            nullableResult = NullableBoolean.And(_nullableFalse, _nullableFalse);
            nua.Assert(error + "(false && false)", !nullableResult.Value);

            // false && null
            nullableResult = NullableBoolean.And(nullableFalse2, _nullableNull);
            nua.Assert(error + "(false2 && null)", nullableResult.IsFalse);

            // null && true
            nullableResult = NullableBoolean.And(_nullableNull, nullableTrue2);
            nua.Assert(error + "(null && true2)", nullableResult.IsNull);

            // null && null
            nullableResult = NullableBoolean.And(_nullableNull, NullableBoolean.Null);
            nua.Assert(error + "(null && null)", nullableResult.IsNull);
        }


        // OnesComplement
        [nu.Test]
        public void OnesComplement() {
            const string error = "OnesComplement method does not work correctly";

            NullableBoolean nullableFalse2 = NullableBoolean.OnesComplement(_nullableTrue);
            nua.Assert(error, !nullableFalse2.Value);

            NullableBoolean nullableTrue2 = NullableBoolean.OnesComplement(_nullableFalse);
            nua.Assert(error, nullableTrue2.Value);

            NullableBoolean nullableNull2 = NullableBoolean.OnesComplement(_nullableNull);
            nua.Assert(error, nullableNull2.IsNull);
        }


        // Or
        [nu.Test]
        public void Or() {
            const string error = "Or method does not work correctly ";

            NullableBoolean nullableTrue2 = new NullableBoolean(true);
            NullableBoolean nullableFalse2 = new NullableBoolean(false);

            NullableBoolean nullableResult;

            // true || false
            nullableResult = NullableBoolean.Or(_nullableTrue, _nullableFalse);
            nua.Assert(error + "(true || false)", nullableResult.Value);
            nullableResult = NullableBoolean.Or(_nullableFalse, _nullableTrue);
            nua.Assert(error + "(false || true)", nullableResult.Value);

            // true || true
            nullableResult = NullableBoolean.Or(_nullableTrue, _nullableTrue);
            nua.Assert(error + "(true || true)", nullableResult.Value);
            nullableResult = NullableBoolean.Or(_nullableTrue, nullableTrue2);
            nua.Assert(error + "(true || true2)", nullableResult.Value);

            // false || false
            nullableResult = NullableBoolean.Or(_nullableFalse, _nullableFalse);
            nua.Assert(error + "(false || false)", !nullableResult.Value);
            nullableResult = NullableBoolean.Or(_nullableFalse, nullableFalse2);
            nua.Assert(error + "(false || false2)", !nullableResult.Value);

            // null && true
            nullableResult = NullableBoolean.Or(_nullableNull, nullableTrue2);
            nua.Assert(error + "(null || true2)", nullableResult.IsTrue);

            // null && null
            nullableResult = NullableBoolean.Or(_nullableNull, NullableBoolean.Null);
            nua.Assert(error + "(null || null)", nullableResult.IsNull);
        }


        // Xor
        [nu.Test]
        public void Xor() {
            const string error = "Xor method does not work correctly ";

            NullableBoolean nullableTrue2 = new NullableBoolean(true);
            NullableBoolean nullableFalse2 = new NullableBoolean(false);

            NullableBoolean nullableResult;

            // true ^ false
            nullableResult = NullableBoolean.Xor(_nullableTrue, _nullableFalse);
            nua.Assert(error + "(true ^ false)", nullableResult.Value);
            nullableResult = NullableBoolean.Xor(_nullableFalse, _nullableTrue);
            nua.Assert(error + "(false ^ true)", nullableResult.Value);

            // true ^ true
            nullableResult = NullableBoolean.Xor(_nullableTrue, nullableTrue2);
            nua.Assert(error + "(true ^ true)", !nullableResult.Value);

            // false ^ false
            nullableResult = NullableBoolean.Xor(_nullableFalse, nullableFalse2);
            nua.Assert(error + "(false ^ false)", !nullableResult.Value);

            // null ^ true
            nullableResult = NullableBoolean.Xor(_nullableNull, nullableTrue2);
            nua.Assert(error + "(null ^ true2)", nullableResult.IsNull);

            // null ^ null
            nullableResult = NullableBoolean.Xor(_nullableNull, NullableBoolean.Null);
            nua.Assert(error + "(null ^ null)", nullableResult.IsNull);
        }


        // GetType
        [nu.Test]
        public void GetTypeTest() {
            nua.AssertEquals("GetType method does not work correctly",
                _nullableTrue.GetType().ToString(), "NullableTypes.NullableBoolean");
        }

        #endregion // Method Tests

        #region Operator Tests
        // Bitwise And operator
        [nu.Test]
        public void BitwiseAndOperator() {
            const string error = "BitwiseAnd operator does not work correctly ";

            NullableBoolean nullableTrue2 = new NullableBoolean(true);
            NullableBoolean nullableFalse2 = new NullableBoolean(false);

            NullableBoolean nullableResult;

            nullableResult = _nullableTrue & _nullableFalse;
            nua.Assert(error + "(true & false)", !nullableResult.Value);
            nullableResult = _nullableFalse & _nullableTrue;
            nua.Assert(error + "(false & true)", !nullableResult.Value);

            nullableResult = _nullableTrue & nullableTrue2;
            nua.Assert(error + "(true & true)", nullableResult.Value);

            nullableResult = _nullableFalse & nullableFalse2;
            nua.Assert(error + "(false & false)", !nullableResult.Value);

            nullableResult = nullableFalse2 & _nullableNull;
            nua.Assert(error + "(false & null)", nullableResult.IsFalse);

            nullableResult = _nullableNull &  nullableTrue2;
            nua.Assert(error + "(null & true)", nullableResult.IsNull);

            nullableResult = _nullableNull &  NullableBoolean.Null;
            nua.Assert(error + "(null & null)", nullableResult.IsNull);
        }


        // Bitwise Or operator
        [nu.Test]
        public void BitwiseOrOperator() {
            const string error = "BitwiseOr operator does not work correctly ";

            NullableBoolean nullableTrue2 = new NullableBoolean(true);
            NullableBoolean nullableFalse2 = new NullableBoolean(false);

            NullableBoolean nullableResult;

            nullableResult = _nullableTrue | _nullableFalse;
            nua.Assert(error + "(true | false)", nullableResult.Value);

            nullableResult = _nullableFalse | _nullableTrue;
            nua.Assert(error + "(false | true)", nullableResult.Value);

            nullableResult = _nullableTrue | nullableTrue2;
            nua.Assert(error + "(true | true)", nullableResult.Value);

            nullableResult = _nullableFalse | nullableFalse2;
            nua.Assert(error + "(false | false)", !nullableResult.Value);

            nullableResult = _nullableNull | nullableTrue2;
            nua.Assert(error + "(null | true)", nullableResult.IsTrue);

            nullableResult = nullableFalse2 | _nullableNull;
            nua.Assert(error + "(false | null)", nullableResult.IsNull);

            nullableResult = NullableBoolean.Null | _nullableNull;
            nua.Assert(error + "(null | null)", nullableResult.IsNull);
        }


        // Exlusive Or operator
        [nu.Test]
        public void ExlusiveOrOperator() {
            const string error = "ExclusiveOr operator does not work correctly ";

            NullableBoolean nullableTrue2 = new NullableBoolean(true);
            NullableBoolean nullableFalse2 = new NullableBoolean(false);

            NullableBoolean nullableResult;


            nullableResult = _nullableTrue ^ _nullableFalse;
            nua.Assert(error + "(true ^ false)", nullableResult.Value);

            nullableResult = _nullableFalse | _nullableTrue;
            nua.Assert(error + "(false ^ true)", nullableResult.Value);

            nullableResult = _nullableTrue ^ nullableTrue2;
            nua.Assert(error + "(true ^ true)", !nullableResult.Value);

            nullableResult = _nullableFalse ^ nullableFalse2;
            nua.Assert(error + "(false ^ false)", !nullableResult.Value);

            nullableResult = _nullableNull ^ nullableTrue2;
            nua.Assert(error + "(null ^ true)", nullableResult.IsNull);

            nullableResult = _nullableFalse ^ _nullableNull;
            nua.Assert(error + "(false ^ null)", nullableResult.IsNull);

            nullableResult = NullableBoolean.Null ^ _nullableNull;
            nua.Assert(error + "(null ^ null)", nullableResult.IsNull);
        }


        // false operator
        [nu.Test]
        public void FalseOperator() {
            const string error = "false operator does not work correctly ";

            nua.Assert(error + "(true)", !_nullableTrue.IsFalse);
            nua.Assert(error + "(false)", _nullableFalse.IsFalse);
            nua.Assert(error + "(null)", !_nullableNull.IsFalse);
        }


        // Logical Not operator
        [nu.Test]
        public void LogicalNotOperator() {
            const string error = "Logical Not operator does not work correctly" ;

            nua.AssertEquals(error + "(true)", NullableBoolean.False, !_nullableTrue);
            nua.AssertEquals(error + "(false)", NullableBoolean.True, !_nullableFalse);
            nua.AssertEquals(error + "(null)", NullableBoolean.Null, !_nullableNull);
        }


        // Ones Complement operator
        [nu.Test]
        public void OnesComplementOperator() {
            const string error = "Ones complement operator does not work correctly" ;

            NullableBoolean nullableResult;

            nullableResult = ~_nullableTrue;
            nua.Assert(error + "(true)", !nullableResult.Value);
            nullableResult = ~_nullableFalse;
            nua.Assert(error + "(false)", nullableResult.Value);
            nullableResult = ~_nullableNull;
            nua.Assert(error + "(null)", nullableResult.IsNull);
        }


        // true operator
        [nu.Test]
        public void TrueOperator() {
            const string error = "true operator does not work correctly ";

            nua.Assert(error + "(true)", _nullableTrue.IsTrue);
            nua.Assert(error + "(false)", !_nullableFalse.IsTrue);
            nua.Assert(error + "(null)", !_nullableNull.IsFalse);
        }
        

        // Inequality operator
        [nu.Test]
        public void InequalityOperator() {
            const string error = "Inequality operator does not work correctly" ;

            NullableBoolean nullableTrue2 = new NullableBoolean(true);
            NullableBoolean nullableFalse2 = new NullableBoolean(false);

            nua.AssertEquals(error + "(true != true)",   NullableBoolean.False, _nullableTrue != _nullableTrue);
            nua.AssertEquals(error + "(true != true)",   NullableBoolean.False, _nullableTrue != nullableTrue2);
            nua.AssertEquals(error + "(false != false)", NullableBoolean.False, _nullableFalse != _nullableFalse);
            nua.AssertEquals(error + "(false != false)", NullableBoolean.False, _nullableFalse != nullableFalse2);
            nua.AssertEquals(error + "(true != false)",  NullableBoolean.True, _nullableTrue != _nullableFalse);
            nua.AssertEquals(error + "(false != true)",  NullableBoolean.True, _nullableFalse != _nullableTrue);
            nua.AssertEquals(error + "(null != true)",   NullableBoolean.Null, NullableBoolean.Null != _nullableTrue);
            nua.AssertEquals(error + "(false != null)",  NullableBoolean.Null, _nullableFalse != NullableBoolean.Null);
        }
        #endregion // Operator Tests

        #region Conversion Operator Tests
        // NullableBoolean to bool
        [nu.Test]
        public void NullableBooleanToBoolean() {
            const string error = "NullableBooleanToBoolean operator does not work correctly ";

            bool testBoolean = (bool)_nullableTrue;
            nua.Assert(error + "(true)",  testBoolean);
            testBoolean = (bool)_nullableFalse;
            nua.Assert(error + "(false)",  !testBoolean);
        }


        [nu.Test, nu.ExpectedException(typeof(NullableNullValueException))]
        public void NullableBooleanNullToBoolean() {
            bool testBoolean = (bool)_nullableNull;
        }


        // NullableByte to NullableBoolean
        [nu.Test]
        public void NullableByteToNullableBoolean() {
            const string error = "NullableByteToNullableBoolean operator does not work correctly ";

            NullableByte nullableTestByte;
            NullableBoolean nullableTestBoolean;

            nullableTestByte = new NullableByte(1);
            nullableTestBoolean = (NullableBoolean)nullableTestByte;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTestByte = new NullableByte(2);
            nullableTestBoolean = (NullableBoolean)nullableTestByte;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTestByte = new NullableByte(0);
            nullableTestBoolean = (NullableBoolean)nullableTestByte;
            nua.Assert(error + "(false)", !nullableTestBoolean.Value);

            nullableTestByte = NullableByte.Null;
            nullableTestBoolean = (NullableBoolean)nullableTestByte;
            nua.Assert(error + "(null)", nullableTestBoolean.IsNull);
        }


        // NullableDecimal to NullableBoolean
        [nu.Test]
        public void NullableDecimalToNullableBoolean() {
            const string error = "NullableDecimalToNullableBoolean operator does not work correctly ";

            NullableDecimal nullableTest;
            NullableBoolean nullableTestBoolean;

            nullableTest = new NullableDecimal(System.Decimal.MaxValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true+)", nullableTestBoolean.Value);

            nullableTest = new NullableDecimal(-System.Decimal.MaxValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true-)", nullableTestBoolean.Value);

            nullableTest = new NullableDecimal(System.Decimal.MinValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true eps)", nullableTestBoolean.Value);

            nullableTest = new NullableDecimal(System.Decimal.Zero);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(false)", !nullableTestBoolean.Value);

            nullableTest = NullableDecimal.Null;
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(null)", nullableTestBoolean.IsNull);

        }


        // NullableDouble to NullableBoolean
        [nu.Test]
        public void NullableDoubleToNullableBoolean() {
            const string error = "NullableDoubleToNullableBoolean operator does not work correctly ";

            NullableDouble nullableTest;
            NullableBoolean nullableTestBoolean;

            nullableTest = new NullableDouble(System.Double.MaxValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true+)", nullableTestBoolean.Value);

            nullableTest = new NullableDouble(-System.Double.MaxValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true-)", nullableTestBoolean.Value);

            nullableTest = new NullableDouble(System.Double.MinValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true eps)", nullableTestBoolean.Value);

            nullableTest = new NullableDouble(0);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(false)", !nullableTestBoolean.Value);

            nullableTest = NullableDouble.Null;
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(null)", nullableTestBoolean.IsNull);
        }


        // NullableInt16 to NullableBoolean
        [nu.Test]
        public void NullableInt16ToNullableBoolean() {
            const string error = "NullableInt16ToNullableBoolean operator does not work correctly ";

            NullableInt16 nullableTest;
            NullableBoolean nullableTestBoolean;

            nullableTest = new NullableInt16(sys.Int16.MaxValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableInt16(sys.Int16.MinValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableInt16(0);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(false)", !nullableTestBoolean.Value);

            nullableTest = NullableInt16.Null;
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(null)", nullableTestBoolean.IsNull);
        }


        // NullableInt32 to NullableBoolean
        [nu.Test]
        public void NullableInt32ToNullableBoolean() {
            const string error = "NullableInt32ToNullableBoolean operator does not work correctly ";

            NullableInt32 nullableTest;
            NullableBoolean nullableTestBoolean;

            nullableTest = new NullableInt32(sys.Int32.MaxValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableInt32(sys.Int32.MinValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableInt32(0);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(false)", !nullableTestBoolean.Value);

            nullableTest = NullableInt32.Null;
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(null)", nullableTestBoolean.IsNull);
        }


        // NullableInt64 to NullableBoolean
        [nu.Test]
        public void NullableInt64ToNullableBoolean() {
            const string error = "NullableInt64ToNullableBoolean operator does not work correctly ";

            NullableInt64 nullableTest;
            NullableBoolean nullableTestBoolean;

            nullableTest = new NullableInt64(long.MaxValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableInt64(long.MinValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableInt64(0);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(false)", !nullableTestBoolean.Value);

            nullableTest = NullableInt64.Null;
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(null)", nullableTestBoolean.IsNull);
        }


        // NullableSingle to NullableBoolean
        [nu.Test]
        public void NullableSingleToNullableBoolean() {
            string error = "NullableSingleToNullableBoolean operator does not work correctly ";

            NullableSingle nullableTest;
            NullableBoolean nullableTestBoolean;

            nullableTest = new NullableSingle(float.MaxValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableSingle(float.MinValue);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableSingle(float.Epsilon);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableSingle(float.NaN);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableSingle(0);
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(false)", !nullableTestBoolean.Value);

            nullableTest = NullableSingle.Null;
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(null)", nullableTestBoolean.IsNull);
        }


        // NullableString to NullableBoolean
        [nu.Test]
        public void NullableStringToNullableBoolean() {
            const string error = "NullableStringToNullableBoolean operator does not work correctly ";

            NullableString nullableTest;
            NullableBoolean nullableTestBoolean;

            nullableTest = new NullableString("true");
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableString("TRUE");
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableString("True");
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = new NullableString("false");
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(false)", !nullableTestBoolean.Value);
        }


        // bool to NullableBoolean
        [nu.Test]
        public void BooleanToNullableBoolean() {
            const string error = "BooleanToNullableBoolean operator does not work correctly ";

            NullableBoolean nullableTestBoolean;
            bool btrue = true;
            bool bfalse = false;

            bool nullableTest = true;
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);
            nullableTestBoolean = (NullableBoolean)btrue;
            nua.Assert(error + "(true)", nullableTestBoolean.Value);

            nullableTest = false;
            nullableTestBoolean = (NullableBoolean)nullableTest;
            nua.Assert(error + "(false)", !nullableTestBoolean.Value);
            nullableTestBoolean = (NullableBoolean)bfalse;
            nua.Assert(error + "(false)", !nullableTestBoolean.Value);
        }
        #endregion // Conversion Operator Tests

        #region Conversion Tests
        // ToNullableByte
        [nu.Test]
        public void ToNullableByte() {
            const string error = "ToNullableByte method does not work correctly ";

            NullableByte nullableTestByte;

            nullableTestByte = _nullableTrue.ToNullableByte();
            nua.AssertEquals(error, (byte)1,nullableTestByte.Value);

            nullableTestByte = _nullableFalse.ToNullableByte();
            nua.AssertEquals(error, (byte)0, nullableTestByte.Value);
        }


        // ToNullableDecimal
        [nu.Test]
        public void ToNullableDecimal() {
            const string error = "ToNullableDecimal method does not work correctly ";

            NullableDecimal nullableTestDecimal;

            nullableTestDecimal = _nullableTrue.ToNullableDecimal();
            nua.AssertEquals(error, (decimal)1, nullableTestDecimal.Value);

            nullableTestDecimal = _nullableFalse.ToNullableDecimal();
            nua.AssertEquals(error, (decimal)0, nullableTestDecimal.Value);
        }


        // ToNullableDouble
        [nu.Test]
        public void ToNullableDouble() {
            const string error = "ToNullableDouble method does not work correctly ";

            NullableDouble nullableTestDouble;

            nullableTestDouble = _nullableTrue.ToNullableDouble();
            nua.AssertEquals(error, (double)1, nullableTestDouble.Value);

            nullableTestDouble = _nullableFalse.ToNullableDouble();
            nua.AssertEquals(error, (double)0, nullableTestDouble.Value);
        }


        // ToNullableInt16
        [nu.Test]
        public void ToNullableInt16() {
            const string error = "ToNullableInt16 method does not work correctly ";

            NullableInt16 nullableTestInt16;

            nullableTestInt16 = _nullableTrue.ToNullableInt16();
            nua.AssertEquals(error, (short)1, nullableTestInt16.Value);

            nullableTestInt16 = _nullableFalse.ToNullableInt16();
            nua.AssertEquals(error, (short)0, nullableTestInt16.Value);
        }


        // ToNullableInt32
        [nu.Test]
        public void ToNullableInt32() {
            const string error = "ToNullableInt32 method does not work correctly ";

            NullableInt32 nullableTestInt32;

            nullableTestInt32 = _nullableTrue.ToNullableInt32();
            nua.AssertEquals(error, (int)1, nullableTestInt32.Value);

            nullableTestInt32 = _nullableFalse.ToNullableInt32();
            nua.AssertEquals(error, (int)0, nullableTestInt32.Value);
        }


        // ToNullableInt64
        [nu.Test]
        public void ToNullableInt64() {
            const string error = "ToNullableInt64 method does not work correctly ";

            NullableInt64 nullableTestInt64;

            nullableTestInt64 = _nullableTrue.ToNullableInt64();
            nua.AssertEquals(error, (long)1, nullableTestInt64.Value);

            nullableTestInt64 = _nullableFalse.ToNullableInt64();
            nua.AssertEquals(error, (long)0, nullableTestInt64.Value);
        }


        // ToNullableSingle
        [nu.Test]
        public void ToNullableSingle() {
            const string error = "ToNullableSingle method does not work correctly ";

            NullableSingle nullableTestSingle;

            nullableTestSingle = _nullableTrue.ToNullableSingle();
            nua.AssertEquals(error, (float)1, nullableTestSingle.Value);

            nullableTestSingle = _nullableFalse.ToNullableSingle();
            nua.AssertEquals(error, (float)0, nullableTestSingle.Value);
        }


        // ToNullableString
        [nu.Test]
        public void ToNullableString() {
            const string error = "ToNullableString method does not work correctly ";

            NullableString nullableTestString;

            nullableTestString = _nullableTrue.ToNullableString();
            nua.AssertEquals(error + "(true)", "True", nullableTestString.Value);

            nullableTestString = _nullableFalse.ToNullableString();
            nua.AssertEquals(error + "(false)", "False", nullableTestString.Value);

            nullableTestString = _nullableNull.ToNullableString();
            nua.Assert(error + "(null)", nullableTestString.IsNull);
        }


        // ToString
        [nu.Test]
        public void ToStringTest() {
            const string error = "ToString method does not work correctly ";

            string testString;

            testString = _nullableTrue.ToString();
            nua.AssertEquals(error + "(true)", "True", testString);

            testString = _nullableFalse.ToString();
            nua.AssertEquals(error + "(false)", "False", testString);

            testString = _nullableNull.ToString();
            nua.AssertEquals(error + "(null)", "Null", testString);
        }
        #endregion // Conversion Tests

        #region Serialization Tests
        [nu.Test]
        public void SerializableAttribute() {
            const string error = "Serialization & Deserialization failed with ";

            NullableBoolean serializedDeserializedNull = SerializeDeserialize(_nullableNull);
            nua.Assert(error + "Null", serializedDeserializedNull.IsNull);
            nua.Assert(error + "Null", !serializedDeserializedNull.IsTrue);
            nua.Assert(error + "Null", !serializedDeserializedNull.IsFalse);

            NullableBoolean serializedDeserializedTrue = SerializeDeserialize(_nullableTrue);
            nua.Assert(error + "True", !serializedDeserializedTrue.IsNull);
            nua.Assert(error + "True", serializedDeserializedTrue.IsTrue);
            nua.Assert(error + "True", !serializedDeserializedTrue.IsFalse);

            NullableBoolean serializedDeserializedFalse = SerializeDeserialize(_nullableFalse);
            nua.Assert(error + "False", !serializedDeserializedFalse.IsNull);
            nua.Assert(error + "False", !serializedDeserializedFalse.IsTrue);
            nua.Assert(error + "False", serializedDeserializedFalse.IsFalse);
        }

        private NullableBoolean SerializeDeserialize(NullableBoolean x) {
            System.Runtime.Serialization.Formatters.Soap.SoapFormatter serializer = 
                new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableBoolean y = (NullableBoolean)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }


        [nu.Test]
        public void XmlSerializable() {
            const string error = "XmlSerialization & XmlDeserialization failed with ";

            NullableBoolean xmlSerializedDeserializedNull = XmlSerializeDeserialize(_nullableNull);
            nua.Assert(error + "Null", xmlSerializedDeserializedNull.IsNull);
            nua.Assert(error + "Null", !xmlSerializedDeserializedNull.IsTrue);
            nua.Assert(error + "Null", !xmlSerializedDeserializedNull.IsFalse);

            NullableBoolean xmlSerializedDeserializedTrue = XmlSerializeDeserialize(_nullableTrue);
            nua.Assert(error + "True", !xmlSerializedDeserializedTrue.IsNull);
            nua.Assert(error + "True", xmlSerializedDeserializedTrue.IsTrue);
            nua.Assert(error + "True", !xmlSerializedDeserializedTrue.IsFalse);

            NullableBoolean xmlSerializedDeserializedFalse = XmlSerializeDeserialize(_nullableFalse);
            nua.Assert(error + "False", !xmlSerializedDeserializedFalse.IsNull);
            nua.Assert(error + "False", !xmlSerializedDeserializedFalse.IsTrue);
            nua.Assert(error + "False", xmlSerializedDeserializedFalse.IsFalse);
        }


        private NullableBoolean XmlSerializeDeserialize(NullableBoolean x) {
            sysXml.Serialization.XmlSerializer serializer = 
                new sysXml.Serialization.XmlSerializer(typeof(NullableBoolean));
                    
            using (sys.IO.MemoryStream stream = new sys.IO.MemoryStream()) {
                serializer.Serialize(stream, x);

//                sys.Text.Decoder d = sys.Text.Encoding.Default.GetDecoder();
//                char[] output = new char[d.GetCharCount(stream.GetBuffer(), 0, (int)stream.Length)];
//                d.GetChars(stream.GetBuffer(), 0, (int)stream.Length, output, 0);
//                sys.Console.WriteLine(new string(output));

                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start
                NullableBoolean y = (NullableBoolean)serializer.Deserialize(stream);
                stream.Close();
                return y;
            }
        }


        [nu.Test]
        public void XmlSerializableEmptyElementNil() {
            // Bug reported by Shaun Bowe (sbowe@users.sourceforge.net)
            // http://sourceforge.net/forum/message.php?msg_id=2399265

            //<?xml version="1.0"?>
            //<NullableBoolean xsi:nil="true" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"></NullableBoolean>

            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(NullableBoolean));

            using (sys.IO.MemoryStream baseStream = new sys.IO.MemoryStream()) {
                using (sys.IO.StreamWriter stream = new System.IO.StreamWriter(baseStream)) {
                    stream.WriteLine("<?xml version=\"1.0\"?>");
                    stream.WriteLine("<NullableBoolean xsi:nil=\"true\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></NullableBoolean>");
                    stream.Flush();

//                    baseStream.Position = 0;
//                    sys.IO.StreamReader streamReader = new System.IO.StreamReader(baseStream);
//                    sys.Console.WriteLine(streamReader.ReadToEnd());
                    
                    baseStream.Position = 0; // Return stream to start
                    NullableBoolean y = (NullableBoolean)serializer.Deserialize(baseStream);

                    nua.Assert(y.IsNull);

                    baseStream.Close();
                    stream.Close();
                }
            }
        }


        [nu.Test]
        public void XmlSerializableSchema() {
            sysXmlScm.XmlSchema xsd = 
                ((sys.Xml.Serialization.IXmlSerializable)NullableBoolean.True).GetSchema();

            xsd.Compile(new sysXmlScm.ValidationEventHandler(ValidationCallBack));

            ValidateXmlAgainstXmlSchema(xsd, NullableBoolean.Null);
            ValidateXmlAgainstXmlSchema(xsd, NullableBoolean.True);
            ValidateXmlAgainstXmlSchema(xsd, NullableBoolean.False);

        }

        private void ValidateXmlAgainstXmlSchema(sysXmlScm.XmlSchema xsd, NullableBoolean x) {
            sysXml.Serialization.XmlSerializer serializer = 
                new sysXml.Serialization.XmlSerializer(typeof(NullableBoolean));

                
            sys.IO.MemoryStream stream = null;
            sys.Xml.XmlValidatingReader validator = null;
            try {
                // Get the serialized NullableBoolean instance
                stream = new sys.IO.MemoryStream();
                serializer.Serialize(stream, x);
                stream.Seek(0, sys.IO.SeekOrigin.Begin); // Return stream to start

                // Add the default namespace
                sysXml.XmlDocument doc = new sysXml.XmlDocument();
                doc.Load(stream);                    
                sysXml.XmlAttribute defaultNs = doc.CreateAttribute("xmlns");
                defaultNs.Value = "http://NullableTypes.SourceForge.Net/NullableBooleanXMLSchema";
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

        #endregion // Serialization Tests
    }
}