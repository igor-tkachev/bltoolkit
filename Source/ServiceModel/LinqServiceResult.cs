using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BLToolkit.ServiceModel
{
	[DataContract]
	public class LinqServiceResult
	{
		public int            FieldCount   { get; set; }
		public int            RowCount     { get; set; }
		public Guid           QueryID      { get; set; }
		public string[]       FieldNames   { get; set; }
		public Type[]         FieldTypes   { get; set; }
		public Type[]         VaryingTypes { get; set; }
		public List<string[]> Data         { get; set; }

		string _resultData;

		[DataMember]
#if SILVERLIGHT
		public
#endif
		string ResultData
		{
			get { return _resultData ?? (_resultData = Serialize()); }
			set { if (Data == null) Deserialize(value); }
		}

		protected virtual string Serialize()
		{
			return LinqServiceSerializer.Serialize(this);
		}

		protected virtual void Deserialize(string data)
		{
			LinqServiceSerializer.Deserialize(this, data);
		}
	}
}
