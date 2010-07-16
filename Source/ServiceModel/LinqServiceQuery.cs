using System;
using System.Runtime.Serialization;

using BLToolkit.Data.Sql;

namespace BLToolkit.ServiceModel
{
	[DataContract]
	public class LinqServiceQuery
	{
		[DataMember] public SqlQuery       Query      { get; set; }
		[DataMember] public SqlParameter[] Parameters { get; set; }
	}
}
