using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace BLToolkit.Data.DataProvider
{
	/// <summary>
	/// Implements access to the Data Provider for Oracle.
	/// </summary>
	/// <remarks>
	/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
	/// </remarks>
	/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
	public class OdpDataProvider : DataProviderBase
	{
		private delegate object GetValueDelegate(object obj);

		private static Dictionary<Type, OracleDbType>     _dicOraDbTypes;
		private static Dictionary<Type, Type>             _dicOraObjectTypes;
		private static Dictionary<Type, GetValueDelegate> _dicOraObjectValueAccessors;

		static OdpDataProvider()
		{
			_dicOraDbTypes = new Dictionary<Type, OracleDbType>(20);
			_dicOraDbTypes.Add(typeof(bool),      OracleDbType.Char);
			_dicOraDbTypes.Add(typeof(byte),      OracleDbType.Byte);
			_dicOraDbTypes.Add(typeof(sbyte),     OracleDbType.Byte);
			_dicOraDbTypes.Add(typeof(short),     OracleDbType.Int16);
			_dicOraDbTypes.Add(typeof(ushort),    OracleDbType.Int16);
			_dicOraDbTypes.Add(typeof(int),       OracleDbType.Int32);
			_dicOraDbTypes.Add(typeof(uint),      OracleDbType.Int32);
			_dicOraDbTypes.Add(typeof(long),      OracleDbType.Int64);
			_dicOraDbTypes.Add(typeof(ulong),     OracleDbType.Int64);
			_dicOraDbTypes.Add(typeof(float),     OracleDbType.Single);
			_dicOraDbTypes.Add(typeof(double),    OracleDbType.Double);
			_dicOraDbTypes.Add(typeof(decimal),   OracleDbType.Decimal);
			_dicOraDbTypes.Add(typeof(char),      OracleDbType.Char);
			_dicOraDbTypes.Add(typeof(string),    OracleDbType.NVarchar2);
			_dicOraDbTypes.Add(typeof(DateTime),  OracleDbType.Date);
			_dicOraDbTypes.Add(typeof(Guid),      OracleDbType.Raw);
			_dicOraDbTypes.Add(typeof(byte[]),    OracleDbType.Raw);
			_dicOraDbTypes.Add(typeof(char[]),    OracleDbType.NVarchar2);

			_dicOraObjectTypes = new Dictionary<Type, Type>(20);
			_dicOraObjectTypes.Add(typeof(OracleString),       typeof(string));
			_dicOraObjectTypes.Add(typeof(OracleDate),         typeof(DateTime));
			_dicOraObjectTypes.Add(typeof(OracleDecimal),      typeof(decimal));
			_dicOraObjectTypes.Add(typeof(OracleIntervalDS),   typeof(TimeSpan));
			_dicOraObjectTypes.Add(typeof(OracleIntervalYM),   typeof(long));
			_dicOraObjectTypes.Add(typeof(OracleRefCursor),    typeof(OracleDataReader));
			_dicOraObjectTypes.Add(typeof(OracleTimeStamp),    typeof(DateTime));
			_dicOraObjectTypes.Add(typeof(OracleTimeStampTZ),  typeof(DateTime));
			_dicOraObjectTypes.Add(typeof(OracleTimeStampLTZ), typeof(DateTime));
			_dicOraObjectTypes.Add(typeof(OracleBFile),        typeof(byte[]));
			_dicOraObjectTypes.Add(typeof(OracleBinary),       typeof(byte[]));
			_dicOraObjectTypes.Add(typeof(OracleBlob),         typeof(byte[]));
			_dicOraObjectTypes.Add(typeof(OracleXmlType),      typeof(string));
			_dicOraObjectTypes.Add(typeof(OracleXmlStream),    typeof(string));
			_dicOraObjectTypes.Add(typeof(OracleClob),         typeof(string));

			_dicOraObjectValueAccessors = new Dictionary<Type, GetValueDelegate>(20);
			_dicOraObjectValueAccessors.Add(typeof(OracleString),       delegate(object obj) { return ((OracleString)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleDate),         delegate(object obj) { return ((OracleDate)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleDecimal),      delegate(object obj) { return ((OracleDecimal)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleIntervalDS),   delegate(object obj) { return ((OracleIntervalDS)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleIntervalYM),   delegate(object obj) { return ((OracleIntervalYM)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleRefCursor),    delegate(object obj) { return ((OracleRefCursor)obj).GetDataReader(); });
			_dicOraObjectValueAccessors.Add(typeof(OracleTimeStamp),    delegate(object obj) { return ((OracleTimeStamp)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleTimeStampTZ),  delegate(object obj) { return ((OracleTimeStampTZ)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleTimeStampLTZ), delegate(object obj) { return ((OracleTimeStampLTZ)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleBFile),        delegate(object obj) { return ((OracleBFile)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleBinary),       delegate(object obj) { return ((OracleBinary)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleBlob),         delegate(object obj) { return ((OracleBlob)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleXmlType),      delegate(object obj) { return ((OracleXmlType)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleXmlStream),    delegate(object obj) { return ((OracleXmlStream)obj).Value; });
			_dicOraObjectValueAccessors.Add(typeof(OracleClob),         delegate(object obj) { return ((OracleClob)obj).Value; });
		}

		private static OracleDbType ConvertToOracleDbType(Type parameterType)
		{
			if (parameterType.IsEnum)
			{
				// All enums are numeric values internally
				return OracleDbType.Decimal;
			}

			OracleDbType ret;
			if (_dicOraDbTypes.TryGetValue(parameterType, out ret))
			{
				return ret;
			}
				
			// PB 01/09/2005 No MyVeryOwnObjectType please!
			// Only strings, numeric values, dates & raw data
			Debug.Fail("Unimplemented type: " + parameterType.ToString());
			return OracleDbType.NVarchar2;
		}

		private static object ConvertFromOracleValue(object value)
		{
			GetValueDelegate ret;
			if (null != value && _dicOraObjectValueAccessors.TryGetValue(value.GetType(), out ret))
			{
				return ret(value);
			}

			return value;
		}

		private static Type ConvertFromOracleType(Type elementType)
		{
			Type ret;
			if (_dicOraObjectTypes.TryGetValue(elementType, out ret))
			{
				return ret;
			}

			// PB 01/09/2005 No MyVeryOwnObjectType please!
			// Only strings, numeric values, dates & raw data
			Debug.Fail("Unimplemented type: " + elementType.ToString());
			return typeof(object);
		}
		
		/// <summary>
		/// Creates the database connection object.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <returns>The database connection object.</returns>
		public override IDbConnection CreateConnectionObject()
		{
			return new OracleConnectionWrap();
		}

		/// <summary>
		/// Creates the data adapter object.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <returns>A data adapter object.</returns>
		public override DbDataAdapter CreateDataAdapterObject()
		{
			return new OracleDataAdapterWrap();
		}

		/// <summary>
		/// Populates the specified IDbCommand object's Parameters collection with 
		/// parameter information for the stored procedure specified in the IDbCommand.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <param name="command">The IDbCommand referencing the stored procedure for which the parameter information is to be derived. The derived parameters will be populated into the Parameters of this command.</param>
		public override bool DeriveParameters(IDbCommand command)
		{
			if (command is OracleCommandWrap)
				OracleCommandBuilder.DeriveParameters((command as OracleCommandWrap).InnerCommand);
			else
				OracleCommandBuilder.DeriveParameters((OracleCommand)command);

			return true;
		}

		public override object Convert(object value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.NameToQueryParameter:
					return String.Concat(":", (string)value);

				case ConvertType.NameToParameter:
					return String.Concat("p", (string)value);

				case ConvertType.ParameterToName:
					Debug.Assert(value is string, "OraDirectDataProvider.Convert: value is null");
					Debug.Assert(Char.ToLower(((string)value).ToLower()[0]) == 'p', "OraDirectDataProvider.Convert: prefix 'p' not set?");
					return ((string)value).Substring(1);
				
				case ConvertType.OutputParameter:
					Array ar = value as Array;
					if (null != ar && !(ar is byte[] || ar is char[]))
					{
						Array convertedArray = Array.CreateInstance(ConvertFromOracleType(ar.GetType().GetElementType()), ar.Length);
						for (int i = 0; i < ar.Length; ++i)
						{
							convertedArray.SetValue(ConvertFromOracleValue(ar.GetValue(i)), i);
						}

						return convertedArray;
					}

					return ConvertFromOracleValue(value);
				
				default:
					return base.Convert(value, convertType);
			}
		}

		public override void AttachParameter(IDbCommand command, IDbDataParameter parameter)
		{
			OracleParameter oraParameter = (parameter is OracleParameterWrap) ? (parameter as OracleParameterWrap).InnerParameter : parameter as OracleParameter;

			if (null != oraParameter)
			{
				if (oraParameter.CollectionType == OracleCollectionType.PLSQLAssociativeArray)
				{
					if (oraParameter.Direction == ParameterDirection.Input || oraParameter.Direction == ParameterDirection.InputOutput)
					{
						if (oraParameter.Size == 0)
						{
							// Skip this parameter
							return;
						}
					}
					else if (oraParameter.Direction == ParameterDirection.Output)
					{
						if (oraParameter.DbType == DbType.String)
						{
							oraParameter.Size = 1024;
							int[] arrayBindSize = new int[oraParameter.Size];
							for (int i = 0; i < oraParameter.Size; ++i)
							{
								arrayBindSize[i] = 1024;
							}
							
							oraParameter.ArrayBindSize = arrayBindSize;
						}
						else
						{
							oraParameter.Size = 32767;
						}
					}
				}
			}
			
			base.AttachParameter(command, parameter);
		}

		public override bool IsValueParameter(IDbDataParameter parameter)
		{
			OracleParameter oraParameter = (parameter is OracleParameterWrap) ? (parameter as OracleParameterWrap).InnerParameter : parameter as OracleParameter;
			if (null != oraParameter)
			{
				if (oraParameter.OracleDbType == OracleDbType.RefCursor && oraParameter.Direction == ParameterDirection.Output)
				{
					// Ignore out ref cursors, while out parameters of other types are o.k.
					return false;
				}
			}

			return base.IsValueParameter(parameter);
		}

		/// <summary>
		/// Returns connection type.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <value>An instance of the <see cref="Type"/> class.</value>
		public override Type ConnectionType
		{
			get { return typeof(OracleConnection); }
		}

		/// <summary>
		/// Returns the data provider name.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataProvider Method</seealso>
		/// <value>Data provider name.</value>
		public override string Name
		{
			get { return "ODP"; }
		}

		#region Wrappers

		internal class OracleParameterWrap : IDbDataParameter, ICloneable, IDisposable
		{
			protected OracleParameter _innerParameter;

			private delegate bool IsTypeNotSetDelegate(OracleParameter oraParameter);
			private static IsTypeNotSetDelegate IsTypeNotSet;

			static OracleParameterWrap()
			{
				FieldInfo fi = typeof (OracleParameter).GetField("m_enumType", BindingFlags.Instance | BindingFlags.NonPublic);

				DynamicMethod dm = new DynamicMethod("xget_m_enumType", typeof(bool), new Type[] { typeof(OracleParameter) }, typeof(OracleParameter), true);
				ILGenerator generator = dm.GetILGenerator();
				generator.Emit(OpCodes.Ldarg_0);
				generator.Emit(OpCodes.Ldfld, fi);
				generator.Emit(OpCodes.Ldc_I4_1);
				generator.Emit(OpCodes.Ceq);
				generator.Emit(OpCodes.Ret);

				IsTypeNotSet = (IsTypeNotSetDelegate)dm.CreateDelegate(typeof(IsTypeNotSetDelegate));
			}

			public OracleParameterWrap(OracleParameter oracleParameter)
			{
				_innerParameter = oracleParameter;
			}

			internal OracleParameter InnerParameter
			{
				get { return _innerParameter; }
			}

			internal bool TypeNotSet
			{
				get { return IsTypeNotSet(_innerParameter); }
			}

			#region IDbDataParameter

			public byte Precision
			{
				get { return _innerParameter.Precision; }
				set { _innerParameter.Precision = value; }
			}

			public byte Scale
			{
				get { return _innerParameter.Scale; }
				set { _innerParameter.Scale = value; }
			}

			public int Size
			{
				get { return _innerParameter.Size; }
				set { _innerParameter.Size = value; }
			}

			#region IDataParameter

			public DbType DbType
			{
				get { return _innerParameter.DbType; }
				set { _innerParameter.DbType = value; }
			}

			public ParameterDirection Direction
			{
				get { return _innerParameter.Direction; }
				set { _innerParameter.Direction = value; }
			}

			public bool IsNullable
			{
				get { return _innerParameter.IsNullable; }
			}

			public string ParameterName
			{
				get { return _innerParameter.ParameterName; }
				set { _innerParameter.ParameterName = value; }
			}

			public string SourceColumn
			{
				get { return _innerParameter.SourceColumn; }
				set { _innerParameter.SourceColumn = value; }
			}

			public DataRowVersion SourceVersion
			{
				get { return _innerParameter.SourceVersion; }
				set { _innerParameter.SourceVersion = value; }
			}

			public object Value
			{
				get { return _innerParameter.Value; }
				set
				{
					Array ar = value as Array;
					if (null != ar && !(ar is Byte[] || ar is Char[]))
					{
						if (TypeNotSet)
						{
							_innerParameter.OracleDbType = ConvertToOracleDbType(value.GetType().GetElementType());
						}

						_innerParameter.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
						_innerParameter.Size = ar.Length;
					}
					else if (null != value && DBNull.Value != value && TypeNotSet)
					{
						switch (Type.GetTypeCode(value.GetType()))
						{
							case TypeCode.Boolean:
								value = ((Boolean)value) ? 'T' : 'F';
								break;
							case TypeCode.SByte:
							case TypeCode.UInt16:
							case TypeCode.UInt32:
							case TypeCode.UInt64:
								value = System.Convert.ToDecimal(value);
								break;
							case TypeCode.Object:
								if (value is Guid)
									value = ((Guid)value).ToByteArray();
								break;
						}

						_innerParameter.CollectionType = OracleCollectionType.None;
						_innerParameter.OracleDbType = ConvertToOracleDbType(value.GetType());
					}
					_innerParameter.Value = value;
				}
			}

			#endregion

			#endregion

			#region ICloneable

			public object Clone()
			{
				OracleParameterWrap clone = new OracleParameterWrap((OracleParameter)_innerParameter.Clone());
				clone._innerParameter.CollectionType = _innerParameter.CollectionType;
				return clone;
			}

			#endregion

			#region IDisposable

			public void Dispose()
			{
				_innerParameter.Dispose();
			}

			#endregion

			#region Object

			public override int GetHashCode()
			{
				return _innerParameter.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (obj is OracleParameterWrap)
				{
					return _innerParameter.Equals((obj as OracleParameterWrap).InnerParameter);
				}

				return _innerParameter.Equals(obj);
			}

			public override string ToString()
			{
				return _innerParameter.ToString();
			}

			#endregion
		}

		internal class OracleParameterCollectionWrap : IDataParameterCollection
		{
			protected IDataParameterCollection _IDataParameterCollection;
			protected OracleParameterCollection _innerParameterCollection;

			public OracleParameterCollectionWrap(OracleParameterCollection oracleParameterCollection)
			{
				_innerParameterCollection = oracleParameterCollection;
				_IDataParameterCollection = oracleParameterCollection;
			}

			internal OracleParameterCollection InnerParameterCollection
			{
				get { return _innerParameterCollection; }
			}

			#region IDataParameterCollection

			public object this[string name]
			{
				get
				{
					object oracleParameter = _IDataParameterCollection[name];
					return (oracleParameter is OracleParameter)
						?
						new OracleParameterWrap(oracleParameter as OracleParameter)
						: null;
				}
				set
				{
					if (value is OracleParameterWrap)
					{
						_IDataParameterCollection[name] = (value as OracleParameterWrap).InnerParameter;
					}
					else
					{
						((IDataParameterCollection)_innerParameterCollection)[name] = value;
					}
				}
			}

			public bool Contains(string parameterName)
			{
				return _IDataParameterCollection.Contains(parameterName);
			}


			public int IndexOf(string parameterName)
			{
				return _IDataParameterCollection.IndexOf(parameterName);
			}


			public void RemoveAt(string parameterName)
			{
				_IDataParameterCollection.RemoveAt(parameterName);
			}

			#region IList

			public bool IsFixedSize
			{
				get { return _IDataParameterCollection.IsFixedSize; }
			}


			public bool IsReadOnly
			{
				get { return _IDataParameterCollection.IsReadOnly; }
			}

			public object this[int index]
			{
				get
				{
					object oracleParameter = _IDataParameterCollection[index];
					return (oracleParameter is OracleParameter)?
						new OracleParameterWrap(oracleParameter as OracleParameter): null;
				}
				set
				{
					if (value is OracleParameterWrap)
					{
						_IDataParameterCollection[index] = (value as OracleParameterWrap).InnerParameter;
					}
					else
					{
						_IDataParameterCollection[index] = value;
					}
				}
			}

			public int Add(object value)
			{
				if (value is OracleParameterWrap)
				{
					return _IDataParameterCollection.Add((value as OracleParameterWrap).InnerParameter);
				}
				else
				{
					return _IDataParameterCollection.Add(value);
				}
			}

			public void Clear()
			{
				_IDataParameterCollection.Clear();
			}


			public bool Contains(object value)
			{
				if (value is OracleParameterWrap)
				{
					return _IDataParameterCollection.Contains((value as OracleParameterWrap).InnerParameter);
				}
				else
				{
					return _IDataParameterCollection.Contains(value);
				}
			}

			public int IndexOf(object value)
			{
				if (value is OracleParameterWrap)
				{
					return _IDataParameterCollection.IndexOf((value as OracleParameterWrap).InnerParameter);
				}
				else
				{
					return _IDataParameterCollection.IndexOf(value);
				}
			}

			public void Insert(int index, object value)
			{
				if (value is OracleParameterWrap)
				{
					_IDataParameterCollection.Insert(index, (value as OracleParameterWrap).InnerParameter);
				}
				else
				{
					_IDataParameterCollection.Insert(index, value);
				}
			}

			public void Remove(object value)
			{
				if (value is OracleParameterWrap)
				{
					_IDataParameterCollection.Remove((value as OracleParameterWrap).InnerParameter);
				}
				else
				{
					_IDataParameterCollection.Remove(value);
				}
			}

			public void RemoveAt(int index)
			{
				_IDataParameterCollection.RemoveAt(index);
			}

			#region ICollection

			public int Count
			{
				get { return _IDataParameterCollection.Count; }
			}


			public bool IsSynchronized
			{
				get { return _IDataParameterCollection.IsSynchronized; }
			}


			public object SyncRoot
			{
				get { return _IDataParameterCollection.SyncRoot; }
			}

			public void CopyTo(Array array, int index)
			{
				_IDataParameterCollection.CopyTo(array, index);
				for (int i = index; i < _IDataParameterCollection.Count; ++i)
				{
					array.SetValue(new OracleParameterWrap((OracleParameter)array.GetValue(i)), i);
				}
			}

			#region IEnumerable

			public IEnumerator GetEnumerator()
			{
				return new OracleParameterCollectionEnumeratorWrap(_IDataParameterCollection.GetEnumerator());
			}

			#endregion

			#endregion

			#endregion

			#endregion

			#region Object

			public override int GetHashCode()
			{
				return _innerParameterCollection.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (obj is OracleParameterCollectionWrap)
				{
					return _innerParameterCollection.Equals((obj as OracleParameterCollectionWrap).InnerParameterCollection);
				}

				return _innerParameterCollection.Equals(obj);
			}

			public override string ToString()
			{
				return _innerParameterCollection.ToString();
			}

			#endregion
		}

		internal class OracleParameterCollectionEnumeratorWrap : IEnumerator
		{
			protected IEnumerator _IEnumerator;

			public OracleParameterCollectionEnumeratorWrap(IEnumerator enumerator)
			{
				_IEnumerator = enumerator;
			}

			#region Object

			public override int GetHashCode()
			{
				return _IEnumerator.GetHashCode();
			}


			public override bool Equals(object obj)
			{
				if (obj is OracleParameterCollectionEnumeratorWrap)
				{
					return _IEnumerator.Equals((obj as OracleParameterCollectionEnumeratorWrap)._IEnumerator);
				}

				return _IEnumerator.Equals(obj);
			}


			public override string ToString()
			{
				return _IEnumerator.ToString();
			}

			#endregion

			#region IEnumerator

			public object Current
			{
				get
				{
					object oracleParameter = _IEnumerator.Current;
					return (oracleParameter is OracleParameter)
						? new OracleParameterWrap(oracleParameter as OracleParameter)
						: oracleParameter;
				}
			}

			public bool MoveNext()
			{
				return _IEnumerator.MoveNext();
			}

			public void Reset()
			{
				_IEnumerator.Reset();
			}

			#endregion
		}

		internal class OracleCommandWrap : IComponent, IDbCommand, ICloneable
		{
			protected OracleCommand _innerCommand;
			protected IDbCommand _IDbCommand;

			public OracleCommandWrap(OracleCommand oracleCommand)
			{
				oracleCommand.BindByName = true;

				_innerCommand = oracleCommand;
				_IDbCommand = oracleCommand;
			}

			internal OracleCommand InnerCommand
			{
				get { return _innerCommand; }
			}

			#region Object

			public override int GetHashCode()
			{
				return _innerCommand.GetHashCode();
			}


			public override bool Equals(object obj)
			{
				if (obj is OracleCommandWrap)
				{
					return _innerCommand.Equals((obj as OracleCommandWrap)._innerCommand);
				}

				return _innerCommand.Equals(obj);
			}


			public override string ToString()
			{
				return _innerCommand.ToString();
			}

			#endregion

			#region ICloneable

			public object Clone()
			{
				return new OracleCommandWrap((OracleCommand)_innerCommand.Clone());
			}

			#endregion

			#region IComponent

			public ISite Site
			{
				get { return _innerCommand.Site; }
				set { _innerCommand.Site = value; }
			}

			public event EventHandler Disposed
			{
				add { _innerCommand.Disposed += value; }
				remove { _innerCommand.Disposed -= value; }
			}

			#region IDisposable

			public void Dispose()
			{
				_innerCommand.Dispose();
			}

			#endregion

			#endregion

			#region IDbCommand

			public string CommandText
			{
				get { return _IDbCommand.CommandText; }
				set { _IDbCommand.CommandText = value; }
			}

			public int CommandTimeout
			{
				get { return _IDbCommand.CommandTimeout; }
				set { _IDbCommand.CommandTimeout = value; }
			}


			public CommandType CommandType
			{
				get { return _IDbCommand.CommandType; }
				set { _IDbCommand.CommandType = value; }
			}


			public IDbConnection Connection
			{
				get { return _IDbCommand.Connection; }
				set { _IDbCommand.Connection = value; }
			}

			public IDataParameterCollection Parameters
			{
				get { return new OracleParameterCollectionWrap(_innerCommand.Parameters); }
			}

			public IDbTransaction Transaction
			{
				get { return _IDbCommand.Transaction; }
				set { _IDbCommand.Transaction = value; }
			}


			public UpdateRowSource UpdatedRowSource
			{
				get { return _IDbCommand.UpdatedRowSource; }
				set { _IDbCommand.UpdatedRowSource = value; }
			}

			public void Cancel()
			{
				_IDbCommand.Cancel();
			}


			public IDbDataParameter CreateParameter()
			{
				return new OracleParameterWrap(_innerCommand.CreateParameter());
			}

			public int ExecuteNonQuery()
			{
				return _IDbCommand.ExecuteNonQuery();
			}


			public IDataReader ExecuteReader(CommandBehavior behavior)
			{
				return _IDbCommand.ExecuteReader(behavior);
			}


			public IDataReader ExecuteReader()
			{
				return _IDbCommand.ExecuteReader();
			}


			public object ExecuteScalar()
			{
				return _IDbCommand.ExecuteScalar();
			}


			public void Prepare()
			{
				_IDbCommand.Prepare();
			}

			#endregion
		}

		internal class OracleConnectionWrap : IComponent, IDbConnection, ICloneable
		{
			protected OracleConnection _innerConnection;
			protected IDbConnection _IDbConnection;

			public OracleConnectionWrap() : this(new OracleConnection()) { }

			public OracleConnectionWrap(OracleConnection oracleConnection)
			{
				_innerConnection = oracleConnection;
				_IDbConnection = oracleConnection;
			}

			public OracleConnection InnerConnection
			{
				get { return _innerConnection; }
			}

			#region Object

			public override int GetHashCode()
			{
				return _innerConnection.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (obj is OracleConnectionWrap)
				{
					return _innerConnection.Equals((obj as OracleConnectionWrap)._innerConnection);
				}

				return _innerConnection.Equals(obj);
			}


			public override string ToString()
			{
				return _innerConnection.ToString();
			}

			#endregion

			#region ICloneable

			public object Clone()
			{
				return new OracleConnectionWrap((OracleConnection)_innerConnection.Clone());
			}

			#endregion

			#region IComponent

			public ISite Site
			{
				get { return _innerConnection.Site; }
				set { _innerConnection.Site = value; }
			}


			public event EventHandler Disposed
			{
				add { _innerConnection.Disposed += value; }
				remove { _innerConnection.Disposed -= value; }
			}

			#region IDisposable

			public void Dispose()
			{
				_innerConnection.Dispose();
			}

			#endregion

			#endregion

			#region IDbConnection

			public string ConnectionString
			{
				get { return _IDbConnection.ConnectionString; }
				set { _IDbConnection.ConnectionString = value; }
			}


			public int ConnectionTimeout
			{
				get { return _IDbConnection.ConnectionTimeout; }
			}


			public string Database
			{
				get { return _IDbConnection.Database; }
			}


			public ConnectionState State
			{
				get { return _IDbConnection.State; }
			}

			public IDbCommand CreateCommand()
			{
				return new OracleCommandWrap(_innerConnection.CreateCommand());
			}


			public IDbTransaction BeginTransaction(IsolationLevel il)
			{
				return _IDbConnection.BeginTransaction(il);
			}


			public IDbTransaction BeginTransaction()
			{
				return _IDbConnection.BeginTransaction();
			}


			public void ChangeDatabase(string databaseName)
			{
				_IDbConnection.ChangeDatabase(databaseName);
			}


			public void Close()
			{
				_IDbConnection.Close();
			}


			public void Open()
			{
				_IDbConnection.Open();
			}

			#endregion
		}

		internal class OracleDataAdapterWrap : DbDataAdapter, IDbDataAdapter, ICloneable, IDisposable
		{
			private OracleDataAdapter _innerDataAdapter;

			public OracleDataAdapterWrap() : this(new OracleDataAdapter()) { }

			public OracleDataAdapterWrap(OracleDataAdapter oracleDataAdapter)
			{
				_innerDataAdapter = oracleDataAdapter;
			}

			public OracleDataAdapter InnerDataAdapter
			{
				get { return _innerDataAdapter; }
			}

			#region DbDataAdapter
			public override int Fill(DataSet dataSet)
			{
				return _innerDataAdapter.Fill(dataSet);
			}


			protected override int Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable, IDbCommand command,
				CommandBehavior behavior)
			{
				return _innerDataAdapter.Fill(dataSet, startRecord, maxRecords, srcTable);
			}


			protected override int Fill(DataTable[] dataTables, int startRecord, int maxRecords, IDbCommand command,
				CommandBehavior behavior)
			{
				return _innerDataAdapter.Fill(dataTables[0]);
			}
			#endregion

			#region ICloneable

			object ICloneable.Clone()
			{
				return new OracleDataAdapterWrap((OracleDataAdapter)(_innerDataAdapter as ICloneable).Clone());
			}
			#endregion

			#region IDisposable
			void IDisposable.Dispose()
			{
				_innerDataAdapter.Dispose();
				Dispose(true);
				GC.SuppressFinalize(this);
			}
			#endregion

			#region Object

			public override int GetHashCode()
			{
				return _innerDataAdapter.GetHashCode();
			}


			public override bool Equals(object obj)
			{
				if (obj is OracleDataAdapterWrap)
				{
					return _innerDataAdapter.Equals((obj as OracleDataAdapterWrap)._innerDataAdapter);
				}

				return _innerDataAdapter.Equals(obj);
			}


			public override string ToString()
			{
				return _innerDataAdapter.ToString();
			}
			#endregion

			#region IDbDataAdapter Members

			public IDbDataAdapter _IDbDataAdapter
			{
				get { return _innerDataAdapter; }
			}


			IDbCommand IDbDataAdapter.DeleteCommand
			{
				get
				{
					IDbCommand cmd = _IDbDataAdapter.DeleteCommand;
					return (cmd is OracleCommand) ? new OracleCommandWrap(cmd as OracleCommand) : cmd;
				}
				set { _IDbDataAdapter.DeleteCommand = (value is OracleCommandWrap) ? (value as OracleCommandWrap).InnerCommand : value; }
			}

			IDbCommand IDbDataAdapter.InsertCommand
			{
				get
				{
					IDbCommand cmd = _IDbDataAdapter.InsertCommand;
					return (cmd is OracleCommand) ? new OracleCommandWrap(cmd as OracleCommand) : cmd;
				}
				set { _IDbDataAdapter.InsertCommand = (value is OracleCommandWrap) ? (value as OracleCommandWrap).InnerCommand : value; }
			}

			IDbCommand IDbDataAdapter.SelectCommand
			{
				get
				{
					IDbCommand cmd = _IDbDataAdapter.SelectCommand;
					return (cmd is OracleCommand) ? new OracleCommandWrap(cmd as OracleCommand) : cmd;
				}
				set { _IDbDataAdapter.SelectCommand = (value is OracleCommandWrap) ? (value as OracleCommandWrap).InnerCommand : value; }
			}

			IDbCommand IDbDataAdapter.UpdateCommand
			{
				get
				{
					IDbCommand cmd = _IDbDataAdapter.SelectCommand;
					return (cmd is OracleCommand) ? new OracleCommandWrap(cmd as OracleCommand) : cmd;
				}
				set { _IDbDataAdapter.UpdateCommand = (value is OracleCommandWrap) ? (value as OracleCommandWrap).InnerCommand : value; }
			}

			int IDataAdapter.Fill(DataSet dataSet)
			{
				return _IDbDataAdapter.Fill(dataSet);
			}

			DataTable[] IDataAdapter.FillSchema(DataSet dataSet, SchemaType schemaType)
			{
				return _IDbDataAdapter.FillSchema(dataSet, schemaType);
			}

			IDataParameter[] IDataAdapter.GetFillParameters()
			{
				IDataParameter[] array = _innerDataAdapter.GetFillParameters();

				if (null != array)
				{
					for (int i = 0; i < array.Length; ++i)
					{
						array.SetValue(new OracleParameterWrap((OracleParameter)array.GetValue(i)), i);
					}
				}

				return array;
			}

			MissingMappingAction IDataAdapter.MissingMappingAction
			{
				get { return _IDbDataAdapter.MissingMappingAction; }
				set { _IDbDataAdapter.MissingMappingAction = value; }
			}

			MissingSchemaAction IDataAdapter.MissingSchemaAction
			{
				get { return _IDbDataAdapter.MissingSchemaAction; }
				set { _IDbDataAdapter.MissingSchemaAction = value; }
			}

			ITableMappingCollection IDataAdapter.TableMappings
			{
				get { return _IDbDataAdapter.TableMappings; }
			}

			int IDataAdapter.Update(DataSet dataSet)
			{
				return _IDbDataAdapter.Update(dataSet);
			}

			#endregion
		}

		#endregion
	}
}
