using System;
using System.Collections;
using System.Reflection;

namespace BLToolkit.Aspects
{
	[System.Diagnostics.DebuggerStepThrough]
	public class CallMethodInfo
	{
		#region Public Members

		public CallMethodInfo(MethodInfo methodInfo)
		{
			_methodInfo = methodInfo;
		}

		private readonly MethodInfo _methodInfo;
		public           MethodInfo  MethodInfo
		{
			get { return _methodInfo; }
		}

		private Hashtable  _items;
		public  IDictionary Items
		{
			get
			{
				if (_items == null) lock (this) if (_items == null)
					_items = Hashtable.Synchronized(new Hashtable());

				return _items;
			}
		}

		#endregion

		#region Proptected Members

		internal CacheAspect.  ConfigParameters CacheParameters;
		internal LoggingAspect.ConfigParameters LogParameters;
		internal MethodCallCounter              Counter;

		private  Hashtable _methodCallCache;
		internal Hashtable  MethodCallCache
		{
			get
			{
				if (_methodCallCache == null) lock (this) if (_methodCallCache == null)
					_methodCallCache = new Hashtable();

				return _methodCallCache;
			}
		}

		#endregion
	}
}
