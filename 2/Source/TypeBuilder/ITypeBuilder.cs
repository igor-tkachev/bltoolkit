using System;

namespace BLToolkit.TypeBuilder
{
	public interface ITypeBuilder
	{
		bool IsCompatible    (ITypeBuilder typeBuilder);
	}
}
