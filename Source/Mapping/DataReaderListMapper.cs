using System;
using System.Data;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public sealed class DataReaderListMapper : IMapDataSourceList
	{
		public DataReaderListMapper(IDataReader dataReader)
		{
			_reader = dataReader;
			_mapper = new DataReaderMapper(dataReader);
		}

		private IDataReader      _reader;
		private DataReaderMapper _mapper;

		void IMapDataSourceList.InitMapping(InitContext initContext)
		{
			initContext.DataSource   = _mapper;
			initContext.SourceObject = _reader;
		}

		bool IMapDataSourceList.SetNextDataSource(InitContext initContext)
		{
			return _reader.Read();
		}

		void IMapDataSourceList.EndMapping(InitContext initContext)
		{
		}
	}
}
