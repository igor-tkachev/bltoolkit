using System;

namespace BLToolkit.Patterns
{
	public abstract class DuckType
	{
		[CLSCompliant(false)]
		protected object[] _objects;
		public    object[]  Objects
		{
			get { return _objects; }
		}

		internal void SetObjects(params object[] objs)
		{
			_objects = objs;
		}
	}
}
