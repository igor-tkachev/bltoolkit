using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping
{
	public class ObjectMapper : MapDataSourceDestinationBase, IEnumerable
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

		private string[] _fieldNames;
		public  string[]  FieldNames
		{
			get
			{
				if (_fieldNames == null)
				{
					_fieldNames = new string[_members.Count];

					for (int i = 0; i < _fieldNames.Length; i++)
					{
						_fieldNames[i] = ((MemberMapper)_members[i]).Name;
					}
				}

				return _fieldNames;
			}
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
						if (ma.MemberName == name)
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

				MapFieldAttribute mapFieldAttr = (MapFieldAttribute)ma.GetAttribute(typeof(MapFieldAttribute));

				if (mapFieldAttr == null || (mapFieldAttr.OrigName == null && mapFieldAttr.Format == null))
				{
					MapMemberInfo mi = new MapMemberInfo();

					mi.MemberAccessor = ma;
					mi.Type = ma.Type;
					mi.MappingSchema = mappingSchema;
					mi.MemberExtension = _extension[ma.Name];
					mi.Name = GetFieldName(ma);
					mi.MemberName = ma.Name;
					mi.Trimmable = GetTrimmable(ma);
					mi.MapValues = GetMapValues(ma);
					mi.DefaultValue = GetDefaultValue(ma);
					mi.Nullable = GetNullable(ma);
					mi.NullValue = GetNullValue(ma, mi.Nullable);

					Add(CreateMemberMapper(mi));
				}
				else if (null != mapFieldAttr.OrigName)
					EnsureMapper(mapFieldAttr.MapName, ma.Name + "." + mapFieldAttr.OrigName);
				else if (null != mapFieldAttr.Format)
				{
					foreach (MemberMapper inner in _mappingSchema.GetObjectMapper(ma.Type))
						EnsureMapper(string.Format(mapFieldAttr.Format, inner.Name), ma.Name + "." + inner.MemberName);
				}
			}

			foreach (AttributeExtension ae in _extension.Attributes["MapField"])
			{
				string mapName  = (string)ae["MapName"];
				string origName = (string)ae["OrigName"];

				if (mapName == null || origName == null)
					throw new MappingException(string.Format(
						"Type '{0}' has invalid  extension. MapField MapName='{1}' OrigName='{2}'.",
							type.FullName, mapName, origName));

				EnsureMapper(mapName, origName);
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
								mi.MemberName     = origName;

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

			return !TypeHelper.IsScalar(type);
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

		public override int Count
		{
			get { return _members.Count; }
		}

		public override Type GetFieldType(int index)
		{
			return ((MemberMapper)_members[index]).Type;
		}

		public override string GetName(int index)
		{
			return ((MemberMapper)_members[index]).Name;
		}

		public override object GetValue(object o, int index)
		{
			return ((MemberMapper)_members[index]).GetValue(o);
		}

		public override object GetValue(object o, string name)
		{
			MemberMapper mm = (MemberMapper)_nameToMember[name];

			if (mm == null)
				mm = this[name];

			return mm == null? null: mm.GetValue(o);
		}

		public override bool     IsNull     (object o, int index) { return this[index].IsNull(o);      }

		public override bool     SupportsTypedValues(int index)   { return this[index].SupportsValue;  }

		// Simple type getters.
		//
		[CLSCompliant(false)]
		public override SByte    GetSByte   (object o, int index) { return this[index].GetSByte   (o); }
		public override Int16    GetInt16   (object o, int index) { return this[index].GetInt16   (o); }
		public override Int32    GetInt32   (object o, int index) { return this[index].GetInt32   (o); }
		public override Int64    GetInt64   (object o, int index) { return this[index].GetInt64   (o); }

		public override Byte     GetByte    (object o, int index) { return this[index].GetByte    (o); }
		[CLSCompliant(false)]
		public override UInt16   GetUInt16  (object o, int index) { return this[index].GetUInt16  (o); }
		[CLSCompliant(false)]
		public override UInt32   GetUInt32  (object o, int index) { return this[index].GetUInt32  (o); }
		[CLSCompliant(false)]
		public override UInt64   GetUInt64  (object o, int index) { return this[index].GetUInt64  (o); }

		public override Boolean  GetBoolean (object o, int index) { return this[index].GetBoolean (o); }
		public override Char     GetChar    (object o, int index) { return this[index].GetChar    (o); }
		public override Single   GetSingle  (object o, int index) { return this[index].GetSingle  (o); }
		public override Double   GetDouble  (object o, int index) { return this[index].GetDouble  (o); }
		public override Decimal  GetDecimal (object o, int index) { return this[index].GetDecimal (o); }
		public override Guid     GetGuid    (object o, int index) { return this[index].GetGuid    (o); }
		public override DateTime GetDateTime(object o, int index) { return this[index].GetDateTime(o); }

#if FW2
		// Nullable type getters.
		//
		[CLSCompliant(false)]
		public override SByte?    GetNullableSByte   (object o, int index) { return this[index].GetNullableSByte   (o); }
		public override Int16?    GetNullableInt16   (object o, int index) { return this[index].GetNullableInt16   (o); }
		public override Int32?    GetNullableInt32   (object o, int index) { return this[index].GetNullableInt32   (o); }
		public override Int64?    GetNullableInt64   (object o, int index) { return this[index].GetNullableInt64   (o); }

		public override Byte?     GetNullableByte    (object o, int index) { return this[index].GetNullableByte    (o); }
		[CLSCompliant(false)]
		public override UInt16?   GetNullableUInt16  (object o, int index) { return this[index].GetNullableUInt16  (o); }
		[CLSCompliant(false)]
		public override UInt32?   GetNullableUInt32  (object o, int index) { return this[index].GetNullableUInt32  (o); }
		[CLSCompliant(false)]
		public override UInt64?   GetNullableUInt64  (object o, int index) { return this[index].GetNullableUInt64  (o); }

		public override Boolean?  GetNullableBoolean (object o, int index) { return this[index].GetNullableBoolean (o); }
		public override Char?     GetNullableChar    (object o, int index) { return this[index].GetNullableChar    (o); }
		public override Single?   GetNullableSingle  (object o, int index) { return this[index].GetNullableSingle  (o); }
		public override Double?   GetNullableDouble  (object o, int index) { return this[index].GetNullableDouble  (o); }
		public override Decimal?  GetNullableDecimal (object o, int index) { return this[index].GetNullableDecimal (o); }
		public override Guid?     GetNullableGuid    (object o, int index) { return this[index].GetNullableGuid    (o); }
		public override DateTime? GetNullableDateTime(object o, int index) { return this[index].GetNullableDateTime(o); }
#endif
		// SQL type getters.
		//
		public override SqlByte     GetSqlByte    (object o, int index) { return this[index].GetSqlByte    (o); }
		public override SqlInt16    GetSqlInt16   (object o, int index) { return this[index].GetSqlInt16   (o); }
		public override SqlInt32    GetSqlInt32   (object o, int index) { return this[index].GetSqlInt32   (o); }
		public override SqlInt64    GetSqlInt64   (object o, int index) { return this[index].GetSqlInt64   (o); }
		public override SqlSingle   GetSqlSingle  (object o, int index) { return this[index].GetSqlSingle  (o); }
		public override SqlBoolean  GetSqlBoolean (object o, int index) { return this[index].GetSqlBoolean (o); }
		public override SqlDouble   GetSqlDouble  (object o, int index) { return this[index].GetSqlDouble  (o); }
		public override SqlDateTime GetSqlDateTime(object o, int index) { return this[index].GetSqlDateTime(o); }
		public override SqlDecimal  GetSqlDecimal (object o, int index) { return this[index].GetSqlDecimal (o); }
		public override SqlMoney    GetSqlMoney   (object o, int index) { return this[index].GetSqlMoney   (o); }
		public override SqlGuid     GetSqlGuid    (object o, int index) { return this[index].GetSqlGuid    (o); }
		public override SqlString   GetSqlString  (object o, int index) { return this[index].GetSqlString  (o); }

		#endregion

		#region IMapDataDestination Members

		public override int GetOrdinal(string name)
		{
			MemberMapper mm = (MemberMapper)_nameToMember[name];

			if (mm == null)
				mm = this[name];

			return mm == null? -1: mm.Ordinal;
		}

		public override void SetValue(object o, int index, object value)
		{
			((MemberMapper)_members[index]).SetValue(o, value);
		}

		public override void SetValue(object o, string name, object value)
		{
			SetValue(o, GetOrdinal(name), value);
		}

		public override void SetNull    (object o, int index)                 { this[index].SetNull    (o); }

		// Simple types setters.
		//
		[CLSCompliant(false)]
		public override void SetSByte   (object o, int index, SByte    value) { this[index].SetSByte   (o, value); }
		public override void SetInt16   (object o, int index, Int16    value) { this[index].SetInt16   (o, value); }
		public override void SetInt32   (object o, int index, Int32    value) { this[index].SetInt32   (o, value); }
		public override void SetInt64   (object o, int index, Int64    value) { this[index].SetInt64   (o, value); }

		public override void SetByte    (object o, int index, Byte     value) { this[index].SetByte    (o, value); }
		[CLSCompliant(false)]
		public override void SetUInt16  (object o, int index, UInt16   value) { this[index].SetUInt16  (o, value); }
		[CLSCompliant(false)]
		public override void SetUInt32  (object o, int index, UInt32   value) { this[index].SetUInt32  (o, value); }
		[CLSCompliant(false)]
		public override void SetUInt64  (object o, int index, UInt64   value) { this[index].SetUInt64  (o, value); }

		public override void SetBoolean (object o, int index, Boolean  value) { this[index].SetBoolean (o, value); }
		public override void SetChar    (object o, int index, Char     value) { this[index].SetChar    (o, value); }
		public override void SetSingle  (object o, int index, Single   value) { this[index].SetSingle  (o, value); }
		public override void SetDouble  (object o, int index, Double   value) { this[index].SetDouble  (o, value); }
		public override void SetDecimal (object o, int index, Decimal  value) { this[index].SetDecimal (o, value); }
		public override void SetGuid    (object o, int index, Guid     value) { this[index].SetGuid    (o, value); }
		public override void SetDateTime(object o, int index, DateTime value) { this[index].SetDateTime(o, value); }

#if FW2
		// Simple types setters.
		//
		[CLSCompliant(false)]
		public override void SetNullableSByte   (object o, int index, SByte?    value) { this[index].SetNullableSByte   (o, value); }
		public override void SetNullableInt16   (object o, int index, Int16?    value) { this[index].SetNullableInt16   (o, value); }
		public override void SetNullableInt32   (object o, int index, Int32?    value) { this[index].SetNullableInt32   (o, value); }
		public override void SetNullableInt64   (object o, int index, Int64?    value) { this[index].SetNullableInt64   (o, value); }

		public override void SetNullableByte    (object o, int index, Byte?     value) { this[index].SetNullableByte    (o, value); }
		[CLSCompliant(false)]
		public override void SetNullableUInt16  (object o, int index, UInt16?   value) { this[index].SetNullableUInt16  (o, value); }
		[CLSCompliant(false)]
		public override void SetNullableUInt32  (object o, int index, UInt32?   value) { this[index].SetNullableUInt32  (o, value); }
		[CLSCompliant(false)]
		public override void SetNullableUInt64  (object o, int index, UInt64?   value) { this[index].SetNullableUInt64  (o, value); }

		public override void SetNullableBoolean (object o, int index, Boolean?  value) { this[index].SetNullableBoolean (o, value); }
		public override void SetNullableChar    (object o, int index, Char?     value) { this[index].SetNullableChar    (o, value); }
		public override void SetNullableSingle  (object o, int index, Single?   value) { this[index].SetNullableSingle  (o, value); }
		public override void SetNullableDouble  (object o, int index, Double?   value) { this[index].SetNullableDouble  (o, value); }
		public override void SetNullableDecimal (object o, int index, Decimal?  value) { this[index].SetNullableDecimal (o, value); }
		public override void SetNullableGuid    (object o, int index, Guid?     value) { this[index].SetNullableGuid    (o, value); }
		public override void SetNullableDateTime(object o, int index, DateTime? value) { this[index].SetNullableDateTime(o, value); }
#endif

		// SQL type setters.
		//
		public override void SetSqlByte    (object o, int index, SqlByte     value) { this[index].SetSqlByte    (o, value); }
		public override void SetSqlInt16   (object o, int index, SqlInt16    value) { this[index].SetSqlInt16   (o, value); }
		public override void SetSqlInt32   (object o, int index, SqlInt32    value) { this[index].SetSqlInt32   (o, value); }
		public override void SetSqlInt64   (object o, int index, SqlInt64    value) { this[index].SetSqlInt64   (o, value); }
		public override void SetSqlSingle  (object o, int index, SqlSingle   value) { this[index].SetSqlSingle  (o, value); }
		public override void SetSqlBoolean (object o, int index, SqlBoolean  value) { this[index].SetSqlBoolean (o, value); }
		public override void SetSqlDouble  (object o, int index, SqlDouble   value) { this[index].SetSqlDouble  (o, value); }
		public override void SetSqlDateTime(object o, int index, SqlDateTime value) { this[index].SetSqlDateTime(o, value); }
		public override void SetSqlDecimal (object o, int index, SqlDecimal  value) { this[index].SetSqlDecimal (o, value); }
		public override void SetSqlMoney   (object o, int index, SqlMoney    value) { this[index].SetSqlMoney   (o, value); }
		public override void SetSqlGuid    (object o, int index, SqlGuid     value) { this[index].SetSqlGuid    (o, value); }
		public override void SetSqlString  (object o, int index, SqlString   value) { this[index].SetSqlString  (o, value); }

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _members.GetEnumerator();
		}

		#endregion
	}
}
