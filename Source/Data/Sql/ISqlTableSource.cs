using System;

namespace BLToolkit.Data.Sql
{
	public interface ISqlTableSource : ISqlExpression
	{
		SqlField All      { get; }
		int      SourceID { get; }
	}
}
