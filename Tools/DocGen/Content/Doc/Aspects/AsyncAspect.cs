[BLToolkitGenerated]
internal delegate int TestObject$Test$Delegate(int, string);

[BLToolkitGenerated]
public sealed class AsyncTestObject : HowTo.Aspects.AsyncAspectTest.AsyncTestObject
{
	public override IAsyncResult BeginTest(int intVal, string strVal)
	{
		AsyncAspectBuilder.InternalAsyncResult r = new AsyncAspectBuilder.InternalAsyncResult();

		r.Delegate    = new TestObject$Test$Delegate(base.Test);
		r.InnerResult = r.Delegate.BeginInvoke(intVal, strVal, null, null);

		return r;
	}

	public override IAsyncResult BeginTest(int intVal, string strVal, AsyncCallback callback)
	{
		AsyncAspectBuilder.InternalAsyncResult r = new AsyncAspectBuilder.InternalAsyncResult();

		r.Delegate      = new TestObject$Test$Delegate(base.Test);
		r.AsyncCallback = callback;
		r.InnerResult   = r.Delegate.BeginInvoke(intVal, strVal, new AsyncCallback(r.CallBack), null);

		return r;
	}

	public override IAsyncResult BeginTest(int intVal, string strVal, AsyncCallback callback, object state)
	{
		AsyncAspectBuilder.InternalAsyncResult r = new AsyncAspectBuilder.InternalAsyncResult();

		r.Delegate      = new TestObject$Test$Delegate(base.Test);
		r.AsyncCallback = callback;
		r.InnerResult   = r.Delegate.BeginInvoke(intVal, strVal, new AsyncCallback(r.CallBack), state);

		return r;
	}

	public override int EndTest(IAsyncResult asyncResult)
	{
		AsyncAspectBuilder.InternalAsyncResult r = (AsyncAspectBuilder.InternalAsyncResult)asyncResult;

		return ((TestObject$Test$Delegate)r.Delegate).EndInvoke(r.InnerResult);
	}
}
