using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			_keyGetter  = MapGetData<K>.I;
			_fromSource = keyField.ByName && keyField.Name[0] == '@';

			if (_fromSource)
				_keyField = keyField.Name.Substring(1);
			else
			{
				MemberMapper mm;

				if (keyField.ByName)
				{
					mm = _mapper[keyField.Name, true];
					_keyField = _mapper.IndexOf(mm);
				}
				else
				{
					mm = _mapper[keyField.Index];
					_keyField = keyField;
				}

				_typeMismatch     = !TypeHelper.IsSameOrParent(typeof(K), mm.Type);

				Debug.WriteLineIf(_typeMismatch, string.Format(
					"Member {0} type '{1}' does not match dictionary key type '{2}'.",
						mm.Name, mm.Type.Name, (typeof(K).Name)));
			}
		}

		private NameOrIndexParameter _keyField;
		private IDictionary<K,T>     _dic;
		private ObjectMapper         _mapper;
		private T                    _newObject;
		private bool                 _typeMismatch;
		private bool                 _fromSource;
		private K                    _keyValue;
		private MapGetData<K>.MB<K>  _keyGetter;

		#region IMapDataDestinationList Members

		private void AddObject()
		{
			if (_newObject != null)
			{
				if (_typeMismatch)
					_keyValue = _mapper.MappingSchema.ConvertTo<K, object>(_mapper[_keyField.Index].GetValue(_newObject));
				else if (!_fromSource)
					_keyValue = _keyGetter.From(_mapper, _newObject, _keyField.Index);

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

			if (_fromSource && _keyField.ByName)
				_keyField = initContext.DataSource.GetOrdinal(_keyField.Name);

		}

		[CLSCompliant(false)]
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
				_keyValue = _keyGetter.From(initContext.DataSource, initContext.SourceObject, _keyField.Index);

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
