using System;

namespace BLToolkit.Data.Sql
{
	public interface ISqlPredicate : ISqlExpressionWalkable, ICloneableElement
	{
		bool CanBeNull();
		int  Precedence { get; }
	}
}
