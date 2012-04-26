using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using BLToolkit.Aspects;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;

namespace BLToolkit.DataAccess
{
    public class FullSqlQuery : SqlQuery
    {
        private readonly bool _ignoreLazyLoad;

        #region Constructors

        public FullSqlQuery(DbManager dbManager, bool ignoreLazyLoad = false, MappingOrder mappingOrder = MappingOrder.ByColumnIndex)
            : base(dbManager)
        {
            dbManager.MappingSchema = new FullMappingSchema(ignoreLazyLoad, mappingOrder);
            _ignoreLazyLoad = ignoreLazyLoad;
        }

        #endregion

        #region Overrides

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

        private readonly Hashtable _actionSqlQueryInfo = new Hashtable();

        [NoInterception]
        public override SqlQueryInfo GetSqlQueryInfo(DbManager db, Type type, string actionName)
        {    
            var key = type.FullName + "$" + actionName + "$" + db.DataProvider.UniqueName + "$" + GetTableName(type);
            var query = (SqlQueryInfo)_actionSqlQueryInfo[key];

            if (query == null)
            {
                query = CreateSqlText(db, type, actionName);
                _actionSqlQueryInfo[key] = query;
            }

            return query;
        }

        #endregion

        #region Protected

        protected SqlQueryInfo CreateSelectAllFullSqlText(DbManager db, Type type)
        {
            var sb = new StringBuilder();
            var query = new FullSqlQueryInfo();

            sb.Append("SELECT\n");

            int index = 0;
            FullObjectMapper mainMapper = ((FullMappingSchema) db.MappingSchema).GetObjectMapper(type, ref index);
            BuildSelectSQL(mainMapper, sb, db);

            sb.Remove(sb.Length - 2, 1);

            sb.Append("FROM\n\t");

            FullAppendTableName(sb, db, type);

            AppendJoinTableName(sb, db, type);

            query.QueryText = sb.ToString();

            return query;
        }

        protected SqlQueryInfo CreateSelectFullByKeySqlText(DbManager db, Type type)
        {
            var sb = new StringBuilder();
            var query = new FullSqlQueryInfo();

            sb.Append("SELECT\n");

            int index = 0;
            FullObjectMapper mainMapper = ((FullMappingSchema) db.MappingSchema).GetObjectMapper(type, ref index);
            BuildSelectSQL(mainMapper, sb, db);

            sb.Remove(sb.Length - 2, 1);

            sb.Append("FROM\n\t");

            FullAppendTableName(sb, db, type);

            AppendJoinTableName(sb, db, type);

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
                                                               database == null
                                                                   ? null
                                                                   : db.DataProvider.Convert(database,
                                                                                             ConvertType.NameToDatabase)
                                                                         .ToString(),
                                                               owner == null
                                                                   ? null
                                                                   : db.DataProvider.Convert(owner,
                                                                                             ConvertType.NameToOwner).
                                                                         ToString(),
                                                               name == null
                                                                   ? null
                                                                   : db.DataProvider.Convert(name,
                                                                                             ConvertType.
                                                                                                 NameToQueryTable).
                                                                         ToString());

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

        #endregion

        private void BuildSelectSQL(IPropertiesMapping mapper, StringBuilder sb, DbManager db)
        {
            foreach (IMapper mapField in mapper.PropertiesMapping)
            {
                if (mapField is ValueMapper)
                    sb.AppendFormat("\t{0}.{1},\n"
                                    , ((IObjectMapper)mapper).PropertyType.Name
                                    ,
                                    db.DataProvider.Convert(((ValueMapper)mapField).ColumnName,
                                                            ConvertType.NameToQueryField)
                        );
                else if (mapField is IPropertiesMapping)
                {
                    var propertiesMapping = (IPropertiesMapping)mapField;
                    var cel = propertiesMapping.ParentMapping;
                    while (cel != null)
                    {
                        // To avoid recursion dont take in account types already loaded.
                        if (((IMapper)cel).PropertyType == mapField.PropertyType)
                            continue;
                        cel = cel.ParentMapping;
                    }
                    var objectMapper = (IObjectMapper)mapField;
                    if (!objectMapper.IsLazy)
                        BuildSelectSQL(propertiesMapping, sb, db);
                }
                else
                    throw new NotImplementedException(mapField.GetType() + " is not yet implemented.");
            }
        }

        private void AppendJoinTableName(StringBuilder sb, DbManager db, Type type)
        {
            string parentName = FullGetTableName(type);

            foreach (PropertyInfo prop in type.GetProperties())
            {
                bool isCollection = prop.PropertyType.GetInterfaces().ToList().Contains(typeof(IList));
                Type listElementType = null;
                if (isCollection)
                {
                    listElementType = FullMappingSchema.GetGenericType(prop.PropertyType);
                }

                if (!_ignoreLazyLoad)
                {
                    object[] lazy = prop.GetCustomAttributes(typeof (LazyInstanceAttribute), true);
                    if (lazy.Length > 0)
                    {
                        if (((LazyInstanceAttribute) lazy[0]).IsLazy)
                        {
                            continue;
                        }
                    }
                }

                object[] attribs = prop.GetCustomAttributes(typeof(AssociationAttribute), true);
                if (attribs.Length > 0)
                {
                    var associationAttribute = (AssociationAttribute)attribs[0];

                    PropertyInfo parentField = type.GetProperty(associationAttribute.ThisKey);
                    PropertyInfo childField = prop.PropertyType.GetProperty(associationAttribute.OtherKey);
                    if (isCollection)
                    {
                        childField = listElementType.GetProperty(associationAttribute.OtherKey);
                        //FullMappingSchema.GetColumnFromProperty(listElementType, associationAttribute.OtherKey);
                    }

                    object[] parentFieldAttributes = parentField.GetCustomAttributes(typeof(MapFieldAttribute), true);
                    string parentDbField = parentFieldAttributes.Length > 0
                                               ? ((MapFieldAttribute)parentFieldAttributes[0]).MapName
                                               : associationAttribute.ThisKey;

                    object[] childFieldAttributes = childField.GetCustomAttributes(typeof(MapFieldAttribute), true);
                    string childDbField = childFieldAttributes.Length > 0
                                              ? ((MapFieldAttribute)childFieldAttributes[0]).MapName
                                              : associationAttribute.OtherKey;


                    string childDatabase = isCollection
                                               ? GetDatabaseName(listElementType)
                                               : GetDatabaseName(prop.PropertyType);

                    string childOwner = isCollection ? base.GetOwnerName(listElementType) : base.GetOwnerName(prop.PropertyType);
                    string childName = isCollection ? base.GetTableName(listElementType) : base.GetTableName(prop.PropertyType);
                    string childAlias = isCollection ? FullGetTableName(listElementType) : FullGetTableName(prop.PropertyType);

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

                    AppendJoinTableName(sb, db, isCollection ? listElementType : prop.PropertyType);
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

        private void AddWherePK(DbManager db, SqlQueryInfo query, StringBuilder sb, int nParameter,
                                FullObjectMapper mapper)
        {
            sb.Append("WHERE\n");

            foreach (IMapper mm in mapper.PropertiesMapping)
            {
                if (mm is ValueMapper && mm.DataReaderIndex == mapper.DataReaderIndex)
                {
                    var valueMapper = (ValueMapper)mm;

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
    }        

    public class FullSqlQueryT<T> : SqlQuery<T>
    {
        private bool _ignoreLazyLoad;

        #region Constructors

        public FullSqlQueryT(DbManager dbManager)
            : this(dbManager, false)
        {
        }

        public FullSqlQueryT(DbManager dbManager, bool ignoreLazyLoad)
            : this(dbManager, ignoreLazyLoad, MappingOrder.ByColumnIndex)
        {
        }

        public FullSqlQueryT(DbManager dbManager, bool ignoreLazyLoad, MappingOrder mappingOrder)
            : base(dbManager)
        {
            dbManager.MappingSchema = new FullMappingSchema(ignoreLazyLoad, mappingOrder);
            _ignoreLazyLoad = ignoreLazyLoad;
        }

        #endregion

        #region Overrides

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

        #endregion

        #region Protected

        protected SqlQueryInfo CreateSelectAllFullSqlText(DbManager db, Type type)
        {
            var sb = new StringBuilder();
            var query = new FullSqlQueryInfo();

            sb.Append("SELECT\n");

            int index = 0;
            FullObjectMapper mainMapper = ((FullMappingSchema) db.MappingSchema).GetObjectMapper(type, ref index);
            BuildSelectSQL(mainMapper, sb, db);

            sb.Remove(sb.Length - 2, 1);

            sb.Append("FROM\n\t");

            FullAppendTableName(sb, db, type);

            AppendJoinTableName(sb, db, type);

            query.QueryText = sb.ToString();

            return query;
        }

        protected SqlQueryInfo CreateSelectFullByKeySqlText(DbManager db, Type type)
        {
            var sb = new StringBuilder();
            var query = new FullSqlQueryInfo();

            sb.Append("SELECT\n");

            int index = 0;
            FullObjectMapper mainMapper = ((FullMappingSchema)db.MappingSchema).GetObjectMapper(type, ref index);
            BuildSelectSQL(mainMapper, sb, db);

            sb.Remove(sb.Length - 2, 1);

            sb.Append("FROM\n\t");

            FullAppendTableName(sb, db, type);

            AppendJoinTableName(sb, db, type);

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

        #endregion

        private void BuildSelectSQL(IPropertiesMapping mapper, StringBuilder sb, DbManager db)
        {
            foreach (IMapper mapField in mapper.PropertiesMapping)
            {
                if (mapField is ValueMapper)
                    sb.AppendFormat("\t{0}.{1},\n", ((IObjectMapper)mapper).PropertyType.Name,
                    db.DataProvider.Convert(((ValueMapper)mapField).ColumnName, ConvertType.NameToQueryField));
                else if (mapField is IPropertiesMapping)
                {
                    var propertiesMapping = (IPropertiesMapping)mapField;
                    var cel = propertiesMapping.ParentMapping;
                    while (cel != null)
                    {
                        // To avoid recursion dont take in account types already loaded.
                        if (((IMapper)cel).PropertyType == mapField.PropertyType)
                            continue;
                        cel = cel.ParentMapping;
                    }
                    var objectMapper = (IObjectMapper)mapField;
                    if (!objectMapper.IsLazy)
                        BuildSelectSQL(propertiesMapping, sb, db);
                }
                else
                    throw new NotImplementedException(mapField.GetType() + " is not yet implemented.");
            }
        }

        private void AppendJoinTableName(StringBuilder sb, DbManager db, Type type)
        {
            string parentName = FullGetTableName(type);

            foreach (PropertyInfo prop in type.GetProperties())
            {
                bool isCollection = prop.PropertyType.GetInterfaces().ToList().Contains(typeof (IList));
                Type listElementType = null;
                if (isCollection)
                {
                    listElementType = FullMappingSchema.GetGenericType(prop.PropertyType);
                }

                if (!_ignoreLazyLoad)
                {
                    object[] lazy = prop.GetCustomAttributes(typeof (LazyInstanceAttribute), true);
                    if (lazy.Length > 0)
                    {
                        if (((LazyInstanceAttribute) lazy[0]).IsLazy)
                        {
                            continue;
                        }
                    }
                }

                object[] attribs = prop.GetCustomAttributes(typeof (AssociationAttribute), true);
                if (attribs.Length > 0)
                {
                    var assocAttrib = (AssociationAttribute) attribs[0];

                    PropertyInfo parentField = type.GetProperty(assocAttrib.ThisKey);
                    PropertyInfo childField = prop.PropertyType.GetProperty(assocAttrib.OtherKey);
                    if (isCollection)
                    {
                        childField = listElementType.GetProperty(assocAttrib.OtherKey);
                        //FullMappingSchema.GetColumnFromProperty(listElementType, associationAttribute.OtherKey);
                    }

                    object[] parentFieldAttributes = parentField.GetCustomAttributes(typeof (MapFieldAttribute), true);
                    string parentDbField = parentFieldAttributes.Length > 0
                                               ? ((MapFieldAttribute) parentFieldAttributes[0]).MapName
                                               : assocAttrib.ThisKey;

                    object[] childFieldAttributes = childField.GetCustomAttributes(typeof (MapFieldAttribute), true);
                    string childDbField = childFieldAttributes.Length > 0
                                              ? ((MapFieldAttribute) childFieldAttributes[0]).MapName
                                              : assocAttrib.OtherKey;


                    string childDatabase = isCollection
                                               ? GetDatabaseName(listElementType)
                                               : GetDatabaseName(prop.PropertyType);

                    string childOwner = isCollection ? base.GetOwnerName(listElementType) : base.GetOwnerName(prop.PropertyType);
                    string childName = isCollection ? base.GetTableName(listElementType) : base.GetTableName(prop.PropertyType);
                    string childAlias = isCollection ? FullGetTableName(listElementType) : FullGetTableName(prop.PropertyType);

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

                    AppendJoinTableName(sb, db, isCollection ? listElementType : prop.PropertyType);
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

        private void AddWherePK(DbManager db, SqlQueryInfo query, StringBuilder sb, int nParameter,
                                FullObjectMapper mapper)
        {
            sb.Append("WHERE\n");

            foreach (IMapper mm in mapper.PropertiesMapping)
            {
                if (mm is ValueMapper && mm.DataReaderIndex == mapper.DataReaderIndex)
                {
                    var valueMapper = (ValueMapper)mm;

                    string tableAlias = mapper.PropertyType.Name;

                    //mm.Name = ID_TRACK
                    SqlQueryParameterInfo p = query.AddParameter(
                        db.DataProvider.Convert(valueMapper.ColumnName + "_W", ConvertType.NameToQueryParameter).
                            ToString(),
                        valueMapper.ColumnName);

                    sb.AppendFormat("\t{0}.{1} = ", tableAlias, db.DataProvider.Convert(p.FieldName, ConvertType.NameToQueryField));

                    if (nParameter < 0)
                        sb.AppendFormat("{0} AND\n", p.ParameterName);
                    else
                        sb.AppendFormat("{{{0}}} AND\n", nParameter++);
                }
            }

            sb.Remove(sb.Length - 5, 5);
        }
    }
}