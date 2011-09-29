using System;
using System.Data.Services;
using System.Data.Services.Common;

namespace Client.Web
{
	public class WcfDataService : BLToolkit.ServiceModel.DataService<DataModel>
	{
		// This method is called only once to initialize service-wide policies.
		public static void InitializeService(DataServiceConfiguration config)
		{
			// TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
			// Examples:
			// config.SetEntitySetAccessRule("MyEntityset", EntitySetRights.AllRead);
			// config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
			config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
		}
	}
}
