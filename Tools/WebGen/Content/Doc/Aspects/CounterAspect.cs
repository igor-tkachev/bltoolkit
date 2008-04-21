[BLToolkitGenerated]
public sealed class TestClass : CounterAspectTest.TestClass
{
	[BLToolkitGenerated]
	private static IInterceptor _interceptor$_BLToolkit.Aspects.CounterAspect = new CounterAspect();

	[BLToolkitGenerated]
	private static CallMethodInfo _methodInfo$TestMethod1;

	[BLToolkitGenerated]
	public override void TestMethod()
	{
		if (_methodInfo$TestMethod1 == null)
		{
			_methodInfo$TestMethod1 = new CallMethodInfo((MethodInfo)MethodBase.GetCurrentMethod());
		}

		InterceptCallInfo info = new InterceptCallInfo();

		try
		{
			info.Object          = this;
			info.CallMethodInfo  = _methodInfo$TestMethod1;
			info.InterceptorID   = 2;
			info.InterceptResult = InterceptResult.Continue;
			info.InterceptType   = InterceptType.BeforeCall;

			// 'BeforeCall' creates or gets a counter for the method and 
			// registers the current call.
			//
			_interceptor$_BLToolkit.Aspects.CounterAspect.Intercept(info);

			if (info.InterceptResult != InterceptResult.Return)
			{
				// Target method call.
				//
				base.TestMethod();
			}
		}
		catch (Exception exception)
		{
			info.Exception       = exception;
			info.InterceptorID   = 2;
			info.InterceptResult = InterceptResult.Continue;
			info.InterceptType   = InterceptType.OnCatch;

			// 'OnCatch' is required to count calls with exceptions.
			//
			_interceptor$_BLToolkit.Aspects.CounterAspect.Intercept(info);

			if (info.InterceptResult != InterceptResult.Return)
			{
				throw;
			}
		}
		finally
		{
			info.InterceptorID   = 2;
			info.InterceptResult = InterceptResult.Continue;
			info.InterceptType   = InterceptType.OnFinally;

			// 'OnFinally' step adds statistic to the method counter.
			//
			_interceptor$_BLToolkit.Aspects.CounterAspect.Intercept(info);
		}
	}
}
