/*
 * File:    MapFieldAttribute.cs
 * Created: 11/29/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;

namespace Rsdn.Framework.Data.Mapping
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
	public class MapXmlAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		public MapXmlAttribute()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeName"></param>
		public MapXmlAttribute(string typeName) 
			: this(null, typeName, null)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="typeName"></param>
		public MapXmlAttribute(string fileName, string typeName)
			 : this(fileName, typeName, null)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="typeName"></param>
		/// <param name="xPath"></param>
		public MapXmlAttribute(string fileName, string typeName, string xPath)
		{
			_fileName = fileName;
			_typeName = typeName;
			_xPath    = xPath;
		}

		private string _fileName;
		/// <summary>
		/// 
		/// </summary>
		public  string  FileName
		{
			get { return _fileName;  }
			set { _fileName = value; }
		}

		private string _typeName;
		/// <summary>
		/// 
		/// </summary>
		public  string  TypeName
		{
			get { return _typeName;  }
			set { _typeName = value; }
		}

		private string _xPath;
		/// <summary>
		/// 
		/// </summary>
		public  string  XPath
		{
			get { return _xPath;  }
			set { _xPath = value; }
		}
	}
}
