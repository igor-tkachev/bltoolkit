using System;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PetShop.ObjectModel
{
	public class CustomProfile
	{
		[PrimaryKey]
		[MapField("UniqueID")] public int       ID;
		[MapField("Username")] public string    UserName;
		                       public string    ApplicationName;
		                       public bool?     IsAnonymous;
		                       public DateTime? LastActivityDate;
		                       public DateTime? LastUpdatedDate;
	}
}
