using System;
using System.Collections;

namespace BLToolkit.Mapping
{
	public class DictionaryMapper : IMapDataSource, IMapDataDestination
	{
		public DictionaryMapper(IDictionary dictionary)
		{
			if (dictionary == null) throw new ArgumentNullException("dictionary");

			_dictionary = dictionary;
		}

		private IDictionary _dictionary;
		public  IDictionary  Dictionary
		{
			get { return _dictionary; }
		}

		private int                   _currentIndex;
		private IDictionaryEnumerator _enumerator;

		private void SetEnumerator(int i)
		{
			if (_enumerator == null)
			{
				_enumerator = _dictionary.GetEnumerator();
				_enumerator.MoveNext();
			}

			if (_currentIndex > i)
			{
				_currentIndex = 0;
				_enumerator.Reset();
			}

			for (; _currentIndex < i; _currentIndex++)
				_enumerator.MoveNext();
		}

		#region IMapDataSource Members

		public int Count
		{
			get { return _dictionary.Count; }
		}

		public string GetName(int index)
		{
			SetEnumerator(index);
			return _enumerator.Key.ToString();
		}

		public object GetValue(object o, int index)
		{
			SetEnumerator(index);
			return _enumerator.Value;
		}

		public object GetValue(object o, string name)
		{
			return _dictionary[name];
		}

		#endregion

		#region IMapDataDestination Members

		private ArrayList _nameList;

		public int GetOrdinal(string name)
		{
			if (_nameList == null)
				_nameList = new ArrayList();

			int idx = _nameList.IndexOf(name);

			return idx >= 0? idx: _nameList.Add(name);
		}

		public void SetValue(object o, int index, object value)
		{
			_dictionary[_nameList[index]] = value;
		}

		public void SetValue(object o, string name, object value)
		{
			_dictionary[name] = value;
		}

		#endregion
	}
}
