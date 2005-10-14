using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder
{
	public class TypeBuilderContext
	{
		private AssemblyBuilderHelper _assemblyBuilder;
		public  AssemblyBuilderHelper  AssemblyBuilder
		{
			get { return _assemblyBuilder;  }
			set { _assemblyBuilder = value; }
		}

		private TypeBuilderInfo _info;
		public  TypeBuilderInfo  Info
		{
			get { return _info;  }
			set { _info = value; }
		}

		public Type Type
		{
			get { return _info.OriginalType; }
		}

		private TypeBuilderHelper _typeBuilder;
		public  TypeBuilderHelper  TypeBuilder
		{
			get { return _typeBuilder;  }
			set { _typeBuilder = value; }
		}

		private Hashtable  _items = new Hashtable();
		public  IDictionary Items
		{
			get { return _items; }
		}

		private Hashtable _interfaceMap;
		public  Hashtable  InterfaceMap
		{
			get 
			{
				if (_interfaceMap == null)
					_interfaceMap = new Hashtable();

				return _interfaceMap;
			}
		}
	}
}
