using System;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

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
			if (member == null) throw new ArgumentNullException("member");

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

		#endregion

		#region ObjectFactory

		private IObjectFactory _objectFactory;
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
			if (originalType == null) throw new ArgumentNullException("originalType");

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
			return TypeAccessor<T>.CreateInstance();
		}

		public static T CreateInstance<T>(InitContext context)
		{
			return TypeAccessor<T>.CreateInstance(context);
		}

		public static T CreateInstanceEx<T>()
		{
			return TypeAccessor<T>.CreateInstanceEx();
		}

		public static T CreateInstanceEx<T>(InitContext context)
		{
			return TypeAccessor<T>.CreateInstance(context);
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

		#region Write Object Info

		public static void WriteDebug(object o)
		{
#if DEBUG
			Write(o, false);
#endif
		}

		public static void WriteConsole(object o)
		{
			Write(o, true);
		}

		[SuppressMessage("Microsoft.Performance", "CA1818:DoNotConcatenateStringsInsideLoops")]
		private static string MapTypeName(Type type)
		{
#if FW2
			if (type.IsGenericType)
			{
				if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
					return string.Format("{0}?", MapTypeName(type.GetGenericArguments()[0]));

				string name = type.Name;

				int idx = name.IndexOf('`');

				if (idx >= 0)
					name = name.Substring(0, idx);

				name += "<";

				foreach (Type t in type.GetGenericArguments())
					name += MapTypeName(t) + ',';

				if (name[name.Length - 1] == ',')
					name = name.Substring(0, name.Length - 1);

				name += ">";

				return name;
			}
#endif
			if (type.IsPrimitive ||
				type == typeof(string) ||
				type == typeof(object) ||
				type == typeof(decimal))
			{
				if (type == typeof(int))    return "int";
				if (type == typeof(bool))   return "bool";
				if (type == typeof(short))  return "short";
				if (type == typeof(long))   return "long";
				if (type == typeof(ushort)) return "ushort";
				if (type == typeof(uint))   return "uint";
				if (type == typeof(ulong))  return "ulong";
				if (type == typeof(float))  return "float";

				return type.Name.ToLower();
			}

			return type.Name;
		}

		[SuppressMessage("Microsoft.Usage", "CA2241:ProvideCorrectArgumentsToFormattingMethods")]
		private static void Write(object o, bool console)
		{
			if (o == null)
			{
				if (console) Console.WriteLine("*** (null) ***");
				else         Debug.  WriteLine("*** (null) ***");

				return;
			}

			TypeAccessor   ta      = TypeAccessor.GetAccessor(o.GetType());
			MemberAccessor ma;
			int            nameLen = 0;
			int            typeLen = 0;

			foreach (DictionaryEntry de in ta._members)
			{
				if (nameLen < de.Key.ToString().Length)
					nameLen = de.Key.ToString().Length;

				ma = (MemberAccessor)de.Value;

				if (typeLen < MapTypeName(ma.Type).Length)
					typeLen = MapTypeName(ma.Type).Length;
			}

			string text = "*** " + o.GetType().FullName + ": ***";

			if (console) Console.WriteLine(text);
			else         Debug.  WriteLine(text);

			string format = string.Format("{{0,-{0}}} {{1,-{1}}} : {{2}}", typeLen, nameLen);

			foreach (DictionaryEntry de in ta._members)
			{
				ma = (MemberAccessor)de.Value;

				object value = ma.GetValue(o);

				if (value == null)
					value = "(null)";
				else if (value is ICollection)
					value = string.Format("(Count = {0})", ((ICollection)value).Count);

				text = string.Format(format, MapTypeName(ma.Type), de.Key, value);

				if (console) Console.WriteLine(text);
				else         Debug.  WriteLine(text);
			}

			if (console) Console.WriteLine("***");
			else         Debug.  WriteLine("***");
		}

		#endregion
	}
}
