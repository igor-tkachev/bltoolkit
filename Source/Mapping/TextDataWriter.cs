using System;
using System.IO;
using System.Text;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class TextDataWriter : ISupportMapping
	{
		#region Constructors

		public TextDataWriter(Stream stream, params string[] fieldNames)
			: this(stream, Map.DefaultSchema, fieldNames)
		{
		}

		public TextDataWriter(Stream stream, Type type)
			: this(stream, Map.DefaultSchema, Map.GetObjectMapper(type).FieldNames)
		{
		}

		public TextDataWriter(Stream stream, MappingSchema mappingSchema, params string[] fieldNames)
			: this(new StreamWriter(stream), mappingSchema, fieldNames)
		{
		}

		public TextDataWriter(Stream stream, MappingSchema mappingSchema, Type type)
			: this(stream, mappingSchema, mappingSchema.GetObjectMapper(type).FieldNames)
		{
		}

		public TextDataWriter(TextWriter writer, params string[] fieldNames)
			: this(writer, Map.DefaultSchema, fieldNames)
		{
		}

		public TextDataWriter(TextWriter writer, Type type)
			: this(writer, Map.DefaultSchema, Map.GetObjectMapper(type).FieldNames)
		{
		}

		public TextDataWriter(TextWriter writer, MappingSchema mappingSchema, params string[] fieldNames)
		{
			GC.SuppressFinalize(this);

			if (mappingSchema == null) throw new ArgumentNullException("mappingSchema");

			_writer        = writer;
			_names         = fieldNames;

			_values = new string[_names.Length];

			WriteHeader();
		}

		public TextDataWriter(TextWriter writer, MappingSchema mappingSchema, Type type)
			: this(writer, mappingSchema, mappingSchema.GetObjectMapper(type).FieldNames)
		{
		}

		#endregion

		#region Public Members

		public virtual void WriteEnd()
		{
			_writer.WriteLine("*-");
			_writer.Flush();
		}

		public virtual Type GetFieldType(int index)
		{
			return typeof(string);
		}

		public virtual int GetOrdinal(string name)
		{
			for (int i = 0; i < _names.Length; i++)
				if (_names[i] == name)
					return i;

			return 0;
		}

		public virtual void SetValue(int index, object value)
		{
			_values[index] = value == null? null: value.ToString();
		}

		public virtual void SetValue(string name, object value)
		{
			SetValue(GetOrdinal(name), value);
		}

		#endregion

		#region Protected Members

		private readonly TextWriter    _writer;
		private readonly string[]      _names;
		private readonly string[]      _values;

		private void WriteHeader()
		{
			_writer.Write("*");

			foreach (string name in _names)
			{
				_writer.Write(':');
				_writer.Write(name);
			}

			_writer.WriteLine();
		}

		private void WriteRecord()
		{
			_writer.Write("**");

			char[] delimiters = new char[] { ',', ':', '|', '-', '_' };
			char   delimiter  = '\0';

			foreach (char ch in delimiters)
			{
				bool found = false;

				foreach (string value in _values)
				{
					if (value != null && value.IndexOf(ch) >= 0)
					{
						found = true;
						break;
					}
				}

				if (!found)
				{
					delimiter = ch;
					break;
				}
			}

			if (delimiter == 0)
				delimiter = delimiters[0];

			char[] exChars = new char[] { delimiter, '\r', '\n', '\t', '\0' };

			foreach (string value in _values)
			{
				_writer.Write(delimiter);

				if (value != null)
				{
					if (value.Length == 0)
						_writer.Write('*');
					else
					{
						if (value.IndexOfAny(exChars) >= 0)
						{
							_writer.Write('+');
							_writer.Write(Convert.ToBase64String(Encoding.Unicode.GetBytes(value)));
						}
						else
						{
							if (value[0] == '*' || value[0] == '+')
								_writer.Write('*');

							_writer.Write(value);
						}
					}
				}
			}

			_writer.WriteLine();
			_writer.Flush();
		}

		#endregion

		#region ISupportMapping Members

		void ISupportMapping.BeginMapping(InitContext initContext)
		{
		}

		void ISupportMapping.EndMapping(InitContext initContext)
		{
			WriteRecord();
		}

		#endregion
	}
}
