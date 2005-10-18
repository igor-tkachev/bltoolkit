using System;
using System.Reflection;

namespace BLToolkit.TypeBuilder
{
	class FakeSetter : FakeMethodInfo
	{
		public FakeSetter(PropertyInfo propertyInfo) 
			: base(propertyInfo, propertyInfo.GetGetMethod(true))
		{
		}

		public override ParameterInfo[] GetParameters()
		{
			ParameterInfo[] index = _property.GetIndexParameters();
			ParameterInfo[] pi    = new ParameterInfo[index.Length + 1];

			pi[0] = new FakeParameterInfo("ret", _property.PropertyType, _property, null);
			index.CopyTo(pi, 1);

			return pi;
		}

		public override string Name
		{
			get { return "set_" + _property.Name; }
		}

		public override Type ReturnType
		{
			get { return _property.PropertyType; }
		}
	}
}
