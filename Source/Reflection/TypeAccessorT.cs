using System;
using System.Diagnostics.CodeAnalysis;

namespace BLToolkit.Reflection
{
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
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

		[SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member")]
		public new T CreateInstanceEx()
		{
			return (T)base.CreateInstanceEx();
		}

		[SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member")]
		public new T CreateInstanceEx(InitContext context)
		{
			return (T)base.CreateInstanceEx(context);
		}

		[SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		[SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static TypeAccessor<T> GetAccessor()
		{
			return (TypeAccessor<T>)GetAccessor(typeof(T));
		}
	}
}
