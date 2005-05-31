/*
 * File:    MapDefaultValueAttribute.cs
 * Created: 10/13/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// Is applied to any members that should be mapped to recordset field. 
	/// </summary>
	public class MapDefaultValueAttribute : MapValueAttribute
	{
		/// <summary>
		/// 
		/// </summary>
		public MapDefaultValueAttribute() 
			: base(null, null)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeValue"></param>
		public MapDefaultValueAttribute(object typeValue) 
			: base(typeValue, null)
		{
		}
	}
}
