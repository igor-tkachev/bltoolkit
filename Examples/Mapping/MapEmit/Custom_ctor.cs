/// example:
/// emit Custom_ctor
/// comment:
/// The following example demonstrates the ability to call custom constructor of the generated class.
using System;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace Examples_Mapping_MapEmit
{
	[TestFixture]
	public class Custom_ctor
	{
		public abstract class BizEntity
		{
			public BizEntity()
			{
				Value = "1";
			}

			public BizEntity(string value)
			{
				Value = value;
			}

			/*
			public BizEntity()
			{
			}
			*/

			public abstract string Value { get; set; }
		}

		[Test]
		public void Test()
		{
			BizEntity entity = (BizEntity)Map.Descriptor(typeof(BizEntity)).CreateInstance();

			Assert.AreEqual("1", entity.Value);

			//entity = (BizEntity)Map.Descriptor(typeof(BizEntity)).CreateInstance("2");

			//Assert.AreEqual("2", entity.Value);
		}
	}
}
