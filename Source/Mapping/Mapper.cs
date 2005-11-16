using System;
using System.Collections;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class Mapper
	{
		public virtual IObjectMapper GetObjectMapper(Type type)
		{
			IObjectMapper om = (IObjectMapper)_mappers[type];

			return om != null? om: Map.DefaultMapper.GetObjectMapper(type);
		}

		public void SetObjectMapper(Type type, IObjectMapper om)
		{
			if (type == null) throw new ArgumentNullException("type");

			_mappers[type] = om;

			if (type.IsAbstract)
				_mappers[TypeAccessor.GetAccessor(type).Type] = om;
		}

		private   Hashtable _mappers = new Hashtable();
		protected Hashtable  Mappers
		{
			get { return _mappers; }
		}
	}
}
