using System;
using System.Collections;

using BLToolkit.Mapping;

namespace BLToolkit.Data
{
	internal class ParameterReader : IMapDataDestination
	{
		DbManager _db;

		public ParameterReader(DbManager db)
		{
			_db = db;
		}

		private ArrayList _paramNames = new ArrayList();

		private ArrayList _paramList = new ArrayList();
		public  ArrayList  ParamList
		{
			get { return _paramList; }
		}

		#region IDataReceiver Members

		public int GetOrdinal(string name)
		{
			return _paramNames.Add(name);
		}

		public void SetValue(object o, int index, object value)
		{
			if (value == null || value.GetType().IsClass == false || value is string || value is byte[])
				_paramList.Add(_db.Parameter("@" + _paramNames[index], value == null? DBNull.Value: value));
		}

		public void SetValue(object o, string name, object value)
		{
			if (value == null || value.GetType().IsClass == false || value is string || value is byte[])
				_paramList.Add(_db.Parameter("@" + name, value == null? DBNull.Value: value));
		}

		#endregion
	}
}
