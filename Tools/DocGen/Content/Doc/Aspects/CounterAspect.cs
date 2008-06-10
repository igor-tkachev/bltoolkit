[BLToolkitGenerated]
public sealed class TestClass : CounterAspectTest.TestClass
{
	private static CallMethodInfo _methodInfo;
	private static IInterceptor   _interceptor;

	public override void TestMethod()
	{
		if (_methodInfo == null)
		{
			_methodInfo = new CallMethodInfo((MethodInfo)MethodBase.GetCurrentMethod());
		}

		InterceptCallInfo info = new InterceptCallInfo();

		try
		{
			info.Object          = this;
			info.CallMethodInfo  = _methodInfo;
			info.InterceptResult = InterceptResult.Continue;
			info.InterceptType   = InterceptType.BeforeCall;

			if (_interceptor == null)
			{
				_interceptor = new CounterAspect();
				_interceptor.Init(_methodInfo, null);
			}

			// 'BeforeCall' creates or gets a counter for the method and 
			// registers the current call.
			// See the [link][file]Aspects/CounterAspect.cs[/file]CounterAspect.BeforeCall[/link] method for details.
			//
			_interceptor.Intercept(info);

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
			info.InterceptResult = InterceptResult.Continue;
			info.InterceptType   = InterceptType.OnCatch;

			// 'OnCatch' is required to count calls with exceptions.
			//
			_interceptor.Intercept(info);

			if (info.InterceptResult != InterceptResult.Return)
			{
				throw;
			}
		}
		finally
		{
			info.InterceptResult = InterceptResult.Continue;
			info.InterceptType   = InterceptType.OnFinally;

			// 'OnFinally' step adds statistic to the method counter.
			// See the [link][file]Aspects/CounterAspect.cs[/file]CounterAspect.OnFinally[/link] method for details.
			//
			_interceptor.Intercept(info);
		}
	}
}
