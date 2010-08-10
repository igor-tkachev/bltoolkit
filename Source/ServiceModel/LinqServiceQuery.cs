using System;
using System.Runtime.Serialization;

namespace BLToolkit.ServiceModel
{
	using Data.Sql;

	[DataContract]
	public class LinqServiceQuery
	{
		public SqlQuery       Query      { get; set; }
		public SqlParameter[] Parameters { get; set; }

		string _queryData;

		[DataMember]
		string QueryData
		{
			get { return _queryData ?? (_queryData = Serialize()); }
			set { if (Query == null) Deserialize(value); }
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
