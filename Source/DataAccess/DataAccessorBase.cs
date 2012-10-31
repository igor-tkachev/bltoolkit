using System;
using System.Collections;
using System.Data;

using BLToolkit.Aspects;
using BLToolkit.Data;
using BLToolkit.Mapping;
using BLToolkit.Properties;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.DataAccess
{
	public abstract class DataAccessorBase
	{
		#region Constructors

		[System.Diagnostics.DebuggerStepThrough]
		protected DataAccessorBase()
		{
		}

		[System.Diagnostics.DebuggerStepThrough]
		protected DataAccessorBase(DbManager dbManager)
		{
			SetDbManager(dbManager, false);
		}

		[System.Diagnostics.DebuggerStepThrough]
		protected DataAccessorBase(DbManager dbManager, bool dispose)
		{
			SetDbManager(dbManager, dispose);
		}

		#endregion

		#region Public Members

		[NoInterception, System.Diagnostics.DebuggerStepThrough]
		public virtual DbManager GetDbManager()
		{
			return _dbManager ?? CreateDbManager();
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
				throw new InvalidOperationException(Resources.DataAccessorBase_NoDbManager);

			_dbManager.BeginTransaction();
		}

		[NoInterception]
		public virtual void BeginTransaction(IsolationLevel il)
		{
			if (_dbManager == null)
				throw new InvalidOperationException(Resources.DataAccessorBase_NoDbManager);

			_dbManager.BeginTransaction(il);
		}

		[NoInterception]
		public virtual void CommitTransaction()
		{
			if (_dbManager == null)
				throw new InvalidOperationException(Resources.DataAccessorBase_NoDbManager);

			_dbManager.CommitTransaction();
		}

		[NoInterception]
		public virtual void RollbackTransaction()
		{
			if (_dbManager == null)
				throw new InvalidOperationException(Resources.DataAccessorBase_NoDbManager);

			_dbManager.RollbackTransaction();
		}

		private ExtensionList _extensions;
		public  ExtensionList  Extensions
		{
			get { return _extensions ?? (_extensions = MappingSchema.Extensions); }
			set { _extensions = value; }
		}

		private        bool _disposeDbManager = true;
		[NoInterception]
		public virtual bool  DisposeDbManager
		{
			get { return _disposeDbManager;  }
			set { _disposeDbManager = value; }
		}

		private MappingSchema _mappingSchema;
		public  MappingSchema  MappingSchema
		{
			get { return _mappingSchema ?? (_mappingSchema = _dbManager != null? _dbManager.MappingSchema: Map.DefaultSchema); }
			set { _mappingSchema = value; }
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

		private static readonly Hashtable _actionSproc = new Hashtable();

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
		protected virtual string GetDatabaseName(Type type)
		{
			bool isSet;
			return MappingSchema.MetadataProvider.GetDatabaseName(type, Extensions, out isSet);
		}

		[NoInterception]
		protected virtual string GetOwnerName(Type type)
		{
			bool isSet;
			return MappingSchema.MetadataProvider.GetOwnerName(type, Extensions, out isSet);
		}

		[NoInterception]
		protected virtual string GetTableName(Type type)
		{
			bool isSet;
			return MappingSchema.MetadataProvider.GetTableName(type, Extensions, out isSet);
		}

		[NoInterception]
		protected virtual void Dispose(DbManager dbManager)
		{
			if (dbManager != null && DisposeDbManager)
				dbManager.Dispose();
		}

		#endregion
	}
}
