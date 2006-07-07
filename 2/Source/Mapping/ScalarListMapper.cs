using System;
using System.Collections;

namespace BLToolkit.Mapping
{
	public class ScalarListMapper : MapDataDestinationBase
	{
		public ScalarListMapper(IList list, Type type)
		{
			_list = list;
			_type = type;
		}

		private IList _list;
		private Type  _type;

		public override Type GetFieldType(int index)
		{
			return _type;
		}

		public override int GetOrdinal(string name)
		{
			return 0;
		}

		public override void SetValue(object o, int index, object value)
		{
			_list.Add(value);
		}

		public override void SetValue(object o, string name, object value)
		{
			_list.Add(value);
		}
	}
}
