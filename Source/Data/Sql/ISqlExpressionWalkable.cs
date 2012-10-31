using System;

namespace BLToolkit.Data.Sql
{
	public interface ISqlExpressionWalkable
	{
		//[Obsolete]
		ISqlExpression Walk(bool skipColumns, Func<ISqlExpression,ISqlExpression> func);
	}
}
