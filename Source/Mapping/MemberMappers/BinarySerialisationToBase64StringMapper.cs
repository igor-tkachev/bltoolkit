using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BLToolkit.Mapping.MemberMappers
{
	public class BinarySerialisationToBase64StringMapper : MemberMapper
	{
		public override void SetValue(object o, object value)
		{
			if (value != null) this.MemberAccessor.SetValue(o, BinarydeSerialize(Convert.FromBase64String(value.ToString())));                                
		}

		public override object GetValue(object o)
		{
			return Convert.ToBase64String(BinarySerialize(this.MemberAccessor.GetValue(o)));            
		}

		static byte[] BinarySerialize(object obj)
		{
			if (obj == null) return null;
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, obj);
			memoryStream.Flush();
			memoryStream.Position = 0;
			return memoryStream.ToArray();
		}

		static object BinarydeSerialize(byte[] data)
		{
			using (var stream = new MemoryStream(data))
			{
				var formatter = new BinaryFormatter();
				stream.Seek(0, SeekOrigin.Begin);
				return formatter.Deserialize(stream);
			}
		}
	}
}
