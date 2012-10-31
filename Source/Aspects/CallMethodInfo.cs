using System;
using System.Collections;
using System.Reflection;

namespace BLToolkit.Aspects
{
	[System.Diagnostics.DebuggerStepThrough]
	public class CallMethodInfo
	{
		#region Init

		readonly object _sync = new object();

		#endregion

		#region Public Members

		public CallMethodInfo(MethodInfo methodInfo)
		{
			_methodInfo = methodInfo;
			_parameters = methodInfo.GetParameters();
		}

		private readonly MethodInfo _methodInfo;
		public           MethodInfo  MethodInfo
		{
			get { return _methodInfo; }
		}

		private readonly ParameterInfo[] _parameters;
		public           ParameterInfo[]  Parameters
		{
			get { return _parameters; }
		}

		volatile Hashtable  _items;
		public   IDictionary Items
		{
			get
			{
				if (_items == null) lock (_sync) if (_items == null)
					_items = Hashtable.Synchronized(new Hashtable());

				return _items;
			}
		}

		private CacheAspect _cacheAspect;
		public  CacheAspect  CacheAspect
		{
			         get { return _cacheAspect;  }
			internal set { _cacheAspect = value; }
		}

		[Obsolete("Use CacheAspect.Cache instead")]
		public  IDictionary  MethodCallCache
		{
			get { return _cacheAspect != null? _cacheAspect.Cache: new Hashtable(); }
		}

		#endregion

		#region Proptected Members

		private  bool[] _cacheableParameters;
		internal bool[]  CacheableParameters
		{
			get { return _cacheableParameters;  }
			set { _cacheableParameters = value; }
		}

		#endregion
	}
}
