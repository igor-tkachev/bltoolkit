using System;
using System.Diagnostics.CodeAnalysis;

namespace BLToolkit.Reflection
{
	public static class TypeAccessor<T>
	{
		public static T CreateInstance()
		{
			return (T)_instance.CreateInstance();
		}

		public static T CreateInstance(InitContext context)
		{
			return (T)_instance.CreateInstance(context);
		}

		public static T CreateInstanceEx()
		{
			return (T)_instance.CreateInstanceEx();
		}

		public static T CreateInstanceEx(InitContext context)
		{
			return (T)_instance.CreateInstanceEx(context);
		}

		public static IObjectFactory  ObjectFactory
		{
			get { return _instance.ObjectFactory;  }
			set { _instance.ObjectFactory = value; }
		}

		public static Type Type         { get { return _instance.Type; } }
		public static Type OriginalType { get { return _instance.OriginalType; } }

		private static TypeAccessor _instance = TypeAccessor.GetAccessor(typeof(T));
		public  static TypeAccessor  Instance
		{
			get { return _instance; }
		}
	}
}

