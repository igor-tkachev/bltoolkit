using System;

using BLToolkit.Common;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PetShop.ObjectModel
{
	[TableName("Orders")]
	public class Order : EntityBase
	{
		[PrimaryKey, NonUpdatable]
		[MapField("OrderId")]        public int      ID;

		[MapField("UserId")]         public string   UserID;
		                             public DateTime OrderDate;
		                             public string   Courier;
		                             public decimal  TotalPrice;
		                             public int      AuthorizationNumber;
		                             public string   Locale;

		[MapField(Format="Ship{0}")] public Address  ShippingAddress = new Address();
		[MapField(Format="Bill{0}")] public Address  BillingAddress  = new Address();

		[NonUpdatable]               public string   Status;

		public OrderLineItem[] Lines;
		public CreditCard      CreditCard;
	}
}
