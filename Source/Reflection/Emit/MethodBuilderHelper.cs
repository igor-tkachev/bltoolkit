using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;

namespace BLToolkit.Reflection.Emit
{
	/// <summary>
	/// A wrapper around the <see cref="MethodBuilder"/> class.
	/// </summary>
	/// <include file="Examples.CS.xml" path='examples/emit[@name="Emit"]/*' />
	/// <include file="Examples.VB.xml" path='examples/emit[@name="Emit"]/*' />
	/// <seealso cref="System.Reflection.Emit.MethodBuilder">MethodBuilder Class</seealso>
	public class MethodBuilderHelper : MethodBuilderBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MethodBuilderHelper"/> class
		/// with the specified parameters.
		/// </summary>
		/// <param name="typeBuilder">Associated <see cref="TypeBuilderHelper"/>.</param>
		/// <param name="methodBuilder">A <see cref="MethodBuilder"/></param>
		public MethodBuilderHelper(TypeBuilderHelper typeBuilder, MethodBuilder methodBuilder)
			: base(typeBuilder)
		{
			if (methodBuilder == null) throw new ArgumentNullException("methodBuilder");

			_methodBuilder = methodBuilder;

			methodBuilder.SetCustomAttribute(Type.Assembly.BLToolkitAttribute);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodBuilderHelper"/> class
		/// with the specified parameters.
		/// </summary>
		/// <param name="typeBuilder">Associated <see cref="TypeBuilderHelper"/>.</param>
		/// <param name="methodBuilder">A <see cref="MethodBuilder"/></param>
		/// <param name="genericArguments">Generic arguments of the method.</param>
		/// <param name="returnType">The return type of the method.</param>
		/// <param name="parameterTypes">The types of the parameters of the method.</param>
		internal MethodBuilderHelper(
			TypeBuilderHelper  typeBuilder,
			MethodBuilder      methodBuilder,
			Type[]             genericArguments,
			Type               returnType,
			Type[]             parameterTypes
			)
			: base(typeBuilder)
		{
			if (methodBuilder    == null) throw new ArgumentNullException("methodBuilder");
			if (genericArguments == null) throw new ArgumentNullException("genericArguments");

			_methodBuilder = methodBuilder;

			string[] genArgNames = Array.ConvertAll<Type, string>(genericArguments, delegate (Type t)
			{
				return t.Name;
			});

			GenericTypeParameterBuilder[] genParams = methodBuilder.DefineGenericParameters(genArgNames);

			// Copy parameter constraints.
			//
			List<Type> interfaceConstraints = null;
			for (int i = 0; i < genParams.Length; i++)
			{
				genParams[i].SetGenericParameterAttributes(genericArguments[i].GenericParameterAttributes);

				foreach (Type constraint in genericArguments[i].GetGenericParameterConstraints())
				{
					if (constraint.IsClass)
						genParams[i].SetBaseTypeConstraint(constraint);
					else
					{
						if (interfaceConstraints == null)
							interfaceConstraints = new List<Type>();
						interfaceConstraints.Add(constraint);
					}
				}

				if (interfaceConstraints != null && interfaceConstraints.Count != 0)
				{
					genParams[i].SetInterfaceConstraints(interfaceConstraints.ToArray());
					interfaceConstraints.Clear();
				}
			}

			// When a method contains a generic parameter we need to replace all
			// generic types from methodInfoDeclaration with local ones.
			//
			for (int i = 0; i < parameterTypes.Length; i++)
				parameterTypes[i] = TypeHelper.TranslateGenericParameters(parameterTypes[i], genParams);

			methodBuilder.SetParameters(parameterTypes);
			methodBuilder.SetReturnType(TypeHelper.TranslateGenericParameters(returnType, genParams));

			// Once all generic stuff is done is it is safe to call SetCustomAttribute
			//
			methodBuilder.SetCustomAttribute(Type.Assembly.BLToolkitAttribute);
		}

		private readonly MethodBuilder _methodBuilder;
		/// <summary>
		/// Gets MethodBuilder.
		/// </summary>
		public  MethodBuilder  MethodBuilder
		{
			get { return _methodBuilder; }
		}

		/// <summary>
		/// Converts the supplied <see cref="MethodBuilderHelper"/> to a <see cref="MethodBuilder"/>.
		/// </summary>
		/// <param name="methodBuilder">The MethodBuilderHelper.</param>
		/// <returns>A MethodBuilder.</returns>
		public static implicit operator MethodBuilder(MethodBuilderHelper methodBuilder)
		{
			if (methodBuilder == null) throw new ArgumentNullException("methodBuilder");

			return methodBuilder.MethodBuilder;
		}

		private EmitHelper _emitter;
		/// <summary>
		/// Gets EmitHelper.
		/// </summary>
		public override EmitHelper Emitter
		{
			get
			{
				if (_emitter == null)
					_emitter = new EmitHelper(this, _methodBuilder.GetILGenerator());

				return _emitter;
			}
		}

		private MethodInfo _overriddenMethod;
		public  MethodInfo  OverriddenMethod
		{
			get { return _overriddenMethod;  }
			set { _overriddenMethod = value; }
		}

		/// <summary>
		/// Returns the type that declares this method.
		/// </summary>
		public Type DeclaringType
		{
			get { return _methodBuilder.DeclaringType; }
		}
	}
}
