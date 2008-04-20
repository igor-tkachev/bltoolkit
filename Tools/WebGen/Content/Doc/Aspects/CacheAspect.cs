[BLToolkitGenerated]
public sealed class TestClass : HowTo.Aspects.TestClass
{
	[BLToolkitGenerated]
	private static IInterceptor _interceptor$_BLToolkit.Aspects.CacheAspect = new CacheAspect();

	[BLToolkitGenerated]
	private static CallMethodInfo _methodInfo$CachedMethod1;

	[BLToolkitGenerated]
	public override int CachedMethod(int p1, int p2)
	{
		int returnValue = 0;

		if (_methodInfo$CachedMethod1 == null)
		{
			_methodInfo$CachedMethod1 = new CallMethodInfo((MethodInfo)MethodBase.GetCurrentMethod());
		}

		InterceptCallInfo info = new InterceptCallInfo();

		info.Object             = this;
		info.CallMethodInfo     = _methodInfo$CachedMethod1;
		info.ParameterValues[0] = p1;
		info.ParameterValues[1] = p2;
		info.ReturnValue        = returnValue;
		info.ConfigString       = "MaxCacheTime=500;IsWeak=False";
		info.InterceptorID      = 2; // Unique interceptor ID
		info.InterceptResult    = InterceptResult.Continue;
		info.InterceptType      = InterceptType.BeforeCall;

		// 'BeforeCall' call checks if the method is cached.
		// If it is and the cache is not expired, the intercept method populates 
		// return value and output parameters with the cached values and 
		// sets info.InterceptResult to InterceptResult.Return.
		//
		_interceptor$_BLToolkit.Aspects.CacheAspect.Intercept(info);

		returnValue = (int)info.ReturnValue;

		if (info.InterceptResult != InterceptResult.Return)
		{
			// If the method call is not cached, target method is called.
			//
			returnValue = base.CachedMethod(p1, p2);

			info.ReturnValue     = returnValue;
			info.ConfigString    = "MaxCacheTime=500;IsWeak=False";
			info.InterceptorID   = 2;
			info.InterceptResult = InterceptResult.Continue;
			info.InterceptType   = InterceptType.AfterCall;

			// 'AfterCall' call stores parameters and return values in the cache
			// for the feature use.
			//
			_interceptor$_BLToolkit.Aspects.CacheAspect.Intercept(info);

			returnValue = (int)info.ReturnValue;
		}

		return returnValue;
	}
}
