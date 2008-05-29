using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

using BLToolkit.Common;

namespace BLToolkit.Aspects
{
	public delegate bool IsCacheableParameterType(Type parameterType);

	/// <summary>
	/// http://www.bltoolkit.net/Doc/Aspects/index.htm
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough]
	public class CacheAspect : Interceptor
	{
		#region Init

		public CacheAspect()
		{
			_registeredAspects.Add(this);
		}

		public override void Init(CallMethodInfo info)
		{
			info.CacheAspect = this;

			_methodInfo = info.MethodInfo;
		}

		private MethodInfo _methodInfo;

		private static IList _registeredAspects = ArrayList.Synchronized(new ArrayList());
		private static IList  RegisteredAspects
		{
			get { return _registeredAspects; }
		}

		public static CacheAspect GetAspect(MethodInfo methodInfo)
		{
			lock (RegisteredAspects.SyncRoot)
				foreach (CacheAspect aspect in RegisteredAspects)
					if (aspect._methodInfo == methodInfo)
						return aspect;

			return null;
		}

		#endregion

		#region Overrides

		protected override void BeforeCall(InterceptCallInfo info)
		{
			if (!IsEnabled)
				return;

			IDictionary cache = Cache;

			lock (cache.SyncRoot)
			{
				CompoundValue   key  = GetKey(info);
				CacheAspectItem item = GetItem(cache, key);

				if (item != null && !item.IsExpired)
				{
					info.InterceptResult = InterceptResult.Return;
					info.ReturnValue     = item.ReturnValue;

					if (item.RefValues != null)
					{
						ParameterInfo[] pis = info.CallMethodInfo.Parameters;
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

			IDictionary cache = Cache;

			lock (cache.SyncRoot)
			{
				CompoundValue key = (CompoundValue)info.Items["CacheKey"];

				if (key == null)
					return;

				int  maxCacheTime = MaxCacheTime;
				bool isWeak       = IsWeak;

				if (!string.IsNullOrEmpty(info.ConfigString))
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

				ParameterInfo[] pis = info.CallMethodInfo.Parameters;

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

		#endregion

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

		private bool _isWeak;
		public  bool  IsWeak
		{
			get { return _isWeak;  }
			set { _isWeak = value; }
		}

		#endregion

		#region IsCacheableParameterType

		private static IsCacheableParameterType _isCacheableParameterType =
			IsCacheableParameterTypeInternal;

		public  static IsCacheableParameterType  IsCacheableParameterType
		{
			get { return _isCacheableParameterType; }
			set { _isCacheableParameterType = value ?? IsCacheableParameterTypeInternal; }
		}

		private static bool IsCacheableParameterTypeInternal(Type parameterType)
		{
			return parameterType.IsValueType || parameterType == typeof(string);
		}

		#endregion

		#region Cache

		private IDictionary _cache;
		public  IDictionary  Cache
		{
			get { return _cache ?? (_cache = GetCache()); }
		}

		protected virtual CacheAspectItem CreateCacheItem(InterceptCallInfo info)
		{
			return new CacheAspectItem();
		}

		protected virtual IDictionary GetCache()
		{
			return Hashtable.Synchronized(new Hashtable());
		}

		protected static CompoundValue GetKey(InterceptCallInfo info)
		{
			ParameterInfo[] parInfo     = info.CallMethodInfo.Parameters;
			object[]        parValues   = info.ParameterValues;
			object[]        keyValues   = new object[parValues.Length];
			bool[]          cacheParams = info.CallMethodInfo.CacheableParameters;

			if (cacheParams == null)
			{
				info.CallMethodInfo.CacheableParameters = cacheParams = new bool[parInfo.Length];

				for (int i = 0; i < parInfo.Length; i++)
					cacheParams[i] = IsCacheableParameterType(parInfo[i].ParameterType);
			}

			for (int i = 0; i < parValues.Length; i++)
				keyValues[i] = cacheParams[i] ? parValues[i] : null;

			return new CompoundValue(keyValues);
		}

		protected static CacheAspectItem GetItem(IDictionary cache, CompoundValue key)
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

		/// <summary>
		/// Clear a method call cache.
		/// </summary>
		/// <param name="methodInfo">The <see cref="MethodInfo"/> representing cached method.</param>
		public static void ClearCache(MethodInfo methodInfo)
		{
			if (methodInfo == null)
				throw new ArgumentNullException("methodInfo");

			CacheAspect aspect = CacheAspect.GetAspect(methodInfo);

			if (aspect != null)
				CleanupThread.ClearCache(aspect.Cache);
		}

		/// <summary>
		/// Clear a method call cache.
		/// </summary>
		/// <param name="declaringType">The method declaring type.</param>
		/// <param name="methodName">The method name.</param>
		/// <param name="types">An array of <see cref="System.Type"/> objects representing
		/// the number, order, and type of the parameters for the method to get.-or-
		/// An empty array of the type <see cref="System.Type"/> (for example, <see cref="System.Type.EmptyTypes"/>)
		/// to get a method that takes no parameters.</param>
		public static void ClearCache(Type declaringType, string methodName, params Type[] types)
		{
			ClearCache(GetMethodInfo(declaringType, methodName, types));
		}

		public static MethodInfo GetMethodInfo(Type declaringType, string methodName, params Type[] types)
		{
			if (declaringType == null)
				throw new ArgumentNullException("declaringType");

			if (declaringType.IsAbstract)
				declaringType = BLToolkit.Reflection.TypeAccessor.GetAccessor(declaringType).Type;

			if (types == null)
				types = Type.EmptyTypes;

			MethodInfo methodInfo = declaringType.GetMethod(
				methodName,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null,
				types,
				null);

			if (methodInfo == null)
			{
				methodInfo = declaringType.GetMethod(
					methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

				if (methodInfo == null)
					throw new ArgumentException(string.Format("Method '{0}.{1}' not found.",
						declaringType.FullName, methodName));
			}

			return methodInfo;
		}

		/// <summary>
		/// Clear all cached method calls.
		/// </summary>
		public static void ClearCache()
		{
			CleanupThread.ClearCache();
		}

		#endregion

		#region Cleanup Thread

		public class CleanupThread
		{
			private CleanupThread() {}

			internal static void Init()
			{
				AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
				Start();
			}

			static void CurrentDomain_DomainUnload(object sender, EventArgs e)
			{
				Stop();
			}

			static Timer  _timer;
			static readonly object _syncTimer = new object();

			private static void Start()
			{
				if (_timer == null)
					lock (_syncTimer)
						if (_timer == null)
						{
							TimeSpan interval = TimeSpan.FromSeconds(10);
							_timer = new Timer(Cleanup, null, interval, interval);
						}
			}

			private static void Stop()
			{
				if (_timer != null)
					lock (_syncTimer)
						if (_timer != null)
						{
							_timer.Dispose();
							_timer = null;
						}
			}

			private static void Cleanup(object state)
			{
				if (!Monitor.TryEnter(CacheAspect.RegisteredAspects.SyncRoot, 10))
				{
					// The Cache is busy, skip this turn.
					//
					return;
				}

				DateTime start = DateTime.Now;
				int objectsInCache = 0;

				try
				{
					_workTimes++;

					List<DictionaryEntry> list = new List<DictionaryEntry>();

					foreach (CacheAspect aspect in CacheAspect.RegisteredAspects)
					{
						IDictionary cache = aspect.GetCache();

						lock (cache.SyncRoot)
						{
							foreach (DictionaryEntry de in cache)
							{
								WeakReference wr = de.Value as WeakReference;

								bool isExpired;

								if (wr != null)
								{
									CacheAspectItem ca = wr.Target as CacheAspectItem;

									isExpired = ca == null || ca.IsExpired;
								}
								else
								{
									isExpired = ((CacheAspectItem)de.Value).IsExpired;
								}

								if (isExpired)
									list.Add(de);
							}

							foreach (DictionaryEntry de in list)
							{
								cache.Remove(de.Key);
								_objectsExpired++;
							}

							list.Clear();

							objectsInCache += cache.Count;
						}
					}

					_objectsInCache = objectsInCache;
				}
				finally
				{
					_workTime += DateTime.Now - start;

					Monitor.Exit(CacheAspect.RegisteredAspects.SyncRoot);
				}
			}

			private static int _workTimes;
			public  static int  WorkTimes
			{
				get { return _workTimes; }
			}

			private static TimeSpan _workTime;
			public  static TimeSpan  WorkTime
			{
				get { return _workTime; }
			}

			private static int _objectsExpired;
			public  static int  ObjectsExpired
			{
				get { return _objectsExpired; }
			}

			private static int _objectsInCache;
			public  static int  ObjectsInCache
			{
				get { return _objectsInCache; }
			}

			public static void UnregisterCache(IDictionary cache)
			{
				lock (CacheAspect.RegisteredAspects.SyncRoot)
					CacheAspect.RegisteredAspects.Remove(cache);
			}

			public static void ClearCache(IDictionary cache)
			{
				lock (CacheAspect.RegisteredAspects.SyncRoot) lock (cache.SyncRoot)
				{
					_objectsExpired += cache.Count;
					cache.Clear();
				}
			}

			public static void ClearCache()
			{
				lock (CacheAspect.RegisteredAspects.SyncRoot)
				{
					foreach (CacheAspect aspect in CacheAspect.RegisteredAspects)
					{
						_objectsExpired += aspect.Cache.Count;
						aspect.Cache.Clear();
					}
				}
			}
		}

		static CacheAspect()
		{
			CleanupThread.Init();
		}

		#endregion
	}
}
