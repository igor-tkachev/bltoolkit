using System;
using System.Collections;
using System.Reflection;

namespace BLToolkit.TypeBuilder.Builders
{
	class FakeParameterInfo : ParameterInfo
	{
		public FakeParameterInfo(string name, Type type, MemberInfo memberInfo, object[] attributes)
		{
			_name       = name;
			_type       = type;
			_memberInfo = memberInfo;
			_attributes = attributes == null? new object[0]: attributes;
		}

		public FakeParameterInfo(MethodInfo method) : this(
			"ret",
			method.ReturnType,
			method,
			method.ReturnTypeCustomAttributes.GetCustomAttributes(true))
		{
		}

		public override ParameterAttributes Attributes
		{
			get { return ParameterAttributes.Retval; }
		}

		public override object DefaultValue
		{
			get { return DBNull.Value; }
		}

		private object[] _attributes;

		public override object[] GetCustomAttributes(bool inherit)
		{
			return _attributes;
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (attributeType == null) throw new ArgumentNullException("attributeType");

			if (_attributes.Length == 0)
				return _attributes;

			ArrayList list = new ArrayList();

			foreach (object o in _attributes)
				if (o.GetType() == attributeType || attributeType.IsInstanceOfType(o))
					list.Add(o);

			return list.ToArray();
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			if (attributeType == null) throw new ArgumentNullException("attributeType");

			foreach (object o in _attributes)
				if (o.GetType() == attributeType || attributeType.IsInstanceOfType(o))
					return true;

			return false;
		}

		private         MemberInfo _memberInfo;
		public override MemberInfo Member
		{
			get { return _memberInfo; }
		}

		private         string _name;
		public override string  Name
		{
			get { return _name; }
		}

		private         Type _type;
		public override Type  ParameterType
		{
			get { return _type; }
		}

		public override int Position
		{
			get { return 0; }
		}
	}
}
