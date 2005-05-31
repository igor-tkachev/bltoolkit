/*
 * File:    MapNullGuidAttribute.cs
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
	public class MapNullGuidAttribute : MapNullValueAttribute
	{
		/// <summary>
		/// 
		/// </summary>
		public MapNullGuidAttribute() 
			: base(Guid.Empty)
		{
		}
	}
}
