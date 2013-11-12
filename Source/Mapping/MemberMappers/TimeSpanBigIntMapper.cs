#region

using System;

#endregion

namespace BLToolkit.Mapping.MemberMappers
{
    public class TimeSpanBigIntMapper : MemberMapper
    {
        public override void SetValue(object o, object value)
        {
            if (value != null) MemberAccessor.SetValue(o, new TimeSpan(Convert.ToInt64(value)));
        }

        public override object GetValue(object o)
        {
            var val = MemberAccessor.GetValue(o);
            if (val != null)
                return ((TimeSpan) val).Ticks;
            return null;
        }
    }
}