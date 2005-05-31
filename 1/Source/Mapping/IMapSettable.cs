/*
 * File:    IMapSettable.cs
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
	/// <include file="Examples.xml" path='examples/mapsettable[@name="SetField(string,object)"]/*' />
	public interface IMapSettable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <include file="Examples.xml" path='examples/mapsettable[@name="SetField(string,object)"]/*' />
		bool SetField(string fieldName, object value);
	}
}
