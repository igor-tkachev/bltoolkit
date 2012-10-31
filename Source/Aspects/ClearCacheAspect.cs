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

		public static Type GetType(object caller, Type declaringType)
		{
			if (declaringType == null)
				declaringType = caller.GetType();

			if (declaringType.IsAbstract)
				declaringType = TypeBuilder.TypeFactory.GetType(declaringType);

			return declaringType;
		}
	}
}
