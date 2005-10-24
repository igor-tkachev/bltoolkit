using System;

namespace BLToolkit.TypeBuilder.Builders
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public abstract class TypeBuilderAttribute : Attribute
	{
		public abstract ITypeBuilder TypeBuilder { get; }
	}
}
