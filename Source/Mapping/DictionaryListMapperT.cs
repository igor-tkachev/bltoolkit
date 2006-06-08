using System;
using System.Collections.Generic;

using BLToolkit.Common;
using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class DictionaryListMapper<K,T> : IMapDataDestinationList
	{
		public DictionaryListMapper(
			IDictionary<K,T>     dic,
			NameOrIndexParameter keyField,
			ObjectMapper         objectMapper)
		{
			_dic        = dic;
			_mapper     = objectMapper;
			_fromSource = keyField.ByName && keyField.Name[0] == '@';
			_keyField   = _fromSource ? keyField.Name.Substring(1): keyField;
		}

		private NameOrIndexParameter _keyField;
		private IDictionary<K,T>     _dic;
		private ObjectMapper         _mapper;
		private T                    _newObject;
		private bool                 _fromSource;
		private K                    _keyValue;

		#region IMapDataDestinationList Members

		private void AddObject()
		{
			if (_newObject != null)
			{
				if (!_fromSource)
					_keyValue = (K)_mapper.TypeAccessor[_keyField].GetValue(_newObject);

				_dic[_keyValue] = _newObject;
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

		static char[] _trim = { ' ' };

		public virtual object GetNextObject(InitContext initContext)
		{
			AddObject();

			if (_fromSource)
			{
				_keyValue = (K)(_keyField.ByName ?
					initContext.DataSource.GetValue(initContext.SourceObject, _keyField.Name) :
					initContext.DataSource.GetValue(initContext.SourceObject, _keyField.Index));

				if (_keyValue is string)
					_keyValue = (K)(object)_keyValue.ToString().TrimEnd(_trim);
			}

			return _newObject = (T)_mapper.CreateInstance(initContext);
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
