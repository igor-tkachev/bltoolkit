using System;

namespace BLToolkit.Data.Linq
{
	public static class Settings
	{
		public static Func<string,IDataContext> CreateDefaultDataContext = config => new DbManager(config ?? DbManager.DefaultConfiguration);
	}
}
