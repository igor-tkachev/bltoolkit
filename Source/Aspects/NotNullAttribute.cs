using System;
using System.Diagnostics.CodeAnalysis;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	[SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class NotNullAttribute : AbstractTypeBuilderAttribute
	{
		public NotNullAttribute()
		{
		}

		public NotNullAttribute(string message)
		{
			_message = message;
		}

		private string _message;
		public  string  Message
		{
			get { return _message;  }
			set { _message = value; }
		}

		public override IAbstractTypeBuilder TypeBuilder
		{
			get { return new NotNullAspectBuilder(_message); }
		}
	}
}
