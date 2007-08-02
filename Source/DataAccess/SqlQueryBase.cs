using System;
using System.Collections;
using System.Text;

using BLToolkit.Aspects;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.DataAccess
{
	public abstract class SqlQueryBase : DataAccessBase
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

		protected static MemberMapper[] GetFieldList(ObjectMapper om)
		{
			ArrayList list = new ArrayList();

			foreach (MemberMapper mm in om)
				list.Add(mm);

			return (MemberMapper[])list.ToArray(typeof(MemberMapper));
		}

		protected MemberMapper[] GetNonKeyFieldList(ObjectMapper om)
		{
			TypeExtension typeExt = TypeExtension.GetTypeExtension(om.TypeAccessor.OriginalType, Extensions);
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

			public readonly MemberMapper MemberMapper;
			public readonly int          Order;
		}

		class PrimaryKeyComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				return ((MemberOrder)x).Order - ((MemberOrder)y).Order;
			}
		}

		private static readonly PrimaryKeyComparer _primaryKeyComparer = new PrimaryKeyComparer();
		private static readonly Hashtable          _keyList            = new Hashtable();

		public MemberMapper[] GetKeyFieldList(DbManager db, Type type)
		{
			string         key    = type.FullName + "$" + db.DataProvider.Name;
			MemberMapper[] mmList = (MemberMapper[])_keyList[key];

			if (mmList == null)
			{
				TypeExtension typeExt = TypeExtension.GetTypeExtension(type, Extensions);
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

		protected void AddWherePK(DbManager db, SqlQueryInfo query, StringBuilder sb)
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
			TypeExtension typeExt = TypeExtension.GetTypeExtension(type, Extensions);
			ObjectMapper  om      = db.MappingSchema.GetObjectMapper(type);
			ArrayList     list    = new ArrayList();
			StringBuilder sb      = new StringBuilder();
			SqlQueryInfo  query   = new SqlQueryInfo(om);

			sb.AppendFormat("INSERT INTO {0} (\n",
				db.DataProvider.Convert(GetTableName(type), ConvertType.NameToQueryTable));

			foreach (MemberMapper mm in GetFieldList(om))
			{
				// IT: This works incorrectly for complex mappers.
				//
				object value = typeExt[mm.ComplexMemberAccessor.Name]["NonUpdatable"].Value;

				if ((value != null && (bool)TypeExtension.ChangeType(value, typeof(bool)) == false) ||
					(value == null && mm.ComplexMemberAccessor.GetAttributes(typeof(NonUpdatableAttribute)) == null))
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
			TypeExtension typeExt = TypeExtension.GetTypeExtension(type, Extensions);
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

		[NoInterception]
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

		private static readonly Hashtable _actionSqlQueryInfo = new Hashtable();

		[NoInterception]
		public virtual SqlQueryInfo GetSqlQueryInfo(DbManager db, Type type, string actionName)
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
	}
}
