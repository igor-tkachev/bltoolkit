using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;

namespace BLToolkit.Mapping
{
	public class TextDataReader : IDataReader
	{
		#region Constructors

		public TextDataReader(Stream stream)
			: this(stream, Map.DefaultSchema)
		{
		}

		public TextDataReader(Stream stream, MappingSchema mappingSchema)
		{
			GC.SuppressFinalize(this);

			if (mappingSchema == null) throw new ArgumentNullException("mappingSchema");

			_reader        = new StreamReader(stream);
			_mappingSchema = mappingSchema;

			ReadHeader();
		}

		#endregion

		#region Protected Members

		private readonly StreamReader  _reader;
		private readonly MappingSchema _mappingSchema;
		private          string        _line       = string.Empty;
		private          string[]      _names      = _empty;
		private          string[]      _values     = _empty;
		private          int           _lineNumber = 0;

		private static readonly string[] _empty = new string[0];

		private bool IsEof
		{
			get { return _line == null; }
		}

		private bool ReadNextLine()
		{
			while (!IsEof)
			{
				_line = _reader.ReadLine();
				_lineNumber++;

				if (!string.IsNullOrEmpty(_line) && _line[0] == '*')
					return true;
			}

			return false;
		}

		private void ReadHeader()
		{
			while (ReadNextLine())
			{
				if (_line.StartsWith("*:"))
				{
					_names  = _line.Substring(2).Split(':');
					_values = new string[_names.Length];

					for (int i = 0; i < _names.Length; i++)
						_names[i] = _names[i].Trim();
				}
				else if (_line.StartsWith("**") || _line.StartsWith("*-"))
					break;
			}
		}

		private bool ReadRecord()
		{
			if (!IsEof)
			{
				if (_line.StartsWith("*-"))
					return false;

				if (_line.StartsWith("**") && _line.Length > 3)
				{
					string[] values = _line.Substring(3).Split(_line[2]);

					for (int i = 0; i < _values.Length && i < values.Length; i++)
					{
						string value = values[i];

						_values[i] =
							value.Length == 0? null:
							value[0] == '*'?   value.Substring(1):
							value[0] == '+'?   Encoding.Unicode.GetString(Convert.FromBase64String(value.Substring(1))):
							                   value;
					}

					ReadNextLine();

					return true;
				}

				throw new MappingException(
					string.Format("Invalid data format in the line {0}.", _lineNumber));
			}

			return false;
		}

		#endregion

		#region IDataReader Members

		public virtual void Close()
		{
			_line = null;
		}

		public virtual int Depth
		{
			get { return 0; }
		}

		public virtual Type GetFieldType(int index)
		{
			return typeof(string);
		}

		public virtual string GetName(int index)
		{
			return _names[index];
		}

		private DataTable _schemaTable;

		public virtual DataTable GetSchemaTable()
		{
			if (_schemaTable == null)
			{
				_schemaTable = new DataTable("SchemaTable");

				_schemaTable.Columns.AddRange(new DataColumn[]
				{
					new DataColumn(SchemaTableColumn.ColumnName,               typeof(string)),
					new DataColumn(SchemaTableColumn.ColumnOrdinal,            typeof(int)),
					new DataColumn(SchemaTableColumn.ColumnSize,               typeof(int)),
					new DataColumn(SchemaTableColumn.NumericPrecision,         typeof(short)),
					new DataColumn(SchemaTableColumn.NumericScale,             typeof(short)),
					new DataColumn(SchemaTableColumn.DataType,                 typeof(Type)),
					new DataColumn(SchemaTableColumn.NonVersionedProviderType, typeof(int)),
					new DataColumn(SchemaTableColumn.ProviderType,             typeof(int)),
					new DataColumn(SchemaTableColumn.IsLong,                   typeof(bool)),
					new DataColumn(SchemaTableColumn.AllowDBNull,              typeof(bool)),
					new DataColumn(SchemaTableColumn.IsUnique,                 typeof(bool)),
					new DataColumn(SchemaTableColumn.IsKey,                    typeof(bool)),
					new DataColumn(SchemaTableColumn.BaseSchemaName,           typeof(string)),
					new DataColumn(SchemaTableColumn.BaseTableName,            typeof(string)),
					new DataColumn(SchemaTableColumn.BaseColumnName,           typeof(string)),
					new DataColumn(SchemaTableColumn.IsAliased,                typeof(bool)),
					new DataColumn(SchemaTableColumn.IsExpression,             typeof(bool)),
				});

				for (int i = 0; i < _names.Length; i++)
				{
					DataRow row = _schemaTable.NewRow();

					row[SchemaTableColumn.ColumnName]               = _names[i];
					row[SchemaTableColumn.ColumnOrdinal]            = i;
					row[SchemaTableColumn.ColumnSize]               = (int)byte.MaxValue;
					row[SchemaTableColumn.NumericPrecision]         = (short)0;
					row[SchemaTableColumn.NumericScale]             = (short)0;
					row[SchemaTableColumn.DataType]                 = typeof(string);
					row[SchemaTableColumn.NonVersionedProviderType] = 1;
					row[SchemaTableColumn.ProviderType]             = 1;
					row[SchemaTableColumn.IsLong]                   = false;
					row[SchemaTableColumn.AllowDBNull]              = true;
					row[SchemaTableColumn.IsUnique]                 = false;
					row[SchemaTableColumn.IsKey]                    = false;
					row[SchemaTableColumn.BaseSchemaName]           = string.Empty;
					row[SchemaTableColumn.BaseTableName]            = string.Empty;
					row[SchemaTableColumn.BaseColumnName]           = string.Empty;
					row[SchemaTableColumn.IsAliased]                = false;
					row[SchemaTableColumn.IsExpression]             = false;

					_schemaTable.Rows.Add(row);
				}
			}

			return _schemaTable;
		}

		public virtual int FieldCount
		{
			get { return _names.Length; }
		}

		public virtual bool IsClosed
		{
			get { return IsEof; }
		}

		public virtual bool NextResult()
		{
			ReadHeader();
			return !IsEof;
		}

		public virtual bool Read()
		{
			return ReadRecord();
		}

		public virtual int RecordsAffected
		{
			get { return -1; }
		}

		#endregion

		#region IDisposable Members

		public virtual void Dispose()
		{
		}

		#endregion

		#region IDataRecord Members

		public virtual bool GetBoolean(int i)
		{
			return _mappingSchema.ConvertToBoolean(_values[i]);
		}

		public virtual byte GetByte(int i)
		{
			return _mappingSchema.ConvertToByte(_values[i]);
		}

		public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public virtual char GetChar(int i)
		{
			return _mappingSchema.ConvertToChar(_values[i]);
		}

		public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public virtual IDataReader GetData(int i)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public virtual string GetDataTypeName(int i)
		{
			return typeof(string).FullName;
		}

		public virtual DateTime GetDateTime(int i)
		{
			return _mappingSchema.ConvertToDateTime(_values[i]);
		}

#if FW3
		public virtual DateTimeOffset GetDateTimeOffset(int i)
		{
			return _mappingSchema.ConvertToDateTimeOffset(_values[i]);
		}
#endif

		public virtual decimal GetDecimal(int i)
		{
			return _mappingSchema.ConvertToDecimal(_values[i]);
		}

		public virtual double GetDouble(int i)
		{
			return _mappingSchema.ConvertToDouble(_values[i]);
		}

		public virtual float GetFloat(int i)
		{
			return _mappingSchema.ConvertToSingle(_values[i]);
		}

		public virtual Guid GetGuid(int i)
		{
			return _mappingSchema.ConvertToGuid(_values[i]);
		}

		public virtual short GetInt16(int i)
		{
			return _mappingSchema.ConvertToInt16(_values[i]);
		}

		public virtual int GetInt32(int i)
		{
			return _mappingSchema.ConvertToInt32(_values[i]);
		}

		public virtual long GetInt64(int i)
		{
			return _mappingSchema.ConvertToInt64(_values[i]);
		}

		public virtual int GetOrdinal(string name)
		{
			for (int i = 0; i < _names.Length; i++)
				if (_names[i] == name)
					return i;

			return -1;
		}

		public virtual string GetString(int i)
		{
			return _values[i];
		}

		public virtual object GetValue(int i)
		{
			return _values[i];
		}

		public virtual int GetValues(object[] values)
		{
			int n = Math.Min(values.Length, _values.Length);

			for (int i = 0; i < n; i++)
				values[i] = _values[i];

			return n;
		}

		public virtual bool IsDBNull(int i)
		{
			return _values[i] == null;
		}

		public virtual object this[string name]
		{
			get
			{
				for (int i = 0; i < _names.Length; i++)
					if (_names[i] == name)
						return _values[i];

				throw new ArgumentException(string.Format("Invalid field name '{0}'", name));
			}
		}

		public virtual object this[int i]
		{
			get { return _values[i]; }
		}

		#endregion
	}
}
