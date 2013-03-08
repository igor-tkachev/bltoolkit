using System;
using BLToolkit.Emit;

namespace BLToolkit.Mapping
{
    public class ValueMapper : TableDescription, IMapper
    {
        public string ColumnAlias { get; set; }
        public string ColumnName { get; set; }

        #region IMapper Members

        public int DataReaderIndex { get; set; }
        public SetHandler Setter { get; set; }
        public Type PropertyType { get; set; }
        public string PropertyName { get; set; }

        #endregion
    }
}