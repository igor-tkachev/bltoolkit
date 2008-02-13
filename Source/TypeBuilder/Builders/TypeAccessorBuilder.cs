using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
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

		readonly TypeHelper        _type;
		readonly TypeHelper        _originalType;
		readonly TypeHelper        _accessorType   = new TypeHelper(typeof(TypeAccessor));
		readonly TypeHelper        _memberAccessor = new TypeHelper(typeof(MemberAccessor));
		readonly ArrayList         _nestedTypes    = new ArrayList();
		         TypeBuilderHelper _typeBuilder;
#if FW2
		         bool              _friendlyAssembly;
#endif

		public string AssemblyNameSuffix
		{
			get { return "TypeAccessor"; }
		}

		public static string GetTypeAccessorClassName(Type originalType)
		{
			return originalType.FullName.Replace('+', '.')
#if !FW2
				.Replace('[', '_').Replace(']', '_')
#endif
				+ ".TypeAccessor";
		}

		public Type Build(Type sourceType, AssemblyBuilderHelper assemblyBuilder)
		{
			if (sourceType == null)      throw new ArgumentNullException("sourceType");
			if (assemblyBuilder == null) throw new ArgumentNullException("assemblyBuilder");

#if FW2
			// Check InternalsVisibleToAttributes of the source type's assembly.
			// Even if the sourceType is public, it may have internal fields and props.
			//
			_friendlyAssembly = false;

			// Usually, there is no such attribute in the source assembly.
			// Therefore we do not cache the result.
			//
			object[] attributes = sourceType.Assembly.GetCustomAttributes(typeof(InternalsVisibleToAttribute), true);
			foreach (InternalsVisibleToAttribute visibleToAttribute in attributes)
			{
				if (AssemblyName.ReferenceMatchesDefinition(assemblyBuilder.AssemblyName,
					new AssemblyName(visibleToAttribute.AssemblyName)))
				{
					_friendlyAssembly = true;
					break;
				}
			}

			if (!sourceType.IsVisible && !_friendlyAssembly)
				throw new TypeBuilderException(string.Format("Can not build type accessor for non-public type '{0}'.", sourceType.FullName));
#else
			if (!sourceType.IsPublic && !sourceType.IsNestedPublic)
				throw new TypeBuilderException(string.Format("Can not build type accessor for non-public type '{0}'.", sourceType.FullName));
#endif

			string typeName = GetTypeAccessorClassName(_type);

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
			bool            isValueType  = _type.IsValueType;
			ConstructorInfo baseDefCtor  = isValueType? null: _type.GetPublicDefaultConstructor();
			ConstructorInfo baseInitCtor = _type.GetPublicConstructor(typeof(InitContext));

			if (baseDefCtor == null && baseInitCtor == null && !isValueType)
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
			else if (isValueType)
			{
				LocalBuilder    locObj = method.Emitter.DeclareLocal(_type);
				
				method.Emitter
					.ldloca  (locObj)
					.initobj (_type)
					.ldloc   (locObj)
					.box     (_type)
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
			else if (isValueType)
			{
				LocalBuilder    locObj = method.Emitter.DeclareLocal(_type);
				
				method.Emitter
					.ldloca  (locObj)
					.initobj (_type)
					.ldloc   (locObj)
					.box     (_type)
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
			ListDictionary members = new ListDictionary();

			foreach (FieldInfo fi in _originalType.GetFields(BindingFlags.Instance | BindingFlags.Public))
				AddMemberToDictionary(members, fi);

#if FW2
			if (_friendlyAssembly)
			{
				foreach (FieldInfo fi in _originalType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
					if (fi.IsAssembly || fi.IsFamilyOrAssembly)
						AddMemberToDictionary(members, fi);
			}
#endif

			foreach (PropertyInfo pi in _originalType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
				if (pi.GetIndexParameters().Length == 0)
					AddMemberToDictionary(members, pi);

			foreach (PropertyInfo pi in _originalType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
			{
				if (pi.GetIndexParameters().Length == 0)
				{
					MethodInfo getter = pi.GetGetMethod(true);
					MethodInfo setter = pi.GetSetMethod(true);

					if (getter != null && getter.IsAbstract || setter != null && setter.IsAbstract)
						AddMemberToDictionary(members, pi);
				}
			}

			foreach (MemberInfo mi in members.Values)
				BuildMember(mi);
		}

		private static void AddMemberToDictionary(IDictionary members, MemberInfo mi)
		{
			string name = mi.Name;

			if (!members.Contains(name))
			{
				members.Add(name, mi);
				return;
			}

			MemberInfo existing = (MemberInfo) members[name];
			if (mi.DeclaringType.IsSubclassOf(existing.DeclaringType))
			{
				// mi is a member of the most descendant type.
				//
				members[name] = mi;
			}
		}

		private void BuildMember(MemberInfo mi)
		{
			TypeBuilderHelper nestedType = _typeBuilder.DefineNestedType(
				"Accessor$" + mi.Name, TypeAttributes.NestedPrivate, typeof(MemberAccessor));

			ConstructorBuilderHelper ctorBuilder = BuildNestedTypeConstructor(nestedType);

			BuildGetter    (mi, nestedType);
			BuildSetter    (mi, nestedType);
			BuildInitMember(mi, ctorBuilder);

			Type type = mi is FieldInfo ? ((FieldInfo)mi).FieldType : ((PropertyInfo)mi).PropertyType;

			if (type.IsEnum)
				type = Enum.GetUnderlyingType(type);

			string typedPropertyName = type.Name;

#if FW2
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

			BuildCloneValueMethod(mi, nestedType, type);

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

		/// <summary>
		/// Figure out is base type method is accessible by extension type method.
		/// </summary>
		/// <param name="method">A <see cref="MethodInfo"/> instance.</param>
		/// <returns>True if the method access is Public or Family and it's assembly is friendly.</returns>
		private bool IsMethodAccessible(MethodInfo method)
		{
			if (method == null) throw new ArgumentNullException("method");

			return method.IsPublic
#if FW2
				|| (_friendlyAssembly && (method.IsAssembly || method.IsFamilyOrAssembly))
#endif
				;
		}

		private void BuildGetter(MemberInfo mi, TypeBuilderHelper nestedType)
		{
			Type       methodType = mi.DeclaringType;
			MethodInfo getMethod  = null;

			if (mi is PropertyInfo)
			{
				getMethod = ((PropertyInfo)mi).GetGetMethod();

				if (getMethod == null)
				{
					if (_type != _originalType)
					{
						getMethod  = _type.GetMethod("get_" + mi.Name);
						methodType = _type;
					}

					if (getMethod == null || !IsMethodAccessible(getMethod))
						return;
				}
			}

			MethodBuilderHelper method = nestedType.DefineMethod(
				_memberAccessor.GetMethod("GetValue", typeof(object)));
			
			EmitHelper emit = method.Emitter;

			emit
				.ldarg_1
				.castType (methodType)
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
			Type       methodType = mi.DeclaringType;
			MethodInfo setMethod  = null;

			if (mi is PropertyInfo)
			{
				setMethod = ((PropertyInfo)mi).GetSetMethod();

				if (setMethod == null)
				{
					if (_type != _originalType)
					{
						setMethod  = _type.GetMethod("set_" + mi.Name);
						methodType = _type;
					}

					if (setMethod == null || !IsMethodAccessible(setMethod))
						return;
				}
			}
			//else if (((FieldInfo)mi).IsLiteral)
			//	return;

			MethodBuilderHelper method = nestedType.DefineMethod(
				_memberAccessor.GetMethod("SetValue", typeof(object), typeof(object)));
			
			EmitHelper emit = method.Emitter;

			emit
				.ldarg_1
				.castType (methodType)
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
			Type       methodType = mi.DeclaringType;
			MethodInfo getMethod  = null;

			if (mi is PropertyInfo)
			{
				getMethod = ((PropertyInfo)mi).GetGetMethod();

				if (getMethod == null)
				{
					if (_type != _originalType)
					{
						getMethod  = _type.GetMethod("get_" + mi.Name);
						methodType = _type;
					}

					if (getMethod == null || !IsMethodAccessible(getMethod))
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
				.castType (methodType)
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
			Type       methodType = mi.DeclaringType;
			MethodInfo setMethod  = null;

			if (mi is PropertyInfo)
			{
				setMethod = ((PropertyInfo)mi).GetSetMethod();

				if (setMethod == null)
				{
					if (_type != _originalType)
					{
						setMethod  = _type.GetMethod("set_" + mi.Name);
						methodType = _type;
					}

					if (setMethod == null || !IsMethodAccessible(setMethod))
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
				.castType (methodType)
				.ldarg_2
				.end();

			if (mi is FieldInfo) emit.stfld   ((FieldInfo)mi);
			else                 emit.callvirt(setMethod);

			emit
				.ret()
				;
		}

		private void BuildCloneValueMethod(
			MemberInfo        mi,
			TypeBuilderHelper nestedType,
			Type              memberType
			)
		{
			Type       methodType = mi.DeclaringType;
			MethodInfo getMethod  = null;
			MethodInfo setMethod  = null;

			if (mi is PropertyInfo)
			{
				getMethod = ((PropertyInfo)mi).GetGetMethod();

				if (getMethod == null)
				{
					if (_type != _originalType)
					{
						getMethod  = _type.GetMethod("get_" + mi.Name);
						methodType = _type;
					}

					if (getMethod == null || !IsMethodAccessible(getMethod))
						return;
				}

				setMethod = ((PropertyInfo)mi).GetSetMethod();

				if (setMethod == null)
				{
					if (_type != _originalType)
					{
						setMethod  = _type.GetMethod("set_" + mi.Name);
						methodType = _type;
					}

					if (setMethod == null || !IsMethodAccessible(setMethod))
						return;
				}
			}

			MethodBuilderHelper method = nestedType.DefineMethod(
				_memberAccessor.GetMethod("CloneValue", typeof(object), typeof(object)));

			EmitHelper emit = method.Emitter;

			emit
				.ldarg_2
				.castType (methodType)
				.ldarg_1
				.castType (methodType)
				.end();

			if (mi is FieldInfo)
				emit.ldfld   ((FieldInfo)mi);
			else
				emit.callvirt(getMethod);

			if (typeof(string) != memberType && TypeHelper.IsSameOrParent(typeof(ICloneable), memberType))
			{
				if (memberType.IsValueType)
					emit
						.box       (memberType)
						.callvirt  (typeof(ICloneable), "Clone")
						.unbox_any (memberType)
						;
				else
				{
					Label valueIsNull = emit.DefineLabel();

					emit
						.dup
						.brfalse_s (valueIsNull)
						.callvirt  (typeof(ICloneable), "Clone")
						.castclass (memberType)
						.MarkLabel (valueIsNull)
						;
				}
			}

			if (mi is FieldInfo)
				emit.stfld   ((FieldInfo)mi);
			else
				emit.callvirt(setMethod);

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
					.call      (typeof(TypeHelper), "GetFirstAttribute", typeof(Type), typeof(Type))
					.castclass (typeof(ObjectFactoryAttribute))
					.call      (typeof(ObjectFactoryAttribute).GetProperty("ObjectFactory").GetGetMethod())
					.call      (typeof(TypeAccessor).          GetProperty("ObjectFactory").GetSetMethod())
					;
			}
		}
	}
}
