using System;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.Validation
{
	[Obsolete]
	public interface IValidatable
	{
		void Validate([MapPropertyInfo] MapPropertyInfo info);
	}
}
