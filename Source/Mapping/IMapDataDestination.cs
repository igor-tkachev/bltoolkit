using System;

namespace BLToolkit.Mapping
{
	public interface IMapDataDestination
	{
		int  GetOrdinal(string name);
		void SetValue  (object o, int index, object value);
	}
}
