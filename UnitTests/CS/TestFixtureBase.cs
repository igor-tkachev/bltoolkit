using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

using NUnit.Framework;

using BLToolkit.Mapping;

public class TestFixtureBase
{
	public static DataTable GetDataTable()
	{
		DataTable table = new DataTable();

		table.Columns.Add("ID",   typeof(int));
		table.Columns.Add("Name", typeof(string));
		table.Columns.Add("Date", typeof(DateTime));
		
		table.Rows.Add(new object[] { 1, "John Pupkin", DateTime.Now });
		table.Rows.Add(new object[] { 2, "Goblin",      DBNull.Value });

		table.AcceptChanges();

		return table;
	}

	public static void CompareLists(object obj1, object obj2)
	{
		IList list1 = obj1 is IListSource? ((IListSource)obj1).GetList(): (IList)obj1;
		IList list2 = obj2 is IListSource? ((IListSource)obj2).GetList(): (IList)obj2;

		Assert.AreEqual(list1.Count, list2.Count);

		for (int i = 0; i < list1.Count; i++)
		{
			IMapDataSource o1 = Map.DefaultSchema.GetDataSource(list1[i]);
			IMapDataSource o2 = Map.DefaultSchema.GetDataSource(list2[i]);

			for (int j = 0; j < o1.Count; j++)
			{
				string name = o1.GetName(j);

				for (int k = 0; k < o1.Count; k++)
				{
					if (name == o2.GetName(k))
					{
						Assert.AreEqual(o1.GetValue(list1[i], j), o2.GetValue(list2[i], k));
						break;
					}
				}
			}
		}
	}
}
