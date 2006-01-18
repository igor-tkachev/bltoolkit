using System;
using System.Collections;
using System.Data;

using BLToolkit.Mapping;

namespace BLToolkit.Data
{
	public class ParameterReader : IMapDataDestination, IEnumerable
	{
		DbManager _db;

		public ParameterReader(DbManager db)
		{
			_db = db;
		}

		private ArrayList _paramNames = new ArrayList();
		private ArrayList _paramList  = new ArrayList();

		public virtual void AddParameter(IDbDataParameter parameter)
		{
			_paramList.Add(parameter);
		}

		public virtual IDbDataParameter[] GetParameters()
		{
			return (IDbDataParameter[])_paramList.ToArray(typeof(IDbDataParameter));
		}

		private string GetName(string name)
		{
			return _db.DataProvider.GetParameterName(name);
		}

		#region IDataReceiver Members

		public virtual int GetOrdinal(string name)
		{
			return _paramNames.Add(name);
		}

		public virtual void SetValue(object o, int index, object value)
		{
			if (value == null || value.GetType().IsClass == false || value is string || value is byte[])
				_paramList.Add(_db.Parameter(GetName(_paramNames[index].ToString()), value == null? DBNull.Value: value));
		}

		public virtual void SetValue(object o, string name, object value)
		{
			if (value == null || value.GetType().IsClass == false || value is string || value is byte[])
				_paramList.Add(_db.Parameter(GetName(name), value == null? DBNull.Value: value));
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _paramList.GetEnumerator();
		}

		#endregion
	}
}
