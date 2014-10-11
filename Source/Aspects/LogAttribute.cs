using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	/// <summary>
	/// http://www.bltoolkit.net/Doc/Aspects/index.htm
	/// </summary>
	[AttributeUsage(
		AttributeTargets.Class |
		AttributeTargets.Interface |
		AttributeTargets.Property |
		AttributeTargets.Method,
		AllowMultiple=true)]
	public class LogAttribute : InterceptorAttribute
	{
		public LogAttribute()
			: this(typeof(LoggingAspect), null)
		{
		}

		public LogAttribute(string parameters)
			: this(typeof(LoggingAspect), parameters)
		{
		}

		protected LogAttribute(Type interceptorType, string parameters)
			: base(
				interceptorType,
				InterceptType.OnCatch | InterceptType.OnFinally,
				parameters,
				TypeBuilderConsts.Priority.LoggingAspect)
		{
		}

		private bool _hasFileName;
		private string  _fileName;
		public  string   FileName
		{
			get { return _fileName; }
			set { _fileName = value; _hasFileName = true; }
		}

		private bool _hasMinCallTime;
		private int     _minCallTime;
		public  int      MinCallTime
		{
			get { return _minCallTime; }
			set { _minCallTime = value; _hasMinCallTime = true; }
		}

		private bool _hasLogExceptions;
		private bool    _logExceptions;
		public  bool     LogExceptions
		{
			get { return _logExceptions; }
			set { _logExceptions = value; _hasLogExceptions = true;}
		}

		private bool _hasLogParameters;
		private bool    _logParameters;
		public  bool     LogParameters
		{
			get { return _logParameters; }
			set { _logParameters = value; _hasLogParameters = true;}
		}

		public override string ConfigString
		{
			get
			{
				string s = base.ConfigString;

				if (_hasFileName)      s += ";FileName="      + FileName;
				if (_hasMinCallTime)   s += ";MinCallTime="   + MinCallTime;
				if (_hasLogExceptions) s += ";LogExceptions=" + LogExceptions;
				if (_hasLogParameters) s += ";LogParameters=" + LogParameters;

				if (!string.IsNullOrEmpty(s) && s[0] == ';')
					s = s.Substring(1);

				return s;
			}
		}
	}
}
