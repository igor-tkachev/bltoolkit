using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	/// <summary>
	/// http://www.bltoolkit.net/Doc/Aspects/index.htm
	/// </summary>
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
			get { return new Builders.NotNullAspectBuilder(_message); }
		}
	}
}
