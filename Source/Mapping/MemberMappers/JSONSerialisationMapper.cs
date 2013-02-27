using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BLToolkit.Mapping.MemberMappers
{
    public class JSONSerialisationMapper : MemberMapper
    {
        public override void SetValue(object o, object value)
        {
            if (value != null) this.MemberAccessor.SetValue(o, this.deserialize(value.ToString()));
        }

        public override object GetValue(object o)
        {
            return this.serialize(this.MemberAccessor.GetValue(o));
        }

        private string serialize(object obj)
        {
            if (obj == null) return null;

            DataContractJsonSerializer ser = new DataContractJsonSerializer(this.Type);
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, obj);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;   
        }

        private object deserialize(string txt)
        {
            object retVal = null;
            if (string.IsNullOrEmpty(txt)) return null;

            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(this.Type);
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(txt));
                retVal = ser.ReadObject(ms);                
            }
            catch (Exception)
            {
            }
            return retVal;
        }
    }
}
