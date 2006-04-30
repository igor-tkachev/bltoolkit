using System;
using System.Collections;
#if FW2
using System.Collections.Generic;
#endif
using System.Data;
using System.Data.SqlTypes;
using System.Text;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.DataAccess
{
	[DataAccessor]
	public class DataAccessor
	{
		#region Constructors

		public DataAccessor()
		{
		}

		public DataAccessor(DbManager dbManager)
		{
			SetDbManager(dbManager);
		}

		#endregion

		#region Public Members

		public virtual DbManager GetDbManager()
		{
			return _dbManager != null? _dbManager: CreateDbManager();
		}

		protected virtual DbManager CreateDbManager()
		{
			return new DbManager();
		}

		public virtual void BeginTransaction()
		{
			if (_dbManager == null)
				throw new InvalidOperationException("DbManager object is not provided.");

			_dbManager.BeginTransaction();
		}

		public virtual void BeginTransaction(IsolationLevel il)
		{
			if (_dbManager == null)
				throw new InvalidOperationException("DbManager object is not provided.");

			_dbManager.BeginTransaction(il);
		}

		public virtual void CommitTransaction()
		{
			if (_dbManager == null)
				throw new InvalidOperationException("DbManager object is not provided.");

			_dbManager.CommitTransaction();
		}

		private ExtensionList _extensions;
		public  ExtensionList  Extensions
		{
			get { return _extensions;  }
			set { _extensions = value; }
		}

		#endregion

		#region CreateInstance

		public static DataAccessor CreateInstance(Type type)
		{
			return (DataAccessor)TypeAccessor.CreateInstance(type);
		}

		public static DataAccessor CreateInstance(Type type, DbManager dbManager)
		{
			DataAccessor da = CreateInstance(type);

			da.SetDbManager(dbManager);

			return da;
		}

#if FW2
		public static T CreateInstance<T>() where T : DataAccessor
		{
			return TypeAccessor<T>.CreateInstanceEx();
		}

		public static T CreateInstance<T>(DbManager dbManager) where T : DataAccessor
		{
			T da = TypeAccessor<T>.CreateInstanceEx();

			da.SetDbManager(dbManager);

			return da;
		}
#endif

		#endregion

		#region Protected Members

		private DbManager _dbManager;

		protected internal void SetDbManager(DbManager dbManager)
		{
			_dbManager = dbManager;
		}

		protected virtual bool DisposeDbManager
		{
			get { return _dbManager == null; }
		}

		protected virtual string GetDefaultSpName(string typeName, string actionName)
		{
			return typeName == null?
				actionName:
				string.Format("{0}_{1}", typeName, actionName);
		}

		private static Hashtable _actionSproc = new Hashtable();

		protected virtual string GetSpName(Type type, string actionName)
		{
			if (type == null)
				return GetDefaultSpName((string)null, actionName);

			string key       = type.Name + "$" + actionName;
			string sprocName = (string)_actionSproc[key];

			if (sprocName == null)
			{
				object[] attrs = type.GetCustomAttributes(typeof(ActionSprocNameAttribute), true);

				foreach (ActionSprocNameAttribute attr in attrs)
				{
					if (attr.ActionName == actionName)
					{
						sprocName = attr.ProcedureName;
						break;
					}
				}

				if (sprocName == null)
					sprocName = GetDefaultSpName(GetTableName(type), actionName);

				_actionSproc[key] = sprocName;
			}

			return sprocName;
		}

		protected virtual IDbDataParameter[] PrepareParameters(object[] parameters)
		{
			// Little optimization.
			// Check if we have only one single ref parameter.
			//
			object refParam = null;

			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i] != null)
				{
					if (refParam != null)
					{
						refParam = null;
						break;
					}

					refParam = parameters[i];
				}
			}

			if (refParam != null)
				return (IDbDataParameter[])refParam;

			ArrayList list = new ArrayList(parameters.Length);
			Hashtable hash = new Hashtable(parameters.Length);

			foreach (object o in parameters)
			{
				IDbDataParameter p = o as IDbDataParameter;

				if (p != null)
				{
					if (!hash.Contains(p.ParameterName))
					{
						list.Add(p);
						hash.Add(p.ParameterName, p);
					}
				}
				else if (o is IDbDataParameter[])
				{
					foreach (IDbDataParameter dbp in (IDbDataParameter[])o)
					{
						if (!hash.Contains(dbp.ParameterName))
						{
							list.Add(dbp);
							hash.Add(dbp.ParameterName, dbp);
						}
					}
				}
			}

			IDbDataParameter[] retParams = new IDbDataParameter[list.Count];

			list.CopyTo(retParams);

			return retParams;
		}

		protected virtual string GetTableName(Type type)
		{
			TypeExtension typeExt = TypeExtension.GetTypeExtenstion(type, Extensions);

			object value = typeExt.Attributes["TableName"].Value;

			if (value != null)
				return value.ToString();

			object[] attrs = type.GetCustomAttributes(typeof(TableNameAttribute), true);

			if (attrs.Length > 0)
				return ((TableNameAttribute)attrs[0]).Name;

			return type.Name;
		}

		protected void ExecuteDictionary(
			DbManager   db,
			IDictionary dictionary,
			Type        objectType,
			Type        keyType,
			string      methodName,
			string      scalarFieldName,
			Type        elementType)
		{
			bool           isIndex = TypeHelper.IsSameOrParent(typeof(IndexValue), keyType);
			MemberMapper[] mms     = GetKeyFieldList(db, objectType);

			if (mms.Length == 0)
				throw new DataAccessException(string.Format(
					"Index is not defined for the method '{0}.{1}'.",
					GetType().Name, methodName));

			if (mms.Length > 1 && keyType != typeof(object) && !isIndex)
				throw new DataAccessException(string.Format(
					"Key type for the method '{0}.{1}' can be of type object or IndexValue.",
					GetType().Name, methodName));

			if (isIndex || mms.Length > 1)
			{
				string[] fields = new string[mms.Length];

				if (scalarFieldName.Length == 0)
					for (int i = 0; i < mms.Length; i++)
						fields[i] = mms[i].MemberName;
				else
					for (int i = 0; i < mms.Length; i++)
						fields[i] = mms[i].Name;

				if (scalarFieldName.Length > 0)
					db.ExecuteScalarDictionary(dictionary, new MapIndex(fields), scalarFieldName, elementType);
				else
					db.ExecuteDictionary(dictionary, new MapIndex(fields), objectType, null);
			}
			else
			{
				if (scalarFieldName.Length > 0)
					db.ExecuteScalarDictionary(
						dictionary,
						scalarFieldName.Length == 0? mms[0].MemberName: mms[0].Name,
						keyType,
						scalarFieldName,
						elementType);
				else
					db.ExecuteDictionary(dictionary, mms[0].MemberName, objectType, null);
			}
		}

		#endregion

		#region CRUDL (SP)

			#region SelectByKey

		public object SelectByKey(DbManager db, Type type, params object[] key)
		{
			return db
				.SetSpCommand(GetSpName(type, "SelectByKey"), key)
				.ExecuteObject(type);
		}

		public object SelectByKey(Type type, params object[] key)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectByKey(db, type, key);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

#if FW2
		public T SelectByKey<T>(DbManager db, params object[] key)
		{
			return (T)SelectByKey(db, typeof(T), key);
		}

		public T SelectByKey<T>(params object[] key)
		{
			return (T)SelectByKey(typeof(T), key);
		}
#endif

			#endregion

			#region SelectAll

		public ArrayList SelectAll(DbManager db, Type type)
		{
			return db
				.SetSpCommand(GetSpName(type, "SelectAll"))
				.ExecuteList(type);
		}

		public ArrayList SelectAll(Type type)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectAll(db, type);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

#if FW2
		public List<T> SelectAll<T>(DbManager db)
		{
			return db
				.SetSpCommand(GetSpName(typeof(T), "SelectAll"))
				.ExecuteList<T>();
		}

		public List<T> SelectAll<T>()
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectAll<T>(db);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}
#endif

			#endregion

			#region Insert

		public void Insert(DbManager db, object obj)
		{
			db
			  .SetSpCommand(
					GetSpName(obj.GetType(), "Insert"),
					db.CreateParameters(obj))
			  .ExecuteNonQuery();
		}

		public void Insert(object obj)
		{
			DbManager db = GetDbManager();

			try
			{
				Insert(db, obj);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

			#endregion

			#region Update

		public int Update(DbManager db, object obj)
		{
			return db
				.SetSpCommand(
					GetSpName(obj.GetType(), "Update"),
					db.CreateParameters(obj))
				.ExecuteNonQuery();
		}

		public int Update(object obj)
		{
			DbManager db = GetDbManager();

			try
			{
				return Update(db, obj);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

			#endregion

			#region DeleteByKey

		public int DeleteByKey(DbManager db, Type type, params object[] key)
		{
			return db
				.SetSpCommand(GetSpName(type, "Delete"), key)
				.ExecuteNonQuery();
		}

		public int DeleteByKey(Type type, params object[] key)
		{
			DbManager db = GetDbManager();

			try
			{
				return DeleteByKey(db, type, key);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

#if FW2
		public int DeleteByKey<T>(DbManager db, params object[] key)
		{
			return DeleteByKey(db, typeof(T), key);
		}

		public int DeleteByKey<T>(params object[] key)
		{
			return DeleteByKey(typeof(T), key);
		}
#endif

			#endregion

			#region Delete

		public int Delete(DbManager db, object obj)
		{
			return db
				.SetSpCommand(
					GetSpName(obj.GetType(), "Delete"), 
					db.CreateParameters(obj))
				.ExecuteNonQuery();
		}

		public int Delete(object obj)
		{
			DbManager db = GetDbManager();

			try
			{
				return Delete(db, obj);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

			#endregion

		#endregion

		#region CRUDL (SQL)

			#region Protected Members

		protected MemberMapper[] GetFieldList(ObjectMapper om)
		{
			ArrayList list = new ArrayList();

			foreach (MemberMapper mm in om)
				list.Add(mm);

			return (MemberMapper[])list.ToArray(typeof(MemberMapper));
		}

		protected MemberMapper[] GetNonKeyFieldList(ObjectMapper om)
		{
			TypeExtension typeExt = TypeExtension.GetTypeExtenstion(om.TypeAccessor.OriginalType, Extensions);
			ArrayList     list    = new ArrayList();

			foreach (MemberMapper mm in om)
			{
				MemberAccessor ma = mm.MemberAccessor;

				if (typeExt[ma.Name]["PrimaryKey"].Value          == null &&
					ma.GetAttributes(typeof(PrimaryKeyAttribute)) == null)
				{
					list.Add(mm);
				}
			}

			return (MemberMapper[])list.ToArray(typeof(MemberMapper));
		}

		class MemberOrder
		{
			public MemberOrder(MemberMapper memberMapper, int order)
			{
				MemberMapper = memberMapper;
				Order        = order;
			}

			public MemberMapper MemberMapper;
			public int          Order;
		}

		class PrimaryKeyComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				return ((MemberOrder)x).Order - ((MemberOrder)y).Order;
			}
		}

		private static PrimaryKeyComparer _primaryKeyComparer = new PrimaryKeyComparer();
		private static Hashtable          _keyList            = new Hashtable();

		protected MemberMapper[] GetKeyFieldList(DbManager db, Type type)
		{
			string         key    = type.FullName + "$" + db.DataProvider.Name;
			MemberMapper[] mmList = (MemberMapper[])_keyList[key];

			if (mmList == null)
			{
				TypeExtension typeExt = TypeExtension.GetTypeExtenstion(type, Extensions);
				ArrayList     list    = new ArrayList();

				foreach (MemberMapper mm in db.MappingSchema.GetObjectMapper(type))
				{
					MemberAccessor ma = mm.MemberAccessor;

					if (TypeHelper.IsScalar(ma.Type))
					{
						object value = typeExt[ma.Name]["PrimaryKey"].Value;

						if (value != null)
						{
							list.Add(new MemberOrder(mm, (int)TypeExtension.ChangeType(value, typeof(int))));
						}
						else
						{
							object[] attrs = ma.GetAttributes(typeof(PrimaryKeyAttribute));

							if (attrs != null)
								list.Add(new MemberOrder(mm, ((PrimaryKeyAttribute)attrs[0]).Order));
						}
					}
				}

				list.Sort(_primaryKeyComparer);

				_keyList[key] = mmList = new MemberMapper[list.Count];

				for (int i = 0; i < list.Count; i++)
					mmList[i] = ((MemberOrder)list[i]).MemberMapper;
			}

			return mmList;
		}

		[Obsolete]
		protected string[] GetFieldNameList(MemberMapper[] mm)
		{
			string[] list = new string[mm.Length];

			for (int i = 0; i < mm.Length; i++)
				list[i] = mm[i].Name;

			return list;
		}

		protected void AddWherePK(DbManager db, SqlQueryInfo query, StringBuilder sb)
		{
			sb.Append("WHERE\n");

			MemberMapper[] memberMappers = GetKeyFieldList(db, query.ObjectType);

			foreach (MemberMapper mm in memberMappers)
			{
				SqlQueryParameterInfo p = query.AddParameter(
					db.DataProvider.Convert(mm.Name + "_W", ConvertType.NameToQueryParameter).ToString(),
					mm.Name);

				sb.AppendFormat("\t{0} = {1} AND\n",
					db.DataProvider.Convert(p.FieldName, ConvertType.NameToQueryField),
					p.ParameterName);
			}

			sb.Remove(sb.Length - 5, 5);
		}

		protected SqlQueryInfo CreateSelectByKeySqlText(DbManager db, Type type)
		{
			ObjectMapper  om    = db.MappingSchema.GetObjectMapper(type);
			StringBuilder sb    = new StringBuilder();
			SqlQueryInfo  query = new SqlQueryInfo(om);

			sb.Append("SELECT\n");

			foreach (MemberMapper mm in GetFieldList(om))
				sb.AppendFormat("\t{0},\n",
					db.DataProvider.Convert(mm.Name, ConvertType.NameToQueryField));
			
			sb.Remove(sb.Length - 2, 1);

			sb.AppendFormat("FROM\n\t{0}\n",
				db.DataProvider.Convert(GetTableName(type), ConvertType.NameToQueryTable));

			AddWherePK(db, query, sb);

			query.QueryText = sb.ToString();

			return query;
		}

		protected SqlQueryInfo CreateSelectAllSqlText(DbManager db, Type type)
		{
			ObjectMapper  om    = db.MappingSchema.GetObjectMapper(type);
			StringBuilder sb    = new StringBuilder();
			SqlQueryInfo  query = new SqlQueryInfo(om);

			sb.Append("SELECT\n");

			foreach (MemberMapper mm in GetFieldList(om))
				sb.AppendFormat("\t{0},\n",
					db.DataProvider.Convert(mm.Name, ConvertType.NameToQueryField));

			sb.Remove(sb.Length - 2, 1);

			sb.AppendFormat("FROM\n\t{0}",
				db.DataProvider.Convert(GetTableName(type), ConvertType.NameToQueryTable));

			query.QueryText = sb.ToString();

			return query;
		}

		protected SqlQueryInfo CreateInsertSqlText(DbManager db, Type type)
		{
			TypeExtension typeExt = TypeExtension.GetTypeExtenstion(type, Extensions);
			ObjectMapper  om      = db.MappingSchema.GetObjectMapper(type);
			ArrayList     list    = new ArrayList();
			StringBuilder sb      = new StringBuilder();
			SqlQueryInfo  query   = new SqlQueryInfo(om);

			sb.AppendFormat("INSERT INTO {0} (\n",
				db.DataProvider.Convert(GetTableName(type), ConvertType.NameToQueryTable));

			foreach (MemberMapper mm in GetFieldList(om))
			{
				object value = typeExt[mm.MemberAccessor.Name]["NonUpdatable"].Value;

				if ((value != null && (bool)TypeExtension.ChangeType(value, typeof(bool)) == false) ||
					(value == null && mm.MemberAccessor.GetAttributes(typeof(NonUpdatableAttribute)) == null))
				{
					sb.AppendFormat("\t{0},\n",
						db.DataProvider.Convert(mm.Name, ConvertType.NameToQueryField));
					list.Add(mm);
				}
			}

			sb.Remove(sb.Length - 2, 1);

			sb.Append(") VALUES (\n");

			foreach (MemberMapper mm in list)
			{
				SqlQueryParameterInfo p = query.AddParameter(
					db.DataProvider.Convert(mm.Name, ConvertType.NameToQueryParameter).ToString(),
					mm.Name);

				sb.AppendFormat("\t{0},\n", p.ParameterName);
			}

			sb.Remove(sb.Length - 2, 1);

			sb.Append(")");

			query.QueryText = sb.ToString();

			return query;
		}

		protected SqlQueryInfo CreateUpdateSqlText(DbManager db, Type type)
		{
			TypeExtension typeExt = TypeExtension.GetTypeExtenstion(type, Extensions);
			ObjectMapper  om      = db.MappingSchema.GetObjectMapper(type);
			StringBuilder sb      = new StringBuilder();
			SqlQueryInfo  query   = new SqlQueryInfo(om);

			sb.AppendFormat("UPDATE\n\t{0}\nSET\n",
				db.DataProvider.Convert(GetTableName(type), ConvertType.NameToQueryTable));

			foreach (MemberMapper mm in GetFieldList(om))
			{
				object value = typeExt[mm.MemberAccessor.Name]["NonUpdatable"].Value;

				if ((value != null && (bool)TypeExtension.ChangeType(value, typeof(bool)) == false) ||
					(value == null && mm.MemberAccessor.GetAttributes(typeof(NonUpdatableAttribute)) == null))
				{
					SqlQueryParameterInfo p = query.AddParameter(
						db.DataProvider.Convert(mm.Name, ConvertType.NameToQueryParameter).ToString(),
						mm.Name);

					sb.AppendFormat("\t{0} = {1},\n",
						db.DataProvider.Convert(p.FieldName, ConvertType.NameToQueryField),
						p.ParameterName);
				}
			}

			sb.Remove(sb.Length - 2, 1);

			AddWherePK(db, query, sb);

			query.QueryText = sb.ToString();

			return query;
		}

		protected SqlQueryInfo CreateDeleteSqlText(DbManager db, Type type)
		{
			ObjectMapper  om    = db.MappingSchema.GetObjectMapper(type);
			StringBuilder sb    = new StringBuilder();
			SqlQueryInfo  query = new SqlQueryInfo(om);

			sb.AppendFormat("DELETE FROM\n\t{0}\n",
				db.DataProvider.Convert(GetTableName(type), ConvertType.NameToQueryTable));

			AddWherePK(db, query, sb);

			query.QueryText = sb.ToString();

			return query;
		}

		protected virtual SqlQueryInfo CreateSqlText(DbManager db, Type type, string actionName)
		{
			switch (actionName)
			{
				case "SelectByKey": return CreateSelectByKeySqlText(db, type);
				case "SelectAll":   return CreateSelectAllSqlText  (db, type);
				case "Insert":      return CreateInsertSqlText     (db, type);
				case "Update":      return CreateUpdateSqlText     (db, type);
				case "Delete":      return CreateDeleteSqlText     (db, type);
				default:
					throw new DataAccessException(
						string.Format("Unknown action '{0}'.", actionName));
			}
		}

		private static Hashtable _actionSqlQueryInfo = new Hashtable();

		protected virtual SqlQueryInfo GetSqlQueryInfo(DbManager db, Type type, string actionName)
		{
			string       key   = type.FullName + "$" + actionName + "$" + db.DataProvider.Name;
			SqlQueryInfo query = (SqlQueryInfo)_actionSqlQueryInfo[key];

			if (query == null)
			{
				query = CreateSqlText(db, type, actionName);
				_actionSqlQueryInfo[key] = query;
			}

			return query;
		}

			#endregion

			#region SelectByKey

		public object SelectByKeySql(DbManager db, Type type, params object[] keys)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, type, "SelectByKey");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, keys))
				.ExecuteObject(type);
		}

		public object SelectByKeySql(Type type, params object[] keys)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectByKeySql(db, type, keys);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

#if FW2
		public T SelectByKeySql<T>(DbManager db, params object[] keys)
		{
			return (T)SelectByKeySql(db, typeof(T), keys);
		}

		public T SelectByKeySql<T>(params object[] keys)
		{
			return (T)SelectByKeySql(typeof(T), keys);
		}
#endif

			#endregion

			#region SelectAll

		public ArrayList SelectAllSql(DbManager db, Type type)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, type, "SelectAll");

			return db
				.SetCommand(query.QueryText)
				.ExecuteList(type);
		}

		public IList SelectAllSql(DbManager db, IList list, Type type)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, type, "SelectAll");

			return db
				.SetCommand(query.QueryText)
				.ExecuteList(list, type);
		}

		public ArrayList SelectAllSql(Type type)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectAllSql(db, type);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		public IList SelectAllSql(IList list, Type type)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectAllSql(db, list, type);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

#if FW2
		public List<T> SelectAllSql<T>(DbManager db)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, typeof(T), "SelectAll");

			return db
				.SetCommand(query.QueryText)
				.ExecuteList<T>();
		}

		public L SelectAllSql<L, T>(DbManager db, L list)
			where L : IList
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, typeof(T), "SelectAll");

			return (L)db
				.SetCommand(query.QueryText)
				.ExecuteList<L,T>(list);
		}

		public List<T> SelectAllSql<T>()
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectAllSql<T>(db);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		public L SelectAllSql<L, T>(L list)
			where L : IList
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectAllSql<L,T>(db, list);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}
#endif

			#endregion

			#region Insert

		public void InsertSql(DbManager db, object obj)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, obj.GetType(), "Insert");

			db
			  .SetCommand(query.QueryText, query.GetParameters(db, obj))
			  .ExecuteNonQuery();
		}

		public void InsertSql(object obj)
		{
			DbManager db = GetDbManager();

			try
			{
				InsertSql(db, obj);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

			#endregion

			#region Update

		public int UpdateSql(DbManager db, object obj)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, obj.GetType(), "Update");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, obj))
				.ExecuteNonQuery();
		}

		public int UpdateSql(object obj)
		{
			DbManager db = GetDbManager();

			try
			{
				return UpdateSql(db, obj);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

			#endregion

			#region DeleteByKey

		public int DeleteByKeySql(DbManager db, Type type, params object[] key)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, type, "Delete");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, key))
				.ExecuteNonQuery();
		}

		public int DeleteByKeySql(Type type, params object[] key)
		{
			DbManager db = GetDbManager();

			try
			{
				return DeleteByKeySql(db, type, key);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

#if FW2
		public int DeleteByKeySql<T>(DbManager db, params object[] key)
		{
			return DeleteByKeySql(db, typeof(T), key);
		}

		public int DeleteByKeySql<T>(params object[] key)
		{
			return DeleteByKeySql(typeof(T), key);
		}
#endif

			#endregion

			#region Delete

		public int DeleteSql(DbManager db, object obj)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, obj.GetType(), "Delete");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, obj))
				.ExecuteNonQuery();
		}

		public int DeleteSql(object obj)
		{
			DbManager db = GetDbManager();

			try
			{
				return DeleteSql(db, obj);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

			#endregion

		#endregion

		#region Convert

		#region Primitive Types

		[CLSCompliant(false)]
		protected virtual SByte ConvertToSByte(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSByte(value);
		}

		protected virtual Int16 ConvertToInt16(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToInt16(value);
		}

		protected virtual Int32 ConvertToInt32(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToInt32(value);
		}

		protected virtual Int64 ConvertToInt64(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToInt64(value);
		}

		protected virtual Byte ConvertToByte(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToByte(value);
		}

		[CLSCompliant(false)]
		protected virtual UInt16 ConvertToUInt16(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToUInt16(value);
		}

		[CLSCompliant(false)]
		protected virtual UInt32 ConvertToUInt32(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToUInt32(value);
		}

		[CLSCompliant(false)]
		protected virtual UInt64 ConvertToUInt64(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToUInt64(value);
		}

		protected virtual Char ConvertToChar(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToChar(value);
		}

		protected virtual Single ConvertToSingle(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSingle(value);
		}

		protected virtual Double ConvertToDouble(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToDouble(value);
		}

		protected virtual Boolean ConvertToBoolean(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToBoolean(value);
		}

		#endregion

		#region Simple Types

		protected virtual String ConvertToString(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToString(value);
		}

		protected virtual DateTime ConvertToDateTime(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToDateTime(value);
		}

		protected virtual Decimal ConvertToDecimal(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToDecimal(value);
		}

		protected virtual Guid ConvertToGuid(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToGuid(value);
		}

		#endregion

#if FW2

		#region Nullable Types

		protected virtual Int16? ConvertToNullableInt16(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableInt16(value);
		}

		protected virtual Int32? ConvertToNullableInt32(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableInt32(value);
		}

		protected virtual Int64? ConvertToNullableInt64(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableInt64(value);
		}

		protected virtual Byte? ConvertToNullableByte(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableByte(value);
		}

		[CLSCompliant(false)]
		protected virtual UInt16? ConvertToNullableUInt16(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableUInt16(value);
		}

		[CLSCompliant(false)]
		protected virtual UInt32? ConvertToNullableUInt32(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableUInt32(value);
		}

		[CLSCompliant(false)]
		protected virtual UInt64? ConvertToNullableUInt64(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableUInt64(value);
		}

		protected virtual Char? ConvertToNullableChar(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableChar(value);
		}

		protected virtual Double? ConvertToNullableDouble(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableDouble(value);
		}

		protected virtual Single? ConvertToNullableSingle(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableSingle(value);
		}

		protected virtual Boolean? ConvertToNullableBoolean(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableBoolean(value);
		}

		protected virtual DateTime? ConvertToNullableDateTime(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableDateTime(value);
		}

		protected virtual Decimal? ConvertToNullableDecimal(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableDecimal(value);
		}

		protected virtual Guid? ConvertToNullableGuid(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToNullableGuid(value);
		}

		#endregion

#endif

		#region SqlTypes

		protected virtual SqlByte ConvertToSqlByte(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlByte(value);
		}

		protected virtual SqlInt16 ConvertToSqlInt16(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlInt16(value);
		}

		protected virtual SqlInt32 ConvertToSqlInt32(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlInt32(value);
		}

		protected virtual SqlInt64 ConvertToSqlInt64(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlInt64(value);
		}

		protected virtual SqlSingle ConvertToSqlSingle(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlSingle(value);
		}

		protected virtual SqlBoolean ConvertToSqlBoolean(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlBoolean(value);
		}

		protected virtual SqlDouble ConvertToSqlDouble(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlDouble(value);
		}

		protected virtual SqlDateTime ConvertToSqlDateTime(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlDateTime(value);
		}

		protected virtual SqlDecimal ConvertToSqlDecimal(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlDecimal(value);
		}

		protected virtual SqlMoney ConvertToSqlMoney(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlMoney(value);
		}

		protected virtual SqlGuid ConvertToSqlGuid(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlGuid(value);
		}

		protected virtual SqlString ConvertToSqlString(DbManager db, object value, object parameter)
		{
			return db.MappingSchema.ConvertToSqlString(value);
		}

		#endregion

		#endregion
	}
}
