using System.Collections.Generic;

using BLToolkit.Data;
using BLToolkit.Mapping.Fluent;
using BLToolkit.Mapping.MemberMappers;
using BLToolkit.Reflection.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLToolkit.Fluent.Test
{
	/// <summary>
	/// Test for FluentConfig
	/// </summary>
	[TestClass]
    public class FluentMapAttributesTest
	{
		/// <summary>
		/// Test configure mapping
		/// </summary>
		[TestMethod]
		public void ShouldConfigMapping()
		{
			ExtensionList extensions = new ExtensionList();
            FluentConfig.Configure(extensions).MapingFromAssemblyOf<FluentMapAttributesTest>();

		    var key = extensions[typeof(DboFluentMapAttributesTest).FullName];
		    var mem1 = key["Id"];
            Assert.IsTrue(mem1.Attributes.ContainsKey("Identity"));
		    
            var mem2 = key["Test"];
            Assert.IsTrue(mem2.Attributes.ContainsKey("MemberMapper"));
            Assert.IsTrue(mem2.Attributes.ContainsKey("DbType"));            
		}

        public class DboFluentMapAttributesTest
		{
            public int Id { get; set; }
            public List<string> Test { get; set; }
		}

        public class DboFluentMapAttributesTestMap : FluentMap<DboFluentMapAttributesTest>
		{
            public DboFluentMapAttributesTestMap()
			{
				TableName("t1");

			    MapField(x => x.Id).Identity();
			    MapField(x => x.Test).MapIgnore(false).MemberMapper(typeof(BinarySerialisationMapper)).DbType(System.Data.DbType.Binary);
			}
		}
		
	}
}