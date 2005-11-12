using System;

namespace BLToolkit.Mapping
{
	public interface IMapSource
	{
		string[] GetNames ();
		void     GetValues(object o, object[] values);
	}
}
