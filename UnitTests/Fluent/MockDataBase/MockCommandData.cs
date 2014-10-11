using System.Collections.Generic;

namespace BLToolkit.Fluent.Test.MockDataBase
{
	public class MockCommandData
	{
		public int NonQueryResult { get; set; }

		public MockReaderData ReaderResult { get; set; }

		public object ScalarResult { get; set; }

		public string CommandText { get; set; }

		public bool IsUsing { get; set; }

		public List<MockDbDataParameter> Parameters { get; set; }

		public Dictionary<string, int> Fields { get; set; }

		public List<string> Tables { get; set; }
	}
}