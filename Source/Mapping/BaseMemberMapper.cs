/*
 * File:    BaseMemberMapper.cs
 * Created: 07/27/2003
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
	 public abstract class BaseMemberMapper : IMemberMapper
	{
		private string _name;
		/// <summary>
		/// Mapped field name.
		/// </summary>
		public  string Name
		{
			get { return _name;  }
			set { _name = value; }
		}

		private string _originalName;
		/// <summary>
		/// Original field name.
		/// </summary>
		public  string OriginalName
		{
			get { return _originalName;  }
			set { _originalName = value; }
		}

		private bool _isNullable;
		/// <summary>
		///
		/// </summary>
		public  bool IsNullable
		{
			get { return _isNullable;  }
			set { _isNullable = value; }
		}

		private bool _isTrimmable;
		/// <summary>
		/// 
		/// </summary>
		public  bool IsTrimmable
		{
			get { return _isTrimmable;  }
			set { _isTrimmable = value; }
		}

		private bool _isClass;
		/// <summary>
		/// 
		/// </summary>
		public  bool IsClass
		{
			get { return _isClass;  }
			set { _isClass = value; }
		}

		private Type _memberType;
		/// <summary>
		/// 
		/// </summary>
		public  Type MemberType
		{
			get { return _memberType;  }
			set
			{
				_memberType = value;
				_typeIsEnum = value.IsEnum;
			}
		}

		private Attribute[] _mapValueAttributeList;
		/// <summary>
		/// 
		/// </summary>
		public  Attribute[]  MapValueAttributeList
		{
			get { return _mapValueAttributeList;  }
			set { _mapValueAttributeList = value; }
		}

		private int _order = -1;
		/// <summary>
		/// 
		/// </summary>
		public  int Order
		{
			get { return _order;  }
			set { _order = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public abstract object GetValue(object obj);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="value"></param>
		public abstract void SetValue(object obj, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributeType"></param>
		/// <param name="inherit"></param>
		/// <returns></returns>
		public abstract object[] GetCustomAttributes(Type attributeType, bool inherit);

		private static char[] _trimArray = new char[0];
		private bool _typeIsEnum;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public object MapFrom(object value)
		{
			return MapFrom(_memberType, _typeIsEnum, _mapValueAttributeList, value, _isTrimmable);
		}

		internal static object MapFrom(Type type, bool isEnum, Attribute[] attributes, object value, bool trimmable)
		{
			if (trimmable && value is string)
				value = ((string)value).TrimEnd(_trimArray);

			if (attributes.Length != 0)
			{
				IComparable comp        = value as IComparable;
				Type        valueType   = value != null? value.GetType(): null;
				object      mappedValue = null;
				bool        isNull      = false;

				foreach (MapValueAttribute attribute in attributes)
				{
					object mapValue = attribute.MappedValue;

					// If the source type is Int16 and the map type is Int32,
					// cast the map type to the source type.
					//
					if (valueType == typeof(short) && mapValue is int && 
						(int)mapValue >= short.MinValue &&
						(int)mapValue <= short.MaxValue)
					{
						mapValue = Convert.ToInt16(mapValue);
					}

					// Compare source and attribute values.
					//
					int compare = -1;

					if (comp != null && !(mapValue is Type))
					{
						try
						{
							compare = comp.CompareTo(mapValue);
						}
						catch
						{
						}
					}

					if (compare == 0)
					{
						mappedValue = attribute.TypeValue;
						break;
					}
					// NULL value.
					//
					else if (attribute.IsNullValue)
					{
						isNull = true;

						if (value is DBNull || value == null)
						{
							mappedValue = attribute.TypeValue;
							break;
						}
					}
					// Default value.
					//
					else if (attribute.IsDefValue)
					{
						mappedValue = attribute.TypeValue;
					}
				}

				// There is no value in the map list.
				//
				if (mappedValue == null)
				{
					if (attributes.Length != 1 || !isNull)
					{
						throw new RsdnMapException(
							value != null?
							string.Format("Cannot map '{0}' value '{1}' to '{2}'.", valueType.Name, value, type.Name):
							string.Format("Cannot map 'null' value to '{0}'.", type.Name));
					}
				}
				else
				{
					value = mappedValue;
				}
			}

			if (value is DBNull)
			{
				// We do not need the string type as a null value.
				//
				value = type == typeof(string)? string.Empty: null;
			}
			else if (value != null && isEnum && type != value.GetType())
			{
				Type underlyingType = Enum.GetUnderlyingType(type);

				if (underlyingType != value.GetType())
					value = Convert.ChangeType(value, underlyingType);

				value = Enum.Parse(type, Enum.GetName(type, value));
				// value = Enum.ToObject(type, value);
			}

			return value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public object MapTo(object value)
		{
			return MapTo(_mapValueAttributeList, value, _isNullable);
		}

		internal static object MapTo(Attribute[] attributes, object value, bool nullable)
		{
			if (attributes.Length != 0)
			{
				IComparable comp = value as IComparable;

				foreach (MapValueAttribute attribute in attributes)
				{
					if (attribute.IsDefValue == false)
					{
						object mapValue = attribute.TypeValue;

						// Compare field and attribute values.
						//
						if (comp != null && comp.CompareTo(mapValue) == 0)
						{
							value = attribute.MappedValue;
							break;
						}
						// Default value.
						//
						else if (mapValue == null)
						{
							value = attribute.MappedValue;
						}
					}
				}
			}

			// Set null value.
			//
			if (value is Type && (Type)value == typeof(DBNull) || nullable && Map.IsNull(value))
			{
				value = null;
			}

			return value;
		}

		#region ICloneable Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public abstract object Clone();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="desc"></param>
		protected void CopyTo(BaseMemberMapper desc)
		{
			desc.Name         = Name;
			desc.OriginalName = OriginalName;
			desc.IsNullable   = IsNullable;
			desc.IsTrimmable  = IsTrimmable;
			desc.IsClass      = IsClass;
			desc.MemberType   = MemberType;
			desc.Order        = Order;

			desc.MapValueAttributeList = new Attribute[MapValueAttributeList.Length];

			for (int i = 0; i < MapValueAttributeList.Length; i++)
			{
				MapValueAttribute attr = MapValueAttributeList[i] as MapValueAttribute;

				desc.MapValueAttributeList[i] = new MapValueAttribute(attr.TypeValue, attr.MappedValue);
			}
		}

		#endregion
	}
}
