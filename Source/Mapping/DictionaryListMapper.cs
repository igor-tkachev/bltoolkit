using System;
using System.Collections;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public sealed class DictionaryListMapper : IMapDataDestinationList
	{
		public DictionaryListMapper(IDictionary dic, string keyFieldName, ObjectMapper objectMapper)
		{
			_dic          = dic;
			_keyFieldName = keyFieldName;
			_mapper       = objectMapper;
		}

		private string       _keyFieldName;
		private IDictionary  _dic;
		private ObjectMapper _mapper;
		private object       _newObject;

		#region IMapDataDestinationList Members

		private void AddObject()
		{
			if (_newObject != null)
			{
				object key = _mapper.TypeAccessor[_keyFieldName].GetValue(_newObject);
				_dic[key]  = _newObject;
			}
		}

		void IMapDataDestinationList.InitMapping(InitContext initContext)
		{
			ISupportMapping sm = _dic as ISupportMapping;

			if (sm != null)
			{
				sm.BeginMapping(initContext);

				if (_mapper != initContext.ObjectMapper)
					_mapper = initContext.ObjectMapper;
			}
		}

		IMapDataDestination IMapDataDestinationList.GetDataDestination(InitContext initContext)
		{
			return _mapper;
		}

		object IMapDataDestinationList.GetNextObject(InitContext initContext)
		{
			AddObject();
			return _newObject = _mapper.CreateInstance(initContext);
		}

		void IMapDataDestinationList.EndMapping(InitContext initContext)
		{
			AddObject();

			ISupportMapping sm = _dic as ISupportMapping;

			if (sm != null)
				sm.EndMapping(initContext);
		}

		#endregion
	}
}
