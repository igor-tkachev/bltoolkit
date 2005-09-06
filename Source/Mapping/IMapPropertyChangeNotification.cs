using System;
using System.Reflection;

namespace Rsdn.Framework.Data.Mapping
{
	public interface IMapNotifyPropertyChanged
	{
		void PropertyChanged(MapPropertyInfo pi);
	}
}
