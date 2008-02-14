using System;
using System.Collections;

namespace BLToolkit.Reflection.Extension
{
	public class ExtensionList : ICollection
	{
		#region ExtensionList
		
		private readonly Hashtable _types = new Hashtable();

		public TypeExtension this[string typeName]
		{
			get
			{
				TypeExtension value = (TypeExtension)_types[typeName];
				return value ?? TypeExtension.Null;
			}
		}

		public TypeExtension this[Type type]
		{
			get
			{
				foreach (TypeExtension ext in _types.Values)
					if (ext.Name == type.Name || ext.Name == type.FullName)
						return ext;

				return TypeExtension.Null;
			}
		}

		public void Add(TypeExtension typeInfo)
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
			get { return _types.IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return _types.SyncRoot; }
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
