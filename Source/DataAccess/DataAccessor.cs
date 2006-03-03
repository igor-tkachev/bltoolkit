using System;
using System.Collections;
#if FW2
using System.Collections.Generic;
#endif
using System.Data;
using System.Text;

using BLToolkit.Data;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

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
			return _dbManager != null? _dbManager: new DbManager();
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
					sprocName = GetDefaultSpName(type.Name, actionName);

				_actionSproc[key] = sprocName;
			}

			return sprocName;
		}

		protected virtual string GetTableName(Type type)
		{
			object[] attrs = type.GetCustomAttributes(typeof(TableNameAttribute), true);

			if (attrs.Length > 0)
				return ((TableNameAttribute)attrs[0]).Name;

			return type.Name;
		}

		private           MappingSchema _mappingSchema = Map.DefaultSchema;
		protected virtual MappingSchema  MappingSchema
		{
			get { return _mappingSchema; }
			set { _mappingSchema = value != null? value: Map.DefaultSchema; }
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
				if (mm.MemberAccessor.Type.IsValueType || mm.MemberAccessor.Type == typeof(string))
					list.Add(mm);

			return (MemberMapper[])list.ToArray(typeof(MemberMapper));
		}

		protected MemberMapper[] GetNonKeyFieldList(ObjectMapper om)
		{
			ArrayList list = new ArrayList();

			foreach (MemberMapper mm in om)
			{
				MemberAccessor ma = mm.MemberAccessor;

				if ((ma.Type.IsValueType || ma.Type == typeof(string)) &&
					ma.GetAttributes(typeof(PrimaryKeyAttribute)) == null)
				{
					list.Add(mm);
				}
			}

			return (MemberMapper[])list.ToArray(typeof(MemberMapper));
		}

		class PrimaryKeyComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				object[] ax = ((MemberMapper)x).MemberAccessor.GetAttributes(typeof(PrimaryKeyAttribute));
				object[] ay = ((MemberMapper)y).MemberAccessor.GetAttributes(typeof(PrimaryKeyAttribute));

				int ix = ((PrimaryKeyAttribute)ax[0]).Order;
				int iy = ((PrimaryKeyAttribute)ay[0]).Order;

				return ix - iy;
			}
		}

		private static PrimaryKeyComparer _primaryKeyComparer = new PrimaryKeyComparer();
		private static Hashtable          _keyList            = new Hashtable();

		protected MemberMapper[] GetKeyFieldList(Type type)
		{
			MemberMapper[] mmList = (MemberMapper[])_keyList[type];

			if (mmList == null)
			{
				ArrayList list = new ArrayList();

				foreach (MemberMapper mm in MappingSchema.GetObjectMapper(type))
				{
					MemberAccessor ma = mm.MemberAccessor;

					if ((ma.Type.IsValueType || ma.Type == typeof(string)) &&
						ma.GetAttributes(typeof(PrimaryKeyAttribute)) != null)
					{
						list.Add(mm);
					}
				}

				list.Sort(_primaryKeyComparer);

				mmList = (MemberMapper[])list.ToArray(typeof(MemberMapper));
				_keyList[type] = mmList;
			}

			return mmList;
		}

		protected string[] GetFieldNameList(MemberMapper[] mm)
		{
			string[] list = new string[mm.Length];

			for (int i = 0; i < mm.Length; i++)
				list[i] = mm[i].Name;

			return list;
		}

		protected void AddWherePK(DbManager db, StringBuilder sb, Type type)
		{
			sb.Append("WHERE\n");

			foreach (string s in GetFieldNameList(GetKeyFieldList(type)))
				sb.AppendFormat("\t[{0}] = {1} AND\n", s, db.DataProvider.GetParameterName(s));

			sb.Remove(sb.Length - 5, 5);
		}

		protected string CreateSelectByKeySqlText(DbManager db, Type type)
		{
			ObjectMapper  om = MappingSchema.GetObjectMapper(type);
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT\n");

			foreach (string s in GetFieldNameList(GetFieldList(om)))
				sb.AppendFormat("\t[{0}],\n", s);
			
			sb.Remove(sb.Length - 2, 1);

			sb.AppendFormat("FROM\n\t[{0}]\n", GetTableName(type));

			AddWherePK(db, sb, type);

			return sb.ToString();
		}

		protected string CreateSelectAllSqlText(DbManager db, Type type)
		{
			ObjectMapper  om = MappingSchema.GetObjectMapper(type);
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT\n");

			foreach (string s in GetFieldNameList(GetFieldList(om)))
				sb.AppendFormat("\t[{0}],\n", s);

			sb.Remove(sb.Length - 2, 1);

			sb.AppendFormat("FROM\n\t[{0}]", GetTableName(type));

			return sb.ToString();
		}

		protected string CreateInsertSqlText(DbManager db, Type type)
		{
			ObjectMapper  om = MappingSchema.GetObjectMapper(type);
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("INSERT INTO [{0}] (\n", GetTableName(type));

			//foreach (MemberMapper mm in GetNonKeyFieldList(om))
			foreach (MemberMapper mm in GetFieldList(om))
				if (mm.MemberAccessor.GetAttributes(typeof(NonUpdatableAttribute)) == null)
					sb.AppendFormat("\t[{0}],\n", mm.Name);

			sb.Remove(sb.Length - 2, 1);

			sb.Append(") VALUES (\n");

			//foreach (MemberMapper mm in GetNonKeyFieldList(om))
			foreach (MemberMapper mm in GetFieldList(om))
				if (mm.MemberAccessor.GetAttributes(typeof(NonUpdatableAttribute)) == null)
					sb.AppendFormat("\t{0},\n", db.DataProvider.GetParameterName(mm.Name));

			sb.Remove(sb.Length - 2, 1);

			sb.Append(")");

			return sb.ToString();
		}

		protected string CreateUpdateSqlText(DbManager db, Type type)
		{
			ObjectMapper  om = MappingSchema.GetObjectMapper(type);
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("UPDATE\n\t[{0}]\nSET\n", GetTableName(type));

			//foreach (MemberMapper mm in GetNonKeyFieldList(om))
			foreach (MemberMapper mm in GetFieldList(om))
				if (mm.MemberAccessor.GetAttributes(typeof(NonUpdatableAttribute)) == null)
					sb.AppendFormat("\t[{0}] = {1},\n", mm.Name, db.DataProvider.GetParameterName(mm.Name));

			sb.Remove(sb.Length - 2, 1);

			AddWherePK(db, sb, type);

			return sb.ToString();
		}

		protected string CreateDeleteSqlText(DbManager db, Type type)
		{
			ObjectMapper  om = MappingSchema.GetObjectMapper(type);
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("DELETE FROM\n\t[{0}]\n", GetTableName(type));

			AddWherePK(db, sb, type);

			return sb.ToString();
		}

		protected virtual string CreateSqlText(DbManager db, Type type, string actionName)
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

		private static Hashtable _actionSql = new Hashtable();

		protected virtual string GetSqlText(DbManager db, Type type, string actionName)
		{
			string key = type.Name + "$" + actionName;
			string sql = (string)_actionSql[key];

			if (sql == null)
			{
				sql = CreateSqlText(db, type, actionName);
				_actionSql[key] = sql;
			}

			return sql;
		}

		protected virtual IDbDataParameter[] GetParameters(DbManager db, Type type, object[] key)
		{
			string[] names = GetFieldNameList(GetKeyFieldList(type));

			if (names.Length != key.Length)
				throw new DataAccessException("Parameter list does match key list.");

			if (key.Length == 1)
			{
				return new IDbDataParameter[] {
					db.Parameter(db.DataProvider.GetParameterName(names[0]), key[0])
				};
			}
			else
			{
				ArrayList list = new ArrayList();

				for (int i = 0; i < names.Length; i++)
					list.Add(db.Parameter(db.DataProvider.GetParameterName(names[i]), key[i]));

				return (IDbDataParameter[])list.ToArray(typeof(IDbDataParameter));
			}
		}

			#endregion

			#region SelectByKey

		public object SelectByKeySql(DbManager db, Type type, params object[] keys)
		{
			return db
				.SetCommand(
					GetSqlText(db, type, "SelectByKey"),
					GetParameters(db, type, keys))
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
			return db
				.SetCommand(GetSqlText(db, type, "SelectAll"))
				.ExecuteList(type);
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

#if FW2
		public List<T> SelectAllSql<T>(DbManager db)
		{
			return db
				.SetCommand(GetSqlText(db, typeof(T), "SelectAll"))
				.ExecuteList<T>();
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
#endif

			#endregion

			#region Insert

		public void InsertSql(DbManager db, object obj)
		{
			db
			  .SetCommand(
					GetSqlText(db, obj.GetType(), "Insert"),
					db.CreateParameters(obj))
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
			return db
				.SetCommand(
					GetSqlText(db, obj.GetType(), "Update"),
					db.CreateParameters(obj))
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
			return db
				.SetCommand(
					GetSqlText(db, type, "Delete"), 
					GetParameters(db, type, key))
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
			ArrayList list = new ArrayList();

			foreach (MemberMapper mm in GetKeyFieldList((obj.GetType())))
				list.Add(db.Parameter(db.DataProvider.GetParameterName(mm.Name), mm.GetValue(obj)));

			return db
				.SetCommand(
					GetSqlText(db, obj.GetType(), "Delete"),
					(IDbDataParameter[])list.ToArray(typeof(IDbDataParameter)))
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
	}
}
