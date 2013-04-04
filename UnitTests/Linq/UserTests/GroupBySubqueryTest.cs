using System;
using System.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using NUnit.Framework;

namespace Data.Linq.UserTests
{
	[TestFixture]
	public class GroupBySubqueryTest : TestBase
	{
		[TableName(Name = "EngineeringCircuitEnd")]
		public class EngineeringCircuitEndRecord
		{
			[Association(ThisKey = "EngineeringConnectorID", OtherKey = "EngineeringConnectorID", CanBeNull = false)]
			public EngineeringConnectorRecord EngineeringConnectoRef { get; set; }

			[Association(ThisKey = "ServiceCircuitID", OtherKey = "ServiceCircuitID", CanBeNull = true)]
			public ServiceCircuitEndRecord ServiceCircuitRef { get; set; }

			[Association(ThisKey = "EngineeringCircuitNumberID", OtherKey = "EngineeringCircuitNumberID", CanBeNull = true)]
			public EngineeringCircuitNumberRecord EngineeringCircuitNumberRef { get; set; }

			[Nullable]
			public int? ServiceCircuitID { get; set; }
		}

		[TableName(Name = "EngineeringCircuitNumber")]
		public class EngineeringCircuitNumberRecord
		{
			public string EngineeringCircuitNumber { get; set; }
		}

		[TableName(Name = "EngineeringConnector")]
		public class EngineeringConnectorRecord
		{
			[AssociationAttribute(ThisKey = "HarnessID", OtherKey = "HarnessID", CanBeNull = false)]
			public HarnessRecord HarnessRef { get; set; }
		}

		[TableNameAttribute(Name = "Harness")]
		public class HarnessRecord
		{
			public int RevisionID { get; set; }
		}

		[TableNameAttribute(Name = "ServiceCircuitEnd")]
		public class ServiceCircuitEndRecord
		{
			[Association(ThisKey = "ServiceFunctionID", OtherKey = "ServiceFunctionID", CanBeNull = true)]
			public ServiceFunctionNameRecord ServiceFunctionRef { get; set; }
		}

		[TableNameAttribute(Name = "ServiceFunctionNames")]
		public class ServiceFunctionNameRecord
		{
			public string ServiceFunctionNames { get; set; }
		}

		[Test]
		public void Test()
		{
			/*
			using (var db = new TestDbManager())
			{
				var q = (
					from engineeringCircuitEnd in db.GetTable<EngineeringCircuitEndRecord>()
					where engineeringCircuitEnd.ServiceCircuitID != null
					select new
					{
						RevisionId      = engineeringCircuitEnd.EngineeringConnectoRef.HarnessRef.RengineeringCircuitEnd.EngineeringCircuitNumberRef.EngineeringCircuitNu
						ServiceFunction = engineeringCircuitEnd.ServiceCircuitRef.ServiceFunctionRef.Service ?? string.Empty
					}
				).Distinct();

				var q2 =
					from t3 in q
					group t3 by new { t3.RevisionId, t3.EngineeringCircuitNumber }
					into g
					where g.Count() > 1
					select new { g.Key.RevisionId, g.Key.EngineeringCircuitNumber, Count = g.Count() };
			}
			*/
		}
	}
}
