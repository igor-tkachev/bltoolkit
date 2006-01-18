using System;
using System.Collections;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class DictionaryListMapper : IMapDataDestinationList
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

		public virtual void InitMapping(InitContext initContext)
		{
			ISupportMapping sm = _dic as ISupportMapping;

			if (sm != null)
			{
				sm.BeginMapping(initContext);

				if (_mapper != initContext.ObjectMapper)
					_mapper = initContext.ObjectMapper;
			}
		}

		public virtual IMapDataDestination GetDataDestination(InitContext initContext)
		{
			return _mapper;
		}

		public virtual object GetNextObject(InitContext initContext)
		{
			AddObject();
			return _newObject = _mapper.CreateInstance(initContext);
		}

		public virtual void EndMapping(InitContext initContext)
		{
			AddObject();

			ISupportMapping sm = _dic as ISupportMapping;

			if (sm != null)
				sm.EndMapping(initContext);
		}

		#endregion
	}
}
