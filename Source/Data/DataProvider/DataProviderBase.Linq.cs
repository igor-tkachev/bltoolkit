using System;

namespace BLToolkit.Data.DataProvider
{
	using Linq;

	public partial class DataProviderBase : ILinqDataProvider
	{
		IDataContext ILinqDataProvider.CreateDataContext()
		{
			return new DbManager();
		}
	}
}
