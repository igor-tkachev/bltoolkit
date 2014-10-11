using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Linq.OverWCF
{
	[ServiceContract]
	public interface IDemoService
	{
		[OperationContract]
		IEnumerable<string> DemoMethod(string str);
	}
}
