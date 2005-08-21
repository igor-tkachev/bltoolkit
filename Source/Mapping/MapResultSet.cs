/*
 * File:    MapResultSet.cs
 * Created: 08/16/2005
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Collections;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public class MapResultSet
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="objectType"></param>
		public MapResultSet(Type objectType)
		{
			_objectType = objectType;
			_descriptor = Map.Descriptor(objectType);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objectType"></param>
		/// <param name="parameters"></param>
		public MapResultSet(Type objectType, object[] parameters)
		{
			_objectType = objectType;
			_parameters = parameters;
		}

		internal MapResultSet(MapResultSet resultSet)
		{
			_objectType = resultSet._objectType;
			_parameters = resultSet._parameters;
			_descriptor = resultSet._descriptor;

			if (resultSet._relationList != null)
			{
				_relationList = new ArrayList(resultSet._relationList.Count);
				_relationList.AddRange(resultSet._relationList);
			}
		}

		private  Type _objectType;
		internal Type  ObjectType
		{
			get { return _objectType; }
		}

		private  MapDescriptor _descriptor;
		internal MapDescriptor  Descriptor
		{
			get { return _descriptor;  }
		}

		private object[] _parameters;
		/// <summary>
		/// 
		/// </summary>
		public  object[]  Parameters
		{
			get { return _parameters;  }
			set { _parameters = value; }
		}

		private IList _list;
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		internal string  IndexID
		{
			get { return _indexID;  }
			set { _indexID = value; }
		}

		private  Hashtable _hashtable;
		/// <summary>
		/// 
		/// </summary>
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
					_relations = (MapRelation[])_relationList.ToArray(typeof(MapRelation));

				return _relations;
			}

			set { _relations = value; }
		}

		private ArrayList _relationList;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="slaveResultSet"></param>
		/// <param name="slaveIndex"></param>
		/// <param name="masterIndex"></param>
		/// <param name="containerName"></param>
		public void AddRelation(
			MapResultSet slaveResultSet,
			MapIndex     slaveIndex,
			MapIndex     masterIndex,
			string       containerName)
		{
			if (_relationList == null)
				_relationList = new ArrayList();

			_relationList.Add(new MapRelation(slaveResultSet, slaveIndex, masterIndex, containerName));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="slaveResultSet"></param>
		/// <param name="slaveIndex"></param>
		/// <param name="masterIndex"></param>
		/// <param name="containerName"></param>
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
