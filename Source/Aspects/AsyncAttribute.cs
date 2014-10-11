using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	/// <summary>
	/// http://www.bltoolkit.net/Doc/Aspects/index.htm
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class AsyncAttribute : AbstractTypeBuilderAttribute
	{
		private readonly string _targetMethodName;
		private readonly Type[] _parameterTypes;

		public AsyncAttribute()
		{
		}

		public AsyncAttribute(string targetMethodName): this(targetMethodName, null)
		{
		}

		public AsyncAttribute(params Type[] parameterTypes): this(null, parameterTypes)
		{
		}

		public AsyncAttribute(string targetMethodName, params Type[] parameterTypes)
		{
			_targetMethodName = targetMethodName;
			_parameterTypes   = parameterTypes;
		}

		public override IAbstractTypeBuilder TypeBuilder
		{
			get { return new Builders.AsyncAspectBuilder(_targetMethodName, _parameterTypes); }
		}
	}
}
