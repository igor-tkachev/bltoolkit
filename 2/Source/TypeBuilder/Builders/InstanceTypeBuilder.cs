using System;
using System.Reflection;

using BLToolkit.Reflection.Emit;
using System.Reflection.Emit;
using BLToolkit.Reflection;

namespace BLToolkit.TypeBuilder.Builders
{
	class InstanceTypeBuilder : DefaultTypeBuilder
	{
		public InstanceTypeBuilder(Type instanceType)
		{
			_instanceType = instanceType;
		}

		private Type _instanceType;
		public  Type  InstanceType
		{
			get { return _instanceType; }
		}

		public override bool IsApplied (BuildContext context)
		{
			return 
				base.IsApplied(context) &&
				context.CurrentProperty != null &&
				context.CurrentProperty.GetIndexParameters().Length == 0;
		}
		protected override Type GetFieldType()
		{
			return InstanceType;
		}

		protected override void BuildAbstractGetter()
		{
			FieldBuilder field        = GetField();
			EmitHelper   emit         = Context.MethodBuilder.Emitter;
			Type         propertyType = Context.CurrentProperty.PropertyType;
			MemberInfo   getter       = GetGetter();

			if (InstanceType.IsValueType) emit.ldarg_0.ldflda(field);
			else                          emit.ldarg_0.ldfld (field);

			if (getter is PropertyInfo)
			{
				PropertyInfo pi = ((PropertyInfo)getter);

				if (InstanceType.IsValueType) emit.call    (pi.GetGetMethod());
				else                          emit.callvirt(pi.GetGetMethod());

				if (propertyType.IsValueType && !pi.PropertyType.IsValueType)
					emit.CastTo(propertyType);
				else if (pi.PropertyType != propertyType && !propertyType.IsEnum)
					emit.castclass(propertyType);
			}
			else
			{
				FieldInfo fi = (FieldInfo)getter;

				emit.ldfld((FieldInfo)getter);

				if (propertyType.IsValueType && !fi.FieldType.IsValueType)
					emit.CastTo(propertyType);
				else if (fi.FieldType != propertyType && !propertyType.IsEnum)
					emit.castclass(fi.FieldType);
			}
		}

		protected override void BuildAbstractSetter()
		{
			FieldBuilder field        = GetField();
			EmitHelper   emit         = Context.MethodBuilder.Emitter;
			Type         propertyType = Context.CurrentProperty.PropertyType;
			MemberInfo   setter       = GetGetter();

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
				if (field.Name == "Value" && TypeHelper.IsSameOrParent(field.FieldType, propertyType))
					return field;

			PropertyInfo[] props = InstanceType.GetProperties(
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

			foreach (PropertyInfo prop in props)
				if (prop.Name == "Value" && TypeHelper.IsSameOrParent(prop.PropertyType, propertyType))
					return prop;

			throw new TypeBuilderException(string.Format(
				"The '{0}' type does not have appropriate getter. " +
				"See '{1}' member of '{2}' type.",
				InstanceType.FullName,
				propertyType.FullName,
				Context.OriginalType.FullName));
		}

		private MemberInfo GetSetter()
		{
			Type propertyType = Context.CurrentProperty.PropertyType;

			FieldInfo[] fields = InstanceType.GetFields(
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField);

			foreach (FieldInfo field in fields)
				if (field.Name == "Value" && TypeHelper.IsSameOrParent(field.FieldType, propertyType))
					return field;

			PropertyInfo[] props = InstanceType.GetProperties(
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

			foreach (PropertyInfo prop in props)
				if (prop.Name == "Value" && TypeHelper.IsSameOrParent(prop.PropertyType, propertyType))
					return prop;

			throw new TypeBuilderException(string.Format(
				"The '{0}' type does not have appropriate setter. " +
				"See '{1}' member of '{2}' type.",
				InstanceType.FullName,
				propertyType.FullName,
				Context.OriginalType.FullName));
		}
	}
}
