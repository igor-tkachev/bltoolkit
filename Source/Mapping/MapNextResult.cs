using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public class MapNextResult
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="slaveIndex"></param>
		/// <param name="masterIndex"></param>
		/// <param name="containerName"></param>
		/// <param name="nextResults"></param>
		public MapNextResult(
			Type     type,
			MapIndex slaveIndex,
			MapIndex masterIndex,
			string   containerName,
			params MapNextResult[] nextResults)
		{
			_objectType    = type;
			_slaveIndex    = slaveIndex;
			_masterIndex   = masterIndex;
			_containerName = containerName;
			_nextResults   = nextResults;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="slaveIndex"></param>
		/// <param name="masterIndex"></param>
		/// <param name="containerName"></param>
		/// <param name="nextResults"></param>
		public MapNextResult(
			Type   type,
			string slaveIndex,
			string masterIndex,
			string containerName,
			params MapNextResult[] nextResults)
			: this(type, new MapIndex(slaveIndex), new MapIndex(masterIndex), containerName, nextResults)
		{
		}

		private  Type _objectType;
		internal Type  ObjectType
		{
			get { return _objectType;  }
		}

		private  MapIndex _slaveIndex;
		internal MapIndex  SlaveIndex
		{
			get { return _slaveIndex;  }
		}

		private  MapIndex _masterIndex;
		internal MapIndex  MasterIndex
		{
			get { return _masterIndex;  }
		}

		private  string _containerName;
		internal string  ContainerName
		{
			get { return _containerName;  }
		}

		private  MapNextResult[] _nextResults;
		internal MapNextResult[]  NextResults
		{
			get { return _nextResults; }
		}
	}
}
