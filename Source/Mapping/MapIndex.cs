using System;

using BLToolkit.Common;

namespace BLToolkit.Mapping
{
	public class MapIndex
	{
		public MapIndex(string[] names)
		{
			if (null == names)
				throw new ArgumentNullException("names");

			if (names.Length == 0)
				throw new ArgumentException("At least one field name must be specified", "names");

			_fields = NameOrIndexParameter.FromStringArray(names);
		}

		public MapIndex(int[] indices)
		{
			if (null == indices)
				throw new ArgumentNullException("indices");

			if (indices.Length == 0)
				throw new ArgumentException("At least one field ndex must be specified", "indices");

			_fields = NameOrIndexParameter.FromIndexArray(indices);
		}
		
		public MapIndex(params NameOrIndexParameter[] fields)
		{
			if (null == fields)
				throw new ArgumentNullException("fields");

			if (fields.Length == 0)
				throw new ArgumentException("At least one field name or index must be specified", "fields");
			
			_fields = fields;
		}

		private readonly NameOrIndexParameter[] _fields;
		public           NameOrIndexParameter[]  Fields
		{
			get { return _fields; }
		}

		private string _id;
		public  string  ID
		{
			get 
			{
				if (_id == null)
				{
					_id = string.Join(".", Array.ConvertAll<NameOrIndexParameter, string>(_fields,
						delegate(NameOrIndexParameter nameOrIndex)
						{
							return nameOrIndex.ToString();
						}));
				}

				return _id;
			}
		}

		[CLSCompliant(false)]
		public object GetValue(IMapDataSource source, object obj, int index)
		{
			object value = _fields[index].ByName ?
				source.GetValue(obj, _fields[index].Name) : source.GetValue(obj, _fields[index].Index);

			if (value == null)
			{
				ObjectMapper objectMapper = source as ObjectMapper;

				if (objectMapper != null)
				{
					MemberMapper mm = _fields[index].ByName ?
						objectMapper[_fields[index].Name] : objectMapper[_fields[index].Index];

					if (mm == null)
						throw new MappingException(string.Format("Type '{0}' does not contain field '{1}'.",
							objectMapper.TypeAccessor.OriginalType.Name, Fields[index]));
				}
			}

			return value;
		}

		[CLSCompliant(false)]
		public object GetValueOrIndex(IMapDataSource source, object obj)
		{
			if (Fields.Length == 1)
				return GetValue(source, obj, 0);

			return GetIndexValue(source, obj);
		}

		[CLSCompliant(false)]
		public CompoundValue GetIndexValue(IMapDataSource source, object obj)
		{
			object[] values = new string[Fields.Length];

			for (int i = 0; i < values.Length; i++)
				values[i] = GetValue(source, obj, i);

			return new CompoundValue(values);
		}
	}
}

