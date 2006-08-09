using System;
using System.Collections;
using System.Data;

namespace BLToolkit.Mapping
{
	public class DbDataParameterListMapper : MapDataSourceDestinationBase, IEnumerable
	{
		static readonly Type[] DbTypes = new Type[]
		{
			typeof(String),   // AnsiString
			typeof(Byte[]),   // Binary
			typeof(Byte),     // Byte
			typeof(Boolean),  // Boolean
			typeof(Decimal),  // Currency
			typeof(DateTime), // Date
			typeof(DateTime), // DateTime
			typeof(Decimal),  // Decimal
			typeof(Double),   // Double
			typeof(Guid),     // Guid
			typeof(Int16),    // Int16
			typeof(Int32),    // Int32
			typeof(Int64),    // Int64
			typeof(Object),   // Object
			typeof(SByte),    // SByte
			typeof(Single),   // Single
			typeof(String),   // String
			typeof(DateTime), // Time
			typeof(UInt16),   // UInt16
			typeof(UInt32),   // UInt32
			typeof(UInt64),   // UInt64
			typeof(Decimal),  // VarNumeric
			typeof(String),   // AnsiStringFixedLength
			typeof(String),   // StringFixedLength
			typeof(String),   // Xml
		};

		IList                   _parameters;

		public DbDataParameterListMapper(IList parameters)
		{
			_parameters = parameters;
		}

		private IDbDataParameter this[int index]
		{
			get { return (IDbDataParameter)_parameters[index]; }
		}

		#region IMapDataSource Members

		public override bool SupportsTypedValues(int index)
		{
			// IDbDataParameter.Value is boxed anyway.
			//
			return false;
		}

		public override void SetValue(object o, int index, object value)
		{
			this[index].Value = value;
		}

		public override void SetValue(object o, string name, object value)
		{
			SetValue(o, GetOrdinal(name), value);
		}

		public override int Count
		{
			get { return _parameters.Count; }
		}

		public override Type GetFieldType(int index)
		{
			return DbTypes[(int)this[index].DbType];
		}

		public override string GetName(int index)
		{
			return this[index].ParameterName;
		}

		#endregion

		#region IMapDataDestination Members

		public override int GetOrdinal(string name)
		{
			name = name.ToLower();

			for (int index = 0; index < _parameters.Count; ++index)
			{
				if (this[index].ParameterName.ToLower() == name)
					return index;
			}

			return -1;
		}

		public override object GetValue(object o, int index)
		{
			return this[index].Value;
		}

		public override object GetValue(object o, string name)
		{
			return GetValue(o, GetOrdinal(name));
		}

		#endregion

		#region IEnumerable Members

		///<summary>
		///Returns an enumerator that iterates through a collection.
		///</summary>
		///<returns>
		///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
		///</returns>
		public IEnumerator GetEnumerator()
		{
			return _parameters.GetEnumerator();
		}

		#endregion
	}
}
