using System;

namespace BLToolkit.Reflection.Extension
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
	public class TypeExtensionAttribute : Attribute
	{
		public TypeExtensionAttribute()
		{
		}

		public TypeExtensionAttribute(string typeName) 
			: this(null, typeName)
		{
		}

		public TypeExtensionAttribute(string fileName, string typeName)
		{
			_fileName = fileName;
			_typeName = typeName;
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
	}
}
