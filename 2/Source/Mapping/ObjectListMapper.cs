using System;
using System.Collections;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class ObjectListMapper : IMapDataSourceList, IMapDataDestinationList
	{
		public ObjectListMapper(IList list, ObjectMapper objectMapper)
		{
			_list   = list;
			_mapper = objectMapper;
		}

		private IList        _list;
		private ObjectMapper _mapper;
		private int          _currentItem;

		#region IMapDataSourceList Members

		void IMapDataSourceList.InitMapping(InitContext initContext)
		{
			initContext.DataSource = _mapper;
		}

		bool IMapDataSourceList.SetNextDataSource(InitContext initContext)
		{
			if (_currentItem >= _list.Count)
				return false;

			initContext.SourceObject = _list[_currentItem];
			_currentItem++;

			return true;
		}

		void IMapDataSourceList.EndMapping(InitContext initContext)
		{
		}

		#endregion

		#region IMapDataDestinationList Members

		void IMapDataDestinationList.InitMapping(InitContext initContext)
		{
			ISupportMapping sm = _list as ISupportMapping;

			if (sm != null)
			{
				sm.BeginMapping(initContext);

				if (initContext.ObjectMapper != null && _mapper != initContext.ObjectMapper)
					_mapper = initContext.ObjectMapper;
			}
		}

		IMapDataDestination IMapDataDestinationList.GetDataDestination(InitContext initContext)
		{
			return _mapper;
		}

		object IMapDataDestinationList.GetNextObject(InitContext initContext)
		{
			object obj = _mapper.CreateInstance(initContext);

			_list.Add(obj);

			return obj;
		}

		void IMapDataDestinationList.EndMapping(InitContext initContext)
		{
			ISupportMapping sm = _list as ISupportMapping;

			if (sm != null)
				sm.EndMapping(initContext);
		}

		#endregion
	}
}
