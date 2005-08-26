/*
 * File:    MapGenerator.cs
 * Created: 11/22/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Rsdn.Framework.Data.Mapping
{
	public class MapGenerator
	{
		#region Constructors & Public Properties

		public MapGenerator()
		{
		}

		public MapGenerator(ILGenerator il)
		{
			m_gen = il;
		}

		private ILGenerator m_gen;
		public  ILGenerator   Gen
		{
			get { return m_gen;  }
			set { m_gen = value; }
		}

		#endregion

		#region Definitions

		public LocalBuilder DeclareLocal(Type localType) { return m_gen.DeclareLocal(localType); }
		public Label        DefineLabel ()               { return m_gen.DefineLabel();           }
		public MapGenerator MarkLabel   (Label l)        { m_gen.MarkLabel(l); return this;      }

		#endregion

		#region Exception Block
		public MapGenerator BeginExceptionBlock
		{
			get 
			{
				m_gen.BeginExceptionBlock();
				return this;
			}
		}

		public MapGenerator BeginCatchBlock_Exception
		{
			get 
			{
				m_gen.BeginCatchBlock(typeof(Exception));
				return this;
			}
		}

		public MapGenerator BeginCatchBlock(Type exceptionType)
		{
			m_gen.BeginCatchBlock(exceptionType);
			return this;
		}

		public MapGenerator EndExceptionBlock
		{
			get
			{
				m_gen.EndExceptionBlock();
				return this;
			}
		}
		#endregion

		#region Newobj & Newarr
		
		public MapGenerator newarr(Type type) { m_gen.Emit(OpCodes.Newarr, type); return this; }
		public MapGenerator newobj(ConstructorInfo ci) { m_gen.Emit(OpCodes.Newobj, ci); return this; }
		public MapGenerator newobj(Type type, params Type[] parameters) 
		{
			return newobj(type.GetConstructor(parameters));
		}

		#endregion

		#region Other

		public MapGenerator castclass(Type objectType) { m_gen.Emit(OpCodes.Castclass, objectType); return this; }

		public MapGenerator nop { get { m_gen.Emit(OpCodes.Nop); return this; } }

		public MapGenerator initobj(Type type) { m_gen.Emit(OpCodes.Initobj, type); return this; }

		public void ret() { m_gen.Emit(OpCodes.Ret); }
		public void EndGen() {}

		#endregion

		#region Boxing

		public MapGenerator unbox(Type type) { m_gen.Emit(OpCodes.Unbox, type); return this; }

#if VER2
		public MapGenerator unbox_any(Type type) { m_gen.Emit(OpCodes.Unbox_Any, type); return this; }
#endif
		
		public MapGenerator UnboxIfValueType(Type type)
		{ 
			return type.IsValueType? unbox(type): this;
		}

		public MapGenerator box(Type type) { m_gen.Emit(OpCodes.Box, type); return this; }
		public MapGenerator BoxIfValueType(Type type)
		{ 
			return type.IsValueType? box(type): this;
		}

		#endregion

		#region Is Instance

		public MapGenerator IsDBNull { get { return isinst(typeof(DBNull)); } }
		public MapGenerator IsGuid   { get { return isinst(typeof(Guid)); } }
		public MapGenerator isinst   (Type type) { m_gen.Emit(OpCodes.Isinst, type); return this; }

		#endregion

		#region Branch

		public MapGenerator brtrue_s (Label l) { m_gen.Emit(OpCodes.Brtrue_S,  l); return this; }
		public MapGenerator brfalse_s(Label l) { m_gen.Emit(OpCodes.Brfalse_S, l); return this; }
		public MapGenerator br_s     (Label l) { m_gen.Emit(OpCodes.Br_S, l);      return this; }

		#endregion

		#region Load

		public MapGenerator ldnull  { get { m_gen.Emit(OpCodes.Ldnull);  return this; } }
		public MapGenerator ldloc_0 { get { m_gen.Emit(OpCodes.Ldloc_0); return this; } }
		public MapGenerator ldloc_1 { get { m_gen.Emit(OpCodes.Ldloc_1); return this; } }
		public MapGenerator ldloc_2 { get { m_gen.Emit(OpCodes.Ldloc_2); return this; } }
		public MapGenerator ldloc_3 { get { m_gen.Emit(OpCodes.Ldloc_3); return this; } }

		public MapGenerator ldloc(short n) 
		{
			switch (n)
			{
				case 0: ldloc_0.EndGen(); break;
				case 1: ldloc_1.EndGen(); break;
				case 2: ldloc_2.EndGen(); break;
				case 3: ldloc_3.EndGen(); break;
				default:
					m_gen.Emit(OpCodes.Ldloc, n);
					break;
			}
			
			return this; 
		}

		public MapGenerator ldloc(LocalBuilder builder) 
		{
			m_gen.Emit(OpCodes.Ldloc, builder);
			return this; 
		}

		public MapGenerator ldloca_s(byte n)     { m_gen.Emit(OpCodes.Ldloca_S, n); return this; }
		public MapGenerator ldobj   (Type type)  { m_gen.Emit(OpCodes.Ldobj, type); return this; }
		public MapGenerator ldsfld(FieldInfo fi) { m_gen.Emit(OpCodes.Ldsfld, fi);  return this; }

		public MapGenerator ldarg_0 { get { m_gen.Emit(OpCodes.Ldarg_0); return this; } }
		public MapGenerator ldarg_1 { get { m_gen.Emit(OpCodes.Ldarg_1); return this; } }
		public MapGenerator ldarg_2 { get { m_gen.Emit(OpCodes.Ldarg_2); return this; } }
		public MapGenerator ldarg_3 { get { m_gen.Emit(OpCodes.Ldarg_3); return this; } }
		public MapGenerator ldarg_s(byte n) 
		{
			switch (n)
			{
				case 0: ldarg_0.EndGen(); break;
				case 1: ldarg_1.EndGen(); break;
				case 2: ldarg_2.EndGen(); break;
				case 3: ldarg_3.EndGen(); break;
				default:
					m_gen.Emit(OpCodes.Ldarg_S, n);
					break;
			}
			
			return this; 
		}

		public MapGenerator ldc_i4_0 { get { m_gen.Emit(OpCodes.Ldc_I4_0); return this; } }
		public MapGenerator ldc_i4_1 { get { m_gen.Emit(OpCodes.Ldc_I4_1); return this; } }
		public MapGenerator ldc_i4_2 { get { m_gen.Emit(OpCodes.Ldc_I4_2); return this; } }
		public MapGenerator ldc_i4_3 { get { m_gen.Emit(OpCodes.Ldc_I4_3); return this; } }
		public MapGenerator ldc_i4(int count) 
		{ 
			switch (count)
			{
				case 0: ldc_i4_0.EndGen(); break;
				case 1: ldc_i4_1.EndGen(); break;
				case 2: ldc_i4_2.EndGen(); break;
				case 3: ldc_i4_3.EndGen(); break;
				default:
					if (count >= byte.MinValue && count <= byte.MaxValue)
						ldc_i4_s((byte)count);
					else
						m_gen.Emit(OpCodes.Ldc_I4, count);
					break;
			}

			return this; 
		}

		public MapGenerator ldfld(FieldInfo fi) { m_gen.Emit(OpCodes.Ldfld, fi); return this; }
		public MapGenerator ldfld(Type type, string fieldName) { return ldfld(type.GetField(fieldName)); }

		public MapGenerator ldflda(FieldInfo fi) { m_gen.Emit(OpCodes.Ldflda, fi); return this; }

		public MapGenerator ldfldWhithCheck(FieldInfo fi)
		{
			if (fi.FieldType.IsValueType) ldflda(fi);
			else                          ldfld (fi);

			return this;
		}

		public MapGenerator ldtoken(Type       tp) { m_gen.Emit(OpCodes.Ldtoken, tp); return this; }
		public MapGenerator ldtoken(MethodInfo mi) { m_gen.Emit(OpCodes.Ldtoken, mi); return this; }

		public MapGenerator ldstr  (string str) { m_gen.Emit(OpCodes.Ldstr, str); return this; }

		public MapGenerator ldelem_ref       { get { m_gen.Emit(OpCodes.Ldelem_Ref);      return this; } }
		public MapGenerator ldc_i4_s(byte   value) { m_gen.Emit(OpCodes.Ldc_I4_S, value); return this; }
		public MapGenerator ldc_i8  (long   value) { m_gen.Emit(OpCodes.Ldc_I8,   value); return this; }
		public MapGenerator ldc_i8  (ulong  value) { m_gen.Emit(OpCodes.Ldc_I8,   value); return this; }
		public MapGenerator ldc_r4  (float  value) { m_gen.Emit(OpCodes.Ldc_R4,   value); return this; }
		public MapGenerator ldc_r8  (double value) { m_gen.Emit(OpCodes.Ldc_R8,   value); return this; }
		public MapGenerator ldind_u1         { get { m_gen.Emit(OpCodes.Ldind_U1);        return this; } }
		public MapGenerator ldind_u2         { get { m_gen.Emit(OpCodes.Ldind_U2);        return this; } }
		public MapGenerator ldind_u4         { get { m_gen.Emit(OpCodes.Ldind_U4);        return this; } }
		public MapGenerator ldind_i1         { get { m_gen.Emit(OpCodes.Ldind_I1);        return this; } }
		public MapGenerator ldind_i2         { get { m_gen.Emit(OpCodes.Ldind_I2);        return this; } }
		public MapGenerator ldind_i4         { get { m_gen.Emit(OpCodes.Ldind_I4);        return this; } }
		public MapGenerator ldind_i8         { get { m_gen.Emit(OpCodes.Ldind_I8);        return this; } }
		public MapGenerator ldind_r4         { get { m_gen.Emit(OpCodes.Ldind_R4);        return this; } }
		public MapGenerator ldind_r8         { get { m_gen.Emit(OpCodes.Ldind_R8);        return this; } }

		public bool LoadObject(object o)
		{
			if      (o == null)   ldnull.EndGen();
			else if (o is string) ldstr ((string)o);
			else if (o is byte)   ldc_i4((byte)o);
			else if (o is char)   ldc_i4((char)o);
			else if (o is ushort) ldc_i4((ushort)o);
			else if (o is uint)   ldc_i4((int)o);
			else if (o is ulong)  ldc_i8((ulong)o);
			else if (o is bool)   ldc_i4((bool)o? 1: 0);
			else if (o is short)  ldc_i4((short)o);
			else if (o is int)    ldc_i4((int)o);
			else if (o is long)   ldc_i8((long)o);
			else if (o is float)  ldc_r4((float)o);
			else if (o is double) ldc_r8((double)o);
			else
				return false;

			return true;
		}

		public MapGenerator CastTo(Type type)
		{
			UnboxIfValueType(type);

			if (type.IsEnum)
				type = Enum.GetUnderlyingType(type);

			if      (type == typeof(byte))   ldind_u1.EndGen();
			else if (type == typeof(char))   ldind_u2.EndGen();
			else if (type == typeof(ushort)) ldind_u2.EndGen();
			else if (type == typeof(uint))   ldind_u4.EndGen();
			else if (type == typeof(ulong))  ldind_i8.EndGen();
			else if (type == typeof(bool))   ldind_i1.EndGen();
			else if (type == typeof(sbyte))  ldind_i1.EndGen();
			else if (type == typeof(short))  ldind_i2.EndGen();
			else if (type == typeof(int))    ldind_i4.EndGen();
			else if (type == typeof(long))   ldind_i8.EndGen();
			else if (type == typeof(float))  ldind_r4.EndGen();
			else if (type == typeof(double)) ldind_r8.EndGen();
			else if (type.IsValueType)       ldobj(type);
			else
				castclass(type);

			return this;
		}
		#endregion

		#region St (pop)
		
		public MapGenerator stloc_0 { get { m_gen.Emit(OpCodes.Stloc_0); return this; } }
		public MapGenerator stloc_1 { get { m_gen.Emit(OpCodes.Stloc_1); return this; } }
		public MapGenerator stloc_2 { get { m_gen.Emit(OpCodes.Stloc_2); return this; } }
		public MapGenerator stloc_3 { get { m_gen.Emit(OpCodes.Stloc_3); return this; } }

		public MapGenerator stloc(short n) 
		{
			switch (n)
			{
				case 0: stloc_0.EndGen(); break;
				case 1: stloc_1.EndGen(); break;
				case 2: stloc_2.EndGen(); break;
				case 3: stloc_3.EndGen(); break;
				default:
					m_gen.Emit(OpCodes.Stloc, n);
					break;
			}
			
			return this; 
		}

		public MapGenerator stloc(LocalBuilder builder) 
		{
			m_gen.Emit(OpCodes.Stloc, builder);
			return this; 
		}

		public MapGenerator stfld(FieldInfo fi) { m_gen.Emit(OpCodes.Stfld, fi); return this; }
		public MapGenerator stfld(Type type, string fieldName) { return stfld(type.GetField(fieldName)); }

		public MapGenerator stelem_ref { get { m_gen.Emit(OpCodes.Stelem_Ref); return this; } }

		public MapGenerator stsfld(FieldInfo fi) { m_gen.Emit(OpCodes.Stsfld, fi); return this; }
		
		#endregion

		#region Call Method
		
		public MapGenerator call(MethodInfo methodInfo)
		{
			return call(methodInfo, null);
		}

		public MapGenerator call(MethodInfo methodInfo, params Type[] optionalParameterTypes)
		{
			m_gen.EmitCall(OpCodes.Call, methodInfo, optionalParameterTypes);
			return this;
		}

		public MapGenerator call(Type type, string methodName, BindingFlags bindingAttr, Type[] optionalParameterTypes)
		{
			return call(type.GetMethod(methodName, bindingAttr), optionalParameterTypes);
		}

		public MapGenerator call(Type type, string methodName, BindingFlags bindingAttr)
		{
			return call(type, methodName, bindingAttr, null);
		}

		public MapGenerator call(Type type, string methodName, params Type[] optionalParameterTypes)
		{
			return call(type.GetMethod(methodName, optionalParameterTypes), null);
		}

		public MapGenerator call(ConstructorInfo ci)
		{
			m_gen.Emit(OpCodes.Call, ci);
			return this;
		}
		
		#endregion

		#region Call Virtual Method
		
		public MapGenerator callvirt(MethodInfo methodInfo)
		{
			return callvirt(methodInfo, null);
		}

		public MapGenerator callvirt(MethodInfo methodInfo, params Type[] optionalParameterTypes)
		{
			m_gen.EmitCall(OpCodes.Callvirt, methodInfo, optionalParameterTypes);
			
			return this;
		}

		public MapGenerator callvirt(Type type, string methodName, BindingFlags bindingAttr, Type[] optionalParameterTypes)
		{
			return callvirt(type.GetMethod(methodName, bindingAttr), optionalParameterTypes);
		}

		public MapGenerator callvirt(Type type, string methodName, BindingFlags bindingAttr)
		{
			return callvirt(type, methodName, bindingAttr, null);
		}

		public MapGenerator callvirt(Type type, string methodName, params Type[] optionalParameterTypes)
		{
			return callvirt(type.GetMethod(methodName, optionalParameterTypes), null);
		}

		public MapGenerator callvirtNoGenerics(Type type, string methodName, params Type[] optionalParameterTypes)
		{
#if VER2
			foreach (MethodInfo method in type.GetMethods())
			{
				if (method.IsGenericMethodDefinition == false && method.Name == methodName)
				{
					ParameterInfo[] pis = method.GetParameters();

					if (pis.Length == optionalParameterTypes.Length)
					{
						bool match = true;

						for (int i = 0; match && i < pis.Length; i++)
							if (pis[i].ParameterType != optionalParameterTypes[i])
								match = false;

						if (match)
							return callvirt(method, optionalParameterTypes.Length == 0? null: optionalParameterTypes);
					}
				}
			}

			throw new RsdnMapException(string.Format("Method '{0}' not found.", methodName));
#else

			return callvirt(type.GetMethod(methodName, optionalParameterTypes), null);
#endif
		}

		#endregion

		#region Method Generation

		public new MapGenerator ToString()
		{
			return callvirt(typeof(object), "ToString");
		}

		public MapGenerator ConvertTo(Type convertTo)
		{
			Type convert = typeof(Convert);

			if (convertTo == typeof(string))
			{
				LocalBuilder lb = DeclareLocal(typeof(object));
				Label        l1 = DefineLabel();
				Label        l2 = DefineLabel();

				this
					.stloc(lb)
					.ldloc(lb)
					.brfalse_s(l1)
					.ldloc(lb)
					.callvirt(typeof(object), "ToString")
					.br_s(l2)
					.MarkLabel(l1)
					.ldnull
					.MarkLabel(l2);
				//castclass(typeof(string));
			}
			else if (convertTo == typeof(bool))     call(convert, "ToBoolean",  typeof(object));
			else if (convertTo == typeof(byte))     call(convert, "ToByte",     typeof(object));
			else if (convertTo == typeof(char))     call(convert, "ToChar",     typeof(object));
			else if (convertTo == typeof(DateTime)) call(convert, "ToDateTime", typeof(object));
			else if (convertTo == typeof(decimal))  call(convert, "ToDecimal",  typeof(object));
			else if (convertTo == typeof(double))   call(convert, "ToDouble",   typeof(object));
			else if (convertTo == typeof(Int16))    call(convert, "ToInt16",    typeof(object));
			else if (convertTo == typeof(Int32))    call(convert, "ToInt32",    typeof(object));
			else if (convertTo == typeof(Int64))    call(convert, "ToInt64",    typeof(object));
			else if (convertTo == typeof(sbyte))    call(convert, "ToSByte",    typeof(object));
			else if (convertTo == typeof(float))    call(convert, "ToSingle",   typeof(object));
			else if (convertTo == typeof(UInt16))   call(convert, "ToUInt16",   typeof(object));
			else if (convertTo == typeof(UInt32))   call(convert, "ToUInt32",   typeof(object));
			else if (convertTo == typeof(UInt64))   call(convert, "ToUInt64",   typeof(object));
			else if (convertTo == typeof(Guid))
			{
				Label        l1 = DefineLabel();
				Label        l2 = DefineLabel();
				LocalBuilder loc0 = DeclareLocal(typeof(object));

				this
					.stloc(loc0)
					.ldloc(loc0)
					.IsGuid                               // IL_0007:  isinst     [mscorlib]System.Guid
					.brtrue_s(l1)                         // IL_000c:  brtrue.s   IL_001b

					.ldloc(loc0)
					.ToString()                           // IL_000f:  callvirt   instance string [mscorlib]System.Object::ToString()
					.newobj(typeof(Guid), typeof(string)) // IL_0014:  newobj     instance void [mscorlib]System.Guid::.ctor(string)
					.br_s(l2)                             // IL_0019:  br.s       IL_0026

					.MarkLabel(l1)                        // IL_001b:

					.ldloc(loc0)
					.unbox(typeof(Guid))                  // IL_001c:  unbox      [mscorlib]System.Guid
					.ldobj(typeof(Guid))                  // IL_0021:  ldobj      [mscorlib]System.Guid

					.MarkLabel(l2);                       // IL_0026:
			}

			return this;
		}

		#endregion
	}
}
