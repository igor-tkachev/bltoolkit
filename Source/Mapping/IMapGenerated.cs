/*
 * File:    IMapEnumerable.cs
 * Created: 11/11/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	public interface IMapGenerated
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <include file="Examples.xml" path='examples/maptype[@name="ctor(Type,object,object)"]/*' />
		object[] GetCreatedMembers();
	}
}
