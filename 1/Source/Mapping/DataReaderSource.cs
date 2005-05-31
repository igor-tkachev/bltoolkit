/*
 * File:    DataReaderSource.cs
 * Created: 07/27/2003
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
	public class DataReaderSource : IMapDataSource
	{
		IDataReader _dataReader;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataReader"></param>
		public DataReaderSource(IDataReader dataReader)
		{
			_dataReader = dataReader;
		}

		#region IDataSource Members

		int IMapDataSource.FieldCount
		{
			get { return _dataReader.FieldCount; }
		}

		string IMapDataSource.GetFieldName(int i)
		{
			return _dataReader.GetName(i);
		}

		object IMapDataSource.GetFieldValue(int i, object entity)
		{
			return _dataReader.GetValue(i);
		}

		object IMapDataSource.GetFieldValue(string name, object entity)
		{
			return _dataReader[name];
		}
		#endregion
	}
}
