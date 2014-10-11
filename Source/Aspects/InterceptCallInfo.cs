using System;
using System.Collections;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using BLToolkit.Properties;

namespace BLToolkit.Aspects
{
	[DebuggerStepThrough] 
	public sealed class InterceptCallInfo
	{
		public InterceptCallInfo()
		{
			_currentPrincipal = Thread.CurrentPrincipal;
			_currentThread    = Thread.CurrentThread;
		}

		private CallMethodInfo _callMethodInfo;
		public  CallMethodInfo  CallMethodInfo
		{
			get { return _callMethodInfo;  }
			set
			{
				if (_callMethodInfo == value)
				{
					// A race condition.
					//
					return;
				}

				if (_callMethodInfo != null)
					throw new InvalidOperationException(Resources.InterceptCallInfo_CallMethodInfoIsNotMutable);

				_callMethodInfo = value;

				int len = value.MethodInfo.GetParameters().Length;

				_parameterValues = len == 0? _emptyValues: new object[len];
			}
		}

		private readonly object[] _emptyValues = new object[0];

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

		private readonly DateTime _beginCallTime = DateTime.Now;
		public           DateTime  BeginCallTime
		{
			get { return _beginCallTime; }
		}

		private readonly IPrincipal _currentPrincipal;
		public           IPrincipal  CurrentPrincipal
		{
			get { return _currentPrincipal; }
		}

		private readonly Thread _currentThread;
		public           Thread  CurrentThread
		{
			get { return _currentThread; }
		}

		private bool _cached;
		public  bool  Cached
		{
			get { return _cached;  }
			set { _cached = value; }
		}

		private object _object;
		public  object  Object
		{
			get { return _object;  }
			set { _object = value; }
		}
	}
}
