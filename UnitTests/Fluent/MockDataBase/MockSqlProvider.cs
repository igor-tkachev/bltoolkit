using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Sql.SqlProvider;

namespace BLToolkit.Fluent.Test.MockDataBase
{
	/// <summary>
	/// For BLToolkit
	/// </summary>
	public class MockSqlProvider : BasicSqlProvider
	{
		public const string FieldMarker = "{D02ADC4A-7838-4FA8-8AD7-1DCE93C8098E}";
		public const string TableMarker = "{A96D8597-A829-496C-AA71-0ED995E184CE}";

		protected override ISqlProvider CreateSqlProvider()
		{
			return new MockSqlProvider();
		}

		public override object Convert(object value, ConvertType convertType)
		{
			if (ConvertType.NameToQueryField == convertType)
			{
				return FieldMarker + value;
			}
			if (ConvertType.NameToQueryTable == convertType)
			{
				return TableMarker + value;
			}
			return base.Convert(value, convertType);
		}
	}
}