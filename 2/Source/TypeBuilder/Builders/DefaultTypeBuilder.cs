using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	public class DefaultTypeBuilder : AbstractTypeBuilderBase
	{
		#region Interface Overrides

		public override bool IsCompatible(BuildContext context, IAbstractTypeBuilder typeBuilder)
		{
			return IsRelative(typeBuilder) == false;
		}

		public override bool IsApplied(BuildContext context)
		{
			if (context.IsAbstractProperty && context.IsBeforeOrBuildStep)
			{
				return context.CurrentProperty.GetIndexParameters().Length <= 1;
			}

			return context.BuildElement == BuildElement.Type && context.IsAfterStep;
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
						.stfld   (field)
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

			emit.call(method);

			if (Context.ReturnValue != null)
				emit.stloc(Context.ReturnValue);
		}

		#endregion

		#region Properties

		private   static TypeHelper _initContextType;
		protected static TypeHelper  InitContextType
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

		#region Overrides

		protected override void BeforeBuildAbstractGetter()
		{
			CallLazyInstanceInsurer(GetField());
		}

		protected override void BeforeBuildAbstractSetter()
		{
			FieldBuilder field = GetField();

			if (field.FieldType != Context.CurrentProperty.PropertyType)
				CallLazyInstanceInsurer(field);
		}

		#endregion

		#region Common

		protected FieldBuilder GetField()
		{
			PropertyInfo propertyInfo = Context.CurrentProperty;
			string       fieldName    = GetFieldName();
			Type         fieldType    = GetFieldType();
			FieldBuilder field        = Context.GetField(fieldName);

			if (field == null)
			{
				field = Context.CreatePrivateField(fieldName, fieldType);

				if (fieldType.IsInterface == false &&
					propertyInfo.GetCustomAttributes(typeof(NoInstanceAttribute), true).Length == 0)
				{
					if (fieldType.IsClass && IsLazyInstance(fieldType))
					{
						BuildLazyInstanceEnsurer();
					}
					else
					{
						BuildDefaultInstance();
						BuildInitContextInstance();
					}
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
					.LoadType (Context.Type)
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

		#region Build

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
					if (fieldType.Type.IsValueType)
						return;

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
			if (parameters.Length == 1)
			{
				object o = parameters[0];

				if (o == null)
				{
					if (fieldType.IsValueType == false)
						emit
							.ldarg_0
							.ldnull
							.stfld   (field)
							;

					return;
				}
				else
				{
					if (fieldType.Type == o.GetType())
					{
						if (fieldType.Type == typeof(string))
						{
							emit
								.ldarg_0
								.ldstr   ((string)o)
								.stfld   (field)
								;

							return;
						}

						if (fieldType.IsValueType)
						{
							emit.ldarg_0.end();

							if (emit.LoadWellKnownValue(o) == false)
							{
								emit
									.ldsfld     (GetParameterField())
									.ldc_i4_0
									.ldelem_ref
									.end()
									;
							}

							emit.stfld(field);

							return;
						}
					}
				}
			}

			Type[] types = new Type[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
				types[i] = parameters[i] != null? parameters[i].GetType(): typeof(object);

			ConstructorInfo ci = fieldType.GetPublicConstructor(types);

			if (ci == null)
			{
				if (fieldType.IsValueType)
					return;

				throw new TypeBuilderException(
					string.Format("Could not build the '{0}' property of the '{1}' type: {2}constructor not found for the '{3}' type.",
						Context.CurrentProperty.Name,
						Context.Type.FullName,
						types.Length == 0? "default ": "",
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
						.ldsfld         (GetParameterField())
						.ldc_i4         (i)
						.ldelem_ref
						.CastFromObject (types[i])
						;
				}
			}

			emit
				.newobj (ci)
				.stfld  (field)
				;
		}

		#endregion

		#region Build InitContext Instance

		private void BuildInitContextInstance()
		{
			string       fieldName = GetFieldName();
			FieldBuilder field     = Context.GetField(fieldName);
			TypeHelper   fieldType = new TypeHelper(field.FieldType);

			EmitHelper emit = Context.TypeBuilder.InitConstructor.Emitter;

			object[] parameters = GetPropertyParameters(Context.CurrentProperty);
			ConstructorInfo  ci = fieldType.GetPublicConstructor(typeof(InitContext));

			if (ci != null || fieldType.IsAbstract)
				CreateAbstractInitContextInstance(field, fieldType, emit, parameters);
			else if (parameters == null)
				CreateDefaultInstance(field, fieldType, emit);
			else
				CreateParametrizedInstance(field, fieldType, emit, parameters);
		}

		private void CreateAbstractInitContextInstance(
			FieldBuilder field, TypeHelper fieldType, EmitHelper emit, object[] parameters)
		{
			MethodInfo memberParams = InitContextType.GetProperty("MemberParameters").GetSetMethod();

			LocalBuilder parentField = (LocalBuilder)Context.Items["$BLToolkit.InitContext.Parent"];

			if (parentField == null)
			{
				Context.Items["$BLToolkit.InitContext.Parent"] = parentField = emit.DeclareLocal(typeof(object));

				Label label = emit.DefineLabel();

				emit
					.ldarg_1
					.brtrue_s(label)

					.newobj(InitContextType.GetPublicDefaultConstructor())
					.starg(1)

					.ldarg_1
					.ldc_i4_1
					.callvirt(InitContextType.GetProperty("IsInternal").GetSetMethod())

					.MarkLabel(label)

					.ldarg_1
					.callvirt (InitContextType.GetProperty("Parent").GetGetMethod())
					.stloc    (parentField)
					;
			}

			emit
				.ldarg_1
				.ldarg_0
				.callvirt (InitContextType.GetProperty("Parent").GetSetMethod())
				;

			object isDirty = Context.Items["$BLToolkit.InitContext.DirtyParameters"];

			if (parameters != null)
			{
				emit
					.ldarg_1
					.ldsfld   (GetParameterField())
					.callvirt (memberParams)
					;
			}
			else if (isDirty != null && (bool)isDirty)
			{
				emit
					.ldarg_1
					.ldnull
					.callvirt (memberParams)
					;
			}

			Context.Items["$BLToolkit.InitContext.DirtyParameters"] = parameters != null;

			if (fieldType.IsAbstract)
			{
				emit
					.ldarg_0
					.ldsfld             (GetTypeAccessorField())
					.ldarg_1
					.callvirtNoGenerics (typeof(TypeAccessor), "CreateInstanceEx", _initContextType)
					.isinst             (fieldType)
					.stfld              (field)
					;
			}
			else
			{
				emit
					.ldarg_0
					.ldarg_1
					.newobj  (fieldType.GetPublicConstructor(typeof(InitContext)))
					.stfld   (field)
					;
			}
		}

		#endregion

		#region Build Default Instance

		private void BuildDefaultInstance()
		{
			string       fieldName = GetFieldName();
			FieldBuilder field     = Context.GetField(fieldName);
			TypeHelper   fieldType = new TypeHelper(field.FieldType);

			EmitHelper       emit = Context.TypeBuilder.DefaultConstructor.Emitter;
			object[]         ps   = GetPropertyParameters(Context.CurrentProperty);
			ConstructorInfo  ci   = fieldType.GetPublicConstructor(typeof(InitContext));

			if (ci != null || fieldType.IsAbstract)
				CreateInitContextDefaultInstance("$BLToolkit.DefaultInitContext.", field, fieldType, emit, ps);
			else if (ps == null)
				CreateDefaultInstance(field, fieldType, emit);
			else
				CreateParametrizedInstance(field, fieldType, emit, ps);
		}

		private void CreateInitContextDefaultInstance(
			string initContextName, FieldBuilder field, TypeHelper fieldType, EmitHelper emit, object[] parameters)
		{
			LocalBuilder initField    = GetInitContextBuilder(initContextName, emit);
			MethodInfo   memberParams = InitContextType.GetProperty("MemberParameters").GetSetMethod();

			if (parameters != null)
			{
				emit
					.ldloc    (initField)
					.ldsfld   (GetParameterField())
					.callvirt (memberParams)
					;
			}
			else if ((bool)Context.Items["$BLToolkit.Default.DirtyParameters"])
			{
				emit
					.ldloc    (initField)
					.ldnull
					.callvirt (memberParams)
					;
			}

			Context.Items["$BLToolkit.Default.DirtyParameters"] = parameters != null;

			if (fieldType.IsAbstract)
			{
				emit
					.ldarg_0
					.ldsfld             (GetTypeAccessorField())
					.ldloc              (initField)
					.callvirtNoGenerics (typeof(TypeAccessor), "CreateInstanceEx", _initContextType)
					.isinst             (fieldType)
					.stfld              (field)
					;
			}
			else
			{
				emit
					.ldarg_0
					.ldloc   (initField)
					.newobj  (fieldType.GetPublicConstructor(typeof(InitContext)))
					.stfld   (field)
					;
			}
		}

		private LocalBuilder GetInitContextBuilder(
			string initContextName, EmitHelper emit)
		{
			LocalBuilder initField = (LocalBuilder)Context.Items[initContextName];

			if (initField == null)
			{
				Context.Items[initContextName] = initField = emit.DeclareLocal(InitContextType);

				emit
					.newobj   (InitContextType.GetPublicDefaultConstructor())
					.stloc    (initField)

					.ldloc    (initField)
					.ldarg_0
					.callvirt (InitContextType.GetProperty("Parent").GetSetMethod())

					.ldloc    (initField)
					.ldc_i4_1
					.callvirt (InitContextType.GetProperty("IsInternal").GetSetMethod())
					;

				Context.Items.Add("$BLToolkit.Default.DirtyParameters", false);
			}

			return initField;
		}

		#endregion

		#region Build Lazy Instance

		private bool IsLazyInstance(Type type)
		{
			object[] attrs = 
				Context.CurrentProperty.GetCustomAttributes(typeof(LazyInstanceAttribute), true);

			if (attrs.Length > 0)
				return ((LazyInstanceAttribute)attrs[0]).IsLazy;

			attrs = Context.Type.GetAttributes(typeof(LazyInstancesAttribute));

			foreach (LazyInstancesAttribute a in attrs)
				if (a.Type == typeof(object) || type == a.Type || type.IsSubclassOf(a.Type))
					return a.IsLazy;

			return false;
		}

		private void BuildLazyInstanceEnsurer()
		{
			string              fieldName = GetFieldName();
			FieldBuilder        field     = Context.GetField(fieldName);
			TypeHelper          fieldType = new TypeHelper(field.FieldType);
			MethodBuilderHelper ensurer   = Context.TypeBuilder.DefineMethod(
				string.Format("$EnsureInstance{0}", fieldName), 
				MethodAttributes.Private | MethodAttributes.HideBySig);

			EmitHelper emit = ensurer.Emitter;
			Label      end  = emit.DefineLabel();

			emit
				.ldarg_0
				.ldfld    (field)
				.brtrue_s (end)
				;

			object[] parameters = GetPropertyParameters(Context.CurrentProperty);
			ConstructorInfo  ci = fieldType.GetPublicConstructor(typeof(InitContext));

			if (ci != null || fieldType.IsAbstract)
				CreateInitContextLazyInstance(field, fieldType, emit, parameters);
			else if (parameters == null)
				CreateDefaultInstance(field, fieldType, emit);
			else
				CreateParametrizedInstance(field, fieldType, emit, parameters);

			emit
				.MarkLabel(end)
				.ret()
				;

			Context.Items.Add("$BLToolkit.FieldInstanceEnsurer." + fieldName, ensurer);
		}

		private void CreateInitContextLazyInstance(
			FieldBuilder field, TypeHelper fieldType, EmitHelper emit, object[] parameters)
		{
			LocalBuilder initField = emit.DeclareLocal(InitContextType);

			emit
				.newobj   (InitContextType.GetPublicDefaultConstructor())
				.stloc    (initField)

				.ldloc    (initField)
				.ldarg_0
				.callvirt (InitContextType.GetProperty("Parent").GetSetMethod())

				.ldloc    (initField)
				.ldc_i4_1
				.callvirt (InitContextType.GetProperty("IsInternal").GetSetMethod())

				.ldloc    (initField)
				.ldc_i4_1
				.callvirt (InitContextType.GetProperty("IsLazyInstance").GetSetMethod())
				;

			if (parameters != null)
			{
				emit
					.ldloc    (initField)
					.ldsfld   (GetParameterField())
					.callvirt (InitContextType.GetProperty("MemberParameters").GetSetMethod())
					;
			}

			if (fieldType.IsAbstract)
			{
				emit
					.ldarg_0
					.ldsfld             (GetTypeAccessorField())
					.ldloc              (initField)
					.callvirtNoGenerics (typeof(TypeAccessor), "CreateInstanceEx", _initContextType)
					.isinst             (fieldType)
					.stfld              (field)
					;
			}
			else
			{
				emit
					.ldarg_0
					.ldloc   (initField)
					.newobj  (fieldType.GetPublicConstructor(typeof(InitContext)))
					.stfld   (field)
					;
			}
		}

		#endregion

		#region Finalize Type

		protected override void AfterBuildType()
		{
			object isDirty = Context.Items["$BLToolkit.InitContext.DirtyParameters"];

			if (isDirty != null && (bool)isDirty)
			{
				Context.TypeBuilder.InitConstructor.Emitter
					.ldarg_1
					.ldnull
					.callvirt (InitContextType.GetProperty("MemberParameters").GetSetMethod())
					;
			}

			LocalBuilder parentField = (LocalBuilder)Context.Items["$BLToolkit.InitContext.Parent"];

			if (parentField != null)
			{
				Context.TypeBuilder.InitConstructor.Emitter
					.ldarg_1
					.ldloc    (parentField)
					.callvirt (InitContextType.GetProperty("Parent").GetSetMethod())
					;
			}

			FinalizeDefaultConstructors();
			FinalizeInitContextConstructors();
		}

		private void FinalizeDefaultConstructors()
		{
			ConstructorInfo ci = Context.Type.GetDefaultConstructor();

			if (ci == null || Context.TypeBuilder.IsDefaultConstructorDefined)
			{
				EmitHelper emit = Context.TypeBuilder.DefaultConstructor.Emitter;

				if (ci != null)
				{
					emit.ldarg_0.call(ci);
				}
				else
				{
					ci = Context.Type.GetConstructor(typeof(InitContext));

					if (ci != null)
					{
						LocalBuilder initField = GetInitContextBuilder("$BLToolkit.DefaultInitContext.", emit);

						emit
							.ldarg_0
							.ldloc   (initField)
							.call    (ci);
					}
					else
					{
						if (Context.Type.GetConstructors().Length > 0)
							throw new TypeBuilderException(
								string.Format("Could not build the '{0}' type: default constructor not found.",
								Context.Type.FullName));
					}
				}
			}
		}

		private void FinalizeInitContextConstructors()
		{
			ConstructorInfo ci = Context.Type.GetConstructor(typeof(InitContext));

			if (ci != null || Context.TypeBuilder.IsInitConstructorDefined)
			{
				EmitHelper emit = Context.TypeBuilder.InitConstructor.Emitter;

				if (ci != null)
				{
					emit
						.ldarg_0
						.ldarg_1
						.call    (ci);
				}
				else
				{
					ci = Context.Type.GetDefaultConstructor();

					if (ci != null)
					{
						emit.ldarg_0.call(ci);
					}
					else
					{
						if (Context.Type.GetConstructors().Length > 0)
							throw new TypeBuilderException(
								string.Format("Could not build the '{0}' type: default constructor not found.",
								Context.Type.FullName));
					}
				}
			}
		}

		#endregion

		#endregion

		#region Public Helpers

		private static object[] GetPropertyParameters(PropertyInfo propertyInfo)
		{
			object[] attrs = propertyInfo.GetCustomAttributes(typeof(ParameterAttribute), true);

			if (attrs != null && attrs.Length > 0)
				return ((ParameterAttribute)attrs[0]).Parameters;

			attrs = propertyInfo.GetCustomAttributes(typeof(InstanceTypeAttribute), true);

			if (attrs == null || attrs.Length == 0)
			{
				attrs = new TypeHelper(
					propertyInfo.DeclaringType).GetAttributes(typeof(InstanceTypeAttribute));
			}

			if (attrs != null && attrs.Length > 0)
				return ((InstanceTypeAttribute)attrs[0]).Parameters;

			return null;
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
