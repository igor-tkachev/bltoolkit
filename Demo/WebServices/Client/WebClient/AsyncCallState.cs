namespace Demo.WebServices.Client.WebClient
{
	/// <summary>
	/// Async call handle holder class.
	/// </summary>
	public class AsyncCallState
	{
		/// <summary>
		/// Async call handle if call is in progress, null otherwise.
		/// </summary>
		public object PendingCall;
	}
}