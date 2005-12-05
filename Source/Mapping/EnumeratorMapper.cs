using System;
using System.Collections;

namespace BLToolkit.Mapping
{
	public class EnumeratorMapper : IMapDataSourceList
	{
		public EnumeratorMapper(IEnumerator enumerator)
		{
			_enumerator = enumerator;
		}

		private IEnumerator _enumerator;
		private Type        _objectType;

		#region IMapDataSourceList Members

		public void InitMapping(BLToolkit.Reflection.InitContext initContext)
		{
			_enumerator.Reset();
		}

		public bool SetNextDataSource(BLToolkit.Reflection.InitContext initContext)
		{
			if (_enumerator.MoveNext() == false)
				return false;

			object sourceObject = _enumerator.Current;

			if (_objectType != sourceObject.GetType())
			{
				_objectType = sourceObject.GetType();
				initContext.DataSource = initContext.MappingSchema.GetObjectMapper(_objectType);
			}

			initContext.SourceObject = sourceObject;

			return true;
		}

		public void EndMapping(BLToolkit.Reflection.InitContext initContext)
		{
		}

		#endregion
	}
}
