using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Property)]
	public class InstanceTypeAttribute : Builders.TypeBuilderAttribute
	{
		public InstanceTypeAttribute(Type type)
		{
			_type = type;
			SetParameters();
		}

		public InstanceTypeAttribute(Type type, object parameter1)
		{
			_type = type;
			SetParameters(parameter1);
		}

		public InstanceTypeAttribute(Type type,
			object parameter1,
			object parameter2)
		{
			_type = type;
			SetParameters(parameter1, parameter2);
		}

		public InstanceTypeAttribute(Type type,
			object parameter1,
			object parameter2,
			object parameter3)
		{
			_type = type;
			SetParameters(parameter1, parameter2, parameter3);
		}

		public InstanceTypeAttribute(Type type,
			object parameter1,
			object parameter2,
			object parameter3,
			object parameter4)
		{
			_type = type;
			SetParameters(parameter1, parameter2, parameter3, parameter4);
		}
		
		public InstanceTypeAttribute(Type type,
			object parameter1,
			object parameter2,
			object parameter3,
			object parameter4,
			object parameter5)
		{
			_type = type;
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

		private Type _type;

		private         Builders.ITypeBuilder _typeBuilder;
		public override Builders.ITypeBuilder  TypeBuilder
		{
			get 
			{
				if (_typeBuilder == null)
					_typeBuilder = new Builders.InstanceTypeBuilder(_type);

				return _typeBuilder;
			}
		}
	}
}
