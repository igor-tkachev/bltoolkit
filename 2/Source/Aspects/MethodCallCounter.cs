using System;
using System.Collections;
using System.Reflection;

namespace BLToolkit.Aspects
{
	public class MethodCallCounter
	{
		public MethodCallCounter(MethodInfo methodInfo)
		{
			_methodInfo = methodInfo;
		}

		#region Public Members

		private MethodInfo _methodInfo;
		public  MethodInfo  MethodInfo
		{
			get { return _methodInfo; }
		}

		private int _totalCount;
		public  int  TotalCount
		{
			get { return _totalCount; }
		}

		private int _exceptionCount;
		public  int  ExceptionCount
		{
			get { return _exceptionCount; }
		}

		private TimeSpan _totalTime;
		public  TimeSpan  TotalTime
		{
			get { return _totalTime; }
		}

		private TimeSpan _minTime = TimeSpan.MaxValue;
		public  TimeSpan  MinTime
		{
			get { return _minTime; }
		}

		private TimeSpan _maxTime;
		public  TimeSpan  MaxTime
		{
			get { return _maxTime; }
		}

		private ArrayList _currentCalls = ArrayList.Synchronized(new ArrayList());
		public  ArrayList  CurrentCalls
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

		internal void AddCall(TimeSpan time, bool withException)
		{
			_totalTime += time;
			_totalCount++;

			if (_minTime > time) _minTime = time;
			if (_maxTime < time) _maxTime = time;

			if (withException) _exceptionCount++;
		}

		#endregion
	}
}
