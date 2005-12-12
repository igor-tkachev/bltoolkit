using System;
using System.Collections;
using System.Reflection;

using BLToolkit.TypeBuilder;

namespace BLToolkit.EditableObjects
{
	public interface IEditable
	{
		void AcceptChanges();
		void RejectChanges();

		bool IsDirty { [return: ReturnIfTrue] get; }
		[return: ReturnIfTrue]
		bool IsDirtyMember  ([PropertyInfo] PropertyInfo propertyInfo, string memberName, ref bool isDirty);
		void GetDirtyMembers([PropertyInfo] PropertyInfo propertyInfo, ArrayList list);
	}
}
