#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Aspects;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Mapping;

#endregion

namespace BLToolkit.DataAccess
{
    public class FullSqlQuery : SqlQuery
    {
        private readonly bool _ignoreLazyLoad;

        #region Constructors

        public FullSqlQuery(DbManager dbManager, bool ignoreLazyLoad = false, MappingOrder mappingOrder = MappingOrder.ByColumnIndex)
            : base(dbManager)
        {
            dbManager.MappingSchema = new FullMappingSchema(dbManager, ignoreLazyLoad: ignoreLazyLoad, mappingOrder: mappingOrder, parentMappingSchema: dbManager.MappingSchema);

            _ignoreLazyLoad = ignoreLazyLoad;
        }

        #endregion

        #region Overrides

        private readonly Hashtable _actionSqlQueryInfo = new Hashtable();

        [NoInterception]
        protected override SqlQueryInfo CreateSqlText(DbManager db, Type type, string actionName)
        {
            switch (actionName)
            {
                case "SelectByKey":
                    return CreateSelectFullByKeySqlText(db, type);
                case "SelectAll":
                    return CreateSelectAllFullSqlText(db, type);
                default:
                    return base.CreateSqlText(db, type, actionName);
            }
        }

        [NoInterception]
        public override SqlQueryInfo GetSqlQueryInfo(DbManager db, Type type, string actionName)
        {
            var key = type.FullName + "$" + actionName + "$" + db.DataProvider.UniqueName + "$" + GetTableName(type);
            var query = (SqlQueryInfo) _actionSqlQueryInfo[key];

            if (query == null)
            {
                query = CreateSqlText(db, type, actionName);
                _actionSqlQueryInfo[key] = query;
            }

            return query;
        }

        #endregion

        #region Protected

        private SqlQueryInfo CreateSelectAllFullSqlText(DbManager db, Type type)
        {
            var sb = new StringBuilder();
            var query = new FullSqlQueryInfo();

            sb.Append("SELECT\n");

            var mainMapper = (FullObjectMapper) db.MappingSchema.GetObjectMapper(type);

            BuildSelectSql(mainMapper, sb, db);

            sb.Remove(sb.Length - 2, 1);

            sb.Append("FROM\n\t");

            FullAppendTableName(sb, db, type);

            AppendJoinTableName(mainMapper, sb, db, type);

            query.QueryText = sb.ToString();

            return query;
        }

        private SqlQueryInfo CreateSelectFullByKeySqlText(DbManager db, Type type)
        {
            var sb = new StringBuilder();
            var query = new FullSqlQueryInfo();

            sb.Append("SELECT\n");

            var mainMapper = (FullObjectMapper) db.MappingSchema.GetObjectMapper(type);
            BuildSelectSql(mainMapper, sb, db);

            sb.Remove(sb.Length - 2, 1);

            sb.Append("FROM\n\t");

            FullAppendTableName(sb, db, type);

            AppendJoinTableName(mainMapper, sb, db, type);

            AddWherePK(db, query, sb, -1, mainMapper);

            query.QueryText = sb.ToString();

            return query;
        }

        protected void FullAppendTableName(StringBuilder sb, DbManager db, Type type)
        {
            var database = GetDatabaseName(type);
            var owner = GetOwnerName(type);
            var name = base.GetTableName(type);

            db.DataProvider.CreateSqlProvider().BuildTableName(sb,
                                                               database == null ? null : db.DataProvider.Convert(database, ConvertType.NameToDatabase).ToString(),
                                                               owner == null ? null : db.DataProvider.Convert(owner, ConvertType.NameToOwner).ToString(),
                                                               name == null ? null : db.DataProvider.Convert(name, ConvertType.NameToQueryTable).ToString());

            //TODO Override OracleSqlProvider in order to avoid this mess...
            string alias = FullGetTableName(type);
            sb.Append(" " + alias);
            sb.AppendLine();
        }

        protected string FullGetTableName(Type type)
        {
            //bool isSet;
            //return MappingSchema.MetadataProvider.GetTableName(type, Extensions, out isSet);

            return type.Name;
        }

        protected void BuildSelectSql(IObjectMapper mapper, StringBuilder sb, DbManager db)
        {
            foreach (IMapper mapField in mapper.PropertiesMapping)
            {
                if (mapField is ValueMapper)
                    sb.AppendFormat("\t{0}.{1} {2},\n"
                                    , (mapper).PropertyType.Name,
                                    db.DataProvider.Convert(((ValueMapper) mapField).ColumnName, ConvertType.NameToQueryField),
                                    ((ValueMapper) mapField).ColumnAlias
                        );
                else if (mapField is IPropertiesMapping)
                {
                    var propertiesMapping = (IPropertiesMapping) mapField;
                    var cel = propertiesMapping.ParentMapping;
                    while (cel != null)
                    {
                        // To avoid recursion dont take in account types already loaded.
                        if (((IMapper) cel).PropertyType == mapField.PropertyType)
                            continue;
                        cel = cel.ParentMapping;
                    }
                    var objectMapper = (IObjectMapper) mapField;
                    if (!objectMapper.IsLazy)
                        BuildSelectSql(objectMapper, sb, db);
                }
                else
                    throw new NotImplementedException(mapField.GetType() + " is not yet implemented.");
            }
        }

        protected void AppendJoinTableName(IPropertiesMapping mapper, StringBuilder sb, DbManager db, Type type)
        {
            string parentName = FullGetTableName(type);
            Dictionary<string, ValueMapper> valueMappers = mapper.PropertiesMapping.Where(e => e is ValueMapper).Cast<ValueMapper>().ToDictionary(e => e.PropertyName, e => e);

            foreach (IMapper mapField in mapper.PropertiesMapping)
            {
                var objectMapper = mapField as IObjectMapper;
                if (objectMapper != null)
                {
                    if (!_ignoreLazyLoad)
                    {
                        if (objectMapper.IsLazy)
                            continue;
                    }

                    string thisKey = objectMapper.Association.ThisKey[0];

                    // TITLE
                    string parentDbField = valueMappers.ContainsKey(thisKey) ? valueMappers[thisKey].ColumnName : thisKey;

                    // ARTIST
                    string childDbField = objectMapper.PropertiesMapping.Where(e => e is ValueMapper).Cast<ValueMapper>().First(
                        e => e.PropertyName == objectMapper.Association.OtherKey[0]).ColumnName;

                    string childDatabase = GetDatabaseName(mapField.PropertyType);
                    string childOwner = base.GetOwnerName(mapField.PropertyType);
                    string childName = base.GetTableName(mapField.PropertyType);
                    string childAlias = FullGetTableName(mapField.PropertyType);

                    StringBuilder childFullName = db.DataProvider.CreateSqlProvider().BuildTableName(
                        new StringBuilder(),
                        childDatabase == null
                            ? null
                            : db.DataProvider.Convert(childDatabase, ConvertType.NameToDatabase).ToString(),
                        childOwner == null
                            ? null
                            : db.DataProvider.Convert(childOwner, ConvertType.NameToOwner).ToString(),
                        childName == null
                            ? null
                            : db.DataProvider.Convert(childName, ConvertType.NameToQueryTable).ToString());

                    sb.AppendFormat("\tINNER JOIN {0} {1} ON {2}.{3}={4}.{5}\n",
                                    childFullName,
                                    childAlias,
                                    parentName,
                                    parentDbField,
                                    childAlias,
                                    childDbField
                        );

                    AppendJoinTableName((IPropertiesMapping) mapField, sb, db, mapField.PropertyType);
                }
            }

            sb.AppendLine();

            //SELECT
            //    ARTIST2.ID_ARTIST,
            //    ARTIST2.ARTIST,
            //    TRACK.ID_TRACK,
            //    TRACK.TRACK,
            //    TRACK.ID_ARTIST,
            //    ARTIST.ID_ARTIST,
            //    ARTIST.ARTIST
            //FROM
            //    PITAFR01.ARTIST ARTIST2
            //    INNER JOIN PITAFR01.TRACK TRACK ON ARTIST2.ID_ARTIST=TRACK.ID_ARTIST
            //    INNER JOIN PITAFR01.ARTIST ARTIST ON TRACK.ID_ARTIST=ARTIST.ID_ARTIST
            //WHERE
            //    ARTIST2.ID_ARTIST = 2566
        }

        protected void AddWherePK(DbManager db, SqlQueryInfo query, StringBuilder sb, int nParameter, FullObjectMapper mapper)
        {
            sb.Append("WHERE\n");

            foreach (IMapper mm in mapper.PropertiesMapping)
            {
                if (mm is ValueMapper && mm.DataReaderIndex == mapper.DataReaderIndex)
                {
                    var valueMapper = (ValueMapper) mm;

                    string tableAlias = mapper.PropertyType.Name;

                    //mm.Name = ID_TRACK
                    SqlQueryParameterInfo p = query.AddParameter(
                        db.DataProvider.Convert(valueMapper.ColumnName + "_W", ConvertType.NameToQueryParameter).
                           ToString(),
                        valueMapper.ColumnName);

                    sb.AppendFormat("\t{0}.{1} = ", tableAlias,
                                    db.DataProvider.Convert(p.FieldName, ConvertType.NameToQueryField));

                    if (nParameter < 0)
                        sb.AppendFormat("{0} AND\n", p.ParameterName);
                    else
                        sb.AppendFormat("{{{0}}} AND\n", nParameter++);
                }
            }

            sb.Remove(sb.Length - 5, 5);
        }

        #endregion
    }
}