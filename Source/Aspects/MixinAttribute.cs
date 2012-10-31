using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	/// <summary>
	/// http://www.bltoolkit.net/Doc/Aspects/index.htm
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public sealed class MixinAttribute : AbstractTypeBuilderAttribute
	{
		public MixinAttribute(
			Type targetInterface, string memberName, bool throwExceptionIfNull, string exceptionMessage)
		{
			_targetInterface      = targetInterface;
			_memberName           = memberName;
			_throwExceptionIfNull = throwExceptionIfNull;
			_exceptionMessage     = exceptionMessage;
		}

		public MixinAttribute(Type targetInterface, string memberName, bool throwExceptionIfNull)
			: this(targetInterface, memberName, throwExceptionIfNull, null)
		{
		}

		public MixinAttribute(Type targetInterface, string memberName, string exceptionMessage)
			: this(targetInterface, memberName, true, exceptionMessage)
		{
		}

		public MixinAttribute(Type targetInterface, string memberName)
			: this(targetInterface, memberName, true, null)
		{
		}

		private readonly Type _targetInterface;
		public           Type  TargetInterface
		{
			get { return _targetInterface;  }
		}

		private readonly string _memberName;
		public           string  MemberName
		{
			get { return _memberName;  }
		}

		private bool _throwExceptionIfNull;
		public  bool  ThrowExceptionIfNull
		{
			get { return _throwExceptionIfNull;  }
			set { _throwExceptionIfNull = value; }
		}

		private string _exceptionMessage;
		public  string  ExceptionMessage
		{
			get { return _exceptionMessage;  }
			set { _exceptionMessage = value; }
		}

		public override IAbstractTypeBuilder TypeBuilder
		{
			get
			{
				return new Builders.MixinAspectBuilder(
					_targetInterface, _memberName, _throwExceptionIfNull, _exceptionMessage);
			}
		}
	}
}
