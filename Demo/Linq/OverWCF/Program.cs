using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Linq.OverWCF
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var host = new ServiceHost(new DemoService(), new Uri("net.tcp://localhost:1234")))
			{
				host.Description.Behaviors.Add(new ServiceMetadataBehavior());
				host.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;
				host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
				host.AddServiceEndpoint(typeof(IDemoService), new NetTcpBinding(SecurityMode.None), "LinqOverWCF");

				host.Open();

				using (var client = new Client())
				{
					foreach (var ch in client.DemoMethod("12345"))
						Console.WriteLine(ch);
				}

				Console.ReadLine();

				host.Close();
			}
		}
	}
}
