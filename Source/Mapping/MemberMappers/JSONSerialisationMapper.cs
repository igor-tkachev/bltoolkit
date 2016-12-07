using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using BLToolkit.Reflection;

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

			var ser = GetSerializer(obj);
			MemoryStream ms = new MemoryStream();
			ser.WriteObject(ms, obj);
			string jsonString = Encoding.UTF8.GetString(ms.ToArray());
			ms.Close();
			return jsonString;   
		}

		private DataContractJsonSerializer GetSerializer(object obj)
		{
			var type       = TypeAccessor.GetAccessor(MemberAccessor.Type).Type;
			var extraType  = obj != null ? TypeHelper.GetListItemType(obj) : typeof(object);
			var extraType2 = TypeHelper.GetListItemType(type);
			var extraTypes = new[] {TypeAccessor.GetAccessor(extraType).Type, TypeAccessor.GetAccessor(extraType2).Type };

			var serializer = extraType != typeof(object) || extraType2 != typeof(object)
				? new DataContractJsonSerializer(type, extraTypes)
				: new DataContractJsonSerializer(type);
			return serializer;
		}

		object Deserialize(string txt)
		{
			object retVal = null;
			if (string.IsNullOrEmpty(txt)) return null;

			try
			{
				DataContractJsonSerializer ser = GetSerializer(null);
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
