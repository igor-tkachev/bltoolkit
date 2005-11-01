using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	public class NotNullAttribute : AbstractTypeBuilderAttribute
	{
		public NotNullAttribute()
		{
		}

		public override IAbstractTypeBuilder TypeBuilder
		{
			get { return new NotNullAspectBuilder(); }
		}
	}
}
