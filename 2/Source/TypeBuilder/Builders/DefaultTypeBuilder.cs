using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection.Emit;
using BLToolkit.Reflection;

namespace BLToolkit.TypeBuilder.Builders
{
	public class DefaultTypeBuilder : AbstractTypeBuilderBase
	{
		#region Interface Overrides

		public override bool IsCompatible(BuildContext context, ITypeBuilder typeBuilder)
		{
			return IsRelative(typeBuilder) == false;
		}

		public override bool IsApplied(BuildContext context)
		{
			if (context.IsAbstractProperty && context.IsBeforeOrBuildStep)
			{
				return context.CurrentProperty.GetIndexParameters().Length <= 1;
			}

			return false;
		}

		#endregion

		#region Get/Set Implementation

		protected override void BuildAbstractGetter()
		{
			FieldBuilder    field = GetField();
			ParameterInfo[] index = Context.CurrentProperty.GetIndexParameters();

			switch (index.Length)
			{
				case 0:
					Context.MethodBuilder.Emitter
						.ldarg_0
						.ldfld   (field)
						.stloc   (Context.ReturnValue)
						;
					break;

				case 1:
					Context.MethodBuilder.Emitter
						.ldarg_0
						.ldfld          (field)
						.ldarg_1
						.boxIfValueType (index[0].ParameterType)
						.callvirt       (typeof(Hashtable), "get_Item", typeof(object))
						.castType       (Context.CurrentProperty.PropertyType)
						.stloc          (Context.ReturnValue)
						;
					break;
			}
		}

		protected override void BuildAbstractSetter()
		{
			FieldBuilder    field = GetField();
			ParameterInfo[] index = Context.CurrentProperty.GetIndexParameters();

			switch (index.Length)
			{
				case 0:
					Context.MethodBuilder.Emitter
						.ldarg_0
						.ldarg_1
						.stfld(field)
						;
					break;

				case 1:
					Context.MethodBuilder.Emitter
						.ldarg_0
						.ldfld          (field)
						.ldarg_1
						.boxIfValueType (index[0].ParameterType)
						.ldarg_2
						.boxIfValueType (Context.CurrentProperty.PropertyType)
						.callvirt       (typeof(Hashtable), "set_Item", typeof(object), typeof(object))
					;
					break;
			}
		}

		#endregion

		#region Call Base Method

		protected override void BuildVirtualGetter()
		{
			CallBaseMethod();
		}

		protected override void BuildVirtualSetter()
		{
			CallBaseMethod();
		}

		protected override void BuildVirtualMethod()
		{
			CallBaseMethod();
		}

		private void CallBaseMethod()
		{
			EmitHelper      emit   = Context.MethodBuilder.Emitter;
			MethodInfo      method = Context.MethodBuilder.OverriddenMethod;
			ParameterInfo[] ps     = method.GetParameters();

			emit.ldarg_0.end();

			for (int i = 0; i < ps.Length; i++)
				emit.ldarg(i + 1);

			emit
				.call  (method)
				.stloc (Context.ReturnValue);
		}

		#endregion

		#region Properties

		private   TypeHelper _initContextType;
		protected TypeHelper  InitContextType
		{
			get
			{
				if (_initContextType == null)
					_initContextType = new TypeHelper(typeof(InitContext));

				return _initContextType;
			}
		}

		#endregion

		#region Field Initialization

		protected override void BeforeBuildAbstractGetter()
		{
			FieldBuilder        field   = GetField();
			MethodBuilderHelper ensurer = Context.GetFieldInstanceEnsurer(field.Name);

			if (ensurer != null)
			{
				Context.MethodBuilder.Emitter
					.ldarg_0
					.call(ensurer);
			}
		}

		protected override void BeforeBuildAbstractSetter()
		{
			FieldBuilder field = GetField();

			if (field.FieldType != Context.CurrentProperty.PropertyType)
			{
				MethodBuilderHelper ensurer = Context.GetFieldInstanceEnsurer(field.Name);

				if (ensurer != null)
				{
					Context.MethodBuilder.Emitter
						.ldarg_0
						.call    (ensurer)
						;
				}
			}
		}

		public FieldBuilder GetField()
		{
			PropertyInfo propertyInfo = Context.CurrentProperty;
			string       fieldName    = GetFieldName();
			Type         fieldType    = GetFieldType();
			FieldBuilder field        = Context.GetField(fieldName);

			if (field == null)
			{
				field = Context.CreatePrivateField(fieldName, fieldType);

				if (propertyInfo.GetCustomAttributes(typeof(NoInstanceAttribute), true).Length == 0)
				{
					if (fieldType.IsClass && IsLazyInstance())
					{
						BuildLazyInstanceEnsurer();
					}

					/*
					ParameterInfo[] index = propertyInfo.GetIndexParameters();
					Type[]          types = index.Length == 0? Type.EmptyTypes: new Type[index.Length];
					*/

					object[] parameters = GetPropertyParameters(propertyInfo);


				}
			}

			return field;
		}

		public FieldBuilder GetParameterField()
		{
			string       fieldName = GetFieldName() + "_$parameters";
			FieldBuilder field     = Context.GetField(fieldName);
			PropertyInfo property  = Context.CurrentProperty;

			if (field == null)
			{
				field = Context.CreatePrivateStaticField(fieldName, typeof(object[]));

				EmitHelper emit = Context.TypeBuilder.TypeInitializer.Emitter;

				ParameterInfo[] index      = property.GetIndexParameters();
				LocalBuilder    parameters = null;

				if (index.Length > 0)
				{
					parameters = (LocalBuilder)Context.Items["$BLToolkit.ParameterLocalBuilder."];

					if (parameters == null)
					{
						parameters = emit.DeclareLocal(typeof(Type[]));

						Context.Items["$BLToolkit.ParameterLocalBuilder."] = parameters;
					}
				}

				emit
					.LoadType (Context.OriginalType)
					.ldstr    (property.Name)
					.LoadType (property.PropertyType)
					;

				if (index.Length == 0)
				{
					emit
						.ldsfld (typeof(Type).GetField("EmptyTypes"))
						;
				}
				else
				{
					emit
						.ldc_i4 (index.Length) 
						.newarr (typeof(Type))
						.stloc  (parameters)
						;

					for (int i = 0; i < index.Length; i++)
						emit
							.ldloc      (parameters)
							.ldc_i4     (i) 
							.LoadType   (index[i].ParameterType)
							.stelem_ref
							.end()
							;

					emit.ldloc(parameters);
				}

				emit
					.call   (typeof(DefaultTypeBuilder).GetMethod("GetPropertyParameters"))
					.stsfld (field)
					;
			}

			return field;
		}

		public FieldBuilder GetTypeAccessorField()
		{
			string       fieldName = "_" + GetFieldType().Name + "_$typeAccessor";
			FieldBuilder field     = Context.GetField(fieldName);
			PropertyInfo property  = Context.CurrentProperty;

			if (field == null)
			{
				field = Context.CreatePrivateStaticField(fieldName, typeof(TypeAccessor));

				EmitHelper emit = Context.TypeBuilder.TypeInitializer.Emitter;

				emit
					.LoadType (GetFieldType())
					.call     (typeof(TypeAccessor), "GetAccessor", typeof(Type))
					.stsfld   (field)
					;
			}

			return field;
		}

		private bool IsLazyInstance()
		{
			object[] attrs = 
				Context.CurrentProperty.GetCustomAttributes(typeof(LazyInstanceAttribute), true);

			if (attrs.Length > 0)
				return ((LazyInstanceAttribute)attrs[0]).IsLazy;

			attrs = Context.OriginalType.GetAttributes(typeof(LazyInstancesAttribute));

			return attrs.Length > 0? ((LazyInstancesAttribute)attrs[0]).IsLazy: false;
		}

		private void BuildLazyInstanceEnsurer()
		{
			string              fieldName = GetFieldName();
			FieldBuilder        field     = Context.GetField(fieldName);
			TypeHelper          fieldType = new TypeHelper(field.FieldType);
			MethodBuilderHelper ensurer   = Context.TypeBuilder.DefineMethod(
				string.Format("EnsureInstance${0}", fieldName), 
				MethodAttributes.Private | MethodAttributes.HideBySig);

			EmitHelper emit = ensurer.Emitter;
			Label      end  = emit.DefineLabel();

			emit
				.ldarg_0
				.ldfld    (field)
				.brtrue_s (end)
				;

			object[] parameters = GetPropertyParameters(Context.CurrentProperty);

			if (fieldType.IsAbstract)
			{
				CreateAbstractLazyInstance("$InitContextA$" + field.Name, field, fieldType, emit, parameters);
			}
			else
			{
				ConstructorInfo  ci = fieldType.GetPublicConstructor(typeof(InitContext));

				if (ci != null)
				{
					CreateInitContextLazyInstance("$InitContext$" + field.Name, field, fieldType, emit, parameters);
				}
				else
				{
					if (parameters == null)
					{
						CreateDefaultInstance(field, fieldType, emit);
					}
					else
					{
						CreateParametrizedInstance(field, fieldType, emit, parameters);
					}
				}
			}

			emit
				.MarkLabel(end)
				.ret()
				;

			Context.Items.Add("$BLToolkit.FieldInstanceEnsurer." + fieldName, ensurer);
		}

		private void CreateAbstractLazyInstance(
			string initContextName, FieldBuilder field, TypeHelper fieldType, EmitHelper emit, object[] parameters)
		{
			initContextName = "$BLToolkit." + initContextName;

			LocalBuilder initField = (LocalBuilder)Context.Items[initContextName];

			if (initField == null)
			{
				Context.Items[initContextName] = initField = emit.DeclareLocal(InitContextType);

				emit
					.newobj   (InitContextType.GetPublicDefaultConstructor())
					.stloc    (initField)

					.ldloc    (initField)
					.ldc_i4_1
					.callvirt (InitContextType.GetProperty("IsInternal").GetSetMethod())

					.ldloc    (initField)
					.ldc_i4_1
					.callvirt (InitContextType.GetProperty("IsLazyInstance").GetSetMethod())
					;
			}

			MethodInfo memberParams = InitContextType.GetProperty("MemberParameters").GetSetMethod();

			if (parameters != null)
			{
				emit
					.ldloc    (initField)
					.ldsfld   (GetParameterField())
					.callvirt (memberParams)
					;
			}

			emit
				.ldarg_0
				.ldsfld             (GetTypeAccessorField())
				.ldloc              (initField)
				.callvirtNoGenerics (typeof(TypeAccessor), "CreateInstanceEx", _initContextType)
				.isinst             (fieldType)
				.stfld              (field)
				;

			if (parameters != null)
			{
				emit
					.ldloc    (initField)
					.ldnull
					.callvirt (memberParams)
					;
			}
		}

		private void CreateInitContextLazyInstance(
			string initContextName, FieldBuilder field, TypeHelper fieldType, EmitHelper emit, object[] parameters)
		{
			initContextName = "$BLToolkit." + initContextName;

			LocalBuilder initField = (LocalBuilder)Context.Items[initContextName];

			if (initField == null)
			{
				Context.Items[initContextName] = initField = emit.DeclareLocal(InitContextType);

				emit
					.newobj   (InitContextType.GetPublicDefaultConstructor())
					.stloc    (initField)

					.ldloc    (initField)
					.ldc_i4_1
					.callvirt (InitContextType.GetProperty("IsInternal").GetSetMethod())

					.ldloc    (initField)
					.ldc_i4_1
					.callvirt (InitContextType.GetProperty("IsLazyInstance").GetSetMethod())
					;
			}

			MethodInfo memberParams = InitContextType.GetProperty("MemberParameters").GetSetMethod();

			if (parameters != null)
			{
				emit
					.ldloc    (initField)
					.ldsfld   (GetParameterField())
					.callvirt (memberParams)
					;
			}

			emit
				.ldarg_0
				.ldloc   (initField)
				.newobj  (fieldType.GetPublicConstructor(typeof(InitContext)))
				.stfld   (field)
				;

			if (parameters != null)
			{
				emit
					.ldloc    (initField)
					.ldnull
					.callvirt (memberParams)
					;
			}
		}

		private void CreateDefaultInstance(FieldBuilder field, TypeHelper fieldType, EmitHelper emit)
		{
			if (fieldType.Type == typeof(string))
			{
				emit
					.ldarg_0
					.LoadInitValue(fieldType)
					.stfld(field)
					;
			}
			else
			{
				ConstructorInfo ci = fieldType.GetPublicDefaultConstructor();

				if (ci == null)
				{
					throw new TypeBuilderException(
						string.Format("Could not build the '{0}' property of the '{1}' type: type '{2}' has to have public default constructor.",
							Context.CurrentProperty.Name,
							Context.Type.FullName,
							fieldType.FullName));
				}

				emit
					.ldarg_0
					.newobj(ci)
					.stfld(field)
					;
			}
		}

		private void CreateParametrizedInstance(
			FieldBuilder field, TypeHelper fieldType, EmitHelper emit, object[] parameters)
		{
			if (fieldType.Type == typeof(string) && parameters.Length == 1 && parameters[0] is string)
			{
				emit
					.ldarg_0
					.ldstr   ((string)parameters[0])
					.stfld   (field)
					;

				return;
			}

			Type[] types = new Type[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
				types[i] = parameters[i] != null? parameters[i].GetType(): typeof(object);

			ConstructorInfo ci = fieldType.GetPublicConstructor(types);

			if (ci == null)
			{
				throw new TypeBuilderException(
					string.Format("Could not build the '{0}' property of the '{1}' type: constructor not found for the '{2}' type.",
						Context.CurrentProperty.Name,
						Context.Type.FullName,
						fieldType.FullName));
			}

			ParameterInfo[] pi = ci.GetParameters();

			emit.ldarg_0.end();

			for (int i = 0; i < parameters.Length; i++)
			{
				object o = parameters[i];

				if (emit.LoadWellKnownValue(o))
				{
					if (o != null && o.GetType().IsValueType)
					{
						if (!pi[i].ParameterType.IsValueType)
							emit.box(o.GetType());
						else if (o.GetType() != pi[i].ParameterType)
							emit.conv(pi[i].ParameterType);
					}
				}
				else
				{
					emit
						.ldsfld     (GetParameterField())
						.ldc_i4     (i)
						.ldelem_ref
						.CastTo     (types[i])
						;
				}
			}

			emit
				.newobj (ci)
				.stfld  (field)
				;
		}

		protected virtual string GetFieldName()
		{
			PropertyInfo pi   = Context.CurrentProperty;
			string       name = pi.Name;

			if (char.IsUpper(name[0]) && name.Length > 1 && char.IsLower(name[1]))
				name = char.ToLower(name[0]) + name.Substring(1, name.Length - 1);

			name = "_" + name;

			foreach (ParameterInfo p in pi.GetIndexParameters())
				name += "." + p.ParameterType.FullName;//.Replace(".", "_").Replace("+", "_");

			return name;
		}

		protected virtual Type GetFieldType()
		{
			PropertyInfo    pi    = Context.CurrentProperty;
			ParameterInfo[] index = pi.GetIndexParameters();

			switch (index.Length)
			{
				case 0: return pi.PropertyType;
				case 1: return typeof(Hashtable);
				default:
					throw new InvalidOperationException();
			}
		}

		#endregion

		#region Public Helpers

		private static object[] GetPropertyParameters(PropertyInfo propertyInfo)
		{
			object[] attrs = propertyInfo.GetCustomAttributes(typeof(ParameterAttribute), true);

			return attrs == null || attrs.Length == 0? null: ((ParameterAttribute)attrs[0]).Parameters;
		}

		public static object[] GetPropertyParameters(
			Type type, string propertyName, Type returnType, Type[] types)
		{
			PropertyInfo pi = type.GetProperty(
				propertyName,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
				null,
				returnType,
				types,
				null);

			return pi == null? null: GetPropertyParameters(pi);
		}

		#endregion
	}
}
