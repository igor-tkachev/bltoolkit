/*
 * File:    IMapDataReceiver.cs
 * Created: 07/27/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public interface IMapDataReceiver
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		int  GetOrdinal   (string name);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <param name="name"></param>
		/// <param name="entity"></param>
		/// <param name="value"></param>
		void SetFieldValue(int i, string name, object entity, object value);
	}
}
