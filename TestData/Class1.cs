using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.Common;
//using System.Data.OracleClient;
using System.Data.SqlClient;

using Rsdn.Framework.Data;
using Rsdn.Framework.Data.Mapping;

namespace TestData
{
	public class TestReader 
	{
		public object Read(object o, IDataReader dr)
		{
			CategoryInfo ci = o == null? new CategoryInfo(): (CategoryInfo)o;
			object obj;

			ci.CategoryID   = Convert.ToInt32(dr["CategoryID"]);

			obj = dr["CategoryName"];
			ci.CategoryName = Convert.ToString(obj);
			ci.CategoryName = obj is DBNull? string.Empty: Convert.ToString(obj);

			ci.Count        = (short)Convert.ToInt32 (dr["Count"]);

			ci.NullValue    = dr["NullValue"] as string;

			return ci;
		}

		public virtual void CreateInstance(object obj, object value)
		{
			((CategoryInfo)obj).CategoryID = Convert.ToInt32(value);
		}
	}

	public class CategoryInfo
	{
		private int    _CategoryID;
		public int    CategoryID
		{
			get { return _CategoryID; }
			set { _CategoryID = value; }
		}
		public string CategoryName;
		public short    Count;
		[MapField(IsNullable=true, IsTrimmable=false)]
		public string NullValue;
		public string NullValue123;
		public string NullValue123456;
	}

	public class Class1
	{
		const int Count = 3000;

		static ArrayList GetCategories1(int min)
		{
			ArrayList al = null;

			string connectionString = "Server=(local);Database=Northwind;Integrated Security=SSPI";
			string commandText = @"
				SELECT 
					p.CategoryID,
					c.CategoryName,
					Count(p.CategoryID) Count,
					NULL   NullValue
				FROM Products p
					INNER JOIN Categories c ON c.CategoryID = p.CategoryID
				GROUP BY p.CategoryID, c.CategoryName
				--HAVING Count(p.CategoryID) >= @min
				ORDER BY c.CategoryName";

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();

				for (int i = 0; i < Count; i++)
				{
					using (SqlCommand cmd = new SqlCommand(commandText, con))
					{
#if VER2
						cmd.Parameters.AddWithValue("@min", min);
#else
						cmd.Parameters.Add("@min", min);
#endif
						using (SqlDataReader rd = cmd.ExecuteReader())
						{
							al = new ArrayList();

							while (rd.Read())
							{
								CategoryInfo ci = new CategoryInfo();

								ci.CategoryID   = Convert.ToInt32 (rd["CategoryID"]);
								ci.CategoryName = Convert.ToString(rd["CategoryName"]).TrimEnd();
								ci.Count        = (short)Convert.ToInt32 (rd["Count"]);

								object o = rd["NullValue"];
								ci.NullValue    = o is DBNull? string.Empty: Convert.ToString(o).TrimEnd();

								al.Add(ci);
							}
						}
					}
				}
			}

			return al;
		}

		static ArrayList GetCategories2(int min)
		{
			ArrayList al = null;

			using (DbManager db = new DbManager())
			{
				for (int i = 0; i < Count; i++)
				{
					al = db
						.SetCommand(@"
							SELECT 
								p.CategoryID,
								c.CategoryName,
								Count(p.CategoryID) Count,
								NULL    NullValue
							FROM Products p
								INNER JOIN Categories c ON c.CategoryID = p.CategoryID
							GROUP BY p.CategoryID, c.CategoryName
							--HAVING Count(p.CategoryID) >= @min
							ORDER BY c.CategoryName",
							db.Parameter("@min", min))
						.ExecuteList(typeof(CategoryInfo));
				}
			}

			return al;
		}

		static DataSet GetCategories3(int min)
		{
			DataSet ds = null;

			using (DbManager db = new DbManager())
			{
				for (int i = 0; i < Count; i++)
				{
					ds = db
						.SetCommand(@"
							SELECT 
								p.CategoryID,
								c.CategoryName,
								Count(p.CategoryID) Count,
								NULL    NullValue
							FROM Products p
								INNER JOIN Categories c ON c.CategoryID = p.CategoryID
							GROUP BY p.CategoryID, c.CategoryName
							--HAVING Count(p.CategoryID) >= @min
							ORDER BY c.CategoryName",
							db.Parameter("@min", min))
						.ExecuteDataSet();
				}
			}

			return ds;
		}

		static char[] trimArr = new char[0];

		static ArrayList GetCategories4(int min)
		{
			ArrayList al = null;

			using (DbManager db = new DbManager())
			{
				for (int ii = 0; ii < Count; ii++)
				{
					al = new ArrayList();

					db.SetCommand(@"
						SELECT 
							Cast(p.CategoryID as smallint) CategoryID,
							c.CategoryName,
							Count(p.CategoryID) Count,
							NULL    NullValue
						FROM Products p
							INNER JOIN Categories c ON c.CategoryID = p.CategoryID
						GROUP BY p.CategoryID, c.CategoryName
						--HAVING Count(p.CategoryID) >= @min
						ORDER BY c.CategoryName",
						db.Parameter("@min", min));

					using (IDataReader dr = db.ExecuteReader())
					{
						while (dr.Read())
						{
							CategoryInfo ci = new CategoryInfo();

							for (int i = 0; i < dr.FieldCount; i++)
							{
								object o = dr[i];

								string name = dr.GetName(i);

								switch (name)
								{
									case "CategoryID":      ci.CategoryID   = (short)(o);  break;
									case "CategoryName":    ci.CategoryName = ((string)o).TrimEnd(trimArr); break;
									case "Count":           ci.Count        = (short)(int)(o);  break;
									case "NullValue":       ci.NullValue    = o is DBNull? string.Empty: ((string)o).TrimEnd(trimArr); break;
									case "NullValue123":    ci.NullValue    = dr.GetString(i); break;
									case "NullValue123456": ci.NullValue    = dr.GetString(i); break;
										//default: dr[i];
								}
							}

							al.Add(ci);
						}
					}
				}
			}

			return al;
		}

		static void Print(ArrayList al)
		{
			foreach (CategoryInfo ci in al)
			{
				Console.WriteLine("{0}\t{1}\t{2}\t{3}.", ci.CategoryID, ci.CategoryName, ci.Count, ci.NullValue);
			}
		}

		static void Print(DataSet dataSet)
		{
			foreach (DataRow dr in dataSet.Tables[0].Rows)
			{
				Console.WriteLine("{0}\t{1}\t{2}\t{3}.", 
					dr["CategoryID"],
					dr["CategoryName"],
					dr["Count"],
					dr["NullValue"]);
			}
		}

		public class Region
		{
			public Region (int id, string desc)
			{
				ID          = id;
				Description = desc;
			}

			public int    ID;
			public string Description;
		}

		static void InsertRegionList(Region[] regionList)
		{
			using (DbManager db = new DbManager())
			{
				db
					.SetCommand(@"
						INSERT INTO Region (
							RegionID,
							RegionDescription
						) VALUES (
							@ID,
							@Description
						)",
						//db.Parameter("@ID",          DbType.Int32),
						//db.Parameter("@Description", DbType.String));
						db.CreateParameters(regionList[0]))
					.Prepare();

				foreach (Region r in regionList)
				{
					//db.Parameter("@ID").Value          = r.ID;
					//db.Parameter("@Description").Value = r.Description;
					//db.Parameter("@Description").Size  = r.Description.Length;

					db.AssignParameterValues(r);
					db.ExecuteNonQuery();
				}
			}
		}

		static void InsertRegionTable(DataTable dataTable)
		{
			using (DbManager db = new DbManager())
			{
				db
					.SetCommand(@"
						INSERT INTO Region (
							RegionID,
							RegionDescription
						) VALUES (
							@ID,
							@Description
						)",
						db.CreateParameters(dataTable.Rows[0]))
					.Prepare();

				foreach (DataRow dr in dataTable.Rows)
				{
					db.AssignParameterValues(dr);
					db.ExecuteNonQuery();
				}
			}
		}

		static void InsertRegion(int id, string description)
		{
			using (DbManager db = new DbManager())
			{
				db
					.SetCommand(@"
						INSERT INTO Region (
							RegionID,
							RegionDescription
						) VALUES (
							@id,
							@desc
						)",
						db.Parameter("@id",   id),
						db.Parameter("@desc", description))
					.ExecuteNonQuery();
			}
		}

		static DataSet SalesByCategory(string categoryName, string ordYear)
		{
			using (DbManager   db = new DbManager())
			using (IDataReader dr = db.SetSpCommand("SalesByCategory", categoryName, ordYear).ExecuteReader())
			{
				while (dr.Read())
				{
					// ...
				}

				DataSet dataSet = db.SetSpCommand("SalesByCategory", categoryName, ordYear).ExecuteDataSet();

				int returnValue = (int)db.Parameter("@RETURN_VALUE").Value;

				if (returnValue != 0)
				{
					throw new Exception(string.Format("Return value is '{0}'", returnValue));
				}

				return dataSet;
			}
		}

		[MapValue(State.Active,   "A")]
		[MapValue(State.Inactive, "I")]
		[MapValue(State.Pending,  "P")]
		[MapValue(State.Active,   typeof(DBNull))]
		[MapValue(State.Active)]
		public enum State
		{
			Active,
			Inactive,
			Pending
		}

		public class RecordHeader
		{
			public int    ID;
			public string Name;
		}

		public class Category
		{
			public RecordHeader Header = new RecordHeader();
			public string       Description;
		}

		static ArrayList GetAllCategories()
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand(@"
						SELECT
							CategoryID   as [Header.ID],
							CategoryName as [Header.Name],
							Description
						FROM Categories")
					.ExecuteList(typeof(Category));
			}
		}

		static void Main()
		{
			DbManager.AddConnectionString("Server=(local);Database=Northwind;Integrated Security=SSPI");

			GetCategories1(10);
			GetCategories2(10);
			GetCategories3(10);
			GetCategories4(10);

			DateTime start;

			start = DateTime.Now;
			ArrayList al = GetCategories1(10);
			Console.WriteLine("{0}", DateTime.Now - start);
			Print(al);

			start = DateTime.Now;
			al = GetCategories2(10);
			Console.WriteLine("{0}", DateTime.Now - start);
			Print(al);

			start = DateTime.Now;
			DataSet ds = GetCategories3(10);
			Console.WriteLine("{0}", DateTime.Now - start);
			Print(ds);

			start = DateTime.Now;
			al = GetCategories4(10);
			Console.WriteLine("{0}", DateTime.Now - start);
			Print(al);

			//InsertRegionList(new Region[] { new Region( 26, "26"), new Region(27, "71717")});
		}
	}
}

/*
using System;
using System.Data;
using System.Collections;
 
using Rsdn.Framework.Data;
using Rsdn.Framework.Data.DataProvider;

namespace Example
{
	public class Category
	{
		[MapField(Name = "CategoryID")]
		public int    ID;

		public string CategoryName;

		[MapField(IsNullable = true)]
		public string Description;

		public int Prop
		{
			get 
			{
				return ID;
			}
		}
	}

	class Test
	{
		static void Main()
		{
			using (DbManager   db = new DbManager())
			using (IDataReader dr = db.ExecuteReader(@"
			    SELECT
					CategoryID,
					CategoryName,
					Description
				FROM Categories

			    SELECT
					CategoryID,
					CategoryName
				FROM Categories"))
			{
				ArrayList al = MapData.MapList(dr, typeof(Category));

				Print(al);

				if (dr.NextResult())
				{
					MapData.MapList(dr, al, typeof(Category));

					Print(al);
				}
			}
		}

		static void Print(ArrayList al)
		{
			foreach (Category c in al)
			{
				Console.WriteLine(
					"{0,2}  {1,-15} {2}", c.ID, c.CategoryName, c.Description);
			}
		}
	}

	class TestDescriptor : Rsdn.Framework.Data.Mapping.MapDescriptor
	{
		public object GetValue(object obj)
		{
			return ((Category)obj).Prop;
		}
	}
}
*/

/*
using System;

using Rsdn.Framework.Data.Mapping;

namespace Test
{
	public class Target
	{
		public class InnerString
		{
			private string _value;
			public  string  Value
			{
				get { return _value;  }
				set { _value = value; }
			}
		}

		public class InnerInt32
		{
			private int _value;
			public  int  Value
			{
				get { return _value;  }
				set { _value = value; }
			}
		}

		[MapType(typeof(char),    typeof(InnerString))]
		[MapType(typeof(short),   typeof(InnerString))]
		[MapType(typeof(int),     typeof(InnerInt32))]
		[MapType(typeof(long),    typeof(InnerString))]
		[MapType(typeof(byte),    typeof(InnerString))]
		[MapType(typeof(double),  typeof(InnerString))]
		[MapType(typeof(string),  typeof(InnerString))]
		[MapType(typeof(decimal), typeof(InnerString))]
		[MapType(typeof(float),   typeof(InnerString))]
		[MapType(typeof(string),  typeof(InnerString))]
		[MapType(typeof(string),  typeof(InnerString))]
		public class Base1
		{
		}

		public class Base2: Base1
		{
		}

		public abstract class Class01 : Base2
		{
			public abstract string Prop00 { get; set; } public abstract int Prop01 { get; set; } public abstract string Prop02 { get; set; } public abstract string Prop03 { get; set; } public abstract string Prop04 { get; set; } public abstract string Prop05 { get; set; } public abstract string Prop06 { get; set; } public abstract string Prop07 { get; set; } public abstract string Prop08 { get; set; } public abstract string Prop09 { get; set; }
			public abstract string Prop10 { get; set; } public abstract int Prop11 { get; set; } public abstract string Prop12 { get; set; } public abstract string Prop13 { get; set; } public abstract string Prop14 { get; set; } public abstract string Prop15 { get; set; } public abstract string Prop16 { get; set; } public abstract string Prop17 { get; set; } public abstract string Prop18 { get; set; } public abstract string Prop19 { get; set; }
			public abstract string Prop20 { get; set; } public abstract int Prop21 { get; set; } public abstract string Prop22 { get; set; } public abstract string Prop23 { get; set; } public abstract string Prop24 { get; set; } public abstract string Prop25 { get; set; } public abstract string Prop26 { get; set; } public abstract string Prop27 { get; set; } public abstract string Prop28 { get; set; } public abstract string Prop29 { get; set; }
			public abstract string Prop30 { get; set; } public abstract int Prop31 { get; set; } public abstract string Prop32 { get; set; } public abstract string Prop33 { get; set; } public abstract string Prop34 { get; set; } public abstract string Prop35 { get; set; } public abstract string Prop36 { get; set; } public abstract string Prop37 { get; set; } public abstract string Prop38 { get; set; } public abstract string Prop39 { get; set; }
			public abstract string Prop40 { get; set; } public abstract int Prop41 { get; set; } public abstract string Prop42 { get; set; } public abstract string Prop43 { get; set; } public abstract string Prop44 { get; set; } public abstract string Prop45 { get; set; } public abstract string Prop46 { get; set; } public abstract string Prop47 { get; set; } public abstract string Prop48 { get; set; } public abstract string Prop49 { get; set; }
			public abstract string Prop50 { get; set; } public abstract int Prop51 { get; set; } public abstract string Prop52 { get; set; } public abstract string Prop53 { get; set; } public abstract string Prop54 { get; set; } public abstract string Prop55 { get; set; } public abstract string Prop56 { get; set; } public abstract string Prop57 { get; set; } public abstract string Prop58 { get; set; } public abstract string Prop59 { get; set; }
			public abstract string Prop60 { get; set; } public abstract int Prop61 { get; set; } public abstract string Prop62 { get; set; } public abstract string Prop63 { get; set; } public abstract string Prop64 { get; set; } public abstract string Prop65 { get; set; } public abstract string Prop66 { get; set; } public abstract string Prop67 { get; set; } public abstract string Prop68 { get; set; } public abstract string Prop69 { get; set; }
			public abstract string Prop70 { get; set; } public abstract int Prop71 { get; set; } public abstract string Prop72 { get; set; } public abstract string Prop73 { get; set; } public abstract string Prop74 { get; set; } public abstract string Prop75 { get; set; } public abstract string Prop76 { get; set; } public abstract string Prop77 { get; set; } public abstract string Prop78 { get; set; } public abstract string Prop79 { get; set; }
			public abstract string Prop80 { get; set; } public abstract int Prop81 { get; set; } public abstract string Prop82 { get; set; } public abstract string Prop83 { get; set; } public abstract string Prop84 { get; set; } public abstract string Prop85 { get; set; } public abstract string Prop86 { get; set; } public abstract string Prop87 { get; set; } public abstract string Prop88 { get; set; } public abstract string Prop89 { get; set; }
			public abstract string Prop90 { get; set; } public abstract int Prop91 { get; set; } public abstract string Prop92 { get; set; } public abstract string Prop93 { get; set; } public abstract string Prop94 { get; set; } public abstract string Prop95 { get; set; } public abstract string Prop96 { get; set; } public abstract string Prop97 { get; set; } public abstract string Prop98 { get; set; } public abstract string Prop99 { get; set; }
		}

		public abstract class Class02 : Base2
		{
			public abstract string Prop00 { get; set; } public abstract int Prop01 { get; set; } public abstract string Prop02 { get; set; } public abstract string Prop03 { get; set; } public abstract string Prop04 { get; set; } public abstract string Prop05 { get; set; } public abstract string Prop06 { get; set; } public abstract string Prop07 { get; set; } public abstract string Prop08 { get; set; } public abstract string Prop09 { get; set; }
			public abstract string Prop10 { get; set; } public abstract int Prop11 { get; set; } public abstract string Prop12 { get; set; } public abstract string Prop13 { get; set; } public abstract string Prop14 { get; set; } public abstract string Prop15 { get; set; } public abstract string Prop16 { get; set; } public abstract string Prop17 { get; set; } public abstract string Prop18 { get; set; } public abstract string Prop19 { get; set; }
			public abstract string Prop20 { get; set; } public abstract int Prop21 { get; set; } public abstract string Prop22 { get; set; } public abstract string Prop23 { get; set; } public abstract string Prop24 { get; set; } public abstract string Prop25 { get; set; } public abstract string Prop26 { get; set; } public abstract string Prop27 { get; set; } public abstract string Prop28 { get; set; } public abstract string Prop29 { get; set; }
			public abstract string Prop30 { get; set; } public abstract int Prop31 { get; set; } public abstract string Prop32 { get; set; } public abstract string Prop33 { get; set; } public abstract string Prop34 { get; set; } public abstract string Prop35 { get; set; } public abstract string Prop36 { get; set; } public abstract string Prop37 { get; set; } public abstract string Prop38 { get; set; } public abstract string Prop39 { get; set; }
			public abstract string Prop40 { get; set; } public abstract int Prop41 { get; set; } public abstract string Prop42 { get; set; } public abstract string Prop43 { get; set; } public abstract string Prop44 { get; set; } public abstract string Prop45 { get; set; } public abstract string Prop46 { get; set; } public abstract string Prop47 { get; set; } public abstract string Prop48 { get; set; } public abstract string Prop49 { get; set; }
			public abstract string Prop50 { get; set; } public abstract int Prop51 { get; set; } public abstract string Prop52 { get; set; } public abstract string Prop53 { get; set; } public abstract string Prop54 { get; set; } public abstract string Prop55 { get; set; } public abstract string Prop56 { get; set; } public abstract string Prop57 { get; set; } public abstract string Prop58 { get; set; } public abstract string Prop59 { get; set; }
			public abstract string Prop60 { get; set; } public abstract int Prop61 { get; set; } public abstract string Prop62 { get; set; } public abstract string Prop63 { get; set; } public abstract string Prop64 { get; set; } public abstract string Prop65 { get; set; } public abstract string Prop66 { get; set; } public abstract string Prop67 { get; set; } public abstract string Prop68 { get; set; } public abstract string Prop69 { get; set; }
			public abstract string Prop70 { get; set; } public abstract int Prop71 { get; set; } public abstract string Prop72 { get; set; } public abstract string Prop73 { get; set; } public abstract string Prop74 { get; set; } public abstract string Prop75 { get; set; } public abstract string Prop76 { get; set; } public abstract string Prop77 { get; set; } public abstract string Prop78 { get; set; } public abstract string Prop79 { get; set; }
			public abstract string Prop80 { get; set; } public abstract int Prop81 { get; set; } public abstract string Prop82 { get; set; } public abstract string Prop83 { get; set; } public abstract string Prop84 { get; set; } public abstract string Prop85 { get; set; } public abstract string Prop86 { get; set; } public abstract string Prop87 { get; set; } public abstract string Prop88 { get; set; } public abstract string Prop89 { get; set; }
			public abstract string Prop90 { get; set; } public abstract int Prop91 { get; set; } public abstract string Prop92 { get; set; } public abstract string Prop93 { get; set; } public abstract string Prop94 { get; set; } public abstract string Prop95 { get; set; } public abstract string Prop96 { get; set; } public abstract string Prop97 { get; set; } public abstract string Prop98 { get; set; } public abstract string Prop99 { get; set; }
		}

		public abstract class Class03 : Base2
		{
			public abstract string Prop00 { get; set; } public abstract int Prop01 { get; set; } public abstract string Prop02 { get; set; } public abstract string Prop03 { get; set; } public abstract string Prop04 { get; set; } public abstract string Prop05 { get; set; } public abstract string Prop06 { get; set; } public abstract string Prop07 { get; set; } public abstract string Prop08 { get; set; } public abstract string Prop09 { get; set; }
			public abstract string Prop10 { get; set; } public abstract int Prop11 { get; set; } public abstract string Prop12 { get; set; } public abstract string Prop13 { get; set; } public abstract string Prop14 { get; set; } public abstract string Prop15 { get; set; } public abstract string Prop16 { get; set; } public abstract string Prop17 { get; set; } public abstract string Prop18 { get; set; } public abstract string Prop19 { get; set; }
			public abstract string Prop20 { get; set; } public abstract int Prop21 { get; set; } public abstract string Prop22 { get; set; } public abstract string Prop23 { get; set; } public abstract string Prop24 { get; set; } public abstract string Prop25 { get; set; } public abstract string Prop26 { get; set; } public abstract string Prop27 { get; set; } public abstract string Prop28 { get; set; } public abstract string Prop29 { get; set; }
			public abstract string Prop30 { get; set; } public abstract int Prop31 { get; set; } public abstract string Prop32 { get; set; } public abstract string Prop33 { get; set; } public abstract string Prop34 { get; set; } public abstract string Prop35 { get; set; } public abstract string Prop36 { get; set; } public abstract string Prop37 { get; set; } public abstract string Prop38 { get; set; } public abstract string Prop39 { get; set; }
			public abstract string Prop40 { get; set; } public abstract int Prop41 { get; set; } public abstract string Prop42 { get; set; } public abstract string Prop43 { get; set; } public abstract string Prop44 { get; set; } public abstract string Prop45 { get; set; } public abstract string Prop46 { get; set; } public abstract string Prop47 { get; set; } public abstract string Prop48 { get; set; } public abstract string Prop49 { get; set; }
			public abstract string Prop50 { get; set; } public abstract int Prop51 { get; set; } public abstract string Prop52 { get; set; } public abstract string Prop53 { get; set; } public abstract string Prop54 { get; set; } public abstract string Prop55 { get; set; } public abstract string Prop56 { get; set; } public abstract string Prop57 { get; set; } public abstract string Prop58 { get; set; } public abstract string Prop59 { get; set; }
			public abstract string Prop60 { get; set; } public abstract int Prop61 { get; set; } public abstract string Prop62 { get; set; } public abstract string Prop63 { get; set; } public abstract string Prop64 { get; set; } public abstract string Prop65 { get; set; } public abstract string Prop66 { get; set; } public abstract string Prop67 { get; set; } public abstract string Prop68 { get; set; } public abstract string Prop69 { get; set; }
			public abstract string Prop70 { get; set; } public abstract int Prop71 { get; set; } public abstract string Prop72 { get; set; } public abstract string Prop73 { get; set; } public abstract string Prop74 { get; set; } public abstract string Prop75 { get; set; } public abstract string Prop76 { get; set; } public abstract string Prop77 { get; set; } public abstract string Prop78 { get; set; } public abstract string Prop79 { get; set; }
			public abstract string Prop80 { get; set; } public abstract int Prop81 { get; set; } public abstract string Prop82 { get; set; } public abstract string Prop83 { get; set; } public abstract string Prop84 { get; set; } public abstract string Prop85 { get; set; } public abstract string Prop86 { get; set; } public abstract string Prop87 { get; set; } public abstract string Prop88 { get; set; } public abstract string Prop89 { get; set; }
			public abstract string Prop90 { get; set; } public abstract int Prop91 { get; set; } public abstract string Prop92 { get; set; } public abstract string Prop93 { get; set; } public abstract string Prop94 { get; set; } public abstract string Prop95 { get; set; } public abstract string Prop96 { get; set; } public abstract string Prop97 { get; set; } public abstract string Prop98 { get; set; } public abstract string Prop99 { get; set; }
		}

		public abstract class Class04 : Base2
		{
			public abstract string Prop00 { get; set; } public abstract int Prop01 { get; set; } public abstract string Prop02 { get; set; } public abstract string Prop03 { get; set; } public abstract string Prop04 { get; set; } public abstract string Prop05 { get; set; } public abstract string Prop06 { get; set; } public abstract string Prop07 { get; set; } public abstract string Prop08 { get; set; } public abstract string Prop09 { get; set; }
			public abstract string Prop10 { get; set; } public abstract int Prop11 { get; set; } public abstract string Prop12 { get; set; } public abstract string Prop13 { get; set; } public abstract string Prop14 { get; set; } public abstract string Prop15 { get; set; } public abstract string Prop16 { get; set; } public abstract string Prop17 { get; set; } public abstract string Prop18 { get; set; } public abstract string Prop19 { get; set; }
			public abstract string Prop20 { get; set; } public abstract int Prop21 { get; set; } public abstract string Prop22 { get; set; } public abstract string Prop23 { get; set; } public abstract string Prop24 { get; set; } public abstract string Prop25 { get; set; } public abstract string Prop26 { get; set; } public abstract string Prop27 { get; set; } public abstract string Prop28 { get; set; } public abstract string Prop29 { get; set; }
			public abstract string Prop30 { get; set; } public abstract int Prop31 { get; set; } public abstract string Prop32 { get; set; } public abstract string Prop33 { get; set; } public abstract string Prop34 { get; set; } public abstract string Prop35 { get; set; } public abstract string Prop36 { get; set; } public abstract string Prop37 { get; set; } public abstract string Prop38 { get; set; } public abstract string Prop39 { get; set; }
			public abstract string Prop40 { get; set; } public abstract int Prop41 { get; set; } public abstract string Prop42 { get; set; } public abstract string Prop43 { get; set; } public abstract string Prop44 { get; set; } public abstract string Prop45 { get; set; } public abstract string Prop46 { get; set; } public abstract string Prop47 { get; set; } public abstract string Prop48 { get; set; } public abstract string Prop49 { get; set; }
			public abstract string Prop50 { get; set; } public abstract int Prop51 { get; set; } public abstract string Prop52 { get; set; } public abstract string Prop53 { get; set; } public abstract string Prop54 { get; set; } public abstract string Prop55 { get; set; } public abstract string Prop56 { get; set; } public abstract string Prop57 { get; set; } public abstract string Prop58 { get; set; } public abstract string Prop59 { get; set; }
			public abstract string Prop60 { get; set; } public abstract int Prop61 { get; set; } public abstract string Prop62 { get; set; } public abstract string Prop63 { get; set; } public abstract string Prop64 { get; set; } public abstract string Prop65 { get; set; } public abstract string Prop66 { get; set; } public abstract string Prop67 { get; set; } public abstract string Prop68 { get; set; } public abstract string Prop69 { get; set; }
			public abstract string Prop70 { get; set; } public abstract int Prop71 { get; set; } public abstract string Prop72 { get; set; } public abstract string Prop73 { get; set; } public abstract string Prop74 { get; set; } public abstract string Prop75 { get; set; } public abstract string Prop76 { get; set; } public abstract string Prop77 { get; set; } public abstract string Prop78 { get; set; } public abstract string Prop79 { get; set; }
			public abstract string Prop80 { get; set; } public abstract int Prop81 { get; set; } public abstract string Prop82 { get; set; } public abstract string Prop83 { get; set; } public abstract string Prop84 { get; set; } public abstract string Prop85 { get; set; } public abstract string Prop86 { get; set; } public abstract string Prop87 { get; set; } public abstract string Prop88 { get; set; } public abstract string Prop89 { get; set; }
			public abstract string Prop90 { get; set; } public abstract int Prop91 { get; set; } public abstract string Prop92 { get; set; } public abstract string Prop93 { get; set; } public abstract string Prop94 { get; set; } public abstract string Prop95 { get; set; } public abstract string Prop96 { get; set; } public abstract string Prop97 { get; set; } public abstract string Prop98 { get; set; } public abstract string Prop99 { get; set; }
		}

		public abstract class Class05 : Base2
		{
			public abstract string Prop00 { get; set; } public abstract int Prop01 { get; set; } public abstract string Prop02 { get; set; } public abstract string Prop03 { get; set; } public abstract string Prop04 { get; set; } public abstract string Prop05 { get; set; } public abstract string Prop06 { get; set; } public abstract string Prop07 { get; set; } public abstract string Prop08 { get; set; } public abstract string Prop09 { get; set; }
			public abstract string Prop10 { get; set; } public abstract int Prop11 { get; set; } public abstract string Prop12 { get; set; } public abstract string Prop13 { get; set; } public abstract string Prop14 { get; set; } public abstract string Prop15 { get; set; } public abstract string Prop16 { get; set; } public abstract string Prop17 { get; set; } public abstract string Prop18 { get; set; } public abstract string Prop19 { get; set; }
			public abstract string Prop20 { get; set; } public abstract int Prop21 { get; set; } public abstract string Prop22 { get; set; } public abstract string Prop23 { get; set; } public abstract string Prop24 { get; set; } public abstract string Prop25 { get; set; } public abstract string Prop26 { get; set; } public abstract string Prop27 { get; set; } public abstract string Prop28 { get; set; } public abstract string Prop29 { get; set; }
			public abstract string Prop30 { get; set; } public abstract int Prop31 { get; set; } public abstract string Prop32 { get; set; } public abstract string Prop33 { get; set; } public abstract string Prop34 { get; set; } public abstract string Prop35 { get; set; } public abstract string Prop36 { get; set; } public abstract string Prop37 { get; set; } public abstract string Prop38 { get; set; } public abstract string Prop39 { get; set; }
			public abstract string Prop40 { get; set; } public abstract int Prop41 { get; set; } public abstract string Prop42 { get; set; } public abstract string Prop43 { get; set; } public abstract string Prop44 { get; set; } public abstract string Prop45 { get; set; } public abstract string Prop46 { get; set; } public abstract string Prop47 { get; set; } public abstract string Prop48 { get; set; } public abstract string Prop49 { get; set; }
			public abstract string Prop50 { get; set; } public abstract int Prop51 { get; set; } public abstract string Prop52 { get; set; } public abstract string Prop53 { get; set; } public abstract string Prop54 { get; set; } public abstract string Prop55 { get; set; } public abstract string Prop56 { get; set; } public abstract string Prop57 { get; set; } public abstract string Prop58 { get; set; } public abstract string Prop59 { get; set; }
			public abstract string Prop60 { get; set; } public abstract int Prop61 { get; set; } public abstract string Prop62 { get; set; } public abstract string Prop63 { get; set; } public abstract string Prop64 { get; set; } public abstract string Prop65 { get; set; } public abstract string Prop66 { get; set; } public abstract string Prop67 { get; set; } public abstract string Prop68 { get; set; } public abstract string Prop69 { get; set; }
			public abstract string Prop70 { get; set; } public abstract int Prop71 { get; set; } public abstract string Prop72 { get; set; } public abstract string Prop73 { get; set; } public abstract string Prop74 { get; set; } public abstract string Prop75 { get; set; } public abstract string Prop76 { get; set; } public abstract string Prop77 { get; set; } public abstract string Prop78 { get; set; } public abstract string Prop79 { get; set; }
			public abstract string Prop80 { get; set; } public abstract int Prop81 { get; set; } public abstract string Prop82 { get; set; } public abstract string Prop83 { get; set; } public abstract string Prop84 { get; set; } public abstract string Prop85 { get; set; } public abstract string Prop86 { get; set; } public abstract string Prop87 { get; set; } public abstract string Prop88 { get; set; } public abstract string Prop89 { get; set; }
			public abstract string Prop90 { get; set; } public abstract int Prop91 { get; set; } public abstract string Prop92 { get; set; } public abstract string Prop93 { get; set; } public abstract string Prop94 { get; set; } public abstract string Prop95 { get; set; } public abstract string Prop96 { get; set; } public abstract string Prop97 { get; set; } public abstract string Prop98 { get; set; } public abstract string Prop99 { get; set; }
		}

		public abstract class Class06 : Base2
		{
			public abstract string Prop00 { get; set; } public abstract int Prop01 { get; set; } public abstract string Prop02 { get; set; } public abstract string Prop03 { get; set; } public abstract string Prop04 { get; set; } public abstract string Prop05 { get; set; } public abstract string Prop06 { get; set; } public abstract string Prop07 { get; set; } public abstract string Prop08 { get; set; } public abstract string Prop09 { get; set; }
			public abstract string Prop10 { get; set; } public abstract int Prop11 { get; set; } public abstract string Prop12 { get; set; } public abstract string Prop13 { get; set; } public abstract string Prop14 { get; set; } public abstract string Prop15 { get; set; } public abstract string Prop16 { get; set; } public abstract string Prop17 { get; set; } public abstract string Prop18 { get; set; } public abstract string Prop19 { get; set; }
			public abstract string Prop20 { get; set; } public abstract int Prop21 { get; set; } public abstract string Prop22 { get; set; } public abstract string Prop23 { get; set; } public abstract string Prop24 { get; set; } public abstract string Prop25 { get; set; } public abstract string Prop26 { get; set; } public abstract string Prop27 { get; set; } public abstract string Prop28 { get; set; } public abstract string Prop29 { get; set; }
			public abstract string Prop30 { get; set; } public abstract int Prop31 { get; set; } public abstract string Prop32 { get; set; } public abstract string Prop33 { get; set; } public abstract string Prop34 { get; set; } public abstract string Prop35 { get; set; } public abstract string Prop36 { get; set; } public abstract string Prop37 { get; set; } public abstract string Prop38 { get; set; } public abstract string Prop39 { get; set; }
			public abstract string Prop40 { get; set; } public abstract int Prop41 { get; set; } public abstract string Prop42 { get; set; } public abstract string Prop43 { get; set; } public abstract string Prop44 { get; set; } public abstract string Prop45 { get; set; } public abstract string Prop46 { get; set; } public abstract string Prop47 { get; set; } public abstract string Prop48 { get; set; } public abstract string Prop49 { get; set; }
			public abstract string Prop50 { get; set; } public abstract int Prop51 { get; set; } public abstract string Prop52 { get; set; } public abstract string Prop53 { get; set; } public abstract string Prop54 { get; set; } public abstract string Prop55 { get; set; } public abstract string Prop56 { get; set; } public abstract string Prop57 { get; set; } public abstract string Prop58 { get; set; } public abstract string Prop59 { get; set; }
			public abstract string Prop60 { get; set; } public abstract int Prop61 { get; set; } public abstract string Prop62 { get; set; } public abstract string Prop63 { get; set; } public abstract string Prop64 { get; set; } public abstract string Prop65 { get; set; } public abstract string Prop66 { get; set; } public abstract string Prop67 { get; set; } public abstract string Prop68 { get; set; } public abstract string Prop69 { get; set; }
			public abstract string Prop70 { get; set; } public abstract int Prop71 { get; set; } public abstract string Prop72 { get; set; } public abstract string Prop73 { get; set; } public abstract string Prop74 { get; set; } public abstract string Prop75 { get; set; } public abstract string Prop76 { get; set; } public abstract string Prop77 { get; set; } public abstract string Prop78 { get; set; } public abstract string Prop79 { get; set; }
			public abstract string Prop80 { get; set; } public abstract int Prop81 { get; set; } public abstract string Prop82 { get; set; } public abstract string Prop83 { get; set; } public abstract string Prop84 { get; set; } public abstract string Prop85 { get; set; } public abstract string Prop86 { get; set; } public abstract string Prop87 { get; set; } public abstract string Prop88 { get; set; } public abstract string Prop89 { get; set; }
			public abstract string Prop90 { get; set; } public abstract int Prop91 { get; set; } public abstract string Prop92 { get; set; } public abstract string Prop93 { get; set; } public abstract string Prop94 { get; set; } public abstract string Prop95 { get; set; } public abstract string Prop96 { get; set; } public abstract string Prop97 { get; set; } public abstract string Prop98 { get; set; } public abstract string Prop99 { get; set; }
		}

		public abstract class Class07 : Base2
		{
			public abstract string Prop00 { get; set; } public abstract int Prop01 { get; set; } public abstract string Prop02 { get; set; } public abstract string Prop03 { get; set; } public abstract string Prop04 { get; set; } public abstract string Prop05 { get; set; } public abstract string Prop06 { get; set; } public abstract string Prop07 { get; set; } public abstract string Prop08 { get; set; } public abstract string Prop09 { get; set; }
			public abstract string Prop10 { get; set; } public abstract int Prop11 { get; set; } public abstract string Prop12 { get; set; } public abstract string Prop13 { get; set; } public abstract string Prop14 { get; set; } public abstract string Prop15 { get; set; } public abstract string Prop16 { get; set; } public abstract string Prop17 { get; set; } public abstract string Prop18 { get; set; } public abstract string Prop19 { get; set; }
			public abstract string Prop20 { get; set; } public abstract int Prop21 { get; set; } public abstract string Prop22 { get; set; } public abstract string Prop23 { get; set; } public abstract string Prop24 { get; set; } public abstract string Prop25 { get; set; } public abstract string Prop26 { get; set; } public abstract string Prop27 { get; set; } public abstract string Prop28 { get; set; } public abstract string Prop29 { get; set; }
			public abstract string Prop30 { get; set; } public abstract int Prop31 { get; set; } public abstract string Prop32 { get; set; } public abstract string Prop33 { get; set; } public abstract string Prop34 { get; set; } public abstract string Prop35 { get; set; } public abstract string Prop36 { get; set; } public abstract string Prop37 { get; set; } public abstract string Prop38 { get; set; } public abstract string Prop39 { get; set; }
			public abstract string Prop40 { get; set; } public abstract int Prop41 { get; set; } public abstract string Prop42 { get; set; } public abstract string Prop43 { get; set; } public abstract string Prop44 { get; set; } public abstract string Prop45 { get; set; } public abstract string Prop46 { get; set; } public abstract string Prop47 { get; set; } public abstract string Prop48 { get; set; } public abstract string Prop49 { get; set; }
			public abstract string Prop50 { get; set; } public abstract int Prop51 { get; set; } public abstract string Prop52 { get; set; } public abstract string Prop53 { get; set; } public abstract string Prop54 { get; set; } public abstract string Prop55 { get; set; } public abstract string Prop56 { get; set; } public abstract string Prop57 { get; set; } public abstract string Prop58 { get; set; } public abstract string Prop59 { get; set; }
			public abstract string Prop60 { get; set; } public abstract int Prop61 { get; set; } public abstract string Prop62 { get; set; } public abstract string Prop63 { get; set; } public abstract string Prop64 { get; set; } public abstract string Prop65 { get; set; } public abstract string Prop66 { get; set; } public abstract string Prop67 { get; set; } public abstract string Prop68 { get; set; } public abstract string Prop69 { get; set; }
			public abstract string Prop70 { get; set; } public abstract int Prop71 { get; set; } public abstract string Prop72 { get; set; } public abstract string Prop73 { get; set; } public abstract string Prop74 { get; set; } public abstract string Prop75 { get; set; } public abstract string Prop76 { get; set; } public abstract string Prop77 { get; set; } public abstract string Prop78 { get; set; } public abstract string Prop79 { get; set; }
			public abstract string Prop80 { get; set; } public abstract int Prop81 { get; set; } public abstract string Prop82 { get; set; } public abstract string Prop83 { get; set; } public abstract string Prop84 { get; set; } public abstract string Prop85 { get; set; } public abstract string Prop86 { get; set; } public abstract string Prop87 { get; set; } public abstract string Prop88 { get; set; } public abstract string Prop89 { get; set; }
			public abstract string Prop90 { get; set; } public abstract int Prop91 { get; set; } public abstract string Prop92 { get; set; } public abstract string Prop93 { get; set; } public abstract string Prop94 { get; set; } public abstract string Prop95 { get; set; } public abstract string Prop96 { get; set; } public abstract string Prop97 { get; set; } public abstract string Prop98 { get; set; } public abstract string Prop99 { get; set; }
		}

		public abstract class Class08 : Base2
		{
			public abstract string Prop00 { get; set; } public abstract int Prop01 { get; set; } public abstract string Prop02 { get; set; } public abstract string Prop03 { get; set; } public abstract string Prop04 { get; set; } public abstract string Prop05 { get; set; } public abstract string Prop06 { get; set; } public abstract string Prop07 { get; set; } public abstract string Prop08 { get; set; } public abstract string Prop09 { get; set; }
			public abstract string Prop10 { get; set; } public abstract int Prop11 { get; set; } public abstract string Prop12 { get; set; } public abstract string Prop13 { get; set; } public abstract string Prop14 { get; set; } public abstract string Prop15 { get; set; } public abstract string Prop16 { get; set; } public abstract string Prop17 { get; set; } public abstract string Prop18 { get; set; } public abstract string Prop19 { get; set; }
			public abstract string Prop20 { get; set; } public abstract int Prop21 { get; set; } public abstract string Prop22 { get; set; } public abstract string Prop23 { get; set; } public abstract string Prop24 { get; set; } public abstract string Prop25 { get; set; } public abstract string Prop26 { get; set; } public abstract string Prop27 { get; set; } public abstract string Prop28 { get; set; } public abstract string Prop29 { get; set; }
			public abstract string Prop30 { get; set; } public abstract int Prop31 { get; set; } public abstract string Prop32 { get; set; } public abstract string Prop33 { get; set; } public abstract string Prop34 { get; set; } public abstract string Prop35 { get; set; } public abstract string Prop36 { get; set; } public abstract string Prop37 { get; set; } public abstract string Prop38 { get; set; } public abstract string Prop39 { get; set; }
			public abstract string Prop40 { get; set; } public abstract int Prop41 { get; set; } public abstract string Prop42 { get; set; } public abstract string Prop43 { get; set; } public abstract string Prop44 { get; set; } public abstract string Prop45 { get; set; } public abstract string Prop46 { get; set; } public abstract string Prop47 { get; set; } public abstract string Prop48 { get; set; } public abstract string Prop49 { get; set; }
			public abstract string Prop50 { get; set; } public abstract int Prop51 { get; set; } public abstract string Prop52 { get; set; } public abstract string Prop53 { get; set; } public abstract string Prop54 { get; set; } public abstract string Prop55 { get; set; } public abstract string Prop56 { get; set; } public abstract string Prop57 { get; set; } public abstract string Prop58 { get; set; } public abstract string Prop59 { get; set; }
			public abstract string Prop60 { get; set; } public abstract int Prop61 { get; set; } public abstract string Prop62 { get; set; } public abstract string Prop63 { get; set; } public abstract string Prop64 { get; set; } public abstract string Prop65 { get; set; } public abstract string Prop66 { get; set; } public abstract string Prop67 { get; set; } public abstract string Prop68 { get; set; } public abstract string Prop69 { get; set; }
			public abstract string Prop70 { get; set; } public abstract int Prop71 { get; set; } public abstract string Prop72 { get; set; } public abstract string Prop73 { get; set; } public abstract string Prop74 { get; set; } public abstract string Prop75 { get; set; } public abstract string Prop76 { get; set; } public abstract string Prop77 { get; set; } public abstract string Prop78 { get; set; } public abstract string Prop79 { get; set; }
			public abstract string Prop80 { get; set; } public abstract int Prop81 { get; set; } public abstract string Prop82 { get; set; } public abstract string Prop83 { get; set; } public abstract string Prop84 { get; set; } public abstract string Prop85 { get; set; } public abstract string Prop86 { get; set; } public abstract string Prop87 { get; set; } public abstract string Prop88 { get; set; } public abstract string Prop89 { get; set; }
			public abstract string Prop90 { get; set; } public abstract int Prop91 { get; set; } public abstract string Prop92 { get; set; } public abstract string Prop93 { get; set; } public abstract string Prop94 { get; set; } public abstract string Prop95 { get; set; } public abstract string Prop96 { get; set; } public abstract string Prop97 { get; set; } public abstract string Prop98 { get; set; } public abstract string Prop99 { get; set; }
		}

		public abstract class Class09 : Base2
		{
			public abstract string Prop00 { get; set; } public abstract int Prop01 { get; set; } public abstract string Prop02 { get; set; } public abstract string Prop03 { get; set; } public abstract string Prop04 { get; set; } public abstract string Prop05 { get; set; } public abstract string Prop06 { get; set; } public abstract string Prop07 { get; set; } public abstract string Prop08 { get; set; } public abstract string Prop09 { get; set; }
			public abstract string Prop10 { get; set; } public abstract int Prop11 { get; set; } public abstract string Prop12 { get; set; } public abstract string Prop13 { get; set; } public abstract string Prop14 { get; set; } public abstract string Prop15 { get; set; } public abstract string Prop16 { get; set; } public abstract string Prop17 { get; set; } public abstract string Prop18 { get; set; } public abstract string Prop19 { get; set; }
			public abstract string Prop20 { get; set; } public abstract int Prop21 { get; set; } public abstract string Prop22 { get; set; } public abstract string Prop23 { get; set; } public abstract string Prop24 { get; set; } public abstract string Prop25 { get; set; } public abstract string Prop26 { get; set; } public abstract string Prop27 { get; set; } public abstract string Prop28 { get; set; } public abstract string Prop29 { get; set; }
			public abstract string Prop30 { get; set; } public abstract int Prop31 { get; set; } public abstract string Prop32 { get; set; } public abstract string Prop33 { get; set; } public abstract string Prop34 { get; set; } public abstract string Prop35 { get; set; } public abstract string Prop36 { get; set; } public abstract string Prop37 { get; set; } public abstract string Prop38 { get; set; } public abstract string Prop39 { get; set; }
			public abstract string Prop40 { get; set; } public abstract int Prop41 { get; set; } public abstract string Prop42 { get; set; } public abstract string Prop43 { get; set; } public abstract string Prop44 { get; set; } public abstract string Prop45 { get; set; } public abstract string Prop46 { get; set; } public abstract string Prop47 { get; set; } public abstract string Prop48 { get; set; } public abstract string Prop49 { get; set; }
			public abstract string Prop50 { get; set; } public abstract int Prop51 { get; set; } public abstract string Prop52 { get; set; } public abstract string Prop53 { get; set; } public abstract string Prop54 { get; set; } public abstract string Prop55 { get; set; } public abstract string Prop56 { get; set; } public abstract string Prop57 { get; set; } public abstract string Prop58 { get; set; } public abstract string Prop59 { get; set; }
			public abstract string Prop60 { get; set; } public abstract int Prop61 { get; set; } public abstract string Prop62 { get; set; } public abstract string Prop63 { get; set; } public abstract string Prop64 { get; set; } public abstract string Prop65 { get; set; } public abstract string Prop66 { get; set; } public abstract string Prop67 { get; set; } public abstract string Prop68 { get; set; } public abstract string Prop69 { get; set; }
			public abstract string Prop70 { get; set; } public abstract int Prop71 { get; set; } public abstract string Prop72 { get; set; } public abstract string Prop73 { get; set; } public abstract string Prop74 { get; set; } public abstract string Prop75 { get; set; } public abstract string Prop76 { get; set; } public abstract string Prop77 { get; set; } public abstract string Prop78 { get; set; } public abstract string Prop79 { get; set; }
			public abstract string Prop80 { get; set; } public abstract int Prop81 { get; set; } public abstract string Prop82 { get; set; } public abstract string Prop83 { get; set; } public abstract string Prop84 { get; set; } public abstract string Prop85 { get; set; } public abstract string Prop86 { get; set; } public abstract string Prop87 { get; set; } public abstract string Prop88 { get; set; } public abstract string Prop89 { get; set; }
			public abstract string Prop90 { get; set; } public abstract int Prop91 { get; set; } public abstract string Prop92 { get; set; } public abstract string Prop93 { get; set; } public abstract string Prop94 { get; set; } public abstract string Prop95 { get; set; } public abstract string Prop96 { get; set; } public abstract string Prop97 { get; set; } public abstract string Prop98 { get; set; } public abstract string Prop99 { get; set; }
		}

		public abstract class Class10 : Base2
		{
			public abstract string Prop00 { get; set; } public abstract int Prop01 { get; set; } public abstract string Prop02 { get; set; } public abstract string Prop03 { get; set; } public abstract string Prop04 { get; set; } public abstract string Prop05 { get; set; } public abstract string Prop06 { get; set; } public abstract string Prop07 { get; set; } public abstract string Prop08 { get; set; } public abstract string Prop09 { get; set; }
			public abstract string Prop10 { get; set; } public abstract int Prop11 { get; set; } public abstract string Prop12 { get; set; } public abstract string Prop13 { get; set; } public abstract string Prop14 { get; set; } public abstract string Prop15 { get; set; } public abstract string Prop16 { get; set; } public abstract string Prop17 { get; set; } public abstract string Prop18 { get; set; } public abstract string Prop19 { get; set; }
			public abstract string Prop20 { get; set; } public abstract int Prop21 { get; set; } public abstract string Prop22 { get; set; } public abstract string Prop23 { get; set; } public abstract string Prop24 { get; set; } public abstract string Prop25 { get; set; } public abstract string Prop26 { get; set; } public abstract string Prop27 { get; set; } public abstract string Prop28 { get; set; } public abstract string Prop29 { get; set; }
			public abstract string Prop30 { get; set; } public abstract int Prop31 { get; set; } public abstract string Prop32 { get; set; } public abstract string Prop33 { get; set; } public abstract string Prop34 { get; set; } public abstract string Prop35 { get; set; } public abstract string Prop36 { get; set; } public abstract string Prop37 { get; set; } public abstract string Prop38 { get; set; } public abstract string Prop39 { get; set; }
			public abstract string Prop40 { get; set; } public abstract int Prop41 { get; set; } public abstract string Prop42 { get; set; } public abstract string Prop43 { get; set; } public abstract string Prop44 { get; set; } public abstract string Prop45 { get; set; } public abstract string Prop46 { get; set; } public abstract string Prop47 { get; set; } public abstract string Prop48 { get; set; } public abstract string Prop49 { get; set; }
			public abstract string Prop50 { get; set; } public abstract int Prop51 { get; set; } public abstract string Prop52 { get; set; } public abstract string Prop53 { get; set; } public abstract string Prop54 { get; set; } public abstract string Prop55 { get; set; } public abstract string Prop56 { get; set; } public abstract string Prop57 { get; set; } public abstract string Prop58 { get; set; } public abstract string Prop59 { get; set; }
			public abstract string Prop60 { get; set; } public abstract int Prop61 { get; set; } public abstract string Prop62 { get; set; } public abstract string Prop63 { get; set; } public abstract string Prop64 { get; set; } public abstract string Prop65 { get; set; } public abstract string Prop66 { get; set; } public abstract string Prop67 { get; set; } public abstract string Prop68 { get; set; } public abstract string Prop69 { get; set; }
			public abstract string Prop70 { get; set; } public abstract int Prop71 { get; set; } public abstract string Prop72 { get; set; } public abstract string Prop73 { get; set; } public abstract string Prop74 { get; set; } public abstract string Prop75 { get; set; } public abstract string Prop76 { get; set; } public abstract string Prop77 { get; set; } public abstract string Prop78 { get; set; } public abstract string Prop79 { get; set; }
			public abstract string Prop80 { get; set; } public abstract int Prop81 { get; set; } public abstract string Prop82 { get; set; } public abstract string Prop83 { get; set; } public abstract string Prop84 { get; set; } public abstract string Prop85 { get; set; } public abstract string Prop86 { get; set; } public abstract string Prop87 { get; set; } public abstract string Prop88 { get; set; } public abstract string Prop89 { get; set; }
			public abstract string Prop90 { get; set; } public abstract int Prop91 { get; set; } public abstract string Prop92 { get; set; } public abstract string Prop93 { get; set; } public abstract string Prop94 { get; set; } public abstract string Prop95 { get; set; } public abstract string Prop96 { get; set; } public abstract string Prop97 { get; set; } public abstract string Prop98 { get; set; } public abstract string Prop99 { get; set; }
		}

		static void Main()
		{
			Class01 cl01 = (Class01)Map.Descriptor(typeof(Class01)).CreateInstance();
			Class02 cl02 = (Class02)Map.Descriptor(typeof(Class02)).CreateInstance();
			Class03 cl03 = (Class03)Map.Descriptor(typeof(Class03)).CreateInstance();
			Class04 cl04 = (Class04)Map.Descriptor(typeof(Class04)).CreateInstance();
			Class05 cl05 = (Class05)Map.Descriptor(typeof(Class05)).CreateInstance();
			Class06 cl06 = (Class06)Map.Descriptor(typeof(Class06)).CreateInstance();
			Class07 cl07 = (Class07)Map.Descriptor(typeof(Class07)).CreateInstance();
			Class08 cl08 = (Class08)Map.Descriptor(typeof(Class08)).CreateInstance();
			Class09 cl09 = (Class09)Map.Descriptor(typeof(Class09)).CreateInstance();
			Class10 cl10 = (Class10)Map.Descriptor(typeof(Class10)).CreateInstance();

			cl01.Prop00 = "";
		}
	}
}
*/