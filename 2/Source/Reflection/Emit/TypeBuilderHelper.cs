using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.TypeBuilder;

namespace BLToolkit.Reflection.Emit
{
	/// <summary>
	/// A wrapper around the <see cref="TypeBuilder"/> class.
	/// </summary>
	/// <include file="Examples.CS.xml" path='examples/emit[@name="Emit"]/*' />
	/// <include file="Examples.VB.xml" path='examples/emit[@name="Emit"]/*' />
	/// <seealso cref="System.Reflection.Emit.TypeBuilder">TypeBuilder Class</seealso>
	public class TypeBuilderHelper
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TypeBuilderHelper"/> class
		/// with the specified parameters.
		/// </summary>
		/// <param name="assemblyBuilder">Associated <see cref="AssemblyBuilderHelper"/>.</param>
		/// <param name="typeBuilder">A <see cref="TypeBuilder"/></param>
		public TypeBuilderHelper(AssemblyBuilderHelper assemblyBuilder, System.Reflection.Emit.TypeBuilder typeBuilder)
		{
			if (assemblyBuilder == null) throw new ArgumentNullException("assemblyBuilder");
			if (typeBuilder     == null) throw new ArgumentNullException("typeBuilder");

			_assembly    = assemblyBuilder;
			_typeBuilder = typeBuilder;

			_typeBuilder.SetCustomAttribute(_assembly.BLToolkitAttribute);
		}

		private AssemblyBuilderHelper _assembly;
		/// <summary>
		/// Gets associated AssemblyBuilderHelper.
		/// </summary>
		public  AssemblyBuilderHelper  Assembly
		{
			get { return _assembly; }
		}

		private System.Reflection.Emit.TypeBuilder _typeBuilder;
		/// <summary>
		/// Gets TypeBuilder.
		/// </summary>
		public  System.Reflection.Emit.TypeBuilder  TypeBuilder
		{
			get { return _typeBuilder; }
		}

		/// <summary>
		/// Converts the supplied <see cref="TypeBuilderHelper"/> to a <see cref="TypeBuilder"/>.
		/// </summary>
		/// <param name="typeBuilder">The TypeBuilderHelper.</param>
		/// <returns>A TypeBuilder.</returns>
		public static implicit operator System.Reflection.Emit.TypeBuilder(TypeBuilderHelper typeBuilder)
		{
			return typeBuilder.TypeBuilder;
		}

		#region DefineMethod Overrides

		/// <summary>
		/// Adds a new method to the class, with the given name and method signature.
		/// </summary>
		/// <param name="name">The name of the method. name cannot contain embedded nulls. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="returnType">The return type of the method.</param>
		/// <param name="parameterTypes">The types of the parameters of the method.</param>
		/// <returns>The defined method.</returns>
		public MethodBuilderHelper DefineMethod(
			string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return new MethodBuilderHelper(this, _typeBuilder.DefineMethod(name, attributes, returnType, parameterTypes));
		}

		/// <summary>
		/// Adds a new method to the class, with the given name and method signature.
		/// </summary>
		/// <param name="name">The name of the method. name cannot contain embedded nulls. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <param name="returnType">The return type of the method.</param>
		/// <returns>The defined method.</returns>
		public MethodBuilderHelper DefineMethod(string name, MethodAttributes attributes, Type returnType)
		{
			return new MethodBuilderHelper(this, _typeBuilder.DefineMethod(name, attributes, returnType, Type.EmptyTypes));
		}

		/// <summary>
		/// Adds a new method to the class, with the given name and method signature.
		/// </summary>
		/// <param name="name">The name of the method. name cannot contain embedded nulls. </param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <returns>The defined method.</returns>
		public MethodBuilderHelper DefineMethod(string name, MethodAttributes attributes)
		{
			return new MethodBuilderHelper(this, _typeBuilder.DefineMethod(name, attributes, typeof(void), Type.EmptyTypes));
		}

		/// <summary>
		/// Adds a new method to the class, with the given name and method signature.
		/// </summary>
		/// <param name="name">The name of the method. name cannot contain embedded nulls. </param>
		/// <param name="methodInfoDeclaration">The method whose declaration is to be used.</param>
		/// <param name="attributes">The attributes of the method. </param>
		/// <returns>The defined method.</returns>
		public MethodBuilderHelper DefineMethod(
			string name, MethodInfo methodInfoDeclaration, MethodAttributes attributes)
		{
			ParameterInfo[] pi = methodInfoDeclaration.GetParameters();
			Type[]  parameters = new Type[pi.Length];

			for (int i = 0; i < pi.Length; i++)
				parameters[i] = pi[i].ParameterType;

			MethodBuilderHelper method = DefineMethod(
				name, attributes, methodInfoDeclaration.ReturnType, parameters);

			// Compiler overrides methods only for interfaces. We do the same.
			// If we wanted to override virtual methods, then methods should've had 
			// MethodAttributes.VtableLayoutMask attribute 
			// and the following condition should've been used below:
			// if ((methodInfoDeclaration is FakeMethodInfo) == false)
			//
			if (methodInfoDeclaration.DeclaringType.IsInterface)
				_typeBuilder.DefineMethodOverride(method.MethodBuilder, methodInfoDeclaration);

			method.OverriddenMethod = methodInfoDeclaration;

			for (int i = 0; i < pi.Length; i++)
				method.MethodBuilder.DefineParameter(i + 1, pi[i].Attributes, pi[i].Name);

			return method;
		}

		/// <summary>
		/// Adds a new method to the class, with the given name and method signature.
		/// </summary>
		/// <param name="name">The name of the method. name cannot contain embedded nulls. </param>
		/// <param name="methodInfoDeclaration">The method whose declaration is to be used.</param>
		/// <returns>The defined method.</returns>
		public MethodBuilderHelper DefineMethod(string name, MethodInfo methodInfoDeclaration)
		{
			return DefineMethod(name, methodInfoDeclaration, MethodAttributes.Virtual);
		}

		/// <summary>
		/// Adds a new private method to the class.
		/// </summary>
		/// <param name="methodInfoDeclaration">The method whose declaration is to be used.</param>
		/// <returns>The defined method.</returns>
		public MethodBuilderHelper DefineMethod(MethodInfo methodInfoDeclaration)
		{
			bool isInterface = methodInfoDeclaration.DeclaringType.IsInterface;

			string name = isInterface?
				methodInfoDeclaration.DeclaringType.FullName + "." + methodInfoDeclaration.Name :
				methodInfoDeclaration.Name;

			MethodAttributes attrs = 
				MethodAttributes.Virtual |
				MethodAttributes.HideBySig |
				MethodAttributes.PrivateScope |
				methodInfoDeclaration.Attributes & MethodAttributes.SpecialName;

			if (isInterface)
				attrs |= MethodAttributes.Private;
			else if ((attrs & MethodAttributes.SpecialName) != 0)
				attrs |= MethodAttributes.Public;
			else
				attrs |= methodInfoDeclaration.Attributes & 
					(MethodAttributes.Public | MethodAttributes.Private);

			return DefineMethod(name, methodInfoDeclaration, attrs);
		}

		#endregion

		/// <summary>
		/// Creates a Type object for the class.
		/// </summary>
		/// <returns>Returns the new Type object for this class.</returns>
		public Type Create()
		{
			return TypeBuilder.CreateType();
		}

		/// <summary>
		/// Sets a custom attribute.
		/// </summary>
		/// <param name="attributeType">Attribute type</param>
		public void SetCustomAttribute(Type attributeType)
		{
			ConstructorInfo        ci        = attributeType.GetConstructor(Type.EmptyTypes);
			CustomAttributeBuilder caBuilder = new CustomAttributeBuilder(ci, new object[0]);

			_typeBuilder.SetCustomAttribute(caBuilder);
		}

		private ConstructorBuilderHelper _typeInitializer;
		/// <summary>
		/// Gets the initializer for this type.
		/// </summary>
		public ConstructorBuilderHelper TypeInitializer
		{
			get 
			{
				if (_typeInitializer == null)
					_typeInitializer = new ConstructorBuilderHelper(this, _typeBuilder.DefineTypeInitializer());

				return _typeInitializer;
			}
		}

		public bool IsTypeInitializerDefined
		{
			get { return _typeInitializer != null; }
		}

		private ConstructorBuilderHelper _defaultConstructor;
		/// <summary>
		/// Gets the default constructor for this type.
		/// </summary>
		public ConstructorBuilderHelper DefaultConstructor
		{
			get 
			{
				if (_defaultConstructor == null)
					_defaultConstructor = new ConstructorBuilderHelper(
						this, _typeBuilder.DefineDefaultConstructor(MethodAttributes.Public));

				return _typeInitializer;
			}
		}

		public bool IsDefaultConstructorDefined
		{
			get { return _defaultConstructor != null; }
		}

		private ConstructorBuilderHelper _initConstructor;
		/// <summary>
		/// Gets the init context constructor for this type.
		/// </summary>
		public ConstructorBuilderHelper InitConstructor
		{
			get 
			{
				if (_initConstructor == null)
				{
					ConstructorBuilder builder = _typeBuilder.DefineConstructor(
						MethodAttributes.Public, 
						CallingConventions.Standard,
						new Type[] { typeof(InitContext) });

					_initConstructor = new ConstructorBuilderHelper(this, builder);
				}

				return _initConstructor;
			}
		}

		public bool IsInitConstructorDefined
		{
			get { return _initConstructor != null; }
		}

		/// <summary>
		/// Adds a new field to the class, with the given name, attributes and field type.
		/// </summary>
		/// <param name="fieldName">The name of the field. fieldName cannot contain embedded nulls.</param>
		/// <param name="type">The type of the field.</param>
		/// <param name="attributes">The attributes of the field.</param>
		/// <returns>The defined field.</returns>
		public FieldBuilder DefineField(string fieldName, Type type, FieldAttributes attributes)
		{
			return _typeBuilder.DefineField(fieldName, type, attributes);
		}
	}
}
