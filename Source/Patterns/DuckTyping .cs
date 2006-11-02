using System;
using System.Collections;

using BLToolkit.Common;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Patterns
{
	public
#if FW2
		static
#endif
		class	DuckTyping
	{
		private static Hashtable _duckTypes = new Hashtable();

		public static Type GetDuckType(Type interfaceType, Type objectType)
		{
			if (!interfaceType.IsInterface) throw new ArgumentException("'interfaceType' must be an interface.");
			if (interfaceType.IsNotPublic)  throw new ArgumentException("The interface muat be public.");

			Hashtable types = (Hashtable)_duckTypes[interfaceType];

			if (types == null)
			{
				lock (_duckTypes.SyncRoot)
				{
					types = (Hashtable)_duckTypes[interfaceType];

					if (types == null)
						_duckTypes.Add(interfaceType, types = new Hashtable());
				}
			}

			Type type = (Type)types[objectType];

			if (type == null)
			{
				lock (types.SyncRoot)
				{
					type = (Type)types[objectType];

					if (type == null)
					{
						type = TypeFactory.GetType(
							new CompoundValue(interfaceType, objectType),
							objectType,
							new DuckTypeBuilder(interfaceType, objectType));

						types.Add(objectType, type);
					}
				}
			}

			return type;
		}

		public static object Implement(Type interfaceType, Type baseObjectType, object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");

			Type duckType = GetDuckType(interfaceType, baseObjectType);

			if (!TypeHelper.IsSameOrParent(baseObjectType, obj.GetType()))
				throw new ArgumentException(
					string.Format("'obj' must be a type of '{0}'.", baseObjectType.FullName));

			object duck = TypeAccessor.CreateInstanceEx(duckType);

			((DuckType)duck).SetObject(obj);

			return duck;
		}

		public static object Implement(Type interfaceType, object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");

			return Implement(interfaceType, obj.GetType(), obj);
		}

		public static object[] Implement(Type interfaceType, Type baseObjectType, params object[] objects)
		{
			if (objects == null) throw new ArgumentNullException("objects");

			object[] result = new object[objects.Length];

			for (int i = 0; i < objects.Length; i++)
				result[i] = Implement(interfaceType, baseObjectType, objects[i]);

			return result;
		}

		public static object[] Implement(Type interfaceType, params object[] objects)
		{
			if (objects == null) throw new ArgumentNullException("objects");

			object[] result = new object[objects.Length];

			for (int i = 0; i < objects.Length; i++)
				result[i] = Implement(interfaceType, objects[i].GetType(), objects[i]);

			return result;
		}

#if FW2

		public static I Implement<I>(object obj)
			where I : class
		{
			if (obj == null) throw new ArgumentNullException("obj");

			return (I)Implement(typeof(I), obj.GetType(), obj);
		}

		public static I Implement<I,T>(object obj)
			where I : class
			where T : class
		{
			return (I)Implement(typeof(I), typeof(T), obj);
		}

		public static I[] Implement<I>(params object[] objects)
			where I : class
		{
			if (objects == null) throw new ArgumentNullException("objects");

			I[] result = new I[objects.Length];

			for (int i = 0; i < objects.Length; i++)
				result[i] = Implement<I>(objects[i]);

			return result;
		}

		public static I[] Implement<I,T>(params object[] objects)
			where I : class
			where T : class
		{
			if (objects == null) throw new ArgumentNullException("objects");

			I[] result = new I[objects.Length];

			for (int i = 0; i < objects.Length; i++)
				result[i] = Implement<I,T>(objects[i]);

			return result;
		}

#endif
	}
}
