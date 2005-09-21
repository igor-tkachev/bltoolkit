/*
 * File:    ParameterReader.cs
 * Created: 07/27/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Collections;

namespace Rsdn.Framework.Data
{
	internal class ParameterReader : Mapping.IMapDataReceiver
	{
		DbManager _db;

		public ParameterReader(DbManager db)
		{
			_db = db;
		}

		private ArrayList _paramList = new ArrayList();
		public  ArrayList  ParamList
		{
			get { return _paramList; }
		}

		#region IDataReceiver Members

		public int GetOrdinal(string name)
		{
			return 0;
		}

		public void SetFieldValue(int i, string name, object entity, object value)
		{
			if (value == null || value.GetType().IsClass == false || value is string || value is byte[])
				_paramList.Add(_db.Parameter("@" + name, value == null? DBNull.Value: value));
		}

		#endregion
	}
}
