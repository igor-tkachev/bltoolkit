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
		[ExpectedException(typeof(RsdnValidationException))]
		public void RequiredFailed()
		{
			BizEntity1 be = (BizEntity1)Map.Descriptor(typeof(BizEntity1)).CreateInstance();

			be.Validate();
		}

		public abstract class BizEntity2 : ValidatableEntityBase
		{
			[MinValue(10)]
			public abstract int Number { get; set; }
		}

		[Test]
		[ExpectedException(typeof(RsdnValidationException))]
		public void MinValueFailed()
		{
			BizEntity2 be = (BizEntity2)Map.Descriptor(typeof(BizEntity2)).CreateInstance();

			be.Number = 1;

			be.Validate();
		}

		public abstract class BizEntity3 : ValidatableEntityBase
		{
			[MinDateValue(2000, 1, 1)]
			public abstract DateTime Date { get; set; }
		}

		[Test]
		[ExpectedException(typeof(RsdnValidationException))]
		public void MinDateValueFailed()
		{
			BizEntity3 be = (BizEntity3)Map.Descriptor(typeof(BizEntity3)).CreateInstance();

			be.Date = new DateTime(1999, 1, 1);

			be.Validate();
		}

		[Test]
		public void MinDateValueOK()
		{
			BizEntity3 be = (BizEntity3)Map.Descriptor(typeof(BizEntity3)).CreateInstance();

			be.Date = new DateTime(2000, 1, 1);

			be.Validate();
		}

		public abstract class BizEntity4 : ValidatableEntityBase
		{
			[MinDateValue(2000, 1, 1, IsExclusive = true)]
			public abstract DateTime Date { get; set; }
		}

		[Test]
		[ExpectedException(typeof(RsdnValidationException))]
		public void MinDateValueExclusiveFailed()
		{
			BizEntity4 be = (BizEntity4)Map.Descriptor(typeof(BizEntity4)).CreateInstance();

			be.Date = new DateTime(2000, 1, 1);

			be.Validate();
		}

		public abstract class BizEntity5 : ValidatableEntityBase
		{
			[MaxValue(100.001)]
			public abstract double Total { get; set; }
		}

		[Test]
		[ExpectedException(typeof(RsdnValidationException))]
		public void MaxDoubleValueFailed()
		{
			BizEntity5 be = (BizEntity5)Map.Descriptor(typeof(BizEntity5)).CreateInstance();

			be.Total = 200;

			be.Validate();
		}

		public abstract class BizEntity6 : ValidatableEntityBase
		{
			[MaxValue(100.001, true)]
			public abstract double Total { get; set; }
		}

		[Test]
		[ExpectedException(typeof(RsdnValidationException))]
		public void MaxDoubleValueExclusiveFailed()
		{
			BizEntity6 be = (BizEntity6)Map.Descriptor(typeof(BizEntity6)).CreateInstance();

			be.Total = 100.001;

			be.Validate();
		}

		public abstract class BizEntity7 : ValidatableEntityBase
		{
			[RegEx("abcd")]
			public abstract string Number { get; set; }
		}

		[Test]
		[ExpectedException(typeof(RsdnValidationException))]
		public void RegexFailed()
		{
			BizEntity7 be = (BizEntity7)Map.Descriptor(typeof(BizEntity7)).CreateInstance();

            be.Number = "dcba";

			be.Validate();
		}

		[Test]
		public void RegexOK()
		{
			BizEntity7 be = (BizEntity7)Map.Descriptor(typeof(BizEntity7)).CreateInstance();

			be.Number = "abcd";

			be.Validate();
		}

		public abstract class BizEntity8 : ValidatableEntityBase
		{
			[MinLength(10)]
			public abstract string Name { get; set; }
		}

		[Test]
		[ExpectedException(typeof(RsdnValidationException), "'BizEntity8.Name' minimum length is 10.")]
		public void MinLengthFailed()
		{
			BizEntity8 be = (BizEntity8)Map.Descriptor(typeof(BizEntity8)).CreateInstance();

			be.Name = "12345678";

			be.Validate();
		}

		public abstract class BizEntity9 : ValidatableEntityBase
		{
			[MaxLength(10), FriendlyName("First Name")]
			public abstract string Name { get; set; }
		}

		[Test]
		[ExpectedException(typeof(RsdnValidationException), "'First Name' maximum length is 10.")]
		public void MaxLengthFailed()
		{
			BizEntity9 be = (BizEntity9)Map.Descriptor(typeof(BizEntity9)).CreateInstance();

			be.Name = "12345678901";

			be.Validate();
		}
	}
}
