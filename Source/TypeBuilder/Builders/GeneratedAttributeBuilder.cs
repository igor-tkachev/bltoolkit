using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using BLToolkit.Reflection;

namespace BLToolkit.TypeBuilder.Builders
{
	internal class GeneratedAttributeBuilder : AbstractTypeBuilderBase
	{
		private CustomAttributeBuilder _attributeBuilder;

		public GeneratedAttributeBuilder(Type attributeType, object[] arguments, string[] names, object[] values)
		{
			if (attributeType == null)
				throw new ArgumentNullException("attributeType");

			ConstructorInfo constructor = null;

			if (arguments == null || arguments.Length == 0)
			{
				constructor = attributeType.GetConstructor(Type.EmptyTypes);
				arguments   = Type.EmptyTypes;
			}
			else
			{
				// Some arguments may be null. We can not infer a type from the null reference.
				// So we must iterate all of them and got a suitable one.
				//
				foreach (ConstructorInfo ci in attributeType.GetConstructors())
				{
					if (CheckParameters(ci.GetParameters(), arguments))
					{
						constructor = ci;
						break;
					}
				}
			}

			if (constructor == null)
				throw new TypeBuilderException(string.Format("No suitable constructors found for the type '{0}'.", attributeType.FullName));

			if (names == null || names.Length == 0)
			{
				_attributeBuilder = new CustomAttributeBuilder(constructor, arguments);
			}
			else if (values == null || names.Length != values.Length)
			{
				throw new TypeBuilderException(string.Format("Named argument names count should match named argument values count."));
			}
			else
			{
				List<PropertyInfo> namedProperties = new List<PropertyInfo>();
				List<object>       propertyValues  = new List<object>();
				List<FieldInfo>    namedFields     = new List<FieldInfo>();
				List<object>       fieldValues     = new List<object>();

				for (int i = 0; i < names.Length; i++)
				{
					string name = names[i];
					MemberInfo[] mi = attributeType.GetMember(name);

					if (mi.Length == 0)
						throw new TypeBuilderException(string.Format("The type '{0}' does not have a public member '{1}'.", attributeType.FullName, name));

					if (mi[0].MemberType == MemberTypes.Property)
					{
						namedProperties.Add((PropertyInfo)mi[0]);
						propertyValues.Add(values[i]);
					}
					else if (mi[0].MemberType == MemberTypes.Field)
					{
						namedFields.Add((FieldInfo)mi[0]);
						fieldValues.Add(values[i]);
					}
					else
						throw new TypeBuilderException(string.Format("The member '{1}' of the type '{0}' is not a filed nor a property.", name, attributeType.FullName));
				}

				_attributeBuilder = new CustomAttributeBuilder(constructor, arguments,
					namedProperties.ToArray(), propertyValues.ToArray(), namedFields.ToArray(), fieldValues.ToArray());
			}
		}

		private static bool CheckParameters(ParameterInfo[] argumentTypes, object[] arguments)
		{
			if (argumentTypes.Length != arguments.Length)
				return false;

			for (int i = 0; i < arguments.Length; i++)
			{
				if (arguments[i] == null && argumentTypes[i].ParameterType.IsClass)
					continue;

				if (argumentTypes[i].ParameterType.IsAssignableFrom(arguments[i].GetType()))
					continue;

				// Bad match
				//
				return false;
			}

			return true;
		}

		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			return context.IsAfterStep && context.BuildElement == BuildElement.Type == TargetElement is TypeHelper;
		}

		public override void Build(BuildContext context)
		{
			if (context.BuildElement == BuildElement.Type)
			{
				context.TypeBuilder.TypeBuilder.SetCustomAttribute(_attributeBuilder);
			}
			else if (TargetElement is MethodInfo)
			{
				context.MethodBuilder.MethodBuilder.SetCustomAttribute(_attributeBuilder);
			}
			else if (TargetElement is PropertyInfo && context.IsAbstractProperty)
			{
				if (_attributeBuilder != null)
				{
					FieldBuilder field = (FieldBuilder)context.Fields[TargetElement];

					field.SetCustomAttribute(_attributeBuilder);

					// Suppress multiple instances when the property has both getter and setter.
					//
					_attributeBuilder = null;
				}
			}
		}
	}
}
