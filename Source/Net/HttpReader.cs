/*
 * File:    HttpReader.cs
 * Created: 01/17/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace BLToolkit.Net
{
	public delegate void ProcessStream(Stream stream);

	/// <summary>
	/// Encapsulates WebReader functions.
	/// </summary>
	public class HttpReader
	{
		#region Costructors

		public HttpReader()
		{
			BaseUri = string.Empty;
		}

		public HttpReader(string baseUri)
		{
			BaseUri = baseUri;
		}

		#endregion

		#region Public Properties

		private X509Certificate _certificate;
		public  X509Certificate  Certificate
		{
			get { return _certificate;  }
			set { _certificate = value; }
		}

		private string _baseUri;
		public  string  BaseUri
		{
			get { return _baseUri;  }
			set { _baseUri = value; }
		}

		private string _previousUri;
		public  string  PreviousUri
		{
			get { return _previousUri;  }
			set { _previousUri = value; }
		}

		private CookieContainer _cookieContainer = new CookieContainer();
		public  CookieContainer  CookieContainer
		{
			get { return _cookieContainer;  }
			set { _cookieContainer = value; }
		}

		private string _userAgent = @"HttpReader";
		public  string  UserAgent
		{
			get { return _userAgent;  }
			set { _userAgent = value; }
		}

		private string _accept =
			@"image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, */*";
		public  string  Accept
		{
			get { return _accept;  }
			set { _accept = value; }
		}

		private Uri _requestUri;
		public  Uri  RequestUri
		{
			get { return _requestUri;  }
			set { _requestUri = value; }
		}

		private string _contentType = string.Empty;
		public  string  ContentType
		{
			get { return _contentType;  }
			set { _contentType = value; }
		}

		private IWebProxy _proxy = new WebProxy();
		public  IWebProxy  Proxy
		{
			get { return _proxy;  }
			set { _proxy = value; }
		}

		private ICredentials _credentials = CredentialCache.DefaultCredentials;
		public  ICredentials  Credentials
		{
			get { return _credentials;  }
			set { _credentials = value; }
		}

		private string _html;
		public  string  Html
		{
			get { return _html; }
		}

		private readonly Hashtable _headers = new Hashtable();
		public           Hashtable  Headers
		{
			get { return _headers; }
		}

		private string _location;
		public  string  Location
		{
			get { return _location; }
		}

		private bool _sendReferer = true;
		public  bool  SendReferer
		{
			get { return _sendReferer;  }
			set { _sendReferer = value; }
		}

		private HttpStatusCode _statusCode;
		public  HttpStatusCode  StatusCode
		{
			get { return _statusCode; }
		}

		private int _timeout;
		public  int  Timeout
		{
			get { return _timeout;  }
			set { _timeout = value; }
		}

		#endregion

		#region Public Methods

		public void LoadCertificate(string fileName)
		{
			Certificate = X509Certificate.CreateFromCertFile(fileName);
		}

		#endregion

		#region Request Methods

		private HttpWebRequest PrepareRequest(string method, string requestUri, ProcessStream requestStreamProcessor)
		{
			_html = "";

			string uri = BaseUri;
			
			if (method != "SOAP")
				uri += requestUri;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

			if (Proxy       != null) request.Proxy       = Proxy;
			if (Credentials != null) request.Credentials = Credentials;

			request.CookieContainer = CookieContainer;
			request.UserAgent       = UserAgent;
			request.Accept          = Accept;
			request.Method          = method == "SOAP"? "POST" : method;
			request.KeepAlive       = true;

			if (SendReferer)
				request.Referer = PreviousUri ?? uri;

			foreach (string key in Headers.Keys)
				request.Headers.Add(key, Headers[key].ToString());

			if (method == "POST")
			{
				request.ContentType       = "application/x-www-form-urlencoded";
				request.AllowAutoRedirect = false;
			}
			else if (method == "SOAP")
			{
				request.ContentType       = "text/xml; charset=utf-8";
				request.AllowAutoRedirect = false;

				request.Headers.Add("SOAPAction", requestUri);
			}
			else
			{
				request.ContentType       = ContentType;
				request.AllowAutoRedirect = true;
			}

			PreviousUri = uri;
			RequestUri  = request.RequestUri;

			if (Certificate != null)
				request.ClientCertificates.Add(Certificate);

			if (Timeout != 0)
				request.Timeout = Timeout;

			if (requestStreamProcessor != null)
				using (Stream st = request.GetRequestStream())
					requestStreamProcessor(st);

			return request;
		}

		public HttpStatusCode Request(
			string        requestUri,
			string        method,
			ProcessStream requestStreamProcessor,
			ProcessStream responseStreamProcessor)
		{
			HttpWebRequest request = PrepareRequest(method, requestUri, requestStreamProcessor);

			using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
			using (Stream          sm   = resp.GetResponseStream())
			{
				_statusCode = resp.StatusCode;
				_location   = resp.Headers["Location"];

				if (resp.ResponseUri.AbsoluteUri.StartsWith(BaseUri) == false)
					BaseUri = resp.ResponseUri.Scheme + "://" + resp.ResponseUri.Host;

				CookieCollection cc = request.CookieContainer.GetCookies(request.RequestUri);

				// This code fixes the situation when a server sets a cookie without the 'path'.
				// IE takes this as the root ('/') value,
				// the HttpWebRequest class as the RequestUri.AbsolutePath value.
				//
				foreach (Cookie c in cc)
					if (c.Path == request.RequestUri.AbsolutePath)
						CookieContainer.Add(new Cookie(c.Name, c.Value, "/", c.Domain));

				if (responseStreamProcessor != null)
					responseStreamProcessor(sm);
			}

			return StatusCode;
		}

		public IEnumerable<string> Request(
			string        requestUri,
			string        method,
			ProcessStream requestStreamProcessor)
		{
			HttpWebRequest request = PrepareRequest(method, requestUri, requestStreamProcessor);

			using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
			using (Stream          sm   = resp.GetResponseStream())
			using (StreamReader    sr   = new StreamReader(sm, Encoding.Default))
			{
				_statusCode = resp.StatusCode;
				_location   = resp.Headers["Location"];

				if (resp.ResponseUri.AbsoluteUri.StartsWith(BaseUri) == false)
					BaseUri = resp.ResponseUri.Scheme + "://" + resp.ResponseUri.Host;

				CookieCollection cc = request.CookieContainer.GetCookies(request.RequestUri);

				// This code fixes the case when a server sets a cookie without the 'path'.
				// IE takes this as the root ('/') value,
				// the HttpWebRequest class as the RequestUri.AbsolutePath value.
				//
				foreach (Cookie c in cc)
					if (c.Path == request.RequestUri.AbsolutePath)
						CookieContainer.Add(new Cookie(c.Name, c.Value, "/", c.Domain));

				while (true)
				{
					string str = sr.ReadLine();

					if (str == null)
						break;

					yield return str;
				}
			}
		}

		class DefaultRequestStreamProcessor
		{
			public DefaultRequestStreamProcessor(string data)
			{
				_data = data;
			}

			readonly string _data;

			public void Process(Stream stream)
			{
				byte[] bytes = Encoding.ASCII.GetBytes(_data);
				stream.Write(bytes, 0, bytes.Length);
			}
		}

		class DefaultResponseStreamProcessor
		{
			public DefaultResponseStreamProcessor(HttpReader reader)
			{
				_reader = reader;
			}

			readonly HttpReader _reader;

			public void Process(Stream stream)
			{
				using (StreamReader sr = new StreamReader(stream, Encoding.Default))
					_reader._html = sr.ReadToEnd();
			}
		}

		public HttpStatusCode Get(string requestUri)
		{
			DefaultResponseStreamProcessor rp = new DefaultResponseStreamProcessor(this);

			return Request(requestUri, "GET", null, rp.Process);
		}

		public HttpStatusCode Get(string requestUri, ProcessStream responseStreamProcessor)
		{
			return Request(requestUri, "GET", null, responseStreamProcessor);
		}

		public HttpStatusCode Post(
			string requestUri,
			string postData)
		{
			return Post(
				requestUri,
				new DefaultRequestStreamProcessor(postData).Process,
				new DefaultResponseStreamProcessor(this).Process);
		}

		public HttpStatusCode Post(
			string        requestUri,
			ProcessStream requestStreamProcessor)
		{
			return Post(
				requestUri,
				requestStreamProcessor,
				new DefaultResponseStreamProcessor(this).Process);
		}

		public HttpStatusCode Post(
			string        requestUri,
			string        postData,
			ProcessStream responseStreamProcessor)
		{
			return Post(
				requestUri,
				new ProcessStream(new DefaultRequestStreamProcessor(postData).Process),
				responseStreamProcessor);
		}

		public HttpStatusCode Post(
			string        requestUri,
			ProcessStream requestStreamProcessor,
			ProcessStream responseStreamProcessor)
		{
			Request(requestUri, "POST", requestStreamProcessor, responseStreamProcessor);

			for (int i = 0; i < 10; i++)
			{
				bool post = false;

				switch (StatusCode)
				{
					case HttpStatusCode.MultipleChoices:   // 300
					case HttpStatusCode.MovedPermanently:  // 301
					case HttpStatusCode.Found:             // 302
					case HttpStatusCode.SeeOther:          // 303
						break;

					case HttpStatusCode.TemporaryRedirect: // 307
						post = true;
						break;

					default:
						return StatusCode;
				}

				if (Location == null)
					break;

				Uri uri = new Uri(new Uri(PreviousUri), Location);

				BaseUri    = uri.Scheme + "://" + uri.Host;
				requestUri = uri.AbsolutePath + uri.Query;

				Request(
					requestUri,
					post? "POST": "GET",
					post? requestStreamProcessor: null,
					responseStreamProcessor);
			}

			return StatusCode;
		}

		private HttpStatusCode Soap(
			string        soapAction,
			ProcessStream inputStreamProcessor,
			ProcessStream outputStreamProcessor)
		{
			return Request("\"" + soapAction + "\"", "SOAP", inputStreamProcessor, outputStreamProcessor);
		}

		public HttpStatusCode Soap(string soapAction, string postData)
		{
			return Soap(soapAction,
				new DefaultRequestStreamProcessor(postData).Process,
				new DefaultResponseStreamProcessor(this).Process);
		}

		public HttpStatusCode Soap(string soapAction, string postData, ProcessStream outputStreamProcessor)
		{
			return Soap(
				soapAction,
				new DefaultRequestStreamProcessor(postData).Process,
				outputStreamProcessor);
		}

		public IEnumerable<string> SoapEx(string soapAction, string postData)
		{
			return Request("\"" + soapAction + "\"", "SOAP", new DefaultRequestStreamProcessor(postData).Process);
		}

		#endregion

		#region Download

		public void Download(string requestUri, string fileName)
		{
			string uri = BaseUri + requestUri;

			WebClient request = new WebClient();

			if (Proxy       != null) request.Proxy       = Proxy;
			if (Credentials != null) request.Credentials = Credentials;

			foreach (string key in Headers.Keys)
				request.Headers.Add(key, Headers[key].ToString());

			request.DownloadFile(uri, fileName);
		}

		#endregion
	}
}
