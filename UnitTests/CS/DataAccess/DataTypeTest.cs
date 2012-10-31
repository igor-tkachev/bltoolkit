using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
using System.Xml;
using BLToolkit.Data.DataProvider;
using BLToolkit.Reflection.Extension;
using NUnit.Framework;

#if FW2
using System.Collections.Generic;
using PersonDataSet = DataAccessTest.PersonDataSet2;
#else
using PersonDataSet = DataAccessTest.PersonDataSet;
#endif

using BLToolkit.EditableObjects;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Validation;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace DataAccess
{
	[TestFixture]
	public class DataTypeTest
	{
		[TableName("DataTypeTest")]
		public abstract class ScalarData
		{
			[MapField("DataTypeID"), PrimaryKey, NonUpdatable]
			public abstract int       ID        { get; set; }
			[MapIgnore(false)]
			public abstract Byte[]    Binary_   { get; set; }
			public abstract Boolean   Boolean_  { get; set; }
			public abstract Byte      Byte_     { get; set; }
			[MapIgnore(false)]
			public abstract Byte[]    Bytes_    { get; set; }
			[NullDateTime]
			public abstract DateTime  DateTime_ { get; set; }
			public abstract Decimal   Decimal_  { get; set; }
			public abstract Double    Double_   { get; set; }
			public abstract Guid      Guid_     { get; set; }
			public abstract Int16     Int16_    { get; set; }
			public abstract Int32     Int32_    { get; set; }
			public abstract Int64     Int64_    { get; set; }
			public abstract Decimal   Money_    { get; set; }
			public abstract Single    Single_   { get; set; }
			public abstract String    String_   { get; set; }
#if FW2
			public abstract Char      Char_     { get; set; }
//			public abstract SByte     SByte_    { get; set; }
//			[MapIgnore(false)]
//			public abstract Stream    Stream_   { get; set; }
//			public abstract UInt16    UInt16_   { get; set; }
//			public abstract UInt32    UInt32_   { get; set; }
//			public abstract UInt64    UInt64_   { get; set; }
//			public abstract XmlReader Xml_      { get; set; }
#endif
		}

		[TableName("DataTypeTest")]
		public class SqlData
		{
			[MapField("DataTypeID"), PrimaryKey, NonUpdatable]
			public int         ID;
			public SqlBinary   Binary_;
/*			public SqlBoolean  Boolean_;
			public SqlByte     Byte_;
			public SqlDateTime DateTime_;
			public SqlDecimal  Decimal_;
			public SqlDouble   Double_;
			public SqlGuid     Guid_;
			public SqlInt16    Int16_;
			public SqlInt32    Int32_;
			public SqlInt64    Int64_;
			public SqlMoney    Money_;
			public SqlSingle   Single_;
			public SqlString   String_;
#if FW2
			[MapIgnore(false)]
			public SqlBytes    Bytes_;
			[MapIgnore(false)]
			public SqlChars    Char_;
			[MapIgnore(false)]
			public SqlXml      Xml_;
#endif
*/		}


		public abstract class DataTypeAccessor : DataAccessor
		{
			public abstract void   Insert([Direction.Output("@DataTypeID")] ScalarData data);
		}

		private DataTypeAccessor _da;
		private SprocQuery       _sproc = new SprocQuery();
		private SqlQuery         _sql   = new SqlQuery();

		public DataTypeTest()
		{
			TypeFactory.SaveTypes = true;

			object o = TypeAccessor.CreateInstance(typeof(ScalarData));
			Assert.IsInstanceOfType(typeof(ScalarData), o);
			
			_da = (DataTypeAccessor)DataAccessor.CreateInstance(typeof(DataTypeAccessor));
			Assert.IsInstanceOfType(typeof(DataTypeAccessor), _da);
		}

		[Test]
		public void Sql_Scalar_InsertDeleteTest()
		{
			ArrayList list = _sql.SelectAll(typeof(ScalarData));
			Hashtable tbl  = new Hashtable();

			foreach (ScalarData d in list)
				tbl[d.ID] = d;

			ScalarData data = (ScalarData) TypeAccessor.CreateInstance(typeof(ScalarData));

			_sql.Insert(data);

			list = _sql.SelectAll(typeof(ScalarData));

			foreach (ScalarData d in list)
				if (tbl.ContainsKey(d.ID) == false)
					_sql.Delete(d);
		}

		[Test]
		public void Sql_SqlTypes_InsertDeleteTest()
		{
			ArrayList list = _sql.SelectAll(typeof(SqlData));
			Hashtable tbl  = new Hashtable();

			foreach (SqlData d in list)
				tbl[d.ID] = d;

			SqlData data = (SqlData) TypeAccessor.CreateInstance(typeof(SqlData));

			_sql.Insert(data);

			list = _sql.SelectAll(typeof(SqlData));

			foreach (SqlData d in list)
				if (tbl.ContainsKey(d.ID) == false)
					_sql.Delete(d);
		}

		[Test]
		public void SunsetByHands()
		{
			using (DbManager db = new DbManager())
			{
				db
					.SetCommand(@"INSERT INTO DataTypeTest (Binary_) VALUES (@Binary)", db.Parameter("@Binary", SqlBinary.Null))
					.ExecuteNonQuery()
					;
			}
		}
	}
}

