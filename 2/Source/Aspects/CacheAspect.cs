using System;
using System.Collections;
using System.Reflection;

using BLToolkit.Common;
using BLToolkit.Reflection;

namespace BLToolkit.Aspects
{
	[System.Diagnostics.DebuggerStepThrough]
	public class CacheAspect : Interceptor
	{
		protected override void BeforeCall(InterceptCallInfo info)
		{
			if (!IsEnabled)
				return;

			IDictionary cache = GetCache(info);

			lock (cache.SyncRoot)
			{
				CompoundValue   key  = GetKey(info);
				CacheAspectItem item = GetItem(cache, key, info);

				if (item != null && !item.IsExpired)
				{
					info.InterceptResult = InterceptResult.Return;
					info.ReturnValue     = item.ReturnValue;

					if (item.RefValues != null)
					{
						ParameterInfo[] pis = info.CallMethodInfo.MethodInfo.GetParameters();
						int             n   = 0;

						for (int i = 0; i < pis.Length; i++)
							if (pis[i].ParameterType.IsByRef)
								info.ParameterValues[i] = item.RefValues[n++];
					}

					info.Cached = true;
				}
				else 
				{
					info.Items["CacheKey"] = key;
				}
			}
		}

		protected override void AfterCall(InterceptCallInfo info)
		{
			if (!IsEnabled)
				return;

			IDictionary cache = GetCache(info);

			lock (cache.SyncRoot)
			{
				CompoundValue key = (CompoundValue)info.Items["CacheKey"];

				if (key == null)
					return;

				int  maxCacheTime = MaxCacheTime;
				bool isWeak       = IsWeak;

				if (info.ConfigString != null && info.ConfigString.Length > 0)
				{
					ConfigParameters cp = GetConfigParameters(info);

					if (cp.MaxCacheTime != null) maxCacheTime = (int) cp.MaxCacheTime;
					if (cp.IsWeak       != null) isWeak       = (bool)cp.IsWeak;
				}

				CacheAspectItem item = new CacheAspectItem();

				item.ReturnValue  = info.ReturnValue;
				item.MaxCacheTime = maxCacheTime == int.MaxValue || maxCacheTime < 0?
					DateTime.MaxValue:
					DateTime.Now.AddMilliseconds(maxCacheTime);

				ParameterInfo[] pis = info.CallMethodInfo.MethodInfo.GetParameters();

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

				cache[key] = isWeak? (object)new WeakReference(item): item;
			}
		}

		#region Config Support

		internal class ConfigParameters
		{
			public object MaxCacheTime;
			public object IsWeak;
		}

		private static ConfigParameters GetConfigParameters(InterceptCallInfo info)
		{
			ConfigParameters cp = info.CallMethodInfo.CacheParameters;

			if (cp == null)
			{
				info.CallMethodInfo.CacheParameters = cp = new ConfigParameters();

				string[] ps = info.ConfigString.Split(';');

				foreach (string p in ps)
				{
					string[] vs = p.Split('=');

					if (vs.Length == 2)
					{
						switch (vs[0].ToLower().Trim())
						{
							case "maxcachetime": cp.MaxCacheTime = int. Parse(vs[1].Trim()); break;
							case "isweak":       cp.IsWeak       = bool.Parse(vs[1].Trim()); break;
						}
					}
				}
			}

			return cp;
		}

		#endregion

		#region Parameters

		private bool _isEnabled = true;
		public  bool  IsEnabled
		{
			get { return _isEnabled;  }
			set { _isEnabled = value; }
		}

		private int _maxCacheTime = int.MaxValue;
		public  int  MaxCacheTime
		{
			get { return _maxCacheTime;  }
			set { _maxCacheTime = value; }
		}

		private bool _isWeak = true;
		public  bool  IsWeak
		{
			get { return _isWeak;  }
			set { _isWeak = value; }
		}

		#endregion

		#region Cache

		protected virtual CacheAspectItem CreateCacheItem(InterceptCallInfo info)
		{
			return new CacheAspectItem();
		}

		protected virtual IDictionary GetCache(InterceptCallInfo info)
		{
			return info.CallMethodInfo.MethodCallCache;
		}

		private static CompoundValue GetKey(InterceptCallInfo info)
		{
			ParameterInfo[] parInfo   = info.CallMethodInfo.MethodInfo.GetParameters();
			object[]        parValues = info.ParameterValues;
			object[]        keyValues = new object[parValues.Length];

			for (int i = 0; i < parValues.Length; i++)
				keyValues[i] = TypeHelper.IsScalar(parInfo[i].ParameterType) ? parValues[i] : null;

			return new CompoundValue(keyValues);
		}

		private CacheAspectItem GetItem(IDictionary cache, CompoundValue key, InterceptCallInfo info)
		{
			object obj = cache[key];

			if (obj == null)
				return null;

			WeakReference wr = obj as WeakReference;

			if (wr == null)
				return (CacheAspectItem)obj;

			obj = wr.Target;

			if (obj != null)
				return (CacheAspectItem)obj;

			cache.Remove(key);

			return null;
		}

		#endregion
	}
}
