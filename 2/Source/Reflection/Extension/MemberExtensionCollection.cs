using System;
using System.Collections;

namespace BLToolkit.Reflection.Extension
{
	public class MemberExtensionCollection : ICollection
	{
		public MemberExtension this[string memberName]
		{
			get
			{
				if (_isNull)
					return MemberExtension.Null;

				MemberExtension value = (MemberExtension)_members[memberName];
				return value != null? value: MemberExtension.Null;
			}
		}

		public void Add(MemberExtension memberInfo)
		{
			if (!_isNull)
				_members[memberInfo.Name] = memberInfo;
		}

		private  Hashtable _members = new Hashtable();
		internal bool      _isNull;

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			_members.CopyTo(array, index);
		}

		public int Count
		{
			get { return _members.Count; }
		}

		public bool IsSynchronized
		{
			get { return _members.IsSynchronized; ; }
		}

		public object SyncRoot
		{
			get { return _members.SyncRoot;; }
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _members.Values.GetEnumerator();
		}

		#endregion
	}
}
