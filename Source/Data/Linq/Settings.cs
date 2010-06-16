using System;

namespace BLToolkit.Data.Linq
{
	public static class Settings
	{
		public static Func<ILinqDataProvider> GetDefaultDataProvider = () => DbManager.GetDataProvider(DbManager.DefaultConfiguration);
	}
}
