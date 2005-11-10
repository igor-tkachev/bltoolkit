using System;
using System.Reflection;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.EditableObject
{
	public interface IEditable
	{
		void AcceptChanges();
		void RejectChanges();
		bool IsDirty { [return: MapReturnIfTrue] get; }

		[return: MapReturnIfTrue]
		bool IsDirtyMember(string memberName, [MapPropertyInfo] MapPropertyInfo propertyInfo, ref bool isDirty);
	}
}
