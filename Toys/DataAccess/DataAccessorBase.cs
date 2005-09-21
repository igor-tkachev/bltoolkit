using System;
using System.Collections;
#if VER2
using System.Collections.Generic;
#endif
using System.Data;
using System.Text;

using Rsdn.Framework.Data;
using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.DataAccess
{
	public class DataAccessorBase
	{
		#region Protected Members

		public virtual DbManager GetDbManager()
		{
			return new DbManager();
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
			using (DbManager db = GetDbManager())
				return SelectByKey(db, type, key);
		}

#if VER2
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
			using (DbManager db = GetDbManager())
				return SelectAll(db, type);
		}

#if VER2
		public List<T> SelectAll<T>(DbManager db)
		{
			return db
				.SetSpCommand(GetSpName(typeof(T), "SelectAll"))
				.ExecuteList<T>();
		}

		public List<T> SelectAll<T>()
		{
			using (DbManager db = GetDbManager())
				return SelectAll<T>(db);
		}
#endif

			#endregion

			#region Insert

		public int Insert(DbManager db, object obj)
		{
			return db
			  .SetSpCommand(
					GetSpName(obj.GetType(), "Insert"),
					db.CreateParameters(obj))
			  .ExecuteNonQuery();
		}

		public int Insert(object obj)
		{
			using (DbManager db = GetDbManager())
				return Insert(db, obj);
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
			using (DbManager db = GetDbManager())
				return Update(db, obj);
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
			using (DbManager db = GetDbManager())
				return DeleteByKey(db, type, key);
		}

#if VER2
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
			using (DbManager db = GetDbManager())
				return Delete(db, obj);
		}

			#endregion

		#endregion

		#region CRUDL (SQL)

			#region Protected Members

		protected IMemberMapper[] GetFieldList(MapDescriptor md)
		{
			ArrayList list = new ArrayList();

			foreach (IMemberMapper mm in md)
				if (mm.MemberType.IsValueType || mm.MemberType == typeof(string))
					list.Add(mm);

			return (IMemberMapper[])list.ToArray(typeof(IMemberMapper));
		}

		protected IMemberMapper[] GetNonKeyFieldList(MapDescriptor md)
		{
			ArrayList list = new ArrayList();

			foreach (IMemberMapper mm in md)
			{
				if ((mm.MemberType.IsValueType || mm.MemberType == typeof(string)) &&
					mm.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Length == 0)
				{
					list.Add(mm);
				}
			}

			return (IMemberMapper[])list.ToArray(typeof(IMemberMapper));
		}

		class PrimaryKeyComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				object[] ax = ((IMemberMapper)x).GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
				object[] ay = ((IMemberMapper)y).GetCustomAttributes(typeof(PrimaryKeyAttribute), true);

				int ix = ((PrimaryKeyAttribute)ax[0]).Index;
				int iy = ((PrimaryKeyAttribute)ay[0]).Index;

				return ix - iy;
			}
		}

		private static PrimaryKeyComparer _primaryKeyComparer = new PrimaryKeyComparer();
		private static Hashtable          _keyList            = new Hashtable();

		protected IMemberMapper[] GetKeyFieldList(Type type)
		{
			IMemberMapper[] mmList = (IMemberMapper[])_keyList[type];

			if (mmList == null)
			{
				ArrayList list = new ArrayList();

				foreach (IMemberMapper mm in Map.Descriptor(type))
				{
					if ((mm.MemberType.IsValueType || mm.MemberType == typeof(string)) &&
						mm.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Length != 0)
					{
						list.Add(mm);
					}
				}

				list.Sort(_primaryKeyComparer);

				mmList = (IMemberMapper[])list.ToArray(typeof(IMemberMapper));
				_keyList[type] = mmList;
			}

			return mmList;
		}

		protected string[] GetFieldNameList(IMemberMapper[] mm)
		{
			string[] list = new string[mm.Length];

			for (int i = 0; i < mm.Length; i++)
				list[i] = mm[i].Name;

			return list;
		}

		protected void AddWherePK(StringBuilder sb, Type type)
		{
			sb.Append("WHERE\n");

			foreach (string s in GetFieldNameList(GetKeyFieldList(type)))
				sb.AppendFormat("\t[{0}] = @{0} AND\n", s);

			sb.Remove(sb.Length - 5, 5);
		}

		protected string CreateSelectByKeySqlText(Type type)
		{
			MapDescriptor md = Map.Descriptor(type);
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT\n");

			foreach (string s in GetFieldNameList(GetFieldList(md)))
				sb.AppendFormat("\t[{0}],\n", s);
			
			sb.Remove(sb.Length - 2, 1);

			sb.AppendFormat("FROM\n\t[{0}]\n", GetTableName(type));

			AddWherePK(sb, type);

			return sb.ToString();
		}

		protected string CreateSelectAllSqlText(Type type)
		{
			MapDescriptor md = Map.Descriptor(type);
			StringBuilder sb = new StringBuilder();

			sb.Append("SELECT\n");

			foreach (string s in GetFieldNameList(GetFieldList(md)))
				sb.AppendFormat("\t[{0}],\n", s);

			sb.Remove(sb.Length - 2, 1);

			sb.AppendFormat("FROM\n\t[{0}]", GetTableName(type));

			return sb.ToString();
		}

		protected string CreateInsertSqlText(Type type)
		{
			MapDescriptor md = Map.Descriptor(type);
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("INSERT INTO [{0}] (\n", GetTableName(type));

			foreach (IMemberMapper mm in GetNonKeyFieldList(md))
				if (mm.GetCustomAttributes(typeof(NonUpdatableAttribute), true).Length == 0)
					sb.AppendFormat("\t[{0}],\n", mm.Name);

			sb.Remove(sb.Length - 2, 1);

			sb.Append(") VALUES (\n");

			foreach (IMemberMapper mm in GetNonKeyFieldList(md))
				if (mm.GetCustomAttributes(typeof(NonUpdatableAttribute), true).Length == 0)
					sb.AppendFormat("\t@{0},\n", mm.Name);

			sb.Remove(sb.Length - 2, 1);

			sb.Append(")");

			return sb.ToString();
		}

		protected string CreateUpdateSqlText(Type type)
		{
			MapDescriptor md = Map.Descriptor(type);
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("UPDATE\n\t[{0}]\nSET\n", GetTableName(type));

			foreach (IMemberMapper mm in GetNonKeyFieldList(md))
				if (mm.GetCustomAttributes(typeof(NonUpdatableAttribute), true).Length == 0)
					sb.AppendFormat("\t[{0}] = @{0},\n", mm.Name);

			sb.Remove(sb.Length - 2, 1);

			AddWherePK(sb, type);

			return sb.ToString();
		}

		protected string CreateDeleteSqlText(Type type)
		{
			MapDescriptor md = Map.Descriptor(type);
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("DELETE FROM\n\t[{0}]\n", GetTableName(type));

			AddWherePK(sb, type);

			return sb.ToString();
		}

		protected virtual string CreateSqlText(Type type, string actionName)
		{
			switch (actionName)
			{
				case "SelectByKey": return CreateSelectByKeySqlText(type);
				case "SelectAll":   return CreateSelectAllSqlText  (type);
				case "Insert":      return CreateInsertSqlText     (type);
				case "Update":      return CreateUpdateSqlText     (type);
				case "Delete":      return CreateDeleteSqlText     (type);
				default:
					throw new RsdnDataAccessException(
						string.Format("Unknown action '{0}'.", actionName));
			}
		}

		private static Hashtable _actionSql = new Hashtable();

		protected virtual string GetSqlText(Type type, string actionName)
		{
			string key = type.Name + "$" + actionName;
			string sql = (string)_actionSql[key];

			if (sql == null)
			{
				sql = CreateSqlText(type, actionName);
				_actionSql[key] = sql;
			}

			return sql;
		}

		protected IDbDataParameter[] GetParameters(DbManager db, Type type, object[] key)
		{
			string[] names = GetFieldNameList(GetKeyFieldList(type));

			if (names.Length != key.Length)
				throw new RsdnDataAccessException("Parameter list does match key list.");

			if (key.Length == 1)
			{
				return new IDbDataParameter[] { db.Parameter("@" + names[0], key[0]) };
			}
			else
			{
				ArrayList list = new ArrayList();

				for (int i = 0; i < names.Length; i++)
					list.Add(db.Parameter("@" + names[i], key[i]));

				return (IDbDataParameter[])list.ToArray(typeof(IDbDataParameter));
			}
		}

			#endregion

			#region SelectByKey

		public object SelectByKeySql(DbManager db, Type type, params object[] key)
		{
			return db
				.SetCommand(
					GetSqlText(type, "SelectByKey"),
					GetParameters(db, type, key))
				.ExecuteObject(type);
		}

		public object SelectByKeySql(Type type, object key)
		{
			using (DbManager db = GetDbManager())
				return SelectByKeySql(db, type, key);
		}

#if VER2
		public T SelectByKeySql<T>(DbManager db, object key)
		{
			return (T)SelectByKeySql(db, typeof(T), key);
		}

		public T SelectByKeySql<T>(object key)
		{
			return (T)SelectByKeySql(typeof(T), key);
		}
#endif

			#endregion

			#region SelectAll

		public ArrayList SelectAllSql(DbManager db, Type type)
		{
			return db
				.SetCommand(GetSqlText(type, "SelectAll"))
				.ExecuteList(type);
		}

		public ArrayList SelectAllSql(Type type)
		{
			using (DbManager db = GetDbManager())
				return SelectAllSql(db, type);
		}

#if VER2
		public List<T> SelectAllSql<T>(DbManager db)
		{
			return db
				.SetCommand(GetSqlText(typeof(T), "SelectAll"))
				.ExecuteList<T>();
		}

		public List<T> SelectAllSql<T>()
		{
			using (DbManager db = GetDbManager())
				return SelectAllSql<T>(db);
		}
#endif

			#endregion

			#region Insert

		public int InsertSql(DbManager db, object obj)
		{
			return db
			  .SetCommand(
					GetSqlText(obj.GetType(), "Insert"),
					db.CreateParameters(obj))
			  .ExecuteNonQuery();
		}

		public int InsertSql(object obj)
		{
			using (DbManager db = GetDbManager())
				return InsertSql(db, obj);
		}

			#endregion

			#region Update

		public int UpdateSql(DbManager db, object obj)
		{
			return db
				.SetCommand(
					GetSqlText(obj.GetType(), "Update"),
					db.CreateParameters(obj))
				.ExecuteNonQuery();
		}

		public int UpdateSql(object obj)
		{
			using (DbManager db = GetDbManager())
				return UpdateSql(db, obj);
		}

			#endregion

			#region DeleteByKey

		public int DeleteByKeySql(DbManager db, Type type, params object[] key)
		{
			return db
				.SetCommand(
					GetSqlText(type, "Delete"), 
					GetParameters(db, type, key))
				.ExecuteNonQuery();
		}

		public int DeleteByKeySql(Type type, params object[] key)
		{
			using (DbManager db = GetDbManager())
				return DeleteByKeySql(db, type, key);
		}

#if VER2
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

			foreach (IMemberMapper mm in GetKeyFieldList((obj.GetType())))
				list.Add(db.Parameter("@" + mm.Name, mm.GetValue(obj)));

			return db
				.SetCommand(
					GetSqlText(obj.GetType(), "Delete"),
					(IDbDataParameter[])list.ToArray(typeof(IDbDataParameter)))
				.ExecuteNonQuery();
		}

		public int DeleteSql(object obj)
		{
			using (DbManager db = GetDbManager())
				return DeleteSql(db, obj);
		}

			#endregion

		#endregion
	}
}
