/*
 * File:    MapEmit.cs
 * Created: 07/17/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;

namespace Rsdn.Framework.Data.Mapping
{
	internal class MapEmit
	{
		private MapEmit()
		{
		}

		private static bool IsMemberOfType(Type memberType, Type type)
		{
			return 
				type == memberType || 
				memberType.IsEnum && Enum.GetUnderlyingType(memberType) == type;
		}

		private static bool IsBasedOnType(Type memberType, Type type)
		{
			for (Type baseType = memberType.BaseType; baseType != null /*&& baseType != typeof(object)*/; baseType = baseType.BaseType)
				if (baseType == type)
					return true;

			return false;
		}

		private static MemberInfo GetValueMember(Type type, Type memberType)
		{
			FieldInfo[] fields = type.GetFields(
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.SetField);

			foreach (FieldInfo field in fields)
				if (field.Name == "Value" && IsMemberOfType(memberType, field.FieldType))
					return field;

			foreach (FieldInfo field in fields)
				if (field.Name == "Value" && IsBasedOnType(memberType, field.FieldType))
					return field;

			PropertyInfo[] props = type.GetProperties(
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty);

			foreach (PropertyInfo prop in props)
				if (prop.Name == "Value" && IsMemberOfType(memberType, prop.PropertyType))
					return prop;

			foreach (PropertyInfo prop in props)
				if (prop.Name == "Value" && IsBasedOnType(memberType, prop.PropertyType))
					return prop;

			foreach (FieldInfo field in fields)
				if (IsMemberOfType(memberType, field.FieldType))
					return field;

			foreach (PropertyInfo prop in props)
				if (IsMemberOfType(memberType, prop.PropertyType))
					return prop;

			return null;
		}

		private static ConstructorInfo GetDefaultConstructor(Type type)
		{
			return type.GetConstructor(
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null,
				Type.EmptyTypes,
				null);
		}

		class CreatedObject
		{
			public CreatedObject(GenContext ctx)
			{
				_isPrimitive  = MapEmit.IsPrimitive(ctx.FieldType);
				_fieldBuilder = ctx.FieldBuilder;
				_propertyInfo = ctx.PropertyInfo;
			}

			private bool _isPrimitive;
			public  bool  IsPrimitive { get { return _isPrimitive; } }

			private FieldBuilder _fieldBuilder;
			public  FieldBuilder  FieldBuilder { get { return _fieldBuilder; } }

			private PropertyInfo _propertyInfo;
			public  PropertyInfo  PropertyInfo { get { return _propertyInfo; } }

			private FieldBuilder _initFieldBuilder;
			public  FieldBuilder  InitFieldBuilder
			{ 
				get { return _initFieldBuilder;  } 
				set { _initFieldBuilder = value; } 
			}
		}

		class GenContext
		{
			// Type context.
			//
			public Type            Type;        // type
			public TypeBuilder     TypeBuilder; // type builder
			public MapGenerator    IniCtorGen;  // static constructor generator
			public MapGenerator    DefCtorGen;  // default constructor generator
			public MapGenerator    ObfCtorGen;  // object factory constructor generator
			public MapGenerator    InitGen;     // init property method generator
			public PropertyBuilder Descriptor;  // descriptor
			public ArrayList       Actions;     // action attributes.

			public ArrayList       Objects;     // created object's list

			// Property context.
			//
			public PropertyInfo    PropertyInfo;     // property info
			public object []       AttributeParams;  // property attribute parameters
			public FieldBuilder    FieldBuilder;     // property implementation field builder
			public Type            FieldType;        // field type
			public bool            IsObject;         // true - field is a reference type
			public FieldBuilder    ParamBuilder;     // parameter builder
			public MethodInfo      GetMethodInfo;    // getter info
			public MethodInfo      SetMethodInfo;    // setter info

			public LocalBuilder DefInitDataField; // Default ctor MapInitializingData local variable
			public LocalBuilder ObfInitDataField; // Object factory ctor MapInitializingData local variable
		}

		private static Type[]    _factoryParams             = new Type[] { typeof(MapInitializingData) };
		private static Hashtable _defaultConstructorBuilder = new Hashtable();
		private static Hashtable _factoryConstructorBuilder = new Hashtable();

		private static Type CreateNonAbstractType(Type type, ModuleBuilder moduleBuilder)
		{
			GenContext ctx = new GenContext();

			ctx.Type    = type;
			ctx.Objects = new ArrayList();
			ctx.Actions = MapDescriptor.GetAllAttributes(type, typeof(MapActionAttribute));

			Type[] interfaces = new Type[ctx.Actions.Count + 1];

			for (int i = 0; i < ctx.Actions.Count; i++)
			{
				interfaces[i] = ((MapActionAttribute)ctx.Actions[i]).Type;

				if (interfaces[i].IsInterface == false)
					throw new RsdnMapException(string.Format(
						"MapAction: {0} type must be an interface.", interfaces[i].Name));
			}

			interfaces[ctx.Actions.Count] = typeof(IMapGenerated);

			ctx.TypeBuilder = moduleBuilder.DefineType(
				type.FullName.Replace('+', '.') + ".MappingExtension." + type.Name,
				TypeAttributes.Public | TypeAttributes.BeforeFieldInit,
				type,
				interfaces);

			// Set Serializable attribute.
			//
			if (type.IsSerializable)
			{
				ConstructorInfo        ci        = typeof(SerializableAttribute).GetConstructor(Type.EmptyTypes);
				CustomAttributeBuilder caBuilder = new CustomAttributeBuilder(ci, new object[0]);

				ctx.TypeBuilder.SetCustomAttribute(caBuilder);
			}

			// Create initializer for the type.
			//
			ConstructorBuilder initCtorBuilder = ctx.TypeBuilder.DefineTypeInitializer();

			// Create default constructor.
			//
			ConstructorBuilder defCtorBuilder = ctx.TypeBuilder.DefineConstructor(
				MethodAttributes.Public,
				CallingConventions.Standard,
				Type.EmptyTypes);

			// Create object factory constructor.
			//
			ConstructorBuilder ofCtorBuilder = ctx.TypeBuilder.DefineConstructor(
				MethodAttributes.Public, 
				CallingConventions.Standard,
				_factoryParams);

			_defaultConstructorBuilder[type] = defCtorBuilder;
			_factoryConstructorBuilder[type] =  ofCtorBuilder;

			// Create method InitProperties
			//
			MethodBuilder initMethodBuilder = ctx.TypeBuilder.DefineMethod(
				"InitPropertiesOf" + type.Name,
				MethodAttributes.Private,
				typeof(void),
				Type.EmptyTypes);

			ctx.IniCtorGen = new MapGenerator(initCtorBuilder.   GetILGenerator());
			ctx.DefCtorGen = new MapGenerator(defCtorBuilder.    GetILGenerator());
			ctx.ObfCtorGen = new MapGenerator(ofCtorBuilder.     GetILGenerator());
			ctx.InitGen    = new MapGenerator(initMethodBuilder. GetILGenerator());

			// Scan properties.
			//
			PropertyInfo[] props = type.GetProperties(
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (PropertyInfo pi in props)
			{
				ctx.PropertyInfo  = pi;
				ctx.GetMethodInfo = pi.GetGetMethod(true);
				ctx.SetMethodInfo = pi.GetSetMethod(true);

				if (ctx.GetMethodInfo != null && ctx.GetMethodInfo.IsAbstract || 
					ctx.SetMethodInfo != null && ctx.SetMethodInfo.IsAbstract)
				{
					ctx.FieldType = pi.PropertyType;
					ctx.IsObject  = false;
					
					// Check for the MapTypeAttribute.
					//
					object[] attr = MapDescriptor.GetPropertyMapType(type, pi);

					if (attr != null)
					{
						foreach (MapTypeAttribute a in attr)
						{
							ctx.FieldType = a.MappedType;

							if (GetValueMember(ctx.FieldType, pi.PropertyType) != null)
							{
								ctx.IsObject = true;
								break;
							}
						}

						if (attr.Length > 0 && !ctx.IsObject)
						{
							throw new RsdnMapException(
								string.Format(
								"The '{0}' type does not have appropriate setter or getter member. " +
								"See '{1}' member of '{2}' type.",
								ctx.FieldType.Name,
								pi.Name,
								type.Name));
						}
					}

					// Create and initialize private field.
					//
					ctx.AttributeParams = MapDescriptor.GetPropertyParameters(pi);
					ctx.FieldBuilder    = ctx.TypeBuilder.DefineField(
						"_" + pi.Name, ctx.FieldType, FieldAttributes.Private);

					ctx.ParamBuilder = InitializeMemberParameters(ctx);

					InitializeAbstractClassField(ctx);
					CreateNonAbstractGettter    (ctx);
					CreateNonAbstractSetter     (ctx);
				}
			}

			ctx.DefCtorGen.ldarg_0.call(initMethodBuilder);
			ctx.ObfCtorGen.ldarg_0.call(initMethodBuilder);

			ConstructorInfo defParentCtor = GetDefaultConstructor(type);

			if (defParentCtor != null)
			{
				ctx.DefCtorGen.ldarg_0.call(defParentCtor);
			}

			if (ctx.ObfInitDataField != null)
			{
				ctx.ObfCtorGen
					.ldarg_1
					.ldloc(ctx.ObfInitDataField)
					.callvirt(typeof(MapInitializingData).GetProperty("MapDescriptor").GetSetMethod())
					
					.ldarg_1
					.ldc_i4_0
					.callvirt(typeof(MapInitializingData).GetProperty("IsInternal").GetSetMethod());
			}

			// Find more appropriate object factory ctor.
			//
			ConstructorInfo parentCtor = type.GetConstructor(
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null, _factoryParams, null);

			if (parentCtor != null)
			{
				ctx.ObfCtorGen
					.ldarg_0
					.ldarg_s(1)
					.call(parentCtor)
					;
			}
			else
			{
				parentCtor = type.GetConstructor(
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
					null, Type.EmptyTypes, null);

				if (parentCtor != null)
				{
					ctx.ObfCtorGen
						.ldarg_0
						.call(parentCtor)
						;
				}
			}

			DefineGetCreatedMembersMethod(ctx);
			DefineActions                (ctx);

			ctx.IniCtorGen.ret();
			ctx.DefCtorGen.ret();
			ctx.ObfCtorGen.ret();
			ctx.InitGen.   ret();

			_defaultConstructorBuilder.Remove(type);
			_factoryConstructorBuilder.Remove(type);

			return ctx.TypeBuilder.CreateType();
		}

		private static void DefineGetCreatedMembersMethod(GenContext ctx)
		{
			// Create IMapGenerated.GetCreatedObjects
			//
			MethodBuilder enumMethod = ctx.TypeBuilder.DefineMethod(
				"$GetCreatedMembers",
				MethodAttributes.Virtual,
				typeof(object[]),
				Type.EmptyTypes);

			MapGenerator gen = new MapGenerator();

			gen.Gen = enumMethod.GetILGenerator();

			LocalBuilder arr = gen.DeclareLocal(typeof(object[]));

			int count = 0;

			foreach (CreatedObject o in ctx.Objects)
				if (!o.IsPrimitive)
					count++;

			gen
				.ldc_i4(count)
				.newarr(typeof(object))
				.stloc(arr)
				.EndGen();

			for (int i = 0, n = 0; i < ctx.Objects.Count; i++)
			{
				CreatedObject createdObject = (CreatedObject)ctx.Objects[i];

				if (createdObject.IsPrimitive)
					continue;

				FieldBuilder fb = createdObject.FieldBuilder;

				gen
					.ldloc(arr)
					.ldc_i4(n)
					.ldarg_0
					.ldfld(fb)
					.BoxIfValueType(fb.FieldType)
					.stelem_ref
					.EndGen();

				n++;
			}

			gen.ldloc_0.ret();

			ctx.TypeBuilder.DefineMethodOverride(
				enumMethod,
				typeof(IMapGenerated).GetMethod("GetCreatedMembers"));
		}

		private static void DefineActions(GenContext ctx)
		{
			foreach (MapActionAttribute attr in ctx.Actions)
			{
				Type         interfaceType = attr.Type;
				MethodInfo[] methods       = interfaceType.GetMethods();

				foreach (MethodInfo m in methods)
				{
					ParameterInfo [] pi = m.GetParameters();
					Type []  parameters = new Type[pi.Length];

					for (int i = 0; i < pi.Length; i++)
						parameters[i] = pi[i].ParameterType;

					MethodBuilder mb = ctx.TypeBuilder.DefineMethod(
						"$" + interfaceType.FullName.Replace("+", ".") + "$" + m.Name,
						MethodAttributes.Virtual,
						m.ReturnType,
						parameters);

					MapGenerator gen = new MapGenerator();

					gen.Gen = mb.GetILGenerator();

					LocalBuilder lb              = null;
					Label        retLabel        = new Label();
					bool         returnIfNonZero = false;
					bool         returnIfZero    = false;
					
					if (m.ReturnType != typeof(void))
					{
						lb       = gen.DeclareLocal(m.ReturnType);
						retLabel = gen.DefineLabel();

						object[] attrs = m.ReturnTypeCustomAttributes.GetCustomAttributes(true);

						foreach (object o in attrs)
						{
							if      (o is MapReturnIfNonZeroAttribute) returnIfNonZero = true;
							else if (o is MapReturnIfZeroAttribute)    returnIfZero    = true;
						}
					}

					for (int i = 0; i < ctx.Objects.Count; i++)
					{
						CreatedObject createdObject = (CreatedObject)ctx.Objects[i];

						if (createdObject.IsPrimitive)
							continue;

						FieldBuilder field = createdObject.FieldBuilder;
						Type[]       types = field.FieldType.GetInterfaces();

						ctx.PropertyInfo = createdObject.PropertyInfo;

						foreach (Type type in types)
						{
							if (type != interfaceType)
								continue;

							InterfaceMapping im = field.FieldType.GetInterfaceMap(type);

							for (int j = 0; j < im.InterfaceMethods.Length; j++)
							{
								if (im.InterfaceMethods[j] == m)
								{
									MethodInfo targetMethod = im.TargetMethods[j];

									gen.ldarg_0.EndGen();

									if (field.FieldType.IsValueType)
									{
										if (targetMethod.IsPublic)
											gen.ldflda(field);
										else
											gen.ldfld(field).box(field.FieldType);
									}
									else
									{
										if (targetMethod.IsPublic)
											gen.ldfld(field);
										else
											gen.ldfld(field).castclass(interfaceType);
									}

									for (int k = 0; k < parameters.Length; k++)
									{
										object[] attrs = pi[k].GetCustomAttributes(true);
										bool     stop  = false;

										foreach (object a in attrs)
										{
											if (a is MapParentAttribute)
											{
												gen.ldarg_0.castclass(pi[k].ParameterType);
												stop = true;

												break;
											}

											if (a is MapPropertyInfoAttribute)
											{
												FieldBuilder ifb = GetPropertyInitField(ctx, createdObject);

												gen.ldsfld(ifb);
												stop = true;

												break;
											}
										}

										if (stop)
											continue;

										gen.ldarg_s((byte)(k + 1));
									}


									if (field.FieldType.IsValueType)
									{
										if (targetMethod.IsPublic)
											gen.call    (targetMethod);
										else
											gen.callvirt(im.InterfaceMethods[j]);
									}
									else
									{
										if (targetMethod.IsPublic)
											gen.callvirt(targetMethod);
										else
											gen.callvirt(im.InterfaceMethods[j]);
									}

									if (lb != null)
									{
										gen.stloc(lb);

										if (i < ctx.Objects.Count - 1)
										{
											if (returnIfNonZero)
											{
												gen
													.ldloc(lb)
													.brtrue(retLabel);
											} 
											else if (returnIfZero)
											{
												gen
													.ldloc(lb)
													.brfalse(retLabel);
											}
										}
									}

									break;
								}
							}

							break;
						}
					}

					if (lb != null)
						gen
							.MarkLabel(retLabel)
							.ldloc(lb);

					gen.ret();

					ctx.TypeBuilder.DefineMethodOverride(mb, m);
				}
			}
		}

		static private void SetNonSerializedAttribute(FieldBuilder fieldBuilder)
		{
			ConstructorInfo        ci        = typeof(NonSerializedAttribute).GetConstructor(Type.EmptyTypes);
			CustomAttributeBuilder caBuilder = new CustomAttributeBuilder(ci, new object[0]);

			fieldBuilder.SetCustomAttribute(caBuilder);
		}

		static private PropertyBuilder GetDescriptor(GenContext ctx)
		{
			if (ctx.Descriptor == null)
			{
				FieldBuilder fieldDescriptor = ctx.TypeBuilder.DefineField(
					"_$MapDescriptor",
					typeof(MapDescriptor),
					FieldAttributes.Private | FieldAttributes.Static);

				SetNonSerializedAttribute(fieldDescriptor);

				ctx.Descriptor = ctx.TypeBuilder.DefineProperty(
					"$MapDescriptor",
					PropertyAttributes.None,
					typeof(MapDescriptor),
					Type.EmptyTypes);

				MethodBuilder getMethod = ctx.TypeBuilder.DefineMethod(
					"get_" + ctx.Descriptor.Name,
					MethodAttributes.Private |
					MethodAttributes.Static |
					MethodAttributes.HideBySig | 
					MethodAttributes.SpecialName,
					ctx.Descriptor.PropertyType,
					Type.EmptyTypes);

				MapGenerator gen = new MapGenerator(getMethod.GetILGenerator());

				Label label = gen.DefineLabel();

				gen
					.ldsfld(fieldDescriptor)
					.brtrue_s(label)
					.LoadType(ctx.TypeBuilder.BaseType)
					.call(typeof(MapDescriptor), "GetDescriptor", typeof(Type))
					.stsfld(fieldDescriptor)
					.MarkLabel(label)
					.ldsfld(fieldDescriptor)
					.ret();

				ctx.Descriptor.SetGetMethod(getMethod);
			}

			return ctx.Descriptor;
		}

		static private FieldBuilder InitializeMemberParameters(GenContext ctx)
		{
			FieldBuilder paramBuilder = null;

			if (ctx.AttributeParams != null)
			{
				paramBuilder = ctx.TypeBuilder.DefineField(
					string.Format("_{0}_$parameters", ctx.PropertyInfo.Name),
					typeof(object[]),
					FieldAttributes.Private | FieldAttributes.Static);

				SetNonSerializedAttribute(paramBuilder);

				PropertyBuilder descriptor = GetDescriptor(ctx);

				ctx.IniCtorGen
					.call(descriptor.GetGetMethod(true))
					.ldstr(ctx.PropertyInfo.Name)
					.callvirt(typeof(MapDescriptor), "GetPropertyParameters", typeof(string))
					.stsfld(paramBuilder)
					;
			}

			return paramBuilder;
		}

		class ConstructType 
		{
			public ConstructType(Type fieldType)
			{
				_fieldType = fieldType;

				if (MapEmit._defaultConstructorBuilder[fieldType] == null)
				{
					_constructType = fieldType.IsAbstract?
						MapDescriptor.GetDescriptor(fieldType).MappedType:
						fieldType;
				}
			}

			Type _fieldType;
			Type _constructType;

			public ConstructorInfo GetConstructor(Type[] types)
			{
				return
					_constructType == null?
					(ConstructorInfo)MapEmit._factoryConstructorBuilder[_fieldType]:
					_constructType.GetConstructor(
						BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
						null, types, null);
			}

			public ConstructorInfo GetDefaultConstructor()
			{
				return 
					_constructType == null?
					(ConstructorInfo)MapEmit._defaultConstructorBuilder[_fieldType]:
					MapEmit.GetDefaultConstructor(_constructType);
			}
		}

		static private void InitializeAbstractClassField(GenContext ctx)
		{
			CreatedObject createdObject = new CreatedObject(ctx);

			ctx.Objects.Add(createdObject);

			if (createdObject.IsPrimitive)
				return;

			if (ctx.FieldType.IsAbstract && MapDescriptor.GetDescriptor(ctx.FieldType) == null)
				return;

			ConstructType constructType = new ConstructType(ctx.FieldType);

			bool callDefaultCtor = ctx.FieldType.IsClass;

			// Call MapDescriptor to construct the object.
			//
			if (ctx.FieldType.IsAbstract)
			{
				// Object factory construction.
				//
				FieldBuilder fieldBuilder = ctx.TypeBuilder.DefineField(
					string.Format("_{0}_$MapDescriptor", ctx.PropertyInfo.Name),
					typeof(MapDescriptor),
					FieldAttributes.Private | FieldAttributes.Static);

				if (ctx.ObfInitDataField == null)
				{
					ctx.ObfInitDataField = ctx.ObfCtorGen.DeclareLocal(typeof(MapDescriptor));

					ctx.ObfCtorGen
						.ldarg_1
						.callvirt(typeof(MapInitializingData).GetProperty("MapDescriptor").GetGetMethod())
						.stloc(ctx.ObfInitDataField)

						.ldarg_1
						.ldc_i4_1
						.callvirt(typeof(MapInitializingData).GetProperty("IsInternal").GetSetMethod());
				}

				Label l1 = ctx.ObfCtorGen.DefineLabel();

				ctx.ObfCtorGen
					.ldsfld(fieldBuilder)
					.brtrue_s(l1)
					.LoadType(ctx.FieldType)
					.call(typeof(MapDescriptor), "GetDescriptor", typeof(Type))
					.stsfld(fieldBuilder)
					.MarkLabel(l1)
					.ldarg_0
					.ldsfld(fieldBuilder)
					.ldarg_1
					.EndGen();

				MethodInfo memberParams = 
					typeof(MapInitializingData).GetProperty("MemberParameters").GetSetMethod();

				if (ctx.ParamBuilder != null)
				{
					ctx.ObfCtorGen
						.ldarg_1
						.ldsfld(ctx.ParamBuilder)
						.callvirt(memberParams);
				}

				ctx.ObfCtorGen
					.callvirtNoGenerics(typeof(MapDescriptor), "CreateInstanceEx", _factoryParams)
					.isinst(ctx.FieldType)
					.stfld(ctx.FieldBuilder);

				if (ctx.ParamBuilder != null)
				{
					ctx.ObfCtorGen
						.ldarg_1
						.ldnull
						.callvirt(memberParams);
				}

				if (ctx.ParamBuilder != null)
				{
					// Default constructor.
					//
					if (ctx.DefInitDataField == null)
					{
						ctx.DefInitDataField = ctx.DefCtorGen.DeclareLocal(typeof(MapInitializingData));

						ctx.DefCtorGen
							.newobj(GetDefaultConstructor(typeof(MapInitializingData)))
							.stloc_0

							.ldloc_0
							.ldc_i4_1
							.callvirt(typeof(MapInitializingData).GetProperty("IsInternal").GetSetMethod());
					}

					Label l2 = ctx.DefCtorGen.DefineLabel();

					ctx.DefCtorGen
						.ldsfld(fieldBuilder)
						.brtrue_s(l2)
						.LoadType(ctx.FieldType)
						.call(typeof(MapDescriptor), "GetDescriptor", typeof(Type))
						.stsfld(fieldBuilder)
						.MarkLabel(l2)

						.ldloc_0
						.ldsfld(ctx.ParamBuilder)
						.callvirt(memberParams)

						.ldarg_0
						.ldsfld(fieldBuilder)
						.ldloc_0
						.callvirtNoGenerics(typeof(MapDescriptor), "CreateInstanceEx", _factoryParams)
						.isinst(ctx.FieldType)
						.stfld(ctx.FieldBuilder)

						.ldloc_0
						.ldnull
						.callvirt(memberParams);

					callDefaultCtor = false;
				}
			} 
			else
			{
				ConstructorInfo ofci = null;

				if (ctx.AttributeParams != null)
				{
					Type[] types = new Type[ctx.AttributeParams.Length];

					for (int i = 0; i < ctx.AttributeParams.Length; i++)
						types[i] = ctx.AttributeParams[i] != null?
							ctx.AttributeParams[i].GetType(): typeof(object);

					ofci = constructType.GetConstructor(types);

					if (ofci != null)
					{
						ParameterInfo[] pi = ofci.GetParameters();

						callDefaultCtor = false;

						ctx.DefCtorGen.ldarg_0.EndGen();
						ctx.ObfCtorGen.ldarg_0.EndGen();

						for (int i = 0; i < ctx.AttributeParams.Length; i++)
						{
							object o = ctx.AttributeParams[i];

							if (ctx.DefCtorGen.LoadConst(o))
							{
								if (o.GetType().IsValueType && !pi[i].ParameterType.IsValueType)
									ctx.DefCtorGen.box(o.GetType());
							}
							else
							{
								ctx.DefCtorGen
									.ldsfld(ctx.ParamBuilder)
									.ldc_i4(i)
									.ldelem_ref
									.CastTo(types[i]);
							}

							if (ctx.ObfCtorGen.LoadConst(o))
							{
								if (o.GetType().IsValueType && !pi[i].ParameterType.IsValueType)
									ctx.ObfCtorGen.box(o.GetType());
							}
							else
							{
								ctx.ObfCtorGen
									.ldsfld(ctx.ParamBuilder)
									.ldc_i4(i)
									.ldelem_ref
									.CastTo(types[i]);
							}
						}

						ctx.DefCtorGen
							.newobj(ofci)
							.stfld(ctx.FieldBuilder);

						ctx.ObfCtorGen
							.newobj(ofci)
							.stfld(ctx.FieldBuilder);
					}
				}

				if (ofci == null)
				{
					ofci = constructType.GetConstructor(_factoryParams);

					if (ofci != null)
					{
						MethodInfo memberParams = 
							typeof(MapInitializingData).GetProperty("MemberParameters").GetSetMethod();

						// Object factory constructor.
						//
						if (ctx.ObfInitDataField == null)
						{
							ctx.ObfInitDataField = ctx.ObfCtorGen.DeclareLocal(typeof(MapDescriptor));

							ctx.ObfCtorGen
								.ldarg_1
								.callvirt(typeof(MapInitializingData).GetProperty("MapDescriptor").GetGetMethod())
								.stloc(ctx.ObfInitDataField)

								.ldarg_1
								.ldc_i4_1
								.callvirt(typeof(MapInitializingData).GetProperty("IsInternal").GetSetMethod());
						}

						if (ctx.ParamBuilder != null)
						{
							ctx.ObfCtorGen
								.ldarg_1
								.ldsfld(ctx.ParamBuilder)
								.callvirt(memberParams);
						}

						ctx.ObfCtorGen
							.ldarg_0
							.ldarg_1
							.newobj(ofci)
							.stfld(ctx.FieldBuilder);

						if (ctx.ParamBuilder != null)
						{
							ctx.ObfCtorGen
								.ldarg_1
								.ldnull
								.callvirt(memberParams);
						}

						if (ctx.ParamBuilder != null)
						{
							// Default constructor.
							//
							if (ctx.DefInitDataField == null)
							{
								ctx.DefInitDataField = ctx.DefCtorGen.DeclareLocal(typeof(MapInitializingData));

								ctx.DefCtorGen
									.newobj(GetDefaultConstructor(typeof(MapInitializingData)))
									.stloc_0

									.ldloc_0
									.ldc_i4_1
									.callvirt(typeof(MapInitializingData).GetProperty("IsInternal").GetSetMethod());
							}

							ctx.DefCtorGen
								.ldloc_0
								.ldsfld(ctx.ParamBuilder)
								.callvirt(memberParams)
								.ldarg_0
								.ldloc_0
								.newobj(ofci)
								.stfld(ctx.FieldBuilder)
								.ldloc_0
								.ldnull
								.callvirt(memberParams);
						}
					}
					else
					{
						ofci = constructType.GetConstructor(Type.EmptyTypes);

						if (ofci != null)
						{
							ctx.ObfCtorGen
								.ldarg_0
								.newobj(ofci)
								.stfld(ctx.FieldBuilder);
						}
					}
				}
			}

			if (callDefaultCtor)
			{
				// Default construction.
				//
				ConstructorInfo ci = constructType.GetDefaultConstructor();

				if (ci == null)
				{
					throw new RsdnMapException(
						string.Format("The '{0}' type has to have public default constructor.",
						ctx.FieldType.Name));
				}

				// Call default member constructor inside default constractor of the type.
				//
				ctx.DefCtorGen
					.ldarg_0
					.newobj(ci)
					.stfld(ctx.FieldBuilder);
			}

			if (ctx.FieldType.GetInterface(typeof(IMapSetPropertyInfo).Name) != null)
			{
				FieldBuilder initFieldBuilder = GetPropertyInitField(ctx, createdObject);

				if (ctx.FieldBuilder.FieldType.IsValueType)
				{
					ctx.InitGen
						.ldarg_0
						.ldflda(ctx.FieldBuilder)
						.ldsfld(initFieldBuilder)
						.ldarg_0
						.call(ctx.FieldBuilder.FieldType, "SetInfo", typeof(MapPropertyInfo), typeof(object));
				}
				else
				{
					ctx.InitGen
						.ldarg_0
						.ldfld(ctx.FieldBuilder)
						.castclass(typeof(IMapSetPropertyInfo))
						.ldsfld(initFieldBuilder)
						.ldarg_0
						.callvirt(typeof(IMapSetPropertyInfo), "SetInfo", typeof(MapPropertyInfo), typeof(object));
				}
			}
		}

		static private FieldBuilder GetPropertyInitField(GenContext ctx, CreatedObject createdObject)
		{
			if (createdObject.InitFieldBuilder == null)
			{
				createdObject.InitFieldBuilder = ctx.TypeBuilder.DefineField(
					string.Format("_{0}_$MapPropertyInfo", ctx.PropertyInfo.Name),
					typeof(MapPropertyInfo),
					FieldAttributes.Private | FieldAttributes.Static);

				SetNonSerializedAttribute(createdObject.InitFieldBuilder);

				ConstructorInfo ci = typeof(MapPropertyInfo).GetConstructor(new Type[] { typeof(PropertyInfo) });

				ctx.IniCtorGen
					.LoadType(ctx.Type)
					.ldstr(ctx.PropertyInfo.Name)
					.ldc_i4((int)(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
					.call(typeof(Type), "GetProperty", typeof(string), typeof(BindingFlags))
					.newobj(ci)
					.stsfld(createdObject.InitFieldBuilder);
			}

			return createdObject.InitFieldBuilder;
		}

		static private void CreateNonAbstractSetter(GenContext ctx)
		{
			MemberInfo member = 
				ctx.IsObject? GetValueMember(ctx.FieldType, ctx.PropertyInfo.PropertyType): null;

			if (ctx.SetMethodInfo == null && ctx.IsObject && 
				member is PropertyInfo && ((PropertyInfo)member).GetSetMethod() == null)
				return;

			MethodBuilder setMethod = ctx.TypeBuilder.DefineMethod(
				ctx.SetMethodInfo != null? ctx.SetMethodInfo.Name: "set_" + ctx.PropertyInfo.Name,
				MethodAttributes.Public |
				MethodAttributes.Virtual |
				MethodAttributes.HideBySig |
				MethodAttributes.SpecialName,
				null,
				new Type[] { ctx.PropertyInfo.PropertyType});

			MapGenerator gen = new MapGenerator(setMethod.GetILGenerator());

			gen
				.ldarg_0
				.EndGen();

			if (ctx.IsObject)
			{
				gen
					.ldfldWhithCheck(ctx.FieldBuilder)
					.ldarg_1
					.EndGen();

				if (member is PropertyInfo)
				{
					PropertyInfo mpi = ((PropertyInfo)member);

					if (ctx.PropertyInfo.PropertyType.IsValueType && !mpi.PropertyType.IsValueType)
						gen.box(ctx.PropertyInfo.PropertyType);

					if (ctx.FieldType.IsValueType) gen.call    (mpi.GetSetMethod());
					else                           gen.callvirt(mpi.GetSetMethod());
				}
				else
				{
					FieldInfo mfi = (FieldInfo)member;

					if (ctx.PropertyInfo.PropertyType.IsValueType && !mfi.FieldType.IsValueType)
						gen.box(ctx.PropertyInfo.PropertyType);

					gen.stfld(mfi);
				}
			}
			else
			{
				gen
					.ldarg_1
					.stfld(ctx.FieldBuilder);
			}

			// IPropertyChangeNotification.
			//
			Type[] interfaces = ctx.Type.GetInterfaces();

			foreach (Type itf in interfaces)
			{
				if (itf == typeof(IMapNotifyPropertyChanged))
				{
					InterfaceMapping im  = ctx.Type.GetInterfaceMap(itf);
					MethodInfo       mi  = im.TargetMethods[0];
					FieldBuilder     ifb =
						GetPropertyInitField(ctx, (CreatedObject)ctx.Objects[ctx.Objects.Count - 1]);

					gen.ldarg_0.EndGen();

					if (mi.IsPublic == false)
						gen.castclass(typeof(IMapNotifyPropertyChanged));

					gen.ldsfld(ifb);

					if (mi.IsPublic)
						gen.callvirt(mi);
					else
						gen.callvirt(im.InterfaceMethods[0]);

					break;
				}
			}

			gen.ret();
		}

		static private void CreateNonAbstractGettter(GenContext ctx)
		{
			MethodBuilder getMethod = ctx.TypeBuilder.DefineMethod(
				ctx.GetMethodInfo != null? ctx.GetMethodInfo.Name: "get_" + ctx.PropertyInfo.Name,
				MethodAttributes.Public | 
				MethodAttributes.Virtual |
				MethodAttributes.HideBySig | 
				MethodAttributes.SpecialName,
				ctx.PropertyInfo.PropertyType,
				Type.EmptyTypes);

			MapGenerator gen = new MapGenerator(getMethod.GetILGenerator());

			if (ctx.FieldType.IsValueType && ctx.IsObject) gen.ldarg_0.ldflda(ctx.FieldBuilder);
			else                                           gen.ldarg_0.ldfld (ctx.FieldBuilder);

			if (ctx.IsObject)
			{
				MemberInfo member = GetValueMember(ctx.FieldType, ctx.PropertyInfo.PropertyType);

				if (member is PropertyInfo)
				{
					PropertyInfo mpi = ((PropertyInfo)member);

					if (ctx.FieldType.IsValueType) gen.call    (mpi.GetGetMethod());
					else                           gen.callvirt(mpi.GetGetMethod());

					if (ctx.PropertyInfo.PropertyType.IsValueType && !mpi.PropertyType.IsValueType)
					{
						gen.CastTo(ctx.PropertyInfo.PropertyType);
					}
					else if (mpi.PropertyType != ctx.PropertyInfo.PropertyType && 
						!ctx.PropertyInfo.PropertyType.IsEnum)
					{
						gen.castclass(ctx.PropertyInfo.PropertyType);
					}
				}
				else
				{
					FieldInfo mfi = (FieldInfo)member;

					gen.ldfld((FieldInfo)member);

					if (ctx.PropertyInfo.PropertyType.IsValueType && !mfi.FieldType.IsValueType)
					{
						gen.CastTo(ctx.PropertyInfo.PropertyType);
					}
					else if (mfi.FieldType != ctx.PropertyInfo.PropertyType && 
						!ctx.PropertyInfo.PropertyType.IsEnum)
					{
						gen.castclass(mfi.FieldType);
					}
				}
			}

			gen.ret();
		}

		internal static MapDescriptor CreateDescriptor(Type type)
		{
			Type         originalType = type;
			AssemblyName assemblyName = new AssemblyName();
			string       assemblyDir  = Path.GetDirectoryName(type.Module.FullyQualifiedName);
			string       fullName     = type.FullName.Replace('+', '.');

			assemblyName.Name = fullName + ".MapDescriptor.dll";

			AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(
				assemblyName,
				AssemblyBuilderAccess.RunAndSave,
				assemblyDir);

			ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

			// Create non abstract class, if the current one is abstract.
			//
			if (type.IsAbstract)
				type = CreateNonAbstractType(type, moduleBuilder);

			// Create mapping extension MapDescriptor.
			//
			TypeBuilder typeBuilder = moduleBuilder.DefineType(
				fullName + ".MappingExtension.$$MapDescriptor", TypeAttributes.Public, typeof(MapDescriptor));

			CreateCreateInstanceMethodOfDescriptor (type, typeBuilder);
			CreateCreateInstance4MethodOfDescriptor(type, typeBuilder);

			typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

			// Create the type descriptor.
			//
			Type descriptorType = typeBuilder.CreateType();

			MapDescriptor desc = (MapDescriptor)Activator.CreateInstance(descriptorType);
			desc.InitMembers(originalType, type, moduleBuilder);

#if DEBUG
			try
			{
				assemblyBuilder.Save(assemblyName.Name);

				System.Diagnostics.Debug.WriteLine(
					string.Format("The '{0}' type saved in '{1}\\{2}'.",
					fullName,
					assemblyDir,
					assemblyName.Name));
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(
					string.Format("Can't save the '{0}' assembly for the '{1}' type in '{2}': {3}.", 
					assemblyName.Name,
					fullName,
					assemblyDir,
					ex.Message));
			}
#endif

			return desc;
		}

		static private void CreateCreateInstanceMethodOfDescriptor(Type type, TypeBuilder typeBuilder)
		{
			// Create CreateInstance Method
			//
			ConstructorInfo ci = GetDefaultConstructor(type);

			if (ci == null || !ci.IsPublic)
			{
				System.Diagnostics.Debug.WriteLine(
					string.Format("The '{0}' type must have the public default constructor.", type.Name));
			}
			else
			{
				MethodBuilder methodBuilder = typeBuilder.DefineMethod(
					"CreateInstance",
					MethodAttributes.Public | 
					MethodAttributes.Virtual |
					MethodAttributes.HideBySig,
					typeof(object),
					Type.EmptyTypes);
			
				MapGenerator gen = new MapGenerator(methodBuilder.GetILGenerator());

#if HANDLE_EXCEPTIONS
				LocalBuilder l0 = gen.DeclareLocal(typeof(Exception));
				LocalBuilder l1 = gen.DeclareLocal(typeof(object));
#endif

				gen
#if HANDLE_EXCEPTIONS
					// try
					// {
					.BeginExceptionBlock
#endif

					// return new BizObject();
					//
					.newobj(ci)

#if HANDLE_EXCEPTIONS
					.stloc(l1)

					// catch (Exception ex)
					// {
					.BeginCatchBlock_Exception

					.stloc(l0)
					.ldloc(l0)
					.call(typeof(MapDescriptor), "HandleException", BindingFlags.Static | BindingFlags.NonPublic)
					.ldnull
					.stloc(l1)

					.EndExceptionBlock

					.ldloc(l1)
#endif
					.ret();
			}
		}

		static private void CreateCreateInstance4MethodOfDescriptor(Type type, TypeBuilder typeBuilder)
		{
			ConstructorInfo ci = type.GetConstructor(
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
				null, _factoryParams, null);

			bool addParameter = false;

			if (ci != null && ci.IsPublic)
			{
				addParameter = true;
			}
			else
			{
				ci = type.GetConstructor(
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
					null, Type.EmptyTypes, null);

				if (ci == null || !ci.IsPublic)
				{
					System.Diagnostics.Debug.WriteLine(
						string.Format("The '{0}' type must have the public default constructor.", type.Name));

					return;
				}
			}

			MethodBuilder methodBuilder = typeBuilder.DefineMethod(
				"CreateInstance",
				MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
				typeof(object),
				_factoryParams);

			MapGenerator gen = new MapGenerator(methodBuilder.GetILGenerator());

			LocalBuilder l0 = gen.DeclareLocal(typeof(Exception));
			LocalBuilder l1 = gen.DeclareLocal(typeof(object));

#if HANDLE_EXCEPTIONS
			gen
				// try
				// {
				.BeginExceptionBlock.EndGen();
#endif

			// return new BizObject();
			//
			if (addParameter)
				gen.ldarg_s(1);

			gen
				.newobj(ci)
#if HANDLE_EXCEPTIONS
				.stloc(l1)

				// catch (Exception ex)
				// {
				.BeginCatchBlock_Exception

				.stloc(l0)
				.ldloc(l0)
				.call(typeof(MapDescriptor), "HandleException", BindingFlags.Static | BindingFlags.NonPublic)
				.ldnull
				.stloc(l1)

				.EndExceptionBlock

				.ldloc(l1)
#endif
				.ret();
		}

		internal static object CreateMemberMapper(
			Type          originalType,
			Type          objectType,
			Type          memberType,
			string        memberName,
			bool          isProperty,
			bool          createGet,
			bool          createSet,
			ModuleBuilder moduleBuilder,
			Attribute[]   valueAttributes)
		{
			string fullName = originalType.FullName.Replace('+', '.');

			TypeBuilder typeBuilder = moduleBuilder.DefineType(
				fullName + ".MappingExtension.$" + memberName,
				TypeAttributes.Public,
				isProperty? typeof(PropertyMapper): typeof(FieldMapper));

			// Create default constructor.
			//
			typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

			MethodInfo getMethod    = null;
			Type       definingType = objectType;

			if (isProperty)
			{
				// Check if mapping extension property was created.
				//
				if (!createGet)
				{
					getMethod = objectType.GetMethod("get_" + memberName);
					createSet = getMethod != null;
				}

				if (createGet && getMethod == null)
				{
					getMethod = objectType.GetProperty(memberName).GetGetMethod();

					if (getMethod == null)
					{
						createGet = false;
					}
					else
					{
						definingType = getMethod.DeclaringType;

						if (definingType == objectType && objectType.BaseType.IsAbstract)
						{
							PropertyInfo pi = objectType.BaseType.GetProperty(memberName);

							if (pi != null)
							{
								MethodInfo mi = pi.GetGetMethod();

								if (mi != null)
									getMethod = mi;
							}
						}
					}
				}
			}

			// Create GetValue method.
			//
			if (createGet)
			{
				CreateGetter(memberType, typeBuilder, objectType, isProperty, getMethod, memberName);
			}

			MethodInfo setMethod = null;

			if (isProperty)
			{
				// Check if mapping extension property was created.
				//
				if (!createSet)
				{
					setMethod = objectType.GetMethod("set_" + memberName);
					createSet = setMethod != null;
				}

				if (createSet && setMethod == null)
				{
					setMethod = objectType.GetProperty(memberName).GetSetMethod();
				}
			}

			// Create SetValue method.
			//
			if (createSet)
			{
				CreateSetter(
					memberType, typeBuilder, objectType, isProperty, setMethod, memberName, valueAttributes);
			}

			Type descriptorType = typeBuilder.CreateType();

			return Activator.CreateInstance(descriptorType);
		}

		static private void CreateSetter(
			Type        memberType,
			TypeBuilder typeBuilder,
			Type        objectType,
			bool        isProperty,
			MethodInfo  setMethod,
			string      memberName,
			Attribute[] valueAttributes)
		{
			if (IsPrimitive(memberType))
			{
				MethodBuilder methodBuilder = typeBuilder.DefineMethod(
					"SetValue",
					MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
					null,
					new Type[] { typeof(object), typeof(object) } );
			
				MapGenerator gen = new MapGenerator(methodBuilder.GetILGenerator());

				// code
				//
				gen
					//.nop                  // IL_0000: nop 
					.ldarg_1                // IL_0001: ldarg.1
					.castclass(objectType); // IL_0002: castclass CS.ActionTest

				if (IsNullableType(memberType))
				{
					PropertyInfo    pi = memberType.GetProperty("Value");
					FieldInfo       fi = memberType.GetField   ("Null");
					ConstructorInfo ci = memberType.GetConstructor(new Type[] { pi.PropertyType } );

					// public override void SetValue(object obj1, object obj2)
					// {
					//     ((MyType) obj1).Field = 
					//         obj2 == null || obj2 is DBNull? SqlInt32.Null:
					//         obj2 is SqlInt32?               (SqlInt32)obj2:
					//                                         new SqlInt32(Convert.ToInt32(obj2));
					// }

					Label l1 = gen.DefineLabel();
					Label l2 = gen.DefineLabel();
					Label l3 = gen.DefineLabel();

					gen
						.ldarg_2                    // IL_0006:  ldarg.2    
						.brfalse_s(l1)              // IL_0007:  brfalse.s  IL_002b
						.ldarg_2                    // IL_0009:  ldarg.2    
						.IsDBNull                   // IL_000a:  isinst     class [mscorlib]System.DBNull
						.brtrue_s(l1)               // IL_000f:  brtrue.s   IL_004d
						.ldarg_2                    // IL_0011:  ldarg.2    

						.isinst(memberType)         // IL_000a:  isinst     valuetype SqlInt32
						.brtrue_s(l2)               // IL_000f:  brtrue.s   IL_001e

						.ldarg_2                    // IL_0011:  ldarg.2
						.ConvertTo(pi.PropertyType) // IL_0012:  call       int32 [mscorlib]System.Convert::ToInt32(object)
						.newobj(ci)                 // IL_0017:  newobj     instance void SqlInt32::.ctor(int32)
						.br_s(l3)                   // IL_001c:  br.s       IL_0029

						.MarkLabel(l2)
					
						.ldarg_2                    // IL_001e:  ldarg.2    
						.unbox(memberType)          // IL_001f:  unbox      [System.Data]System.Data.SqlTypes.SqlInt32
						.ldobj(memberType)          // IL_0024:  ldobj      [System.Data]System.Data.SqlTypes.SqlInt32
						.br_s(l3)                   // IL_0029:  br.s       IL_0030

						.MarkLabel(l1)

						.ldsfld(fi)                 // IL_002b:  ldsfld     valuetype SqlInt32 SqlInt32::Null
					
						.MarkLabel(l3);

					// IL_0030:  stfld      valuetype [System.Data]System.Data.SqlTypes.SqlInt32 AllTest.Dest::m_int
					// IL_0035:  ret        
				}
#if VER2
				else if (IsNullableValue(memberType))
				{
					PropertyInfo    pi = memberType.GetProperty("Value");
					ConstructorInfo ci = memberType.GetConstructor(new Type[] { pi.PropertyType });

					// public void SetValue(object obj, object value)
					// {
					//     ((ActionTest) obj).NullInt = 
					//         value == null || value is DBNull? null:
					//         value is int? ?                   (int?)value:
					//                                           new int?(Convert.ToInt32(value));
					// }

					Label l1 = gen.DefineLabel();
					Label l2 = gen.DefineLabel();
					Label l3 = gen.DefineLabel();

					// .maxstack 3
					// .locals ([mscorlib]System.Nullable`1<int32> nullable1)

					LocalBuilder lb = gen.DeclareLocal(memberType);

					gen
						.ldarg_2                    // IL_0007: ldarg.2 
						.brfalse_s(l1)              // IL_0008: brfalse.s L_002f
						.ldarg_2                    // IL_000a: ldarg.2
						.IsDBNull                   // IL_000b: isinst [mscorlib]System.DBNull
						.brtrue_s(l1)               // IL_0010: brtrue.s L_002f
						.ldarg_2                    // IL_0012: ldarg.2

						.isinst(memberType)         // IL_0013: isinst [mscorlib]System.Nullable`1<int32>
						.brtrue_s(l2)               // IL_0018: brtrue.s L_0027

						.ldarg_2                    // IL_001a: ldarg.2
						.ConvertTo(pi.PropertyType) // IL_001b: call int32 [mscorlib]System.Convert::ToInt32(object)
						.newobj(ci)                 // IL_0020: newobj instance void [mscorlib]System.Nullable`1<int32>::.ctor(!0)
						.br_s(l3)                   // IL_0025: br.s L_002d

						.MarkLabel(l2)

						.ldarg_2                    // IL_0027: ldarg.2
						.unbox_any(memberType)      // IL_0028: unbox.any [mscorlib]System.Nullable`1<int32>
						.br_s(l3)                   // IL_002d: br.s L_0038

						.MarkLabel(l1)

						.ldloca_s(0)                // IL_002f: ldloca.s nullable1
						.initobj(memberType)        // IL_0031: initobj [mscorlib]System.Nullable`1<int32>
						.ldloc_0                    // IL_0037: ldloc.0 

						.MarkLabel(l3);

					// L_0038: stfld [mscorlib]System.Nullable`1<int32> CS.ActionTest::NullInt
					// L_003d: ret 
				}
#endif
				else
				{
					if (memberType.IsEnum || memberType == typeof(string) || valueAttributes.Length > 0)
					{
						gen
							.ldarg_0
							.ldarg_2
							.call(typeof(BaseMemberMapper), "MapFrom", typeof(object));
					}
					else
					{
						Label l1 = gen.DefineLabel();
						Label l2 = gen.DefineLabel();

						gen
							.ldarg_2
							.isinst(typeof(DBNull))
							.brtrue_s(l1)
							.ldarg_2
							.br_s(l2)
							.MarkLabel(l1)
							.ldnull
							.MarkLabel(l2);

						//gen
						//	.ldarg_0
						//	.ldarg_2
						//	.call(typeof(BaseMemberMapper), "MapFrom2", typeof(object));
					}

					if (memberType.IsEnum)
					{
						gen.CastTo(memberType);
					}
					else
					{
						gen.ConvertTo(memberType);
					}
				}

				if (isProperty)
				{
					gen.callvirt(setMethod);
				}
				else
				{
					gen.stfld(objectType, memberName);
				}

				gen.ret();
			}
		}

		static private void CreateGetter(
			Type        memberType,
			TypeBuilder typeBuilder,
			Type        objectType,
			bool        isProperty,
			MethodInfo  getMethod,
			string      memberName)
		{
			PropertyInfo piIsNull  = memberType.GetProperty("IsNull");
			PropertyInfo piHasNull = memberType.GetProperty("HasValue");
			PropertyInfo piValue   = memberType.GetProperty("Value");

			MethodBuilder methodBuilder = typeBuilder.DefineMethod(
				"GetValue",
				MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig,
				typeof(object),
				new Type[] { typeof(object) } );
			
			MapGenerator gen = new MapGenerator(methodBuilder.GetILGenerator());

			gen
				.ldarg_0
				.ldarg_1
				.castclass(getMethod != null? getMethod.DeclaringType: objectType)
				.EndGen();

			if (isProperty)
			{
				gen.callvirt(getMethod);
			}
			else
			{
				gen.ldfld(objectType, memberName);
			}

			Label end = gen.DefineLabel();

			if (piIsNull != null)
			{
				LocalBuilder local = gen.DeclareLocal(memberType);
				Label        l1    = gen.DefineLabel();

				gen
					.stloc(local)
					.ldloca_s(0)

					.call(piIsNull.GetGetMethod())
					.brfalse_s(l1)
					.ldnull
					.br_s(end)

					.MarkLabel(l1)
					.ldloca_s(0);
			}

			if (piHasNull != null)
			{
				LocalBuilder local = gen.DeclareLocal(memberType);
				Label l1 = gen.DefineLabel();

				gen
					.stloc_0
					.ldloca_s(0)

					.call(piHasNull.GetGetMethod())
					.brtrue_s(l1)
					.ldnull
					.br_s(end)

					.MarkLabel(l1)
					.ldloca_s(0);
			}

			Type retType = memberType;

			if (piValue != null)
			{
				retType = piValue.PropertyType;

				gen.call(piValue.GetGetMethod());
			}

			gen
				.BoxIfValueType(retType)

				.MarkLabel(end)

				.call(typeof(BaseMemberMapper), "MapTo", typeof(object))
				.ret();
		}

		static private bool IsPrimitive(Type type)
		{
			return IsBaseType(type) || IsNullableValue(type) || IsNullableType(type);
		}

		static private bool IsBaseType(Type type)
		{
			return
				type == typeof(string)   || type == typeof(bool)    ||
				type == typeof(byte)     || type == typeof(char)    ||
				type == typeof(DateTime) || type == typeof(decimal) ||
				type == typeof(double)   || type == typeof(Int16)   ||
				type == typeof(Int32)    || type == typeof(Int64)   ||
				type == typeof(sbyte)    || type == typeof(float)   ||
				type == typeof(UInt16)   || type == typeof(UInt32)  ||
				type == typeof(UInt64)   || type == typeof(Guid)    ||
				type.IsEnum;
		}

		private static bool IsNullableType(Type type)
		{
			return 
				type.GetInterface("INullable") != null &&
				type.GetProperty ("Value")     != null &&
				type.GetField    ("Null")      != null;
		}

		private static bool IsNullableValue(Type type)
		{
#if VER2
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				Type[] types = type.GetGenericArguments();

				return types.Length == 1 && IsBaseType(types[0]);
			}
#endif

			return false;
		}
	}
}
