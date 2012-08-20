using System;
using System.Web.Services;

using BLToolkit.ServiceModel;

namespace Client.Web
{
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class LinqWebService : LinqService
	{
		public override BLToolkit.Data.Linq.IDataContext CreateDataContext()
		{
			return new DataModel();
		}

		protected override void ValidateQuery(LinqServiceQuery query)
		{
			//base.ValidateQuery(query);
		}
	}
}
