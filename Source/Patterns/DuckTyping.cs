using System;
using System.Collections;

using BLToolkit.Common;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Patterns
{
	/// <summary>
	/// Duck typing implementation.
	/// In computer science, duck typing is a term for dynamic typing typical of some programming languages,
	/// such as Smalltalk, Python or ColdFusion, where a variable's value itself determines what the variable can do.
	/// Thus an object having all the methods described in an interface can be made to implement that interface
	/// dynamically at runtime, even if the object’s class does not include the interface in its implements clause.
	/// </summary>
	public static class DuckTyping
	{
		private static readonly Hashtable _duckTypes = new Hashtable();

		/// <summary>
		/// Build a proxy type which implements the requested interface by redirecting all calls to the supplied object type.
		/// </summary>
		/// <param name="interfaceType">An interface type to implement.</param>
		/// <param name="objectType">Any type which has all members of the given interface.</param>
		/// <returns>The duck object type.</returns>
		public static Type GetDuckType(Type interfaceType, Type objectType)
		{
			if (interfaceType == null)      throw new ArgumentNullException("interfaceType");
			if (!interfaceType.IsInterface) throw new ArgumentException("'interfaceType' must be an interface.", "interfaceType");
			if (!interfaceType.IsPublic && !interfaceType.IsNestedPublic)
				throw new ArgumentException("The interface must be public.", "interfaceType");

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

					if (type != null || types.ContainsKey(objectType))
						return type;

					type = TypeFactory.GetType(
						new CompoundValue(interfaceType, objectType),
						objectType,
						new DuckTypeBuilder(interfaceType, objectType));

					types.Add(objectType, type);
				}
			}

			return type;
		}

		/// <summary>
		/// Implements the requested interface for supplied object.
		/// If the supplied object implements the interface, the object itself will be returned.
		/// Otherwise a convenient duck object will be created.
		/// </summary>
		/// <param name="interfaceType">An interface type to implement.</param>
		/// <param name="baseObjectType">Any type which has all members of the given interface.
		/// When this parameter is set to null, the object type will be used.</param>
		/// <param name="obj">An object which type has all members of the given interface.</param>
		/// <returns>An object which implements the interface.</returns>
		public static object Implement(Type interfaceType, Type baseObjectType, object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");

			Type objType = obj.GetType();

			if (TypeHelper.IsSameOrParent(interfaceType, objType))
				return obj;

			if (obj is DuckType)
			{
				// Switch to underlying object when a duck object was passed.
				//
				return Implement(interfaceType, baseObjectType, ((DuckType)obj).Object);
			}

			if (baseObjectType == null)
				baseObjectType = objType;
			else if (!TypeHelper.IsSameOrParent(baseObjectType, objType))
				throw new ArgumentException(
					string.Format("'{0}' is not a subtype of '{1}'.", objType.FullName, baseObjectType.FullName), "obj");

			Type duckType = GetDuckType(interfaceType, baseObjectType);

			if (duckType == null)
				return null;

			object duck = TypeAccessor.CreateInstanceEx(duckType);

			((DuckType)duck).SetObject(obj);

			return duck;
		}

		/// <summary>
		/// Implements the requested interface.
		/// If the supplied object implements the interface, the object itself will be returned.
		/// Otherwise a convenient duck object will be created.
		/// </summary>
		/// <param name="interfaceType">An interface type to implement.</param>
		/// <param name="obj">An object which type has all members of the given interface.</param>
		/// <returns>An object which implements the interface.</returns>
		public static object Implement(Type interfaceType, object obj)
		{
			return Implement(interfaceType, null, obj);
		}

		/// <summary>
		/// Implements the requested interface for all supplied objects.
		/// If any of supplied object implements the interface, the object itself will be returned.
		/// Otherwise a convenient duck object will be created.
		/// </summary>
		/// <param name="interfaceType">An interface type to implement.</param>
		/// <param name="baseObjectType">Any type which has all members of the given interface.
		/// When this parameter is set to null, the object type will be used.</param>
		/// <param name="objects">An object array which types has all members of the given interface.
		/// All objects may have different types.</param>
		/// <returns>An array of object which implements the interface.</returns>
		public static object[] Implement(Type interfaceType, Type baseObjectType, params object[] objects)
		{
			if (objects == null) throw new ArgumentNullException("objects");

			object[] result = new object[objects.Length];
	
			for (int i = 0; i < objects.Length; i++)
				result[i] = Implement(interfaceType, baseObjectType, objects[i]);

			return result;
		}

		/// <summary>
		/// Implements the requested interface for all supplied objects.
		/// If any of supplied object implements the interface, the object itself will be returned.
		/// Otherwise a convenient duck object will be created.
		/// </summary>
		/// <param name="interfaceType">An interface type to implement.</param>
		/// <param name="objects">An object array which types has all members of the given interface.
		/// All objects may have different types.</param>
		/// <returns>An array of object which implements the interface.</returns>
		public static object[] Implement(Type interfaceType, params object[] objects)
		{
			return Implement(interfaceType, null, objects);
		}

		/// <summary>
		/// Implements the requested interface for supplied object.
		/// If the supplied object implements the interface, the object itself will be returned.
		/// Otherwise a convenient duck object will be created.
		/// </summary>
		/// <typeparam name="I">An interface type to implement.</typeparam>
		/// <param name="obj">An object which type has all members of the given interface.</param>
		/// <returns>An object which implements the interface.</returns>
		public static I Implement<I>(object obj)
			where I : class
		{
			return (I)Implement(typeof(I), null, obj);
		}

		/// <summary>
		/// Implements the requested interface for supplied object.
		/// If the supplied object implements the interface, the object itself will be returned.
		/// Otherwise a convenient duck object will be created.
		/// </summary>
		/// <typeparam name="I">An interface type to implement.</typeparam>
		/// <typeparam name="T">Any type which has all members of the given interface.</typeparam>
		/// <param name="obj">An object which type has all members of the given interface.</param>
		/// <returns>An object which implements the interface.</returns>
		public static I Implement<I,T>(T obj)
			where I : class
		{
			return (I)Implement(typeof(I), typeof(T), obj);
		}

		/// <summary>
		/// Implements the requested interface for all supplied objects.
		/// If any of supplied object implements the interface, the object itself will be returned.
		/// Otherwise a convenient duck object will be created.
		/// </summary>
		/// <typeparam name="I">An interface type to implement.</typeparam>
		/// <param name="objects">An object array which types has all members of the given interface.
		/// All objects may have different types.</param>
		/// <returns>An array of object which implements the interface.</returns>
		public static I[] Implement<I>(params object[] objects)
			where I : class
		{
			if (objects == null) throw new ArgumentNullException("objects");

			I[] result = new I[objects.Length];

			for (int i = 0; i < objects.Length; i++)
				result[i] = Implement<I>(objects[i]);

			return result;
		}

		/// <summary>
		/// Implements the requested interface for all supplied objects.
		/// If any of supplied object implements the interface, the object itself will be returned.
		/// Otherwise a convenient duck object will be created.
		/// </summary>
		/// <typeparam name="I">An interface type to implement.</typeparam>
		/// <typeparam name="T">Any type which has all members of the given interface.</typeparam>
		/// <param name="objects">An object array which types has all members of the given interface.
		/// All objects may have different types.</param>
		/// <returns>An array of object which implements the interface.</returns>
		public static I[] Implement<I,T>(params T[] objects)
			where I : class
		{
			if (objects == null) throw new ArgumentNullException("objects");

			I[] result = new I[objects.Length];

			for (int i = 0; i < objects.Length; i++)
				result[i] = Implement<I,T>(objects[i]);

			return result;
		}

		private static bool _allowStaticMembers;
		public  static bool  AllowStaticMembers
		{
			get { return _allowStaticMembers;  }
			set { _allowStaticMembers = value; }
		}
	}
}
