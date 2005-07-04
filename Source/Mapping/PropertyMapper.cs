/*
 * File:    PropertyMapper.cs
 * Created: 07/04/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.ComponentModel;
using System.Reflection;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// Internal class.
	/// </summary>
	public class PropertyMapper : BaseMemberMapper
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="schema"></param>
		/// <param name="propertyInfo"></param>
		/// <returns></returns>
		internal PropertyMapper InitProperty(MappingSchema schema, PropertyInfo propertyInfo)
		{
			_propertyInfo = propertyInfo;
			MemberType    = propertyInfo.PropertyType;
			IsClass       = !MemberType.IsValueType && MemberType != typeof(string);
			Name          = propertyInfo.Name;
			OriginalName  = propertyInfo.Name;
			IsTrimmable   = true;
			_canRead      = _propertyInfo.CanRead;
			_canWrite     = _propertyInfo.CanWrite;

			// Get field values.
			//
			MapValueAttributeList = schema.GetValueAttributes(propertyInfo, Name, MemberType);

			// Get field name.
			//
			Attribute[] attributes = schema.GetFieldAttributes(propertyInfo, Name);

			// If field has MapField attribute.
			//
			foreach (MapFieldAttribute attribute in attributes)
			{
				if (attribute.SourceName != null)
					Name = attribute.SourceName;

				IsNullable  = attribute.IsNullable;
				IsTrimmable = attribute.IsTrimmable;

				break;
			}

			IsTrimmable = IsTrimmable && MemberType == typeof(string);

			return this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="propertyInfo"></param>
		/// <param name="name"></param>
		/// <param name="member"></param>
		/// <returns></returns>
		internal PropertyMapper InitProperty(
			PropertyInfo propertyInfo, string name, IMemberMapper member)
		{
			InitProperty(new MappingSchema(), propertyInfo);

			Name         = name;
			_classMember = member;
			MapValueAttributeList = new Attribute[0];

			return this;
		}

		private IMemberMapper _classMember;
		private PropertyInfo  _propertyInfo;

		private bool _canRead;
		private bool _canWrite;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override object GetValue(object obj)
		{
			if (_canRead) 
			{
				object o = _propertyInfo.GetValue(obj, null);

				if (o != null)
					return _classMember != null? _classMember.GetValue(o): MapTo(o);
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="value"></param>
		public override void SetValue(object obj, object value)
		{
			if (_classMember != null)
			{
				object o = _propertyInfo.GetValue(obj, null);

				if (o != null)
				{
					//ISupportInitialize si = o as ISupportInitialize;

					//if (si != null) si.BeginInit();
					_classMember.SetValue(o, value);
					//if (si != null) si.EndInit();
				}
			}
			else if (_canWrite)
			{
				_propertyInfo.SetValue(obj, MapFrom(value), null);
			}
			else if (obj is IMapSettable)
			{
				((IMapSettable)obj).SetField(Name, value);
			}
		}

		#region ICloneable Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			PropertyMapper mapper = new PropertyMapper();

			CopyTo(mapper);

			mapper._classMember  = _classMember;
			mapper._propertyInfo = _propertyInfo;
			mapper._canRead      = _canRead;
			mapper._canWrite     = _canWrite;

			return mapper;
		}
		#endregion
	}
}
