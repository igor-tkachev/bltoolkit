using System;

namespace BLToolkit.Data.SqlBuilder
{
	public interface ISqlExpression : IEquatable<ISqlExpression>, IExpressionScannable
	{
	}
}
