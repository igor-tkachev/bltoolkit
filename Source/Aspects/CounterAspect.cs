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
		public override void Init(CallMethodInfo info, string configString)
		{
			base.Init(info, configString);

			_counters.Add(_counter = CreateCounter(info));
		}

		private MethodCallCounter _counter;

		static readonly LocalDataStoreSlot counterSlot = Thread.AllocateDataSlot();

		protected override void BeforeCall(InterceptCallInfo info)
		{
			if (!IsEnabled || Thread.GetData(counterSlot) != null)
				return;

			lock (_counter.CurrentCalls.SyncRoot)
				_counter.CurrentCalls.Add(info);

			Thread.SetData(counterSlot, _counter);
		}

		protected override void OnFinally(InterceptCallInfo info)
		{
			if (!IsEnabled)
				return;

			MethodCallCounter prev = (MethodCallCounter)Thread.GetData(counterSlot);

			if (_counter == prev)
			{
				_counter.AddCall(DateTime.Now - info.BeginCallTime, info.Exception != null, info.Cached);

				lock (_counter.CurrentCalls.SyncRoot)
					_counter.CurrentCalls.Remove(info);

				Thread.SetData(counterSlot, null);
			}
		}

		#region Parameters

		private static bool _isEnabled = true;
		public  static bool  IsEnabled
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
			lock (_counters.SyncRoot)
			foreach (MethodCallCounter c in _counters)
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

		#endregion
	}
}
