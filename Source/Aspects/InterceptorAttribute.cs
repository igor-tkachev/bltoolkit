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
			: this(interceptorType, interceptType, null, TypeBuilderConsts.Priority.Normal)
		{
		}

		public InterceptorAttribute(Type interceptorType, InterceptType interceptType, int priority)
			: this(interceptorType, interceptType, null, priority)
		{
		}

		public InterceptorAttribute(Type interceptorType, InterceptType interceptType, string parameters)
			: this(interceptorType, interceptType, parameters, TypeBuilderConsts.Priority.Normal)
		{
		}

		public InterceptorAttribute(
			Type interceptorType, InterceptType interceptType, string configString, int priority)
		{
			if (interceptorType == null && interceptType != 0)
				throw new ArgumentNullException("interceptorType");

			_interceptorType = interceptorType;
			_interceptType   = interceptType;
			_configString    = configString;
			_priority        = priority;
		}

		private readonly Type _interceptorType;
		public  virtual  Type  InterceptorType
		{
			get { return _interceptorType; }
		}

		private readonly InterceptType _interceptType;
		public  virtual  InterceptType  InterceptType
		{
			get { return _interceptType; }
		}

		private readonly int _priority;
		public  virtual  int  Priority
		{
			get { return _priority; }
		}

		private readonly string _configString;
		public  virtual  string  ConfigString
		{
			get { return _configString; }
		}

		public override IAbstractTypeBuilder TypeBuilder
		{
			get
			{
				 return new Builders.InterceptorAspectBuilder(
					 InterceptorType, InterceptType, ConfigString, Priority);
			}
		}
	}
}
