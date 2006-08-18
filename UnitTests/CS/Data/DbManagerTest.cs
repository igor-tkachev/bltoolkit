using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
#if FW2
using System.IO;
using System.Xml;
#endif

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
			public DateTime  DateTime_;
			public Decimal   Decimal_;
			public Double    Double_;
			public Guid      Guid_;
			public Int16     Int16_;
			public Int32     Int32_;
			public Int64     Int64_;
			public Decimal   Money_;
			public Single    Single_;
			public String    String_;
#if FW2
			public Char      Char_;
			public SByte     SByte_;
			[MapIgnore(false)]
			public Stream    Stream_;
			public UInt16    UInt16_;
			public UInt32    UInt32_;
			public UInt64    UInt64_;
			public XmlReader Xml_;
#endif
		}

		public class DataTypeSqlTest
		{
			[MapField("DataTypeID")]
			public int ID;
			public SqlBinary   Binary_;
			public SqlBoolean  Boolean_;
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
			public SqlChars    Chars_;
			[MapIgnore(false)]
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
				
				Assert.IsNotEmpty(list);
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
				
				Assert.IsNotEmpty(list);
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

		public class OutRefTest
		{
			public int    ID             = 5;
			public int    outputID;
			public int    inputOutputID  = 10;
			public string str            = "5";
			public string outputStr;
			public string inputOutputStr = "10";
		}

		[Test]
		public void MapOutput()
		{
			OutRefTest o = new OutRefTest();

			using (DbManager db = new DbManager())
			{
				db
					.SetSpCommand("OutRefTest", db.CreateParameters(o,
						new string[] {      "outputID",      "outputStr" },
						new string[] { "inputOutputID", "inputOutputStr" },
						null))
					.ExecuteNonQuery(o);
			}

			Assert.AreEqual(5,     o.outputID);
			Assert.AreEqual(15,    o.inputOutputID);
			Assert.AreEqual("5",   o.outputStr);
			Assert.AreEqual("510", o.inputOutputStr);
		}

		public class PeturnParameter
		{
			public int Value;
		}

		[Test]
		public void MapReturnValue()
		{
			PeturnParameter e = new PeturnParameter();

			using (DbManager db = new DbManager())
			{
				db
					.SetSpCommand("Scalar_ReturnParameter")
					.ExecuteNonQuery("Value", e);

				Assert.AreEqual(12345, e.Value);
			}
		}

		[Test]
		public void InsertAndMapBack()
		{
			Person e = new Person();
			e.FirstName = "Crazy";
			e.LastName  = "Frog";
			e.Gender    =  Gender.Other;

			using (DbManager db = new DbManager())
			{
				db
					.SetSpCommand("Person_Insert", db.CreateParameters(e))
					.ExecuteObject(e);

				Assert.IsTrue(e.ID > 0);

				db
					.SetSpCommand("Person_Delete", db.CreateParameters(e))
					.ExecuteNonQuery();
			}
		}
		
		[Test]
		public void CreateParametersTest()
		{
			using (DbManager db = new DbManager())
			{
				DataTypeTest dt = new DataTypeTest();
				
				dt.ID        = 12345;
				dt.Binary_   = new byte[2] {1, 2};
				dt.Boolean_  = true;
				dt.Byte_     = 250;
				dt.Bytes_    = new byte[2] {2, 1};
				dt.DateTime_ = DateTime.Now;
				dt.Decimal_  = 9876543210.0m;
				dt.Double_   = 12345.67890;
				dt.Guid_     = Guid.Empty;
				dt.Int16_    = 12345;
				dt.Int32_    = 1234567890;
				dt.Int64_    = 1234567890123456789;
				dt.Money_    = 99876543210.0m;
				dt.Single_   = 1234.0f;
				dt.String_   = "Crazy Frog";
#if FW2
				dt.Char_     = 'F';
				dt.SByte_    = 123;
				dt.Stream_   = new MemoryStream(5);
				dt.UInt16_   = 65432;
				dt.UInt32_   = 4000000000;
				dt.UInt64_   = 12345678901234567890;
				dt.Xml_      = new XmlTextReader(new StringReader("<xml/>"));
#endif
				
				IDbDataParameter[] parameters = db.CreateParameters(dt);

				foreach (IDbDataParameter parameter in parameters)
				{
					Console.WriteLine("{0}: '{1}'", parameter.ParameterName, parameter.Value);
				}
			}
		}

		[Test]
		public void CreateParametersSqlTest()
		{
			using (DbManager db = new DbManager())
			{
				DataTypeSqlTest dt = new DataTypeSqlTest();
				
				dt.ID        = 12345;
				dt.Binary_   = new SqlBinary(new byte[2] {1, 2});
				dt.Boolean_  = new SqlBoolean(1);
				dt.Byte_     = new SqlByte(250);
				dt.DateTime_ = new SqlDateTime(DateTime.Now);
				dt.Decimal_  = new SqlDecimal(9876543210.0m);
				dt.Double_   = new SqlDouble(12345.67890);
				dt.Guid_     = new SqlGuid(Guid.Empty);
				dt.Int16_    = new SqlInt16(12345);
				dt.Int32_    = new SqlInt32(1234567890);
				dt.Int64_    = new SqlInt64(1234567890123456789);
				dt.Money_    = new SqlMoney(99876543210.0m);
				dt.Single_   = new SqlSingle(1234.0f);
				dt.String_   = new SqlString("Crazy Frog");
#if FW2
				dt.Bytes_    = new SqlBytes(new byte[2] {2, 1});
				dt.Chars_    = new SqlChars(new char[2] {'B', 'L'});
				dt.Xml_      = new SqlXml(new XmlTextReader(new StringReader("<xml/>")));
#endif
				
				IDbDataParameter[] parameters = db.CreateParameters(dt);

				foreach (IDbDataParameter parameter in parameters)
				{
					Console.WriteLine("{0}: '{1}'", parameter.ParameterName, parameter.Value);
				}
			}
		}

		public struct DBInfo
		{
			public DateTime TimeValue;
		}
		
		[Test]
		public void CreateParametersStructTest()
		{
			DBInfo dbInfo = new DBInfo();
			dbInfo.TimeValue = DateTime.Now;
			
			using (DbManager db = new DbManager())
			{
				IDbDataParameter[] parameters = db.CreateParameters(dbInfo);
				
				Assert.IsNotNull(parameters);
				Assert.AreEqual(1, parameters.Length);
				Assert.AreEqual(dbInfo.TimeValue, parameters[0].Value);
				
			}
		}
	}
}
