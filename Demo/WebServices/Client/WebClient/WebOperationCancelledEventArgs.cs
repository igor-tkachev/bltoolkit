using System;
using System.Text;
using System.Collections;

namespace Demo.WebServices.Client.WebClient
{
	public class WebOperationCancelledEventArgs : EventArgs
	{
		private readonly string _url;
		public           string  Url
		{
			get { return _url; }
		}

		private readonly string _methodName;
		public           string  MethodName
		{
			get { return _methodName; }
		}

		private readonly object[] _parameters;
		public           object[]  Parameters
		{
			get { return _parameters; }
		}

		public WebOperationCancelledEventArgs(string url, string methodName, object[] parameters)
		{
			_url        = url;
			_methodName = methodName;
			_parameters = parameters;
		}

		public string Format()
		{
			StringBuilder sb = new StringBuilder();
			sb
				.Append(Url)
				.Append('/')
				.Append(MethodName)
				.Append('(')
				;

			if (Parameters != null)
			{
				bool first = true;

				foreach (object parameter in Parameters)
				{
					if (first)
						first = false;
					else
						sb.Append(", ");

					try
					{
						FormatParameter(parameter, sb);
					}
					catch (Exception ex)
					{
						sb.Append(ex.ToString());
					}
				}
			}

			return sb.Append(')').ToString();
		}

		private static void FormatParameter(object parameter, StringBuilder sb)
		{
			if (parameter == null)
				sb.Append("null");
			else if (parameter is string)
				sb.Append('"').Append((string)parameter).Append('"');
			else if (parameter is char)
				sb.Append('\'').Append((char)parameter).Append('\'');
			else if (parameter is IEnumerable)
			{
				sb.Append('[');
				bool first = true;
				foreach (object item in (IEnumerable)parameter)
				{
					FormatParameter(item, first? sb: sb.Append(','));
					first = false;
				}
				sb.Append(']');
			}
			else
				sb.Append(parameter.ToString());
		}
	}
}