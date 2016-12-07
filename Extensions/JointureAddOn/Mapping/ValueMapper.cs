using System;
using System.Collections.Generic;
using BLToolkit.Emit;

namespace BLToolkit.Mapping
{
    public class ValueMapper : TableDescription, IMapper
    {
        private readonly Dictionary<string, List<string>> _columnVariations = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, int> _columnOccurences = new Dictionary<string, int>();

        public string ColumnAlias { get; set; }
        public string ColumnName { get; set; }

        #region IMapper Members

        public int DataReaderIndex { get; set; }
        public SetHandler Setter { get; set; }
        public Type PropertyType { get; set; }
        public string PropertyName { get; set; }

        #endregion

        public string GetColumnName(string columnName)
        {
            int occurenceCount;
            if (_columnOccurences.ContainsKey(columnName))
            {
                occurenceCount = _columnOccurences[columnName] + 1;
                _columnOccurences[columnName] = occurenceCount;
            }
            else
            {
                _columnOccurences[columnName] = 1;
                occurenceCount = 1;
            }

            string res = columnName + (occurenceCount > 1 ? string.Format("_{0}", occurenceCount - 1) : "");

            var variations = new List<string>();
            if (_columnVariations.ContainsKey(columnName))
            {
                variations = _columnVariations[columnName];
            }

            variations.Add(res);
            _columnVariations[columnName] = variations;

            return res;
        }

        public bool SetDataReaderIndex(List<string> schemaColumns)
        {
            string colName = ColumnName;
            int index = -1;
            if (!schemaColumns.Contains(colName))
            {
                bool found = false;
                int order = 1;
                foreach (string key in _columnVariations.Keys)
                {
                    List<string> variations = _columnVariations[key];
                    if (variations.Contains(colName))
                    {
                        if (colName.Contains(key + "_"))
                        {
                            string orderString = colName.Replace(key + "_", "");
                            order = int.Parse(orderString) + 1;
                            colName = key;
                            found = true;
                            break;
                        }
                    }
                }
                if (found)
                {
                    int i = 0, occurenceCnt = 0;
                    foreach (string column in schemaColumns)
                    {
                        if (column == colName)
                        {
                            occurenceCnt++;
                            if (occurenceCnt == order)
                            {
                                index = i;
                                break;
                            }
                        }
                        i++;
                    }
                }
                else
                {
                    // TODO Check this condition...
                    //if (!_ignoreMissingColumns)
                    //{
                    //    throw new Exception(string.Format("Couldnt find db column {0} in the query result", colName));
                    //}
                    return true;
                }
            }
            else
                index = schemaColumns.IndexOf(colName);

            DataReaderIndex = index;
            return false;
        }
    }
}