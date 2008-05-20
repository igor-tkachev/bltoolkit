using System;
using System.Collections;
using System.Collections.Generic;

namespace BLToolkit.Mapping
{
	public class MapResultSet
	{
		public MapResultSet(Type objectType)
		{
			_objectType   = objectType;
			_objectMapper = Map.GetObjectMapper(objectType);
		}

		public MapResultSet(Type objectType, IList list)
		{
			_objectType   = objectType;
			_objectMapper = Map.GetObjectMapper(objectType);
			_list         = list;
		}

		public MapResultSet(Type objectType, object[] parameters)
		{
			_objectType = objectType;
			_parameters = parameters;
		}

		public MapResultSet(Type objectType, IList list, object[] parameters)
		{
			_objectType = objectType;
			_parameters = parameters;
			_list         = list;
		}

		internal MapResultSet(MapResultSet resultSet)
		{
			_objectType   = resultSet._objectType;
			_parameters   = resultSet._parameters;
			_objectMapper = resultSet._objectMapper;

			if (resultSet._relationList != null)
			{
				_relationList = new List<MapRelation>(resultSet._relationList.Count);
				_relationList.AddRange(resultSet._relationList);
			}
		}

		private readonly Type _objectType;
		internal         Type  ObjectType
		{
			get { return _objectType; }
		}

		private readonly ObjectMapper _objectMapper;
		internal         ObjectMapper  ObjectMapper
		{
			get { return _objectMapper;  }
		}

		private object[] _parameters;
		public  object[]  Parameters
		{
			get { return _parameters;  }
			set { _parameters = value; }
		}

		private IList _list;
		public  IList  List
		{
			get
			{
				if (_list == null)
					_list = new ArrayList();

				return _list;
			}

			set { _list = value; }
		}

		private  string _indexID;
		internal string  IndexID
		{
			get { return _indexID;  }
			set { _indexID = value; }
		}

		private  Hashtable _hashtable;
		internal Hashtable  Hashtable
		{
			get { return _hashtable;  }
			set { _hashtable = value; }
		}

		private  MapRelation[] _relations;
		internal MapRelation[]  Relations
		{
			get 
			{
				if (_relationList != null && (_relations == null || _relations.Length != _relationList.Count))
					_relations = _relationList.ToArray();

				return _relations;
			}

			set { _relations = value; }
		}

		private List<MapRelation> _relationList;

		public void AddRelation(
			MapResultSet slaveResultSet,
			MapIndex     slaveIndex,
			MapIndex     masterIndex,
			string       containerName)
		{
			if (_relationList == null)
				_relationList = new List<MapRelation>();

			_relationList.Add(new MapRelation(slaveResultSet, slaveIndex, masterIndex, containerName));
		}

		public void AddRelation(
			MapResultSet slaveResultSet,
			string       slaveIndex,
			string       masterIndex,
			string       containerName)
		{
			AddRelation( slaveResultSet, new MapIndex(slaveIndex), new MapIndex(masterIndex),containerName);
		}
	}
}

