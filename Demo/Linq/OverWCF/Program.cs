﻿using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

using BLToolkit.Data;
using BLToolkit.ServiceModel;

namespace Linq.OverWCF
{
	class Program
	{
		static void Main()
		{
			DbManager.TurnTraceSwitchOn();

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

				var q =
					from p in client.Person
					select new
					{
						p.PersonID,
						p.FirstName,
						p.MiddleName,
						p.LastName,
						p.Gender
					};

				foreach (var p in q)
					Console.WriteLine(p);

				Console.ReadLine();

				host.Close();
			}
		}
	}
}
