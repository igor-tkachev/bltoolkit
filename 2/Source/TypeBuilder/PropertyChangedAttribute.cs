using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Interface)]
	sealed class PropertyChangedAttribute : AbstractTypeBuilderAttribute
	{
		public override IAbstractTypeBuilder TypeBuilder
		{
			get { return new PropertyChangedBuilder(); }
		}
	}
}
