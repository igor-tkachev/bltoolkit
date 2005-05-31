/*
 * File:    IMapObjectFactory.cs
 * Created: 11/21/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public interface IMapObjectFactory
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		object CreateInstance(MapInitializingData data);
	}
}
