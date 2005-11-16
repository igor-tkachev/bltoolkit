using System;

namespace BLToolkit.Mapping
{
	public interface IMapDataSource
	{
		int    Count { get; }

		string GetName (int index);
		object GetValue(object o, int index);
		object GetValue(object o, string name);
	}
}
