/*
 * File:    MapIndex.cs
 * Created: 08/16/2005
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public class MapIndex
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fields"></param>
		public MapIndex(params string[] fields)
		{
			_fields = fields;
		}

		private string[] _fields;
		/// <summary>
		/// 
		/// </summary>
		public  string[]  Fields
		{
			get { return _fields; }
		}
	}
}
