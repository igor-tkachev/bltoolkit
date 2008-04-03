using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using BLToolkit.Aspects;
using BLToolkit.Common;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Reflection;

namespace PetShop.BusinessLogic.DataAccess
{
	[Log, Counter]
	public abstract class AccessorBase<D, A> : DataAccessor
		where D : DbManager, new()
		where A : AccessorBase<D, A>
	{
		#region Overrides

		[NoInterception]
		protected override DbManager CreateDbManager()
		{
			return new D();
		}

		#endregion

		#region CreateInstance

		public static A CreateInstance()
		{
			return TypeAccessor<A>.CreateInstance();
		}

		#endregion

		#region Query

		[Log, Counter]
		public abstract class PetShopQuery : SqlQuery
		{
			[NoInterception]
			protected override DbManager CreateDbManager()
			{
				return new D();
			}

			#region SelectByKeyWords

			public virtual List<T> SelectByKeyWords<T>(string[] keyWords)
			{
				StringBuilder sql = new StringBuilder("SELECT * FROM ");

				sql.Append(GetTableName(typeof(T)));
				sql.Append(" WHERE ");

				for (int i = 0; i < keyWords.Length; i++)
				{
					if (i > 0)
						sql.Append(" OR ");

					sql.AppendFormat("LOWER(Name) LIKE @kw{0} OR LOWER(CategoryId) LIKE @kw{0}", i);
				}

				using (DbManager db = GetDbManager())
				{
					IDbDataParameter[] parms = new IDbDataParameter[keyWords.Length];

					for (int i = 0; i < keyWords.Length; i++)
						parms[i] = db.Parameter("@kw" + i, "%" + keyWords[i].ToLower() + "%");

					return db.SetCommand(sql.ToString(), parms).ExecuteList<T>();
				}
			}

			#endregion

			#region InsertAndGetIdentity

			public virtual int InsertAndGetIdentity(DbManager db, object obj)
			{
				SqlQueryInfo query = GetSqlQueryInfo(db, obj.GetType(), "InsertAndGetIdentity");

				return db
					.SetCommand(query.QueryText, query.GetParameters(db, obj))
					.ExecuteScalar<int>();
			}

			public virtual int InsertAndGetIdentity(object obj)
			{
				DbManager db = GetDbManager();

				try
				{
					return InsertAndGetIdentity(db, obj);
				}
				finally
				{
					Dispose(db);
				}
			}

			[NoInterception]
			protected override SqlQueryInfo CreateSqlText(DbManager db, Type type, string actionName)
			{
				switch (actionName)
				{
					case "InsertAndGetIdentity":
						SqlQueryInfo qi = CreateInsertSqlText(db, type);

						qi.QueryText += "\nSELECT Cast(SCOPE_IDENTITY() as int)";

						return qi;

					default :
						return  base.CreateSqlText(db, type, actionName);
				}
			}

			#endregion
		}

		public PetShopQuery Query
		{
			get { return TypeAccessor<PetShopQuery>.CreateInstance(); }
		}

		#endregion
	}
}
