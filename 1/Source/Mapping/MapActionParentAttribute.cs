/*
 * File:    MapActionParentAttribute.cs
 * Created: 8/11/2003
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
	public sealed class MapActionParentAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		public MapActionParentAttribute()
		{
		}
	}
}
