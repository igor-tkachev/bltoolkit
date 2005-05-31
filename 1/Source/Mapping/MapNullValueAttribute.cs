/*
 * File:    MapNullValueAttribute.cs
 * Created: 10/13/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// Is applied to any members that should be mapped to recordset field. 
	/// </summary>
	//[AttributeUsage(
	//	AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Class,
	//	AllowMultiple = true
	//)]
	public class MapNullValueAttribute : MapValueAttribute
	{
		/// <summary>
		/// 
		/// </summary>
		public MapNullValueAttribute() 
			: base(null)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeValue"></param>
		public MapNullValueAttribute(object typeValue) 
			: base(typeValue, typeof(DBNull))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeValue"></param>
		/// <param name="mappedValue"></param>
		public MapNullValueAttribute(object typeValue, Type mappedValue) 
			: base(typeValue, mappedValue)
		{
		}
	}
}
