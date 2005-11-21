using System;
using System.Diagnostics.CodeAnalysis;

namespace BLToolkit.Reflection
{
	public static class TypeAccessor<T>
	{
		public static T CreateInstance()
		{
			return (T)_accessor.CreateInstance();
		}

		public static T CreateInstance(InitContext context)
		{
			return (T)_accessor.CreateInstance(context);
		}

		public static T CreateInstanceEx()
		{
			return (T)_accessor.CreateInstanceEx();
		}

		public static T CreateInstanceEx(InitContext context)
		{
			return (T)_accessor.CreateInstanceEx(context);
		}

		public static IObjectFactory  ObjectFactory
		{
			get { return _accessor.ObjectFactory;  }
			set { _accessor.ObjectFactory = value; }
		}

		public static Type Type         { get { return _accessor.Type; } }
		public static Type OriginalType { get { return _accessor.OriginalType; } }

		private static TypeAccessor _accessor = TypeAccessor.GetAccessor(typeof(T));
		public  static TypeAccessor  Accessor
		{
			get { return _accessor; }
		}
	}
}

