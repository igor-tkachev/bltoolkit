using System;

using BLToolkit.Mapping;
using BLToolkit.Validation;

namespace BLToolkit.Demo.ObjectModel
{
	[MapField("PersonID", "ID")]
	public abstract class Person : BizEntity
	{
		[MaxLength(50), Required] public abstract string LastName   { get; set; }
		[MaxLength(50), Required] public abstract string FirstName  { get; set; }
		[MaxLength(50)]           public abstract string MiddleName { get; set; }
		[               Required] public abstract Gender Gender     { get; set; }

		[MapIgnore]
		public string FullName
		{
			get
			{
				return string.Format(
					string.IsNullOrEmpty(MiddleName)? "{2}, {0}": "{2}, {0} {1}.",
					FirstName, MiddleName, LastName);
			}
		}
	}
}
