using System;
using System.Data;

namespace BLToolkit.Mapping
{
	public class DataReaderMapSource : IMapSource
	{
		IDataReader _dataReader;

		public DataReaderMapSource(IDataReader dataReader)
		{
			_dataReader = dataReader;
		}

		#region IMapSource Members

		public string[] GetNames()
		{
			string[] names = new string[_dataReader.FieldCount];

			for (int i = 0; i < names.Length; i++)
				names[i] = _dataReader.GetName(i);

			return names;
		}

		public void GetValues(object o, object[] values)
		{
			_dataReader.GetValues(values);
		}

		#endregion
	}
}
