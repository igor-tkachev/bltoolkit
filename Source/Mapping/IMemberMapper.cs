/*
 * File:    IMemberMapper.cs
 * Created: 07/04/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Reflection;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// Internal class.
	/// </summary>
	public interface IMemberMapper : ICloneable
	{
		/// <summary>
		/// 
		/// </summary>
		string Name         { get; }
		/// <summary>
		/// 
		/// </summary>
		string OriginalName { get; }
		/// <summary>
		/// 
		/// </summary>
		bool   IsNullable   { get; }
		/// <summary>
		/// 
		/// </summary>
		bool   IsTrimmable  { get; }
		/// <summary>
		/// 
		/// </summary>
		bool   IsClass      { get; }
		/// <summary>
		/// 
		/// </summary>
		Type   MemberType   { get; }
		/// <summary>
		/// Gets or sets the member order number.
		/// </summary>
		int    Order        { get; set; }
		/// <summary>
		/// 
		/// </summary>
		Attribute[] MapValueAttributeList { get; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		object GetValue(object obj);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="value"></param>
		void   SetValue(object obj, object value);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributeType"></param>
		/// <param name="inherit"></param>
		/// <returns></returns>
		object[] GetCustomAttributes(Type attributeType, bool inherit);
		/// <summary>
		/// 
		/// </summary>
		MemberInfo MemberInfo { get; }		
	}
}
