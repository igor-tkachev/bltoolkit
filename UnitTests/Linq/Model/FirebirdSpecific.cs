using System;

using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UnitTests.Linq.Interface.Model
{
	public class FirebirdSpecific
	{
		public class SequenceTest
		{
			[Identity, SequenceName("SequenceTestSeq")]
			public int    ID;

			[MapField("VALUE_")] // 'Value' reserved by firebird
			public string Value;
		}
	}
}
