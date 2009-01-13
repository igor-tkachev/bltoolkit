using System;

namespace Demo.WebServices.Client.WebClient
{
	public class WebOperationExceptionEventArgs : WebOperationCancelledEventArgs
	{
		private readonly Exception _exception;
		public           Exception Exception
		{
			get { return _exception; }
		}

		private bool _retry;
		public  bool  Retry
		{
			get { return _retry;  }
			set { _retry = value; }
		}

		public WebOperationExceptionEventArgs(string url, string methodName, object[] parameters, Exception exception)
			: base(url, methodName, parameters)
		{
			_exception = exception;
		}
	}
}