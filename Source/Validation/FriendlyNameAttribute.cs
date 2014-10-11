using System;

namespace BLToolkit.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
	public class FriendlyNameAttribute : Attribute
	{
		public FriendlyNameAttribute(string name)
		{
			_name = name;
		}

		private readonly string _name;
		public           string  Name
		{
			get { return _name; }
		}
	}
}
