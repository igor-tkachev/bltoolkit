/*
 * File:    IMapSettable.cs
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
	[Serializable]
	public class MapPropertyInfo
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyType"></param>
		/// <param name="propertyName"></param>
		public MapPropertyInfo(Type propertyType, string propertyName)
		{
			_propertyType = propertyType;
			_propertyName = propertyName;
		}

		private Type _propertyType;
		/// <summary>
		/// 
		/// </summary>
		public  Type  PropertyType
		{
			get { return _propertyType; }
		}

		private string _propertyName;
		/// <summary>
		/// 
		/// </summary>
		public  string  PropertyName
		{
			get { return _propertyName; }
		}
	}
}
