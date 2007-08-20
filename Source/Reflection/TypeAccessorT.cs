using System;

namespace BLToolkit.Reflection
{
	public static class TypeAccessor<T>
	{
		#region CreateInstance

		[System.Diagnostics.DebuggerStepThrough]
		public static T CreateInstance()
		{
			return (T)_instance.CreateInstance();
		}

		[System.Diagnostics.DebuggerStepThrough]
		public static T CreateInstance(InitContext context)
		{
			return (T)_instance.CreateInstance(context);
		}

		[System.Diagnostics.DebuggerStepThrough]
		public static T CreateInstanceEx()
		{
			return (T)_instance.CreateInstanceEx();
		}

		[System.Diagnostics.DebuggerStepThrough]
		public static T CreateInstanceEx(InitContext context)
		{
			return (T)_instance.CreateInstanceEx(context);
		}

		#endregion

		#region Copy & AreEqual

		public static T Copy(T source, T dest)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (dest   == null) throw new ArgumentNullException("dest");

			foreach (MemberAccessor ma in _instance)
				ma.SetValue(dest, TypeAccessor.CloneOrCopy(ma.GetValue(source)));

			return dest;
		}

		public static T Copy(T source)
		{
			if (source == null) return source;

			T dest = CreateInstanceEx();

			foreach (MemberAccessor ma in _instance)
				ma.SetValue(dest, TypeAccessor.CloneOrCopy(ma.GetValue(source)));

			return dest;
		}

		public static bool AreEqual(T obj1, T obj2)
		{
			if (ReferenceEquals(obj1, obj2))
				return true;

			if (obj1 == null || obj2 == null)
				return false;

			foreach (MemberAccessor ma in _instance)
				if ((!Equals(ma.GetValue(obj1), ma.GetValue(obj2))))
					return false;

			return true;
		}

		#endregion

		public static IObjectFactory ObjectFactory
		{
			get { return _instance.ObjectFactory;  }
			set { _instance.ObjectFactory = value; }
		}

		public static Type Type         { get { return _instance.Type; } }
		public static Type OriginalType { get { return _instance.OriginalType; } }

		private static readonly TypeAccessor _instance = TypeAccessor.GetAccessor(typeof(T));
		public  static          TypeAccessor  Instance
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return _instance; }
		}
	}
}

