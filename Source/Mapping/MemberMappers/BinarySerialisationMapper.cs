using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BLToolkit.Mapping.MemberMappers
{
	public class BinarySerialisationMapper : MemberMapper
	{
		public override void SetValue(object o, object value)
		{
			if (value != null) this.MemberAccessor.SetValue(o, BinarydesSrialize((byte[])value));
		}

		public override object GetValue(object o)
		{
			return BinarySerialize(this.MemberAccessor.GetValue(o));
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

		static object BinarydesSrialize(byte[] data)
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
