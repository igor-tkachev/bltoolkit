using System;
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
		TypeBuilderHelper _typeBuilder;

		public string AssemblyNameSuffix
		{
			get { return "TypeAccessor"; }
		}

		public Type Build(Type sourceType, AssemblyBuilderHelper assemblyBuilder)
		{
			string typeName = _type.FullName.Replace('+', '.') + ".TypeAccessor";
			
			_typeBuilder = assemblyBuilder.DefineType(typeName, typeof(TypeAccessor));

			BuildCreateInstanceMethods();
			BuildTypeProperties();

			return _typeBuilder.Create();
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
				TypeHelper.GetMethodNoGeneric(typeof(TypeAccessor), "CreateInstance", Type.EmptyTypes));

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
				TypeHelper.GetMethodNoGeneric(typeof(TypeAccessor), "CreateInstance", typeof(InitContext)));

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
				_typeBuilder.DefineMethod(typeof(TypeAccessor).GetProperty("Type").GetGetMethod());

			method.Emitter
				.LoadType(_type)
				.ret()
				;

			// OriginalType.
			//
			method = 
				_typeBuilder.DefineMethod(typeof(TypeAccessor).GetProperty("OriginalType").GetGetMethod());

			method.Emitter
				.LoadType(_originalType)
				.ret()
				;
		}
	}
}
