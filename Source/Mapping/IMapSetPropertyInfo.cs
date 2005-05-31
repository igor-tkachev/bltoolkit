/*
 * File:    IMapSetPropertyInfo.cs
 * Created: 04/07/2004
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public interface IMapSetPropertyInfo
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="parent"></param>
		void SetInfo(MapPropertyInfo info, object parent);
	}
}
