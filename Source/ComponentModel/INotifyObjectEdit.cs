using System;

namespace BLToolkit.ComponentModel
{
	public delegate void ObjectEditEventHandler(object sender, ObjectEditEventArgs args);

	public interface INotifyObjectEdit
	{
		event ObjectEditEventHandler ObjectEdit;
	}
}
