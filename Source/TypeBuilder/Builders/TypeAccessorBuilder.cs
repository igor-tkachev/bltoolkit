using System;
using System.Collections;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	class TypeAccessorBuilder : ITypeBuilder
	{
		public TypeAccessorBuilder(Type type, Type originalType)
		{
			_type         = type;
			_originalType = originalType;
		}

		TypeHelper        _type;
		TypeHelper        _originalType;
		TypeHelper        _accessorType   = new TypeHelper(typeof(TypeAccessor));
		TypeHelper        _memberAccessor = new TypeHelper(typeof(MemberAccessor));
		ArrayList         _nestedTypes    = new ArrayList();
		TypeBuilderHelper _typeBuilder;

		public string AssemblyNameSuffix
		{
			get { return "TypeAccessor"; }
		}

		public Type Build(Type sourceType, AssemblyBuilderHelper assemblyBuilder)
		{
			if (assemblyBuilder == null) throw new ArgumentNullException("assemblyBuilder");

			string typeName = _type.FullName.Replace('+', '.')
#if !FW2
				.Replace('[', '_').Replace(']', '_')
#endif
				+ ".TypeAccessor";

			_typeBuilder = assemblyBuilder.DefineType(typeName, _accessorType);

			_typeBuilder.DefaultConstructor.Emitter
				.ldarg_0
				.call    (TypeHelper.GetDefaultConstructor(_accessorType))
				;

			BuildCreateInstanceMethods();
			BuildTypeProperties();
			BuildMembers();
			BuildObjectFactory();

			_typeBuilder.DefaultConstructor.Emitter
				.ret()
				;

			Type result = _typeBuilder.Create();

			foreach (TypeBuilderHelper tb in _nestedTypes)
				tb.Create();

			return result;
		}

		private void BuildCreateInstanceMethods()
		{
			ConstructorInfo baseDefCtor  = _type.GetPublicDefaultConstructor();
			ConstructorInfo baseInitCtor = _type.GetPublicConstructor(typeof(InitContext));

			if (baseDefCtor == null && baseInitCtor == null)
				return;

			// CreateInstance.
			//
			MethodBuilderHelper method = _typeBuilder.DefineMethod(
				_accessorType.GetMethod(
#if FW2
				false,
#endif
				"CreateInstance", Type.EmptyTypes));

			if (baseDefCtor != null)
			{
				method.Emitter
					.newobj (baseDefCtor)
					.ret()
					;
			}
			else
			{
				method.Emitter
					.ldnull
					.newobj (baseInitCtor)
					.ret()
					;
			}

			// CreateInstance(IniContext).
			//
			method = _typeBuilder.DefineMethod(
				_accessorType.GetMethod(
#if FW2
				false,
#endif
				"CreateInstance", typeof(InitContext)));

			if (baseInitCtor != null)
			{
				method.Emitter
					.ldarg_1
					.newobj (baseInitCtor)
					.ret()
					;
			}
			else
			{
				method.Emitter
					.newobj (baseDefCtor)
					.ret()
					;
			}
		}

		private void BuildTypeProperties()
		{
			// Type.
			//
			MethodBuilderHelper method =
				_typeBuilder.DefineMethod(_accessorType.GetProperty("Type").GetGetMethod());

			method.Emitter
				.LoadType(_type)
				.ret()
				;

			// OriginalType.
			//
			method = 
				_typeBuilder.DefineMethod(_accessorType.GetProperty("OriginalType").GetGetMethod());

			method.Emitter
				.LoadType(_originalType)
				.ret()
				;
		}

		private void BuildMembers()
		{
			foreach (MemberInfo mi in _type.GetFields())
				BuildMember(mi);

			foreach (PropertyInfo pi in _type.GetProperties())
				if (pi.GetIndexParameters().Length == 0)
					BuildMember(pi);
		}

		private void BuildMember(MemberInfo mi)
		{
			TypeBuilderHelper nestedType = _typeBuilder.DefineNestedType(
				"Accessor$" + mi.Name, TypeAttributes.NestedPrivate, typeof(MemberAccessor));

			ConstructorBuilderHelper ctorBuilder = BuildNestedTypeConstructor(nestedType);

			BuildInitMember (mi, ctorBuilder);
			BuildGetter     (mi, nestedType);
			BuildSetter     (mi, nestedType);

			Type type = mi is FieldInfo ? ((FieldInfo)mi).FieldType : ((PropertyInfo)mi).PropertyType;

			if (type.IsEnum)
				type = Enum.GetUnderlyingType(type);

			string typedPropertyName = type.Name;

#if FW2
			BuildGetterT     (mi, nestedType);
			BuildSetterT     (mi, nestedType);

			if (type.IsGenericType)
			{
				Type underlyingType = Nullable.GetUnderlyingType(type);

				if (underlyingType != null)
				{
					if (underlyingType.IsEnum)
						underlyingType = Enum.GetUnderlyingType(underlyingType);

					typedPropertyName = "Nullable" + underlyingType.Name;
				}
				else
				{
					typedPropertyName = null;
				}
			}
#endif

			if (typedPropertyName != null)
			{
				BuildTypedGetter(mi, nestedType, type, typedPropertyName);
				BuildTypedSetter(mi, nestedType, type, typedPropertyName);
			}

			// FW 1.1 wants nested types to be created before parent.
			//
			_nestedTypes.Add(nestedType);
		}

		private void BuildInitMember(MemberInfo mi, ConstructorBuilderHelper ctorBuilder)
		{
			_typeBuilder.DefaultConstructor.Emitter
				.ldarg_0
				.ldarg_0
				.ldarg_0
				.ldc_i4  (mi is FieldInfo? 1: 2)
				.ldstr   (mi.Name)
				.call    (_accessorType.GetMethod("GetMember", typeof(int), typeof(string)))
				.newobj  (ctorBuilder)
				.call    (_accessorType.GetMethod("AddMember", typeof(MemberAccessor)))
				;
		}

		private void BuildGetter(MemberInfo mi, TypeBuilderHelper nestedType)
		{
			MethodInfo getMethod = null;

			if (mi is PropertyInfo)
			{
				getMethod = ((PropertyInfo)mi).GetGetMethod();

				if (getMethod == null)
				{
					if (_type != _originalType)
						getMethod = _type.GetMethod("get_" + mi.Name);

					if (getMethod == null)
						return;
				}
			}

			MethodBuilderHelper method = nestedType.DefineMethod(
				_memberAccessor.GetMethod("GetValue", typeof(object)));
			
			EmitHelper emit = method.Emitter;

			emit
				.ldarg_1
				.castclass (_type)
				.end();

			if (mi is FieldInfo)
			{
				FieldInfo fi = (FieldInfo)mi;

				emit
					.ldfld          (fi)
					.boxIfValueType (fi.FieldType)
					;
			}
			else
			{
				PropertyInfo pi = (PropertyInfo)mi;

				emit
					.callvirt       (getMethod)
					.boxIfValueType (pi.PropertyType)
					;
			}

			emit
				.ret()
				;

			nestedType.DefineMethod(_memberAccessor.GetProperty("HasGetter").GetGetMethod()).Emitter
				.ldc_i4_1
				.ret()
				;
		}

		private void BuildSetter(MemberInfo mi, TypeBuilderHelper nestedType)
		{
			MethodInfo setMethod = null;

			if (mi is PropertyInfo)
			{
				setMethod = ((PropertyInfo)mi).GetSetMethod();

				if (setMethod == null)
				{
					if (_type != _originalType)
						setMethod = _type.GetMethod("set_" + mi.Name);

					if (setMethod == null)
						return;
				}
			}

			MethodBuilderHelper method = nestedType.DefineMethod(
				_memberAccessor.GetMethod("SetValue", typeof(object), typeof(object)));
			
			EmitHelper emit = method.Emitter;

			emit
				.ldarg_1
				.castclass (_type)
				.ldarg_2
				.end();

			if (mi is FieldInfo)
			{
				FieldInfo fi = (FieldInfo)mi;

				emit
					.CastFromObject (fi.FieldType)
					.stfld          (fi)
					;
			}
			else
			{
				PropertyInfo pi = (PropertyInfo)mi;

				emit
					.CastFromObject (pi.PropertyType)
					.callvirt       (setMethod)
					;
			}

			emit
				.ret()
				;

			nestedType.DefineMethod(_memberAccessor.GetProperty("HasSetter").GetGetMethod()).Emitter
				.ldc_i4_1
				.ret()
				;
		}

		private void BuildTypedGetter(
			MemberInfo        mi,
			TypeBuilderHelper nestedType,
			Type              memberType,
			string            typedPropertyName)
		{
			MethodInfo getMethod = null;

			if (mi is PropertyInfo)
			{
				getMethod = ((PropertyInfo)mi).GetGetMethod();

				if (getMethod == null)
				{
					if (_type != _originalType)
						getMethod = _type.GetMethod("get_" + mi.Name);

					if (getMethod == null)
						return;
				}
			}

			MethodInfo methodInfo = _memberAccessor.GetMethod("Get" + typedPropertyName, memberType);

			if (methodInfo == null)
				return;

			MethodBuilderHelper method = nestedType.DefineMethod(methodInfo);
			
			EmitHelper emit = method.Emitter;

			emit
				.ldarg_1
				.castclass (_type)
				.end();

			if (mi is FieldInfo) emit.ldfld   ((FieldInfo)mi);
			else                 emit.callvirt(getMethod);

			emit
				.ret()
				;
		}

		private void BuildTypedSetter(
			MemberInfo        mi,
			TypeBuilderHelper nestedType,
			Type              memberType,
			string            typedPropertyName)
		{
			MethodInfo setMethod = null;

			if (mi is PropertyInfo)
			{
				setMethod = ((PropertyInfo)mi).GetSetMethod();

				if (setMethod == null)
				{
					if (_type != _originalType)
						setMethod = _type.GetMethod("set_" + mi.Name);

					if (setMethod == null)
						return;
				}
			}

			MethodInfo methodInfo =
				_memberAccessor.GetMethod("Set" + typedPropertyName, typeof(object), memberType);

			if (methodInfo == null)
				return;

			MethodBuilderHelper method = nestedType.DefineMethod(methodInfo);
			
			EmitHelper emit = method.Emitter;

			emit
				.ldarg_1
				.castclass (_type)
				.ldarg_2
				.end();

			if (mi is FieldInfo) emit.stfld   ((FieldInfo)mi);
			else                 emit.callvirt(setMethod);

			emit
				.ret()
				;
		}

		private void BuildGetterT(MemberInfo mi, TypeBuilderHelper nestedType)
		{
			MethodBuilderHelper method = nestedType.DefineMethod(
				_memberAccessor.GetMethod("GetValueT"));

			EmitHelper emit = method.Emitter;

			emit
				.ldarg_2
				.ldarg_1
				.ldind_ref
				.end();

			if (mi is FieldInfo)
			{
				FieldInfo fi = (FieldInfo)mi;
				emit
					
					.ldfld(fi)
					.stobj(fi.FieldType)
					.ret()
					;

//				emit
//					.ldfld(fi)
//					.stind(fi.FieldType)
//					.ret()
//					;
			}
			else
			{
				PropertyInfo pi = (PropertyInfo)mi;
				MethodInfo getMethod = (pi).GetGetMethod();

				if (getMethod == null)
				{
					if (_type != _originalType)
						getMethod = _type.GetMethod("get_" + mi.Name);

					if (getMethod == null)
						return;
				}

				emit
					.callvirt(getMethod)
					.stobj(pi.PropertyType)
					.ret()
					;

//				emit
//					.callvirt(getMethod)
//					.stind(pi.PropertyType)
//					.ret()
//					;
			}
		}

		private void BuildSetterT(MemberInfo mi, TypeBuilderHelper nestedType)
		{
			MethodInfo setMethod = null;

			if (mi is PropertyInfo)
			{
				setMethod = ((PropertyInfo)mi).GetSetMethod();

				if (setMethod == null)
				{
					if (_type != _originalType)
						setMethod = _type.GetMethod("set_" + mi.Name);

					if (setMethod == null)
						return;
				}
			}

			MethodBuilderHelper method = nestedType.DefineMethod(
				_memberAccessor.GetMethod("SetValueT"));

			EmitHelper emit = method.Emitter;

			emit
				.ldarg_1
				.ldarg_2
				.end();

			if (mi is FieldInfo) emit.stfld((FieldInfo)mi);
			else                 emit.callvirt(setMethod);

			emit
				.ret()
				;
		}

		private static ConstructorBuilderHelper BuildNestedTypeConstructor(TypeBuilderHelper nestedType)
		{
			Type[] parameters = { typeof(TypeAccessor), typeof(MemberInfo) };

			ConstructorBuilderHelper ctorBuilder = nestedType.DefinePublicConstructor(parameters);

			ctorBuilder.Emitter
				.ldarg_0
				.ldarg_1
				.ldarg_2
				.call    (TypeHelper.GetConstructor(typeof(MemberAccessor), parameters))
				.ret()
				;

			return ctorBuilder;
		}

		private void BuildObjectFactory()
		{
			Attribute attr = TypeHelper.GetFirstAttribute(_type, typeof(ObjectFactoryAttribute));

			if (attr != null)
			{
				_typeBuilder.DefaultConstructor.Emitter
					.ldarg_0
					.LoadType  (_type)
					.LoadType  (typeof(ObjectFactoryAttribute))
					.call      (typeof(TypeHelper).            GetMethod("GetFirstAttribute"))
					.castclass (typeof(ObjectFactoryAttribute))
					.call      (typeof(ObjectFactoryAttribute).GetProperty("ObjectFactory").GetGetMethod())
					.call      (typeof(TypeAccessor).          GetProperty("ObjectFactory").GetSetMethod())
					;
			}
		}
	}
}
