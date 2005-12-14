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

		[return: ReturnIfTrue] bool AcceptMemberChanges([PropertyInfo] PropertyInfo propertyInfo, string memberName);
		[return: ReturnIfTrue] bool RejectMemberChanges([PropertyInfo] PropertyInfo propertyInfo, string memberName);
		[return: ReturnIfTrue] bool IsDirtyMember      ([PropertyInfo] PropertyInfo propertyInfo, string memberName, ref bool isDirty);

		void GetDirtyMembers([PropertyInfo] PropertyInfo propertyInfo, ArrayList list);
		[SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
		void PrintDebugState([PropertyInfo] PropertyInfo propertyInfo, ref string str);
	}
}
