/*
 * File:    MapRelation.cs
 * Created: 08/16/2005
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public class MapRelation
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="slaveResultSet"></param>
		/// <param name="slaveIndex"></param>
		/// <param name="masterIndex"></param>
		/// <param name="containerName"></param>
		public MapRelation(
			MapResultSet slaveResultSet,
			MapIndex     slaveIndex,
			MapIndex     masterIndex,
			string       containerName)
		{
			if (masterIndex.Fields.Length == 0)
				throw new RsdnMapException("Master index length can not be 0.");

			if ( slaveIndex.Fields.Length == 0)
				throw new RsdnMapException("Slave index length can not be 0.");

			if (masterIndex.Fields.Length != slaveIndex.Fields.Length)
				throw new RsdnMapException("Master and slave indexes do not match.");

			if (containerName == null || containerName.Length == 0)
				throw new RsdnMapException("Master container field name is wrong.");

			_slaveResultSet  = slaveResultSet;
			_masterIndex     = masterIndex;
			_slaveIndex      = slaveIndex;
			_containerName   = containerName;
		}

		private MapIndex _masterIndex;
		/// <summary>
		/// 
		/// </summary>
		public  MapIndex  MasterIndex
		{
			get { return _masterIndex; }
		}

		private MapResultSet _slaveResultSet;
		/// <summary>
		/// 
		/// </summary>
		public  MapResultSet  SlaveResultSet
		{
			get { return _slaveResultSet; }
		}

		private MapIndex _slaveIndex;
		/// <summary>
		/// 
		/// </summary>
		public  MapIndex  SlaveIndex
		{
			get { return _slaveIndex; }
		}

		private string _containerName;
		/// <summary>
		/// 
		/// </summary>
		public  string  ContainerName
		{
			get { return _containerName; }
		}
	}
}
