using System;
using System.Reflection;

namespace BLToolkit.Aspects
{
	public class InterceptCallInfo
	{
		private MethodInfo _methodInfo;
		public  MethodInfo  MethodInfo
		{
			get { return _methodInfo;  }
			set
			{
				if (_methodInfo != null)
					throw new InvalidOperationException("MethodInfo can not be changed.");

				_methodInfo      = value;
				_parameterValues = new object[value.GetParameters().Length];
			}
		}

		private object[] _parameterValues;
		public  object[]  ParameterValues
		{
			get { return _parameterValues; }
		}

		private object _returnValue;
		public  object  ReturnValue
		{
			get { return _returnValue;  }
			set { _returnValue = value; }
		}

		private bool _skipCall;
		public  bool  SkipCall
		{
			get { return _skipCall;  }
			set { _skipCall = value; }
		}
	}
}
