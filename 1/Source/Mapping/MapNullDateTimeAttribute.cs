/*
 * File:    MapNullDateTimeAttribute.cs
 * Created: 06/02/2004
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public class MapNullDateTimeAttribute : MapNullValueAttribute
	{
		/// <summary>
		/// 
		/// </summary>
		public MapNullDateTimeAttribute()
			: base(DateTime.MinValue)
		{
		}
	}
}
