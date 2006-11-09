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

		[System.Diagnostics.DebuggerStepThrough]
		protected DataAccessBase()
		{
		}

		[System.Diagnostics.DebuggerStepThrough]
		protected DataAccessBase(DbManager dbManager)
		{
			SetDbManager(dbManager, false);
		}

		[System.Diagnostics.DebuggerStepThrough]
		protected DataAccessBase(DbManager dbManager, bool dispose)
		{
			SetDbManager(dbManager, dispose);
		}

		#endregion

		#region Public Members

		[NoInterception, System.Diagnostics.DebuggerStepThrough]
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

		private        bool _disposeDbManager = true;
		[NoInterception]
		public virtual bool  DisposeDbManager
		{
			get { return _disposeDbManager;  }
			set { _disposeDbManager = value; }
		}

		#endregion

		#region Protected Members

		private   DbManager _dbManager;
		protected DbManager  DbManager
		{
			get { return _dbManager; }
		}

		protected internal void SetDbManager(DbManager dbManager, bool dispose)
		{
			_dbManager        = dbManager;
			_disposeDbManager = dispose;
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
			TypeExtension typeExt = TypeExtension.GetTypeExtension(type, Extensions);

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
