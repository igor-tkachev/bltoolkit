using System;
using System.Collections;
#if VER2
using System.Collections.Generic;
#endif

using Rsdn.Framework.Data;

namespace Rsdn.Framework.DataAccess
{
	public class DataAccessorBase
	{
		#region Protected Members

		protected virtual DbManager GetDbManager()
		{
			return new DbManager();
		}

		protected virtual string GetSpName(string typeName, string actionName)
		{
			return string.Format("{0}_{1}", typeName, actionName);
		}

		private static Hashtable _actionSproc = new Hashtable();

		protected virtual string GetSpName(Type type, string actionName)
		{
			string key       = type.Name + "$" + actionName;
			string sprocName = (string)_actionSproc[key];

			if (sprocName == null)
			{
				object[] attrs = type.GetCustomAttributes(typeof(SprocNameAttribute), true);

				foreach (SprocNameAttribute attr in attrs)
				{
					if (attr.ActionName == actionName)
					{
						sprocName = attr.ProcedureName;
						break;
					}
				}

				if (sprocName == null)
					sprocName = GetSpName(type.Name, actionName);

				_actionSproc[key] = sprocName;
			}

			return sprocName;
		}

		#endregion

		#region CRUDL (SP)

			#region SelectByID

		public object SelectByID(DbManager db, Type type, object id)
		{
			return db
				.SetSpCommand(GetSpName(type, "SelectByID"), id)
				.ExecuteObject(type);
		}

		public object SelectByID(Type type, object id)
		{
			using (DbManager db = GetDbManager())
				return SelectByID(db, type, id);
		}

#if VER2
		public T SelectByID<T>(DbManager db, object id)
		{
			return (T)SelectByID(db, typeof(T), id);
		}

		public T SelectByID<T>(object id)
		{
			return (T)SelectByID(typeof(T), id);
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

			#region Delete

		public int Delete(DbManager db, Type type, object id)
		{
			return db
				.SetSpCommand(GetSpName(type, "Delete"), id)
				.ExecuteNonQuery();
		}

		public int Delete(Type type, object id)
		{
			using (DbManager db = GetDbManager())
				return Delete(db, type, id);
		}

#if VER2
		public int Delete<T>(DbManager db, object id)
		{
			return Delete(db, typeof(T), id);
		}

		public int Delete<T>(object id)
		{
			return Delete(typeof(T), id);
		}
#endif

			#endregion

		#endregion
	}
}
