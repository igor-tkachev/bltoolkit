using System;
using System.Globalization;
using System.Text;
using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace HowTo.Mapping
{
	public class JsonWriter : MapDataDestinationBase, IMapDataDestinationList , ISupportMapping
	{
		private string[]               _fieldNames;
		private readonly StringBuilder _sb;
		private MappingSchema          _mappingSchema;
		private bool                   _scalar;
		private bool                   _first;
		private int                    _indent;

		public JsonWriter() : this(new StringBuilder(), 0)
		{
		}

		public JsonWriter(StringBuilder sb) : this(sb, 0)
		{
		}

		public JsonWriter(StringBuilder sb, int indent)
		{
			_first  = true;
			_sb     = sb;
			_indent = indent;
		}

		public override Type GetFieldType(int index)
		{
			// Same as typeof(object)
			//
			return null;
		}

		public override int GetOrdinal(string name)
		{
			return Array.IndexOf(_fieldNames, name);
		}

		public override void SetValue(object o, int index, object value)
		{
			SetValue(o, _fieldNames[index], value);
		}

		public override void SetValue(object o, string name, object value)
		{
			if (!_scalar)
			{
				if (_first)
					_first = false;
				else
					_sb
						.Append(',')
						.AppendLine()
						;

				for (int i = 0; i < _indent; ++i)
					_sb.Append(' ');

				_sb
					.Append('"')
					.Append(name)
					.Append("\":")
					;
			}

			if (value == null)
				_sb.Append("null");
			else
			{
				switch (Type.GetTypeCode(value.GetType()))
				{
					case TypeCode.Empty:
					case TypeCode.DBNull:
						_sb.Append("null");
						break;
					case TypeCode.Boolean:
						_sb.Append((bool)value? "true": "false");
						break;
					case TypeCode.Char:
						_sb
							.Append('\'')
							.Append((char)value)
							.Append('\'')
							;
						break;
					case TypeCode.SByte:
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Byte:
					case TypeCode.UInt16:
					case TypeCode.UInt32:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						_sb.Append(((IFormattable)value).ToString(null, CultureInfo.InvariantCulture));
						break;
					case TypeCode.DateTime:
						_sb
							.Append("new Date(")
							.Append(((DateTime)value - new DateTime(1970, 1, 1)).Ticks)
							.Append(")");
						break;
					case TypeCode.String:
						_sb
							.Append('"')
							.Append(((string)value).Replace("\r\n", "\\r")
								.Replace("\n\r", "\\r")
								.Replace("\n", "\\r")
								.Replace("\r", "\\r")
								.Replace("\"","\\\""))
							.Append('"')
							;
						break;
					default:
						JsonWriter inner = new JsonWriter(_sb, _indent + 1);

						if (value.GetType().IsArray)
							_mappingSchema.MapSourceListToDestinationList(
								_mappingSchema.GetDataSourceList(value), inner);
						else
							_mappingSchema.MapSourceToDestination(
								_mappingSchema.GetDataSource(value), value, inner, inner);

						break;
				}
			}
		}

		#region ISupportMapping Members

		void ISupportMapping.BeginMapping(InitContext initContext)
		{
			_mappingSchema = initContext.MappingSchema;
			_fieldNames    = new string[initContext.DataSource.Count];

			for (int i = 0; i < _fieldNames.Length; ++i)
				_fieldNames[i] = initContext.DataSource.GetName(i);

			_scalar = _fieldNames.Length == 1 && string.IsNullOrEmpty(_fieldNames[0]);

			if (_scalar)
				return;

			if (_fieldNames.Length <= 1)
			{
				// Reset the indent since output is a single line.
				//
				_indent = 0;
				_sb.Append('{');
			}
			else
			{
				if (_indent > 0)
					_sb.AppendLine();

				for (int i = 0; i < _indent; ++i)
					_sb.Append(' ');

				_sb
					.Append('{')
					.AppendLine()
					;
			}
		}

		void ISupportMapping.EndMapping(InitContext initContext)
		{
			if (_scalar)
				return;

			if (_fieldNames.Length > 1)
				_sb.AppendLine();

			for (int i = 0; i < _indent; ++i)
				_sb.Append(' ');
			_sb.Append('}');
		}

		#endregion

		#region IMapDataDestinationList Members

		void IMapDataDestinationList.InitMapping(InitContext initContext)
		{
			_sb.Append('[');
		}

		IMapDataDestination IMapDataDestinationList.GetDataDestination(InitContext initContext)
		{
			return this;
		}

		object IMapDataDestinationList.GetNextObject(InitContext initContext)
		{
			if (_first)
				_first = false;
			else
				_sb.Append(',');

			return this;
		}

		void IMapDataDestinationList.EndMapping(InitContext initContext)
		{
			_sb.Append(']');
		}

		#endregion

		public override string ToString()
		{
			return _sb.ToString();
		}
	}

	[TestFixture]
	public class MapToJson
	{
		public class Inner
		{
			public string Name = "inner \"object \n name";
		}

		public class Inner2
		{
			public string Name;
			public int    Value;
		}

		public class SourceObject
		{
			public string   Foo = "Foo";
			public double   Bar  = 1.23;
			public DateTime Baz  = DateTime.Today;
			[MapIgnore(false)]
			public Inner    Inner = new Inner();
			[MapIgnore(false)]
			public Inner2   Inner2 = new Inner2();
			public string[] StrArray = {"One", "Two", "Three"};
		}

		[Test]
		public void Test()
		{
			JsonWriter    jw = new JsonWriter(new StringBuilder(256));

			Map./*[a]*/MapSourceToDestination/*[/a]*/(Map.GetObjectMapper(typeof(SourceObject)), new SourceObject(), jw, jw);
			Console.Write(jw.ToString());

			// Expected output:
			//
			// {
			// "Foo":"Foo",
			// "Bar":1.23,
			// "Baz":new Date(11823840000000000),
			// "Inner":{ "Name":"inner \"object \r name"},
			// "Inner2":
			//  {
			//  "Name":null,
			//  "Value":0
			//  },
			//  "StrArray":["One","Two","Three"]
			// }
		}
	}
}
