using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
	public sealed class GenerateAttributeAttribute : Builders.AbstractTypeBuilderAttribute
	{
		public GenerateAttributeAttribute(Type attributeType)
		{
			_attributeType = attributeType;
		}

		public GenerateAttributeAttribute(Type attributeType, params object[] arguments)
		{
			_attributeType = attributeType;
			_arguments     = arguments;
		}

		private readonly Type _attributeType;
		public           Type  attributeType
		{
			get { return _attributeType; }
		}

		private readonly object[] _arguments;
		public           object[]  arguments
		{
			get { return _arguments; }
		}

		public override Builders.IAbstractTypeBuilder  TypeBuilder
		{
			get { return new Builders.GeneratedAttributeBuilder(_attributeType, _arguments); }
		}
	}
}