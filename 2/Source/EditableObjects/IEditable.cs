using System;
using System.Collections;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

using BLToolkit.TypeBuilder;

namespace BLToolkit.EditableObjects
{
	public interface IEditable
	{
		void AcceptChanges();
		void RejectChanges();
		bool IsDirty { [return: ReturnIfTrue] get; }
	}
}
