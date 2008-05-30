using System;
using System.Diagnostics;
using System.IO;

namespace BLToolkit.Aspects
{
	public delegate void LogOperation(InterceptCallInfo interceptCallInfo, LoggingAspect.Parameters parameters);
	public delegate void LogOutput   (string logText, string fileName);

	/// <summary>
	/// http://www.bltoolkit.net/Doc/Aspects/index.htm
	/// </summary>
	public class LoggingAspect : Interceptor
	{
		public class Parameters
		{
			public string FileName;
			public int    MinCallTime;
			public bool   LogExceptions;
			public bool   LogParameters;
		}

		private string     _instanceFileName;
		private int?       _instanceMinCallTime;
		private bool?      _instanceLogExceptions;
		private bool?      _instanceLogParameters;
		private Parameters _parameters = new Parameters();

		public override void Init(CallMethodInfo info, string configString)
		{
			base.Init(info, configString);

			string[] ps = configString.Split(';');

			foreach (string p in ps)
			{
				string[] vs = p.Split('=');

				if (vs.Length == 2)
				{
					switch (vs[0].ToLower().Trim())
					{
						case "filename":      _instanceFileName      =            vs[1].Trim();  break;
						case "mincalltime":   _instanceMinCallTime   = int. Parse(vs[1].Trim()); break;
						case "logexceptions": _instanceLogExceptions = bool.Parse(vs[1].Trim()); break;
						case "logparameters": _instanceLogParameters = bool.Parse(vs[1].Trim()); break;
					}
				}
			}
		}

		protected override void OnFinally(InterceptCallInfo info)
		{
			if (IsEnabled)
			{
				_parameters.FileName      = _instanceFileName      ?? FileName;
				_parameters.MinCallTime   = _instanceMinCallTime   ?? MinCallTime;
				_parameters.LogExceptions = _instanceLogExceptions ?? LogExceptions;
				_parameters.LogParameters = _instanceLogParameters ?? LogParameters;

				LogOperation(info, _parameters);
			}
		}

		#region Parameters

		private static bool _logParameters = true;
		public  static bool  LogParameters
		{
			get { return _logParameters;  }
			set { _logParameters = value; }
		}

		private static bool _logExceptions = true;
		public  static bool  LogExceptions
		{
			get { return _logExceptions;  }
			set { _logExceptions = value; }
		}

		private static int _minCallTime;
		public  static int  MinCallTime
		{
			get { return _minCallTime;  }
			set { _minCallTime = value; }
		}

		private static string _fileName;
		public  static string  FileName
		{
			get { return _fileName;  }
			set { _fileName = value; }
		}

		private bool _isEnabled = true;
		public  bool  IsEnabled
		{
			get { return _isEnabled;  }
			set { _isEnabled = value; }
		}

		#endregion

		#region LogOparation

		private static LogOperation _logOperation = LogOperationInternal;
		public  static LogOperation  LogOperation
		{
			get { return _logOperation; }
			set { _logOperation = value ?? LogOperationInternal; }
		}

		private static void LogOperationInternal(InterceptCallInfo info, Parameters parameters)
		{
			DateTime end  = DateTime.Now;
			int      time = (int)((end - info.BeginCallTime).TotalMilliseconds);

			if (info.Exception != null && parameters.LogExceptions ||
				info.Exception == null && time >= parameters.MinCallTime)
			{
				string callParameters = null;
				int    plen           = info.ParameterValues.Length;

				if (parameters.LogParameters && plen > 0)
				{
					string[] pvs    = new string[plen];
					object[] values = info.ParameterValues;

					for (int i = 0; i < plen; i++)
					{
						object o = values[i];
						pvs[i] =
							o == null?   "<null>":
							o is string? "\"" + o + "\"":
							o is char?   "'"  + o + "'":
							o.ToString();
					}

					callParameters = string.Join(", ", pvs);
				}

				string exText = null;

				if (info.Exception != null)
					exText = string.Format(
						" with exception '{0}' - \"{1}\"",
						info.Exception.GetType().FullName,
						info.Exception.Message);

				LogOutput(
					string.Format("{0}: {1}.{2}({3}) - {4} ms{5}.",
						end,
						info.CallMethodInfo.MethodInfo.DeclaringType.FullName,
						info.CallMethodInfo.MethodInfo.Name,
						callParameters,
						time,
						exText),
					parameters.FileName);
			}
		}

		#endregion

		#region LogOuput

		private static LogOutput _logOutput = LogOutputInternal;
		public  static LogOutput  LogOutput
		{
			get { return _logOutput; }
			set { _logOutput = value ?? LogOutputInternal; }
		}

		private static void LogOutputInternal(string logText, string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
				Debug.WriteLine(logText);
			else
				using (StreamWriter sw = new StreamWriter(fileName, true)) 
					sw.WriteLine(logText);
		}

		#endregion
	}
}
