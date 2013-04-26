using System;
using System.Collections.Generic;
using System.Linq;

using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

// ReSharper disable ClassNeverInstantiated.Local

namespace Data.Linq.UserTests
{
	[TestFixture]
	public class MultiPartIdentifierTest : TestBase
	{
		class Table1
		{
			public long   Field1;
			public bool   ConfirmedEndView           { get; set; }
			public bool   ConfirmedSchematic         { get; set; }
			public char?  Gender                     { get; set; }
			public string WireSegmentLabel           { get; set; }
			public int    EngineeringCavityLabelsID  { get; set; }
			public int?   EngineeringFunctionID      { get; set; }
			public int?   CircuitColor               { get; set; }
			public int?   TraceColor                 { get; set; }
			public int?   TraceTwoColor              { get; set; }
			public int?   CircuitMaterialID          { get; set; }
			public long   EngineeringConnectorID     { get; set; }
			public int?   QualifierID                { get; set; }
			public int?   EngineeringCircuitNumberID { get; set; }
			public int?   GaugeID                    { get; set; }
			public int?   TerminalPartNumberID       { get; set; }
			public int?   ServiceCircuitID           { get; set; }

			[Association(ThisKey = "EngineeringConnectorID", OtherKey = "EngineeringConnectorID", CanBeNull = false)]
			public Table2 Table2Ref { get; set; }

			[Association(ThisKey = "ServiceCircuitID", OtherKey = "ServiceCircuitID", CanBeNull = true)]
			public Table4 Table4Ref { get; set; }
		}

		class Table2
		{
			[Identity, PrimaryKey(1)] public long   EngineeringConnectorID            { get; set; }
			                          public int    CavityCount                       { get; set; }
			                          public string ConnectorNumber                   { get; set; }
			[Nullable               ] public char?  ConnectorGender                   { get; set; }
			[Nullable               ] public string EndviewFileName                   { get; set; }
			[Nullable               ] public string UnEditedConnectorNumber           { get; set; }
			                          public bool   ConfirmedEndView                  { get; set; }
			                          public bool   ConfirmedSchematic                { get; set; }
			                          public bool   IsNonDEFData                      { get; set; }
			                          public bool   IsInLine                          { get; set; }
			[Nullable               ] public long?  PreviousConnectorID               { get; set; }
			                          public int    SupplierID                        { get; set; }
			[Nullable               ] public int?   ColorID                           { get; set; }
			                          public int    HarnessID                         { get; set; }
			[Nullable               ] public int?   DeviceID                          { get; set; }
			[Nullable               ] public int?   ProblematicalField                { get; set; }
			[Nullable               ] public int?   EngineeringConnectorPartNumberID  { get; set; }
			[Nullable               ] public int?   EngineeringConnectorDescriptionID { get; set; }

			[Association(ThisKey = "EngineeringConnectorID", OtherKey = "EngineeringConnectorID", CanBeNull = false)]
			public List<Table1> Table1s { get; set; }

			[Association(ThisKey="HarnessID", OtherKey="HarnessID", CanBeNull=false)]
			public Table3 Table3Ref { get; set; }
		}

		class Table3
		{
			[Identity, PrimaryKey(1)] public int    HarnessID         { get; set; }
			                          public string HarnessPartNumber { get; set; }
			[Nullable               ] public string RevisionNumber    { get; set; }
			                          public bool   HarnessFlag       { get; set; }
			                          public bool   VirtualHarness    { get; set; }
			[Nullable               ] public int?   PreviousHarnessID { get; set; }
			                          public int    FamilyID          { get; set; }
			                          public int    RevisionID        { get; set; }
			[Nullable               ] public int?   ServiceFamilyID   { get; set; }

			[Association(ThisKey="HarnessID", OtherKey="HarnessID", CanBeNull=true)]
			public List<Table2> Table2s { get; set; }
		}

		class Table4
		{
			[Association(ThisKey = "ServiceCircuitID", OtherKey = "ServiceCircuitID", CanBeNull = true)]
			public List<Table1> EngineeringCircuits { get; set; }

			public int    ServiceCircuitID            { get; set; }
			public char?  Gender                      { get; set; }
			public string BestView                    { get; set; }
			public string TerminalPartLocation        { get; set; }
			public string CoreCrimpType               { get; set; }
			public string InsulatorCrimpType          { get; set; }
			public int?   ServiceFunctionID           { get; set; }
			public int?   ServiceCavityLabelsID       { get; set; }
			public int    ProblematicalField          { get; set; }
			public int?   QualifierID                 { get; set; }
			public int?   CircuitEndGroupID           { get; set; }
			public int?   ServiceCircuitNumberID      { get; set; }
			public int?   GaugeID                     { get; set; }
			public int?   ReleaseToolPartNumberID     { get; set; }
			public int?   TestProbePartNumberID       { get; set; }
			public int?   CircuitColor                { get; set; }
			public int?   TraceColor                  { get; set; }
			public int?   TraceTwoColor               { get; set; }
			public int?   TestProbeColorID            { get; set; }
			public int?   ServiceTerminalPartNumberID { get; set; }

			[Association(ThisKey="ProblematicalField", OtherKey="ProblematicalField", CanBeNull=false)]
			public Table5 Table5Ref { get; set; }
		}

		class Table5
		{
			public int? ParentProblematicalField;
			public int  ProblematicalField;
			public int  RevisionID;

			[Association(ThisKey = "ParentProblematicalField", OtherKey = "ProblematicalField", CanBeNull = true)]
			public Table5 Table5Ref { get; set; }

			[Association(ThisKey = "ProblematicalField", OtherKey = "ProblematicalField", CanBeNull = true)]
			public List<Table4> Table4s { get; set; }
		}

		[Test]
		public void Test([IncludeDataContexts("Sql2008")] string context)
		{
			using (var db = GetDataContext(context))
			{
				var q =
					from t1 in db.GetTable<Table5>()
					from t2 in
						(from t3 in t1.Table4s.SelectMany(x => x.EngineeringCircuits)
						 from t4 in
							from t5 in t3.Table4Ref.Table5Ref.Table5Ref.Table4s
							from t6 in t5.EngineeringCircuits
							select t6
						 select t4.Field1)
					from t7 in
						(from t8 in t1.Table5Ref.Table4s.SelectMany(x => x.EngineeringCircuits)
						 from t9 in
							from t10 in t8.Table2Ref.Table3Ref.Table2s
							from t11 in t10.Table1s
							select t11
						 select t9.Field1)
					where t2 == t7
					select t7;

				var sql = q.ToString();
			}
		}
	}
}
