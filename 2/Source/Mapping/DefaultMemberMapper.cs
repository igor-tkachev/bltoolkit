using System;

namespace BLToolkit.Mapping
{
	class DefaultMemberMapper : MemberMapper
	{
		public override object GetValue(object o)
		{
			return MapTo(base.GetValue(o));
		}

		public override void SetValue(object o, object value)
		{
			value = MapFrom(value);
			base.SetValue(o, value);
		}
	}
}
