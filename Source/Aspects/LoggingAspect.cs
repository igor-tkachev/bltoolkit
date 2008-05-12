using System;
using System.Diagnostics;
using System.IO;

namespace BLToolkit.Aspects
{
	public delegate void LogOperation(InterceptCallInfo interceptCallInfo);
	public delegate void LogOutput   (string logText, string fileName);

	/// <summary>
	/// http://www.bltoolkit.net/Doc/Aspects/index.htm
	/// </summary>
	public class LoggingAspect : Interceptor
	{
		protected override void OnFinally(InterceptCallInfo info)
		{
			if (IsEnabled)
				LogOperation(info);
		}

		#region Config Support

		internal class ConfigParameters
		{
			public object FileName;
			public object MinCallTime;
			public object LogExceptions;
			public object LogParameters;
		}

		private static ConfigParameters GetConfigParameters(InterceptCallInfo info)
		{
			ConfigParameters cp = info.CallMethodInfo.LogParameters;

			if (cp == null)
			{
				info.CallMethodInfo.LogParameters = cp = new ConfigParameters();

				string[] ps = info.ConfigString.Split(';');

				foreach (string p in ps)
				{
					string[] vs = p.Split('=');

					if (vs.Length == 2)
					{
						switch (vs[0].ToLower().Trim())
						{
							case "filename":      cp.FileName      =            vs[1].Trim();  break;
							case "mincalltime":   cp.MinCallTime   = int. Parse(vs[1].Trim()); break;
							case "logexceptions": cp.LogExceptions = bool.Parse(vs[1].Trim()); break;
							case "logparameters": cp.LogParameters = bool.Parse(vs[1].Trim()); break;
						}
					}
				}
			}

			return cp;
		}

		#endregion

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

		private static void LogOperationInternal(InterceptCallInfo info)
		{
			string   fileName      = FileName;
			int      minCallTime   = MinCallTime;
			bool     logExceptions = LogExceptions;
			bool     logParameters = LogParameters;

			if (!string.IsNullOrEmpty(info.ConfigString))
			{
				ConfigParameters cp = GetConfigParameters(info);

				if (cp.FileName      != null) fileName      =       cp.FileName.ToString();
				if (cp.MinCallTime   != null) minCallTime   = (int) cp.MinCallTime;
				if (cp.LogExceptions != null) logExceptions = (bool)cp.LogExceptions;
				if (cp.LogParameters != null) logParameters = (bool)cp.LogParameters;
			}

			DateTime end  = DateTime.Now;
			int      time = (int)((end - info.BeginCallTime).TotalMilliseconds);

			if (info.Exception != null && logExceptions ||
				info.Exception == null && time >= minCallTime)
			{
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
						parameters,
						time,
						exText),
					fileName);
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
