namespace BLToolkit.ComponentModel
{
	public abstract class TypeDescriptorExtender<T> : TypeDescriptorExtender
	{
		public TypeDescriptorExtender()
			: base(typeof(T))
		{
		}

		public TypeDescriptorExtender(T t)
			: base(t)
		{
		}

		public new T BaseObject
		{
			get { return (T)base.BaseObject; }
		}
	}
}
