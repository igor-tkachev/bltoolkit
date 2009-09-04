using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public interface ISqlPredicate : ISqlExpressionWalkable
	{
		object Clone    (Dictionary<object,object> objectTree);
		bool   CanBeNull();
		int    Precedence { get; }
	}
}
