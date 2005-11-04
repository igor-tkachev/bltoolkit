using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	public class NotNullAttribute : AbstractTypeBuilderAttribute
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
