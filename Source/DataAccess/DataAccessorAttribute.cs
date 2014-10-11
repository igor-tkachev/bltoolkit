using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Class)]
	class DataAccessorAttribute : AbstractTypeBuilderAttribute
	{
		public override IAbstractTypeBuilder TypeBuilder
		{
			get { return new DataAccessorBuilder(); }
		}
	}
}
