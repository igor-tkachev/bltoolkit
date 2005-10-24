using System;

namespace BLToolkit.Reflection
{
	public interface IObjectFactory
	{
		object CreateInstance(InitContext context);
	}
}
