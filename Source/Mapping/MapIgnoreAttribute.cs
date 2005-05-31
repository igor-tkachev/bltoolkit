/*
 * File:    MapIgnoreAttribute.cs
 * Created: 11/23/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class MapIgnoreAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		public MapIgnoreAttribute()
		{
		}
	}
}
