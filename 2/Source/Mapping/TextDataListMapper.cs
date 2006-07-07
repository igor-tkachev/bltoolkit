using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class TextDataListMapper : IMapDataDestinationList
	{
		public TextDataListMapper(TextDataMapper mapper)
		{
			_mapper = mapper;
		}

		public TextDataListMapper(TextDataWriter writer)
			: this(new TextDataMapper(writer))
		{
		}

		private TextDataMapper _mapper;

		#region IMapDataDestinationList Members

		void IMapDataDestinationList.InitMapping(InitContext initContext)
		{
		}

		IMapDataDestination IMapDataDestinationList.GetDataDestination(InitContext initContext)
		{
			return _mapper;
		}

		object IMapDataDestinationList.GetNextObject(BLToolkit.Reflection.InitContext initContext)
		{
			return _mapper.Writer;
		}

		void IMapDataDestinationList.EndMapping(InitContext initContext)
		{
			_mapper.WriteEnd();
		}

		#endregion
	}
}
