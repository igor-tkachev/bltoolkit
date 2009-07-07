using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public interface ISqlTableSource
	{
		object Clone(Dictionary<object,object> objectTree);
		int    SourceID { get; }
	}
}
