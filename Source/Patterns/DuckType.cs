using System;

namespace BLToolkit.Patterns
{
	public abstract class DuckType
	{
		[CLSCompliant(false)]
		protected object _object;
		public    object  Object
		{
			get { return _object; }
		}

		internal void SetObject(object obj)
		{
			_object = obj;
		}
	}
}
