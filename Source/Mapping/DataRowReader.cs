/*
 * File:    DataRowReader.cs
 * Created: 07/27/2002
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Data;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public class DataRowReader : IMapDataSource, IMapDataReceiver
	{
		bool           _createColumns;
		DataRowVersion _version;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataRow"></param>
		public DataRowReader(DataRow dataRow)
			: this(dataRow, DataRowVersion.Default)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataRow"></param>
		/// <param name="version"></param>
		public DataRowReader(DataRow dataRow, DataRowVersion version)
		{
			_version = version;

			Init(dataRow);
		}

		void Init(DataRow dataRow)
		{
			if (_dataRow == null && dataRow != null)
			{
				_createColumns = dataRow.Table.Columns.Count == 0;
			}

			_dataRow = dataRow;
		}

		private DataRow _dataRow;
		/// <summary>
		/// 
		/// </summary>
		public  DataRow DataRow
		{
			get { return _dataRow; }
			set { Init(value);     }
		}

		#region IDataSource Members

		int IMapDataSource.FieldCount
		{
			get { return _dataRow.Table.Columns.Count; }
		}

		string IMapDataSource.GetFieldName(int i)
		{
			return _dataRow.Table.Columns[i].ColumnName;
		}

		object IMapDataSource.GetFieldValue(int i, object entity)
		{
			return _dataRow[i, _version];
		}

		object IMapDataSource.GetFieldValue(string name, object entity)
		{
			return _dataRow[name, _version];
		}

		#endregion

		#region IDataReceiver Members

		int IMapDataReceiver.GetOrdinal(string name)
		{
			DataColumnCollection cc = _dataRow.Table.Columns;

			int idx = cc.IndexOf(name);

			if (idx < 0 && _createColumns)
			{
				idx = cc.IndexOf(cc.Add(name));
			}

			return idx;
		}

		void IMapDataReceiver.SetFieldValue(int i, string name, object entity, object value)
		{
			if (value == null || value is DBNull)
			{
				_dataRow[i] = DBNull.Value;
			}
			else
			{
				DataColumn dc = _dataRow.Table.Columns[i];

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

				_dataRow[i] = value;
			}
		}

		#endregion
	}
}
