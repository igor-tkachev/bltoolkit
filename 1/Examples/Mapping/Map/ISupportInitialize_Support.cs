/// example:
/// map ISupportInitialize
/// comment:
/// The following example demonstrates how to use the <see cref="ISupportInitialize"/> interface 
/// which is supported by the mapper.
using System;
using System.ComponentModel;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace Examples_Mapping_Map
{
	[TestFixture]
	public class ISupportInitialize_Support
	{
		public class SourceEntity
		{
		}

		public class InitializedEntity : ISupportInitialize
		{
			[MapIgnore] public string BeginString;
			[MapIgnore] public string EndString;

			void ISupportInitialize.BeginInit()
			{
				BeginString = "begin";
			}

			void ISupportInitialize.EndInit()
			{
				EndString = "end";
			}
		}

		[Test]
		public void Test()
		{
			InitializedEntity ie = 
				(InitializedEntity)Map.ToObject(new SourceEntity(), typeof(InitializedEntity));

			Assert.AreEqual("begin", ie.BeginString);
			Assert.AreEqual("end",   ie.EndString);
		}
	}
}
