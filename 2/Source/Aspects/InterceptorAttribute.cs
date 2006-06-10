using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	[AttributeUsage(
		AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method,
		AllowMultiple=true)]
	public class InterceptorAttribute : AbstractTypeBuilderAttribute
	{
		public InterceptorAttribute(Type interceptorType, InterceptType interceptType)
		{
			if (interceptorType == null) throw new ArgumentNullException("interceptorType");

			_interceptorType = interceptorType;
			_interceptType   = interceptType;
		}

		private readonly Type _interceptorType;
		public           Type  InterceptorType
		{
			get { return _interceptorType; }
		}

		private readonly InterceptType _interceptType;
		public           InterceptType  InterceptType
		{
			get { return _interceptType; }
		}

		public override IAbstractTypeBuilder TypeBuilder
		{
			get { return new InterceptorAspectBuilder(InterceptorType, InterceptType); }
		}
	}
}
