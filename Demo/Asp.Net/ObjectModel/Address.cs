using System;

using BLToolkit.Common;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PetShop.ObjectModel
{
	public class Address : EntityBase
	{
		[MapField("ToFirstName")] public string FirstName;
		[MapField("ToLastName")]  public string LastName;
		[MapField("Addr1")]       public string Line1;
		[MapField("Addr2")]       public string Line2;
		                          public string City;
		                          public string State;
		                          public string Zip;
		                          public string Country;

		[NonUpdatable]            public string Phone;
		[NonUpdatable]            public string Email;
	}
}
