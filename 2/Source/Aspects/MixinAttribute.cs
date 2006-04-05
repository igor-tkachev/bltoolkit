using System;

using BLToolkit.TypeBuilder.Builders;

namespace BLToolkit.Aspects
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public sealed class MixinAttribute : AbstractTypeBuilderAttribute
	{
		public MixinAttribute(Type targetInterface, string memberName)
		{
			_targetInterface = targetInterface;
			_memberName      = memberName;
		}

		private Type _targetInterface;
		public  Type  TargetInterface
		{
			get { return _targetInterface;  }
		}

		private string _memberName;
		public  string  MemberName
		{
			get { return _memberName;  }
		}

		public override IAbstractTypeBuilder TypeBuilder
		{
			get { return new MixinAspectBuilder(_targetInterface, _memberName); }
		}
	}
}
