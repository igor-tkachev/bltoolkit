using System;

namespace Rsdn.Framework.Validation
{
	public class ValidatableObjectBase
	{
		public virtual void Validate()
		{
			Validator.Validate(this);
		}
	}
}
