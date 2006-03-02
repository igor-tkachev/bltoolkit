using System;
using System.Data.SqlClient;
using System.Data;
using System.Text;
#if FW2
using System.Collections.Generic;
#endif

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder.Builders;

namespace UnitTests.All
{
#if FW2
	[TestFixture]
	public class GetValueTest
	{
		public DayOfWeek? Dow;

		public void Cast1(object value)
		{
			Dow = (DayOfWeek)value;
		}

		public void Cast2(object value)
		{
			Dow = (DayOfWeek?)(DayOfWeek)value;
		}

		public void Cast3(object value)
		{
			Dow = (DayOfWeek?)value;
		}

		[Test]
		public void TestFW2()
		{
			Cast1(DayOfWeek.Thursday);
			Cast1(2);
			Cast2(DayOfWeek.Thursday);
			Cast2(2);
			Cast3(DayOfWeek.Thursday);
			Cast3(2);
		}
	}
#else
	[TestFixture]
	public class GetValueTest
	{
		[Test]
		public void Test()
		{
		}

		public void Foo(string toWhom)
		{
			Console.WriteLine(string.Format("Hello, {0}!", toWhom));
		}
	}
#endif
}
