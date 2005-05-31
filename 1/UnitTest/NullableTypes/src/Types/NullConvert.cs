//
// NullableTypes.HelperFunctions.NullConvert
// 
// Authors: Luca Minudel (lukadotnet@users.sourceforge.net)
//
// Date         Author  Changes      Reasons
// 19-Aug-2003  Luca    Create
// 23-Aug-2003  Luca    Upgrade      New requirement: The ability to obtain a string representation of a 
//                                   nullable type value with a specified string to represent the null value 
//                                   and with a specified format.
// 06-Oct-2003  Luca    Upgrade      Code upgrade: Replaced tabs with spaces
//

namespace NullableTypes.HelperFunctions
{
    using sys = System;
    using sysGlb = System.Globalization;

    /// <summary>
    /// Converts non nullable .NET built-in types to and from NullableTypes using conventional values to 
    /// simulate null values. Can be used to seamlessly integrate NullableTypes with Web server controls 
    /// and WinForms controls.
    /// </summary>
    public sealed class NullConvert {
        private NullConvert() {}


        /// <summary>
        /// Converts the specified <see cref="NullableBoolean"/> value to an equivalent 
        /// <see cref="System.Boolean"/> value using a conventional <see cref="System.Boolean"/> value to 
        /// represent the <see cref="NullableBoolean.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableBoolean"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Boolean"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableBoolean.Null"/> otherwise the  <see cref="NullableBoolean.Value"/> of 
        /// <paramref name="x"/>.
        /// </returns>
        public static bool From(NullableBoolean x, bool conventionalNullValue) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified <see cref="NullableBoolean"/> value to an equivalent 
        /// <see cref="System.String"/> value using a conventional <see cref="System.String"/> value to 
        /// represent the <see cref="NullableBoolean.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableBoolean"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableBoolean.Null"/> otherwise the string representation of the 
        /// <see cref="NullableBoolean.Value"/> of <paramref name="x"/>.
        /// </returns>
        public static string From(NullableBoolean x, string conventionalNullValue) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value.ToString(sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Converts the specified <see cref="NullableByte"/> value to an equivalent 
        /// <see cref="System.Byte"/> value using a conventional <see cref="System.Byte"/> value to 
        /// represent the <see cref="NullableByte.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableByte"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Byte"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableByte.Null"/> otherwise the  <see cref="NullableByte.Value"/> of 
        /// <paramref name="x"/>.
        /// </returns>
        public static byte From(NullableByte x, byte conventionalNullValue) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified <see cref="NullableByte"/> value to an equivalent 
        /// <see cref="System.String"/> value using a conventional <see cref="System.String"/> value to 
        /// represent the <see cref="NullableByte.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableByte"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the 
        /// <see cref="NullableByte.Null"/> value. 
        /// </param>
        /// <param name="format">
        /// A string that specifies the return format of <paramref name="x"/> if it is not
        /// <see cref="NullableByte.Null"/>. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableByte.Null"/> otherwise the equivalent string of the
        /// <see cref="NullableByte.Value"/> of <paramref name="x"/> formatted as specified by 
        /// <paramref name="format"/>.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> is not <see cref="NullableByte.Null"/> and the <paramref name="format"/> 
        /// is invalid for a Byte. 
        /// </exception>
        /// <remarks>
        /// If <paramref name="format"/> is a null reference (Nothing in Visual Basic) or an empty string (""), 
        /// general format specifier ("G") will be used. 
        /// <para>
        /// The <see cref="System.Globalization.NumberFormatInfo"/> for the current culture is used when 
        /// applying fomatting.
        /// </para>
        /// </remarks>
        public static string From(NullableByte x, string conventionalNullValue, string format) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value.ToString(format, sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Converts the specified <see cref="NullableInt16"/> value to an equivalent 
        /// <see cref="System.Int16"/> value using a conventional <see cref="System.Int16"/> value to 
        /// represent the <see cref="NullableInt16.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableInt16"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Int16"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt16.Null"/> otherwise the  <see cref="NullableInt16.Value"/> of 
        /// <paramref name="x"/>.
        /// </returns>
        public static short From(NullableInt16 x, short conventionalNullValue) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified <see cref="NullableInt16"/> value to an equivalent 
        /// <see cref="System.String"/> value using a conventional <see cref="System.String"/> value to 
        /// represent the <see cref="NullableInt16.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableInt16"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the 
        /// <see cref="NullableInt16.Null"/> value. 
        /// </param>
        /// <param name="format">
        /// A string that specifies the return format of <paramref name="x"/> if it is not
        /// <see cref="NullableInt16.Null"/>. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt16.Null"/> otherwise the equivalent string of the
        /// <see cref="NullableInt16.Value"/> of <paramref name="x"/> formatted as specified by 
        /// <paramref name="format"/>.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> is not <see cref="NullableInt16.Null"/> and the <paramref name="format"/> 
        /// is invalid for an Int16. 
        /// </exception>
        /// <remarks>
        /// If <paramref name="format"/> is a null reference (Nothing in Visual Basic) or an empty string (""), 
        /// general format specifier ("G") will be used. 
        /// <para>
        /// The <see cref="System.Globalization.NumberFormatInfo"/> for the current culture is used when 
        /// applying fomatting.
        /// </para>
        /// </remarks>
        public static string From(NullableInt16 x, string conventionalNullValue, string format) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value.ToString(format, sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Converts the specified <see cref="NullableInt32"/> value to an equivalent 
        /// <see cref="System.Int32"/> value using a conventional <see cref="System.Int32"/> value to 
        /// represent the <see cref="NullableInt32.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableInt32"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Int32"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt32.Null"/> otherwise the  <see cref="NullableInt32.Value"/> of 
        /// <paramref name="x"/>.
        /// </returns>
        public static int From(NullableInt32 x, int conventionalNullValue) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified <see cref="NullableInt32"/> value to an equivalent 
        /// <see cref="System.String"/> value using a conventional <see cref="System.String"/> value to 
        /// represent the <see cref="NullableInt32.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableInt32"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the 
        /// <see cref="NullableInt32.Null"/> value. 
        /// </param>
        /// <param name="format">
        /// A string that specifies the return format of <paramref name="x"/> if it is not
        /// <see cref="NullableInt32.Null"/>. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt32.Null"/> otherwise the equivalent string of the
        /// <see cref="NullableInt32.Value"/> of <paramref name="x"/> formatted as specified by 
        /// <paramref name="format"/>.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> is not <see cref="NullableInt32.Null"/> and the <paramref name="format"/> 
        /// is invalid for an Int32. 
        /// </exception>
        /// <remarks>
        /// If <paramref name="format"/> is a null reference (Nothing in Visual Basic) or an empty string (""), 
        /// general format specifier ("G") will be used. 
        /// <para>
        /// The <see cref="System.Globalization.NumberFormatInfo"/> for the current culture is used when 
        /// applying fomatting.
        /// </para>
        /// </remarks>
        public static string From(NullableInt32 x, string conventionalNullValue, string format) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value.ToString(format, sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Converts the specified <see cref="NullableInt64"/> value to an equivalent 
        /// <see cref="System.Int64"/> value using a conventional <see cref="System.Int64"/> value to 
        /// represent the <see cref="NullableInt64.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableInt64"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Int64"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt64.Null"/> otherwise the  <see cref="NullableInt64.Value"/> of 
        /// <paramref name="x"/>.
        /// </returns>
        public static long From(NullableInt64 x, long conventionalNullValue) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified <see cref="NullableInt64"/> value to an equivalent 
        /// <see cref="System.String"/> value using a conventional <see cref="System.String"/> value to 
        /// represent the <see cref="NullableInt64.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableInt64"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the 
        /// <see cref="NullableInt64.Null"/> value. 
        /// </param>
        /// <param name="format">
        /// A string that specifies the return format of <paramref name="x"/> if it is not
        /// <see cref="NullableInt64.Null"/>. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableInt64.Null"/> otherwise the equivalent string of the
        /// <see cref="NullableInt64.Value"/> of <paramref name="x"/> formatted as specified by 
        /// <paramref name="format"/>.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> is not <see cref="NullableInt64.Null"/> and the <paramref name="format"/> 
        /// is invalid for an Int64. 
        /// </exception>
        /// <remarks>
        /// If <paramref name="format"/> is a null reference (Nothing in Visual Basic) or an empty string (""), 
        /// general format specifier ("G") will be used. 
        /// <para>
        /// The <see cref="System.Globalization.NumberFormatInfo"/> for the current culture is used when 
        /// applying fomatting.
        /// </para>
        /// </remarks>
        public static string From(NullableInt64 x, string conventionalNullValue, string format) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value.ToString(format, sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Converts the specified <see cref="NullableSingle"/> value to an equivalent 
        /// <see cref="System.Single"/> value using a conventional <see cref="System.Single"/> value to 
        /// represent the <see cref="NullableSingle.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableSingle"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Single"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise the  <see cref="NullableSingle.Value"/> of 
        /// <paramref name="x"/>.
        /// </returns>
        public static float From(NullableSingle x, float conventionalNullValue) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified <see cref="NullableSingle"/> value to an equivalent 
        /// <see cref="System.String"/> value using a conventional <see cref="System.String"/> value to 
        /// represent the <see cref="NullableSingle.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableSingle"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the 
        /// <see cref="NullableSingle.Null"/> value. 
        /// </param>
        /// <param name="format">
        /// A string that specifies the return format of <paramref name="x"/> if it is not
        /// <see cref="NullableSingle.Null"/>. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableSingle.Null"/> otherwise the equivalent string of the
        /// <see cref="NullableSingle.Value"/> of <paramref name="x"/> formatted as specified by 
        /// <paramref name="format"/>.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> is not <see cref="NullableSingle.Null"/> and the <paramref name="format"/> 
        /// is invalid for a Single. 
        /// </exception>
        /// <remarks>
        /// If <paramref name="format"/> is a null reference (Nothing in Visual Basic) or an empty string (""), 
        /// general format specifier ("G") will be used. 
        /// <para>
        /// The <see cref="System.Globalization.NumberFormatInfo"/> for the current culture is used when 
        /// applying fomatting.
        /// </para>
        /// </remarks>
        public static string From(NullableSingle x, string conventionalNullValue, string format) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value.ToString(format, sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Converts the specified <see cref="NullableDouble"/> value to an equivalent 
        /// <see cref="System.Double"/> value using a conventional <see cref="System.Double"/> value to 
        /// represent the <see cref="NullableDouble.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableDouble"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Double"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise the  <see cref="NullableDouble.Value"/> of 
        /// <paramref name="x"/>.
        /// </returns>
        public static double From(NullableDouble x, double conventionalNullValue) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified <see cref="NullableDouble"/> value to an equivalent 
        /// <see cref="System.String"/> value using a conventional <see cref="System.String"/> value to 
        /// represent the <see cref="NullableDouble.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableDouble"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the 
        /// <see cref="NullableDouble.Null"/> value. 
        /// </param>
        /// <param name="format">
        /// A string that specifies the return format of <paramref name="x"/> if it is not
        /// <see cref="NullableDouble.Null"/>. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableDouble.Null"/> otherwise the equivalent string of the
        /// <see cref="NullableDouble.Value"/> of <paramref name="x"/> formatted as specified by 
        /// <paramref name="format"/>.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> is not <see cref="NullableDouble.Null"/> and the <paramref name="format"/> 
        /// is invalid for a Double. 
        /// </exception>
        /// <remarks>
        /// If <paramref name="format"/> is a null reference (Nothing in Visual Basic) or an empty string (""), 
        /// general format specifier ("G") will be used. 
        /// <para>
        /// The <see cref="System.Globalization.NumberFormatInfo"/> for the current culture is used when 
        /// applying fomatting.
        /// </para>
        /// </remarks>
        public static string From(NullableDouble x, string conventionalNullValue, string format) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value.ToString(format, sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Converts the specified <see cref="NullableDecimal"/> value to an equivalent 
        /// <see cref="System.Decimal"/> value using a conventional <see cref="System.Decimal"/> value to 
        /// represent the <see cref="NullableDecimal.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableDecimal"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Decimal"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise the  <see cref="NullableDecimal.Value"/> of 
        /// <paramref name="x"/>.
        /// </returns>
        public static decimal From(NullableDecimal x, decimal conventionalNullValue) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified <see cref="NullableDecimal"/> value to an equivalent 
        /// <see cref="System.String"/> value using a conventional <see cref="System.String"/> value to 
        /// represent the <see cref="NullableDecimal.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableDecimal"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the 
        /// <see cref="NullableDecimal.Null"/> value. 
        /// </param>
        /// <param name="format">
        /// A string that specifies the return format of <paramref name="x"/> if it is not
        /// <see cref="NullableDecimal.Null"/>. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableDecimal.Null"/> otherwise the equivalent string of the
        /// <see cref="NullableDecimal.Value"/> of <paramref name="x"/> formatted as specified by 
        /// <paramref name="format"/>.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> is not <see cref="NullableDecimal.Null"/> and the <paramref name="format"/> 
        /// is invalid for a Decimal. 
        /// </exception>
        /// <remarks>
        /// If <paramref name="format"/> is a null reference (Nothing in Visual Basic) or an empty string (""), 
        /// general format specifier ("G") will be used. 
        /// <para>
        /// The <see cref="System.Globalization.NumberFormatInfo"/> for the current culture is used when 
        /// applying fomatting.
        /// </para>
        /// </remarks>
        public static string From(NullableDecimal x, string conventionalNullValue, string format) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value.ToString(format, sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Converts the specified <see cref="NullableDateTime"/> value to an equivalent 
        /// <see cref="System.DateTime"/> value using a conventional <see cref="System.DateTime"/> value to 
        /// represent the <see cref="NullableDateTime.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableDateTime"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.DateTime"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableDateTime.Null"/> otherwise the  <see cref="NullableDateTime.Value"/> of 
        /// <paramref name="x"/>.
        /// </returns>
        public static sys.DateTime From(NullableDateTime x, sys.DateTime conventionalNullValue) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified <see cref="NullableDateTime"/> value to an equivalent 
        /// <see cref="System.String"/> value using a conventional <see cref="System.String"/> value to 
        /// represent the <see cref="NullableDateTime.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableDateTime"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the 
        /// <see cref="NullableDateTime.Null"/> value. 
        /// </param>
        /// <param name="format">
        /// A string that specifies the return format of <paramref name="x"/> if it is not
        /// <see cref="NullableDateTime.Null"/>. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableDateTime.Null"/> otherwise the equivalent string of the
        /// <see cref="NullableDateTime.Value"/> of <paramref name="x"/> formatted as specified by 
        /// <paramref name="format"/>.
        /// </returns>
        /// <exception cref="System.FormatException">
        /// The length of <paramref name="format"/> is 1, and it is not one of the format specifier characters 
        /// defined for DateTimeFormatInfo. 
        /// <para> -or- </para>
        /// <paramref name="format"/> does not contain a valid custom format pattern.
        /// </exception>
        /// <remarks>
        /// If <paramref name="format"/> is a null reference (Nothing in Visual Basic) or an empty string (""), 
        /// general format specifier ("G") will be used. 
        /// <para>
        /// The <see cref="System.Globalization.DateTimeFormatInfo"/> for the current culture is used when 
        /// applying fomatting.
        /// </para>
        /// </remarks>
        public static string From(NullableDateTime x, string conventionalNullValue, string format) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value.ToString(format, sysGlb.CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Converts the specified <see cref="NullableString"/> value to an equivalent 
        /// <see cref="System.String"/> value using a conventional <see cref="System.String"/> value to 
        /// represent the <see cref="NullableString.Null"/> value.
        /// </summary>
        /// <param name="x">
        /// The <see cref="NullableString"/> value to convert. 
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <paramref name="conventionalNullValue"/> if <paramref name="x"/> is 
        /// <see cref="NullableString.Null"/> otherwise the  <see cref="NullableString.Value"/> of 
        /// <paramref name="x"/>.
        /// </returns>
        public static string From(NullableString x, string conventionalNullValue) {
            if (x.IsNull)
                return conventionalNullValue;

            return x.Value;
        }


        /// <summary>
        /// Converts the specified <see cref="System.Boolean"/> value whose null value is simulated using a 
        /// a conventional <see cref="System.Boolean"/> value to an equivalent <see cref="NullableBoolean"/>.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Boolean"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Boolean"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableBoolean.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableBoolean"/> constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static NullableBoolean ToNullableBoolean(bool x, bool conventionalNullValue) {
            if (x == conventionalNullValue)
                return NullableBoolean.Null;

            return new NullableBoolean(x);
        }


        /// <summary>
        /// Converts the specified <see cref="System.Int32"/> value whose null value is simulated using a 
        /// a conventional <see cref="System.Int32"/> value to an equivalent <see cref="NullableBoolean"/>.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Int32"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Int32"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableBoolean.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableBoolean"/> constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static NullableBoolean ToNullableBoolean(int x, int conventionalNullValue) {
            if (x == conventionalNullValue)
                return NullableBoolean.Null;

            return new NullableBoolean(x);
        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a logical value to its 
        /// <see cref="NullableBoolean"/> equivalent.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.String"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableBoolean.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableBoolean"/> constructed 
        /// parsing <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is a null reference (Nothing in Visual Basic) and
        /// <paramref name="conventionalNullValue"/> is not a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableBoolean.Parse"/> failed on <paramref name="x"/>.
        /// </exception>
        public static NullableBoolean ToNullableBoolean(string x, string conventionalNullValue) {
            if (x == null && conventionalNullValue == null) 
                return NullableBoolean.Null;

            if (x == null) throw new sys.ArgumentNullException("x");

            if (x.CompareTo(conventionalNullValue) == 0)
                return NullableBoolean.Null;

            return NullableBoolean.Parse(x);
        }


        /// <summary>
        /// Converts the specified <see cref="System.Byte"/> value whose null value is simulated using a 
        /// a conventional <see cref="System.Byte"/> value to an equivalent <see cref="NullableByte"/>.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Byte"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Byte"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableByte.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableByte"/> constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static NullableByte ToNullableByte(byte x, byte conventionalNullValue) {
            if (x == conventionalNullValue)
                return NullableByte.Null;

            return new NullableByte(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a number to its 
        /// <see cref="NullableByte"/> equivalent.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.String"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableByte.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableByte"/> constructed 
        /// parsing <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is a null reference (Nothing in Visual Basic) and
        /// <paramref name="conventionalNullValue"/> is not a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableByte.Parse"/> failed on <paramref name="x"/>.
        /// </exception>
        public static NullableByte ToNullableByte(string x, string conventionalNullValue) {

            if (x == null && conventionalNullValue == null) 
                return NullableByte.Null;

            if (x == null) throw new sys.ArgumentNullException("x");

            if (x.CompareTo(conventionalNullValue) == 0)
                return NullableByte.Null;

            return NullableByte.Parse(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.Int16"/> value whose null value is simulated using a 
        /// a conventional <see cref="System.Int16"/> value to an equivalent <see cref="NullableInt16"/>.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Int16"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Int16"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableInt16.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableInt16"/> constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static NullableInt16 ToNullableInt16(short x, short conventionalNullValue) {
            if (x == conventionalNullValue)
                return NullableInt16.Null;

            return new NullableInt16(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a number to its 
        /// <see cref="NullableInt16"/> equivalent.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.String"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableInt16.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableInt16"/> constructed 
        /// parsing <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is a null reference (Nothing in Visual Basic) and
        /// <paramref name="conventionalNullValue"/> is not a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableInt16.Parse"/> failed on <paramref name="x"/>.
        /// </exception>
        public static NullableInt16 ToNullableInt16(string x, string conventionalNullValue) {

            if (x == null && conventionalNullValue == null) 
                return NullableInt16.Null;

            if (x == null) throw new sys.ArgumentNullException("x");

            if (x.CompareTo(conventionalNullValue) == 0)
                return NullableInt16.Null;

            return NullableInt16.Parse(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.Int32"/> value whose null value is simulated using a 
        /// a conventional <see cref="System.Int32"/> value to an equivalent <see cref="NullableInt32"/>.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Int32"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Int32"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableInt32.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableInt32"/> constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static NullableInt32 ToNullableInt32(int x, int conventionalNullValue) {
            if (x == conventionalNullValue)
                return NullableInt32.Null;

            return new NullableInt32(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a number to its 
        /// <see cref="NullableInt32"/> equivalent.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.String"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableInt32.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableInt32"/> constructed 
        /// parsing <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is a null reference (Nothing in Visual Basic) and
        /// <paramref name="conventionalNullValue"/> is not a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableInt32.Parse"/> failed on <paramref name="x"/>.
        /// </exception>
        public static NullableInt32 ToNullableInt32(string x, string conventionalNullValue) {

            if (x == null && conventionalNullValue == null) 
                return NullableInt32.Null;

            if (x == null) throw new sys.ArgumentNullException("x");

            if (x.CompareTo(conventionalNullValue) == 0)
                return NullableInt32.Null;

            return NullableInt32.Parse(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.Int64"/> value whose null value is simulated using a 
        /// a conventional <see cref="System.Int64"/> value to an equivalent <see cref="NullableInt64"/>.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Int64"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Int64"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableInt64.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableInt64"/> constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static NullableInt64 ToNullableInt64(long x, long conventionalNullValue) {
            if (x == conventionalNullValue)
                return NullableInt64.Null;

            return new NullableInt64(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a number to its 
        /// <see cref="NullableInt64"/> equivalent.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.String"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableInt64.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableInt64"/> constructed 
        /// parsing <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is a null reference (Nothing in Visual Basic) and
        /// <paramref name="conventionalNullValue"/> is not a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableInt64.Parse"/> failed on <paramref name="x"/>.
        /// </exception>
        public static NullableInt64 ToNullableInt64(string x, string conventionalNullValue) {

            if (x == null && conventionalNullValue == null) 
                return NullableInt64.Null;

            if (x == null) throw new sys.ArgumentNullException("x");

            if (x.CompareTo(conventionalNullValue) == 0)
                return NullableInt64.Null;

            return NullableInt64.Parse(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.Single"/> value whose null value is simulated using a 
        /// a conventional <see cref="System.Single"/> value to an equivalent <see cref="NullableSingle"/>.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Single"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Single"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableSingle.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableSingle"/> constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static NullableSingle ToNullableSingle(float x, float conventionalNullValue) {

            // Important: float.Equals has a different behavior from 
            // float.operator== with NaN:
            //    float.NaN.Equals(float.NaN) is true
            //    float.NaN == float.NaN is false (IEEE 754 specs)

            if (x.Equals(conventionalNullValue)) 
                return NullableSingle.Null;

            return new NullableSingle(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a number to its 
        /// <see cref="NullableSingle"/> equivalent.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.String"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableSingle.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableSingle"/> constructed 
        /// parsing <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is a null reference (Nothing in Visual Basic) and
        /// <paramref name="conventionalNullValue"/> is not a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableSingle.Parse"/> failed on <paramref name="x"/>.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableSingle.Parse"/> caused an overflow on <paramref name="x"/>.
        /// </exception>
        public static NullableSingle ToNullableSingle(string x, string conventionalNullValue) {

            if (x == null && conventionalNullValue == null) 
                return NullableSingle.Null;

            if (x == null) throw new sys.ArgumentNullException("x");

            if (x.CompareTo(conventionalNullValue) == 0)
                return NullableSingle.Null;

            return NullableSingle.Parse(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.Double"/> value whose null value is simulated using a 
        /// a conventional <see cref="System.Double"/> value to an equivalent <see cref="NullableDouble"/>.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Double"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Double"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableDouble.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableDouble"/> constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static NullableDouble ToNullableDouble(double x, double conventionalNullValue) {

            // Important: double.Equals has a different behavior from 
            // double.operator== with NaN:
            //    double.NaN.Equals(double.NaN) is true
            //    double.NaN == double.NaN is false (IEEE 754 specs)

            if (x.Equals(conventionalNullValue)) 
                return NullableDouble.Null;

            return new NullableDouble(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a number to its 
        /// <see cref="NullableDouble"/> equivalent.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.String"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableDouble.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableDouble"/> constructed 
        /// parsing <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is a null reference (Nothing in Visual Basic) and
        /// <paramref name="conventionalNullValue"/> is not a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableDouble.Parse"/> failed on <paramref name="x"/>.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableDouble.Parse"/> caused an overflow on <paramref name="x"/>.
        /// </exception>
        public static NullableDouble ToNullableDouble(string x, string conventionalNullValue) {

            if (x == null && conventionalNullValue == null) 
                return NullableDouble.Null;

            if (x == null) throw new sys.ArgumentNullException("x");

            if (x.CompareTo(conventionalNullValue) == 0)
                return NullableDouble.Null;

            return NullableDouble.Parse(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.Decimal"/> value whose null value is simulated using a 
        /// a conventional <see cref="System.Decimal"/> value to an equivalent <see cref="NullableDecimal"/>.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Decimal"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.Decimal"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableDecimal.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableDecimal"/> constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static NullableDecimal ToNullableDecimal(decimal x, decimal conventionalNullValue) {

            if (x == conventionalNullValue) 
                return NullableDecimal.Null;

            return new NullableDecimal(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a number to its 
        /// <see cref="NullableDecimal"/> equivalent.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.String"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableDecimal.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableDecimal"/> constructed 
        /// parsing <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is a null reference (Nothing in Visual Basic) and
        /// <paramref name="conventionalNullValue"/> is not a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableDecimal.Parse"/> failed on <paramref name="x"/>.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableDecimal.Parse"/> caused an overflow on <paramref name="x"/>.
        /// </exception>
        public static NullableDecimal ToNullableDecimal(string x, string conventionalNullValue) {

            if (x == null && conventionalNullValue == null) 
                return NullableDecimal.Null;

            if (x == null) throw new sys.ArgumentNullException("x");

            if (x.CompareTo(conventionalNullValue) == 0)
                return NullableDecimal.Null;

            return NullableDecimal.Parse(x);

        }



        /// <summary>
        /// Converts the specified <see cref="System.DateTime"/> value whose null value is simulated using a 
        /// a conventional <see cref="System.DateTime"/> value to an equivalent <see cref="NullableDateTime"/>.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.DateTime"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.DateTime"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableDateTime.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableDateTime"/> constructed from 
        /// <paramref name="x"/>.
        /// </returns>
        public static NullableDateTime ToNullableDateTime(sys.DateTime x, sys.DateTime conventionalNullValue) {
            if (x == conventionalNullValue)
                return NullableDateTime.Null;

            return new NullableDateTime(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> representation of a number to its 
        /// <see cref="NullableDateTime"/> equivalent.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.String"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the null value. 
        /// </param>
        /// <returns>
        /// <see cref="NullableDateTime.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> otherwise a <see cref="NullableDateTime"/> constructed 
        /// parsing <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is a null reference (Nothing in Visual Basic) and
        /// <paramref name="conventionalNullValue"/> is not a null reference.
        /// </exception>
        /// <exception cref="System.FormatException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableDateTime.Parse"/> failed on <paramref name="x"/>.
        /// </exception>
        /// <exception cref="System.OverflowException">
        /// <paramref name="x"/> value is not <paramref name="conventionalNullValue"/> and 
        /// <see cref="NullableDateTime.Parse"/> caused an overflow on <paramref name="x"/>.
        /// </exception>
        public static NullableDateTime ToNullableDateTime(string x, string conventionalNullValue) {

            if (x == null && conventionalNullValue == null) 
                return NullableDateTime.Null;

            if (x == null) throw new sys.ArgumentNullException("x");

            if (x.CompareTo(conventionalNullValue) == 0)
                return NullableDateTime.Null;

            return NullableDateTime.Parse(x);

        }


        /// <summary>
        /// Converts the specified <see cref="System.String"/> to its <see cref="NullableString"/> equivalent.
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.String"/> value to convert.
        /// </param>
        /// <param name="conventionalNullValue">
        /// The <see cref="System.String"/> value that conventionally represent the null value, can be a null
        /// reference (Nothing in Visual Basic). 
        /// </param>
        /// <returns>
        /// <see cref="NullableString.Null"/> if <paramref name="x"/> is equals to 
        /// <paramref name="conventionalNullValue"/> or both are null, otherwise a <see cref="NullableString"/> 
        /// constructed from <paramref name="x"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="x"/> is a null reference (Nothing in Visual Basic) and
        /// <paramref name="conventionalNullValue"/> is not a null reference.
        /// </exception>
        public static NullableString ToNullableString(string x, string conventionalNullValue) {
            if (x == null && conventionalNullValue == null) 
                return NullableString.Null;

            if (x == null) throw new sys.ArgumentNullException("x");

            if (x.CompareTo(conventionalNullValue) == 0)
                return NullableString.Null;

            return new NullableString(x);
        }

    }
}