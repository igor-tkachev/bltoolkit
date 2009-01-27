using System;

namespace BLToolkit.Data.Sql
{
	public interface IExpressionScannable
	{
		void ForEach(Action<ISqlExpression> action);
	}
}
