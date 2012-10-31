namespace BLToolkit.Aspects
{
	public interface IInterceptor
	{
		void Init     (CallMethodInfo    info, string configString);
		void Intercept(InterceptCallInfo info);
	}
}
