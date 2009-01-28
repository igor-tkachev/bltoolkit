using System;

namespace BLToolkit.Data.Sql
{
	public interface ISqlExpressionScannable
	{
		void ForEach(Action<ISqlExpression> action);
	}
}
