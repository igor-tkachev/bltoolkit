using System;
using System.Collections;
using System.Reflection;

using BLToolkit.Common;
using BLToolkit.Reflection;

namespace BLToolkit.Aspects
{
	public class CacheAspect : Interceptor
	{
		protected override void BeforeCall(InterceptCallInfo info)
		{
			CompoundValue key  = GetKey(info);
			CacheItem     item = GetItem(info.MethodInfo, key);

			if (item != null && item.MaxCacheTime > DateTime.Now)
			{
				info.InterceptResult = InterceptResult.Return;
				info.ReturnValue     = item.ReturnValue;

				if (item.RefValues != null)
				{
					ParameterInfo[] pis = info.MethodInfo.GetParameters();
					int             n   = 0;

					for (int i = 0; i < pis.Length; i++)
						if (pis[i].ParameterType.IsByRef)
							info.ParameterValues[i] = item.RefValues[n++];
				}
			}
			else
			{
				info.Items["CacheKey"] = key;
			}
		}

		protected override void AfterCall(InterceptCallInfo info)
		{
			CompoundValue key = (CompoundValue)info.Items["CacheKey"];

			if (key == null)
				return;

			int maxCacheTime = MaxCacheTime;

			if (info.ConfigString != null && info.ConfigString.Length > 0)
			{
				ConfigParameters cp = GetConfigParameters(info.ConfigString);

				if (cp.MaxCacheTime != null) maxCacheTime = (int)cp.MaxCacheTime;
			}

			CacheItem item = new CacheItem();

			item.ReturnValue  = info.ReturnValue;
			item.MaxCacheTime = maxCacheTime == int.MaxValue || maxCacheTime < 0?
				DateTime.MaxValue:
				DateTime.Now.AddMilliseconds(maxCacheTime);

			ParameterInfo[] pis = info.MethodInfo.GetParameters();

			int n = 0;

			foreach (ParameterInfo pi in pis)
				if (pi.ParameterType.IsByRef)
					n++;

			if (n > 0)
			{
				item.RefValues = new object[n];

				n = 0;

				for (int i = 0; i < pis.Length; i++)
					if (pis[i].ParameterType.IsByRef)
						item.RefValues[n++] = info.ParameterValues[i];
			}

			AddItem(info.MethodInfo, key, item);
		}

		#region Config Support

		class ConfigParameters
		{
			public object MaxCacheTime;
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

		#region Cache

		private CompoundValue GetKey(InterceptCallInfo info)
		{
			ParameterInfo[] parInfo   = info.MethodInfo.GetParameters();
			object[]        parValues = info.ParameterValues;
			object[]        keyValues = new object[parValues.Length];

			for (int i = 0; i < parValues.Length; i++)
				keyValues[i] = TypeHelper.IsScalar(parInfo[i].ParameterType) ? parValues[i] : null;

			return new CompoundValue(keyValues);
		}

		private static Hashtable _methodCache = Hashtable.Synchronized(new Hashtable());

		private static CacheItem GetItem(MethodInfo methodInfo, CompoundValue key)
		{
			Hashtable keys = (Hashtable)_methodCache[methodInfo];

			if (keys == null)
				return null;

			return (CacheItem)keys[key];
		}

		private static void AddItem(MethodInfo methodInfo, CompoundValue key, CacheItem item)
		{
			Hashtable keys = (Hashtable)_methodCache[methodInfo];

			if (keys == null)
				_methodCache[methodInfo] = keys = new Hashtable();

			keys[key] = item;
		}

		class CacheItem
		{
			public DateTime MaxCacheTime;
			public object   ReturnValue;
			public object[] RefValues;
		}

		#endregion
	}
}
