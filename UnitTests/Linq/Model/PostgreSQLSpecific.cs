using System;

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;

namespace UnitTests.Linq.Interface.Model
{
	public class PostgreSQLSpecific
	{
		public class SequenceTest1
		{
			[Identity, SequenceName("SequenceTestSeq")]
			public int    ID;
			public string Value;
		}

		public class SequenceTest2
		{
			[Identity]
			public int    ID;
			public string Value;
		}

		public class SequenceTest3
		{
			[Identity, SequenceName("SequenceTestSeq")]
			public int    ID;
			public string Value;
		}
	}
}
