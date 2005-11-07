using System;

namespace BLToolkit.Reflection
{
	public abstract class TypeAccessor<T> : TypeAccessor
	{
		public new T CreateInstance()
		{
			return (T)CreateInstanceInternal();
		}

		public new T CreateInstance(InitContext context)
		{
			return (T)CreateInstanceInternal(context);
		}

		public new T CreateInstanceEx()
		{
			return (T)base.CreateInstanceEx();
		}

		public new T CreateInstanceEx(InitContext context)
		{
			return (T)base.CreateInstanceEx(context);
		}

		public static TypeAccessor<T> GetAccessor()
		{
			return (TypeAccessor<T>)GetAccessor(typeof(T));
		}
	}
}
