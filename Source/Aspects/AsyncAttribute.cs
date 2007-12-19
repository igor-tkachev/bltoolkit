using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class AsyncAttribute : AbstractTypeBuilderAttribute
	{
		private readonly string _targetMethodName;
		private readonly Type[] _parameterTypes;

		public AsyncAttribute()
		{
		}

		public AsyncAttribute(string targetMethodName)
		{
			_targetMethodName = targetMethodName;
		}

		public AsyncAttribute(string targetMethodName, params Type[] parameterTypes)
		{
			_targetMethodName = targetMethodName;
			_parameterTypes = parameterTypes;
		}

		public override IAbstractTypeBuilder TypeBuilder
		{
			get { return new AsyncAspectBuilder(_targetMethodName, _parameterTypes); }
		}
	}
}
