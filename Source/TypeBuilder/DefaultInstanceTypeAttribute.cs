using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DefaultInstanceTypeAttribute : Builders.TypeBuilderAttribute
	{
		public DefaultInstanceTypeAttribute ()
		{
			_typeBuilder = new Builders.DefaultTypeBuilder();
		}

		private         Builders.ITypeBuilder _typeBuilder;
		public override Builders.ITypeBuilder  TypeBuilder
		{
			get { return _typeBuilder; }
		}
	}
}
