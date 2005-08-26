using System;

namespace Rsdn.Framework.Validation
{
	public class ValidatableGuid : ValidatableValue
	{
		public ValidatableGuid() : base(Guid.Empty)
		{
		}
	}
}
