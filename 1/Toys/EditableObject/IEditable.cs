using System;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.EditableObject
{
	public interface IEditable
	{
		void AcceptChanges();
		void RejectChanges();
		bool IsDirty { [return: MapReturnIfFalse] get; }
	}
}
