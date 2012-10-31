using System;

namespace BLToolkit.Data.Linq
{
	public static class Settings
	{
#if SILVERLIGHT
		public static Func<string,IDataContext> CreateDefaultDataContext = config => { throw new InvalidOperationException(); };
#else
		public static Func<string,IDataContext> CreateDefaultDataContext = config => new DbManager(config ?? DbManager.DefaultConfiguration);
#endif
	}
}
