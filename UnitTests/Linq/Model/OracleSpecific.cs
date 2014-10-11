using System;

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;

namespace UnitTests.Linq.Interface.Model
{
	public class OracleSpecific
	{
		public class SequenceTest
		{
			[Identity, SequenceName("SequenceTestSeq")]
			public int    ID;
			public string Value;
		}
	}
}
