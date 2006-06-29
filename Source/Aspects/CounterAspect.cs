using System;
using System.Collections;
using System.Reflection;
using System.Threading;

namespace BLToolkit.Aspects
{
	public class CounterAspect : Interceptor
	{
		protected override void BeforeCall(InterceptCallInfo info)
		{
			if (!IsEnabled)
				return;

			Counter counter = GetCounter(info.CallMethodInfo);

			lock (counter.CurrentCalls.SyncRoot)
				counter.CurrentCalls.Add(info);

			info.Items["CurrentPrincipal"] = Thread.CurrentPrincipal;
		}

		protected override void OnFinally(InterceptCallInfo info)
		{
			if (!IsEnabled)
				return;

			Counter counter = GetCounter(info.CallMethodInfo);

			counter.TotalTime += DateTime.Now - info.BeginCallTime;
			counter.TotalCount++;

			lock (counter.CurrentCalls.SyncRoot)
				counter.CurrentCalls.Remove(info);
		}

		#region Parameters

		private bool _isEnabled = true;
		public  bool  IsEnabled
		{
			get { return _isEnabled;  }
			set { _isEnabled = value; }
		}

		#endregion

		#region Counter

		public class Counter
		{
			public Counter(MethodInfo methodInfo)
			{
				MethodInfo = methodInfo;
			}

			public MethodInfo MethodInfo;
			public TimeSpan   TotalTime;
			public int        TotalCount;
			public ArrayList  CurrentCalls = ArrayList.Synchronized(new ArrayList());
		}

		private static ArrayList _counters = ArrayList.Synchronized(new ArrayList());
		public  static ArrayList  Counters
		{
			get { return _counters; }
		}

		public static Counter GetCounter(MethodInfo methodInfo)
		{
			lock (_counters.SyncRoot) foreach (Counter c in _counters)
			{
				if ((methodInfo.DeclaringType == c.MethodInfo.DeclaringType ||
					 methodInfo.DeclaringType == c.MethodInfo.DeclaringType.BaseType) &&
					 methodInfo.Name          == c.MethodInfo.Name)
				{
					ParameterInfo[] ps1 = c.MethodInfo.GetParameters();
					ParameterInfo[] ps2 =   methodInfo.GetParameters();

					if (ps1.Length == ps2.Length)
					{
						bool isMatched = true;

						for (int i = 0; isMatched && i < ps1.Length; i++)
							isMatched = ps1[i].ParameterType == ps2[i].ParameterType;

						if (isMatched)
							return c;
					}
				}
			}

			return null;
		}

		private static Counter GetCounter(CallMethodInfo methodInfo)
		{
			if (methodInfo.Counter == null)
				lock (_counters.SyncRoot)
					if (methodInfo.Counter == null)
						_counters.Add(methodInfo.Counter = new Counter(methodInfo.MethodInfo));

			return methodInfo.Counter;
		}

		#endregion
	}
}
