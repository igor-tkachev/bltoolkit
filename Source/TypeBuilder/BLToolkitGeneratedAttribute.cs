using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.All)]
	public class BLToolkitGeneratedAttribute : Attribute
	{
		public BLToolkitGeneratedAttribute()
		{
		}
	}
}
