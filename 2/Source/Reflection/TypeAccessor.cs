using System;

namespace BLToolkit.Reflection
{
	public abstract class TypeAccessor
	{
		protected TypeAccessor(Type type)
		{
			_type = type;
		}

		public abstract object CreateInstance();

		public virtual object CreateInstance(InitContext context)
		{
			return CreateInstance();
		}

		public virtual object CreateInstanceEx()
		{
			return _objectFactory != null? _objectFactory.CreateInstance(null): CreateInstance(null);
		}

		public virtual object CreateInstanceEx(InitContext context)
		{
			return _objectFactory != null? _objectFactory.CreateInstance(context): CreateInstance(context);
		}

#if FW2
		public T CreateInstance<T>()
		{
			return (T)CreateInstance();
		}

		public T CreateInstance<T>(InitContext context)
		{
			return (T)CreateInstance(context);
		}

		public T CreateInstanceEx<T>()
		{
			return (T)CreateInstanceEx();
		}

		public T CreateInstanceEx<T>(InitContext context)
		{
			return (T)CreateInstanceEx(context);
		}
#endif

		private IObjectFactory _objectFactory = null;
		public  IObjectFactory  ObjectFactory
		{
			get { return _objectFactory;  }
			set { _objectFactory = value; }
		}

		private Type _type;
		public  Type  Type
		{
			get { return _type; }
		}

		public static TypeAccessor GetAccessor(Type type)
		{
			return new TempAccessor(type);
		}

		class TempAccessor : TypeAccessor
		{
			public TempAccessor(Type type)
				: base(type)
			{
			}

			public override object CreateInstance()
			{
				return Activator.CreateInstance(Type);
			}
		}
	}
}
