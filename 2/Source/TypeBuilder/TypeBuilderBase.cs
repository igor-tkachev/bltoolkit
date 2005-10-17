using System;

namespace BLToolkit.TypeBuilder
{
	public abstract class TypeBuilderBase : ITypeBuilder
	{
		public virtual bool IsCompatible(BuildContext context, ITypeBuilder typeBuilder)
		{
			return true;
		}

		protected bool IsRelative(ITypeBuilder typeBuilder)
		{
			return GetType().IsInstanceOfType(typeBuilder) || typeBuilder.GetType().IsInstanceOfType(this);
		}
	}
}
