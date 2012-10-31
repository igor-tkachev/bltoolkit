using System;

namespace BLToolkit.Mapping
{
	public class TextDataMapper : MapDataDestinationBase
	{
		public TextDataMapper(TextDataWriter writer)
		{
			if (writer == null) throw new ArgumentNullException("writer");

			_writer = writer;
		}

		private readonly TextDataWriter _writer;
		public           TextDataWriter  Writer
		{
			get { return _writer; }
		}

		public virtual void WriteEnd()
		{
			_writer.WriteEnd();
		}

		public override Type GetFieldType(int index)
		{
			return _writer.GetFieldType(index);
		}

		public override int GetOrdinal(string name)
		{
			return _writer.GetOrdinal(name);
		}

		public override void SetValue(object o, int index, object value)
		{
			_writer.SetValue(index, value);
		}

		public override void SetValue(object o, string name, object value)
		{
			_writer.SetValue(name, value);
		}
	}
}
