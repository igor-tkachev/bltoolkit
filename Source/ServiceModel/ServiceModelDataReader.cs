using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace BLToolkit.ServiceModel
{
	using Common;

	class ServiceModelDataReader : IDataReader
	{
		public ServiceModelDataReader(LinqServiceResult result)
		{
			_result = result;

			for (var i = 0; i < result.FieldNames.Length; i++)
				_ordinal.Add(result.FieldNames[i], i);
		}

		readonly LinqServiceResult      _result;
		readonly Dictionary<string,int> _ordinal = new Dictionary<string,int>();

		string[] _data;
		int      _current = -1;

		#region IDataReader Members

		public void Close()
		{
		}

		public int Depth
		{
			get { return 0; }
		}

#if !SILVERLIGHT

		public DataTable GetSchemaTable()
		{
			throw new InvalidOperationException();
		}

#endif

		public bool IsClosed
		{
			get { throw new InvalidOperationException(); }
		}

		public bool NextResult()
		{
			throw new InvalidOperationException();
		}

		public bool Read()
		{
			if (++_current < _result.RowCount)
			{
				_data = _result.Data[_current];

				return true;
			}

			_data = null;

			return false;
		}

		public int RecordsAffected
		{
			get { throw new InvalidOperationException(); }
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion

		#region IDataRecord Members

		public int FieldCount
		{
			get { return _result.FieldCount; }
		}

		public bool GetBoolean(int i)
		{
			return bool.Parse(_data[i]);
		}

		public byte GetByte(int i)
		{
			return byte.Parse(_data[i]);
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new InvalidOperationException();
		}

		public char GetChar(int i)
		{
			return _data[i][0];
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new InvalidOperationException();
		}

		public IDataReader GetData(int i)
		{
			throw new InvalidOperationException();
		}

		public string GetDataTypeName(int i)
		{
			return _result.FieldTypes[i].FullName;
		}

		public DateTime GetDateTime(int i)
		{
			return DateTime.Parse(_data[i], CultureInfo.InvariantCulture);
		}

		public decimal GetDecimal(int i)
		{
			return decimal.Parse(_data[i], CultureInfo.InvariantCulture);
		}

		public double GetDouble(int i)
		{
			return double.Parse(_data[i], CultureInfo.InvariantCulture);
		}

		public Type GetFieldType(int i)
		{
			return _result.FieldTypes[i];
		}

		public float GetFloat(int i)
		{
			return float.Parse(_data[i], CultureInfo.InvariantCulture);
		}

		public Guid GetGuid(int i)
		{
			return new Guid(_data[i]);
		}

		public short GetInt16(int i)
		{
			return short.Parse(_data[i]);
		}

		public int GetInt32(int i)
		{
			return int.Parse(_data[i]);
		}

		public long GetInt64(int i)
		{
			return long.Parse(_data[i]);
		}

		public string GetName(int i)
		{
			return _result.FieldNames[i];
		}

		public int GetOrdinal(string name)
		{
			return _ordinal[name];
		}

		public string GetString(int i)
		{
			return _data[i];
		}

		public object GetValue(int i)
		{
			var type  = _result.FieldTypes[i];
			var value = _data[i];

			if (_result.VaryingTypes.Length > 0 && !string.IsNullOrEmpty(value) && value[0] == '\0')
			{
				type  = _result.VaryingTypes[value[1]];
				value = value.Substring(2);
			}

			if (type.IsArray && type == typeof(byte[]))
				return value == null ? null : System.Convert.FromBase64String(value);

            if (type == typeof(double) || type == typeof(double?))
            {
                return value == null ? null : (object)double.Parse(value, CultureInfo.InvariantCulture);
            }
            else if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return value == null ? null : (object)decimal.Parse(value, CultureInfo.InvariantCulture);
            }
            else if (type == typeof(float) || type == typeof(float?))
            {
                return value == null ? null : (object)float.Parse(value, CultureInfo.InvariantCulture);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return value == null ? null : (object)DateTime.Parse(value, CultureInfo.InvariantCulture);
            }
            else if (type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
            {
                return value == null ? null : (object)DateTimeOffset.Parse(value, CultureInfo.InvariantCulture);
            }
            return Convert.ChangeTypeFromString(value, type);
		}

		public int GetValues(object[] values)
		{
			throw new InvalidOperationException();
		}

		public bool IsDBNull(int i)
		{
			return _data[i] == null;
		}

		public object this[string name]
		{
			get { return GetValue(GetOrdinal(name)); }
		}

		public object this[int i]
		{
			get { return GetValue(i); }
		}

		#endregion
	}
}
