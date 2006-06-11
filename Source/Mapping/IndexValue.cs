using System;

using BLToolkit.Common;

namespace BLToolkit.Mapping
{
	[Obsolete("Use CompoundValue instead.")]
	public class IndexValue : CompoundValue
	{
		public IndexValue(params object[] values)
			: base(values)
		{
		}
	}
}
