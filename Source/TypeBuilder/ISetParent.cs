using System.Reflection;

namespace BLToolkit.TypeBuilder
{
	public interface ISetParent
	{
		void SetParent([Parent]object parent, [PropertyInfo]PropertyInfo propertyInfo);
	}
}
