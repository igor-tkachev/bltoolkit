using System;

namespace BLToolkit.Aspects
{
	/// <summary>
	/// http://www.bltoolkit.net/Doc/Aspects/index.htm
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
	public class MixinOverrideAttribute : Attribute
	{
		public MixinOverrideAttribute(Type targetInterface, string methodName)
		{
			_targetInterface = targetInterface;
			_methodName      = methodName;
		}

		public MixinOverrideAttribute(Type targetInterface)
			: this(targetInterface, null)
		{
		}

		public MixinOverrideAttribute(string methodName)
			: this(null, methodName)
		{
		}

		public MixinOverrideAttribute()
		{
		}

		private Type _targetInterface;
		public  Type  TargetInterface
		{
			get { return _targetInterface; }
			set { _targetInterface = value; }
		}

		private string _methodName;
		public  string  MethodName
		{
			get { return _methodName; }
			set { _methodName = value; }
		}
	}
}
