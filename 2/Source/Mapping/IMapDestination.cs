using System;

namespace BLToolkit.Mapping
{
	public interface IMapDestination
	{
		object SetNames (string[] names);
		void   SetValues(object o, object context, object[] values);
	}
}
