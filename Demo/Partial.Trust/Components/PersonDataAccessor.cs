using System;
using System.Collections.Generic;

using BLToolkit.DataAccess;

namespace Partial.Trust.Components
{
	public abstract class PersonDataAccessor : DataAccessor
	{
		[SqlQuery("SELECT * FROM Customers")]
		public abstract List<Customers> GetPersonList();
	}
}
