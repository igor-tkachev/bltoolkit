using System;
using System.Collections;
using System.Data;

using BLToolkit.Aspects;
using BLToolkit.Data;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.DataAccess
{
	public abstract class DataAccessBase
	{
		#region Constructors

		protected DataAccessBase()
		{
		}

		protected DataAccessBase(DbManager dbManager)
		{
			SetDbManager(dbManager);
		}

		#endregion

		#region Public Members

		[NoInterception]
		public virtual DbManager GetDbManager()
		{
			return _dbManager != null? _dbManager: CreateDbManager();
		}

		[NoInterception]
		protected virtual DbManager CreateDbManager()
		{
			return new DbManager();
		}

		[NoInterception]
		public virtual void BeginTransaction()
		{
			if (_dbManager == null)
				throw new InvalidOperationException("DbManager object is not provided.");

			_dbManager.BeginTransaction();
		}

		[NoInterception]
		public virtual void BeginTransaction(IsolationLevel il)
		{
			if (_dbManager == null)
				throw new InvalidOperationException("DbManager object is not provided.");

			_dbManager.BeginTransaction(il);
		}

		[NoInterception]
		public virtual void CommitTransaction()
		{
			if (_dbManager == null)
				throw new InvalidOperationException("DbManager object is not provided.");

			_dbManager.CommitTransaction();
		}

		[NoInterception]
		public virtual void RollbackTransaction()
		{
			if (_dbManager == null)
				throw new InvalidOperationException("DbManager object is not provided.");

			_dbManager.RollbackTransaction();
		}

		private ExtensionList _extensions;
		public  ExtensionList  Extensions
		{
			get
			{
				return _extensions == null && _dbManager != null ?
					_dbManager.MappingSchema.Extensions :
					_extensions;
			}

			set { _extensions = value; }
		}

		#endregion

		#region Protected Members

		private   DbManager _dbManager;
		protected DbManager  DbManager
		{
			get { return _dbManager; }
		}

		protected internal void SetDbManager(DbManager dbManager)
		{
			_dbManager = dbManager;
		}

		[NoInterception]
		protected virtual bool DisposeDbManager
		{
			get { return _dbManager == null; }
		}

		[NoInterception]
		protected virtual string GetDefaultSpName(string typeName, string actionName)
		{
			return typeName == null?
				actionName:
				string.Format("{0}_{1}", typeName, actionName);
		}

		private static Hashtable _actionSproc = new Hashtable();

		[NoInterception]
		protected virtual string GetSpName(Type type, string actionName)
		{
			if (type == null)
				return GetDefaultSpName(null, actionName);

			string key       = type.FullName + "$" + actionName;
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

		[NoInterception]
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

		#endregion
	}
}
