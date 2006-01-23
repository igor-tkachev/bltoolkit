using System;

namespace BLToolkit.Mapping
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
	public class MapXmlAttribute : Attribute
	{
		public MapXmlAttribute()
		{
		}

		public MapXmlAttribute(string typeName) 
			: this(null, typeName, null)
		{
		}

		public MapXmlAttribute(string fileName, string typeName)
			 : this(fileName, typeName, null)
		{
		}

		public MapXmlAttribute(string fileName, string typeName, string xPath)
		{
			_fileName = fileName;
			_typeName = typeName;
			_xPath    = xPath;
		}

		private string _fileName;
		public  string  FileName
		{
			get { return _fileName;  }
			set { _fileName = value; }
		}

		private string _typeName;
		public  string  TypeName
		{
			get { return _typeName;  }
			set { _typeName = value; }
		}

		private string _xPath;
		public  string  XPath
		{
			get { return _xPath;  }
			set { _xPath = value; }
		}
	}
}
