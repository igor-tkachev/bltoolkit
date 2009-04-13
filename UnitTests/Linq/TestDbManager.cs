using System;

using BLToolkit.Data;
using BLToolkit.Data.Linq;

namespace Data.Linq
{
	using Model;

	public class TestDbManager : DbManager
	{
		public TestDbManager(string configString)
			: base(configString)
		{
		}

		public Table<Person> Person
		{
			get { return GetTable<Person>(); }
		}
	}
}
