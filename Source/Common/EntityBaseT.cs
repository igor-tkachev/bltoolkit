using System;

using BLToolkit.Reflection;

namespace BLToolkit.Common
{
	[Serializable]
	public abstract class EntityBase<T> : EntityBase
		where T : EntityBase<T>
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
	}
}
