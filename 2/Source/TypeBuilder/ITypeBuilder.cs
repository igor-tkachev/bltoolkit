using System;

namespace BLToolkit.TypeBuilder
{
	public interface ITypeBuilder
	{
		bool IsCompatible(BuildContext context, ITypeBuilder typeBuilder);
	}
}
