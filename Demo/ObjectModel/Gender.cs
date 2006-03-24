using System;

using BLToolkit.Mapping;

namespace BLToolkit.Demo.ObjectModel
{
	public enum Gender
	{
		[MapValue("F")] Female,
		[MapValue("M")] Male,
		[MapValue("U")] Unknown,
		[MapValue("O")] Other
	}
}
