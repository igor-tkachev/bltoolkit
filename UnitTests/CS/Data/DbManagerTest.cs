using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace Data
{
	[TestFixture]
	public class DbManagerTest
	{
		public enum Gender
		{
			[MapValue("F")] Female,
			[MapValue("M")] Male,
			[MapValue("U")] Unknown,
			[MapValue("O")] Other
		}

		public class Person
		{
			[MapField("PersonID")]
			public int    ID;
			public string FirstName;
			public string MiddleName;
			public string LastName;
			public Gender Gender;
		}

		public class DataTypeTest
		{
			[MapField("DataTypeID")]
			public int       ID;
			[MapIgnore(false)]
			public Byte[]    Binary_;
			public Boolean   Boolean_;
			public Byte      Byte_;
			[MapIgnore(false)]
			public Byte[]    Bytes_;
			public Char      Char_;
			public DateTime  DateTime_;
			public Decimal   Decimal_;
			public Double    Double_;
			public Guid      Guid_;
			public Int16     Int16_;
			public Int32     Int32_;
			public Int64     Int64_;
			public Decimal   Money_;
			public SByte     SByte_;
			public Single    Single_;
			public Stream    Stream_;
			public String    String_;
			public UInt16    UInt16_;
			public UInt32    UInt32_;
			public UInt64    UInt64_;
			//[MapIgnore(false)]
			//public XmlReader Xml_;
		}

		public class DataTypeSqlTest
		{
			[MapField("DataTypeID")]
			public int ID;
			public SqlBinary   Binary_;
			public SqlBoolean  Boolean_;
			public SqlByte     Byte_;
#if FW2
			[MapIgnore(false)]
			public SqlBytes    Bytes_;
			public SqlChars    Char_;
#endif
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
			public SqlXml      Xml_;
#endif
		}

		[Test]
		public void ExecuteList1()
		{
			using (DbManager db = new DbManager("Sql"))
			{
				ArrayList list = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteList(typeof(Person));
			}
		}

		[Test]
		public void ExecuteList2()
		{
			using (DbManager db = new DbManager("Sql"))
			{
				IList list = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteList(new EditableArrayList(typeof(Person)), typeof(Person));
			}
		}

		[Test]
		public void ExecuteObject()
		{
			using (DbManager db = new DbManager())
			{
				Person p = (Person)db
					.SetCommand("SELECT * FROM Person WHERE PersonID = @id",
						db.Parameter("@id", 1))
					.ExecuteObject(typeof(Person));

				TypeAccessor.WriteConsole(p);
			}
		}

		[Test]
		public void ExecuteObject2()
		{
			using (DbManager db = new DbManager())
			{
				DataTypeTest dt = (DataTypeTest)db
					.SetCommand("SELECT * FROM DataTypeTest WHERE DataTypeID = @id",
						db.Parameter("@id", 2))
					.ExecuteObject(typeof(DataTypeTest));

				TypeAccessor.WriteConsole(dt);
			}
		}

		[Test]
		public void ExecuteObject2Sql()
		{
			using (DbManager db = new DbManager())
			{
				DataTypeSqlTest dt = (DataTypeSqlTest)db
					.SetCommand("SELECT * FROM DataTypeTest WHERE DataTypeID = @id",
						db.Parameter("@id", 2))
					.ExecuteObject(typeof(DataTypeSqlTest));

				TypeAccessor.WriteConsole(dt);
			}
		}

		[Test]
		public void NewConnection()
		{
			string connectionString = "Server=.;Database=BLToolkitData;Integrated Security=SSPI";

			using (DbManager db = new DbManager(new SqlConnection(connectionString)))
			{
				db
					.SetSpCommand ("Person_SelectByName",
						db.Parameter("@firstName", "John"),
						db.Parameter("@lastName",  "Pupkin"))
					.ExecuteScalar();
			}
		}
	}
}
