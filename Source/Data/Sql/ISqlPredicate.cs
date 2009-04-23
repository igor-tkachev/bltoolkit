using System;

namespace BLToolkit.Data.Sql
{
	public interface ISqlPredicate : ISqlExpressionScannable
	{
		int Precedence { get; }
	}
}
