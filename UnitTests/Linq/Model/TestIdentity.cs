using System;

using BLToolkit.DataAccess;

namespace Data.Linq.Model
{
	public class TestIdentity
	{
		[Identity, PrimaryKey]
		//[SequenceName(ProviderName.PostgreSQL, "Seq")]
		//[SequenceName(ProviderName.Firebird,   "PersonID")]
		//[SequenceName("ID")]
		public int ID;
	}
}
