using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace BLToolkit.TypeBuilder
{
	public class DefaultTypeBuilder : AbstractTypeBuilderBase
	{
		public override bool IsCompatible (BuildContext context, ITypeBuilder typeBuilder)
		{
			return IsRelative(typeBuilder) == false;
		}

		public override bool IsApplied(BuildContext context)
		{
			if (context.IsAbstractProperty && context.Step == BuildStep.Build)
			{
				return context.CurrentProperty.GetIndexParameters().Length <= 1;
			}

			return false;
		}

		protected override void BuildAbstractGetter(BuildContext context)
		{
			FieldBuilder    field = GetField(context);
			ParameterInfo[] index = context.CurrentProperty.GetIndexParameters();

			switch (index.Length)
			{
				case 0:
					context.MethodBuilder.Emitter
						.ldarg_0
						.ldfld(field)
						.stloc(context.ReturnValue)
						;
					break;

				case 1:
					context.MethodBuilder.Emitter
						.ldarg_0
						.ldfld     (field)
						.ldarg_1
						.boxIfValueType(index[0].ParameterType)
						.callvirt  (typeof(Hashtable), "get_Item", typeof(object))
						.castType  (context.CurrentProperty.PropertyType)
						.stloc     (context.ReturnValue)
						;
					break;
			}
		}

		protected override void BuildAbstractSetter(BuildContext context)
		{
			FieldBuilder    field = GetField(context);
			ParameterInfo[] index = context.CurrentProperty.GetIndexParameters();

			switch (index.Length)
			{
				case 0:
					context.MethodBuilder.Emitter
						.ldarg_0
						.ldarg_1
						.stfld(field)
						;
					break;

				case 1:
					context.MethodBuilder.Emitter
						.ldarg_0
						.ldfld     (field)
						.ldarg_1
						.boxIfValueType(index[0].ParameterType)
						.ldarg_2
						.boxIfValueType(context.CurrentProperty.PropertyType)
						.callvirt(typeof(Hashtable), "set_Item", typeof(object), typeof(object))
					;
					break;
			}
		}

		protected override void BuildVirtualGetter(BuildContext context)
		{
		}

		protected override void BuildVirtualSetter(BuildContext context)
		{
		}

		protected override void BuildVirtualMethod(BuildContext context)
		{
		}

		private FieldBuilder GetField(BuildContext context)
		{
			PropertyInfo propertyInfo = context.CurrentProperty;

			return context.GetField(
				GetFieldName(context),
				GetFieldType(context),
				FieldAttributes.Private);
		}

		protected virtual string GetFieldName(BuildContext context)
		{
			PropertyInfo pi   = context.CurrentProperty;
			string       name = "_" + pi.Name;

			foreach (ParameterInfo p in pi.GetIndexParameters())
				name += "_" + p.ParameterType.FullName.Replace(".", "_").Replace("+", "_");

			return name;
		}

		protected virtual Type GetFieldType(BuildContext context)
		{
			PropertyInfo    pi    = context.CurrentProperty;
			ParameterInfo[] index = pi.GetIndexParameters();

			switch (index.Length)
			{
				case 0: return pi.PropertyType;
				case 1: return typeof(Hashtable);
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
