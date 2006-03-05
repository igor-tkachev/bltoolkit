using System;

using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	public enum Gender
	{
		[MapValue("F")] Female,
		[MapValue("M")] Male,
		[MapValue("U")] Unknown,
		[MapValue("O")] Other
	}
}
