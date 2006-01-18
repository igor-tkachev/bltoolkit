using System;
using System.Data;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class DataReaderListMapper : IMapDataSourceList
	{
		public DataReaderListMapper(DataReaderMapper mapper)
		{
			_mapper = mapper;
		}

		private DataReaderMapper _mapper;

		public virtual void InitMapping(InitContext initContext)
		{
			initContext.DataSource   = _mapper;
			initContext.SourceObject = _mapper.DataReader;
		}

		public virtual bool SetNextDataSource(InitContext initContext)
		{
			return _mapper.DataReader.Read();
		}

		public virtual void EndMapping(InitContext initContext)
		{
		}
	}
}
