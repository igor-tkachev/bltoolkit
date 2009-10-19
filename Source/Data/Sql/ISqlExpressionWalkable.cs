using System;

namespace BLToolkit.Data.Sql
{
	public delegate ISqlExpression WalkingFunc(ISqlExpression expr);

	public interface ISqlExpressionWalkable
	{
		//[Obsolete]
		ISqlExpression Walk(bool skipColumns, WalkingFunc func);
	}
}
