using System;
using System.Reflection;
using System.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	internal class GeneratedAttributeBuilder : AbstractTypeBuilderBase
	{
		private CustomAttributeBuilder _attributeBuilder;

		public GeneratedAttributeBuilder(Type attributeType, object[] arguments)
		{
			if (attributeType == null)
				throw new ArgumentNullException("attributeType");

			if (arguments != null && arguments.Length > 0)
			{
				// Some arguments may be null. We can not infer a type from the null reference.
				// So we must iterate all of them and got a suitable one.
				//
				foreach (ConstructorInfo constructor in attributeType.GetConstructors())
				{
					if (CheckParameters(constructor.GetParameters(), arguments))
					{
						_attributeBuilder = new CustomAttributeBuilder(constructor, arguments);
						break;
					}
				}
			}
			else
			{
				ConstructorInfo constructor = attributeType.GetConstructor(Type.EmptyTypes);
				if (constructor != null)
					_attributeBuilder = new CustomAttributeBuilder(constructor, Type.EmptyTypes);
			}

			if (_attributeBuilder == null)
				throw new TypeBuilderException(string.Format("No suitable constructors found for type '{0}'.", attributeType.FullName));
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
			return context.IsAfterStep;
		}

		public override void Build(BuildContext context)
		{
			if (TargetElement is Type)
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
