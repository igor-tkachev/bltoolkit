using System;
using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	/// <summary>
	/// http://www.bltoolkit.net/Doc/Aspects/index.htm
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class OverloadAttribute : AbstractTypeBuilderAttribute
	{
		private readonly string _overloadedMethodName;
		private readonly Type[] _parameterTypes;

		public OverloadAttribute()
		{
		}

		public OverloadAttribute(string overloadedMethodName): this(overloadedMethodName, null)
		{
		}

		public OverloadAttribute(params Type[] parameterTypes): this(null, parameterTypes)
		{
		}

		public OverloadAttribute(string overloadedMethodName, params Type[] parameterTypes)
		{
			_overloadedMethodName = overloadedMethodName;
			_parameterTypes       = parameterTypes;
		}

		public override IAbstractTypeBuilder TypeBuilder
		{
			get { return new Builders.OverloadAspectBuilder(_overloadedMethodName, _parameterTypes); }
		}
	}
}