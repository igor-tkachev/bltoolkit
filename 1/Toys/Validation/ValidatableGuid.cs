using System;

namespace Rsdn.Framework.Validation
{
	[Obsolete]
	public class ValidatableGuid : ValidatableValue
	{
		public ValidatableGuid() : base(Guid.Empty)
		{
		}
	}
}
