using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;

namespace Mono.Model
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
