using System;
using System.Collections;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.EditableObject
{
	public interface IEditable
	{
		void AcceptChanges();
		void RejectChanges();
		[return: MapReturnIfTrue] bool AcceptChanges(string memberName, [MapPropertyInfo] MapPropertyInfo propertyInfo);
		[return: MapReturnIfTrue] bool RejectChanges(string memberName, [MapPropertyInfo] MapPropertyInfo propertyInfo);

		bool IsDirty { [return: MapReturnIfTrue] get; }
		[return: MapReturnIfTrue]
		bool IsDirtyMember  (string memberName, [MapPropertyInfo] MapPropertyInfo propertyInfo, ref bool isDirty);
		void GetDirtyMembers([MapPropertyInfo] MapPropertyInfo propertyInfo, ArrayList list);

		void PrintDebugState([MapPropertyInfo] MapPropertyInfo propertyInfo, ref string str);
	}
}
