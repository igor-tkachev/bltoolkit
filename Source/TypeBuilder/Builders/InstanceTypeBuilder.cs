using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	class InstanceTypeBuilder : DefaultTypeBuilder
	{
		public InstanceTypeBuilder(Type instanceType)
		{
			_instanceType = instanceType;
		}

		public InstanceTypeBuilder(Type propertyType, Type instanceType)
		{
			_propertyType = propertyType;
			_instanceType = instanceType;
		}

		private Type _propertyType;
		public  Type  PropertyType
		{
			get { return _propertyType; }
		}

		private Type _instanceType;
		public  Type  InstanceType
		{
			get { return _instanceType; }
		}

		public override bool IsApplied(BuildContext context)
		{
			if (context == null) throw new ArgumentNullException("context");

			return
				base.IsApplied(context) &&
				context.CurrentProperty != null &&
				context.CurrentProperty.GetIndexParameters().Length == 0 &&
				(PropertyType == null ||
				 TypeHelper.IsSameOrParent(PropertyType, context.CurrentProperty.PropertyType));
		}

		protected override Type GetFieldType()
		{
			return InstanceType;
		}

		[SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
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
			else
			{
				FieldInfo fi = (FieldInfo)getter;

				emit.ldfld(fi);

				memberType = fi.FieldType;
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

		[SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
		protected override void BuildAbstractSetter()
		{
			FieldBuilder field        = GetField();
			EmitHelper   emit         = Context.MethodBuilder.Emitter;
			Type         propertyType = Context.CurrentProperty.PropertyType;
			MemberInfo   setter       = GetSetter();

			if (InstanceType.IsValueType) emit.ldarg_0.ldflda(field);
			else                          emit.ldarg_0.ldfld (field);

			emit.ldarg_1.end();

			if (setter is PropertyInfo)
			{
				PropertyInfo pi = ((PropertyInfo)setter);

				if (propertyType.IsValueType && !pi.PropertyType.IsValueType)
					emit.box(propertyType);

				if (InstanceType.IsValueType) emit.call    (pi.GetSetMethod());
				else                          emit.callvirt(pi.GetSetMethod());
			}
			else
			{
				FieldInfo fi = (FieldInfo)setter;

				if (propertyType.IsValueType && !fi.FieldType.IsValueType)
					emit.box(propertyType);

				emit.stfld(fi);
			}
		}

		private MemberInfo GetGetter()
		{
			Type propertyType = Context.CurrentProperty.PropertyType;

			FieldInfo[] fields = InstanceType.GetFields(
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField);

			foreach (FieldInfo field in fields)
			{
				object[] attrs = field.GetCustomAttributes(typeof(GetValueAttribute), true);

				if (attrs.Length > 0 && TypeHelper.IsSameOrParent(field.FieldType, propertyType))
					return field;
			}

			PropertyInfo[] props = InstanceType.GetProperties(
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

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

			throw new TypeBuilderException(string.Format(
				(IFormatProvider)null,
				"The '{0}' type does not have appropriate getter. See '{1}' member of '{2}' type.",
				InstanceType.FullName,
				propertyType.FullName,
				Context.Type.FullName));
		}

		private MemberInfo GetSetter()
		{
			Type propertyType = Context.CurrentProperty.PropertyType;

			FieldInfo[] fields = InstanceType.GetFields(
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField);

			foreach (FieldInfo field in fields)
			{
				object[] attrs = field.GetCustomAttributes(typeof(SetValueAttribute), true);

				if (attrs.Length > 0 && TypeHelper.IsSameOrParent(field.FieldType, propertyType))
					return field;
			}

			PropertyInfo[] props = InstanceType.GetProperties(
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

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

			throw new TypeBuilderException(string.Format(
				(IFormatProvider)null,
				"The '{0}' type does not have appropriate setter. See '{1}' member of '{2}' type.",
				InstanceType.FullName,
				propertyType.FullName,
				Context.Type.FullName));
		}
	}
}
