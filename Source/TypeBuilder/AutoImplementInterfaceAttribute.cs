using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Interface)]
	public class AutoImplementInterfaceAttribute : Builders.AbstractTypeBuilderAttribute
	{
		private         Builders.IAbstractTypeBuilder _typeBuilder;
		public override Builders.IAbstractTypeBuilder  TypeBuilder
		{
			get { return _typeBuilder ?? (_typeBuilder = new Builders.AutoImplementInterfaceBuilder()); }
		}
	}
}
