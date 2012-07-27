using System;
using System.Collections.Generic;
using BLToolkit.Emit;

namespace BLToolkit.Mapping
{
    public interface IMapper
    {
        int DataReaderIndex { get; set; }
        SetHandler Setter { get; set; }
        Type PropertyType { get; set; }
        string PropertyName { get; set; }
    }

    public class OwnerDescription
    {
        public string Database { get; set; }
        public string Owner { get; set; }
    }

    public class TableDescription : OwnerDescription
    {
        public string TableName { get; set; }
    }

    public class ValueMapper : TableDescription, IMapper
    {
        public string ColumnAlias { get; set; }
        public string ColumnName { get; set; }
        public Type DbType { get; set; }

        #region IMapper Members

        public int DataReaderIndex { get; set; }
        public SetHandler Setter { get; set; }
        public Type PropertyType { get; set; }
        public string PropertyName { get; set; }

        #endregion
    }

    public class CollectionFullObjectMapper : TableDescription, IObjectMapper
    {
        public  CollectionFullObjectMapper()
        {
            PropertiesMapping = new List<IMapper>();
        }

        #region IPropertiesMapping Members

        public List<IMapper> PropertiesMapping { get; private set; }
        public IPropertiesMapping ParentMapping { get; set;}

        #endregion

        public Type PropertyCollectionType { get; set; }

        #region IMapper Members

        public int DataReaderIndex { get; set; }
        public SetHandler Setter { get; set; }
        public Type PropertyType { get; set; }
        public string PropertyName { get; set; }

        #endregion

        #region IObjectMapper

        public bool IsLazy { get; set; }
        public bool ContainsLazyChild { get; set; }
        public GetHandler Getter { get; set; }
        public GetHandler PrimaryKeyValueGetter { get; set; }
        public AssociationAttribute Association { get; set; }

        #endregion

        #region ILazyMapper

        public GetHandler ParentKeyGetter { get; set; }

        #endregion
    }

    public interface ILazyMapper
    {
        GetHandler ParentKeyGetter { get; set; }
    }

    public interface IObjectMapper : IPropertiesMapping, IMapper, ILazyMapper
    {
        bool IsLazy { get; set; }
        bool ContainsLazyChild { get; set; }
        /// <summary>
        /// Is set only for CollectionFullObjectMapper. TODO : Should refactor this?
        /// </summary>
        GetHandler Getter { get; set; }
        GetHandler PrimaryKeyValueGetter { get; set; }
        AssociationAttribute Association { get; set; }
    }

    public interface IPropertiesMapping
    {
        List<IMapper> PropertiesMapping { get; }
        IPropertiesMapping ParentMapping { get; set; }
    }

    public class FullObjectMapper : ObjectMapper, IObjectMapper
    {
        public FullObjectMapper()
        {
            PropertiesMapping = new List<IMapper>();
        }

        #region IPropertiesMapping Members

        public List<IMapper> PropertiesMapping { get; private set; }
        public IPropertiesMapping ParentMapping { get; set; }

        #endregion

        public bool IsNullable { get; set; }
        public bool ColParent { get; set; }

        #region IMapper Members

        public int DataReaderIndex { get; set; }
        public SetHandler Setter { get; set; }
        public Type PropertyType { get; set; }
        public string PropertyName { get; set; }
        public AssociationAttribute Association { get; set; }

        #endregion

        #region IObjectMapper

        public bool IsLazy { get; set; }
        public bool ContainsLazyChild { get; set; }
        public GetHandler Getter { get; set; }

        #endregion

        #region ILazyMapper

        public GetHandler ParentKeyGetter { get; set; }
        public GetHandler PrimaryKeyValueGetter { get; set; }

        #endregion
    }
}