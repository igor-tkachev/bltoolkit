using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace BLToolkit.Aspects
{
	using Common;

	public delegate bool IsCacheableParameterType(Type parameterType);

	/// <summary>
	/// http://www.bltoolkit.net/Doc/Aspects/index.htm
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough]
	public class CacheAspect : Interceptor
	{
		#region Init

		static CacheAspect()
		{
			MaxCacheTime = int.MaxValue;
			IsEnabled    = true;

			CleanupThread.Init();
		}

		public CacheAspect()
		{
			_registeredAspects.Add(this);
		}

		private MethodInfo _methodInfo;
		private int?       _instanceMaxCacheTime;
		private bool?      _instanceIsWeak;

		public override void Init(CallMethodInfo info, string configString)
		{
 			base.Init(info, configString);

			info.CacheAspect = this;

			_methodInfo = info.MethodInfo;

			var ps = configString.Split(';');

			foreach (var p in ps)
			{
				var vs = p.Split('=');

				if (vs.Length == 2)
				{
					switch (vs[0].ToLower().Trim())
					{
						case "maxcachetime": _instanceMaxCacheTime = int. Parse(vs[1].Trim()); break;
						case "isweak":       _instanceIsWeak       = bool.Parse(vs[1].Trim()); break;
					}
				}
			}
		}

		private static readonly IList _registeredAspects = ArrayList.Synchronized(new ArrayList());
		protected static          IList  RegisteredAspects
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

			var cache = Cache;

			lock (cache.SyncRoot)
			{
				var key  = GetKey(info);
				var item = GetItem(cache, key);

				if (item != null && !item.IsExpired)
				{
					info.InterceptResult = InterceptResult.Return;
					info.ReturnValue     = item.ReturnValue;

					if (item.RefValues != null)
					{
						var pis = info.CallMethodInfo.Parameters;
						var n   = 0;

						for (var i = 0; i < pis.Length; i++)
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

			var cache = Cache;

			lock (cache.SyncRoot)
			{
				var key = (CompoundValue)info.Items["CacheKey"];

				if (key == null)
					return;

				var maxCacheTime = _instanceMaxCacheTime ?? MaxCacheTime;
				var isWeak       = _instanceIsWeak       ?? IsWeak;

				var item = new CacheAspectItem
				{
					ReturnValue = info.ReturnValue,
					MaxCacheTime = maxCacheTime == int.MaxValue || maxCacheTime < 0 ?
						DateTime.MaxValue :
						DateTime.Now.AddMilliseconds(maxCacheTime),
				};

				var pis = info.CallMethodInfo.Parameters;
				var n   = 0;

				foreach (var pi in pis)
					if (pi.ParameterType.IsByRef)
						n++;

				if (n > 0)
				{
					item.RefValues = new object[n];

					n = 0;

					for (var i = 0; i < pis.Length; i++)
						if (pis[i].ParameterType.IsByRef)
							item.RefValues[n++] = info.ParameterValues[i];
				}

				cache[key] = isWeak? (object)new WeakReference(item): item;
			}
		}

		#endregion

		#region Global Parameters

		public static bool IsEnabled    { get; set; }
		public static int  MaxCacheTime { get; set; }
		public static bool IsWeak       { get; set; }

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
			get { return _cache ?? (_cache = CreateCache()); }
		}

		protected virtual CacheAspectItem CreateCacheItem(InterceptCallInfo info)
		{
			return new CacheAspectItem();
		}

		protected virtual IDictionary CreateCache()
		{
			return Hashtable.Synchronized(new Hashtable());
		}

		protected static CompoundValue GetKey(InterceptCallInfo info)
		{
			var parInfo     = info.CallMethodInfo.Parameters;
			var parValues   = info.ParameterValues;
			var keyValues   = new object[parValues.Length];
			var cacheParams = info.CallMethodInfo.CacheableParameters;

			if (cacheParams == null)
			{
				info.CallMethodInfo.CacheableParameters = cacheParams = new bool[parInfo.Length];

				for (var i = 0; i < parInfo.Length; i++)
					cacheParams[i] = IsCacheableParameterType(parInfo[i].ParameterType);
			}

			for (var i = 0; i < parValues.Length; i++)
				keyValues[i] = cacheParams[i] ? parValues[i] : null;

			return new CompoundValue(keyValues);
		}

		protected static CacheAspectItem GetItem(IDictionary cache, CompoundValue key)
		{
			var obj = cache[key];

			if (obj == null)
				return null;

			var wr = obj as WeakReference;

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

			var aspect = GetAspect(methodInfo);

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

		/// <summary>
		/// Clear a method call cache.
		/// </summary>
		/// <param name="declaringType">The method declaring type.</param>
		/// <param name="methodName">The method name.</param>
		/// <param name="types">An array of <see cref="System.Type"/> objects representing
		/// the number, order, and type of the parameters for the method to get.-or-
		/// An empty array of the type <see cref="System.Type"/> (for example, <see cref="System.Type.EmptyTypes"/>)
		/// to get a method that takes no parameters.</param>
		/// <param name="values">An array of values of the parameters for the method to get</param>
		public static void ClearCache(Type declaringType, string methodName, Type[] types, object[] values)
		{
			var methodInfo = GetMethodInfo(declaringType, methodName, types);

			if (methodInfo == null)
				throw new ArgumentNullException("methodInfo");

			var aspect = GetAspect(methodInfo);

			if (aspect != null)
				CleanupThread.ClearCache(aspect.Cache, new CompoundValue(values));
		}

		public static void ClearCache(Type declaringType)
		{
			if (declaringType == null)
				throw new ArgumentNullException("declaringType");

			if (declaringType.IsAbstract)
				declaringType = TypeBuilder.TypeFactory.GetType(declaringType);

			lock (RegisteredAspects.SyncRoot)
				foreach (CacheAspect aspect in RegisteredAspects)
					if (aspect._methodInfo.DeclaringType == declaringType)
						CleanupThread.ClearCache(aspect.Cache);
		}

		public static MethodInfo GetMethodInfo(Type declaringType, string methodName, params Type[] parameterTypes)
		{
			if (declaringType == null)
				throw new ArgumentNullException("declaringType");

			if (declaringType.IsAbstract)
				declaringType = TypeBuilder.TypeFactory.GetType(declaringType);

			if (parameterTypes == null)
				parameterTypes = Type.EmptyTypes;

			var methodInfo = declaringType.GetMethod(
				methodName,
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null,
				parameterTypes,
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

			static volatile Timer  _timer;
			static readonly object _syncTimer = new object();

			private static void Start()
			{
				if (_timer == null)
					lock (_syncTimer)
						if (_timer == null)
						{
							var interval = TimeSpan.FromSeconds(10);
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
				if (!Monitor.TryEnter(RegisteredAspects.SyncRoot, 10))
				{
					// The Cache is busy, skip this turn.
					//
					return;
				}

				var start          = DateTime.Now;
				var objectsInCache = 0;

				try
				{
					_workTimes++;

					var list = new List<DictionaryEntry>();

					foreach (CacheAspect aspect in RegisteredAspects)
					{
						var cache = aspect.Cache;

						lock (cache.SyncRoot)
						{
							foreach (DictionaryEntry de in cache)
							{
								var wr = de.Value as WeakReference;

								bool isExpired;

								if (wr != null)
								{
									var ca = wr.Target as CacheAspectItem;

									isExpired = ca == null || ca.IsExpired;
								}
								else
								{
									isExpired = ((CacheAspectItem)de.Value).IsExpired;
								}

								if (isExpired)
									list.Add(de);
							}

							foreach (var de in list)
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

					Monitor.Exit(RegisteredAspects.SyncRoot);
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
				lock (RegisteredAspects.SyncRoot)
					RegisteredAspects.Remove(cache);
			}

			public static void ClearCache(IDictionary cache)
			{
				lock (RegisteredAspects.SyncRoot) lock (cache.SyncRoot)
				{
					_objectsExpired += cache.Count;
					cache.Clear();
				}
			}

			public static void ClearCache(IDictionary cache, CompoundValue key)
			{
				lock (RegisteredAspects.SyncRoot)
					lock (cache.SyncRoot)
					{
						_objectsExpired += 1;
						cache.Remove(key);
					}
			}

			public static void ClearCache()
			{
				lock (RegisteredAspects.SyncRoot)
				{
					foreach (CacheAspect aspect in RegisteredAspects)
					{
						_objectsExpired += aspect.Cache.Count;
						aspect.Cache.Clear();
					}
				}
			}
		}

		#endregion
	}
}
