using System;
using System.Reflection;

namespace BLToolkit.TypeBuilder
{
	[PropertyChanged]
	public interface IPropertyChanged
	{
		void PropertyChanged(PropertyInfo propertyInfo);
	}
}
