using System;
using System.Collections;

using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Reflection
{
	public abstract class TypeAccessor
	{
		protected TypeAccessor()
		{
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

		public abstract Type Type         { get; }
		public abstract Type OriginalType { get; }

		private static Hashtable _accessors = new Hashtable(10);

		public static TypeAccessor GetAccessor(Type originalType)
		{
			TypeAccessor accessor = (TypeAccessor)_accessors[originalType];

			if (accessor == null)
			{
				lock (_accessors.SyncRoot)
				{
					accessor = (TypeAccessor)_accessors[originalType];

					if (accessor == null)
					{
						Type type = originalType.IsAbstract?
							TypeFactory.GetType(originalType, new AbstractClassBuilder()):
							originalType;

						Type accessorType = 
							TypeFactory.GetType(originalType, new TypeAccessorBuilder(type, originalType));

						_accessors[originalType] = accessor = (TypeAccessor)Activator.CreateInstance(accessorType);

						if (type.IsAbstract)
							_accessors[type] = accessor;
					}
				}
			}

			return accessor;
		}
	}
}
