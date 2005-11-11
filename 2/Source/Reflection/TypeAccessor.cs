using System;
using System.Collections;
using System.Reflection;

using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Reflection
{
	public abstract class TypeAccessor : ICollection
	{
		protected TypeAccessor()
		{
		}

		#region Protected Emit Helpers

		protected MemberInfo GetMember(int memberType, string memberName)
		{
			MemberInfo mi;

			switch (memberType)
			{
				case 1: mi = Type.GetField   (memberName); break;
				case 2: mi = Type.GetProperty(memberName); break;
				default:
					throw new InvalidOperationException();
			}

			return mi;
		}

		protected void AddMember(MemberAccessor member)
		{
			_members.Add(member.MemberInfo.Name, member);
		}

		#endregion

		#region CreateInstance

		public virtual object CreateInstance()
		{
			throw new InvalidOperationException();
		}

		public virtual object CreateInstance(InitContext context)
		{
			return CreateInstance();
		}

		public object CreateInstanceEx()
		{
			return _objectFactory != null?
				_objectFactory.CreateInstance(this, null): CreateInstance((InitContext)null);
		}

		public object CreateInstanceEx(InitContext context)
		{
			return _objectFactory != null? _objectFactory.CreateInstance(this, context): CreateInstance(context);
		}

#if FW2

		protected object CreateInstanceInternal()
		{
			return CreateInstance();
		}

		protected object CreateInstanceInternal(InitContext context)
		{
			return CreateInstance(context);
		}

#endif

		#endregion

		#region ObjectFactory

		private IObjectFactory _objectFactory = null;
		public  IObjectFactory  ObjectFactory
		{
			get { return _objectFactory;  }
			set { _objectFactory = value; }
		}

		#endregion

		#region Abstract Members

		public abstract Type Type         { get; }
		public abstract Type OriginalType { get; }

		#endregion

		#region Items

		private Hashtable _members = new Hashtable();

		public MemberAccessor this[string memberName]
		{
			get { return (MemberAccessor)_members[memberName]; }
		}

		#endregion

		#region Static Members

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

		public static object CreateInstance(Type type)
		{
			return GetAccessor(type).CreateInstance();
		}

		public static object CreateInstance(Type type, InitContext context)
		{
			return GetAccessor(type).CreateInstance(context);
		}

		public static object CreateInstanceEx(Type type)
		{
			return GetAccessor(type).CreateInstanceEx();
		}

		public static object CreateInstanceEx(Type type, InitContext context)
		{
			return GetAccessor(type).CreateInstance(context);
		}

#if FW2

		public static T CreateInstance<T>()
		{
			return (T)GetAccessor(typeof(T)).CreateInstance();
		}

		public static T CreateInstance<T>(InitContext context)
		{
			return (T)GetAccessor(typeof(T)).CreateInstance(context);
		}

		public static T CreateInstanceEx<T>()
		{
			return (T)GetAccessor(typeof(T)).CreateInstanceEx();
		}

		public static T CreateInstanceEx<T>(InitContext context)
		{
			return (T)GetAccessor(typeof(T)).CreateInstance(context);
		}

#endif

		#endregion

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			_members.Values.CopyTo(array, index);
		}

		public int Count
		{
			get { return _members.Count; }
		}

		public bool IsSynchronized
		{
			get { return _members.IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return _members.SyncRoot; }
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _members.Values.GetEnumerator();
		}

		#endregion
	}
}
