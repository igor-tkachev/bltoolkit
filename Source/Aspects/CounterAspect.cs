using System;
using System.Collections;
using System.Reflection;
using System.Threading;

namespace BLToolkit.Aspects
{
	public delegate MethodCallCounter CreateCounter(CallMethodInfo methodInfo);

	/// <summary>
	/// http://www.bltoolkit.net/Doc/Aspects/index.htm
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough]
	public class CounterAspect : Interceptor
	{
		static readonly LocalDataStoreSlot counterSlot = Thread.AllocateDataSlot();

		protected override void BeforeCall(InterceptCallInfo info)
		{
			if (!IsEnabled || Thread.GetData(counterSlot) != null)
				return;

			MethodCallCounter counter = GetCounter(info);

			lock (counter.CurrentCalls.SyncRoot)
				counter.CurrentCalls.Add(info);

			Thread.SetData(counterSlot, counter);
		}

		protected override void OnFinally(InterceptCallInfo info)
		{
			if (!IsEnabled)
				return;

			MethodCallCounter counter = GetCounter(info);
			MethodCallCounter prev    = (MethodCallCounter)Thread.GetData(counterSlot);

			if (counter == prev)
			{
				counter.AddCall(DateTime.Now - info.BeginCallTime, info.Exception != null, info.Cached);

				lock (counter.CurrentCalls.SyncRoot)
					counter.CurrentCalls.Remove(info);

				Thread.SetData(counterSlot, null);
			}
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

		private static readonly ArrayList _counters = ArrayList.Synchronized(new ArrayList());
		public  static          ArrayList  Counters
		{
			get { return _counters; }
		}

		public static MethodCallCounter GetCounter(MethodInfo methodInfo)
		{
			lock (_counters.SyncRoot) foreach (MethodCallCounter c in _counters)
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

		#region CreateCounter

		private static CreateCounter _createCounter = CreateCounterInternal;

		public static CreateCounter CreateCounter
		{
			get { return _createCounter; }
			set { _createCounter = value ?? new CreateCounter(CreateCounterInternal); }
		}

		private static MethodCallCounter CreateCounterInternal(CallMethodInfo methodInfo)
		{
			return new MethodCallCounter(methodInfo);
		}

		#endregion

		private static MethodCallCounter GetCounter(InterceptCallInfo info)
		{
			if (info.CallMethodInfo.Counter == null)
				lock (_counters.SyncRoot)
					if (info.CallMethodInfo.Counter == null)
					{
						foreach (MethodCallCounter c in _counters)
							if (c.InterceptorID == info.InterceptorID)
								return info.CallMethodInfo.Counter = c;

						_counters.Add(info.CallMethodInfo.Counter = CreateCounter(info.CallMethodInfo));

						info.CallMethodInfo.Counter.InterceptorID = info.InterceptorID;
					}

			return info.CallMethodInfo.Counter;
		}

		#endregion
	}
}
