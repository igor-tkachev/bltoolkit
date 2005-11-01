using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DefaultInstanceTypeAttribute : Builders.AbstractTypeBuilderAttribute
	{
		public DefaultInstanceTypeAttribute ()
		{
			_typeBuilder = new Builders.DefaultTypeBuilder();
		}

		private         Builders.IAbstractTypeBuilder _typeBuilder;
		public override Builders.IAbstractTypeBuilder  TypeBuilder
		{
			get { return _typeBuilder; }
		}
	}
}
