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
			_mapper       = objectMapper;
			_fromSource   = keyFieldName[0] == '@';
			_keyFieldName = _fromSource? keyFieldName.Substring(1): keyFieldName;
		}

		private string       _keyFieldName;
		private IDictionary  _dic;
		private ObjectMapper _mapper;
		private object       _newObject;
		private bool         _fromSource;
		private object       _keyValue;

		#region IMapDataDestinationList Members

		private void AddObject()
		{
			if (_newObject != null)
			{
				if (!_fromSource)
					_keyValue = _mapper.TypeAccessor[_keyFieldName].GetValue(_newObject);

				_dic[_keyValue]  = _newObject;
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

		static char[] _trim = new char[] { ' ' };

		public virtual object GetNextObject(InitContext initContext)
		{
			AddObject();

			if (_fromSource)
			{
				_keyValue = initContext.DataSource.GetValue(initContext.SourceObject, _keyFieldName);

				if (_keyValue is string)
					_keyValue = _keyValue.ToString().TrimEnd(_trim);
			}

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
