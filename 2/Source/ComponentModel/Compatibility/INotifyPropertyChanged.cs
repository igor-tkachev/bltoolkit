using System;
using System.ComponentModel;

namespace BLToolkit.ComponentModel
{
	//public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

	public interface INotifyPropertyChanged
	{
		event PropertyChangedEventHandler PropertyChanged;
	}
}
