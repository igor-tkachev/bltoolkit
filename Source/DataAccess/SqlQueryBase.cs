using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using BLToolkit.Aspects;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;
using BLToolkit.Reflection.MetadataProvider;

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

		#region Protected Members

		[NoInterception]
		protected virtual MemberMapper[] GetFieldList(ObjectMapper om)
		{
			List<MemberMapper> list = new List<MemberMapper>(om.Count);

			foreach (MemberMapper mm in om)
				if (mm.MapMemberInfo.SqlIgnore == false)
					list.Add(mm);

			return list.ToArray();
		}

		[NoInterception]
		protected virtual MemberMapper[] GetNonKeyFieldList(ObjectMapper om)
		{
			TypeExtension      typeExt = TypeExtension.GetTypeExtension(om.TypeAccessor.OriginalType, Extensions);
			List<MemberMapper> list    = new List<MemberMapper>();

			foreach (MemberMapper mm in om)
			{
				if (mm.MapMemberInfo.SqlIgnore)
					continue;

				MemberAccessor ma = mm.MapMemberInfo.MemberAccessor;

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
			string         key    = type.FullName + "$" + db.DataProvider.UniqueName;
			MemberMapper[] mmList = (MemberMapper[])_keyList[key];

			if (mmList == null)
			{
				TypeExtension     typeExt = TypeExtension.GetTypeExtension(type, Extensions);
				List<MemberOrder> list    = new List<MemberOrder>();

				foreach (MemberMapper mm in db.MappingSchema.GetObjectMapper(type))
				{
					if (mm.MapMemberInfo.SqlIgnore)
						continue;

					MemberAccessor ma = mm.MapMemberInfo.MemberAccessor;

					if (TypeHelper.IsScalar(ma.Type))
					{
						bool isSet;
						int order = MappingSchema.MetadataProvider.GetPrimaryKeyOrder(type, typeExt, ma, out isSet);

						if (isSet)
							list.Add(new MemberOrder(mm, order));
					}
				}

				list.Sort(delegate(MemberOrder x, MemberOrder y) { return x.Order - y.Order; });

				_keyList[key] = mmList = new MemberMapper[list.Count];

				for (int i = 0; i < list.Count; i++)
					mmList[i] = list[i].MemberMapper;
			}

			return mmList;
		}

		[NoInterception]
		protected virtual void AddWherePK(DbManager db, SqlQueryInfo query, StringBuilder sb, int nParameter)
		{
			sb.Append("WHERE\n");

			MemberMapper[] memberMappers = GetKeyFieldList(db, query.ObjectType);

			if (memberMappers.Length == 0)
				throw new DataAccessException(
						string.Format("No primary key field(s) in the type '{0}'.", query.ObjectType.FullName));

			foreach (MemberMapper mm in memberMappers)
			{
				SqlQueryParameterInfo p = query.AddParameter(
					db.DataProvider.Convert(mm.Name + "_W", ConvertType.NameToQueryParameter).ToString(),
					mm.Name);

				sb.AppendFormat("\t{0} = ", db.DataProvider.Convert(p.FieldName, ConvertType.NameToQueryField));

				if (nParameter < 0)
					sb.AppendFormat("{0} AND\n", p.ParameterName);
				else
					sb.AppendFormat("{{{0}}} AND\n", nParameter);
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

			sb.Append("FROM\n\t");

			AppendTableName(sb, db, type);

			AddWherePK(db, query, sb, -1);

			query.QueryText = sb.ToString();

			return query;
		}

		protected void AppendTableName(StringBuilder sb, DbManager db, Type type)
		{
			string database = GetDatabaseName(type);
			string owner    = GetOwnerName   (type);
			string name     = GetTableName   (type);

			db.DataProvider.BuildTableName(sb,
				database == null ? null : db.DataProvider.Convert(database, ConvertType.NameToDatabase).  ToString(),
				owner    == null ? null : db.DataProvider.Convert(owner,    ConvertType.NameToOwner).     ToString(),
				name     == null ? null : db.DataProvider.Convert(name,     ConvertType.NameToQueryTable).ToString());

			sb.AppendLine();
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

			sb.Append("FROM\n\t");
			AppendTableName(sb, db, type);

			query.QueryText = sb.ToString();

			return query;
		}

		protected SqlQueryInfo CreateInsertSqlText(DbManager db, Type type, int nParameter)
		{
			TypeExtension        typeExt = TypeExtension.GetTypeExtension(type, Extensions);
			ObjectMapper         om      = db.MappingSchema.GetObjectMapper(type);
			List<MemberMapper>   list    = new List<MemberMapper>();
			StringBuilder        sb      = new StringBuilder();
			SqlQueryInfo         query   = new SqlQueryInfo(om);
			MetadataProviderBase mp      = MappingSchema.MetadataProvider;

			sb.Append("INSERT INTO ");
			AppendTableName(sb, db, type);
			sb.Append(" (\n");

			foreach (MemberMapper mm in GetFieldList(om))
			{
				// IT: This works incorrectly for complex mappers.
				//
				// [2009-03-24] ili: use mm.MemberAccessor instead of mm.ComplexMemberAccessor
				// as in CreateUpdateSqlText
				//

				bool isSet;

				if (mp.GetNonUpdatableAttribute(type, typeExt, mm.MapMemberInfo.MemberAccessor, out isSet) == null || !isSet)
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
					db.DataProvider.Convert(mm.Name + "_P", ConvertType.NameToQueryParameter).ToString(),
					mm.Name);

				if (nParameter < 0)
					sb.AppendFormat("\t{0},\n", p.ParameterName);
					//sb.AppendFormat("\t{0},\n", db.DataProvider.Convert(p.ParameterName, ConvertType.NameToQueryParameter));
				else
					sb.AppendFormat("\t{{{0}}},\n", nParameter++);
			}

			sb.Remove(sb.Length - 2, 1);

			sb.Append(")");

			query.QueryText = sb.ToString();

			return query;
		}

		protected SqlQueryInfo CreateUpdateSqlText(DbManager db, Type type, int nParameter)
		{
			TypeExtension   typeExt = TypeExtension.GetTypeExtension(type, Extensions);
			ObjectMapper    om      = db.MappingSchema.GetObjectMapper(type);
			StringBuilder   sb      = new StringBuilder();
			SqlQueryInfo    query   = new SqlQueryInfo(om);
			MetadataProviderBase mp = MappingSchema.MetadataProvider;

			sb.Append("UPDATE\n\t");
			AppendTableName(sb, db, type);
			sb.Append("\nSET\n");

			MemberMapper[] fields    = GetFieldList(om);
			bool           hasFields = false;

			foreach (MemberMapper mm in fields)
			{
				bool isSet;

				if (mp.GetNonUpdatableAttribute(type, typeExt, mm.MapMemberInfo.MemberAccessor, out isSet) != null && isSet)
					continue;

				mp.GetPrimaryKeyOrder(type, typeExt, mm.MapMemberInfo.MemberAccessor, out isSet);

				if (isSet)
					continue;

				hasFields = true;

				SqlQueryParameterInfo p = query.AddParameter(
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
			ObjectMapper  om    = db.MappingSchema.GetObjectMapper(type);
			StringBuilder sb    = new StringBuilder();
			SqlQueryInfo  query = new SqlQueryInfo(om);

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
				case "Insert":      return CreateInsertSqlText     (db, type, -1);
				case "InsertBatch": return CreateInsertSqlText     (db, type,  0);
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

		[NoInterception]
		public virtual SqlQueryInfo GetSqlQueryInfo(DbManager db, Type type, string actionName)
		{
			string       key   = type.FullName + "$" + actionName + "$" + db.DataProvider.UniqueName + "$" + GetTableName(type);
			SqlQueryInfo query = (SqlQueryInfo)_actionSqlQueryInfo[key];

			if (query == null)
			{
				query = CreateSqlText(db, type, actionName);
				_actionSqlQueryInfo[key] = query;
			}

			return query;
		}

		#endregion
	}
}
