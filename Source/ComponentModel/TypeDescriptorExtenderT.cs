using System;

namespace BLToolkit.ComponentModel
{
	public abstract class TypeDescriptorExtender<T> : TypeDescriptorExtender
	{
		protected TypeDescriptorExtender()
			: base(typeof(T))
		{
		}

		protected TypeDescriptorExtender(T t)
			: base(t)
		{
		}

		public new T BaseObject
		{
			get { return (T)base.BaseObject; }
		}
	}
}
