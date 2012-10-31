using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Linq.OverWCF
{
	class Client : ClientBase<IDemoService>, IDemoService
	{
		public Client() : base(
			new NetTcpBinding(SecurityMode.None)
			{
				MaxReceivedMessageSize = 10000000,
				MaxBufferPoolSize      = 10000000,
				MaxBufferSize          = 10000000,
			},
			new EndpointAddress("net.tcp://localhost:1234/LinqOverWCF"))
		{
		}

		public IEnumerable<string> DemoMethod(string str)
		{
			return Channel.DemoMethod(str);
		}
	}
}
