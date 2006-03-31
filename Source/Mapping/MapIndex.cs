using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class MapIndex
	{
		public MapIndex(params string[] fields)
		{
			_fields = fields;
		}

		private string[] _fields;
		public  string[]  Fields
		{
			get { return _fields; }
		}

		private string _id;
		public  string  ID
		{
			get 
			{
				if (_id == null)
					_id = string.Join(".", _fields);

				return _id;
			}
		}

		public object GetValue(IMapDataSource source, object obj, int index)
		{
			object value = source.GetValue(obj, _fields[index]);

			if (value == null)
			{
				ObjectMapper objectMapper = source as ObjectMapper;

				if (objectMapper != null)
				{
					MemberMapper mm = objectMapper[_fields[index]];

					if (mm == null)
						throw new MappingException(string.Format("Type '{0}' does not contain field '{1}'.",
							objectMapper.TypeAccessor.OriginalType.Name, Fields[index]));
				}
			}

			return value;
		}

		public object GetValueOrIndex(IMapDataSource source, object obj)
		{
			if (Fields.Length == 1)
				return GetValue(source, obj, 0);

			return GetIndexValue(source, obj);
		}

		public IndexValue GetIndexValue(IMapDataSource source, object obj)
		{
			object[] values = new string[Fields.Length];

			for (int i = 0; i < values.Length; i++)
				values[i] = GetValue(source, obj, i);

			return new IndexValue(values);
		}
	}
}

