using System;

using BLToolkit.Common;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PetShop.ObjectModel
{
	public class Category : EntityBase
	{
		[PrimaryKey]
		[MapField("CategoryId")] public string ID;
		                         public string Name;
		[MapField("Descn")]      public string Description;
	}
}
