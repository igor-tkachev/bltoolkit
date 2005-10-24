using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;

namespace BLToolkit.TypeBuilder.Builders
{
	public class BuildContext
	{
		public BuildContext(Type type)
		{
			_originalType = _type = type;
		}

		private bool _inProgress = true;
		public  bool  InProgress
		{
			get { return _inProgress;  }
			set { _inProgress = value; }
		}

		private TypeHelper _type;
		public  TypeHelper  Type
		{
			get { return _type;  }
			set { _type = value; }
		}

		private TypeHelper _originalType;
		public  TypeHelper  OriginalType
		{
			get { return _originalType;  }
			set { _originalType = value; }
		}

		private AssemblyBuilderHelper _assemblyBuilder;
		public  AssemblyBuilderHelper  AssemblyBuilder
		{
			get { return _assemblyBuilder;  }
			set { _assemblyBuilder = value; }
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

		private TypeHelper _currentInterface;
		public  TypeHelper  CurrentInterface
		{
			get { return _currentInterface;  }
			set { _currentInterface = value; }
		}

		private MethodBuilderHelper _methodBuilder;
		public  MethodBuilderHelper  MethodBuilder
		{
			get { return _methodBuilder;  }
			set { _methodBuilder = value; }
		}

		private LocalBuilder _returnValue;
		public  LocalBuilder  ReturnValue
		{
			get { return _returnValue;  }
			set { _returnValue = value; }
		}

		private Label _returnLabel;
		public  Label  ReturnLabel
		{
			get { return _returnLabel;  }
			set { _returnLabel = value; }
		}

		#region BuildElement

		private BuildElement _element;
		public  BuildElement  Element
		{
			get { return _element;  }
			set { _element = value; }
		}

		public bool IsAbstractProperty
		{
			get { return Element == BuildElement.AbstractGetter || Element == BuildElement.AbstractSetter; }
		}

		public bool IsAbstractGetter
		{
			get { return Element == BuildElement.AbstractGetter; }
		}

		public bool IsAbstractSetter
		{
			get { return Element == BuildElement.AbstractSetter; }
		}

		public bool IsAbstractMethod
		{
			get { return Element == BuildElement.AbstractMethod; }
		}

		public bool IsVirtualProperty
		{
			get { return Element == BuildElement.VirtualGetter|| Element == BuildElement.VirtualSetter; }
		}

		public bool IsVirtualGetter
		{
			get { return Element == BuildElement.VirtualGetter; }
		}

		public bool IsVirtualSetter
		{
			get { return Element == BuildElement.VirtualSetter; }
		}

		public bool IsVirtualMethod
		{
			get { return Element == BuildElement.VirtualMethod; }
		}

		#endregion

		#region BuildStep

		private BuildStep _step;
		public  BuildStep  Step
		{
			get { return _step;  }
			set { _step = value; }
		}

		public bool IsBeforeStep
		{
			get { return Step == BuildStep.Before; }
		}

		public bool IsBuildStep
		{
			get { return Step == BuildStep.Build; }
		}

		public bool IsAfterStep
		{
			get { return Step == BuildStep.After; }
		}

		public bool IsBeforeOrBuildStep
		{
			get { return Step == BuildStep.Before || Step == BuildStep.Build; }
		}

		#endregion

		private TypeBuilderList _typeBuilders;
		public  TypeBuilderList  TypeBuilders
		{
			get { return _typeBuilders;  }
			set { _typeBuilders = value; }
		}

		private PropertyInfo _currentProperty;
		public  PropertyInfo  CurrentProperty
		{
			get { return _currentProperty;  }
			set { _currentProperty = value; }
		}

		#region Internal Fields

		private Hashtable _fields;
		private Hashtable  Fields
		{
			get 
			{
				if (_fields == null)
					_fields = new Hashtable();

				return _fields;
			}
		}

		public FieldBuilder GetField(string fieldName)
		{
			return (FieldBuilder)Fields[fieldName];
		}

		public FieldBuilder CreateField(string fieldName, Type type, FieldAttributes attributes)
		{
			FieldBuilder field = TypeBuilder.DefineField(fieldName, type, attributes);

			field.SetCustomAttribute(MethodBuilder.Type.Assembly.BLToolkitAttribute);

			Fields.Add(fieldName, field);

			return field;
		}

		public FieldBuilder CreatePrivateField(string fieldName, Type type)
		{
			return CreateField(fieldName, type, FieldAttributes.Private);
		}

		public FieldBuilder CreatePrivateStaticField(string fieldName, Type type)
		{
			return CreateField(fieldName, type, FieldAttributes.Private | FieldAttributes.Static);
		}

		private  Hashtable _fieldInstanceEnsurers;
		internal Hashtable  FieldInstanceEnsurers
		{
			get
			{
				if (_fieldInstanceEnsurers == null)
					_fieldInstanceEnsurers = new Hashtable();

				return _fieldInstanceEnsurers;
			}
		}

		public MethodBuilderHelper GetFieldInstanceEnsurer(string fieldName)
		{
			return (MethodBuilderHelper)FieldInstanceEnsurers[fieldName];
		}

		#endregion
	}
}
