using System;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.Validation
{
	[Obsolete]
	public abstract class ValidatableEntityBase : IValidatableEntity
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
