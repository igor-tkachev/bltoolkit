using System.Globalization;

namespace Data.Linq
{
	public static class Helper
	{
		public static string InvariantDecimal(this string value)
		{
			return value.Replace(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator, 
				CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
		}
	}
}