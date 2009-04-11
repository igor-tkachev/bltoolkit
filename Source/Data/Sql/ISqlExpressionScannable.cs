using System;

namespace BLToolkit.Data.Sql
{
	public interface ISqlExpressionScannable
	{
		void ForEach(bool skipColumns, Action<ISqlExpression> action);
	}
}
