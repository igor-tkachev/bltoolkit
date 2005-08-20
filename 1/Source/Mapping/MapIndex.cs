/*
 * File:    MapIndex.cs
 * Created: 08/16/2005
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public class MapIndex
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fields"></param>
		public MapIndex(params string[] fields)
		{
			_fields = fields;
		}

		private string[] _fields;
		/// <summary>
		/// 
		/// </summary>
		public  string[]  Fields
		{
			get { return _fields; }
		}

		private string _id;
		/// <summary>
		/// 
		/// </summary>
		public  string  ID
		{
			get 
			{
				if (_id == null)
					_id = string.Join(".", _fields);

				return _id;
			}
		}

		private object GetValue(MapDescriptor descriptor, object obj, int idx)
		{
			IMemberMapper mm = descriptor[Fields[idx]];

			if (mm == null)
				throw new RsdnMapException(string.Format("Type '{0}' does not contain field '{1}'.",
					descriptor.OriginalType.Name, Fields[idx]));

			return mm.GetValue(obj);
		}

		internal object GetKey(MapDescriptor descriptor, object obj)
		{
			if (Fields.Length == 1)
				return GetValue(descriptor, obj, 0);

			string[] keyFields = new string[Fields.Length];

			for (int i = 0; i < keyFields.Length; i++)
				keyFields[i] = GetValue(descriptor, obj, i).ToString();

			return string.Join(".", keyFields);
		}
	}
}
