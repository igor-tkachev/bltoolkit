using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture]
	public class ObjectMapperTest
	{
		public class Object1
		{
			[MapField("col2")]
			public int Col1 { get; set; }
		}


		[Test]
		public void NameTest1()
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("col1", typeof(int));
			dt.Columns.Add("col2", typeof(int));
			dt.Rows.Add(1, 2);

			Object1 o = Map.DataRowToObject<Object1>(dt.Rows[0]);
			Assert.AreEqual(2, o.Col1);
		}

		[Test]
		public void NameTest2()
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("col2", typeof(int));
			dt.Columns.Add("col1", typeof(int));
			dt.Rows.Add(2, 1);

			Object1 o = Map.DataRowToObject<Object1>(dt.Rows[0]);
			Assert.AreEqual(2, o.Col1);
		}

		[Test]
		public void GetMemberMapperByNameAliasTest()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Assert.IsNotNull(om["col2"]);
			Assert.IsNull   (om["Col1"]);
			Assert.IsNotNull(om["Col1", true]);
			Assert.IsNull   (om["col1", true]);
		}
	}
}
