using System;

using BLToolkit.Common;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PetShop.ObjectModel
{
	public class Product : EntityBase
	{
		[PrimaryKey]
		[MapField("ProductId")]  public string ID;

		[MapField("CategoryId")] public string CategoryID;
		                         public string Name;
		[MapField("Descn")]      public string Description;
		                         public string Image;
	}
}
