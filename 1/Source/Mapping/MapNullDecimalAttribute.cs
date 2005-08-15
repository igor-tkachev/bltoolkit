/*
 * File:    MapNullDecimalAttribute.cs
 * Created: 04/20/2005
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public class MapNullDecimalAttribute : MapNullValueAttribute
	{
		/// <summary>
		/// 
		/// </summary>
		public MapNullDecimalAttribute() :base(0m)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nullValue"></param>
		public MapNullDecimalAttribute(decimal nullValue) :base(nullValue)
		{
		}
	}
}
