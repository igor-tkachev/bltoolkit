using System;

using JNotNull = JetBrains.Annotations.NotNullAttribute;

namespace BLToolkit.Mapping
{
	using Common;
	using Reflection;

	public class Association
	{
		public Association(
			[JNotNull] MemberAccessor memberAccessor,
			[JNotNull] string[]       thisKey,
			[JNotNull] string[]       otherKey,
			           string         storage,
			           bool           canBeNull)
		{
			if (memberAccessor == null) throw new ArgumentNullException("memberAccessor");
			if (thisKey        == null) throw new ArgumentNullException("thisKey");
			if (otherKey       == null) throw new ArgumentNullException("otherKey");

			if (thisKey.Length == 0)
				throw new ArgumentOutOfRangeException(
					"thisKey",
					string.Format("Association '{0}.{1}' does not define keys.", memberAccessor.TypeAccessor.Type.Name, memberAccessor.Name));

			if (thisKey.Length != otherKey.Length)
				throw new ArgumentException(
					string.Format(
						"Association '{0}.{1}' has different number of keys for parent and child objects.",
						memberAccessor.TypeAccessor.Type.Name, memberAccessor.Name));

			_memberAccessor = memberAccessor;
			_thisKey        = thisKey;
			_otherKey       = otherKey;
			_storage        = storage;
			_canBeNull      = canBeNull;
		}

		private MemberAccessor _memberAccessor;
		public  MemberAccessor  MemberAccessor { get { return _memberAccessor; } set { _memberAccessor = value; } }

		private string[] _thisKey;
		public  string[]  ThisKey { get { return _thisKey; } set { _thisKey = value; } }

		private string[] _otherKey;
		public  string[]  OtherKey { get { return _otherKey; } set { _otherKey = value; } }

		private string _storage;
		public  string  Storage { get { return _storage; } set { _storage = value; } }

		private bool _canBeNull;
		public  bool  CanBeNull { get { return _canBeNull; } set { _canBeNull = value; } }

		public static string[] ParseKeys(string keys)
		{
			return keys == null ? Array<string>.Empty : keys.Replace(" ", "").Split(',');
		}
	}
}
