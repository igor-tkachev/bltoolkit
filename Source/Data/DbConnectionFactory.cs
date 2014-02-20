using System.Collections.Generic;
using BLToolkit.Common;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Sql;

namespace BLToolkit.Data
{
    public abstract class DbConnectionFactory : IDbConnectionFactory
    {
        public DataProviderBase Provider;
        public string ConnectionString { get; set; }

        #region IDbConnectionFactory Members

        public DbManager CreateDbManager()
        {
            return CreateDbManager(Provider, ConnectionString);
        }

        public virtual DbManager CreateDbManager(DataProviderBase provider, string connectionString)
        {
            return new MyDbManager(provider, connectionString);
        }

        #endregion
    }

    public class MyDbManager : DbManager
    {
        public MyDbManager(DataProviderBase provider, string connectionString) : base(provider, connectionString)
        {
        }

        public override BLToolkit.Data.Sql.SqlQuery ProcessQuery(BLToolkit.Data.Sql.SqlQuery sqlQuery)
        {
            return base.ProcessQuery(sqlQuery);

            if (sqlQuery.IsInsert && sqlQuery.Insert.Into.Name == "Parent")
            {
                var expr =
                    new QueryVisitor().Find(sqlQuery.Insert, e =>
                    {
                        if (e.ElementType == QueryElementType.SetExpression)
                        {
                            var se = (BLToolkit.Data.Sql.SqlQuery.SetExpression)e;
                            return ((SqlField)se.Column).Name == "ParentID";
                        }

                        return false;
                    }) as BLToolkit.Data.Sql.SqlQuery.SetExpression;

                if (expr != null)
                {
                    var value = ConvertTo<int>.From(((IValueContainer)expr.Expression).Value);

                    if (value == 555)
                    {
                        var tableName = "Parent1";
                        var dic = new Dictionary<IQueryElement, IQueryElement>();

                        sqlQuery = new QueryVisitor().Convert(sqlQuery, e =>
                        {
                            if (e.ElementType == QueryElementType.SqlTable)
                            {
                                var oldTable = (SqlTable)e;

                                if (oldTable.Name == "Parent")
                                {
                                    var newTable = new SqlTable(oldTable) { Name = tableName, PhysicalName = tableName };

                                    foreach (var field in oldTable.Fields.Values)
                                        dic.Add(field, newTable.Fields[field.Name]);

                                    return newTable;
                                }
                            }

                            IQueryElement ex;
                            return dic.TryGetValue(e, out ex) ? ex : null;
                        });
                    }
                }
            }

            return sqlQuery;
        }
    }
}