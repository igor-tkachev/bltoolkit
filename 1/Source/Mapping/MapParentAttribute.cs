/*
 * File:    MapParentAttribute.cs
 * Created: 8/11/2005
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class MapParentAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		public MapParentAttribute()
		{
		}
	}
}
