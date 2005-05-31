//
// NullableTypes.HelperFunctions.DBNullConvert
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes      Reasons
// 16-May-2003  Luca    Create
// 19-May-2003  Luca    Create       Added XML documentation
// 27-Jun-2003  Luca    Refactoring  Unified equivalent error messages 
// 07-Jul-2003  Luca    Upgrade      Applied FxCop guideline: explicited CurrentCulture in ToXxx code and
//                                   renamed DbValueConvert to DBNullConvert
// 09-Aug-2003  Luca    Upgrade      Code upgrade: changed namespace NullableTypes.Data to 
//                                   NullableTypes.HelperFunctions
// 06-Oct-2003  Luca    Upgrade      Code upgrade: Replaced tabs with spaces
//

namespace NullableTypes.HelperFunctions
{
    using sys = System;
    using sysGlb = System.Globalization;

    /// <summary>
    /// Converts values coming from .NET Data Providers (Command Parameters and
    /// DataReader values) and from DataSet (DataRow Items value) to NullableTypes
    /// and vice versa.
    /// </summary>
    public sealed class DBNullConvert
    {
        private DBNullConvert() {}

        /// <summary>
        /// Converts an object value that is DBNull or System.Boolean to an 
        /// equivalent NullableBoolean value.
        /// </summary>
        /// <param name="x">
        /// A value of <see cref="System.Boolean"/> type from either a 
        /// <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> GetValue returned value.
        /// </param>
        /// <returns>
        /// <see cref="NullableBoolean.Null"/> if <paramref name="x"/> is 
        /// <see cref="System.DBNull"/> otherwise a 
        /// <see cref="NullableBoolean"/> constructed from <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is null.
        /// </exception>
        /// <exception cref="System.InvalidCastException">
        /// <paramref name="x"/> is neither DBNull or of type 
        /// <see cref="System.Boolean"/>.
        /// </exception>
        public static NullableBoolean ToNullableBoolean(object x) {
            if (x == sys.DBNull.Value)
                return NullableBoolean.Null;

            if (x is bool)
                return new NullableBoolean((bool)x);

            if (x == null)
                throw new sys.ArgumentNullException("x");

            throw new sys.InvalidCastException(
                string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText(
                    "Value is not a {0} neither a DBNull Value."), "System.Boolean"));
        }


        /// <summary>
        /// Converts an object value that is DBNull or System.Byte to an 
        /// equivalent NullableByte value.
        /// </summary>
        /// <param name="x">
        /// A value of <see cref="System.Byte"/> type from either a 
        /// <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> GetValue returned value.
        /// </param>
        /// <returns>
        /// <see cref="NullableByte.Null"/> if <paramref name="x"/> is 
        /// <see cref="System.DBNull"/> otherwise a 
        /// <see cref="NullableByte"/> constructed from <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is null.
        /// </exception>
        /// <exception cref="System.InvalidCastException">
        /// <paramref name="x"/> is neither DBNull or of type 
        /// <see cref="System.Byte"/>.
        /// </exception>
        public static NullableByte ToNullableByte(object x) {
            if (x == sys.DBNull.Value)
                return NullableByte.Null;

            if (x is byte)
                return new NullableByte((byte)x);

            if (x == null)
                throw new sys.ArgumentNullException("x");

            throw new sys.InvalidCastException(
                string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText(
                "Value is not a {0} neither a DBNull Value."), "System.Byte"));
        }


        /// <summary>
        /// Converts an object value that is DBNull or System.Int16 to an 
        /// equivalent NullableInt16 value.
        /// </summary>
        /// <param name="x">
        /// A value of <see cref="System.Int16"/> type from either a 
        /// <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> GetValue returned value.
        /// </param>
        /// <returns>
        /// <see cref="NullableInt16.Null"/> if <paramref name="x"/> is 
        /// <see cref="System.DBNull"/> otherwise a 
        /// <see cref="NullableInt16"/> constructed from <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is null.
        /// </exception>
        /// <exception cref="System.InvalidCastException">
        /// <paramref name="x"/> is neither DBNull or of type 
        /// <see cref="System.Int16"/>.
        /// </exception>
        public static NullableInt16 ToNullableInt16(object x) {
            if (x == sys.DBNull.Value)
                return NullableInt16.Null;

            if (x is short)
                return new NullableInt16((short)x);

            if (x == null)
                throw new sys.ArgumentNullException("x");

            throw new sys.InvalidCastException(
                string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText(
                "Value is not a {0} neither a DBNull Value."), "System.Int16"));
        }


        /// <summary>
        /// Converts an object value that is DBNull or System.Int32 to an 
        /// equivalent NullableInt32 value.
        /// </summary>
        /// <param name="x">
        /// A value of <see cref="System.Int32"/> type from either a 
        /// <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> GetValue returned value.
        /// </param>
        /// <returns>
        /// <see cref="NullableInt32.Null"/> if <paramref name="x"/> is 
        /// <see cref="System.DBNull"/> otherwise a 
        /// <see cref="NullableInt32"/> constructed from <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is null.
        /// </exception>
        /// <exception cref="System.InvalidCastException">
        /// <paramref name="x"/> is neither DBNull or of type 
        /// <see cref="System.Int32"/>.
        /// </exception>
        public static NullableInt32 ToNullableInt32(object x) {
            if (x == sys.DBNull.Value)
                return NullableInt32.Null;

            if (x is int)
                return new NullableInt32((int)x);

            if (x == null)
                throw new sys.ArgumentNullException("x");

            throw new sys.InvalidCastException(
                string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText(
                "Value is not a {0} neither a DBNull Value."), "System.Int32"));
        }


        /// <summary>
        /// Converts an object value that is DBNull or System.Int64 to an 
        /// equivalent NullableInt64 value.
        /// </summary>
        /// <param name="x">
        /// A value of <see cref="System.Int64"/> type from either a 
        /// <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> GetValue returned value.
        /// </param>
        /// <returns>
        /// <see cref="NullableInt64.Null"/> if <paramref name="x"/> is 
        /// <see cref="System.DBNull"/> otherwise a 
        /// <see cref="NullableInt64"/> constructed from <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is null.
        /// </exception>
        /// <exception cref="System.InvalidCastException">
        /// <paramref name="x"/> is neither DBNull or of type 
        /// <see cref="System.Int64"/>.
        /// </exception>
        public static NullableInt64 ToNullableInt64(object x) {
            if (x == sys.DBNull.Value)
                return NullableInt64.Null;

            if (x is long)
                return new NullableInt64((long)x);

            if (x == null)
                throw new sys.ArgumentNullException("x");

            throw new sys.InvalidCastException(
                string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText(
                "Value is not a {0} neither a DBNull Value."), "System.Int64"));
        }


        /// <summary>
        /// Converts an object value that is DBNull or System.Single to an 
        /// equivalent NullableSingle value.
        /// </summary>
        /// <param name="x">
        /// A value of <see cref="System.Single"/> type from either a 
        /// <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> GetValue returned value.
        /// </param>
        /// <returns>
        /// <see cref="NullableSingle.Null"/> if <paramref name="x"/> is 
        /// <see cref="System.DBNull"/> otherwise a 
        /// <see cref="NullableSingle"/> constructed from <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is null.
        /// </exception>
        /// <exception cref="System.InvalidCastException">
        /// <paramref name="x"/> is neither DBNull or of type 
        /// <see cref="System.Single"/>.
        /// </exception>
        public static NullableSingle ToNullableSingle(object x) {
            if (x == sys.DBNull.Value)
                return NullableSingle.Null;

            if (x is float)
                return new NullableSingle((float)x);

            if (x == null)
                throw new sys.ArgumentNullException("x");

            throw new sys.InvalidCastException(
                string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText(
                "Value is not a {0} neither a DBNull Value."), "System.Single"));
        }


        /// <summary>
        /// Converts an object value that is DBNull or System.Double to an 
        /// equivalent NullableDouble value.
        /// </summary>
        /// <param name="x">
        /// A value of <see cref="System.Double"/> type from either a 
        /// <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> GetValue returned value.
        /// </param>
        /// <returns>
        /// <see cref="NullableDouble.Null"/> if <paramref name="x"/> is 
        /// <see cref="System.DBNull"/> otherwise a 
        /// <see cref="NullableDouble"/> constructed from <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is null.
        /// </exception>
        /// <exception cref="System.InvalidCastException">
        /// <paramref name="x"/> is neither DBNull or of type 
        /// <see cref="System.Double"/>.
        /// </exception>
        public static NullableDouble ToNullableDouble(object x) {
            if (x == sys.DBNull.Value)
                return NullableDouble.Null;

            if (x is double)
                return new NullableDouble((double)x);

            if (x == null)
                throw new sys.ArgumentNullException("x");

            throw new sys.InvalidCastException(
                string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText(
                "Value is not a {0} neither a DBNull Value."), "System.Double"));
        }


        /// <summary>
        /// Converts an object value that is DBNull or System.DateTime to an 
        /// equivalent NullableDateTime value.
        /// </summary>
        /// <param name="x">
        /// A value of <see cref="System.DateTime"/> type from either a 
        /// <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> GetValue returned value.
        /// </param>
        /// <returns>
        /// <see cref="NullableDateTime.Null"/> if <paramref name="x"/> is 
        /// <see cref="System.DBNull"/> otherwise a 
        /// <see cref="NullableDateTime"/> constructed from <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is null.
        /// </exception>
        /// <exception cref="System.InvalidCastException">
        /// <paramref name="x"/> is neither DBNull or of type 
        /// <see cref="System.DateTime"/>.
        /// </exception>
        public static NullableDateTime ToNullableDateTime(object x) {
            if (x == sys.DBNull.Value)
                return NullableDateTime.Null;

            if (x is sys.DateTime)
                return new NullableDateTime((sys.DateTime)x);

            if (x == null)
                throw new sys.ArgumentNullException("x");

            throw new sys.InvalidCastException(
                string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText(
                "Value is not a {0} neither a DBNull Value."), "System.DateTime"));
        }


        /// <summary>
        /// Converts an object value that is DBNull or System.Decimal to an 
        /// equivalent NullableDecimal value.
        /// </summary>
        /// <param name="x">
        /// A value of <see cref="System.Decimal"/> type from either a 
        /// <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> GetValue returned value.
        /// </param>
        /// <returns>
        /// <see cref="NullableDecimal.Null"/> if <paramref name="x"/> is 
        /// <see cref="System.DBNull"/> otherwise a 
        /// <see cref="NullableDecimal"/> constructed from <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is null.
        /// </exception>
        /// <exception cref="System.InvalidCastException">
        /// <paramref name="x"/> is neither DBNull or of type 
        /// <see cref="System.Decimal"/>.
        /// </exception>
        public static NullableDecimal ToNullableDecimal(object x) {
            if (x == sys.DBNull.Value)
                return NullableDecimal.Null;

            if (x is sys.Decimal)
                return new NullableDecimal((sys.Decimal)x);

            if (x == null)
                throw new sys.ArgumentNullException("x");

            throw new sys.InvalidCastException(
                string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText(
                "Value is not a {0} neither a DBNull Value."), "System.Decimal"));
        }


        /// <summary>
        /// Converts an object value that is DBNull or System.String to an 
        /// equivalent NullableString value.
        /// </summary>
        /// <param name="x">
        /// A value of <see cref="System.String"/> type from either a 
        /// <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> GetValue returned value.
        /// </param>
        /// <returns>
        /// <see cref="NullableString.Null"/> if <paramref name="x"/> is 
        /// <see cref="System.DBNull"/> otherwise a 
        /// <see cref="NullableString"/> constructed from <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is null.
        /// </exception>
        /// <exception cref="System.InvalidCastException">
        /// <paramref name="x"/> is neither DBNull or of type 
        /// <see cref="System.String"/>.
        /// </exception>
        public static NullableString ToNullableString(object x) {
            if (x == sys.DBNull.Value)
                return NullableString.Null;

            if (x is string)
                return new NullableString((string)x);

            if (x == null)
                throw new sys.ArgumentNullException("x");

            throw new sys.InvalidCastException(
                string.Format(sysGlb.CultureInfo.CurrentCulture, Locale.GetText(
                "Value is not a {0} neither a DBNull Value."), "System.String"));
        }


        /// <summary>
        /// Converts the specified value to an 
        /// equivalent <see cref="System.Object"/> value that can be assigned to 
        /// a <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> Item value.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableBoolean"/> value. 
        /// </param>
        /// <returns>
        /// <see cref="System.DBNull"/> if <paramref name="x"/> is 
        /// <see cref="NullableBoolean.Null"/> otherwise 
        /// the  <see cref="NullableBoolean.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static object From(NullableBoolean x) {
            if (x.IsNull)
                return sys.DBNull.Value;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified value to an 
        /// equivalent <see cref="System.Object"/> value that can be assigned to 
        /// a <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> Item value.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableByte"/> value. 
        /// </param>
        /// <returns>
        /// <see cref="System.DBNull"/> if <paramref name="x"/> is 
        /// <see cref="NullableByte.Null"/> otherwise
        /// the <see cref="NullableByte.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static object From(NullableByte x) {
            if (x.IsNull)
                return sys.DBNull.Value;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified value to an 
        /// equivalent <see cref="System.Object"/> value that can be assigned to 
        /// a <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> Item value.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt16"/> value. 
        /// </param>
        /// <returns>
        /// <see cref="System.DBNull"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt16.Null"/> otherwise
        /// the <see cref="NullableInt16.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static object From(NullableInt16 x) {
            if (x.IsNull)
                return sys.DBNull.Value;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified value to an 
        /// equivalent <see cref="System.Object"/> value that can be assigned to 
        /// a <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> Item value.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt32"/> value. 
        /// </param>
        /// <returns>
        /// <see cref="System.DBNull"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt32.Null"/> otherwise 
        /// the <see cref="NullableInt32.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static object From(NullableInt32 x) {
            if (x.IsNull)
                return sys.DBNull.Value;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified value to an 
        /// equivalent <see cref="System.Object"/> value that can be assigned to 
        /// a <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> Item value.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableInt64"/> value. 
        /// </param>
        /// <returns>
        /// <see cref="System.DBNull"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt64.Null"/> otherwise
        /// the <see cref="NullableInt64.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static object From(NullableInt64 x) {
            if (x.IsNull)
                return sys.DBNull.Value;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified value to an 
        /// equivalent <see cref="System.Object"/> value that can be assigned to 
        /// a <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> Item value.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableSingle"/> value. 
        /// </param>
        /// <returns>
        /// <see cref="System.DBNull"/> if <paramref name="x"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise 
        /// the <see cref="NullableSingle.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static object From(NullableSingle x) {
            if (x.IsNull)
                return sys.DBNull.Value;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified value to an 
        /// equivalent <see cref="System.Object"/> value that can be assigned to 
        /// a <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> Item value.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDouble"/> value. 
        /// </param>
        /// <returns>
        /// <see cref="System.DBNull"/> if <paramref name="x"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise 
        /// the <see cref="NullableDouble.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static object From(NullableDouble x) {
            if (x.IsNull)
                return sys.DBNull.Value;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified value to an 
        /// equivalent <see cref="System.Object"/> value that can be assigned to 
        /// a <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> Item value.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDateTime"/> value. 
        /// </param>
        /// <returns>
        /// <see cref="System.DBNull"/> if <paramref name="x"/> is 
        /// <see cref="NullableDateTime.Null"/> otherwise 
        /// the <see cref="NullableDateTime.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static object From(NullableDateTime x) {
            if (x.IsNull)
                return sys.DBNull.Value;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified value to an 
        /// equivalent <see cref="System.Object"/> value that can be assigned to 
        /// a <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> Item value.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableDecimal"/> value. 
        /// </param>
        /// <returns>
        /// <see cref="System.DBNull"/> if <paramref name="x"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise 
        /// the <see cref="NullableDecimal.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static object From(NullableDecimal x) {
            if (x.IsNull)
                return sys.DBNull.Value;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified value to an 
        /// equivalent <see cref="System.Object"/> value that can be assigned to 
        /// a <see cref="System.Data.DataRow"/> Item value, a 
        /// <see cref="System.Data.IDbCommand.Parameters"/> Item value or a 
        /// <see cref="System.Data.IDataRecord"/> Item value.
        /// </summary>
        /// <param name="x">
        /// A <see cref="NullableString"/> value. 
        /// </param>
        /// <returns>
        /// <see cref="System.DBNull"/> if <paramref name="x"/> is 
        /// <see cref="NullableString.Null"/> otherwise 
        /// the <see cref="NullableString.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static object From(NullableString x) {
            if (x.IsNull)
                return sys.DBNull.Value;

            return x.Value;
        }
    }
}
