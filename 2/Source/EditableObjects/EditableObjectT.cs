using System;

using BLToolkit.Reflection;

namespace BLToolkit.EditableObjects
{
	[Serializable]
	public abstract class EditableObject<T> : EditableObject, IEquatable<T>
		where T : EditableObject<T>
	{
		#region CreateInstance

		public static T CreateInstance()
		{
			return TypeAccessor.CreateInstanceEx<T>();
		}

		#endregion

		#region Clone

		public virtual T Clone()
		{
			return TypeAccessor<T>.Copy((T)this);
		}
			
		#endregion

		#region IEquatable<T> Members

		///<summary>
		///Indicates whether the current object is equal to another object of the same type.
		///</summary>
		///<returns>
		///true if the current object is equal to the other parameter; otherwise, false.
		///</returns>
		///<param name="other">An object to compare with this object.</param>
		bool IEquatable<T>.Equals(T other)
		{
			return TypeAccessor<T>.AreEqual((T)this, other);
		}

		#endregion
	}
}
