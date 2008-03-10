using System;

using BLToolkit.Reflection;

namespace BLToolkit.EditableObjects
{
	[Serializable]
	public abstract class EditableObject<T> : EditableObject
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
			return (T)TypeAccessor.Copy(this);
		}
			
		#endregion

		#region Copy

		public void CopyTo(T dest)
		{
			TypeAccessor.Copy(this, dest);
		}

		#endregion

	}
}
