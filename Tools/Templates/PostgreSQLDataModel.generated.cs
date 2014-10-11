﻿//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by BLToolkit template for T4.
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------
using System;

using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Validation;

using NpgsqlTypes;

namespace PostgreSqlDataModel
{
	public partial class PostgreSqlDataContext : DbManager
	{
		public Table<alltypes>      alltypes      { get { return this.GetTable<alltypes>();      } }
		public Table<Child>         Child         { get { return this.GetTable<Child>();         } }
		public Table<Doctor>        Doctor        { get { return this.GetTable<Doctor>();        } }
		public Table<entity>        entity        { get { return this.GetTable<entity>();        } }
		public Table<GrandChild>    GrandChild    { get { return this.GetTable<GrandChild>();    } }
		public Table<LinqDataTypes> LinqDataTypes { get { return this.GetTable<LinqDataTypes>(); } }
		public Table<Parent>        Parent        { get { return this.GetTable<Parent>();        } }
		public Table<Patient>       Patient       { get { return this.GetTable<Patient>();       } }
		public Table<Person>        Person        { get { return this.GetTable<Person>();        } }
		public Table<SequenceTest1> SequenceTest1 { get { return this.GetTable<SequenceTest1>(); } }
		public Table<SequenceTest2> SequenceTest2 { get { return this.GetTable<SequenceTest2>(); } }
		public Table<SequenceTest3> SequenceTest3 { get { return this.GetTable<SequenceTest3>(); } }
		public Table<TestIdentity>  TestIdentity  { get { return this.GetTable<TestIdentity>();  } }
	}

	[TableName(Owner="public", Name="alltypes")]
	public partial class alltypes
	{
		[Identity, PrimaryKey(1), Required] public Int32             id                  { get; set; } // integer
		[Nullable                         ] public Int64?            bigintdatatype      { get; set; } // bigint
		[Nullable                         ] public Decimal?          numericdatatype     { get; set; } // numeric
		[Nullable                         ] public Int16?            smallintdatatype    { get; set; } // smallint
		[Nullable                         ] public Int32?            intdatatype         { get; set; } // integer
		[Nullable                         ] public Decimal?          moneydatatype       { get; set; } // money
		[Nullable                         ] public Double?           doubledatatype      { get; set; } // double precision
		[Nullable                         ] public Single?           realdatatype        { get; set; } // real
		[Nullable                         ] public DateTime?         timestampdatatype   { get; set; } // timestamp without time zone
		[Nullable                         ] public DateTime?         timestamptzdatatype { get; set; } // timestamp with time zone
		[Nullable                         ] public DateTime?         datedatatype        { get; set; } // date
		[Nullable                         ] public DateTime?         timedatatype        { get; set; } // time without time zone
		[Nullable                         ] public DateTime?         timetzdatatype      { get; set; } // time with time zone
		[Nullable                         ] public NpgsqlInterval?   intervaldatatype    { get; set; } // interval
		[                         Required] public String            chardatatype        { get; set; } // character(1)(1)
		[                         Required] public String            varchardatatype     { get; set; } // character varying(20)(20)
		[                         Required] public String            textdatatype        { get; set; } // text
		[                         Required] public Byte[]            binarydatatype      { get; set; } // bytea
		[Nullable                         ] public Guid?             uuiddatatype        { get; set; } // uuid
		[Nullable                         ] public BitString?        bitdatatype         { get; set; } // bit(3)(3)
		[Nullable                         ] public Boolean?          booleandatatype     { get; set; } // boolean
		[Nullable                         ] public object            colordatatype       { get; set; } // color
		[Nullable                         ] public NpgsqlPoint?      pointdatatype       { get; set; } // point
		[Nullable                         ] public NpgsqlLSeg?       lsegdatatype        { get; set; } // lseg
		[Nullable                         ] public NpgsqlBox?        boxdatatype         { get; set; } // box
		[Nullable                         ] public NpgsqlPath?       pathdatatype        { get; set; } // path
		[Nullable                         ] public NpgsqlPolygon?    polygondatatype     { get; set; } // polygon
		[Nullable                         ] public NpgsqlCircle?     circledatatype      { get; set; } // circle
		[Nullable                         ] public NpgsqlInet?       inetdatatype        { get; set; } // inet
		[Nullable                         ] public NpgsqlMacAddress? macaddrdatatype     { get; set; } // macaddr
		[Nullable                         ] public String            xmldatatype         { get; set; } // xml
	}

	[TableName(Owner="public", Name="Child")]
	public partial class Child
	{
		[Nullable] public Int32? ParentID { get; set; } // integer
		[Nullable] public Int32? ChildID  { get; set; } // integer
	}

	[TableName(Owner="public", Name="Doctor")]
	public partial class Doctor
	{
		[Required] public Int32  PersonID { get; set; } // integer
		[Required] public String Taxonomy { get; set; } // character varying(50)(50)
	}

	[TableName(Owner="public", Name="entity")]
	public partial class entity
	{
		[Required] public String the_name { get; set; } // character varying(255)(255)
	}

	[TableName(Owner="public", Name="GrandChild")]
	public partial class GrandChild
	{
		[Nullable] public Int32? ParentID     { get; set; } // integer
		[Nullable] public Int32? ChildID      { get; set; } // integer
		[Nullable] public Int32? GrandChildID { get; set; } // integer
	}

	[TableName(Owner="public", Name="LinqDataTypes")]
	public partial class LinqDataTypes
	{
		[Nullable          ] public Int32?    ID             { get; set; } // integer
		[Nullable          ] public Decimal?  MoneyValue     { get; set; } // numeric(10,4)(10)(4)
		[Nullable          ] public DateTime? DateTimeValue  { get; set; } // timestamp without time zone
		[Nullable          ] public DateTime? DateTimeValue2 { get; set; } // timestamp without time zone
		[Nullable          ] public Boolean?  BoolValue      { get; set; } // boolean
		[Nullable          ] public Guid?     GuidValue      { get; set; } // uuid
		[          Required] public Byte[]    BinaryValue    { get; set; } // bytea
		[Nullable          ] public Int16?    SmallIntValue  { get; set; } // smallint
		[Nullable          ] public Int32?    IntValue       { get; set; } // integer
		[Nullable          ] public Int64?    BigIntValue    { get; set; } // bigint
	}

	[TableName(Owner="public", Name="Parent")]
	public partial class Parent
	{
		[Nullable] public Int32? ParentID { get; set; } // integer
		[Nullable] public Int32? Value1   { get; set; } // integer
	}

	[TableName(Owner="public", Name="Patient")]
	public partial class Patient
	{
		[Required] public Int32  PersonID  { get; set; } // integer
		[Required] public String Diagnosis { get; set; } // character varying(256)(256)
	}

	[TableName(Owner="public", Name="Person")]
	public partial class Person
	{
		[Identity, PrimaryKey(1), Required] public Int32  PersonID   { get; set; } // integer
		[                         Required] public String FirstName  { get; set; } // character varying(50)(50)
		[                         Required] public String LastName   { get; set; } // character varying(50)(50)
		[                         Required] public String MiddleName { get; set; } // character varying(50)(50)
		[                         Required] public String Gender     { get; set; } // character(1)(1)
	}

	[TableName(Owner="public", Name="SequenceTest1")]
	public partial class SequenceTest1
	{
		[PrimaryKey(1), Required] public Int32  ID    { get; set; } // integer
		[               Required] public String Value { get; set; } // character varying(50)(50)
	}

	[TableName(Owner="public", Name="SequenceTest2")]
	public partial class SequenceTest2
	{
		[Identity, PrimaryKey(1), Required] public Int32  ID    { get; set; } // integer
		[                         Required] public String Value { get; set; } // character varying(50)(50)
	}

	[TableName(Owner="public", Name="SequenceTest3")]
	public partial class SequenceTest3
	{
		[Identity, PrimaryKey(1), Required] public Int32  ID    { get; set; } // integer
		[                         Required] public String Value { get; set; } // character varying(50)(50)
	}

	[TableName(Owner="public", Name="TestIdentity")]
	public partial class TestIdentity
	{
		[Identity, PrimaryKey(1), Required] public Int32 ID { get; set; } // integer
	}
}
