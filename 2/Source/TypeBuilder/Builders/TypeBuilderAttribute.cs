using System;

namespace BLToolkit.TypeBuilder.Builders
{
	public abstract class TypeBuilderAttribute : Attribute
	{
		public abstract ITypeBuilder TypeBuilder { get; }
	}
}
