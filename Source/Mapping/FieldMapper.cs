/*
 * File:    FieldMapper.cs
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
	/// 
	/// </summary>
	public class FieldMapper : BaseMemberMapper
	{
		internal FieldMapper InitField(MappingSchema schema, FieldInfo fieldInfo, Attribute[] valueAttributes)
		{
			_fieldInfo   = fieldInfo;
			MemberType   = fieldInfo.FieldType;
			IsClass      = !MemberType.IsValueType && MemberType != typeof(string);
			Name         = fieldInfo.Name;
			OriginalName = fieldInfo.Name;
			IsTrimmable  = true;
			
			// Get field values.
			//
			MapValueAttributeList = valueAttributes != null?
				valueAttributes:
				schema.GetValueAttributes(fieldInfo, Name, MemberType);

			// Get field name.
			//
			Attribute[] attributes = schema.GetFieldAttributes(fieldInfo, Name);

			// If the field has the MapField attribute.
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
		/// <param name="fieldInfo"></param>
		/// <param name="name"></param>
		/// <param name="member"></param>
		/// <returns></returns>
		public FieldMapper InitField(FieldInfo fieldInfo, string name, IMemberMapper member)
		{
			InitField(new MappingSchema(), fieldInfo, null);

			Name         = name;
			_classMember = member;
			MapValueAttributeList = new MapValueAttribute[0];

			return this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public FieldMapper InitField(string name)
		{
			Name  = name;
			Order = -1;
			MapValueAttributeList = new MapValueAttribute[0];

			return this;
		}

		private IMemberMapper _classMember;
		private FieldInfo     _fieldInfo;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override object GetValue(object obj)
		{
			object o = _fieldInfo.GetValue(obj);

			return _classMember != null? _classMember.GetValue(o): MapTo(o);
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
				object o = _fieldInfo.GetValue(obj);

				if (o != null)
				{
					ISupportInitialize si = o as ISupportInitialize;

					if (si != null) si.BeginInit();
					_classMember.SetValue(o, value);
					if (si != null) si.EndInit();
				}
			}
			else
			{
				_fieldInfo.SetValue(obj, MapFrom(value));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributeType"></param>
		/// <param name="inherit"></param>
		/// <returns></returns>
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return _fieldInfo.GetCustomAttributes(attributeType, inherit);
		}

		#region ICloneable Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			FieldMapper mapper = new FieldMapper();

			CopyTo(mapper);

			mapper._classMember = _classMember;
			mapper._fieldInfo   = _fieldInfo;

			return mapper;
		}
		#endregion
	}
}
