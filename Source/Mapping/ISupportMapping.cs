using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public interface ISupportMapping
	{
		void BeginMapping(InitContext initContext);
		void EndMapping  (InitContext initContext);
	}
}
