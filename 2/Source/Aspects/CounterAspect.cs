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

			Counter counter = GetCounterInternal(info.MethodInfo);

			counter.CurrentCalls.Add(info);

			info.Items["CurrentPrincipal"] = Thread.CurrentPrincipal;
		}

		protected override void OnFinally(InterceptCallInfo info)
		{
			if (!IsEnabled)
				return;

			Counter counter = GetCounterInternal(info.MethodInfo);

			counter.TotalTime += DateTime.Now - info.BeginCallTime;
			counter.TotalCount++;
			counter.CurrentCalls.Remove(info);
		}

		#region Config Support

		/*
		class ConfigParameters
		{
			public object MinCallTime;
		}

		private static Hashtable _configParameters = new Hashtable();

		private static ConfigParameters GetConfigParameters(string configString)
		{
			ConfigParameters cp = (ConfigParameters)_configParameters[configString];

			if (cp == null)
			{
				cp = new ConfigParameters();

				string[] ps = configString.Split(';');

				foreach (string p in ps)
				{
					string[] vs = p.Split('=');

					if (vs.Length == 2)
					{
						switch (vs[0].ToLower().Trim())
						{
							case "mincalltime": cp.MinCallTime = int.Parse(vs[1].Trim()); break;
						}
					}
				}
			}

			return cp;
		}
		*/

		#endregion

		#region Parameters

		private bool _isEnabled = true;
		public  bool  IsEnabled
		{
			get { return _isEnabled;  }
			set { _isEnabled = value; }
		}

		/*
		private static int _minCallTime;
		public  static int  MinCallTime
		{
			get { return _minCallTime;  }
			set { _minCallTime = value; }
		}
		*/

		#endregion

		#region Counter

		public class Counter
		{
			public MethodInfo MethodInfo;
			public TimeSpan   TotalTime;
			public int        TotalCount;
			public ArrayList  CurrentCalls = ArrayList.Synchronized(new ArrayList());
		}

		private static Hashtable _counters = Hashtable.Synchronized(new Hashtable());
		public  static Hashtable  Counters
		{
			get { return _counters; }
		}

		public static Counter GetCounter(MethodInfo methodInfo)
		{
			foreach (Counter c in _counters.Values)
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

		private static Counter GetCounterInternal(MethodInfo methodInfo)
		{
			Counter counter = (Counter)_counters[methodInfo];

			if (counter == null)
			{
				_counters[methodInfo] = counter = new Counter();

				counter.MethodInfo = methodInfo;
			}

			return counter;
		}

		#endregion
	}
}
