using System;
using System.ComponentModel;

namespace BLToolkit.ComponentModel
{
	public interface ITypeDescriptionProvider
	{
		Type                         OriginalType  { get; }
		string                       ClassName     { get; }
		string                       ComponentName { get; }

		EventDescriptor              GetEvent      (string name);
		PropertyDescriptor           GetProperty   (string name);

		AttributeCollection          GetAttributes ();
		EventDescriptorCollection    GetEvents     ();
		PropertyDescriptorCollection GetProperties ();
	}
}
