using System;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.Validation
{
	public interface IValidatable
	{
		void Validate([MapPropertyInfo] MapPropertyInfo info);
	}
}
