using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
	public class GenerateAttributeAttribute: Builders.AbstractTypeBuilderAttribute
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

		public object this[string name]
		{
			get
			{
				if (_namedArgumentNames == null)
					return null;

				int idx = Array.IndexOf(_namedArgumentNames, name);

				return idx < 0? null: _namedArgumentValues[idx];
			}
			set
			{
				if (_namedArgumentNames == null)
				{
					_namedArgumentNames  = new string[]{ name  };
					_namedArgumentValues = new object[]{ value };
					return;
				}

				int idx = Array.IndexOf(_namedArgumentNames, name);
				if (idx < 0)
				{
					idx = _namedArgumentNames.Length;

					Array.Resize(ref _namedArgumentNames,  idx + 1);
					Array.Resize(ref _namedArgumentValues, idx + 1);

					_namedArgumentNames [idx] = name;
					_namedArgumentValues[idx] = value;
				}
				else
				{
					_namedArgumentValues[idx] = value;
				}
			}
		}

		public T GetValue<T>(string name)
		{
			object value = this[name];
			return value == null? default(T): (T)value;
		}

		public T GetValue<T>(string name, T defaultValue)
		{
			return _namedArgumentNames == null || Array.IndexOf(_namedArgumentNames, name) < 0?
				defaultValue : GetValue<T>(name);
		}

		public void SetValue<T>(string name, T value)
		{
			this[name] = value;
		}

		public override Builders.IAbstractTypeBuilder  TypeBuilder
		{
			get
			{
				return new Builders.GeneratedAttributeBuilder(
					_attributeType, _arguments, _namedArgumentNames, _namedArgumentValues);
			}
		}
	}
}