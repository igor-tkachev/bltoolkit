using System;

namespace Rsdn.Framework.EditableObject
{
	public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

	public interface INotifyPropertyChanged
	{
		event PropertyChangedEventHandler PropertyChanged;
	}
}
