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
		/// <param name="masterResultSet"></param>
		/// <param name="masterIndex"></param>
		/// <param name="slaveResultSet"></param>
		/// <param name="slaveIndex"></param>
		public MapRelation(
			MapResultSet masterResultSet, MapIndex masterIndex,
			MapResultSet  slaveResultSet, MapIndex  slaveIndex)
		{
			if (masterIndex.Fields.Length == 0)
				throw new RsdnMapException("Master index length can not be 0.");

			if ( slaveIndex.Fields.Length == 0)
				throw new RsdnMapException("Slave index length can not be 0.");

			if (masterIndex.Fields.Length != slaveIndex.Fields.Length)
				throw new RsdnMapException("Master and slave indexes do not match.");

			_masterResultSet = masterResultSet;
			_slaveResultSet  = slaveResultSet;
			_masterIndex     = masterIndex;
			_slaveIndex      = slaveIndex;
		}

		private MapResultSet _masterResultSet;
		/// <summary>
		/// 
		/// </summary>
		public  MapResultSet  MasterResultSet
		{
			get { return _masterResultSet; }
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
	}
}
