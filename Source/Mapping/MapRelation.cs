namespace BLToolkit.Mapping
{
	public class MapRelation
	{
		public MapRelation(
			MapResultSet slaveResultSet,
			MapIndex     slaveIndex,
			MapIndex     masterIndex,
			string       containerName)
		{
			if (masterIndex.Fields.Length == 0)
				throw new MappingException("Master index length can not be 0.");

			if ( slaveIndex.Fields.Length == 0)
				throw new MappingException("Slave index length can not be 0.");

			if (masterIndex.Fields.Length != slaveIndex.Fields.Length)
				throw new MappingException("Master and slave indexes do not match.");

			if (string.IsNullOrEmpty(containerName))
				throw new MappingException("Master container field name is wrong.");

			_slaveResultSet  = slaveResultSet;
			_masterIndex     = masterIndex;
			_slaveIndex      = slaveIndex;
			_containerName   = containerName;
		}

		private readonly MapIndex _masterIndex;
		public           MapIndex  MasterIndex
		{
			get { return _masterIndex; }
		}

		private readonly MapResultSet _slaveResultSet;
		public           MapResultSet  SlaveResultSet
		{
			get { return _slaveResultSet; }
		}

		private readonly MapIndex _slaveIndex;
		public           MapIndex  SlaveIndex
		{
			get { return _slaveIndex; }
		}

		private readonly string _containerName;
		public           string  ContainerName
		{
			get { return _containerName; }
		}
	}
}
