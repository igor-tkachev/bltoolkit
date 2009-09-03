using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public interface ISqlExpression : IEquatable<ISqlExpression>, ISqlExpressionWalkable
	{
		object Clone(Dictionary<object,object> objectTree);
		bool   CanBeNull();
		int    Precedence { get; }
	}
}
