using System;
using BLToolkit.TypeBuilder.Builders;

namespace Demo.WebServices.Client.WebClient
{
	[AttributeUsage(AttributeTargets.Class)]
	public class WebClientAttribute : AbstractTypeBuilderAttribute
	{
		public override IAbstractTypeBuilder TypeBuilder
		{
			get { return new WebClientTypeBuilder(); }
		}
	}
}