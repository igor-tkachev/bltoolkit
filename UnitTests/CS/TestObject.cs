using System;

using BLToolkit.Mapping;

public abstract class TestObject
{
	public abstract int      ID   { get; set; }
	public abstract string   Name { get; set; }
	[Nullable]
	public abstract DateTime Date { get; set; }

	public static ObjectMapper ObjectMapper
	{
		get { return Map.GetObjectMapper(typeof(TestObject)); }
	}
}
