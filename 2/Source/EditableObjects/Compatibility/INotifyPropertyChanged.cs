using System;

namespace BLToolkit.EditableObjects
{
	public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

	public interface INotifyPropertyChanged
	{
		event PropertyChangedEventHandler PropertyChanged;
	}
}
