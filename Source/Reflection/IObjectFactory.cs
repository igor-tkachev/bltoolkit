using System;

namespace BLToolkit.Reflection
{
	public interface IObjectFactory
	{
		object CreateInstance(TypeAccessor typeAccessor, InitContext context);
	}
}
