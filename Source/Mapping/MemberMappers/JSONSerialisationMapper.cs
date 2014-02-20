using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace BLToolkit.Mapping.MemberMappers
{
	public class JSONSerialisationMapper : MemberMapper
	{
		public override void SetValue(object o, object value)
		{
			if (value != null) this.MemberAccessor.SetValue(o, Deserialize(value.ToString()));
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

		object Deserialize(string txt)
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
