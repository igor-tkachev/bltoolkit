using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace HowTo.Mapping
{
	[TestFixture]
	public class MapField
	{
		public class SourceObject1
		{
			public string Street = "1 Main";
			public string City   = "Bigtown";
			public string State  = "XX";
			public string Zip    = "00000";
		}

		public class Address
		{
			public string Street;
			public string City;
			public string State;
			public string Zip;
		}

		/*[a]*/[MapField("Street", "Address.Street")]/*[/a]*/
		/*[a]*/[MapField("City",   "Address.City")]/*[/a]*/
		/*[a]*/[MapField("State",  "Address.State")]/*[/a]*/
		/*[a]*/[MapField("Zip",    "Address.Zip")]/*[/a]*/
		public class Order1
		{
			public Address Address = new Address();
		}

		[Test]
		public void MapFieldTest1()
		{
			SourceObject1 source = new SourceObject1();
			Order1        order  = Map.ObjectToObject<Order1>(source);

			Assert.AreEqual("1 Main",  order.Address.Street);
			Assert.AreEqual("Bigtown", order.Address.City);
			Assert.AreEqual("XX",      order.Address.State);
			Assert.AreEqual("00000",   order.Address.Zip);
		}

		public class SourceObject2
		{
			public string BillingStreet = "1 Main";
			public string BillingCity   = "Bigtown";
			public string BillingState  = "XX";
			public string BillingZip    = "00000";

			public string ShippingStreet = "2 Main";
			public string ShippingCity   = "Bigtown";
			public string ShippingState  = "XX";
			public string ShippingZip    = "00000";
		}

		public class Order2
		{
			/*[a]*/[MapField(Format="Billing{0}")]/*[/a]*/
			public Address BillingAddress = new Address();

			/*[a]*/[MapField(Format="Shipping{0}")]/*[/a]*/
			public Address ShippingAddress = new Address();
		}

		[Test]
		public void MapFieldTest2()
		{
			SourceObject2 source = new SourceObject2();
			Order2        order  = Map.ObjectToObject<Order2>(source);

			Assert.AreEqual("1 Main",  order.BillingAddress.Street);
			Assert.AreEqual("Bigtown", order.BillingAddress.City);
			Assert.AreEqual("XX",      order.BillingAddress.State);
			Assert.AreEqual("00000",   order.BillingAddress.Zip);

			Assert.AreEqual("2 Main",  order.ShippingAddress.Street);
			Assert.AreEqual("Bigtown", order.ShippingAddress.City);
			Assert.AreEqual("XX",      order.ShippingAddress.State);
			Assert.AreEqual("00000",   order.ShippingAddress.Zip);
		}
	}
}
