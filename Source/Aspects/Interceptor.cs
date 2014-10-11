namespace BLToolkit.Aspects
{
	[System.Diagnostics.DebuggerStepThrough]
	public abstract class Interceptor : IInterceptor
	{
		public virtual void Init(CallMethodInfo info, string configString)
		{
		}

		public virtual void Intercept(InterceptCallInfo info)
		{
			switch (info.InterceptType)
			{
				case InterceptType.BeforeCall: BeforeCall(info); break;
				case InterceptType.AfterCall:  AfterCall (info); break;
				case InterceptType.OnCatch:    OnCatch   (info); break;
				case InterceptType.OnFinally:  OnFinally (info); break;
			}
		}

		protected virtual void BeforeCall(InterceptCallInfo info) {}
		protected virtual void AfterCall (InterceptCallInfo info) {}
		protected virtual void OnCatch   (InterceptCallInfo info) {}
		protected virtual void OnFinally (InterceptCallInfo info) {}
	}
}
