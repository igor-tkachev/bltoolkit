using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace BLToolkit.ServiceModel
{
	[DataContract]
	public class LinqServiceResult
	{
		[XmlIgnore] public int            FieldCount   { get; set; }
		[XmlIgnore] public int            RowCount     { get; set; }
		[XmlIgnore] public Guid           QueryID      { get; set; }
		[XmlIgnore] public string[]       FieldNames   { get; set; }
		[XmlIgnore] public Type[]         FieldTypes   { get; set; }
		[XmlIgnore] public Type[]         VaryingTypes { get; set; }
		[XmlIgnore] public List<string[]> Data         { get; set; }

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
