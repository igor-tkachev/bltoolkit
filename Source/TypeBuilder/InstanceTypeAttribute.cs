using System;
using System.Diagnostics.CodeAnalysis;

namespace BLToolkit.TypeBuilder
{
	[SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
	[AttributeUsage(AttributeTargets.Property)]
	public class InstanceTypeAttribute : Builders.AbstractTypeBuilderAttribute
	{
		public InstanceTypeAttribute (Type instanceType)
		{
			_instanceType = instanceType;
			//SetParameters();
		}

		public InstanceTypeAttribute (Type instanceType, object parameter1)
		{
			_instanceType = instanceType;
			SetParameters(parameter1);
		}

		public InstanceTypeAttribute (Type instanceType,
			object parameter1,
			object parameter2)
		{
			_instanceType = instanceType;
			SetParameters(parameter1, parameter2);
		}

		public InstanceTypeAttribute (Type instanceType,
			object parameter1,
			object parameter2,
			object parameter3)
		{
			_instanceType = instanceType;
			SetParameters(parameter1, parameter2, parameter3);
		}

		public InstanceTypeAttribute (Type instanceType,
			object parameter1,
			object parameter2,
			object parameter3,
			object parameter4)
		{
			_instanceType = instanceType;
			SetParameters(parameter1, parameter2, parameter3, parameter4);
		}

		public InstanceTypeAttribute (Type instanceType,
			object parameter1,
			object parameter2,
			object parameter3,
			object parameter4,
			object parameter5)
		{
			_instanceType = instanceType;
			SetParameters(parameter1, parameter2, parameter3, parameter4, parameter5);
		}

		protected void SetParameters(params object[] parameters)
		{
			_parameters = parameters;
		}

		private object[] _parameters;
		public  object[]  Parameters
		{
			get { return _parameters;  }
		}

		private readonly Type _instanceType;
		protected        Type  InstanceType
		{
			get { return _instanceType; }
		}

		private bool _isObjectHolder;
		public  bool  IsObjectHolder
		{
			get { return _isObjectHolder;  }
			set { _isObjectHolder = value; }
		}

		private         Builders.IAbstractTypeBuilder _typeBuilder;
		public override Builders.IAbstractTypeBuilder  TypeBuilder
		{
			get 
			{
				if (_typeBuilder == null)
					_typeBuilder = new Builders.InstanceTypeBuilder(_instanceType, _isObjectHolder);

				return _typeBuilder;
			}
		}
	}
}
