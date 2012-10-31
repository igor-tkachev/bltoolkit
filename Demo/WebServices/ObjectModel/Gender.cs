using BLToolkit.Mapping;

namespace Demo.WebServices.ObjectModel
{
	public enum Gender
	{
		[MapValue("F")] Female,
		[MapValue("M")] Male,
		[MapValue("U")] Unknown,
		[MapValue("O")] Other
	}
}