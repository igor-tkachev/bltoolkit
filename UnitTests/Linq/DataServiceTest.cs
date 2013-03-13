using System;
using System.Data.Services.Providers;

using BLToolkit.ServiceModel;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class DataServiceTest
	{
		[Test]
		public void Test1()
		{
			var ds = new DataService<NorthwindDB>();
			var mp = ds.GetService(typeof(IDataServiceMetadataProvider));
		}
	}
}
