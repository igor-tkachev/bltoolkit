using System;

using BLToolkit.Reflection;

namespace BLToolkit.EditableObjects
{
	[Serializable]
	public abstract class EditableObject<T> : EditableObject, IComparable<T>, IEquatable<T>
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

		#region IComparable<T> Members

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects
		/// being compared. The return value has the following meanings:
		/// Value Meaning Less than zero This object is less than the other parameter
		/// Zero This object is equal to other.
		/// Greater than zero This object is greater than other.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		int IComparable<T>.CompareTo(T other)
		{
			return TypeAccessor<T>.Compare((T)this, other);
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
			return TypeAccessor<T>.Compare((T)this, other) == 0;
		}

		#endregion
	}
}
