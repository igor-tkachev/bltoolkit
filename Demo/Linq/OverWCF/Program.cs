using System;
using System.ServiceModel;
using System.ServiceModel.Description;

using BLToolkit.Data.Linq;
using BLToolkit.ServiceModel;

namespace Linq.OverWCF
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var host = new ServiceHost(new LinqService("Sql2008"), new Uri("net.tcp://localhost:1234")))
			{
				host.Description.Behaviors.Add(new ServiceMetadataBehavior());
				host.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;
				host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
				host.AddServiceEndpoint(
					typeof(ILinqService),
					new NetTcpBinding(SecurityMode.None)
					{
						MaxReceivedMessageSize = 10000000,
						MaxBufferPoolSize      = 10000000,
						MaxBufferSize          = 10000000,
						CloseTimeout           = new TimeSpan(00, 01, 00),
						OpenTimeout            = new TimeSpan(00, 01, 00),
						ReceiveTimeout         = new TimeSpan(00, 10, 00),
						SendTimeout            = new TimeSpan(00, 10, 00),
					},
					"LinqOverWCF");

				host.Open();

				var client = new DataModel();

				client.Person.Insert(() => new Person
				{
					FirstName = "1",
					LastName  = "2",
					Gender    = 'F',
				});

				Console.ReadLine();

				host.Close();
			}
		}
	}
}
