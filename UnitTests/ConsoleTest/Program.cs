using System;
using System.Data;

using Data.Linq;

namespace ConsoleTest
{
	class Program
	{
		class Test : TestBase
		{
			public Test()
			{
				ForEachProvider(db =>
				{
					if (db.Connection.State != ConnectionState.Open)
						db.Connection.Open();
				});
			}
		}

		static void Main(string[] args)
		{
			new JoinTest();
			new Test();
		}
	}
}
