using System;

namespace BLToolkit.Mapping
{
	[AttributeUsage(
		AttributeTargets.Class    | AttributeTargets.Interface | 
		AttributeTargets.Property | AttributeTargets.Field,
		AllowMultiple=true)]
	public sealed class MemberMapperAttribute : Attribute
	{
		public MemberMapperAttribute(Type memberMapperType)
			: this(null, memberMapperType)
		{
		}

		public MemberMapperAttribute(Type memberType, Type memberMapperType)
		{
			if (memberMapperType == null) throw new ArgumentNullException("memberMapperType");

			_memberType   = memberType;
			_memberMapper = Activator.CreateInstance(memberMapperType) as MemberMapper;

			if (_memberMapper == null)
				throw new ArgumentException(
					string.Format("Type '{0}' is not MemberMapper.", memberMapperType));
		}

		private Type _memberType;
		public  Type  MemberType
		{
			get { return _memberType; }
		}

		private MemberMapper _memberMapper;
		public  MemberMapper  MemberMapper
		{
			get { return _memberMapper; }
		}
	}
}
