using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace BLToolkit.Aspects
{
	[DebuggerStepThrough] 
	public sealed class InterceptCallInfo
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
			get { return _parameterValues;  }
			set { _parameterValues = value; }
		}

		private object _returnValue;
		public  object  ReturnValue
		{
			get { return _returnValue;  }
			set { _returnValue = value; }
		}

		private InterceptResult _interceptResult = InterceptResult.Continue;
		public  InterceptResult  InterceptResult
		{
			get { return _interceptResult;  }
			set { _interceptResult = value; }
		}

		private InterceptType _interceptType;
		public  InterceptType  InterceptType
		{
			get { return _interceptType;  }
			set { _interceptType = value; }
		}

		private Exception _exception;
		public  Exception  Exception
		{
			get { return _exception;  }
			set { _exception = value; }
		}

		private string _configString;
		public  string  ConfigString
		{
			get { return _configString;  }
			set { _configString = value; }
		}

		private Hashtable  _items;
		public  IDictionary Items
		{
			get
			{
				if (_items == null)
					_items = new Hashtable();

				return _items;
			}
		}

		private DateTime _beginCallTime = DateTime.Now;
		public  DateTime  BeginCallTime
		{
			get { return _beginCallTime; }
		}
	}
}
