using System;

using BLToolkit.ServiceModel;

namespace Client.Web
{
	public class TestLinqService : LinqService
	{
		protected override void ValidateQuery(LinqServiceQuery query)
		{
			base.ValidateQuery(query);
		}
	}
}
