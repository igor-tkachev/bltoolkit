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

		private object GetValue(ObjectMapper objectMapper, object obj, int idx)
		{
			MemberAccessor mm = objectMapper.TypeAccessor[Fields[idx]];

			if (mm == null)
				throw new MappingException(string.Format("Type '{0}' does not contain field '{1}'.",
					objectMapper.TypeAccessor.OriginalType.Name, Fields[idx]));

			return mm.GetValue(obj);
		}

		internal object GetKey(ObjectMapper objectMapper, object obj)
		{
			if (Fields.Length == 1)
				return GetValue(objectMapper, obj, 0);

			string[] keyFields = new string[Fields.Length];

			for (int i = 0; i < keyFields.Length; i++)
				keyFields[i] = GetValue(objectMapper, obj, i).ToString();

			return string.Join(".", keyFields);
		}
	}
}

