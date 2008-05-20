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
			else
				if (declaringType.IsAbstract)
					declaringType = BLToolkit.Reflection.TypeAccessor.GetAccessor(declaringType).Type;

			if (parameterTypes == null)
				parameterTypes = Type.EmptyTypes;

			MethodInfo methodInfo = declaringType.GetMethod(
				methodName, BindingFlags.Instance | BindingFlags.Public, null, parameterTypes, null);

			if (methodInfo == null)
				throw new ArgumentException(string.Format("Method '{0}.{1}' not found.",
					declaringType.FullName, methodName));

			return methodInfo;
		}
	}
}
