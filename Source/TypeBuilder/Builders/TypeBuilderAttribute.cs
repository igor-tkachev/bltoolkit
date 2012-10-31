using System;

namespace BLToolkit.TypeBuilder.Builders
{
	public abstract class AbstractTypeBuilderAttribute : Attribute
	{
		public abstract IAbstractTypeBuilder TypeBuilder { get; }
	}
}
