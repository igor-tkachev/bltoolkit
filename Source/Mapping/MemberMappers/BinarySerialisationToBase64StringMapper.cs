using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BLToolkit.Mapping.MemberMappers
{
    public class BinarySerialisationToBase64StringMapper : MemberMapper
    {
        public override void SetValue(object o, object value)
        {
            if (value != null) this.MemberAccessor.SetValue(o, this.binarydeserialize(Convert.FromBase64String(o.ToString())));                                
        }

        public override object GetValue(object o)
        {
            return Convert.ToBase64String(this.binaryserialize(this.MemberAccessor.GetValue(o)));            
        }        

        private byte[] binaryserialize(object obj)
        {
            if (obj == null) return null;
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, obj);
            memoryStream.Flush();
            memoryStream.Position = 0;
            return memoryStream.ToArray();
        }

        private object binarydeserialize(byte[] data)
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
