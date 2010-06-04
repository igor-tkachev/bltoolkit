using System;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Properties;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	class InstanceTypeBuilder : DefaultTypeBuilder
	{
		public InstanceTypeBuilder(Type instanceType, bool isObjectHolder)
		{
			_instanceType   = instanceType;
			_isObjectHolder = isObjectHolder;
		}

		public InstanceTypeBuilder(Type propertyType, Type instanceType, bool isObjectHolder)
		{
			_propertyType   = propertyType;
			_instanceType   = instanceType;
			_isObjectHolder = isObjectHolder;
		}

		private readonly bool _isObjectHolder;

		private readonly Type _propertyType;
		public           Type  PropertyType
		{
			get { return _propertyType; }
		}

		private readonly Type _instanceType;
		public           Type  InstanceType
		{
			get { return _instanceType; }
		}

		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			if (context == null) throw new ArgumentNullException("context");

			return
				base.IsApplied(context, builders) &&
				context.CurrentProperty != null &&
				context.CurrentProperty.GetIndexParameters().Length == 0 &&
				(PropertyType == null ||
				 TypeHelper.IsSameOrParent(PropertyType, context.CurrentProperty.PropertyType));
		}

		protected override Type GetFieldType()
		{
			return InstanceType;
		}

		protected override Type GetObjectType()
		{
			return IsObjectHolder? Context.CurrentProperty.PropertyType: base.GetObjectType();
		}

		protected override bool IsObjectHolder
		{
			get { return _isObjectHolder && Context.CurrentProperty.PropertyType.IsClass; }
		}

		protected override void BuildAbstractGetter()
		{
			FieldBuilder field        = GetField();
			EmitHelper   emit         = Context.MethodBuilder.Emitter;
			Type         propertyType = Context.CurrentProperty.PropertyType;
			MemberInfo   getter       = GetGetter();

			if (InstanceType.IsValueType) emit.ldarg_0.ldflda(field);
			else                          emit.ldarg_0.ldfld (field);

			Type memberType;

			if (getter is PropertyInfo)
			{
				PropertyInfo pi = ((PropertyInfo)getter);

				if (InstanceType.IsValueType) emit.call    (pi.GetGetMethod());
				else                          emit.callvirt(pi.GetGetMethod());

				memberType = pi.PropertyType;
			}
			else if (getter is FieldInfo)
			{
				FieldInfo fi = (FieldInfo)getter;

				emit.ldfld(fi);

				memberType = fi.FieldType;
			}
			else
			{
				MethodInfo      mi    = (MethodInfo)getter;
				ParameterInfo[] pi = mi.GetParameters();

				for (int k = 0; k < pi.Length; k++)
				{
					ParameterInfo p = pi[k];

					if (p.IsDefined(typeof (ParentAttribute), true))
					{
						// Parent - set this.
						//
						emit.ldarg_0.end();

						if (!TypeHelper.IsSameOrParent(p.ParameterType, Context.Type))
							emit.castclass(p.ParameterType);
					}
					else if (p.IsDefined(typeof (PropertyInfoAttribute), true))
					{
						// PropertyInfo.
						//
						emit.ldsfld(GetPropertyInfoField()).end();
					}
					else
						throw new TypeBuilderException(string.Format(
							Resources.TypeBuilder_UnknownParameterType,
							mi.Name, mi.DeclaringType.FullName, p.Name));

				}

				if (InstanceType.IsValueType) emit.call    (mi);
				else                          emit.callvirt(mi);

				memberType = mi.ReturnType;
			}

			if (propertyType.IsValueType)
			{
				if (memberType.IsValueType == false)
					emit.CastFromObject(propertyType);
			}
			else
			{
				if (memberType != propertyType)
					emit.castclass(propertyType);
			}

			emit.stloc(Context.ReturnValue);
		}

		protected override void BuildAbstractSetter()
		{
			FieldBuilder field        = GetField();
			EmitHelper   emit         = Context.MethodBuilder.Emitter;
			Type         propertyType = Context.CurrentProperty.PropertyType;
			MemberInfo   setter       = GetSetter();

			if (InstanceType.IsValueType) emit.ldarg_0.ldflda(field);
			else                          emit.ldarg_0.ldfld (field);

			if (setter is PropertyInfo)
			{
				PropertyInfo pi = ((PropertyInfo)setter);

				emit.ldarg_1.end();

				if (propertyType.IsValueType && !pi.PropertyType.IsValueType)
					emit.box(propertyType);

				if (InstanceType.IsValueType) emit.call    (pi.GetSetMethod());
				else                          emit.callvirt(pi.GetSetMethod());
			}
			else if (setter is FieldInfo)
			{
				FieldInfo fi = (FieldInfo)setter;

				emit.ldarg_1.end();

				if (propertyType.IsValueType && !fi.FieldType.IsValueType)
					emit.box(propertyType);

				emit.stfld(fi);
			}
			else
			{
				MethodInfo      mi = (MethodInfo)setter;
				ParameterInfo[] pi = mi.GetParameters();
				bool gotValueParam = false;

				for (int k = 0; k < pi.Length; k++)
				{
					ParameterInfo p = pi[k];

					if (p.IsDefined(typeof (ParentAttribute), true))
					{
						// Parent - set this.
						//
						emit.ldarg_0.end();

						if (!TypeHelper.IsSameOrParent(p.ParameterType, Context.Type))
							emit.castclass(p.ParameterType);
					}
					else if (p.IsDefined(typeof (PropertyInfoAttribute), true))
					{
						// PropertyInfo.
						//
						emit.ldsfld(GetPropertyInfoField()).end();
					}
					else if (!gotValueParam)
					{
						// This must be a value.
						//
						emit.ldarg_1.end();

						if (propertyType.IsValueType && !p.ParameterType.IsValueType)
							emit.box(propertyType);

						gotValueParam = true;
					}
					else
						throw new TypeBuilderException(string.Format(
							Resources.TypeBuilder_UnknownParameterType,
							mi.Name, mi.DeclaringType.FullName, p.Name));
				}

				if (InstanceType.IsValueType) emit.call(mi);
				else                          emit.callvirt(mi);
			}
		}

		private MemberInfo GetGetter()
		{
			const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

			Type propertyType = Context.CurrentProperty.PropertyType;

			FieldInfo[] fields = InstanceType.GetFields(bindingFlags);

			foreach (FieldInfo field in fields)
			{
				object[] attrs = field.GetCustomAttributes(typeof(GetValueAttribute), true);

				if (attrs.Length > 0 && TypeHelper.IsSameOrParent(field.FieldType, propertyType))
					return field;
			}

			PropertyInfo[] props = InstanceType.GetProperties(bindingFlags);

			foreach (PropertyInfo prop in props)
			{
				object[] attrs = prop.GetCustomAttributes(typeof(GetValueAttribute), true);

				if (attrs.Length > 0 && TypeHelper.IsSameOrParent(prop.PropertyType, propertyType))
					return prop;
			}

			foreach (FieldInfo field in fields)
				if (field.Name == "Value" && TypeHelper.IsSameOrParent(field.FieldType, propertyType))
					return field;

			foreach (PropertyInfo prop in props)
				if (prop.Name == "Value" && TypeHelper.IsSameOrParent(prop.PropertyType, propertyType))
					return prop;

			MethodInfo method = TypeHelper.GetMethod(InstanceType, false, "GetValue", bindingFlags);

			if (method != null && TypeHelper.IsSameOrParent(propertyType, method.ReturnType))
				return method;

			throw new TypeBuilderException(string.Format(
				Resources.TypeBuilder_CannotGetGetter, InstanceType.FullName,
				propertyType.FullName, Context.CurrentProperty.Name, Context.Type.FullName));
		}

		private MemberInfo GetSetter()
		{
			const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

			Type propertyType = Context.CurrentProperty.PropertyType;

			FieldInfo[] fields = InstanceType.GetFields(bindingFlags);

			foreach (FieldInfo field in fields)
			{
				object[] attrs = field.GetCustomAttributes(typeof(SetValueAttribute), true);

				if (attrs.Length > 0 && TypeHelper.IsSameOrParent(field.FieldType, propertyType))
					return field;
			}

			PropertyInfo[] props = InstanceType.GetProperties(bindingFlags);

			foreach (PropertyInfo prop in props)
			{
				object[] attrs = prop.GetCustomAttributes(typeof(SetValueAttribute), true);

				if (attrs.Length > 0 && TypeHelper.IsSameOrParent(prop.PropertyType, propertyType))
					return prop;
			}

			foreach (FieldInfo field in fields)
				if (field.Name == "Value" && TypeHelper.IsSameOrParent(field.FieldType, propertyType))
					return field;

			foreach (PropertyInfo prop in props)
				if (prop.Name == "Value" && TypeHelper.IsSameOrParent(prop.PropertyType, propertyType))
					return prop;

			MethodInfo method = TypeHelper.GetMethod(InstanceType, false, "SetValue", bindingFlags);

			if (method != null && method.ReturnType == typeof(void))
				return method;

			throw new TypeBuilderException(string.Format(
				Resources.TypeBuilder_CannotGetSetter, InstanceType.FullName,
				propertyType.FullName, Context.CurrentProperty.Name, Context.Type.FullName));
		}
	}
}
