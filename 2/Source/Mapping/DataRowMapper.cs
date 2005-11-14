using System;
using System.Data;

namespace BLToolkit.Mapping
{
	public class DataRowMapper : IMapDataSource, IMapDataDestination
	{
		bool           _createColumns;
		DataRowVersion _version;

		public DataRowMapper(DataRow dataRow)
			: this(dataRow, DataRowVersion.Default)
		{
		}

		public DataRowMapper(DataRow dataRow, DataRowVersion version)
		{
			_version = version;

			Init(dataRow);
		}

		private void Init(DataRow dataRow)
		{
			if (_dataRow == null && dataRow != null)
				_createColumns = dataRow.Table.Columns.Count == 0;

			_dataRow = dataRow;
		}

		private DataRow _dataRow;
		public  DataRow  DataRow
		{
			get { return _dataRow; }
			set { Init(value);     }
		}

		#region IMapDataSource Members

		int IMapDataSource.GetCount()
		{
			return _dataRow.Table.Columns.Count;
		}

		string IMapDataSource.GetName(int index)
		{
			return _dataRow.Table.Columns[index].ColumnName;
		}

		object IMapDataSource.GetValue(object o, int index)
		{
			return _version == DataRowVersion.Default? _dataRow[index]: _dataRow[index, _version];
		}

		object IMapDataSource.GetValue(object o, string name)
		{
			return _version == DataRowVersion.Default? _dataRow[name]: _dataRow[name, _version];
		}

		#endregion

		#region IMapDataDestination Members

		int IMapDataDestination.GetOrdinal(string name)
		{
			DataColumnCollection cc = _dataRow.Table.Columns;

			int index = cc.IndexOf(name);

			if (index < 0 && _createColumns)
				index = cc.IndexOf(cc.Add(name));

			return index;
		}

		void IMapDataDestination.SetValue(object o, int index, object value)
		{
			if (value == null || value is DBNull)
			{
				_dataRow[index] = DBNull.Value;
			}
			else
			{
				DataColumn dc = _dataRow.Table.Columns[index];

				if (dc.DataType != value.GetType())
				{
					if (dc.DataType == typeof(Guid))
					{
						value = new Guid(value.ToString());
					}
					else
					{
						if (dc.DataType != typeof(string))
							value = Convert.ChangeType(value, dc.DataType);
					}
				}

				_dataRow[index] = value;
			}
		}

		#endregion
	}
}
