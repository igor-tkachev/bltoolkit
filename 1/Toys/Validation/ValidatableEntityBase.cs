using System;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.Validation
{
	[MapAction(typeof(IValidatable))]
	[MapType(typeof(int),    typeof(ValidatableValue<int>))]
	[MapType(typeof(string), typeof(ValidatableValue<string>), "")]
	public abstract class ValidatableEntityBase
	{
		public virtual void Validate()
		{
			if (this is IValidatable)
			{
				((IValidatable)this).Validate(null);
			}
		}
	}
}
