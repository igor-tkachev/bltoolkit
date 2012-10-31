using System;

using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace BLToolkit.Demo.ObjectModel
{
	public abstract class BizEntity : EditableObject
	{
		[PrimaryKey(0), NonUpdatable]
		public abstract int ID { get; protected internal set; }

		public virtual BizEntity CopyTo(BizEntity obj)
		{
			Map.ObjectToObject(this, obj);

			return obj;
		}

		public virtual BizEntity Clone()
		{
			BizEntity obj = (BizEntity)TypeAccessor.CreateInstanceEx(GetType());

			CopyTo(obj);
			obj.AcceptChanges();

			return obj;
		}
	}
}
