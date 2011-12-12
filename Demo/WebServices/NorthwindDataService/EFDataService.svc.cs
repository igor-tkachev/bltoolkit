using System;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Services;
using System.Data.Services.Common;

namespace NorthwindDataService
{
	public class EFDataService : DataService<NorthwindEntities>
	{
		// This method is called only once to initialize service-wide policies.
		public static void InitializeService(DataServiceConfiguration config)
		{
			var w = new MetadataWorkspace();
			var m = new ObjectStateManager(w);

			// TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
			// Examples:
			config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
			// config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
			config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
		}
	}
}
