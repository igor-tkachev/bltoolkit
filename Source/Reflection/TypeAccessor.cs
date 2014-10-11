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
#if !SILVERLIGHT && !DATA
using BLToolkit.ComponentModel;
using BLToolkit.EditableObjects;
#endif
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

using JNotNull = JetBrains.Annotations.NotNullAttribute;

namespace BLToolkit.Reflection
{
	public delegate object NullValueProvider(Type type);
	public delegate bool   IsNullHandler    (object obj);

	[DebuggerDisplay("Type = {Type}, OriginalType = {OriginalType}")]
	public abstract class TypeAccessor : ICollection<MemberAccessor>
#if !SILVERLIGHT && !DATA
		, ITypeDescriptionProvider
#endif
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
#if !SILVERLIGHT && !DATA
			var isDirty        = false;
			var sourceEditable = source as IMemberwiseEditable;
			var destEditable   = dest   as IMemberwiseEditable;

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
#endif
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

			var sType = source.GetType();
			var dType = dest.  GetType();

			if      (TypeHelper.IsSameOrParent(sType, dType)) ta = GetAccessor(sType);
			else if (TypeHelper.IsSameOrParent(dType, sType)) ta = GetAccessor(dType);
			else
				throw new ArgumentException();

			return CopyInternal(source, dest, ta);
		}

		public static object Copy(object source)
		{
			if (source == null) throw new ArgumentNullException("source");

			var ta = GetAccessor(source.GetType());

			return CopyInternal(source, ta.CreateInstanceEx(), ta);
		}

		public static bool AreEqual(object obj1, object obj2)
		{
			if (ReferenceEquals(obj1, obj2))
				return true;

			if (obj1 == null || obj2 == null)
				return false;

			TypeAccessor ta;

			var sType = obj1.GetType();
			var dType = obj2.GetType();

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

			var    hash = 0;
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

		private readonly List<MemberAccessor>              _members     = new List<MemberAccessor>();
		private readonly Dictionary<string,MemberAccessor> _memberNames = new Dictionary<string,MemberAccessor>();

		public MemberAccessor this[string memberName]
		{
			get
			{
				MemberAccessor ma;
				return _memberNames.TryGetValue(memberName, out ma) ? ma : null;
			}
		}

		public MemberAccessor this[int index]
		{
			get { return _members[index]; }
		}

		public MemberAccessor this[NameOrIndexParameter nameOrIndex]
		{
			get
			{
				return nameOrIndex.ByName ? _memberNames[nameOrIndex.Name] : _members[nameOrIndex.Index];
			}
		}

		#endregion

		#region Static Members

		[Obsolete("Use TypeFactory.LoadTypes instead")]
		public static bool LoadTypes
		{
			get { return TypeFactory.LoadTypes;  }
			set { TypeFactory.LoadTypes = value; }
		}

		private static readonly Dictionary<Type,TypeAccessor> _accessors = new Dictionary<Type,TypeAccessor>(10);

		public static TypeAccessor GetAccessor(Type originalType)
		{
			if (originalType == null) throw new ArgumentNullException("originalType");

			lock (_accessors)
			{
				TypeAccessor accessor;

				if (_accessors.TryGetValue(originalType, out accessor))
					return accessor;

				if (IsAssociatedType(originalType))
					return _accessors[originalType];

				var instanceType = (IsClassBulderNeeded(originalType) ? null : originalType) ?? TypeFactory.GetType(originalType);

				var accessorType = TypeFactory.GetType(originalType, originalType, new TypeAccessorBuilder(instanceType, originalType));

				accessor = (TypeAccessor)Activator.CreateInstance(accessorType);

				_accessors.Add(originalType, accessor);

				if (originalType != instanceType)
					_accessors.Add(instanceType, accessor);

				return accessor;
			}
		}

		public static TypeAccessor GetAccessor([JNotNull] object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");
			return GetAccessor(obj.GetType());
		}

		public static TypeAccessor GetAccessor<T>()
		{
			return TypeAccessor<T>.Instance;
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
					var attrs = TypeHelper.GetAttributes(type, typeof(AutoImplementInterfaceAttribute));

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

			lock (_accessors)
			{
				if (_accessors.ContainsKey(type))
					return true;

				if (IsAssociatedType(type))
					return true;
			}

			var attrs = TypeHelper.GetAttributes(type, typeof(AutoImplementInterfaceAttribute));

			return attrs != null && attrs.Length > 0;
		}

		private static bool IsAssociatedType(Type type)
		{
			if (AssociatedTypeHandler != null)
			{
				var child = AssociatedTypeHandler(type);

				if (child != null)
				{
					AssociateType(type, child);
					return true;
				}
			}

			return false;
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
					"child");

			var accessor = GetAccessor(child);

			accessor = (TypeAccessor)Activator.CreateInstance(accessor.GetType());

			lock (_accessors)
				_accessors.Add(parent, accessor);

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
					if (type == typeof(Int32))          return Common.Configuration.NullableValues.Int32;
					if (type == typeof(Double))         return Common.Configuration.NullableValues.Double;
					if (type == typeof(Int16))          return Common.Configuration.NullableValues.Int16;
					if (type == typeof(Boolean))        return Common.Configuration.NullableValues.Boolean;
					if (type == typeof(SByte))          return Common.Configuration.NullableValues.SByte;
					if (type == typeof(Int64))          return Common.Configuration.NullableValues.Int64;
					if (type == typeof(Byte))           return Common.Configuration.NullableValues.Byte;
					if (type == typeof(UInt16))         return Common.Configuration.NullableValues.UInt16;
					if (type == typeof(UInt32))         return Common.Configuration.NullableValues.UInt32;
					if (type == typeof(UInt64))         return Common.Configuration.NullableValues.UInt64;
					if (type == typeof(Single))         return Common.Configuration.NullableValues.Single;
					if (type == typeof(Char))           return Common.Configuration.NullableValues.Char;
				}
				else
				{
					if (type == typeof(DateTime))       return Common.Configuration.NullableValues.DateTime;
					if (type == typeof(DateTimeOffset)) return Common.Configuration.NullableValues.DateTimeOffset;
					if (type == typeof(Decimal))        return Common.Configuration.NullableValues.Decimal;
					if (type == typeof(Guid))           return Common.Configuration.NullableValues.Guid;

#if !SILVERLIGHT

					if (type == typeof(SqlInt32))       return SqlInt32.   Null;
					if (type == typeof(SqlString))      return SqlString.  Null;
					if (type == typeof(SqlBoolean))     return SqlBoolean. Null;
					if (type == typeof(SqlByte))        return SqlByte.    Null;
					if (type == typeof(SqlDateTime))    return SqlDateTime.Null;
					if (type == typeof(SqlDecimal))     return SqlDecimal. Null;
					if (type == typeof(SqlDouble))      return SqlDouble.  Null;
					if (type == typeof(SqlGuid))        return SqlGuid.    Null;
					if (type == typeof(SqlInt16))       return SqlInt16.   Null;
					if (type == typeof(SqlInt64))       return SqlInt64.   Null;
					if (type == typeof(SqlMoney))       return SqlMoney.   Null;
					if (type == typeof(SqlSingle))      return SqlSingle.  Null;
					if (type == typeof(SqlBinary))      return SqlBinary.  Null;

#endif
				}
			}
			else
			{
				if (type == typeof(String)) return Common.Configuration.NullableValues.String;
				if (type == typeof(DBNull)) return DBNull.Value;
				if (type == typeof(Stream)) return Stream.Null;
#if !SILVERLIGHT
				if (type == typeof(SqlXml)) return SqlXml.Null;
#endif
			}

			return null;
		}

		const FieldAttributes EnumField = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;

		static readonly Dictionary<Type,object> _nullValues = new Dictionary<Type,object>();

		static object GetEnumNullValue(Type type)
		{
			object nullValue;

			lock (_nullValues)
				if (_nullValues.TryGetValue(type, out nullValue))
					return nullValue;

			var fields = type.GetFields();

			foreach (var fi in fields)
			{
				if ((fi.Attributes & EnumField) == EnumField)
				{
					var attrs = Attribute.GetCustomAttributes(fi, typeof(NullValueAttribute));

					if (attrs.Length > 0)
					{
						nullValue = Enum.Parse(type, fi.Name, false);
						break;
					}
				}
			}

			lock (_nullValues)
				if (!_nullValues.ContainsKey(type))
					_nullValues.Add(type, nullValue);

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

			var nullValue = GetNullValue(value.GetType());

			return nullValue != null && value.Equals(nullValue);
		}

		#endregion

		#region ICollection Members

		void ICollection<MemberAccessor>.Add(MemberAccessor item)
		{
			_members.Add(item);
		}

		void ICollection<MemberAccessor>.Clear()
		{
			_members.Clear();
		}

		bool ICollection<MemberAccessor>.Contains(MemberAccessor item)
		{
			return _members.Contains(item);
		}

		void ICollection<MemberAccessor>.CopyTo(MemberAccessor[] array, int arrayIndex)
		{
			_members.CopyTo(array, arrayIndex);
		}

		bool ICollection<MemberAccessor>.Remove(MemberAccessor item)
		{
			return _members.Remove(item);
		}

		public int Count
		{
			get { return _members.Count; }
		}

		bool ICollection<MemberAccessor>.IsReadOnly
		{
			get { return ((ICollection<MemberAccessor>)_members).IsReadOnly; }
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

		#region IEnumerable<MemberAccessor> Members

		IEnumerator<MemberAccessor> IEnumerable<MemberAccessor>.GetEnumerator()
		{
			foreach (MemberAccessor member in _members)
				yield return member;
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
					return string.Format("{0}?", MapTypeName(Nullable.GetUnderlyingType(type)));

				var name = type.Name;
				var idx  = name.IndexOf('`');

				if (idx >= 0)
					name = name.Substring(0, idx);

				name += "<";

				foreach (var t in type.GetGenericArguments())
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

			MemberAccessor ma;

			var ta      = GetAccessor(o.GetType());
			var nameLen = 0;
			var typeLen = 0;

			foreach (var de in ta._memberNames)
			{
				if (nameLen < de.Key.Length)
					nameLen = de.Key.Length;

				ma = de.Value;

				if (typeLen < MapTypeName(ma.Type).Length)
					typeLen = MapTypeName(ma.Type).Length;
			}

			var text = "*** " + o.GetType().FullName + ": ***";

			writeLine(text);

			var format = string.Format("{{0,-{0}}} {{1,-{1}}} : {{2}}", typeLen, nameLen);

			foreach (var de in ta._memberNames)
			{
				ma = de.Value;

				var value = ma.GetValue(o);

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

		#region TypeDescriptor

#if !SILVERLIGHT && !DATA

		#region CustomTypeDescriptor

		private static readonly Hashtable _descriptors = new Hashtable();

		public static ICustomTypeDescriptor GetCustomTypeDescriptor(Type type)
		{
			var descriptor = (ICustomTypeDescriptor)_descriptors[type];

			if (descriptor == null)
			{
				lock (_descriptors.SyncRoot)
				{
					descriptor = (ICustomTypeDescriptor)_descriptors[type];
					
					if (descriptor == null)
					{
						descriptor = new CustomTypeDescriptorImpl(type);
						
						_descriptors.Add(type, descriptor);
					}
				}
			}
			return descriptor;
		}

		private ICustomTypeDescriptor _customTypeDescriptor;
		public  ICustomTypeDescriptor  CustomTypeDescriptor
		{
			get { return _customTypeDescriptor ?? (_customTypeDescriptor = GetCustomTypeDescriptor(OriginalType)); }
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
						var descriptor = CreateInstance() as ICustomTypeDescriptor;

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
			if (Data.DbManager.TraceSwitch.TraceInfo)
				Data.DbManager.WriteTraceLine(OriginalType.FullName, "CreatePropertyDescriptors");

			var pd = new PropertyDescriptor[Count];

			var i = 0;
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

			var pdc = CreatePropertyDescriptors();

			if (objectViewType != null)
			{
				var viewAccessor = GetAccessor(objectViewType);
				var objectView   = (IObjectView)viewAccessor.CreateInstanceEx();
				var list         = new List<PropertyDescriptor>();

				var viewpdc = viewAccessor.PropertyDescriptors;

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
			var list      = new ArrayList(pdc.Count);
			var objects   = new ArrayList();
			var isDataRow = itemType.IsSubclassOf(typeof(DataRow));

			foreach (PropertyDescriptor p in pdc)
			{
				var propertyType = p.PropertyType;

				if (p.Attributes.Matches(BindableAttribute.No) ||
					//propertyType == typeof(Type)               ||
					isDataRow && p.Name == "ItemArray")
					continue;

				var isList           = false;
				var explicitlyBound  = p.Attributes.Contains(BindableAttribute.Yes);
				var pd               = p;

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
					var childParentTypes = new Type[parentTypes.Length + 1];

					parentTypes.CopyTo(childParentTypes, 0);
					childParentTypes[parentTypes.Length] = itemType;

					var childParentAccessors = new PropertyDescriptor[parentAccessors.Length + 1];

					parentAccessors.CopyTo(childParentAccessors, 0);
					childParentAccessors[parentAccessors.Length] = pd;

					var pdch = GetAccessor(propertyType).PropertyDescriptors;

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
				var value = base.GetValue(component);

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
			protected readonly PropertyDescriptor   _descriptor;
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
				for (var i = 0;
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
					switch (Common.Configuration.CheckNullReturnIfNull)
					{
						case Common.Configuration.NullEquivalent.DBNull:
							return DBNull.Value;
						case Common.Configuration.NullEquivalent.Null:
							return null;
						case Common.Configuration.NullEquivalent.Value:
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
			var ma = this[name];
			return ma != null ? ma.PropertyDescriptor : null;
		}

		AttributeCollection ITypeDescriptionProvider.GetAttributes()
		{
			var attributesAsObj = new TypeHelper(OriginalType).GetAttributes();
			var attributes      = new Attribute[attributesAsObj.Length];

			for (var i = 0; i < attributesAsObj.Length; i++)
				attributes[i] = attributesAsObj[i] as Attribute;

			return new AttributeCollection(attributes);
		}

		EventDescriptorCollection ITypeDescriptionProvider.GetEvents()
		{
			var ei = OriginalType.GetEvents();
			var ed = new EventDescriptor[ei.Length];

			for (var i = 0; i < ei.Length; i++)
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

#endif

		#endregion
	}
}
