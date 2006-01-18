using System;
using System.Data;

namespace BLToolkit.Mapping
{
	public class DataReaderMapper : IMapDataSource
	{
		public DataReaderMapper(IDataReader dataReader)
		{
			_dataReader = dataReader;
		}

		private IDataReader _dataReader;
		public  IDataReader  DataReader
		{
			get { return _dataReader; }
		}

		#region IMapDataSource Members

		public virtual int Count
		{
			get { return _dataReader.FieldCount; }
		}

		public virtual string GetName(int index)
		{
			return _dataReader.GetName(index);
		}

		public virtual object GetValue(object o, int index)
		{
			object value = _dataReader.GetValue(index);
			return value is DBNull? null: value;
		}

		public virtual object GetValue(object o, string name)
		{
			object value = _dataReader[name];
			return value is DBNull? null: value;
		}

		#endregion
	}
}
