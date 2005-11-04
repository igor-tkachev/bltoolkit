using System;
using System.Collections;
using System.Reflection;

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
		TypeHelper        _accessor   = new TypeHelper(typeof(TypeAccessor));
		ArrayList         _nestedTypes = new ArrayList();
		TypeBuilderHelper _typeBuilder;

		public string AssemblyNameSuffix
		{
			get { return "TypeAccessor"; }
		}

		public Type Build(Type sourceType, AssemblyBuilderHelper assemblyBuilder)
		{
			string typeName = _type.FullName.Replace('+', '.') + ".TypeAccessor";
			
			_typeBuilder = assemblyBuilder.DefineType(typeName, _accessor);

			_typeBuilder.DefaultConstructor.Emitter
				.ldarg_0
				.call(_accessor.GetDefaultConstructor())
				;

			BuildCreateInstanceMethods();
			BuildTypeProperties();
			BuildMembers();

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
			{
				throw new TypeBuilderException(
					string.Format("The '{0}' type must have public default or init constructor.", _type.Name));
			}

			// CreateInstance.
			//
			MethodBuilderHelper method = _typeBuilder.DefineMethod(
				TypeHelper.GetMethodNoGeneric(_accessor, "CreateInstance", Type.EmptyTypes));

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
				TypeHelper.GetMethodNoGeneric(_accessor, "CreateInstance", typeof(InitContext)));

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
				_typeBuilder.DefineMethod(_accessor.GetProperty("Type").GetGetMethod());

			method.Emitter
				.LoadType(_type)
				.ret()
				;

			// OriginalType.
			//
			method = 
				_typeBuilder.DefineMethod(_accessor.GetProperty("OriginalType").GetGetMethod());

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

			BuildNestedTypeConstructor(nestedType, mi);

			_nestedTypes.Add(nestedType);
		}

		private void BuildNestedTypeConstructor(TypeBuilderHelper nestedType, MemberInfo mi)
		{
			ConstructorBuilderHelper ctorBuilder = nestedType.DefinePublicConstructor(typeof(MemberInfo));

			ctorBuilder.Emitter
				.ldarg_0
				.ldarg_1
				.call    (TypeHelper.GetConstructor(typeof(MemberAccessor), typeof(MemberInfo)))
				.ret()
				;
		}
	}
}
