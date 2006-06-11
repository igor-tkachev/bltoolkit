using System;
using System.Collections;

namespace BLToolkit.Aspects
{
	public class CacheAspect : Interceptor
	{
		protected override void BeforeCall(InterceptCallInfo info)
		{
		}

		protected override void AfterCall(InterceptCallInfo info)
		{
		}

		#region Config Support

		class ConfigParameters
		{
			public int MaxCacheTime;
		}

		private static Hashtable _configParameters = new Hashtable();

		private static ConfigParameters GetConfigParameters(string configString)
		{
			ConfigParameters cp = (ConfigParameters)_configParameters[configString];

			if (cp == null)
			{
				cp = new ConfigParameters();

				string[] ps = configString.Split(';');

				foreach (string p in ps)
				{
					string[] vs = p.Split('=');

					if (vs.Length == 2)
					{
						switch (vs[0].ToLower().Trim())
						{
							case "maxcachetime": cp.MaxCacheTime = int.Parse(vs[1].Trim()); break;
						}
					}
				}
			}

			return cp;
		}

		#endregion

		#region Parameters

		private int _maxCacheTime = int.MaxValue;
		public  int  MaxCacheTime
		{
			get { return _maxCacheTime;  }
			set { _maxCacheTime = value; }
		}

		#endregion

		#region CacheItem

		class CacheItem
		{
			
		}

		#endregion
	}
}
