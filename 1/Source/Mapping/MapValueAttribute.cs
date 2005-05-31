/*
 * File:    MapValueAttribute.cs
 * Created: 03/17/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// Is applied to any members that should be mapped on recordset field. 
	/// </summary>
	[AttributeUsage(
		AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Class,
		AllowMultiple = true
	)]
	public class MapValueAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		public MapValueAttribute() 
			: this(null, null)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeValue"></param>
		public MapValueAttribute(object typeValue) 
			: this(typeValue, null)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeValue"></param>
		/// <param name="mappedValue"></param>
		public MapValueAttribute(object typeValue, object mappedValue)
		{
			TypeValue   = typeValue;
			MappedValue = mappedValue;
		}

		private object _typeValue;
		/// <summary>
		/// 
		/// </summary>
		public object TypeValue
		{
			get { return _typeValue;  }
			set { _typeValue = value; }
		}

		private object _mappedValue;
		/// <summary>
		/// 
		/// </summary>
		public object MappedValue
		{
			get { return _mappedValue;  }
			set { _mappedValue = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsNullValue
		{
			get { return _mappedValue is Type && (Type)_mappedValue == typeof(DBNull); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsDefValue
		{
			get { return MappedValue == null; }
		}

		private bool _definedInXmlSchema;
		/// <summary>
		/// 
		/// </summary>
		public  bool  DefinedInXmlSchema
		{
			get { return _definedInXmlSchema;  }
		}

		internal void SetDefinedInXmlSchema()
		{
			_definedInXmlSchema = true;
		}
	}
}
