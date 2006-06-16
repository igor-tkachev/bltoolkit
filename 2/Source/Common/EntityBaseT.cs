using System;

using BLToolkit.Reflection;

namespace BLToolkit.Common
{
	[Serializable]
	public abstract class EntityBase<T> : EntityBase
		where T : EntityBase<T>
	{
		#region Clone

		public virtual T Clone()
		{
			return (T)TypeAccessor.Copy(this);
		}

		#endregion
	}
}
