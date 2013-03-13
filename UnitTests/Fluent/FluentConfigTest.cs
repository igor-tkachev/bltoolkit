using BLToolkit.Data;
using BLToolkit.Mapping.Fluent;
using BLToolkit.Reflection.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLToolkit.Fluent.Test
{
	/// <summary>
	/// Test for FluentConfig
	/// </summary>
	[TestClass]
	public class FluentConfigTest
	{
		/// <summary>
		/// Test configure mapping
		/// </summary>
		[TestMethod]
		public void ShouldConfigMapping()
		{
			ExtensionList extensions = new ExtensionList();
			FluentConfig.Configure(extensions)
				.MapingFromAssemblyOf<FluentConfigTest>();

			Assert.IsTrue(extensions.ContainsKey(typeof(Dbo1).FullName), "Not mapping");
			Assert.IsFalse(extensions.ContainsKey(typeof(Dbo2).FullName), "Fail mapping for abstract");
			Assert.IsFalse(extensions.ContainsKey(typeof(Dbo3).FullName), "Fail mapping for generic");
		}

		public class Dbo1
		{
		}
		public class Dbo2
		{
		}
		public class Dbo3
		{
		}
		public class Dbo1Map : FluentMap<Dbo1>
		{
			public Dbo1Map()
			{
				TableName("t1");
			}
		}
		public abstract class Dbo2Map : FluentMap<Dbo1>
		{
			public Dbo2Map()
			{
				TableName("t2");
			}
		}
		public class Dbo3Map<T> : FluentMap<Dbo1>
		{
			public Dbo3Map()
			{
				TableName("t3");
			}
		}
	}
}