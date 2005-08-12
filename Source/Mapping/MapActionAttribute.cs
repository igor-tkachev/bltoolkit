/*
 * File:    MapActionAttribute.cs
 * Created: 8/10/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
	public class MapActionAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		public MapActionAttribute(Type type)
		{
			_type = type;
		}

		private Type _type;
		/// <summary>
		/// 
		/// </summary>
		public  Type  Type
		{
			get { return _type;  }
			set { _type = value; }
		}
	}
}
