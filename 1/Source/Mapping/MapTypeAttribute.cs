/*
 * File:    MapTypeAttribute.cs
 * Created: 11/11/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// Defines an internal implementation type for an abstract property.
	/// </summary>
	/// <include file="Doc.xml" path='examples/maptype[@name="remarks"]/*' />
	[AttributeUsage(
	AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface,
	AllowMultiple = true)]
	public class MapTypeAttribute : MapParameterAttribute
	{
		/// <summary>
		/// 
		/// </summary>
		/// <include file="Examples.xml" path='examples/maptype[@name="ctor(Type)"]/*' />
		/// <param name="mappedType"></param>
		public MapTypeAttribute(Type mappedType)
		{
			_mappedType = mappedType;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mappedType"></param>
		/// <param name="parameter1"></param>
		/// <include file="Examples.xml" path='examples/maptype[@name="ctor(Type,object,object)"]/*' />
		public MapTypeAttribute(
			Type   mappedType,
			object parameter1)
			: this(mappedType)
		{
			SetParameters(parameter1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mappedType"></param>
		/// <param name="parameter1"></param>
		/// <param name="parameter2"></param>
		/// <include file="Examples.xml" path='examples/maptype[@name="ctor(Type,object,object)"]/*' />
		public MapTypeAttribute(
			Type   mappedType,
			object parameter1,
			object parameter2)
			: this(mappedType)
		{
			SetParameters(parameter1, parameter2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mappedType"></param>
		/// <param name="parameter1"></param>
		/// <param name="parameter2"></param>
		/// <param name="parameter3"></param>
		/// <include file="Examples.xml" path='examples/maptype[@name="ctor(Type,object,object)"]/*' />
		public MapTypeAttribute(
			Type   mappedType,
			object parameter1,
			object parameter2,
			object parameter3)
			: this(mappedType)
		{
			SetParameters(parameter1, parameter2, parameter3);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mappedType"></param>
		/// <param name="parameter1"></param>
		/// <param name="parameter2"></param>
		/// <param name="parameter3"></param>
		/// <param name="parameter4"></param>
		/// <include file="Examples.xml" path='examples/maptype[@name="ctor(Type,object,object)"]/*' />
		public MapTypeAttribute(
			Type   mappedType,
			object parameter1,
			object parameter2,
			object parameter3,
			object parameter4)
			: this(mappedType)
		{
			SetParameters(parameter1, parameter2, parameter3, parameter4);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mappedType"></param>
		/// <param name="parameter1"></param>
		/// <param name="parameter2"></param>
		/// <param name="parameter3"></param>
		/// <param name="parameter4"></param>
		/// <param name="parameter5"></param>
		/// <include file="Examples.xml" path='examples/maptype[@name="ctor(Type,object,object)"]/*' />
		public MapTypeAttribute(
			Type   mappedType,
			object parameter1,
			object parameter2,
			object parameter3,
			object parameter4,
			object parameter5)
			: this(mappedType)
		{
			SetParameters(parameter1, parameter2, parameter3, parameter4, parameter5);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <include file="Examples.xml" path='examples/maptype[@name="ctor(Type)"]/*' />
		/// <param name="propertyType"></param>
		/// <param name="mappedType"></param>
		public MapTypeAttribute(Type propertyType, Type mappedType)
		{
			_mappedType   = mappedType;
			_propertyType = propertyType;
		}

		private Type _propertyType;
		/// <summary>
		/// 
		/// </summary>
		public  Type PropertyType
		{
			get { return _propertyType;  }
			set { _propertyType = value; }
		}

		private Type _mappedType;
		/// <summary>
		/// 
		/// </summary>
		public  Type MappedType
		{
			get { return _mappedType;  }
			set { _mappedType = value; }
		}
	}
}
