using System;

namespace BLToolkit.Data.Sql
{
	public interface ISqlPredicate : ISqlExpressionWalkable
	{
		int Precedence { get; }
	}
}
