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

		private IDictionary _methodCallCache;
		public  IDictionary  MethodCallCache
		{
			get
			{
				if (_methodCallCache == null) lock (this) if (_methodCallCache == null)
					CacheAspect.CleanupThread.RegisterCache(_methodCallCache = new Hashtable());

				return _methodCallCache;
			}
			set { _methodCallCache = value; }
		}

		private  bool[] _cacheableParameters;
		internal bool[]  CacheableParameters
		{
			get { return _cacheableParameters;  }
			set { _cacheableParameters = value; }
		}

		#endregion
	}
}
