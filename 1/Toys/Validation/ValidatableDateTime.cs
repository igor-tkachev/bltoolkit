using System;

namespace Rsdn.Framework.Validation
{
	[Obsolete]
	public class ValidatableDateTime: ValidatableValue
	{
		public ValidatableDateTime() : base(DateTime.MinValue)
		{
		}
	}
}
