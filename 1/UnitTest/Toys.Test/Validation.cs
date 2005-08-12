using System;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;
using Rsdn.Framework.Validation;

namespace Toys.Test
{
	[TestFixture]
	public class Validation
	{
		public abstract class BizEntity1 : ValidatableEntityBase
		{
			[Required]
			public abstract string Name { get; set; }
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void RequiredFailed()
		{
			BizEntity1 be = (BizEntity1)Map.Descriptor<BizEntity1>().CreateInstance();

			be.Validate();
		}
	}
}
