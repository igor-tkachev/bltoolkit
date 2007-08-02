using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection.Emit;
using BLToolkit.Reflection;

namespace BLToolkit.TypeBuilder.Builders
{
	class ImplementInterfaceBuilder : AbstractTypeBuilderBase
	{
		public ImplementInterfaceBuilder(Type type)
		{
			_type = type;
		}

		private readonly Type _type;

		public override Type[] GetInterfaces()
		{
			return new Type[] { _type };
		}

		public override bool IsApplied(BuildContext context, AbstractTypeBuilderList builders)
		{
			if (context == null) throw new ArgumentNullException("context");

			return context.BuildElement == BuildElement.InterfaceMethod;
		}

		protected override void BuildInterfaceMethod()
		{
			bool returnIfNonZero = false;
			bool returnIfZero    = false;
			
			if (Context.ReturnValue != null)
			{
				object[] attrs = 
					Context.MethodBuilder.OverriddenMethod.ReturnTypeCustomAttributes.GetCustomAttributes(true);

				foreach (object o in attrs)
				{
					if      (o is ReturnIfNonZeroAttribute) returnIfNonZero = true;
					else if (o is ReturnIfZeroAttribute)    returnIfZero    = true;
				}
			}

			Type       interfaceType = Context.CurrentInterface;
			EmitHelper emit          = Context.MethodBuilder.Emitter;

			foreach (DictionaryEntry de in Context.Fields)
			{
				PropertyInfo property = (PropertyInfo)de.Key;
				FieldBuilder field    = (FieldBuilder)de.Value;

				if (field.FieldType.IsPrimitive || field.FieldType == typeof(string))
					continue;

				Type[] types = field.FieldType.GetInterfaces();

				foreach (Type type in types)
				{
					if (type != interfaceType)
						continue;

					InterfaceMapping im = field.FieldType.GetInterfaceMap(type);

					for (int j = 0; j < im.InterfaceMethods.Length; j++)
					{
						if (im.InterfaceMethods[j] == Context.MethodBuilder.OverriddenMethod)
						{
							MethodInfo targetMethod = im.TargetMethods[j];

							Label label     = new Label();
							bool  checkNull = false;

							if (CallLazyInstanceInsurer(field) == false && field.FieldType.IsClass)
							{
								// Check if field is null.
								//
								checkNull = true;

								label = emit.DefineLabel();

								emit
									.ldarg_0
									.ldfld     (field)
									.brfalse_s (label)
									;
							}

							// this.
							//
							emit
								.ldarg_0
								.end();

							// Load the field and prepare it for interface method call if the method is private.
							//
							if (field.FieldType.IsValueType)
							{
								if (targetMethod.IsPublic)
									emit.ldflda (field);
								else
									emit
										.ldfld  (field)
										.box    (field.FieldType);
							}
							else
							{
								if (targetMethod.IsPublic)
									emit.ldfld (field);
								else
									emit
										.ldfld     (field)
										.castclass (interfaceType);
							}

							// Check parameter attributes.
							//
							ParameterInfo[] pi = Context.MethodBuilder.OverriddenMethod.GetParameters();

							for (int k = 0; k < pi.Length; k++)
							{
								object[] attrs = pi[k].GetCustomAttributes(true);
								bool     stop  = false;

								foreach (object a in attrs)
								{
									// Parent - set this.
									//
									if (a is ParentAttribute)
									{
										emit
											.ldarg_0
											.end()
											;

										if (!TypeHelper.IsSameOrParent(pi[k].ParameterType, Context.Type))
											emit
												.castclass (pi[k].ParameterType)
												;

										stop = true;

										break;
									}

									// PropertyInfo.
									//
									if (a is PropertyInfoAttribute)
									{
										FieldBuilder ifb = GetPropertyInfoField(property);

										emit.ldsfld(ifb);
										stop = true;

										break;
									}
								}

								if (stop)
									continue;

								// Pass argument.
								//
								emit.ldarg ((byte)(k + 1));
							}

							// Call the method.
							//
							if (field.FieldType.IsValueType)
							{
								if (targetMethod.IsPublic) emit.call     (targetMethod);
								else                       emit.callvirt (im.InterfaceMethods[j]);
							}
							else
							{
								if (targetMethod.IsPublic) emit.callvirt (targetMethod);
								else                       emit.callvirt (im.InterfaceMethods[j]);
							}

							// Return if appropriated result.
							//
							if (Context.ReturnValue != null)
							{
								emit.stloc(Context.ReturnValue);

								if (returnIfNonZero)
								{
									emit
										.ldloc  (Context.ReturnValue)
										.brtrue (Context.ReturnLabel);
								}
								else if (returnIfZero)
								{
									emit
										.ldloc   (Context.ReturnValue)
										.brfalse (Context.ReturnLabel);
								}
							}

							if (checkNull)
								emit.MarkLabel(label);

							break;
						}
					}

					break;
				}
			}
		}
	}
}
