using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class SimpleDestinationListMapper : IMapDataDestinationList
	{
		[CLSCompliant(false)]
		public SimpleDestinationListMapper(IMapDataDestination mapper)
		{
			_mapper = mapper;
		}

		private readonly IMapDataDestination _mapper;

		#region IMapDataDestinationList Members

		public virtual void InitMapping(InitContext initContext)
		{
		}

		[CLSCompliant(false)]
		public virtual IMapDataDestination GetDataDestination(InitContext initContext)
		{
			return _mapper;
		}

		public virtual object GetNextObject(InitContext initContext)
		{
			return _mapper;
		}

		public virtual void EndMapping(InitContext initContext)
		{
		}

		#endregion
	}
}
