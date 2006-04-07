using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping
{
	public class ObjectMapper : IMapDataSource, IMapDataDestination, IEnumerable
	{
		#region Constructor

		public ObjectMapper()
		{
			_members      = new ArrayList();
			_nameToMember = new Hashtable();
		}

		#endregion

		#region Protected Members

		protected virtual MemberMapper CreateMemberMapper(MapMemberInfo mapMemberInfo)
		{
			if (mapMemberInfo == null) throw new ArgumentNullException("mapMemberInfo");

			MemberMapper mm = null;

			Attribute attr = mapMemberInfo.MemberAccessor.GetAttribute(typeof(MemberMapperAttribute));

			if (attr != null)
				mm = ((MemberMapperAttribute)attr).MemberMapper;

			if (mm == null)
			{
				object[] attrs = TypeHelper.GetAttributes(
					mapMemberInfo.MemberAccessor.MemberInfo.DeclaringType, typeof(MemberMapperAttribute));

				foreach (MemberMapperAttribute a in attrs)
				{
					if (a.MemberType == mapMemberInfo.Type)
					{
						mm = a.MemberMapper;
						break;
					}
				}
			}

			if (mm == null)
				mm = MemberMapper.CreateMemberMapper(mapMemberInfo);

			mm.Init(mapMemberInfo);

			return mm;
		}

		[SuppressMessage("Microsoft.Performance", "CA1807:AvoidUnnecessaryStringCreation", MessageId = "stack1")]
		protected virtual void Add(MemberMapper memberMapper)
		{
			if (memberMapper == null) throw new ArgumentNullException("memberMapper");

			memberMapper.SetOrdinal(_members.Count);

			_members.     Add(memberMapper);
			_nameToMember.Add(memberMapper.Name.ToLower(),  memberMapper);
		}

		private   object[] _mapFieldAttributes;
		protected object[]  MapFieldAttributes
		{
			get
			{
				if (_mapFieldAttributes == null)
					_mapFieldAttributes = TypeHelper.GetAttributes(_typeAccessor.Type, typeof(MapFieldAttribute));

				return _mapFieldAttributes;
			}
		}

		#endregion

		#region Public Members

		private ArrayList _members;
		public  MemberMapper this[int index]
		{
			get { return (MemberMapper)_members[index]; }
		}

		private TypeExtension _extension;
		public  TypeExtension  Extension
		{
			get { return _extension;  }
			set { _extension = value; }
		}

		private Hashtable _nameToMember;
		public  MemberMapper this[string name]
		{
			get
			{
				if (name == null) throw new ArgumentNullException("name");

				MemberMapper mm = (MemberMapper)_nameToMember[name];

				if (mm == null)
				{
					lock (_nameToMember.SyncRoot)
					{
						mm = (MemberMapper)_nameToMember[name];

						if (mm == null)
						{
							mm = (MemberMapper)_nameToMember[name.ToLower(CultureInfo.CurrentCulture)];

							if (mm == null)
							{
								mm = GetComplexMapper(name, name);

								if (mm != null)
								{
									if (_members.Contains(mm))
										throw new MappingException(string.Format(
											"Wrong mapping field name: '{0}', type: '{1}'. Use name '{2}' instead",
											name, _typeAccessor.OriginalType.Name, mm.Name));

									Add(mm);
								}
							}
							else
							{
								_nameToMember[name] = mm;
							}
						}
					}
				}

				return mm;
			}
		}

		public MemberMapper this[string name, bool byPropertyName]
		{
			get
			{
				if (byPropertyName)
				{
					foreach (MemberMapper ma in _members)
						if (ma.MemberAccessor.Name == name)
							return ma;

					return null;
				}

				return this[name];
			}
		}

		private TypeAccessor _typeAccessor;
		public  TypeAccessor  TypeAccessor
		{
			get { return _typeAccessor; }
		}

		private MappingSchema _mappingSchema;
		public  MappingSchema  MappingSchema
		{
			get { return _mappingSchema; }
		}

		#endregion

		#region Init Mapper

		public virtual void Init(MappingSchema mappingSchema, Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			_typeAccessor  = TypeAccessor.GetAccessor(type);
			_mappingSchema = mappingSchema;
			_extension     = TypeExtension.GetTypeExtenstion(
				_typeAccessor.OriginalType, mappingSchema.Extensions);

			foreach (MemberAccessor ma in _typeAccessor)
			{
				if (GetIgnore(ma))
					continue;

				MapMemberInfo mi = new MapMemberInfo();

				mi.MemberAccessor  = ma;
				mi.Type            = ma.Type;
				mi.MappingSchema   = mappingSchema;
				mi.MemberExtension = _extension[ma.Name];
				mi.Name            = GetFieldName   (ma);
				mi.Trimmable       = GetTrimmable   (ma);
				mi.MapValues       = GetMapValues   (ma);
				mi.DefaultValue    = GetDefaultValue(ma);
				mi.Nullable        = GetNullable    (ma);
				mi.NullValue       = GetNullValue   (ma, mi.Nullable);

				Add(CreateMemberMapper(mi));
			}

			foreach (MapFieldAttribute attr in MapFieldAttributes)
				EnsureMapper(attr.MapName, attr.OrigName);
		}

		private MemberMapper EnsureMapper(string mapName, string origName)
		{
			MemberMapper mm = this[mapName];

			if (mm == null)
			{
				string name = mapName.ToLower();

				foreach (MemberMapper m in _members)
				{
					if (m.MemberAccessor.Name.ToLower() == name)
					{
						_nameToMember.Add(name,  m);
						return m;
					}
				}

				mm = GetComplexMapper(mapName, origName);

				if (mm != null)
					Add(mm);
			}

			return mm;
		}

		[SuppressMessage("Microsoft.Performance", "CA1807:AvoidUnnecessaryStringCreation", MessageId = "stack0")]
		[SuppressMessage("Microsoft.Performance", "CA1807:AvoidUnnecessaryStringCreation", MessageId = "origName")]
		protected MemberMapper GetComplexMapper(string mapName, string origName)
		{
			if (origName == null) throw new ArgumentNullException("origName");

			string name = origName.ToLower();
			int    idx  = origName.IndexOf('.');

			if (idx > 0 /*&& idx < origName.Length*/)
			{
				name = name.Substring(0, idx);

				foreach (MemberAccessor ma in TypeAccessor)
				{
					if (ma.Name.ToLower() == name)
					{
						ObjectMapper om = MappingSchema.GetObjectMapper(ma.Type);

						if (om != null)
						{
							MemberMapper mm = om.GetComplexMapper(mapName, origName.Substring(idx + 1));

							if (mm != null)
							{
								MapMemberInfo mi = new MapMemberInfo();

								mi.MemberAccessor = ma;
								mi.Type           = mm.Type;
								mi.MappingSchema  = MappingSchema;
								mi.Name           = mapName;

								MemberMapper mapper = new MemberMapper.ComplexMapper(mm);

								mapper.Init(mi);

								return mapper;
							}
						}

						break;
					}
				}
			}
			else
				foreach (MemberMapper m in _members)
					if (m.MemberAccessor.Name.ToLower() == name)
						return m;

			return null;
		}

		protected virtual string GetFieldName(MemberAccessor memberAccessor)
		{
			object extValue = Extension[memberAccessor.Name]["MapField"].Value;

			if (extValue != null)
				return extValue.ToString();

			MapFieldAttribute a = (MapFieldAttribute)memberAccessor.GetAttribute(typeof(MapFieldAttribute));

			if (a != null)
				return a.MapName;

			string name = memberAccessor.Name.ToLower();

			foreach (MapFieldAttribute attr in MapFieldAttributes)
				if (attr.OrigName.ToLower() == name)
					return attr.MapName;

			return memberAccessor.Name;
		}

		protected virtual bool GetIgnore(MemberAccessor memberAccessor)
		{
			object extValue = Extension[memberAccessor.Name]["MapIgnore"].Value;

			if (extValue != null)
				return TypeExtension.ToBoolean(extValue);

			MapIgnoreAttribute attr = 
				(MapIgnoreAttribute)memberAccessor.GetAttribute(typeof(MapIgnoreAttribute));

			if (attr != null)
				return attr.Ignore;

			Type type = memberAccessor.Type;

			return 
				TypeHelper.IsScalar(type) == false &&
				memberAccessor.GetAttribute(typeof(MemberMapperAttribute)) == null;
		}

		protected virtual bool GetTrimmable(MemberAccessor memberAccessor)
		{
			if (memberAccessor.Type != typeof(string))
				return false;

			object extValue = Extension[memberAccessor.Name]["Trimmable"].Value;

			if (extValue != null)
				return TypeExtension.ToBoolean(extValue);

			TrimmableAttribute attr = 
				(TrimmableAttribute)memberAccessor.GetAttribute(typeof(TrimmableAttribute));

			if (attr != null)
				return attr.IsTrimmable;

			attr = (TrimmableAttribute)TypeHelper.GetFirstAttribute(
				memberAccessor.MemberInfo.DeclaringType, typeof(TrimmableAttribute));

			if (attr != null)
				return attr.IsTrimmable;

			return TrimmableAttribute.Default.IsTrimmable;
		}

		private ArrayList GetExtensionMapValues(MemberAccessor memberAccessor)
		{
			AttributeExtensionCollection extList = Extension[memberAccessor.Name]["MapValue"];

			ArrayList list = null;

			if (extList == AttributeExtensionCollection.Null)
			{
				if (memberAccessor.Type.IsEnum)
					list = MappingSchema.GetEnumMapValuesFromExtension(Extension, memberAccessor.Type);

				return list != null? list: MappingSchema.GetExtensionMapValues(Extension, memberAccessor.Type);
			}

			list = new ArrayList(extList.Count);

			foreach (AttributeExtension ext in extList)
			{
				object origValue = ext["OrigValue"];

				if (origValue != null)
				{
					origValue = TypeExtension.ChangeType(origValue, memberAccessor.Type);
					list.Add(new MapValue(origValue, ext.Value));
				}
			}

			return list;
		}

		protected virtual MapValue[] GetMapValues(MemberAccessor memberAccessor)
		{
			ArrayList list = GetExtensionMapValues(memberAccessor);

			if (list != null)
				return (MapValue[])list.ToArray(typeof(MapValue));

			object[] attrs = memberAccessor.GetAttributes(typeof(MapValueAttribute));

			if (attrs != null)
			{
				list = new ArrayList(attrs.Length);

				foreach (MapValueAttribute a in attrs)
					list.Add(new MapValue(a.OrigValue, a.Values));
			}

			attrs = memberAccessor.GetTypeAttributes(typeof(MapValueAttribute));

			if (attrs != null && attrs.Length > 0)
			{
				if (list == null)
					list = new ArrayList(attrs.Length);

				foreach (MapValueAttribute a in attrs)
					if (a.Type == null && a.OrigValue != null && a.OrigValue.GetType() == memberAccessor.Type ||
						a.Type != null && a.Type == memberAccessor.Type)
						list.Add(new MapValue(a.OrigValue, a.Values));
			}

			MapValue[] typeMapValues = _mappingSchema.GetMapValues(memberAccessor.Type);

			if (list == null) return typeMapValues;

			if (typeMapValues != null)
				list.AddRange(typeMapValues);

			return (MapValue[])list.ToArray(typeof(MapValue));
		}

		private object GetExtensionDefaultValue(MemberAccessor memberAccessor)
		{
			object value = Extension[memberAccessor.Name]["DefaultValue"].Value;

			if (value == null)
				value = MappingSchema.GetExtensionDefaultValue(Extension, memberAccessor.Type);

			return TypeExtension.ChangeType(value, memberAccessor.Type);
		}

		protected virtual object GetDefaultValue(MemberAccessor memberAccessor)
		{
			object value = GetExtensionDefaultValue(memberAccessor);

			if (value != null)
				return value;

			// Check member [DefaultValue(0)]
			//
			DefaultValueAttribute attr =
				(DefaultValueAttribute)memberAccessor.GetAttribute(typeof(DefaultValueAttribute));

			if (attr != null)
				return attr.Value;

			// Check type [DefaultValues(typeof(int), 0)]
			//
			object[] attrs = memberAccessor.GetTypeAttributes(typeof(DefaultValueAttribute));

			foreach (DefaultValueAttribute a in attrs)
				if (a.Type == null && a.Value != null && a.Value.GetType() == memberAccessor.Type ||
					a.Type != null && a.Type == memberAccessor.Type)
					return a.Value;

			return _mappingSchema.GetDefaultValue(memberAccessor.Type);
		}

		private object GetExtensionIsNullable(MemberAccessor memberAccessor)
		{
			object value = Extension[memberAccessor.Name]["Nullable"].Value;

			if (value != null)
				return TypeExtension.ToBoolean(value);

			value = Extension[memberAccessor.Name]["NullValue"].Value;

			return value != null;
		}

		protected virtual bool GetNullable(MemberAccessor memberAccessor)
		{
			// Check extension <Member1 Nullable='true' />
			//
			object value = Extension[memberAccessor.Name]["Nullable"].Value;

			if (value != null)
				return TypeExtension.ToBoolean(value);

			// Check extension <Member1 NullValue='-1' />
			//
			if (Extension[memberAccessor.Name]["NullValue"].Value != null)
				return true;

			// Check member [Nullable(true | false)]
			//
			NullableAttribute attr1 =
				(NullableAttribute)memberAccessor.GetAttribute(typeof(NullableAttribute));

			if (attr1 != null)
				return attr1.IsNullable;

			// Check member [NullValue(0)]
			//
			NullValueAttribute attr2 =
				(NullValueAttribute)memberAccessor.GetAttribute(typeof(NullValueAttribute));

			if (attr2 != null)
				return true;

			// Check type [Nullable(true || false)]
			//
			attr1 = (NullableAttribute)TypeHelper.GetFirstAttribute(
				memberAccessor.MemberInfo.DeclaringType, typeof(NullableAttribute));

			if (attr1 != null)
				return attr1.IsNullable;

			// Check type [NullValues(typeof(int), 0)]
			//
			object[] attrs = memberAccessor.GetTypeAttributes(typeof(NullValueAttribute));

			foreach (NullValueAttribute a in attrs)
				if (a.Type == null && a.Value != null && a.Value.GetType() == memberAccessor.Type ||
					a.Type != null && a.Type == memberAccessor.Type)
					return true;

			if (memberAccessor.Type.IsEnum)
				return MappingSchema.GetNullValue(memberAccessor.Type) != null;

			return false;
		}

		private static object CheckNullValue(object value, MemberAccessor memberAccessor)
		{
			if (value is Type && (Type)value == typeof(DBNull))
			{
				value = DBNull.Value;

				if (memberAccessor.Type == typeof(string))
					value = null;
			}

			return value;
		}

		protected virtual object GetNullValue(MemberAccessor memberAccessor, bool isNullable)
		{
			if (isNullable)
			{
				// Check extension <Member1 NullValue='-1' />
				//
				object value = Extension[memberAccessor.Name]["NullValue"].Value;

				if (value != null)
					return TypeExtension.ChangeType(value, memberAccessor.Type);

				// Check member [NullValue(0)]
				//
				NullValueAttribute attr =
					(NullValueAttribute)memberAccessor.GetAttribute(typeof(NullValueAttribute));

				if (attr != null)
					return CheckNullValue(attr.Value, memberAccessor);

				// Check type [NullValues(typeof(int), 0)]
				//
				object[] attrs = memberAccessor.GetTypeAttributes(typeof(NullValueAttribute));

				foreach (NullValueAttribute a in attrs)
					if (a.Type == null && a.Value != null && a.Value.GetType() == memberAccessor.Type ||
						a.Type != null && a.Type == memberAccessor.Type)
						return CheckNullValue(a.Value, memberAccessor);
			}

			return CheckNullValue(MappingSchema.GetNullValue(memberAccessor.Type), memberAccessor);
		}

		#endregion

		#region IObjectMappper Members

		public virtual object CreateInstance()
		{
			return _typeAccessor.CreateInstanceEx();
		}

		public virtual object CreateInstance(InitContext context)
		{
			return _typeAccessor.CreateInstanceEx(context);
		}

		#endregion

		#region IMapDataSource Members

		public virtual int Count
		{
			get { return _members.Count; }
		}

		public virtual string GetName(int index)
		{
			return ((MemberMapper)_members[index]).Name;
		}

		public virtual object GetValue(object o, int index)
		{
			return ((MemberMapper)_members[index]).GetValue(o);
		}

		public virtual object GetValue(object o, string name)
		{
			MemberMapper mm = (MemberMapper)_nameToMember[name];

			if (mm == null)
				mm = this[name];

			return mm == null? null: mm.GetValue(o);
		}

		#endregion

		#region IMapDataDestination Members

		public virtual int GetOrdinal(string name)
		{
			MemberMapper mm = (MemberMapper)_nameToMember[name];

			if (mm == null)
				mm = this[name];

			return mm == null? -1: mm.Ordinal;
		}

		public virtual void SetValue(object o, int index, object value)
		{
			((MemberMapper)_members[index]).SetValue(o, value);
		}

		public virtual void SetValue(object o, string name, object value)
		{
			SetValue(o, GetOrdinal(name), value);
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _members.GetEnumerator();
		}

		#endregion
	}
}
