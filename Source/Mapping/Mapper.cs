using System;
using System.Collections;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class Mapper
	{
		private Hashtable _mappers = new Hashtable();

		public IObjectMapper GetObjectMapper(Type type)
		{
			IObjectMapper om = (IObjectMapper)_mappers[type];

			if (om == null)
			{
				om = CreateMapper(type);

				if (om == null)
					om = Map.DefaulMapper.CreateMapper(type);

				SetObjectMapper(type, om);
			}

			return om;
		}

		public void SetObjectMapper(Type type, IObjectMapper om)
		{
			if (type == null) throw new ArgumentNullException("type");

			_mappers[type] = om;

			if (type.IsAbstract)
				_mappers[TypeAccessor.GetAccessor(type).Type] = om;
		}

		protected virtual IObjectMapper CreateMapper(Type type)
		{
			return null;
		}
	}
}
