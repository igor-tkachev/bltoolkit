using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
	public class ImplementInterfaceAttribute : AbstractTypeBuilderAttribute
	{
		public ImplementInterfaceAttribute(Type type)
		{
			_type = type;
		}

		private Type _type;
		public  Type  Type
		{
			get { return _type;  }
			set { _type = value; }
		}

		public override IAbstractTypeBuilder TypeBuilder
		{
			get { return new ImplementInterfaceBuilder(_type); }
		}
	}
}
