using System;

namespace BLToolkit.Data.Sql
{
	public interface IQueryElement //: ICloneableElement
	{
		QueryElementType ElementType { get; }
	}
}
