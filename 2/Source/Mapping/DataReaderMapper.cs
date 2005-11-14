using System;
using System.Data;

namespace BLToolkit.Mapping
{
	public class DataReaderMapper : IMapDataSource
	{
		IDataReader _dataReader;

		public DataReaderMapper(IDataReader dataReader)
		{
			_dataReader = dataReader;
		}

		#region IMapDataSource Members

		int IMapDataSource.GetCount()
		{
			return _dataReader.FieldCount;
		}

		string IMapDataSource.GetName(int index)
		{
			return _dataReader.GetName(index);
		}

		object IMapDataSource.GetValue(object o, int index)
		{
			return _dataReader.GetValue(index);
		}

		object IMapDataSource.GetValue(object o, string name)
		{
			return _dataReader[name];
		}

		#endregion
	}
}
