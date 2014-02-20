using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using BLToolkit.Aspects;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.DataAccess
{
	public abstract class SqlQueryBase : DataAccessorBase
	{
		#region Constructors

		protected SqlQueryBase()
		{
		}

		protected SqlQueryBase(DbManager dbManager)
			: base(dbManager)
		{
		}

		protected SqlQueryBase(DbManager dbManager, bool dispose)
			: base(dbManager, dispose)
		{
		}

		#endregion

        #region InsertWithIdentity

        public virtual object InsertWithIdentity(DbManager db, object obj)
	    {
	        var query = GetSqlQueryInfo(db, obj.GetType(), "InsertWithIdentity");

	        var parameters = query.GetParameters(db, obj);

	        db.SetCommand(query.QueryText, parameters).ExecuteNonQuery();

	        var outputParameter = parameters.ToList().First(e => e.Direction == ParameterDirection.Output);
	        return outputParameter.Value;
	    }

	    public virtual object InsertWithIdentity(object obj)
	    {
	        var db = GetDbManager();

	        try
	        {
	            return InsertWithIdentity(db, obj);
	        }
	        finally
	        {
	            Dispose(db);
	        }
	    }

	    #endregion

		#region Protected Members

		[NoInterception]
		protected virtual MemberMapper[] GetFieldList(ObjectMapper om)
		{
			var list = new List<MemberMapper>(om.Count);

			foreach (MemberMapper mm in om)
				if (mm.MapMemberInfo.SqlIgnore == false)
					list.Add(mm);

			return list.ToArray();
		}

		[NoInterception]
		protected virtual MemberMapper[] GetNonKeyFieldList(ObjectMapper om)
		{
			var typeExt = TypeExtension.GetTypeExtension(om.TypeAccessor.OriginalType, Extensions);
			var list    = new List<MemberMapper>();

			foreach (MemberMapper mm in om)
			{
				if (mm.MapMemberInfo.SqlIgnore)
					continue;

				var ma = mm.MapMemberInfo.MemberAccessor;

				bool isSet;
				MappingSchema.MetadataProvider.GetPrimaryKeyOrder(om.TypeAccessor.OriginalType, typeExt, ma, out isSet);

				if (!isSet)
					list.Add(mm);
			}

			return list.ToArray();
		}

		struct MemberOrder
		{
			public MemberOrder(MemberMapper memberMapper, int order)
			{
				MemberMapper = memberMapper;
				Order        = order;
			}

			public readonly MemberMapper MemberMapper;
			public readonly int          Order;
		}

		private static readonly Hashtable _keyList = new Hashtable();

		[NoInterception]
		protected internal virtual MemberMapper[] GetKeyFieldList(DbManager db, Type type)
		{
			var key    = type.FullName + "$" + db.DataProvider.UniqueName;
			var mmList = (MemberMapper[])_keyList[key];

			if (mmList == null)
			{
				var typeExt = TypeExtension.GetTypeExtension(type, Extensions);
				var list    = new List<MemberOrder>();

				foreach (MemberMapper mm in db.MappingSchema.GetObjectMapper(type))
				{
					if (mm.MapMemberInfo.SqlIgnore)
						continue;

					var ma = mm.MapMemberInfo.MemberAccessor;

					if (TypeHelper.IsScalar(ma.Type))
					{
						bool isSet;
						var order = MappingSchema.MetadataProvider.GetPrimaryKeyOrder(type, typeExt, ma, out isSet);

						if (isSet)
							list.Add(new MemberOrder(mm, order));
					}
				}

				list.Sort((x, y) => x.Order - y.Order);

				_keyList[key] = mmList = new MemberMapper[list.Count];

				for (var i = 0; i < list.Count; i++)
					mmList[i] = list[i].MemberMapper;
			}

			return mmList;
		}

		[NoInterception]
		protected virtual void AddWherePK(DbManager db, SqlQueryInfo query, StringBuilder sb, int nParameter)
		{
			sb.Append("WHERE\n");

			var memberMappers = GetKeyFieldList(db, query.ObjectType);

			if (memberMappers.Length == 0)
				throw new DataAccessException(
						string.Format("No primary key field(s) in the type '{0}'.", query.ObjectType.FullName));

			foreach (var mm in memberMappers)
			{
				var p = query.AddParameter(
					db.DataProvider.Convert(mm.Name + "_W", ConvertType.NameToQueryParameter).ToString(),
					mm.Name);

				sb.AppendFormat("\t{0} = ", db.DataProvider.Convert(p.FieldName, ConvertType.NameToQueryField));

				if (nParameter < 0)
					sb.AppendFormat("{0} AND\n", p.ParameterName);
				else
					sb.AppendFormat("{{{0}}} AND\n", nParameter++);
			}

			sb.Remove(sb.Length - 5, 5);
		}

		protected SqlQueryInfo CreateSelectByKeySqlText(DbManager db, Type type)
		{
			var om    = db.MappingSchema.GetObjectMapper(type);
			var sb    = new StringBuilder();
			var query = new SqlQueryInfo(om);

			sb.Append("SELECT\n");

			foreach (var mm in GetFieldList(om))
				sb.AppendFormat("\t{0},\n",
					db.DataProvider.Convert(mm.Name, ConvertType.NameToQueryField));

			sb.Remove(sb.Length - 2, 1);

			sb.Append("FROM\n\t");

			AppendTableName(sb, db, type);

			AddWherePK(db, query, sb, -1);

			query.QueryText = sb.ToString();

			return query;
		}

		// NOTE changed to virtual
		protected virtual void AppendTableName(StringBuilder sb, DbManager db, Type type)
		{
			var database = GetDatabaseName(type);
			var owner    = GetOwnerName   (type);
			var name     = GetTableName   (type);

			db.DataProvider.CreateSqlProvider().BuildTableName(sb,
				database == null ? null : db.DataProvider.Convert(database, ConvertType.NameToDatabase).  ToString(),
				owner    == null ? null : db.DataProvider.Convert(owner,    ConvertType.NameToOwner).     ToString(),
				name     == null ? null : db.DataProvider.Convert(name,     ConvertType.NameToQueryTable).ToString());

			sb.AppendLine();
		}

		protected SqlQueryInfo CreateSelectAllSqlText(DbManager db, Type type)
		{
			var om    = db.MappingSchema.GetObjectMapper(type);
			var sb    = new StringBuilder();
			var query = new SqlQueryInfo(om);

			sb.Append("SELECT\n");

			foreach (var mm in GetFieldList(om))
				sb.AppendFormat("\t{0},\n",
					db.DataProvider.Convert(mm.Name, ConvertType.NameToQueryField));

			sb.Remove(sb.Length - 2, 1);

			sb.Append("FROM\n\t");
			AppendTableName(sb, db, type);

			query.QueryText = sb.ToString();

			return query;
		}

		protected SqlQueryInfo CreateInsertSqlText(DbManager db, Type type, int nParameter, bool insertAutoSequence = false)
		{
			var typeExt = TypeExtension.GetTypeExtension(type, Extensions);
			var om      = db.MappingSchema.GetObjectMapper(type);
			var list    = new List<MemberMapper>();
			var sb      = new StringBuilder();
			var query   = new SqlQueryInfo(om);
			var mp      = MappingSchema.MetadataProvider;

			sb.Append("INSERT INTO ");
			AppendTableName(sb, db, type);
			sb.Append(" (\n");

            
			foreach (var mm in GetFieldList(om))
			{
				// IT: This works incorrectly for complex mappers.
				//
				// [2009-03-24] ili: use mm.MemberAccessor instead of mm.ComplexMemberAccessor
				// as in CreateUpdateSqlText
				//


                bool isSet;
				var nonUpdatableAttribute = mp.GetNonUpdatableAttribute(type, typeExt, mm.MapMemberInfo.MemberAccessor, out isSet);
                if (insertAutoSequence && nonUpdatableAttribute is IdentityAttribute)
                {                    
                    sb.AppendFormat("\t{0},\n", db.DataProvider.Convert(mm.Name, ConvertType.NameToQueryField));
                    list.Add(mm);
                }
                else
                {
                    if (nonUpdatableAttribute == null || !isSet || nonUpdatableAttribute.IsIdentity || nonUpdatableAttribute.OnInsert == false)
                    {
                        sb.AppendFormat("\t{0},\n", db.DataProvider.Convert(mm.Name, ConvertType.NameToQueryField));
                        list.Add(mm);
                    }
                }
			}

			sb.Remove(sb.Length - 2, 1);

			sb.Append(") VALUES (\n");

            MemberMapper identityMember = null;
            foreach (var mm in list)
            {
                var keyGenerator = mm.MapMemberInfo.KeyGenerator as SequenceKeyGenerator;
                if (keyGenerator != null && insertAutoSequence)
                {
                    string seqQuery = db.DataProvider.NextSequenceQuery(keyGenerator.Sequence);
                    sb.AppendFormat("\t{0},\n", seqQuery);
                    identityMember = mm;
                }
                else
                {
                    // Previously : mm.Name
                    var p = query.AddParameter(
                        db.DataProvider.Convert(mm.MemberName + "_P", ConvertType.NameToQueryParameter).ToString(),
                        mm.Name);

                    if (nParameter < 0)
                        sb.AppendFormat("\t{0},\n", p.ParameterName);
                        //sb.AppendFormat("\t{0},\n", db.DataProvider.Convert(p.ParameterName, ConvertType.NameToQueryParameter));
                    else
                        sb.AppendFormat("\t{{{0}}},\n", nParameter++);
                }
            }

		    sb.Remove(sb.Length - 2, 1);

			sb.Append(")");

            if (identityMember != null)
            {
                sb.AppendFormat("\r\n{0}", db.DataProvider.GetReturningInto(identityMember.Name));

                query.AddParameter(
                    db.DataProvider.Convert("IDENTITY_PARAMETER", ConvertType.NameToQueryParameter).ToString(),
                    identityMember.Name);
            }

			query.QueryText = sb.ToString();

			return query;
		}

		protected SqlQueryInfo CreateUpdateSqlText(DbManager db, Type type, int nParameter)
		{
			var typeExt = TypeExtension.GetTypeExtension(type, Extensions);
			var om      = db.MappingSchema.GetObjectMapper(type);
			var sb      = new StringBuilder();
			var query   = new SqlQueryInfo(om);
			var mp      = MappingSchema.MetadataProvider;

			sb.Append("UPDATE\n\t");
			AppendTableName(sb, db, type);
			sb.Append("\nSET\n");

			var fields    = GetFieldList(om);
			var hasFields = false;

			foreach (var mm in fields)
			{
				bool isSet;

				var nonUpdatableAttribute = mp.GetNonUpdatableAttribute(type, typeExt, mm.MapMemberInfo.MemberAccessor, out isSet);

				if (nonUpdatableAttribute != null && isSet && nonUpdatableAttribute.OnUpdate == true)
					continue;

				mp.GetPrimaryKeyOrder(type, typeExt, mm.MapMemberInfo.MemberAccessor, out isSet);

				if (isSet)
					continue;

				hasFields = true;

				var p = query.AddParameter(
					db.DataProvider.Convert(mm.Name + "_P", ConvertType.NameToQueryParameter).ToString(),
					mm.Name);

				sb.AppendFormat("\t{0} = ", db.DataProvider.Convert(p.FieldName, ConvertType.NameToQueryField));

				if (nParameter < 0)
					sb.AppendFormat("{0},\n", p.ParameterName);
				else
					sb.AppendFormat("\t{{{0}}},\n", nParameter++);
			}

			if (!hasFields)
				throw new DataAccessException(
						string.Format("There are no fields to update in the type '{0}'.", query.ObjectType.FullName));

			sb.Remove(sb.Length - 2, 1);

			AddWherePK(db, query, sb, nParameter);

			query.QueryText = sb.ToString();

			return query;
		}

		protected SqlQueryInfo CreateDeleteSqlText(DbManager db, Type type, int nParameter)
		{
			var om    = db.MappingSchema.GetObjectMapper(type);
			var sb    = new StringBuilder();
			var query = new SqlQueryInfo(om);

			sb.Append("DELETE FROM\n\t");
			AppendTableName(sb, db, type);
			sb.AppendLine();

			AddWherePK(db, query, sb, nParameter);

			query.QueryText = sb.ToString();

			return query;
		}

		[NoInterception]
		protected virtual SqlQueryInfo CreateSqlText(DbManager db, Type type, string actionName)
		{
			switch (actionName)
			{
				case "SelectByKey": return CreateSelectByKeySqlText(db, type);
				case "SelectAll":   return CreateSelectAllSqlText  (db, type);      
          
				case "Insert":      return CreateInsertSqlText     (db, type, -1, false);
                case "InsertWithIdentity": return CreateInsertSqlText(db, type, -1, true);
                case "InsertBatch": return CreateInsertSqlText(db, type, 0, false);
                case "InsertBatchWithIdentity": return CreateInsertSqlText(db, type, 0, db.UseQueryText || db.Transaction != null);                

				case "Update":      return CreateUpdateSqlText     (db, type, -1);
				case "UpdateBatch": return CreateUpdateSqlText     (db, type,  0);

				case "Delete":      return CreateDeleteSqlText     (db, type, -1);
				case "DeleteBatch": return CreateDeleteSqlText     (db, type,  0);
				default:
					throw new DataAccessException(
						string.Format("Unknown action '{0}'.", actionName));
			}
		}

		private static readonly Hashtable _actionSqlQueryInfo = new Hashtable();

	    public static void ClearCache()
	    {
	        _actionSqlQueryInfo.Clear();
	    }

		[NoInterception]
		public virtual SqlQueryInfo GetSqlQueryInfo(DbManager db, Type type, string actionName)
		{
			var key   = type.FullName + "$" + actionName + "$" + db.DataProvider.UniqueName + "$" + GetTableName(type);
			var query = (SqlQueryInfo)_actionSqlQueryInfo[key];

			if (query == null)
			{
				query = CreateSqlText(db, type, actionName);
                query.OwnerName = GetOwnerName(type);
			    query.ActionName = actionName;
			    _actionSqlQueryInfo[key] = query;
			}

			return query;
		}

		#endregion
	}
}
