using System;
using System.Data;

namespace BLToolkit.Mapping
{
	public sealed class DataReaderMapper : IMapDataSource
	{
		IDataReader _dataReader;

		public DataReaderMapper(IDataReader dataReader)
		{
			_dataReader = dataReader;
		}

		#region IMapDataSource Members

		int IMapDataSource.Count
		{
			get { return _dataReader.FieldCount; }
		}

		string IMapDataSource.GetName(int index)
		{
			return _dataReader.GetName(index);
		}

		object IMapDataSource.GetValue(object o, int index)
		{
			object value = _dataReader.GetValue(index);
			return value is DBNull? null: value;
		}

		object IMapDataSource.GetValue(object o, string name)
		{
			object value = _dataReader[name];
			return value is DBNull? null: value;
		}

		#endregion
	}
}
