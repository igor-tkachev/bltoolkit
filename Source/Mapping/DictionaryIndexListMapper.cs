using System;
using System.Collections;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class DictionaryIndexListMapper : IMapDataDestinationList
	{
		public DictionaryIndexListMapper(IDictionary dic, MapIndex index, ObjectMapper objectMapper)
		{
			_dic    = dic;
			_mapper = objectMapper;

			_fields     = new string[index.Fields.Length];
			_fromSource = new bool  [index.Fields.Length];

			for (int i = 0; i < _fields.Length; i++)
			{
				bool fromSource = index.Fields[i][0] == '@';

				_fields    [i] = fromSource? index.Fields[i].Substring(1): index.Fields[i];
				_fromSource[i] = fromSource;
				_isFromSource  = _isFromSource ||  fromSource;
				_isFromDest    = _isFromDest   || !fromSource;
			}
		}

		private string[]     _fields;
		private IDictionary  _dic;
		private ObjectMapper _mapper;
		private object       _newObject;
		private bool[]       _fromSource;
		private bool         _isFromSource;
		private bool         _isFromDest;
		private object[]     _indexValue;

		#region IMapDataDestinationList Members

		private void AddObject()
		{
			if (_newObject != null)
			{
				if (_isFromDest)
					for (int i = 0; i < _fields.Length; i++)
						if (!_fromSource[i])
							_indexValue[i] = _mapper.TypeAccessor[_fields[i]].GetValue(_newObject);

				_dic[new IndexValue(_indexValue)] = _newObject;
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

			_indexValue = new object[_fields.Length];

			if (_isFromSource)
				for (int i = 0; i < _fields.Length; i++)
					if (_fromSource[i])
						_indexValue[i] = initContext.DataSource.GetValue(
							initContext.SourceObject, _fields[i]);

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
