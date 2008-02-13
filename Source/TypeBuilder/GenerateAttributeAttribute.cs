using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
	public class GenerateAttributeAttribute : Builders.AbstractTypeBuilderAttribute
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
		public           Type  AttributeType
		{
			get { return _attributeType; }
		}

		private readonly object[] _arguments;
		public           object[]  Arguments
		{
			get { return _arguments; }
		}

		private string[] _namedArgumentNames;
		public  string[]  NamedArgumentNames
		{
			get { return _namedArgumentNames;  }
			set { _namedArgumentNames = value; }
		}

		private object[] _namedArgumentValues;
		public  object[]  NamedArgumentValues
		{
			get { return _namedArgumentValues;  }
			set { _namedArgumentValues = value; }
		}

		public override Builders.IAbstractTypeBuilder  TypeBuilder
		{
			get { return new Builders.GeneratedAttributeBuilder(_attributeType, _arguments, _namedArgumentNames, _namedArgumentValues); }
		}
	}
}