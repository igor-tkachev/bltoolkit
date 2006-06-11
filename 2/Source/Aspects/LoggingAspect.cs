using System;
using System.Diagnostics;
using System.IO;

namespace BLToolkit.Aspects
{
	public delegate void LogOperation(InterceptCallInfo interceptCallInfo);
	public delegate void LogOutput   (string logText, string fileName);

	public class LoggingAspect : Interceptor
	{
		protected override void OnFinally(InterceptCallInfo info)
		{
			LogOperation(info);
		}

		private static LogOperation _logOperation = new LogOperation(LogOperationInternal);
		public  static LogOperation  LogOperation
		{
			get
			{
				if (_logOperation == null)
					_logOperation = new LogOperation(LogOperationInternal);

				return _logOperation;
			}

			set { _logOperation = value; }
		}

		private static void LogOperationInternal(InterceptCallInfo info)
		{
			string   fileName      = FileName;
			int      minCallTime   = MinCallTime;
			bool     logExceptions = LogExceptions;
			bool     logParameters = LogParameters;
			DateTime end           = DateTime.Now;

			if (info.Parameters != null && info.Parameters.Length > 0)
			{
				string[] ps = info.Parameters.Split(';');

				foreach (string p in ps)
				{
					string[] vs = p.Split('=');

					if (vs.Length == 2)
					{
						switch (vs[0].ToLower().Trim())
						{
							case "filename":      fileName      = vs[1].Trim();             break;
							case "mincalltime":   minCallTime   = int. Parse(vs[1].Trim()); break;
							case "logexceptions": logExceptions = bool.Parse(vs[1].Trim()); break;
							case "logparameters": logParameters = bool.Parse(vs[1].Trim()); break;
						}
					}
				}
			}

			string parameters = null;
			int    plen       = info.ParameterValues.Length;

			if (logParameters && plen > 0)
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

				parameters = string.Join(", ", pvs);
			}

			int time = (int)((end - info.BeginCallTime).TotalMilliseconds);

			if (info.Exception != null && logExceptions ||
				info.Exception == null && time >= minCallTime)
			{
				string exText = null;

				if (info.Exception != null)
					exText = string.Format(
						" with exception '{0}' - \"{1}\"",
						info.Exception.GetType().FullName,
						info.Exception.Message);

				LogOutput(
					string.Format("{0}: {1}.{2}({3}) - {4} ms{5}.",
						end,
						info.MethodInfo.DeclaringType.FullName,
						info.MethodInfo.Name,
						parameters,
						time,
						exText),
					fileName);
			}
		}

		private static bool _logParameters;
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

		private static LogOutput _logOutput = new LogOutput(LogOutputInternal);
		public  static LogOutput  LogOutput
		{
			get
			{
				if (_logOutput == null)
					_logOutput = new LogOutput(LogOutputInternal);

				return _logOutput;
			}

			set { _logOutput = value; }
		}

		private static void LogOutputInternal(string logText, string fileName)
		{
			if (fileName == null || fileName.Length == 0)
				Debug.WriteLine(logText);
			else
				using (StreamWriter sw = new StreamWriter(fileName)) 
					sw.WriteLine(logText);
		}
	}
}
