//
// NullableTypes.HelperFunctions.Tests.DBNullConvert
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes    Reasons
// 16-May-2003  Luca    Create
// 09-Aug-2003  Luca    Upgrade    Code upgrade: changed namespace NullableTypes.Data to 
//                                 NullableTypes.HelperFunctions as for DBNullConvert class
// 06-Oct-2003  Luca    Upgrade    Code upgrade: Replaced tabs with spaces
//

namespace NullableTypes.HelperFunctions.Tests
{

    using nu = NUnit.Framework;
    using nua = NUnit.Framework.Assertion;
    using sys = System;
    using sysDat = System.Data;
    
    [nu.TestFixture]
    public class DBNullConvertTest {

        sysDat.DataTable _dbTable;

        [nu.SetUp]
        public void SetUp() {
            _dbTable = new sysDat.DataTable();
        }


        #region DbValue to NullableTypes Tests - A#

        [nu.Test]
        public void ToNullableBoolean() {
            _dbTable.Columns.Add("System.Boolean", typeof(bool));

            _dbTable.Rows.Add(new object[] {true});
            nua.AssertEquals(
                "TestA#01", 
                NullableBoolean.True, 
                DBNullConvert.ToNullableBoolean(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {false});
            nua.AssertEquals(
                "TestA#02", 
                NullableBoolean.False, 
                DBNullConvert.ToNullableBoolean(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {sys.DBNull.Value});
            nua.AssertEquals(
                "TestA#03", 
                NullableBoolean.Null, 
                DBNullConvert.ToNullableBoolean(_dbTable.Rows[_dbTable.Rows.Count-1][0]));

            _dbTable.Rows.Add(new object[] {null});
            nua.AssertEquals(
                "TestA#04", 
                NullableBoolean.Null, 
                DBNullConvert.ToNullableBoolean(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.InvalidCastException))]
        public void ToNullableBooleanInvalidCastException() {
            _dbTable.Columns.Add("System.Int32", typeof(int));
            _dbTable.Rows.Add(new object[] {1000});
            DBNullConvert.ToNullableBoolean(_dbTable.Rows[0][0]);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableBooleanArgumentNullException() {
            DBNullConvert.ToNullableBoolean(null);
        }


        [nu.Test]
        public void ToNullableByte() {
            _dbTable.Columns.Add("System.Byte", typeof(byte));

            _dbTable.Rows.Add(new object[] {byte.MinValue});
            nua.AssertEquals(
                "TestA#11", 
                NullableByte.MinValue, 
                DBNullConvert.ToNullableByte(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {byte.MaxValue});
            nua.AssertEquals(
                "TestA#12", 
                NullableByte.MaxValue, 
                DBNullConvert.ToNullableByte(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {sys.DBNull.Value});
            nua.AssertEquals(
                "TestA#13", 
                NullableByte.Null, 
                DBNullConvert.ToNullableByte(_dbTable.Rows[_dbTable.Rows.Count-1][0]));

            _dbTable.Rows.Add(new object[] {null});
            nua.AssertEquals(
                "TestA#14", 
                NullableByte.Null, 
                DBNullConvert.ToNullableByte(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.InvalidCastException))]
        public void ToNullableByteInvalidCastException() {
            _dbTable.Columns.Add("System.Int32", typeof(int));
            _dbTable.Rows.Add(new object[] {1000});
            DBNullConvert.ToNullableByte(_dbTable.Rows[0][0]);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableByteArgumentNullException() {
            DBNullConvert.ToNullableByte(null);
        }


        [nu.Test]
        public void ToNullableInt16() {
            _dbTable.Columns.Add("System.Int16", typeof(short));

            _dbTable.Rows.Add(new object[] {short.MinValue});
            nua.AssertEquals(
                "TestA#21", 
                NullableInt16.MinValue, 
                DBNullConvert.ToNullableInt16(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {short.MaxValue});
            nua.AssertEquals(
                "TestA#22", 
                NullableInt16.MaxValue, 
                DBNullConvert.ToNullableInt16(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {sys.DBNull.Value});
            nua.AssertEquals(
                "TestA#23", 
                NullableInt16.Null, 
                DBNullConvert.ToNullableInt16(_dbTable.Rows[_dbTable.Rows.Count-1][0]));

            _dbTable.Rows.Add(new object[] {null});
            nua.AssertEquals(
                "TestA#24", 
                NullableInt16.Null, 
                DBNullConvert.ToNullableInt16(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.InvalidCastException))]
        public void ToNullableInt16InvalidCastException() {
            _dbTable.Columns.Add("System.Boolean", typeof(bool));
            _dbTable.Rows.Add(new object[] {false});
            DBNullConvert.ToNullableInt16(_dbTable.Rows[0][0]);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableInt16ArgumentNullException() {
            DBNullConvert.ToNullableInt16(null);
        }


        [nu.Test]
        public void ToNullableInt32() {
            _dbTable.Columns.Add("System.Int32", typeof(int));

            _dbTable.Rows.Add(new object[] {int.MinValue});
            nua.AssertEquals(
                "TestA#31", 
                NullableInt32.MinValue, 
                DBNullConvert.ToNullableInt32(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {int.MaxValue});
            nua.AssertEquals(
                "TestA#32", 
                NullableInt32.MaxValue, 
                DBNullConvert.ToNullableInt32(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {sys.DBNull.Value});
            nua.AssertEquals(
                "TestA#33", 
                NullableInt32.Null, 
                DBNullConvert.ToNullableInt32(_dbTable.Rows[_dbTable.Rows.Count-1][0]));

            _dbTable.Rows.Add(new object[] {null});
            nua.AssertEquals(
                "TestA#34", 
                NullableInt32.Null, 
                DBNullConvert.ToNullableInt32(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.InvalidCastException))]
        public void ToNullableInt32InvalidCastException() {
            _dbTable.Columns.Add("System.Int32", typeof(bool));
            _dbTable.Rows.Add(new object[] {false});
            DBNullConvert.ToNullableInt32(_dbTable.Rows[0][0]);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableInt32ArgumentNullException() {
            DBNullConvert.ToNullableInt32(null);
        }


        [nu.Test]
        public void ToNullableInt64() {
            _dbTable.Columns.Add("System.Int64", typeof(long));

            _dbTable.Rows.Add(new object[] {long.MinValue});
            nua.AssertEquals(
                "TestA#41", 
                NullableInt64.MinValue, 
                DBNullConvert.ToNullableInt64(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {long.MaxValue});
            nua.AssertEquals(
                "TestA#42", 
                NullableInt64.MaxValue, 
                DBNullConvert.ToNullableInt64(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {sys.DBNull.Value});
            nua.AssertEquals(
                "TestA#43", 
                NullableInt64.Null, 
                DBNullConvert.ToNullableInt64(_dbTable.Rows[_dbTable.Rows.Count-1][0]));

            _dbTable.Rows.Add(new object[] {null});
            nua.AssertEquals(
                "TestA#44", 
                NullableInt64.Null, 
                DBNullConvert.ToNullableInt64(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.InvalidCastException))]
        public void ToNullableInt64InvalidCastException() {
            _dbTable.Columns.Add("System.Int64", typeof(bool));
            _dbTable.Rows.Add(new object[] {false});
            DBNullConvert.ToNullableInt64(_dbTable.Rows[0][0]);
        }


        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableInt64ArgumentNullException() {
            DBNullConvert.ToNullableInt64(null);
        }


        [nu.Test]
        public void ToNullableSingle() {
            _dbTable.Columns.Add("System.Single", typeof(float));

            _dbTable.Rows.Add(new object[] {float.MinValue});
            nua.AssertEquals(
                "TestA#51", 
                NullableSingle.MinValue, 
                DBNullConvert.ToNullableSingle(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {float.MaxValue});
            nua.AssertEquals(
                "TestA#52", 
                NullableSingle.MaxValue, 
                DBNullConvert.ToNullableSingle(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {sys.DBNull.Value});
            nua.AssertEquals(
                "TestA#53", 
                NullableSingle.Null, 
                DBNullConvert.ToNullableSingle(_dbTable.Rows[_dbTable.Rows.Count-1][0]));

            _dbTable.Rows.Add(new object[] {null});
            nua.AssertEquals(
                "TestA#54", 
                NullableSingle.Null, 
                DBNullConvert.ToNullableSingle(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.InvalidCastException))]
        public void ToNullableSingleInvalidCastException() {
            _dbTable.Columns.Add("System.Single", typeof(bool));
            _dbTable.Rows.Add(new object[] {false});
            DBNullConvert.ToNullableSingle(_dbTable.Rows[0][0]);
        }
        

        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableSingleArgumentNullException() {
            DBNullConvert.ToNullableSingle(null);
        }


        [nu.Test]
        public void ToNullableDouble() {
            _dbTable.Columns.Add("System.Double", typeof(double));

            _dbTable.Rows.Add(new object[] {double.MinValue});
            nua.AssertEquals(
                "TestA#61", 
                NullableDouble.MinValue, 
                DBNullConvert.ToNullableDouble(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {double.MaxValue});
            nua.AssertEquals(
                "TestA#62", 
                NullableDouble.MaxValue, 
                DBNullConvert.ToNullableDouble(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {sys.DBNull.Value});
            nua.AssertEquals(
                "TestA#63", 
                NullableDouble.Null, 
                DBNullConvert.ToNullableDouble(_dbTable.Rows[_dbTable.Rows.Count-1][0]));

            _dbTable.Rows.Add(new object[] {null});
            nua.AssertEquals(
                "TestA#64", 
                NullableDouble.Null, 
                DBNullConvert.ToNullableDouble(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.InvalidCastException))]
        public void ToNullableDoubleInvalidCastException() {
            _dbTable.Columns.Add("System.Double", typeof(bool));
            _dbTable.Rows.Add(new object[] {false});
            DBNullConvert.ToNullableDouble(_dbTable.Rows[0][0]);
        }
        

        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableDoubleArgumentNullException() {
            DBNullConvert.ToNullableDouble(null);
        }


        [nu.Test]
        public void ToNullableDateTime() {
            _dbTable.Columns.Add("System.DateTime", typeof(sys.DateTime));

            _dbTable.Rows.Add(new object[] {sys.DateTime.MinValue});
            nua.AssertEquals(
                "TestA#71", 
                NullableDateTime.MinValue, 
                DBNullConvert.ToNullableDateTime(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {sys.DateTime.MaxValue});
            nua.AssertEquals(
                "TestA#72", 
                NullableDateTime.MaxValue, 
                DBNullConvert.ToNullableDateTime(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {sys.DBNull.Value});
            nua.AssertEquals(
                "TestA#73", 
                NullableDateTime.Null, 
                DBNullConvert.ToNullableDateTime(_dbTable.Rows[_dbTable.Rows.Count-1][0]));

            _dbTable.Rows.Add(new object[] {null});
            nua.AssertEquals(
                "TestA#74", 
                NullableDateTime.Null, 
                DBNullConvert.ToNullableDateTime(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.InvalidCastException))]
        public void ToNullableDateTimeInvalidCastException() {
            DBNullConvert.ToNullableDateTime(false);
        }
        

        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableDateTimeArgumentNullException() {
            DBNullConvert.ToNullableDateTime(null);
        }


        [nu.Test]
        public void ToNullableDecimal() {
            _dbTable.Columns.Add("System.Decimal", typeof(sys.Decimal));

            _dbTable.Rows.Add(new object[] {sys.Decimal.MinValue});
            nua.AssertEquals(
                "TestA#81", 
                NullableDecimal.MinValue, 
                DBNullConvert.ToNullableDecimal(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {sys.Decimal.MaxValue});
            nua.AssertEquals(
                "TestA#82", 
                NullableDecimal.MaxValue, 
                DBNullConvert.ToNullableDecimal(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {sys.DBNull.Value});
            nua.AssertEquals(
                "TestA#83", 
                NullableDecimal.Null, 
                DBNullConvert.ToNullableDecimal(_dbTable.Rows[_dbTable.Rows.Count-1][0]));

            _dbTable.Rows.Add(new object[] {null});
            nua.AssertEquals(
                "TestA#84", 
                NullableDecimal.Null, 
                DBNullConvert.ToNullableDecimal(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.InvalidCastException))]
        public void ToNullableDecimalInvalidCastException() {
            DBNullConvert.ToNullableDecimal("gino");
        }
        

        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableDecimalArgumentNullException() {
            DBNullConvert.ToNullableDecimal(null);
        }


        [nu.Test]
        public void ToNullableString() {
            _dbTable.Columns.Add("System.String", typeof(string));

            _dbTable.Rows.Add(new object[] {sys.String.Empty});
            nua.AssertEquals(
                "TestA#91", 
                NullableString.Empty, 
                DBNullConvert.ToNullableString(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {"Gino Guatenalbare"});
            nua.AssertEquals(
                "TestA#92", 
                new NullableString("Gino Guatenalbare"), 
                DBNullConvert.ToNullableString(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
            
            _dbTable.Rows.Add(new object[] {sys.DBNull.Value});
            nua.AssertEquals(
                "TestA#93", 
                NullableString.Null, 
                DBNullConvert.ToNullableString(_dbTable.Rows[_dbTable.Rows.Count-1][0]));

            _dbTable.Rows.Add(new object[] {null});
            nua.AssertEquals(
                "TestA#94", 
                NullableString.Null, 
                DBNullConvert.ToNullableString(_dbTable.Rows[_dbTable.Rows.Count-1][0]));
        }


        [nu.Test, nu.ExpectedException(typeof(sys.InvalidCastException))]
        public void ToNullableStringInvalidCastException() {
            _dbTable.Columns.Add("System.String", typeof(bool));
            _dbTable.Rows.Add(new object[] {false});
            DBNullConvert.ToNullableString(_dbTable.Rows[0][0]);
        }
        

        [nu.Test, nu.ExpectedException(typeof(sys.ArgumentNullException))]
        public void ToNullableStringArgumentNullException() {
            DBNullConvert.ToNullableString(null);
        }


        #endregion // DbValue to NullableTypes Tests - A#

        #region DbValue from NullableTypes Tests - B#
        [nu.Test]
        public void FromNullableBoolean() {
            nua.AssertEquals("TestB#001", 
                             sys.DBNull.Value, 
                             DBNullConvert.From(NullableBoolean.Null));
            nua.AssertEquals("TestB#002", 
                             true, 
                             DBNullConvert.From(NullableBoolean.True));
            nua.AssertEquals("TestB#003", 
                             false, 
                             DBNullConvert.From(NullableBoolean.False));
        }


        [nu.Test]
        public void FromNullableByte() {
            nua.AssertEquals("TestB#011", 
                sys.DBNull.Value, 
                DBNullConvert.From(NullableByte.Null));
            nua.AssertEquals("TestB#012", 
                byte.MinValue, 
                DBNullConvert.From(NullableByte.MinValue));
            nua.AssertEquals("TestB#013", 
                byte.MaxValue, 
                DBNullConvert.From(NullableByte.MaxValue));
        }


        [nu.Test]
        public void FromNullableInt16() {
            nua.AssertEquals("TestB#021", 
                sys.DBNull.Value, 
                DBNullConvert.From(NullableInt16.Null));
            nua.AssertEquals("TestB#022", 
                short.MinValue, 
                DBNullConvert.From(NullableInt16.MinValue));
            nua.AssertEquals("TestB#023", 
                short.MaxValue, 
                DBNullConvert.From(NullableInt16.MaxValue));
        }


        [nu.Test]
        public void FromNullableInt32() {
            nua.AssertEquals("TestB#031", 
                sys.DBNull.Value, 
                DBNullConvert.From(NullableInt32.Null));
            nua.AssertEquals("TestB#032", 
                int.MinValue, 
                DBNullConvert.From(NullableInt32.MinValue));
            nua.AssertEquals("TestB#033", 
                int.MaxValue, 
                DBNullConvert.From(NullableInt32.MaxValue));
        }


        [nu.Test]
        public void FromNullableInt64() {
            nua.AssertEquals("TestB#041", 
                sys.DBNull.Value, 
                DBNullConvert.From(NullableInt64.Null));
            nua.AssertEquals("TestB#042", 
                long.MinValue, 
                DBNullConvert.From(NullableInt64.MinValue));
            nua.AssertEquals("TestB#043", 
                long.MaxValue, 
                DBNullConvert.From(NullableInt64.MaxValue));
        }        


        [nu.Test]
        public void FromNullableSingle() {
            nua.AssertEquals("TestB#051", 
                sys.DBNull.Value, 
                DBNullConvert.From(NullableSingle.Null));
            nua.AssertEquals("TestB#052", 
                float.MinValue, 
                DBNullConvert.From(NullableSingle.MinValue));
            nua.AssertEquals("TestB#053", 
                float.MaxValue, 
                DBNullConvert.From(NullableSingle.MaxValue));
        }        


        [nu.Test]
        public void FromNullableDouble() {
            nua.AssertEquals("TestB#061", 
                sys.DBNull.Value, 
                DBNullConvert.From(NullableDouble.Null));
            nua.AssertEquals("TestB#062", 
                double.MinValue, 
                DBNullConvert.From(NullableDouble.MinValue));
            nua.AssertEquals("TestB#063", 
                double.MaxValue, 
                DBNullConvert.From(NullableDouble.MaxValue));
        }        


        [nu.Test]
        public void FromNullableDateTime() {
            nua.AssertEquals("TestB#071", 
                sys.DBNull.Value, 
                DBNullConvert.From(NullableDateTime.Null));
            nua.AssertEquals("TestB#072", 
                sys.DateTime.MinValue, 
                DBNullConvert.From(NullableDateTime.MinValue));
            nua.AssertEquals("TestB#073", 
                sys.DateTime.MaxValue, 
                DBNullConvert.From(NullableDateTime.MaxValue));
        }        


        [nu.Test]
        public void FromNullableDecimal() {
            nua.AssertEquals("TestB#081", 
                sys.DBNull.Value, 
                DBNullConvert.From(NullableDecimal.Null));
            nua.AssertEquals("TestB#082", 
                sys.Decimal.MinValue, 
                DBNullConvert.From(NullableDecimal.MinValue));
            nua.AssertEquals("TestB#083", 
                sys.Decimal.MaxValue, 
                DBNullConvert.From(NullableDecimal.MaxValue));
        }        


        [nu.Test]
        public void FromNullableString() {
            nua.AssertEquals("TestB#081", 
                sys.DBNull.Value, 
                DBNullConvert.From(NullableString.Null));
            nua.AssertEquals("TestB#082", 
                string.Empty, 
                DBNullConvert.From(NullableString.Empty));
            nua.AssertEquals("TestB#083", 
                "petroKle", 
                DBNullConvert.From(new NullableString("petroKle")));
        }        


        #endregion // DbValue from NullableTypes Tests - B#


    }

}
