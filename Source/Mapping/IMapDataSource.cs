/*
 * File:    IMapDataSource.cs
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
	public interface IMapDataSource
	{
		/// <summary>
		/// 
		/// </summary>
		int FieldCount { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		string GetFieldName (int i);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		object GetFieldValue(int i, object entity);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="entity"></param>
		/// <returns></returns>
		object GetFieldValue(string name, object entity);
	}
}
