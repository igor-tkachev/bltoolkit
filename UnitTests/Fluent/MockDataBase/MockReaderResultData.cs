using System;
using System.Collections.Generic;
using System.Linq;

namespace BLToolkit.Fluent.Test.MockDataBase
{
	public class MockReaderResultData
	{
		public MockReaderResultData()
		{
			Names = new List<string>();
			Types = new List<Type>();
			Values = new List<object[]>();
		}

		public List<string> Names { get; private set; }

		public List<Type> Types { get; private set; }

		public List<object[]> Values { get; private set; }

		public void SetNames(string[] fields)
		{
			Names.Clear();
			Names.AddRange(fields);
			Types = Types.Take(Math.Min(Types.Count, Names.Count)).ToList();
			for (int i = Types.Count; i < Names.Count; i++)
			{
				Types.Add(null);
			}
		}
	}
}