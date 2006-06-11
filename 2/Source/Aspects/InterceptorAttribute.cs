using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	[AttributeUsage(
		AttributeTargets.Class |
		AttributeTargets.Interface |
		AttributeTargets.Property |
		AttributeTargets.Method,
		AllowMultiple=true)]
	public class InterceptorAttribute : AbstractTypeBuilderAttribute
	{
		public InterceptorAttribute(Type interceptorType, InterceptType interceptType)
			: this(interceptorType, interceptType, null, TypeBuilderConsts.NormalBuilderPriority)
		{
		}

		public InterceptorAttribute(Type interceptorType, InterceptType interceptType, int priority)
			: this(interceptorType, interceptType, null, priority)
		{
		}

		public InterceptorAttribute(Type interceptorType, InterceptType interceptType, string parameters)
			: this(interceptorType, interceptType, parameters, TypeBuilderConsts.NormalBuilderPriority)
		{
		}

		public InterceptorAttribute(
			Type interceptorType, InterceptType interceptType, string parameters, int priority)
		{
			if (interceptorType == null) throw new ArgumentNullException("interceptorType");

			_interceptorType = interceptorType;
			_interceptType   = interceptType;
			_parameters      = parameters;
			_priority        = priority;
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

		private readonly int _priority;
		public           int  Priority
		{
			get { return _priority; }
		}

		private readonly string _parameters;
		public           string  Parameters
		{
			get { return _parameters; }
		}

		public override IAbstractTypeBuilder TypeBuilder
		{
			get
			{
				 return new InterceptorAspectBuilder(
					 InterceptorType, InterceptType, Parameters, Priority);
			}
		}
	}
}
