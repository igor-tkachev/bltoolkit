[BLToolkitGenerated]
public sealed class TestClass : HowTo.Aspects.TestClass
{
	private static CallMethodInfo _methodInfo;
	private static IInterceptor   _interceptor;

	public override int CachedMethod(int p1, int p2)
	{
		int returnValue = 0;

		if (_methodInfo == null)
		{
			_methodInfo = new CallMethodInfo((MethodInfo)MethodBase.GetCurrentMethod());
		}

		InterceptCallInfo info = new InterceptCallInfo();

		info.Object             = this;
		info.CallMethodInfo     = _methodInfo;
		info.ParameterValues[0] = p1;
		info.ParameterValues[1] = p2;
		info.ReturnValue        = returnValue;
		info.InterceptResult    = InterceptResult.Continue;
		info.InterceptType      = InterceptType.BeforeCall;

		if (_interceptor == null)
		{
			_interceptor = new CacheAspect();
			_interceptor.Init(_methodInfo, "MaxCacheTime=500;IsWeak=False");
		}

		// 'BeforeCall' step checks if the method is cached.
		// If it is and the cache is not expired, the Intercept method populates 
		// return value and output parameters with the cached values and 
		// sets info.InterceptResult to InterceptResult.Return.
		// See the [link][file]Aspects/CacheAspect.cs[/file]CacheAspect.BeforeCall[/link] method for details.
		//
		_interceptor.Intercept(info);

		returnValue = (int)info.ReturnValue;

		if (info.InterceptResult != InterceptResult.Return)
		{
			// If the method call is not cached, target method is called.
			//
			returnValue = base.CachedMethod(p1, p2);

			info.ReturnValue     = returnValue;
			info.InterceptResult = InterceptResult.Continue;
			info.InterceptType   = InterceptType.AfterCall;

			// 'AfterCall' step stores parameters and return values in the cache.
			// See the [link][file]Aspects/CacheAspect.cs[/file]CacheAspect.AfterCall[/link] method for details.
			//
			_interceptor.Intercept(info);

			returnValue = (int)info.ReturnValue;
		}

		return returnValue;
	}
}
