using System;

using BLToolkit.Mapping;

namespace Data.Linq.Model
{
	public enum Gender
	{
		[MapValue("M")] Male,
		[MapValue("F")] Female,
		[MapValue("U")] Unknown,
		[MapValue("O")] Other,
	}
}
