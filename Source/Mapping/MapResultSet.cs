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

		private  object[] _parameters;
		internal object[]  Parameters
		{
			get { return _parameters; }
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
		/// <param name="masterIndex"></param>
		/// <param name="slaveResultSet"></param>
		/// <param name="slaveIndex"></param>
		/// <param name="containerName"></param>
		public void AddRelation(
			MapIndex     masterIndex,
			MapResultSet slaveResultSet,
			MapIndex     slaveIndex,
			string       containerName)
		{
			if (_relationList == null)
				_relationList = new ArrayList();

			_relationList.Add(new MapRelation(this, masterIndex, slaveResultSet, slaveIndex, containerName));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="masterIndex"></param>
		/// <param name="slaveResultSet"></param>
		/// <param name="slaveIndex"></param>
		/// <param name="containerName"></param>
		public void AddRelation(
			string       masterIndex,
			MapResultSet slaveResultSet,
			string       slaveIndex,
			string       containerName)
		{
			AddRelation(new MapIndex(masterIndex), slaveResultSet, new MapIndex(slaveIndex), containerName);
		}
	}
}
