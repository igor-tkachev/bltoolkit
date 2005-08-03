/*
 * File:    DataRowReader.cs
 * Created: 08/03/2005
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Collections;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public class DictionaryReader : IMapDataSource, IMapDataReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dictionary"></param>
		public DictionaryReader(IDictionary dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException("dictionary");

			_dictionary = dictionary;
		}

		private IDictionary _dictionary;
		/// <summary>
		/// 
		/// </summary>
		public  IDictionary  Dictionary
		{
			get { return _dictionary; }
		}

		#region IMapDataReceiver Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int GetOrdinal(string name)
		{
			return 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <param name="name"></param>
		/// <param name="entity"></param>
		/// <param name="value"></param>
		public void SetFieldValue(int i, string name, object entity, object value)
		{
			_dictionary[name] = value;
		}

		#endregion

		#region IMapDataSource Members

		/// <summary>
		/// 
		/// </summary>
		public int FieldCount
		{
			get { return _dictionary.Count; }
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public string GetFieldName(int i)
		{
			SetEnumerator(i);
			return _enumerator.Key.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		public object GetFieldValue(int i, object entity)
		{
			SetEnumerator(i);
			return _enumerator.Value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		public object GetFieldValue(string name, object entity)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
}
}
