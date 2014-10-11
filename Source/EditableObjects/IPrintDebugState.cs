using System.Reflection;
using System.Diagnostics.CodeAnalysis;

using BLToolkit.TypeBuilder;

namespace BLToolkit.EditableObjects
{
	public interface IPrintDebugState
	{
		[SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
		void PrintDebugState([PropertyInfo] PropertyInfo propertyInfo, ref string str);
	}
}
