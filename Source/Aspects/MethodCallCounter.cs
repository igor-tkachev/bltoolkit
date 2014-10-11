using System;
using System.Collections;
using System.Reflection;

namespace BLToolkit.Aspects
{
	[System.Diagnostics.DebuggerStepThrough]
	public class MethodCallCounter
	{
		public MethodCallCounter(CallMethodInfo methodInfo)
		{
			_callMethodInfo = methodInfo;
			_methodInfo     = methodInfo.MethodInfo;
		}

		#region Public Members

		private MethodInfo _methodInfo;
		public  MethodInfo  MethodInfo
		{
			get { return _methodInfo;  }
			set { _methodInfo = value; }
		}

		private CallMethodInfo _callMethodInfo;
		public  CallMethodInfo  CallMethodInfo
		{
			get { return _callMethodInfo;  }
			set { _callMethodInfo = value; }
		}

		private int _totalCount;
		public  int  TotalCount
		{
			get { return _totalCount;  }
			set { _totalCount = value; }
		}

		private int _exceptionCount;
		public  int  ExceptionCount
		{
			get { return _exceptionCount;  }
			set { _exceptionCount = value; }
		}

		private int _cachedCount;
		public  int  CachedCount
		{
			get { return _cachedCount;  }
			set { _cachedCount = value; }
		}

		private TimeSpan _totalTime;
		public  TimeSpan  TotalTime
		{
			get { return _totalTime;  }
			set { _totalTime = value; }
		}

		private TimeSpan _minTime = TimeSpan.MaxValue;
		public  TimeSpan  MinTime
		{
			get { return _minTime;  }
			set { _minTime = value; }
		}

		private TimeSpan _maxTime;
		public  TimeSpan  MaxTime
		{
			get { return _maxTime;  }
			set { _maxTime = value; }
		}

		private readonly ArrayList _currentCalls = ArrayList.Synchronized(new ArrayList());
		public           ArrayList  CurrentCalls
		{
			get { return _currentCalls; }
		}

		public TimeSpan AverageTime
		{
			get
			{
				if (_totalCount == 0)
					return TimeSpan.MinValue;

				return new TimeSpan(TotalTime.Ticks / TotalCount);
			}
		}

		#endregion

		#region Protected Members

		public virtual void RegisterCall(InterceptCallInfo info)
		{
			lock (_currentCalls.SyncRoot)
				_currentCalls.Add(info);
		}

		public virtual void UnregisterCall(InterceptCallInfo info)
		{
			AddCall(DateTime.Now - info.BeginCallTime, info.Exception != null, info.Cached);

			lock (_currentCalls.SyncRoot)
				_currentCalls.Remove(info);
		}

		protected void AddCall(TimeSpan time, bool withException, bool cached)
		{
			if (cached)
			{
				_cachedCount++;
			}
			else
			{
				_totalTime += time;
				_totalCount++;

				if (_minTime > time) _minTime = time;
				if (_maxTime < time) _maxTime = time;
			}

			if (withException) _exceptionCount++;
		}

		#endregion
	}
}
