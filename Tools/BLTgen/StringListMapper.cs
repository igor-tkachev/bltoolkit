using System;

using BLToolkit.Mapping;

namespace BLTgen
{
	public class StringListMapper : MapDataSourceBase
	{
		string[] _list;

		public StringListMapper(string[] list)
		{
			_list = list;
		}

		#region IMapDataSource

		public override int Count
		{
			get { return _list.Length; }
		}

		public override Type GetFieldType(int index)
		{
			return (index > 0 && index < _list.Length)? typeof(string): null;
		}

		public override string GetName(int index)
		{
			return GetNameOrValue(_list[index], true);
		}

		public override int GetOrdinal(string name)
		{
			throw new InvalidOperationException("IMapDataSource.GetOrdinal(string)");
		}

		public override object GetValue(object o, int index)
		{
			return GetNameOrValue(_list[index], false);
		}

		public override object GetValue(object o, string name)
		{
			throw new InvalidOperationException("IMapDataSource.GetValue(object, string)");
		}

		#endregion

		#region Implementation

		private static string GetNameOrValue(string str, bool name)
		{
			if (str.StartsWith("\"") && str.EndsWith("\""))
				str = str.Substring(1, str.Length - 2);

			// Option
			//
			if (str.StartsWith("-") || str.StartsWith("/"))
			{
				int colon = str.IndexOfAny(new char[] { ':', '=' }, 1);
				if (colon > 0)
					return name ? str.Substring(1, colon - 1) : str.Substring(colon + 1);

				return name ? str.Substring(1) : string.Empty;
			}

			// Default parameter
			//
			return name ? string.Empty : str;
		}

		#endregion
	}
}
