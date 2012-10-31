using System;

using BLToolkit.Mapping;

namespace PetShop.ObjectModel
{
	public enum ItemStatus
	{
		[NullValue]     Null,
		[MapValue("P")] PStatus
	}
}
