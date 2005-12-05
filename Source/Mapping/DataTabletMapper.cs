using System;
using System.Data;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public sealed class DataTabletMapper : IMapDataSourceList, IMapDataDestinationList
	{
		public DataTabletMapper(DataTable dataTable)
			: this(dataTable, DataRowVersion.Default)
		{
		}

		public DataTabletMapper(DataTable dataTable, DataRowVersion version)
		{
			_table  = dataTable;
			_mapper = new DataRowMapper(null, version);
		}

		private DataTable     _table;
		private DataRowMapper _mapper;
		private int           _currentRow;

		#region IMapDataSourceList Members

		void IMapDataSourceList.InitMapping(InitContext initContext)
		{
			initContext.DataSource = _mapper;
		}

		bool IMapDataSourceList.SetNextDataSource(InitContext initContext)
		{
			if (_currentRow >= _table.Rows.Count)
				return false;

			DataRow row = _table.Rows[_currentRow++];

			if (row.RowState == DataRowState.Deleted)
				return ((IMapDataSourceList)this).SetNextDataSource(initContext);

			_mapper.DataRow          = row;
			initContext.SourceObject = row;

			return true;
		}

		void IMapDataSourceList.EndMapping(InitContext initContext)
		{
		}

		#endregion

		#region IMapDataDestinationList Members

		void IMapDataDestinationList.InitMapping(InitContext initContext)
		{
		}

		IMapDataDestination IMapDataDestinationList.GetDataDestination(InitContext initContext)
		{
			return _mapper;
		}

		object IMapDataDestinationList.GetNextObject(InitContext initContext)
		{
			DataRow row = _table.NewRow();

			_mapper.DataRow = row;
			_table.Rows.Add(row);

			return row;
		}

		void IMapDataDestinationList.EndMapping(InitContext initContext)
		{
		}

		#endregion
	}
}
