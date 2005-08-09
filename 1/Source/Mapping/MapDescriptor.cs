/*
 * File:    TypeDescriptor.cs
 * Created: 07/04/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml;
using System.Xml.Schema;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	///
	/// </summary>
	public class MapDescriptor : IMapDataSource, IMapDataReceiver, ICloneable, IEnumerable
	{
		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual object CreateInstance()
		{
			try
			{
				return Activator.CreateInstance(_mappedType);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public virtual object CreateInstance(MapInitializingData data)
		{
			return CreateInstance();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual object CreateInstanceEx()
		{
			return CreateInstanceEx((MapInitializingData)null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public virtual object CreateInstanceEx(MapInitializingData data)
		{
			return _objectFactory != null? _objectFactory.CreateInstance(data): CreateInstance(data);
		}

#if VER2
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T CreateInstance<T>()
		{
			return (T)CreateInstance();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <returns></returns>
		public T CreateInstance<T>(MapInitializingData data)
		{
			return (T)CreateInstance(data);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T CreateInstanceEx<T>()
		{
			return (T)CreateInstanceEx();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <returns></returns>
		public T CreateInstanceEx<T>(MapInitializingData data)
		{
			return (T)CreateInstanceEx(data);
		}
#endif

		/*
		/// <summary>
		/// Attempts to create new instance using the constructor
		/// that best matches specified parameters.
		/// </summary>
		/// <param name="parameters">An array of arguments that match in number, order, 
		/// and type the parameters of the constructor to invoke.</param>
		/// <returns></returns>
		public object CreateInstance(params object[] parameters)
		{
			try
			{
				return Activator.CreateInstance(_mappedType, parameters);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return null;
			}
		}
		*/

		/// <summary>
		/// 
		/// </summary>
		public IMemberMapper this[string name]
		{
			get 
			{
				object obj = _memberTable[name];

				if (obj == null)
				{
					obj = _memberTable[name.ToLower()];

					_memberTable[name] = obj;
				}

				return (IMemberMapper)obj;
			}

			set
			{
				lock (_memberList.SyncRoot)
				{
					string key = name.ToLower();
					object obj = _memberTable[key];

					// Add new item.
					//
					if (obj == null)
					{
						value.Order = _memberList.Count;

						_memberList.Add(value);
					}
					// Replace existing item.
					//
					else
					{
						value.Order = ((IMemberMapper)obj).Order;

						_memberList[_memberList.IndexOf(obj)] = value;
					}

					_memberTable[key] = value;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public void Remove(string name)
		{
			string key = name.ToLower();
			object obj = _memberTable[key];

			if (obj != null)
			{
				lock (_memberList.SyncRoot)
				{
					_memberList. Remove(obj);
					_memberTable.Remove(key);

					for (int i = 0; i < _memberList.Count; i++)
						((IMemberMapper)_memberList[i]).Order = i;
				}
			}
		}

		private IMapObjectFactory _objectFactory = null;
		/// <summary>
		/// 
		/// </summary>
		public  IMapObjectFactory  ObjectFactory
		{
			get { return _objectFactory;  }
			set { _objectFactory = value; }
		}

		private MapXmlAttribute _xmlAttribute = null;
		/// <summary>
		/// 
		/// </summary>
		public  MapXmlAttribute  XmlAttribute
		{
			get { return _xmlAttribute;  }
			set { _xmlAttribute = value; }
		}

		private static Hashtable _mapTypes = new Hashtable(10);

		private static object[] GetMapTypeAttributes(Type type)
		{
			object[] attrs = (object[])_mapTypes[type];

			if (attrs == null)
			{
				attrs = type.GetCustomAttributes(typeof(MapTypeAttribute), false);

				_mapTypes[type] = attrs;
			}

			return attrs;
		}

		private static object[] GetPropertyMapTypeInternal(Type type, PropertyInfo pi)
		{
			object[] attr = GetMapTypeAttributes(type);

			foreach (MapTypeAttribute a in attr)
				if (a.PropertyType == pi.PropertyType)
					return new object[] { a };

			foreach (MapTypeAttribute a in attr)
				for (Type baseType = pi.PropertyType; baseType != null && baseType != typeof(object); baseType = baseType.BaseType)
					if (a.PropertyType == baseType)
						return new object[] { a };

			Type[] interfaces = type.GetInterfaces();

			for (int i = interfaces.Length - 1; i >= 0; i--)
			{
				Type itf = interfaces[i];

				PropertyInfo prop = itf.GetProperty(pi.Name, pi.PropertyType);

				if (prop != null)
				{
					attr = prop.GetCustomAttributes(typeof(MapTypeAttribute), true);

					if (attr.Length > 0)
						return ((MapTypeAttribute)attr[0]).MappedType == null? null: attr;
				}

				attr = GetMapTypeAttributes(itf);

				foreach (MapTypeAttribute a in attr)
					if (a.PropertyType == pi.PropertyType)
						return new object[] { a };

				foreach (MapTypeAttribute a in attr)
					for (Type baseType = pi.PropertyType; baseType != null && baseType != typeof(object); baseType = baseType.BaseType)
						if (a.PropertyType == baseType)
							return new object[] { a };
			}

			if (type.BaseType != typeof(object))
				return GetPropertyMapTypeInternal(type.BaseType, pi);

			return null;
		}

		internal static object[] GetPropertyMapType(Type type, PropertyInfo pi)
		{
			object[] attr = pi.GetCustomAttributes(typeof(MapTypeAttribute), true);

			if (attr.Length > 0)
				return ((MapTypeAttribute)attr[0]).MappedType == null? null: attr;

			return GetPropertyMapTypeInternal(type, pi);
		}

		internal static object[] GetPropertyParameters(PropertyInfo pi)
		{
			if (pi != null)
			{
				object[] paramAttr = pi.GetCustomAttributes(typeof(MapParameterAttribute), true);

				if (paramAttr == null || paramAttr.Length == 0)
					paramAttr = GetPropertyMapType(pi.DeclaringType, pi);

				if (paramAttr != null && paramAttr.Length != 0)
				{
					MapParameterAttribute pa = null;

					foreach (MapParameterAttribute a in paramAttr)
					{
						if (a is MapTypeAttribute)
						{
							if (pa != null)
								throw new RsdnMapException(string.Format(
									"Parameter ambiguity. See '{0}' member of '{1}' type",
									pi.Name,
									pi.DeclaringType));

							pa = a;
						}
					}

					if (pa != null && (pa.Parameters == null || pa.Parameters.Length == 0))
						pa = null;

					foreach (MapParameterAttribute a in paramAttr)
					{
						if (!(a is MapTypeAttribute))
						{
							if (pa != null)
								throw new RsdnMapException(string.Format(
									"Parameter ambiguity. See '{0}' member of '{1}' type",
									pi.Name,
									pi.DeclaringType));

							pa = a;
						}
					}

					if (pa != null)
						return pa.Parameters;
				}
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public object[] GetPropertyParameters(string propertyName)
		{
			PropertyInfo pi = OriginalType.GetProperty(
				propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			return GetPropertyParameters(pi);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void Init()
		{
		}

		#endregion

		#region Protected Methods

		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			lock (_typeList.SyncRoot)
			{
				foreach (MapDescriptor desc in _typeList.Values)
					if (desc.MappedType.Assembly.FullName == args.Name)
						return desc.MappedType.Assembly;
			}

			int idx = args.Name.IndexOf(".MappingExtension.");

			if (idx < 0)
				idx = args.Name.IndexOf(".MapDescriptor.dll");

			if (idx > 0)
			{
				string typeName = args.Name.Substring(0, idx);

				Type type = Type.GetType(typeName);

				if (type != null)
				{
					MapDescriptor desc = GetDescriptor(type);

					if (desc.MappedType.Assembly.FullName == args.Name)
						return desc.MappedType.Assembly;
				}
			}

			return null;
		}

		static MapDescriptor()
		{
			AppDomain.CurrentDomain.AssemblyResolve += 
				new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ex"></param>
		protected static void HandleException(Exception ex)
		{
			Map.HandleException(ex);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="originalType"></param>
		/// <param name="mappedType"></param>
		/// <param name="moduleBuilder"></param>
		internal void InitMembers(Type originalType, Type mappedType, ModuleBuilder moduleBuilder)
		{
			_originalType = originalType;
			_mappedType   = mappedType;

			object[] customAttrs = originalType.GetCustomAttributes(typeof(MapXmlAttribute), true);

			if (customAttrs != null && customAttrs.Length != 0)
				XmlAttribute = (MapXmlAttribute)customAttrs[0];

			MappingSchema  schema     = GetMappingSchema();
			FieldInfo[]    fields     = _mappedType.GetFields    (BindingFlags.Public | BindingFlags.Instance);
			PropertyInfo[] properties = _mappedType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			_memberTable = new Hashtable(fields.Length + properties.Length);
			_memberList  = new ArrayList(fields.Length + properties.Length);

			InitFields    (fields,     schema, moduleBuilder);
			InitProperties(properties, schema, moduleBuilder);
			InitMapping   (schema);
		}

		private void InitFields(FieldInfo[] fields, MappingSchema schema, ModuleBuilder moduleBuilder)
		{
			foreach (FieldInfo fi in fields)
			{
				if (!schema.HasIgnoreAttribute(
					fi.GetCustomAttributes(typeof(MapIgnoreAttribute), true), fi.Name))
				{
					Attribute[] valueAttributes = schema.GetValueAttributes(fi, fi.Name, fi.FieldType);

					FieldMapper fm = moduleBuilder != null?
						(FieldMapper)MapEmit.CreateMemberMapper(
							_originalType,
							_mappedType,
							fi.FieldType,
							fi.Name,
							false,
							true,
							true,
							moduleBuilder,
							valueAttributes):
						new FieldMapper();

					fm.InitField(schema, fi, valueAttributes);

					this[fm.Name] = fm;

					_hasClass = _hasClass || fm.IsClass;
				}
			}
		}

		private void InitProperties(PropertyInfo[] properties, MappingSchema schema, ModuleBuilder moduleBuilder)
		{
			foreach (PropertyInfo pi in properties)
			{
				if (pi.GetIndexParameters().Length == 0 &&
					!schema.HasIgnoreAttribute(
					pi.GetCustomAttributes(typeof(MapIgnoreAttribute), true), pi.Name))
				{
					Attribute[] valueAttributes = schema.GetValueAttributes(pi, pi.Name, pi.PropertyType);

					PropertyMapper pm = moduleBuilder != null?
						(PropertyMapper)(MapEmit.CreateMemberMapper(
							_originalType,
							_mappedType,
							pi.PropertyType,
							pi.Name,
							true,
							pi.CanRead,
							pi.CanWrite,
							moduleBuilder,
							valueAttributes)):
						new PropertyMapper();

					pm.InitProperty(schema, pi, valueAttributes);

					this[pm.Name] = pm;

					_hasClass = _hasClass || pm.IsClass;
				}
			}
		}

		private void InitMapping(MappingSchema schema)
		{
			object[] attrs = schema.GetMapAttributes(_originalType);

			if (attrs != null && attrs.Length != 0)
			{
				_sourceToTarget = new Hashtable(attrs.Length);
				_targetToSource = new Hashtable(attrs.Length);

				foreach (MapFieldAttribute attr in attrs)
				{
					if (attr.SourceName != null && attr.SourceName.Length != 0 &&
						attr.TargetName != null && attr.TargetName.Length != 0)
					{
						if (_sourceToTarget.Contains(attr.SourceName) == false)
							_sourceToTarget.Add(attr.SourceName, attr.TargetName);
						
						if (_targetToSource.Contains(attr.TargetName) == false)
							_targetToSource.Add(attr.TargetName, attr.SourceName);

						IMemberMapper mm = this[attr.TargetName.ToLower()];

						if (mm != null)
						{
							for (bool b = true; b; )
							{
								b = false;

								foreach (DictionaryEntry de in _memberTable)
								{
									if (de.Value == mm)
									{
										_memberTable.Remove(de.Key);
										b = true;

										break;
									}
								}
							}

							((BaseMemberMapper)mm).Name = attr.SourceName;
							_memberTable[attr.SourceName.ToLower()] = mm;
						}
						else
						{
							// Just to add the member into the member collections.
							//
							((IMapDataReceiver)this).GetOrdinal(attr.TargetName);
						}
					}
				}
			}
		}

		private Type _mappedType;
		/// <summary>
		/// 
		/// </summary>
		public  Type  MappedType
		{
			get { return _mappedType; }
		}

		private Type _originalType;
		/// <summary>
		/// 
		/// </summary>
		public  Type  OriginalType
		{
			get { return _originalType; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsAbstract
		{
			get { return _originalType != _mappedType; }
		}

		private  ArrayList _memberList;
		internal ArrayList  MemberList
		{
			get { return _memberList; }
		}

		private  Hashtable _memberTable;
		internal Hashtable  MemberTable
		{
			get { return _memberTable; }
		}

		private  Hashtable _sourceToTarget = null;
		internal Hashtable  SourceToTarget
		{
			get { return _sourceToTarget; }
		}

		private  Hashtable _targetToSource = null;
		internal Hashtable  TargetToSource
		{
			get { return _targetToSource; }
		}

		private bool _hasClass = false;

		#endregion

		#region IDataSource Members

		int IMapDataSource.FieldCount 
		{ 
			get { return _memberList.Count; }
		}

		string IMapDataSource.GetFieldName(int i)
		{
			IMemberMapper mm = (IMemberMapper)_memberList[i];

			if (mm.Order < 0)
				return string.Empty;

			string name = mm.Name;

			if (_targetToSource != null && _targetToSource.Count != 0)
			{
				object sourceName = _targetToSource[name];

				if (sourceName != null)
					name = sourceName as string;
			}

			return name;
		}

		object IMapDataSource.GetFieldValue(int i, object entity)
		{
			return ((IMemberMapper)_memberList[i]).GetValue(entity);
		}

		object IMapDataSource.GetFieldValue(string name, object entity)
		{
			IMemberMapper mm = this[name];

			return mm != null? mm.GetValue(entity): null;
		}

		#endregion

		#region IDataReceiver Members

		int IMapDataReceiver.GetOrdinal(string name)
		{
			if (_sourceToTarget != null && _sourceToTarget.Count != 0)
			{
				object targetName = _sourceToTarget[name];

				if (targetName != null)
					name = (string)targetName;
			}

			IMemberMapper md = this[name];

			if (md == null && _hasClass)
			{
				int idx = name.IndexOf('.');

				if (idx > 0 && idx < name.Length)
				{
					lock (_memberList.SyncRoot)
					{
						string fieldName = name.Substring(0, idx);

						FieldInfo fi = _mappedType.GetField(fieldName);

						if (fi != null)
						{
							IMemberMapper field = 
								GetMemberMapper(fi.FieldType, name.Substring(idx + 1));

							if (field != null)
							{
								md = new FieldMapper().InitField(fi, name, field);
							}
						}
						else
						{
							PropertyInfo pi = _mappedType.GetProperty(fieldName);

							if (pi != null)
							{
								IMemberMapper property = 
									GetMemberMapper(pi.PropertyType, name.Substring(idx + 1));

								if (property != null)
								{
									md = new PropertyMapper().InitProperty(pi, name, property);
								}
							}
						}

						if (md == null)
						{
							md = new FieldMapper().InitField(name);

							lock (_memberList.SyncRoot)
							{
								_memberList.Add(md);
								_memberTable[name.ToLower()] = md;
							}

						}
						else
						{
							this[name] = md;
						}
					}
				}
			}

			return md == null? -1: md.Order;
		}

		private static IMemberMapper GetMemberMapper(
			Type   type,
			string memberName)
		{
			MapDescriptor md = MapDescriptor.GetDescriptor(type);

            if (md == null)
				return null;

			int n = ((IMapDataReceiver)md).GetOrdinal(memberName);

			return n >= 0? (IMemberMapper)md.MemberList[n]: null;
		}

		void IMapDataReceiver.SetFieldValue(int i, string name, object entity, object value)
		{
			try
			{
				((IMemberMapper)_memberList[i]).SetValue(entity, value);
			}
			catch (Exception ex)
			{
				throw new RsdnMapException(
					string.Format("Cannot assign value '{0}' of '{1}' type to '{2}.{3}'. {4}",
					value, 
					value != null? value.GetType().Name: "null",
					entity.GetType().Name,
					name,
					ex.Message),
					ex);
			}
		}
		#endregion

		#region GetDescriptor

		private static Hashtable _typeList       = Hashtable.Synchronized(new Hashtable(10));
		private static Hashtable _processingList = new Hashtable();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static MapDescriptor GetDescriptor(Type type)
		{
			try
			{
				MapDescriptor desc = (MapDescriptor)_typeList[type];

				if (desc == null)
				{
					lock (_typeList.SyncRoot)
					{
						desc = (MapDescriptor)_typeList[type];

						if (desc == null)
						{
							if (_processingList[type] != null)
								return null;

							_processingList.Add(type, type);

							desc = MapEmit.CreateDescriptor(type);
							SetDescriptor(type, desc);

							if (desc.MappedType != type)
								SetDescriptor(desc.MappedType, desc);

							_processingList.Remove(type);
						}
					}
				}

				return desc;
			}
			catch (Exception ex)
			{
				throw new RsdnMapException(
					string.Format("Could not create descriptor of the '{0}' type. {1}",
					type.FullName, ex.Message),
					ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="desc"></param>
		public static void SetDescriptor(Type type, MapDescriptor desc)
		{
			_typeList[type] = desc;
		}

		#endregion

		#region GetValueAttributes

		private static Hashtable _valueAttributeList = Hashtable.Synchronized(new Hashtable(10));

		internal static Attribute[] GetValueAttributes(Type type)
		{
			try
			{
				Attribute[] attrs = (Attribute[])_valueAttributeList[type];

				if (attrs == null)
				{
					lock (_valueAttributeList.SyncRoot)
					{
						attrs = (MapValueAttribute[])_valueAttributeList[type];

						if (attrs == null)
						{
							attrs = new MappingSchema(_defaultSchemaNamespace, null, _defaultSchemaDocument).
								GetValueTypeAttributes(type);

							_valueAttributeList[type] = attrs;
						}
					}
				}

				return attrs;
			}
			catch (Exception ex)
			{
				throw new RsdnMapException(
					string.Format("Could not get value attributes of the '{0}' type. {1}",
					type.FullName, ex.Message),
					ex);
			}
		}
		#endregion

		#region Mapping Xml Schema

		private static XmlDocument GetMappingSchema(string schemaFile, Assembly assembly)
		{
			StreamReader streamReader = null;

			try
			{
				if (File.Exists(schemaFile))
				{
					streamReader = File.OpenText(schemaFile);
				}
				else
				{
					string combinePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, schemaFile);

					if (File.Exists(combinePath))
						streamReader = File.OpenText(combinePath);
				}

				bool embedded = streamReader == null;
				
				Stream stream   = embedded?
					assembly.GetManifestResourceStream(schemaFile):
					streamReader.BaseStream;

				if (stream == null)
					throw new RsdnMapException(
						string.Format("Could not find file '{0}'.", schemaFile));

				using (stream)
				{
					return GetMappingSchema(stream);
				}
			} 
			finally
			{
				if (streamReader != null)
					streamReader.Close();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <include file="Examples.xml" path='examples/desc[@name="SetMappingSchema(string)"]/*' />
		/// <param name="schemaFile"></param>
		public static void SetMappingSchema(string schemaFile)
		{
			try
			{
				SetMappingSchema(GetMappingSchema(schemaFile, Assembly. GetCallingAssembly()));
			} 
			catch (Exception ex)
			{
				throw new RsdnMapException(string.Format(
					"Error during parsing the '{0}' file: {1}", schemaFile, ex.Message),
					ex);
			}
		}

		private static XmlDocument GetMappingSchema(Stream schemaStream)
		{
#if VER2
			XmlSchemaSet schema = new XmlSchemaSet();
#else
			XmlSchemaCollection schema = new XmlSchemaCollection();
#endif

			string resourceName = "Rsdn.Framework.Data.Mapping.Mapping.xsd";
			Stream mapSchema    = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

			if (mapSchema == null)
				throw new RsdnMapException(
					string.Format("Cannot load embedded resource '{0}'", resourceName));

			schema.Add(XmlSchema.Read(mapSchema, null));

#if VER2
			XmlReader reader = XmlReader.Create(schemaStream);

			reader.Settings.Schemas.Add(schema);
#else
			XmlValidatingReader reader = new XmlValidatingReader(new XmlTextReader(schemaStream));

			reader.ValidationType = ValidationType.Schema;
			reader.Schemas.Add(schema);
#endif

			XmlDocument doc = new XmlDocument();

			doc.Load(reader);

			return doc;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="schemaStream"></param>
		public static void SetMappingSchema(Stream schemaStream)
		{
			try
			{
				SetMappingSchema(GetMappingSchema(schemaStream));
			} 
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private static XmlDocument GetMappingSchemaX(XmlDocument mappingSchema)
		{
			if (mappingSchema.DocumentElement.NamespaceURI == _namespaceUri)
			{
				_defaultSchemaNamespace = new XmlNamespaceManager(mappingSchema.NameTable);
				_defaultSchemaNamespace.AddNamespace("m", _namespaceUri);
			}

			return mappingSchema;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mappingSchema"></param>
		public static void SetMappingSchema(XmlDocument mappingSchema)
		{
			try
			{
				if (mappingSchema.DocumentElement.NamespaceURI == _namespaceUri)
				{
					_defaultSchemaNamespace = new XmlNamespaceManager(mappingSchema.NameTable);
					_defaultSchemaNamespace.AddNamespace("m", _namespaceUri);
				}

				_defaultSchemaDocument = mappingSchema;
			} 
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private const  string _namespaceUri = "http://www.rsdn.ru/mapping.xsd";
		private static XmlNamespaceManager _defaultSchemaNamespace;
		private static XmlDocument         _defaultSchemaDocument;

		private MappingSchema GetMappingSchema()
		{
			XmlDocument         doc  = _defaultSchemaDocument;
			XmlNamespaceManager nmsp = _defaultSchemaNamespace;

			if (XmlAttribute != null)
			{
				if (XmlAttribute.FileName != null)
				{
					doc  = GetMappingSchema(XmlAttribute.FileName, OriginalType.Assembly);
					nmsp = null;

					if (doc != null && doc.DocumentElement.NamespaceURI == _namespaceUri)
					{
						nmsp = new XmlNamespaceManager(doc.NameTable);
						nmsp.AddNamespace("m", _namespaceUri);
					}
				}

				if (doc == null)
				{
					throw new RsdnMapException(string.Format(
						"Could not find xml file '{0}' for the '{1}' type.",
						XmlAttribute.FileName,
						OriginalType.FullName));
				}
			}

			if (doc != null)
			{
				XmlNode node;
				string  name;
				string  xPath;

				if (XmlAttribute != null && XmlAttribute.XPath != null)
				{
					name = xPath = XmlAttribute.XPath;
				}
				else
				{
					name = XmlAttribute != null && XmlAttribute.TypeName != null?
						XmlAttribute.TypeName: OriginalType.FullName.Replace("+", ".");

					xPath = string.Format(nmsp != null? "m:type[@name='{0}']": "type[@name='{0}']", name);
				}

				if (nmsp != null)
				{
					node = doc.DocumentElement.SelectSingleNode(xPath, nmsp);
				}
				else
				{
					node = doc.DocumentElement.SelectSingleNode(xPath);
				}

				if (node == null && XmlAttribute != null)
				{
					throw new RsdnMapException(string.Format(
						"Could not find node \"{0}\" for the '{1}' type.",
						xPath,
						OriginalType.FullName));
				}

				return new MappingSchema(nmsp, node, doc);
			}

			return new MappingSchema();
		}
		#endregion

		#region ICloneable Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			MapDescriptor desc = new MapDescriptor();

			desc._mappedType  = _mappedType;
			desc._hasClass    = _hasClass;

			foreach (string key in _memberTable.Keys)
			{
				desc[key] = this[key].Clone() as IMemberMapper;
			}

			return desc;
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return _memberList.GetEnumerator();
		}

		#endregion
	}
}
