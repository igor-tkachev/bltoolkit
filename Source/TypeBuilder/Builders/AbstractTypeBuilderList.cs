using System.Collections.Generic;

namespace BLToolkit.TypeBuilder.Builders
{
	public class AbstractTypeBuilderList : List<IAbstractTypeBuilder>
	{
		public AbstractTypeBuilderList()
		{
		}

		public AbstractTypeBuilderList(int capacity)
			: base(capacity)
		{
		}
	}
}
