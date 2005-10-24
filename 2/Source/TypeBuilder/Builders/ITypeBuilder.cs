using System;

namespace BLToolkit.TypeBuilder.Builders
{
	public interface ITypeBuilder
	{
		bool IsCompatible(BuildContext context, ITypeBuilder typeBuilder);
	}
}
