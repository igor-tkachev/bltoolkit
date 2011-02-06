using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Web.Services;
using System.Web.Services.Protocols;

using BLToolkit.Common;

namespace Demo.WebServices.Client.WebClient
{
	[DebuggerStepThrough]
	[System.ComponentModel.DesignerCategory("Code")]
	[WebClient]
	public abstract class WebClientBase: SoapHttpClientProtocol
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WebClientBase"/> class.
		/// </summary>
		protected WebClientBase()
		{
			if (DefaultCredentials == null)
				UseDefaultCredentials = true;
			else
				Credentials = DefaultCredentials;

			// Use custom redirection since we need to repost some data.
			//
			AllowAutoRedirect     = false;

			EnableDecompression   = true;
			PreAuthenticate       = true;

			// Setup appropriate user agent string.
			//
			var asm = Assembly.GetEntryAssembly();
			if (asm != null)
				UserAgent = asm.FullName;

#if DEBUG
			// By default the timeout value is about 2 minutes,
			// wich is quite enought in a normal run,
			// but not for debugging.
			//
			Timeout = Int32.MaxValue;
#endif

			if (string.IsNullOrEmpty(BaseUrl))
				return;

			var attr = (WebServiceBindingAttribute)Attribute.GetCustomAttribute(GetType(), typeof(WebServiceBindingAttribute));

			if (attr == null)
				throw new InvalidOperationException("Please specify relative url or mark the avatar with WebServiceBindingAttribute");

			var ns = attr.Namespace;

			if (string.IsNullOrEmpty(ns))
				throw new InvalidOperationException("Please specify namespace in WebServiceBindingAttribute");

			if (ns.EndsWith("/"))
				ns = ns.Substring(0, ns.Length - 1);

			var builder = new UriBuilder(BaseUrl) { Path = new UriBuilder(ns).Path };

			Url = builder.Uri.AbsoluteUri;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WebClientBase"/> class.
		/// </summary>
		/// <param name="relativeUrl">Path to web service, relative to <see cref="BaseUrl"/>.</param>
		protected WebClientBase(string relativeUrl) : this()
		{
			Debug.Assert(Url == BaseUrl + relativeUrl, string.Format("Expected url '{0}' got '{1}'", Url, BaseUrl + relativeUrl) );
			Url = BaseUrl + relativeUrl;
		}

		public static ICredentials DefaultCredentials { get; set; }

		/// <summary>
		/// Customizable url path.
		/// </summary>
		public static string BaseUrl { get; set; }

		/// <summary>
		/// Returns <see langword="true"/>, program runs in offline mode.
		/// </summary>
		public static bool OffLine
		{
			get { return string.IsNullOrEmpty(BaseUrl); }
		}

		#region Invoke

		private object[] InvokeInternal(string methodName, object[] parameters)
		{
			object[] ret = null;

			for (;;)
			{
				try
				{
#if DEBUG
					var sw = Stopwatch.StartNew();
#endif
					ret = base.Invoke(methodName, parameters);
#if DEBUG
					Debug.WriteLineIf(TS.TraceVerbose,
						string.Format("Sync call {0}/{1} = {2} msec.",
							Url, methodName, sw.ElapsedMilliseconds), TS.DisplayName);
#endif
				}
				catch (Exception ex)
				{
					if (ex is WebException)
					{
						var webException = (WebException) ex;

						if (webException.Status == WebExceptionStatus.RequestCanceled)
						{
							OnWebOperationCancelled(methodName, parameters);
							break;
						}

						// Internal redirection
						//
						if (webException.Status == WebExceptionStatus.ReceiveFailure)
							continue;
					}

					if (OnWebOperationException(methodName, parameters, ex))
						continue;
				}

				break;
			}

			return AcceptChanges(ret);
		}

		/// <summary>
		/// Invokes a web method synchronously.
		/// </summary>
		/// <param name="methodName">Web method name.</param>
		/// <param name="parameters">Web method parameters.</param>
		/// <returns>Web method return value or values on success,
		/// a null reference otherwise.</returns>
		public new object[] Invoke(string methodName, params object[] parameters)
		{
			return InvokeInternal(methodName, parameters) ?? new object[]{ null };
		}

		/// <summary>
		/// Invokes a web method synchronously.
		/// </summary>
		/// <param name="methodName">Web method name.</param>
		/// <param name="parameters">Web method parameters.</param>
		/// <returns>Web method return value or default(T) if the call fails.</returns>
		public T Invoke<T>(string methodName, params object[] parameters)
		{
			var ret = InvokeInternal(methodName, parameters);

			return ret != null && ret.Length != 0? (T)ret[0]: default(T);
		}

		/// <summary>
		/// Invokes a web method asynchronously.
		/// </summary>
		/// <param name="methodName">Web method name.</param>
		/// <param name="asyncCallState">Call state handle.
		/// Upon return, may be used to cancel the call</param>
		/// <param name="parameters">Web method parameters.</param>
		/// <param name="callback">Callback method to process the result.</param>
		/// <param name="exceptionHandler">Fail handler.</param>
		/// <seealso cref="CancelAsync(AsyncCallState)"/>
		public void InvokeAsync(
			string            methodName,
			AsyncCallState    asyncCallState,
			Action<Exception> exceptionHandler,
			Delegate          callback,
			params object[]   parameters)
		{
#if DEBUG
			var sw = Stopwatch.StartNew();
#endif

			if (asyncCallState != null)
			{
#if DEBUG
			Debug.WriteLineIf(TS.TraceVerbose && asyncCallState.PendingCall != null,
				string.Format("Cancelling async call {0}/{1}",
					Url, methodName), TS.DisplayName);
#endif
				CancelAsync(asyncCallState);
			}

			var exceptionCallback = exceptionHandler ?? delegate(Exception ex)
			{
				if (ex is WebException)
				{
					var webException = (WebException)ex;

					// Request cancelled.
					//
					if (webException.Status == WebExceptionStatus.RequestCanceled)
					{
						OnWebOperationCancelled(methodName, parameters);
						return;
					}
				}

				// Check for retry.
				//
				if (OnWebOperationException(methodName, parameters, ex))
				{
					InvokeAsync(methodName, asyncCallState, exceptionHandler, callback, parameters);
				}
			};

			System.Threading.SendOrPostCallback sendCallback = delegate(object obj)
			{
#if DEBUG
				Debug.WriteLineIf(TS.TraceVerbose,
					string.Format("Async call {0}/{1} = {2} msec.",
						Url, methodName, sw.ElapsedMilliseconds), TS.DisplayName);
#endif

				var ea = (InvokeCompletedEventArgs)obj;

				if (ea.Error != null)
				{
					// Internal redirection
					//
					if (ea.Error is WebException && ((WebException)ea.Error).Status == WebExceptionStatus.ReceiveFailure)
					{
						InvokeAsync(methodName, asyncCallState, exceptionHandler, callback, parameters);
					}
					else
					{
						exceptionCallback(ea.Error);
					}
				}
				else if (ea.Cancelled || (asyncCallState != null && ea.UserState != asyncCallState.PendingCall))
				{
					exceptionCallback(new WebException(methodName, WebExceptionStatus.RequestCanceled));
				}
				else
				{
					callback.DynamicInvoke(AcceptChanges(ea.Results));
				}

				if (asyncCallState != null && ea.UserState == asyncCallState.PendingCall)
					asyncCallState.PendingCall = null;
			};

			object cookie = new CompoundValue(methodName, parameters);

			if (asyncCallState!= null)
				asyncCallState.PendingCall = cookie;

			InvokeAsync(methodName, parameters, sendCallback, cookie);
		}

		/// <summary>
		/// Invokes a web method asynchronously.
		/// </summary>
		/// <param name="methodName">Web method name.</param>
		/// <param name="asyncCallState">Call state handle.
		/// Upon return, may be used to cancel the call</param>
		/// <param name="parameters">Web method parameters.</param>
		/// <param name="callback">Callback method to process the result.</param>
		/// <seealso cref="CancelAsync(AsyncCallState)"/>
		public void InvokeAsync(
			string          methodName,
			AsyncCallState  asyncCallState,
			Delegate        callback,
			params object[] parameters)
		{
			InvokeAsync(methodName, asyncCallState, null, callback, parameters);
		}

		private static void AcceptChanges(object obj)
		{
			if (obj == null || obj is IConvertible)
			{
				//
				// Do nothing on bool, int, string, etc.
				//
			}
			else if (obj is BLToolkit.EditableObjects.IEditable)
				((BLToolkit.EditableObjects.IEditable)obj).AcceptChanges();
			else if (obj is System.Collections.IDictionary)
			{
				foreach (System.Collections.DictionaryEntry pair in (System.Collections.IDictionary)obj)
				{
					AcceptChanges(pair.Key);
					AcceptChanges(pair.Value);
				}
			}
			else if (obj is System.Collections.IEnumerable)
			{
				foreach (var elm in (System.Collections.IEnumerable)obj)
					AcceptChanges(elm);
			}
		}

		private static object[] AcceptChanges(object[] array)
		{
			if (array != null)
				Array.ForEach(array, AcceptChanges);

			return array;
		}

		/// <summary>
		///.Cancel an asynchronous call if it is not completed already.
		/// </summary>
		/// <param name="asyncCallState">Async call state.</param>
		public void CancelAsync(AsyncCallState asyncCallState)
		{
			if (asyncCallState.PendingCall == null)
				return;

			CancelAsync(asyncCallState.PendingCall);
			asyncCallState.PendingCall = null;
		}

		#endregion

		#region Events

		private static readonly object EventWebOperationCancelled = new object();
		public event EventHandler<WebOperationCancelledEventArgs> WebOperationCancelled
		{
			add    { Events.AddHandler   (EventWebOperationCancelled, value); }
			remove { Events.RemoveHandler(EventWebOperationCancelled, value); }
		}

		public static event EventHandler<WebOperationCancelledEventArgs> WebOperationCancelledDefaultHandler;

		protected virtual void OnWebOperationCancelled(string methodName, params object[] parameters)
		{
			Debug.WriteLineIf(TS.TraceInfo, string.Format("OnWebOperationCancelled; op={0}/{1}", Url, methodName));
			var handler = (EventHandler<WebOperationCancelledEventArgs>)Events[EventWebOperationCancelled] ?? WebOperationCancelledDefaultHandler;
			if (handler != null)
			{
				var ea = new WebOperationCancelledEventArgs(Url, methodName, parameters);
				handler(this, ea);
			}
		}

		private static readonly object EventWebOperationException = new object();
		public event EventHandler<WebOperationExceptionEventArgs> WebOperationException
		{
			add    { Events.AddHandler   (EventWebOperationException, value); }
			remove { Events.RemoveHandler(EventWebOperationException, value); }
		}
		public static event EventHandler<WebOperationExceptionEventArgs> WebOperationExceptionDefaultHandler;

		protected virtual bool OnWebOperationException(string methodName, object[] parameters, Exception ex)
		{
			Debug.WriteLineIf(TS.TraceError, string.Format("OnWebOperationException; op={0}/{1}; ex={2}", Url, methodName, ex));
			var handler = (EventHandler<WebOperationExceptionEventArgs>)Events[EventWebOperationException] ?? WebOperationExceptionDefaultHandler;

			if (handler != null)
			{
				var ea = new WebOperationExceptionEventArgs(Url, methodName, parameters, ex);
				handler(this, ea);
				return ea.Retry;
			}

			throw new TargetInvocationException(methodName, ex);
		}

		#endregion

		#region Cookies

		private string _cookie;

		/// <summary>
		/// Creates a <see cref="T:System.Net.WebRequest"/> for the specified uri.
		/// </summary>
		/// <returns> The <see cref="T:System.Net.WebRequest"/>. </returns>
		/// <param name="uri">The <see cref="T:System.Uri"></see> to use when creating the <see cref="T:System.Net.WebRequest"></see>. </param>
		/// <exception cref="T:System.InvalidOperationException">The uri parameter is null. </exception>
		protected override WebRequest GetWebRequest(Uri uri)
		{
			var webRequest = base.GetWebRequest(uri);
			PrepareRequest(webRequest);
			return webRequest;
		}

		/// <summary>
		/// Returns a response from a synchronous request to an XML Web service method.
		/// </summary>
		/// <returns> The <see cref="T:System.Net.WebResponse"/>. </returns>
		/// <param name="request">The <see cref="T:System.Net.WebRequest"/>
		/// from which to get the response. </param>
		protected override WebResponse GetWebResponse(WebRequest request)
		{
			var response = (HttpWebResponse)base.GetWebResponse(request);
			ProcessResponse(response);
			return response;
		}

		/// <summary>
		/// Returns a response from an asynchronous request to an XML Web service method.
		/// </summary>
		/// <returns> The <see cref="T:System.Net.WebResponse"/>.</returns>
		/// <param name="result">The <see cref="T:System.IAsyncResult"/> to pass to
		/// <see cref="M:System.Net.HttpWebRequest.EndGetResponse(System.IAsyncResult)"/> when the response has completed. </param>
		/// <param name="request">The <see cref="T:System.Net.WebRequest"/> from which to get the response. </param>
		protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			var response = (HttpWebResponse)base.GetWebResponse(request, result);
			ProcessResponse(response);
			return response;
		}

		private void ProcessResponse(HttpWebResponse response)
		{
			if (response.StatusCode == HttpStatusCode.MovedPermanently)
			{
				var redirectedLocation = response.Headers["Location"];
				Url = new Uri(new Uri(Url), redirectedLocation).AbsoluteUri;
				throw new WebException(redirectedLocation, WebExceptionStatus.ReceiveFailure);
			}

			var cookies = response.Headers.GetValues("Set-Cookie");

			if (cookies == null)
				return;

			foreach (var cookie in cookies)
			{
				if (cookie.StartsWith("ASP.NET_SessionId=", StringComparison.Ordinal))
				{
					_cookie = cookie;
					break;
				}
			}
		}

		private void PrepareRequest(WebRequest request)
		{
			if (!string.IsNullOrEmpty(_cookie))
				request.Headers.Add("Cookie", _cookie);
		}

		#endregion

		#region Debug

		private  static TraceSwitch _ts;
		internal static TraceSwitch  TS
		{
			get { return _ts ?? (_ts = new TraceSwitch("WebServiceClient", "Web service client trace switch")); }
		}

		#endregion

	}
}
