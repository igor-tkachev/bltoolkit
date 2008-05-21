using System;
using System.Reflection;

namespace BLToolkit.Aspects
{
	public class ClearCacheAspect
	{
		public static MethodInfo GetMethodInfo(
			object caller, Type declaringType, string methodName, Type[] parameterTypes)
		{
			if (declaringType == null)
				declaringType = caller.GetType();

			return CacheAspect.GetMethodInfo(declaringType, methodName, parameterTypes);
		}
	}
}
