using System;
using System.Collections;

namespace BLToolkit.TypeInfo
{
	public class TypeInfoCollection : ICollection
	{
		#region TypeInfo
		
		private Hashtable _types = new Hashtable();

		public TypeInfo this[string typeName]
		{
			get
			{
				TypeInfo value = (TypeInfo)_types[typeName];
				return value != null? value: TypeInfo.Null;
			}
		}

		public void Add(TypeInfo typeInfo)
		{
			_types[typeInfo.Name] = typeInfo;
		}

		#endregion

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			_types.CopyTo(array, index);
		}

		public int Count
		{
			get { return _types.Count; }
		}

		public bool IsSynchronized
		{
			get { return _types.IsSynchronized; ; }
		}

		public object SyncRoot
		{
			get { return _types.SyncRoot;; }
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _types.Values.GetEnumerator();
		}

		#endregion
	}
}
