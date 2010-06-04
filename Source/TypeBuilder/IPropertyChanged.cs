using System.Reflection;

namespace BLToolkit.TypeBuilder
{
	[PropertyChanged]
	public interface IPropertyChanged
	{
		void OnPropertyChanged(PropertyInfo propertyInfo);
	}
}
