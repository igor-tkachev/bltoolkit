using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BLToolkit.Mapping.MemberMappers
{
    public class TimeSpanBigIntMapper : MemberMapper
    {
        public override void SetValue(object o, object value)
        {
            if (value != null) this.MemberAccessor.SetValue(o, new TimeSpan((long)value));                                
        }

        public override object GetValue(object o)
        {
            var val = this.MemberAccessor.GetValue(o);
            if (val != null)
                return ((TimeSpan) val).Ticks;
            return null;                        
        }                
    }
}
