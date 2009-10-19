using System;

namespace BLToolkit.Data.Sql
{
	public interface ISqlTableSource : IQueryElement, ICloneableElement
	{
		SqlField All      { get; }
		int      SourceID { get; }
	}
}
