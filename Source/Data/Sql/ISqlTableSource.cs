using System;

namespace BLToolkit.Data.Sql
{
	public interface ISqlTableSource : ICloneableElement
	{
		SqlField All      { get; }
		int      SourceID { get; }
	}
}
