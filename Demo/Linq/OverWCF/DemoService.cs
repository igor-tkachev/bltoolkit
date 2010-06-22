using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Linq.OverWCF
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class DemoService : IDemoService
	{
		public IEnumerable<string> DemoMethod(string str)
		{
			foreach (var ch in str)
			{
				yield return ch.ToString();
			}
		}
	}
}
