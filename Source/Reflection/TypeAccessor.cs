using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

using BLToolkit.Common;
using BLToolkit.ComponentModel;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Reflection
{
	public delegate object NullValueProvider(Type type);
	public delegate bool   IsNullHandler    (object obj);

	[DebuggerDisplay("Type = {Type}, OriginalType = {OriginalType}")]
	public abstract class TypeAccessor : ICollection, ITypeDescriptionProvider
	{
		#region Protected Emit Helpers

		protected MemberInfo GetMember(int memberType, string memberName)
		{
			const BindingFlags allInstaceMembers =
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			MemberInfo mi;

			switch (memberType)
			{
				case 1: mi = Type.GetField   (memberName, allInstaceMembers); break;
				case 2:
					mi =
						Type.        GetProperty(memberName, allInstaceMembers) ??
						OriginalType.GetProperty(memberName, allInstaceMembers);
					break;
				default:
					throw new InvalidOperationException();
			}

			return mi;
		}

		protected void AddMember(MemberAccessor member)
		{
			if (member == null) throw new ArgumentNullException("member");

			_members.Add(member);
			_memberNames.Add(member.MemberInfo.Name, member);
		}

		#endregion

		#region CreateInstance

		[DebuggerStepThrough]
		public virtual object CreateInstance()
		{
			throw new TypeBuilderException(string.Format(
				"The '{0}' type must have public default or init constructor.",
				OriginalType.Name));
		}

		[DebuggerStepThrough]
		public virtual object CreateInstance(InitContext context)
		{
			return CreateInstance();
		}

		[DebuggerStepThrough]
		public object CreateInstanceEx()
		{
			return _objectFactory != null?
				_objectFactory.CreateInstance(this, null): CreateInstance((InitContext)null);
		}

		[DebuggerStepThrough]
		public object CreateInstanceEx(InitContext context)
		{
			return _objectFactory != null? _objectFactory.CreateInstance(this, context): CreateInstance(context);
		}

		#endregion

		#region ObjectFactory

		private IObjectFactory _objectFactory;
		public  IObjectFactory  ObjectFactory
		{
			get { return _objectFactory;  }
			set { _objectFactory = value; }
		}

		#endregion

		#region Copy & AreEqual

		internal static object CopyInternal(object source, object dest, TypeAccessor ta)
		{
			bool                       isDirty = false;
			IMemberwiseEditable sourceEditable = source as IMemberwiseEditable;
			IMemberwiseEditable   destEditable = dest   as IMemberwiseEditable;

			if (sourceEditable != null && destEditable != null)
			{
				foreach (MemberAccessor ma in ta)
				{
					ma.CloneValue(source, dest);
					if (sourceEditable.IsDirtyMember(null, ma.MemberInfo.Name, ref isDirty) && !isDirty)
						destEditable.AcceptMemberChanges(null, ma.MemberInfo.Name);
				}
			}
			else
			{
				foreach (MemberAccessor ma in ta)
					ma.CloneValue(source, dest);
			}

			return dest;
		}

		public static object Copy(object source, object dest)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (dest   == null) throw new ArgumentNullException("dest");

			TypeAccessor ta;
			Type         sType = source.GetType();
			Type         dType = dest.  GetType();

			if      (TypeHelper.IsSameOrParent(sType, dType)) ta = GetAccessor(sType);
			else if (TypeHelper.IsSameOrParent(dType, sType)) ta = GetAccessor(dType);
			else
				throw new ArgumentException();

			return CopyInternal(source, dest, ta);
		}

		public static object Copy(object source)
		{
			if (source == null) throw new ArgumentNullException("source");

			TypeAccessor ta = GetAccessor(source.GetType());

			return CopyInternal(source, ta.CreateInstanceEx(), ta);
		}

		public static bool AreEqual(object obj1, object obj2)
		{
			if (ReferenceEquals(obj1, obj2))
				return true;

			if (obj1 == null || obj2 == null)
				return false;

			TypeAccessor ta;
			Type         sType = obj1.GetType();
			Type         dType = obj2.GetType();

			if      (TypeHelper.IsSameOrParent(sType, dType)) ta = GetAccessor(sType);
			else if (TypeHelper.IsSameOrParent(dType, sType)) ta = GetAccessor(dType);
			else
				return false;

			foreach (MemberAccessor ma in ta)
				if ((!Equals(ma.GetValue(obj1), ma.GetValue(obj2))))
					return false;

			return true;
		}

		public static int GetHashCode(object obj)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			int    hash = 0;
			object value;

			foreach (MemberAccessor ma in GetAccessor(obj.GetType()))
			{
				value = ma.GetValue(obj);
				hash = ((hash << 5) + hash) ^ (value == null ? 0 : value.GetHashCode());
			}

			return hash;
		}

		#endregion

		#region Abstract Members

		public abstract Type Type         { get; }
		public abstract Type OriginalType { get; }

		#endregion

		#region Items

		private readonly ArrayList _members     = new ArrayList();
		private readonly Hashtable _memberNames = new Hashtable();

		public MemberAccessor this[string memberName]
		{
			get { return (MemberAccessor)_memberNames[memberName]; }
		}

		public MemberAccessor this[int index]
		{
			get { return (MemberAccessor)_members[index]; }
		}

		public MemberAccessor this[NameOrIndexParameter nameOrIndex]
		{
			get
			{
				return (MemberAccessor)
					(nameOrIndex.ByName ? _memberNames[nameOrIndex.Name] : _members[nameOrIndex.Index]);
			}
		}
		
		#endregion

		#region Static Members

		private static bool _loadTypes;
		public  static bool  LoadTypes
		{
			get { return _loadTypes;  }
			set { _loadTypes = value; }
		}

		private static readonly Hashtable _accessors  = new Hashtable(10);
		private static readonly Hashtable _assemblies = new Hashtable(10);

		public static TypeAccessor GetAccessor(Type originalType)
		{
			if (originalType == null) throw new ArgumentNullException("originalType");

			TypeAccessor accessor = (TypeAccessor)_accessors[originalType];

			if (accessor == null)
			{
				lock (_accessors.SyncRoot)
				{
					accessor = (TypeAccessor)_accessors[originalType];

					if (accessor == null)
					{
						if (IsAssociatedType(originalType))
							return (TypeAccessor)_accessors[originalType];

						Type instanceType = IsClassBulderNeeded(originalType)? null: originalType;
						Type accessorType = null;

						if (_loadTypes)
						{
							Assembly originalAssembly = originalType.Assembly;
							Assembly extensionAssembly;

							if (_assemblies.Contains(originalAssembly))
								extensionAssembly = (Assembly)_assemblies[originalAssembly];
							else
							{
								extensionAssembly = LoadExtensionAssembly(originalAssembly);
								_assemblies.Add(originalAssembly, extensionAssembly);
							}

							if (extensionAssembly != null)
							{
								if (instanceType == null)
									instanceType = extensionAssembly.GetType(
										AbstractClassBuilder.GetTypeName(originalType));

								if (instanceType != null)
									accessorType = extensionAssembly.GetType(
										TypeAccessorBuilder.GetTypeAccessorClassName(instanceType));
							}
						}

						if (accessorType == null)
						{
							if (instanceType == null)
								instanceType = TypeFactory.GetType(originalType, new AbstractClassBuilder());

							accessorType = TypeFactory.GetType(originalType, new TypeAccessorBuilder(instanceType, originalType));
						}

						accessor = (TypeAccessor)Activator.CreateInstance(accessorType);

						_accessors[originalType] = accessor;

						if (originalType != instanceType)
							_accessors[instanceType] = accessor;
					}
				}
			}

			return accessor;
		}

		private static bool IsClassBulderNeeded(Type type)
		{
			if (type.IsAbstract && !type.IsSealed)
			{
				if (!type.IsInterface)
				{
					if (TypeHelper.GetDefaultConstructor(type) != null)
						return true;

					if (TypeHelper.GetConstructor(type, typeof(InitContext)) != null)
						return true;
				}
				else
				{
					object[] attrs = TypeHelper.GetAttributes(type, typeof(AutoImplementInterfaceAttribute));

					if (attrs != null && attrs.Length > 0)
						return true;
				}
			}

			return false;
		}

		internal static bool IsInstanceBuildable(Type type)
		{
			if (!type.IsInterface)
				return true;

			lock (_accessors.SyncRoot)
			{
				if (_accessors[type] != null)
					return true;

				if (IsAssociatedType(type))
					return true;
			}

			object[] attrs = TypeHelper.GetAttributes(type, typeof(AutoImplementInterfaceAttribute));

			if (attrs != null && attrs.Length > 0)
				return true;

			return false;
		}

		private static bool IsAssociatedType(Type type)
		{
			if (AssociatedTypeHandler != null)
			{
				Type child = AssociatedTypeHandler(type);

				if (child != null)
				{
					AssociateType(type, child);
					return true;
				}
			}

			return false;
		}

		private static Assembly LoadExtensionAssembly(Assembly originalAssembly)
		{
			if (originalAssembly is System.Runtime.InteropServices._AssemblyBuilder)
			{
				// This is a generated assembly. Even if it has a valid Location,
				// there is definitelly no extension assembly at this path.
				//
				return null;
			}

			try
			{
				string  originalAssemblyLocation = new Uri(originalAssembly.EscapedCodeBase).AbsolutePath;
				string extensionAssemblyLocation = Path.ChangeExtension(
					originalAssemblyLocation, "BLToolkitExtension.dll");

				if (File.GetLastWriteTime(originalAssemblyLocation) <= File.GetLastWriteTime(extensionAssemblyLocation))
					return Assembly.LoadFrom(extensionAssemblyLocation);

				Debug.WriteLineIf(File.Exists(extensionAssemblyLocation),
					string.Format("Extension assembly '{0}' is out of date. Please rebuild.",
						extensionAssemblyLocation), typeof(TypeAccessor).FullName);

				// Some good man may load this assembly already. Like IIS does it.
				//
				AssemblyName extensionAssemblyName = originalAssembly.GetName(true);
				extensionAssemblyName.Name += ".BLToolkitExtension";
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					// Note that assembly version and strong name are compared too.
					//
					if (AssemblyName.ReferenceMatchesDefinition(assembly.GetName(false), extensionAssemblyName))
						return assembly;
				}
			}
			catch (Exception ex)
			{
				// Extension exist, but can't be loaded for some reason.
				// Switch back to code generation
				//
				Debug.WriteLine(ex, typeof(TypeAccessor).FullName);
			}

			return null;
		}

		public static object CreateInstance(Type type)
		{
			return GetAccessor(type).CreateInstance();
		}

		public static object CreateInstance(Type type, InitContext context)
		{
			return GetAccessor(type).CreateInstance(context);
		}

		public static object CreateInstanceEx(Type type)
		{
			return GetAccessor(type).CreateInstanceEx();
		}

		public static object CreateInstanceEx(Type type, InitContext context)
		{
			return GetAccessor(type).CreateInstance(context);
		}

		public static T CreateInstance<T>()
		{
			return TypeAccessor<T>.CreateInstance();
		}

		public static T CreateInstance<T>(InitContext context)
		{
			return TypeAccessor<T>.CreateInstance(context);
		}

		public static T CreateInstanceEx<T>()
		{
			return TypeAccessor<T>.CreateInstanceEx();
		}

		public static T CreateInstanceEx<T>(InitContext context)
		{
			return TypeAccessor<T>.CreateInstance(context);
		}

		public static TypeAccessor AssociateType(Type parent, Type child)
		{
			if (!TypeHelper.IsSameOrParent(parent, child))
				throw new ArgumentException(
					string.Format("'{0}' must be a base type of '{1}'", parent, child),
					"inheritedType");

			TypeAccessor accessor = GetAccessor(child);

			accessor = (TypeAccessor)Activator.CreateInstance(accessor.GetType());

			lock (_accessors.SyncRoot)
				_accessors[parent] = accessor;

			return accessor;
		}

		public delegate Type GetAssociatedType(Type parent);
		public static event GetAssociatedType AssociatedTypeHandler;

		#endregion

		#region GetNullValue

		private static NullValueProvider _getNullValue = GetNullInternal;
		public  static NullValueProvider  GetNullValue
		{
			get { return _getNullValue ?? (_getNullValue = GetNullInternal);}
			set { _getNullValue = value; }
		}

		private static object GetNullInternal(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			if (type.IsValueType)
			{
				if (type.IsEnum)
					return GetEnumNullValue(type);

				if (type.IsPrimitive)
				{
					if (type == typeof(Int32))       return 0;
					if (type == typeof(Double))      return (Double)0;
					if (type == typeof(Int16))       return (Int16)0;
					if (type == typeof(Boolean))     return false;
					if (type == typeof(SByte))       return (SByte)0;
					if (type == typeof(Int64))       return (Int64)0;
					if (type == typeof(Byte))        return (Byte)0;
					if (type == typeof(UInt16))      return (UInt16)0;
					if (type == typeof(UInt32))      return (UInt32)0;
					if (type == typeof(UInt64))      return (UInt64)0;
					if (type == typeof(Single))      return (Single)0;
					if (type == typeof(Char))        return new char();
				}
				else
				{
					if (type == typeof(DateTime))    return DateTime.MinValue;
#if FW3
					if (type == typeof(DateTimeOffset))    return DateTimeOffset.MinValue;
#endif
					if (type == typeof(Decimal))     return 0m;
					if (type == typeof(Guid))        return Guid.Empty;

					if (type == typeof(SqlInt32))    return SqlInt32.   Null;
					if (type == typeof(SqlString))   return SqlString.  Null;
					if (type == typeof(SqlBoolean))  return SqlBoolean. Null;
					if (type == typeof(SqlByte))     return SqlByte.    Null;
					if (type == typeof(SqlDateTime)) return SqlDateTime.Null;
					if (type == typeof(SqlDecimal))  return SqlDecimal. Null;
					if (type == typeof(SqlDouble))   return SqlDouble.  Null;
					if (type == typeof(SqlGuid))     return SqlGuid.    Null;
					if (type == typeof(SqlInt16))    return SqlInt16.   Null;
					if (type == typeof(SqlInt64))    return SqlInt64.   Null;
					if (type == typeof(SqlMoney))    return SqlMoney.   Null;
					if (type == typeof(SqlSingle))   return SqlSingle.  Null;
					if (type == typeof(SqlBinary))   return SqlBinary.  Null;
				}
			}
			else
			{
				if (type == typeof(String)) return string.Empty;
				if (type == typeof(DBNull)) return DBNull.Value;
				if (type == typeof(Stream)) return Stream.Null;
				if (type == typeof(SqlXml)) return SqlXml.Null;
			}

			return null;
		}

		const FieldAttributes EnumField = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;

		private static readonly Hashtable _nullValues = new Hashtable();

		private static object GetEnumNullValue(Type type)
		{
			object nullValue = _nullValues[type];

			if (nullValue != null || _nullValues.Contains(type))
				return nullValue;

			FieldInfo[] fields = type.GetFields();

			foreach (FieldInfo fi in fields)
			{
				if ((fi.Attributes & EnumField) == EnumField)
				{
					Attribute[] attrs = Attribute.GetCustomAttributes(fi, typeof(NullValueAttribute));

					if (attrs.Length > 0)
					{
						nullValue = Enum.Parse(type, fi.Name);
						break;
					}
				}
			}

			_nullValues[type] = nullValue;

			return nullValue;
		}

		private static IsNullHandler _isNull = IsNullInternal;
		public  static IsNullHandler  IsNull
		{
			get { return _isNull ?? (_isNull = IsNullInternal); }
			set { _isNull = value; }
		}

		private static bool IsNullInternal(object value)
		{
			if (value == null)
				return true;

			object nullValue = GetNullValue(value.GetType());

			if (nullValue == null)
				return false;

			return value.Equals(nullValue);
		}

		#endregion

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			_members.CopyTo(array, index);
		}

		public int Count
		{
			get { return _members.Count; }
		}

		public bool IsSynchronized
		{
			get { return _members.IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return _members.SyncRoot; }
		}

		public int IndexOf(MemberAccessor ma)
		{
			return _members.IndexOf(ma);
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _members.GetEnumerator();
		}

		#endregion

		#region Write Object Info

		public static void WriteDebug(object o)
		{
#if DEBUG
			Write(o, DebugWriteLine);
#endif
		}

		private static void DebugWriteLine(string text)
		{
			Debug.WriteLine(text);
		}

		public static void WriteConsole(object o)
		{
			Write(o, Console.WriteLine);
		}

		[SuppressMessage("Microsoft.Performance", "CA1818:DoNotConcatenateStringsInsideLoops")]
		private static string MapTypeName(Type type)
		{
			if (type.IsGenericType)
			{
				if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
					return string.Format("{0}?", MapTypeName(type.GetGenericArguments()[0]));

				string name = type.Name;

				int idx = name.IndexOf('`');

				if (idx >= 0)
					name = name.Substring(0, idx);

				name += "<";

				foreach (Type t in type.GetGenericArguments())
					name += MapTypeName(t) + ',';

				if (name[name.Length - 1] == ',')
					name = name.Substring(0, name.Length - 1);

				name += ">";

				return name;
			}

			if (type.IsPrimitive ||
				type == typeof(string) ||
				type == typeof(object) ||
				type == typeof(decimal))
			{
				if (type == typeof(int))    return "int";
				if (type == typeof(bool))   return "bool";
				if (type == typeof(short))  return "short";
				if (type == typeof(long))   return "long";
				if (type == typeof(ushort)) return "ushort";
				if (type == typeof(uint))   return "uint";
				if (type == typeof(ulong))  return "ulong";
				if (type == typeof(float))  return "float";

				return type.Name.ToLower();
			}

			return type.Name;
		}

		public delegate void WriteLine(string text);

		[SuppressMessage("Microsoft.Usage", "CA2241:ProvideCorrectArgumentsToFormattingMethods")]
		public static void Write(object o, WriteLine writeLine)
		{
			if (o == null)
			{
				writeLine("*** (null) ***");
				return;
			}

			TypeAccessor   ta      = GetAccessor(o.GetType());
			MemberAccessor ma;
			int            nameLen = 0;
			int            typeLen = 0;

			foreach (DictionaryEntry de in ta._memberNames)
			{
				if (nameLen < de.Key.ToString().Length)
					nameLen = de.Key.ToString().Length;

				ma = (MemberAccessor)de.Value;

				if (typeLen < MapTypeName(ma.Type).Length)
					typeLen = MapTypeName(ma.Type).Length;
			}

			string text = "*** " + o.GetType().FullName + ": ***";

			writeLine(text);

			string format = string.Format("{{0,-{0}}} {{1,-{1}}} : {{2}}", typeLen, nameLen);

			foreach (DictionaryEntry de in ta._memberNames)
			{
				ma = (MemberAccessor)de.Value;

				object value = ma.GetValue(o);

				if (value == null)
					value = "(null)";
				else if (value is ICollection)
					value = string.Format("(Count = {0})", ((ICollection)value).Count);

				text = string.Format(format, MapTypeName(ma.Type), de.Key, value);

				writeLine(text);
			}

			writeLine("***");
		}

		#endregion

		#region CustomTypeDescriptor

		private static readonly Hashtable _descriptors = new Hashtable();

		public static ICustomTypeDescriptor GetCustomTypeDescriptor(Type type)
		{
			ICustomTypeDescriptor descriptor = (ICustomTypeDescriptor)_descriptors[type];

			if (descriptor == null)
				descriptor = new CustomTypeDescriptorImpl(type);

			return descriptor;
		}

		private ICustomTypeDescriptor _customTypeDescriptor;
		public  ICustomTypeDescriptor  CustomTypeDescriptor
		{
			get
			{
				if (_customTypeDescriptor == null)
					_customTypeDescriptor = GetCustomTypeDescriptor(OriginalType);

				return _customTypeDescriptor;
			}
		}

		#endregion

		#region Property Descriptors

		private PropertyDescriptorCollection _propertyDescriptors;
		public  PropertyDescriptorCollection  PropertyDescriptors
		{
			get
			{
				if (_propertyDescriptors == null)
				{
					if (TypeHelper.IsSameOrParent(typeof(ICustomTypeDescriptor), OriginalType))
					{
						ICustomTypeDescriptor descriptor = CreateInstance() as ICustomTypeDescriptor;

						if (descriptor != null)
							_propertyDescriptors = descriptor.GetProperties();
					}

					if (_propertyDescriptors == null)
						_propertyDescriptors = CreatePropertyDescriptors();
				}

				return _propertyDescriptors;
			}
		}

		public PropertyDescriptorCollection CreatePropertyDescriptors()
		{
			Debug.WriteLineIf(BLToolkit.Data.DbManager.TS.TraceInfo,
				OriginalType.FullName, "CreatePropertyDescriptors");

			PropertyDescriptor[] pd = new PropertyDescriptor[Count];

			int i = 0;
			foreach (MemberAccessor ma in _members)
				pd[i++] = ma.PropertyDescriptor;

			return new PropertyDescriptorCollection(pd);
		}

		public PropertyDescriptorCollection CreateExtendedPropertyDescriptors(
			Type          objectViewType,
			IsNullHandler isNull)
		{
			// This is definitely wrong.
			//
			//if (isNull == null)
			//	isNull = _isNull;

			PropertyDescriptorCollection pdc;

			pdc = CreatePropertyDescriptors();

			if (objectViewType != null)
			{
				TypeAccessor             viewAccessor = GetAccessor(objectViewType);
				IObjectView              objectView   = (IObjectView)viewAccessor.CreateInstanceEx();
				List<PropertyDescriptor> list         = new List<PropertyDescriptor>();

				PropertyDescriptorCollection viewpdc = viewAccessor.PropertyDescriptors;

				foreach (PropertyDescriptor pd in viewpdc)
					list.Add(new ObjectViewPropertyDescriptor(pd, objectView));

				foreach (PropertyDescriptor pd in pdc)
					if (viewpdc.Find(pd.Name, false) == null)
						list.Add(pd);

				pdc = new PropertyDescriptorCollection(list.ToArray());
			}

			pdc = pdc.Sort(new PropertyDescriptorComparer());

			pdc = GetExtendedProperties(pdc, OriginalType, String.Empty, Type.EmptyTypes, new PropertyDescriptor[0], isNull);

			return pdc;
		}

		private static PropertyDescriptorCollection GetExtendedProperties(
			PropertyDescriptorCollection pdc,
			Type                         itemType,
			string                       propertyPrefix,
			Type[]                       parentTypes,
			PropertyDescriptor[]         parentAccessors,
			IsNullHandler                isNull)
		{
			ArrayList list      = new ArrayList(pdc.Count);
			ArrayList objects   = new ArrayList();
			bool      isDataRow = itemType.IsSubclassOf(typeof(DataRow));

			foreach (PropertyDescriptor p in pdc)
			{
				Type propertyType = p.PropertyType;

				if (p.Attributes.Matches(BindableAttribute.No) ||
					//propertyType == typeof(Type)               ||
					isDataRow && p.Name == "ItemArray")
					continue;

				bool isList           = false;
				bool explicitlyBound  = p.Attributes.Contains(BindableAttribute.Yes);
				PropertyDescriptor pd = p;

				if (propertyType.GetInterface("IList") != null)
				{
					//if (!explicitlyBound)
					//	continue;

					isList = true;
					pd     = new ListPropertyDescriptor(pd);
				}

				if (!isList                   &&
					!propertyType.IsValueType &&
					!propertyType.IsArray     &&
					(!propertyType.FullName.StartsWith("System.") || explicitlyBound
					|| propertyType.IsGenericType) &&
					 propertyType != typeof(Type)   &&
					 propertyType != typeof(string) &&
					 propertyType != typeof(object) &&
					Array.IndexOf(parentTypes, propertyType) == -1)
				{
					Type[] childParentTypes = new Type[parentTypes.Length + 1];

					parentTypes.CopyTo(childParentTypes, 0);
					childParentTypes[parentTypes.Length] = itemType;

					PropertyDescriptor[] childParentAccessors = new PropertyDescriptor[parentAccessors.Length + 1];

					parentAccessors.CopyTo(childParentAccessors, 0);
					childParentAccessors[parentAccessors.Length] = pd;

					PropertyDescriptorCollection pdch = GetAccessor(propertyType).PropertyDescriptors;

					pdch = pdch.Sort(new PropertyDescriptorComparer());
					pdch = GetExtendedProperties(
						pdch,
						propertyType,
						propertyPrefix + pd.Name + "+",
						childParentTypes,
						childParentAccessors,
						isNull);

					objects.AddRange(pdch);
				}
				else
				{
					if (propertyPrefix.Length != 0 || isNull != null)
						pd = new StandardPropertyDescriptor(pd, propertyPrefix, parentAccessors, isNull);

					list.Add(pd);
				}
			}

			list.AddRange(objects);

			return new PropertyDescriptorCollection(
				(PropertyDescriptor[])list.ToArray(typeof(PropertyDescriptor)));
		}

		#region PropertyDescriptorComparer

		class PropertyDescriptorComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				return String.Compare(((PropertyDescriptor)x).Name, ((PropertyDescriptor)y).Name);
			}
		}

		#endregion

		#region ListPropertyDescriptor

		class ListPropertyDescriptor : PropertyDescriptorWrapper
		{
			public ListPropertyDescriptor(PropertyDescriptor descriptor)
				: base(descriptor)
			{
			}

			public override object GetValue(object component)
			{
				object value = base.GetValue(component);

				if (value == null)
					return value;

				if (value is IBindingList && value is ITypedList)
					return value;

				return EditableArrayList.Adapter((IList)value);
			}
		}

		#endregion

		#region StandardPropertyDescriptor

		class StandardPropertyDescriptor : PropertyDescriptorWrapper
		{
			protected readonly PropertyDescriptor   _descriptor = null;
			protected readonly IsNullHandler        _isNull;

			protected readonly string               _prefixedName;
			protected readonly PropertyDescriptor[] _chainAccessors;

			public StandardPropertyDescriptor(
				PropertyDescriptor   pd,
				string               namePrefix,
				PropertyDescriptor[] chainAccessors,
				IsNullHandler        isNull)
				: base(pd)
			{
				_descriptor     = pd;
				_isNull         = isNull;
				_prefixedName   = namePrefix + pd.Name;
				_chainAccessors = chainAccessors;
			}

			protected object GetNestedComponent(object component)
			{
				for (int i = 0;
					i < _chainAccessors.Length && component != null && !(component is DBNull);
					i++)
				{
					component = _chainAccessors[i].GetValue(component);
				}

				return component;
			}

			public override void SetValue(object component, object value)
			{
				component = GetNestedComponent(component);

				if (component != null && !(component is DBNull))
					_descriptor.SetValue(component, value);
			}

			public override object GetValue(object component)
			{
				component = GetNestedComponent(component);

				return CheckNull(
					component != null && !(component is DBNull)? _descriptor.GetValue(component): null);
			}

			public override string Name
			{
				get { return _prefixedName; }
			}

			protected object CheckNull(object value)
			{
				if (_isNull != null && _isNull(value))
				{
					switch (Configuration.CheckNullReturnIfNull)
					{
						case Configuration.NullEquivalent.DBNull:
							return DBNull.Value;
						case Configuration.NullEquivalent.Null:
							return null;
						case Configuration.NullEquivalent.Value:
							return value;
					}

					return DBNull.Value;
				}

				return value;
			}
		}

		#endregion

		#region objectViewPropertyDescriptor

		class ObjectViewPropertyDescriptor : PropertyDescriptorWrapper
		{
			public ObjectViewPropertyDescriptor(PropertyDescriptor pd, IObjectView objectView)
				: base(pd)
			{
				_objectView = objectView;
			}

			private readonly IObjectView _objectView;

			public override object GetValue(object component)
			{
				_objectView.Object = component;

				return base.GetValue(_objectView);
			}

			public override void SetValue(object component, object value)
			{
				_objectView.Object = component;

				base.SetValue(_objectView, value);
			}
		}

		#endregion

		#endregion

		#region ITypeDescriptionProvider Members

		string ITypeDescriptionProvider.ClassName
		{
			get { return OriginalType.Name; }
		}

		string ITypeDescriptionProvider.ComponentName
		{
			get { return OriginalType.Name; }
		}

		EventDescriptor ITypeDescriptionProvider.GetEvent(string name)
		{
			return new CustomEventDescriptor(OriginalType.GetEvent(name));
		}

		PropertyDescriptor ITypeDescriptionProvider.GetProperty(string name)
		{
			MemberAccessor ma = this[name];
			return ma != null ? ma.PropertyDescriptor : null;
		}

		AttributeCollection ITypeDescriptionProvider.GetAttributes()
		{
			return new AttributeCollection((Attribute[])new TypeHelper(OriginalType).GetAttributes());
		}

		EventDescriptorCollection ITypeDescriptionProvider.GetEvents()
		{
			EventInfo[]       ei = OriginalType.GetEvents();
			EventDescriptor[] ed = new EventDescriptor[ei.Length];

			for (int i = 0; i < ei.Length; i++)
				ed[i] = new CustomEventDescriptor(ei[i]);

			return new EventDescriptorCollection(ed);
		}

		PropertyDescriptorCollection ITypeDescriptionProvider.GetProperties()
		{
			return CreatePropertyDescriptors();
		}

		#region CustomEventDescriptor

		class CustomEventDescriptor : EventDescriptor
		{
			public CustomEventDescriptor(EventInfo eventInfo)
				: base(eventInfo.Name, null)
			{
				_eventInfo = eventInfo;
			}

			private readonly EventInfo _eventInfo;

			public override void AddEventHandler(object component, Delegate value)
			{
				_eventInfo.AddEventHandler(component, value);
			}

			public override void RemoveEventHandler(object component, Delegate value)
			{
				_eventInfo.RemoveEventHandler(component, value);
			}

			public override Type ComponentType { get { return _eventInfo.DeclaringType;    } }
			public override Type EventType     { get { return _eventInfo.EventHandlerType; } }
			public override bool IsMulticast   { get { return _eventInfo.IsMulticast;      } }
		}

		#endregion

		#endregion
	}
}
