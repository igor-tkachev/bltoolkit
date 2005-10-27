using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Property)]
	public class InstanceTypeAttribute : Builders.TypeBuilderAttribute
	{
		public InstanceTypeAttribute(Type type)
		{
			_typeBuilder = new Builders.InstanceTypeBuilder(type);
		}

		private         Builders.ITypeBuilder _typeBuilder;
		public override Builders.ITypeBuilder  TypeBuilder
		{
			get { return _typeBuilder; }
		}
	}
}
