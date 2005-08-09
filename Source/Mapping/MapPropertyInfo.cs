/*
 * File:    IMapSettable.cs
 * Created: 04/07/2004
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Reflection;

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
		/// <param name="propertyInfo"></param>
		public MapPropertyInfo(PropertyInfo propertyInfo)
		{
			_propertyInfo = propertyInfo;
			_propertyType = propertyInfo.PropertyType;
			_propertyName = propertyInfo.Name;
		}

		private PropertyInfo _propertyInfo;
		/// <summary>
		/// 
		/// </summary>
		public  PropertyInfo  PropertyInfo
		{
			get { return _propertyInfo; }
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
