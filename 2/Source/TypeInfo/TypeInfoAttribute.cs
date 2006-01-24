using System;

namespace BLToolkit.TypeInfo
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
	public class TypeInfoAttribute : Attribute
	{
		public TypeInfoAttribute()
		{
		}

		public TypeInfoAttribute(string typeName) 
			: this(null, typeName)
		{
		}

		public TypeInfoAttribute(string fileName, string typeName)
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
