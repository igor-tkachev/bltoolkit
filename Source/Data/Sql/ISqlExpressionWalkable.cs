using System;

namespace BLToolkit.Data.Sql
{
	public delegate ISqlExpression WalkingFunc(ISqlExpression expr);

	public interface ISqlExpressionWalkable
	{
		ISqlExpression Walk(bool skipColumns, WalkingFunc func);
	}
}
