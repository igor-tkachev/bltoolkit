using System;

namespace BLToolkit.Data.Sql
{
	public interface ISqlExpression : IEquatable<ISqlExpression>, ISqlExpressionWalkable, ICloneableElement
	{
		bool CanBeNull();
		int  Precedence { get; }
	}
}
