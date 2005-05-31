using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace CS.DbManagerTest
{
	public abstract class Test
	{
		public abstract string ConfigurationString 
		{
			get; 
		}

		public virtual string ParamText(string param)
		{
			return "@" + param;
		}

		#region ExecuteNonQuery

		[Test]
		public virtual void SetCommand_CommandType_Text_ExecuteNonQuery()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				db.SetCommand(CommandType.Text, "SELECT 1").ExecuteNonQuery();
			}
		}

		[Test]
		public virtual void SetCommand_CommandType_TableDirect_ExecuteNonQuery()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				db.SetCommand(CommandType.TableDirect, "Customers").ExecuteNonQuery();
			}
		}

		[Test]
		public virtual void SetCommand_CommandType_StoredProcedure_ExecuteNonQuery()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				db.SetCommand(CommandType.StoredProcedure, "[Ten Most Expensive Products]").ExecuteNonQuery();
			}
		}

		#endregion

		#region ExecuteScalar

		[Test]
		public virtual void ExecuteScalar()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				int count = (int)db
					.SetCommand(
						string.Format(
							"SELECT Count(*) FROM Customers WHERE Country = {0}",
							ParamText("country")),
						db.Parameter("@country", "USA"))
					.ExecuteScalar();

				Assert.IsFalse(count == 0);
			}
		}

		#endregion

		#region ExecuteDictionary
		
		public class DictionaryItem
		{
			public int    KeyField;
			public string ValueField;
		}

		[Test]
		public virtual void ExecuteDictionary()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				Hashtable ht = db
					.SetCommand("SELECT 1 KeyField, '11' ValueField UNION SELECT 2, '22'")
					.ExecuteDictionary("KeyField", typeof(DictionaryItem));

				foreach (DictionaryItem di in ht.Values)
				{
					Console.WriteLine("ID  : {0}\nName: {1}",
				        di.KeyField, di.ValueField);
					Console.WriteLine();
				}

				Assert.IsNotNull(ht[1]); 
				Assert.IsNotNull(ht[2]);
				
				Assert.IsTrue((ht[1] as DictionaryItem).ValueField == "11");
				Assert.IsTrue((ht[2] as DictionaryItem).ValueField == "22");
			}
		}

		[Test]
		public virtual void ExecuteDictionary_CommandType_Text()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				Hashtable ht = db
					.SetCommand(
						CommandType.Text, 
						"SELECT 1 KeyField, '11' ValueField UNION SELECT 2, '22'")
					.ExecuteDictionary("KeyField", typeof(DictionaryItem));

				Assert.IsNotNull(ht[1]); 
				Assert.IsNotNull(ht[2]);
				
				Assert.IsTrue((ht[1] as DictionaryItem).ValueField == "11");
				Assert.IsTrue((ht[2] as DictionaryItem).ValueField == "22");
			}
		}

		[Test]
		public virtual void ExecuteDictionary_Parameters()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				Hashtable ht = db
					.SetCommand(
						string.Format(
							"SELECT 1 KeyField, {0} ValueField UNION SELECT 2, {1}",
							ParamText("p1"),
							ParamText("p2")),
						db.Parameter("@p1", "11"),
						db.Parameter("@p2", "22"))
					.ExecuteDictionary("KeyField", typeof(DictionaryItem));

				Assert.IsNotNull(ht[1]); 
				Assert.IsNotNull(ht[2]);
				
				Assert.IsTrue((ht[1] as DictionaryItem).ValueField == "11");
				Assert.IsTrue((ht[2] as DictionaryItem).ValueField == "22");
			}
		}

		[Test]
		public virtual void ExecuteDictionary_CommandType_Text_Parameters()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				Hashtable ht = db
					.SetCommand(
						CommandType.Text,
						string.Format(
							"SELECT 1 KeyField, {0} ValueField UNION SELECT 2, {1}",
							ParamText("p1"),
							ParamText("p2")),
						db.Parameter("@p1", "11"),
						db.Parameter("@p2", "22"))
					.ExecuteDictionary("KeyField", typeof(DictionaryItem));

				Assert.IsNotNull(ht[1]); 
				Assert.IsNotNull(ht[2]);
				
				Assert.IsTrue((ht[1] as DictionaryItem).ValueField == "11");
				Assert.IsTrue((ht[2] as DictionaryItem).ValueField == "22");
			}
		}

		[Test]
		public virtual void ExecutePreparedDictionary()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				db
					.SetCommand(
						string.Format("SELECT {0} KeyField, {1} ValueField",
							ParamText("p1"),
							ParamText("p2")),
						db.Parameter("@p1", DbType.Int32,  4),
						db.Parameter("@p2", DbType.String, 4))
					.Prepare();

				db.Parameter("@p1").Value = 1;
				db.Parameter("@p2").Value = "11";

				Hashtable ht = db.ExecuteDictionary("KeyField", typeof(DictionaryItem));

				db.Parameter("@p1").Value = 2;
				db.Parameter("@p2").Value = "22";

				db.ExecuteDictionary(ht, "KeyField", typeof(DictionaryItem));

				Assert.IsNotNull(ht[1]); 
				Assert.IsNotNull(ht[2]);
				
				Assert.IsTrue((ht[1] as DictionaryItem).ValueField == "11");
				Assert.IsTrue((ht[2] as DictionaryItem).ValueField == "22");
			}
		}

		public class DictionarySpItem
		{
			public string TenMostExpensiveProducts;
			public double UnitPrice;
		}

		[Test]
		public virtual void ExecuteSpDictionary()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				Hashtable ht = db
					.SetSpCommand("[Ten Most Expensive Products]")
					.ExecuteDictionary("TenMostExpensiveProducts", typeof(DictionarySpItem));

				Console.WriteLine("Count = {0}", ht.Count);

				Assert.IsFalse(ht.Count == 0);
			}
		}

		[Test]
		public virtual void ExecuteDictionary_Dictionary()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				Hashtable ht = new Hashtable();

				db
					.SetCommand("SELECT 1 KeyField, '11' ValueField UNION SELECT 2, '22'")
					.ExecuteDictionary(ht, "KeyField", typeof(DictionaryItem));

				Assert.IsNotNull(ht[1]); 
				Assert.IsNotNull(ht[2]);
				
				Assert.IsTrue((ht[1] as DictionaryItem).ValueField == "11");
				Assert.IsTrue((ht[2] as DictionaryItem).ValueField == "22");
			}
		}

		[Test]
		public virtual void ExecuteDictionary_Dictionary_CommandType_Text()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				Hashtable ht = new Hashtable();

				db
					.SetCommand(
						CommandType.Text,
						"SELECT 1 KeyField, '11' ValueField UNION SELECT 2, '22'")
					.ExecuteDictionary(ht, "KeyField", typeof(DictionaryItem));

				Assert.IsNotNull(ht[1]); 
				Assert.IsNotNull(ht[2]);
				
				Assert.IsTrue((ht[1] as DictionaryItem).ValueField == "11");
				Assert.IsTrue((ht[2] as DictionaryItem).ValueField == "22");
			}
		}

		[Test]
		public virtual void ExecuteDictionary_Dictionary_Parameters()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				Hashtable ht = new Hashtable();
				
				db
					.SetCommand(
						string.Format(
							"SELECT 1 KeyField, {0} ValueField UNION SELECT 2, {1}",
							ParamText("p1"),
							ParamText("p2")),
						db.Parameter("@p1", "11"),
						db.Parameter("@p2", "22"))
					.ExecuteDictionary(ht, "KeyField", typeof(DictionaryItem));

				Assert.IsNotNull(ht[1]); 
				Assert.IsNotNull(ht[2]);
				
				Assert.IsTrue((ht[1] as DictionaryItem).ValueField == "11");
				Assert.IsTrue((ht[2] as DictionaryItem).ValueField == "22");
			}
		}

		[Test]
		public virtual void ExecuteDictionary_Dictionary_CommandType_Text_Parameters()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				HybridDictionary ht = new HybridDictionary();

				db
					.SetCommand(
						CommandType.Text,
						string.Format(
							"SELECT 1 KeyField, {0} ValueField UNION SELECT 2, {1}",
							ParamText("p1"),
							ParamText("p2")),
						db.Parameter("@p1", "11"),
						db.Parameter("@p2", "22"))
					.ExecuteDictionary(ht, "KeyField", typeof(DictionaryItem));

				Assert.IsNotNull(ht[1]); 
				Assert.IsNotNull(ht[2]);
				
				Assert.IsTrue((ht[1] as DictionaryItem).ValueField == "11");
				Assert.IsTrue((ht[2] as DictionaryItem).ValueField == "22");
			}
		}

		[Test]
		public virtual void ExecuteSpDictionary_Dictionary()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				Hashtable ht = new Hashtable();

				db
					.SetSpCommand("[Ten Most Expensive Products]")
					.ExecuteDictionary(ht, "TenMostExpensiveProducts", typeof(DictionarySpItem));

				Console.WriteLine("Count = {0}", ht.Count);

				Assert.IsFalse(ht.Count == 0);
			}
		}

		#endregion
	}
}
