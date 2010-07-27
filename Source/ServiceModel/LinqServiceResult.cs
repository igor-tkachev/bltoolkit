using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BLToolkit.ServiceModel
{
	[DataContract]
	public class LinqServiceResult
	{
		[DataMember] public int            FieldCount   { get; set; }
		[DataMember] public int            RowCount     { get; set; }
		[DataMember] public Guid           QueryID      { get; set; }
		[DataMember] public string[]       FieldNames   { get; set; }
		[DataMember] public Type[]         FieldTypes   { get; set; }
		[DataMember] public List<string[]> Data         { get; set; }
	}
}
