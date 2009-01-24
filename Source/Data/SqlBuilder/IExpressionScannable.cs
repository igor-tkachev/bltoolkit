using System;

namespace BLToolkit.Data.SqlBuilder
{
	public interface IExpressionScannable
	{
		void ForEach(Action<ISqlExpression> action);
	}
}
