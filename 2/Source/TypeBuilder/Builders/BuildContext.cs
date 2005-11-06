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
			_type = type;
		}

		private TypeHelper _type;
		public  TypeHelper  Type
		{
			get { return _type;  }
			set { _type = value; }
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

		private Hashtable  _items = new Hashtable(100);
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
		public  BuildElement  BuildElement
		{
			get { return _element;  }
			set { _element = value; }
		}

		public bool IsAbstractProperty
		{
			get { return IsAbstractGetter || IsAbstractSetter; }
		}

		public bool IsSetter
		{
			get { return IsAbstractSetter || IsVirtualSetter; }
		}

		public bool IsAbstractGetter
		{
			get { return BuildElement == BuildElement.AbstractGetter; }
		}

		public bool IsAbstractSetter
		{
			get { return BuildElement == BuildElement.AbstractSetter; }
		}

		public bool IsAbstractMethod
		{
			get { return BuildElement == BuildElement.AbstractMethod; }
		}

		public bool IsVirtualProperty
		{
			get { return IsVirtualGetter|| IsVirtualSetter; }
		}

		public bool IsVirtualGetter
		{
			get { return BuildElement == BuildElement.VirtualGetter; }
		}

		public bool IsVirtualSetter
		{
			get { return BuildElement == BuildElement.VirtualSetter; }
		}

		public bool IsVirtualMethod
		{
			get { return BuildElement == BuildElement.VirtualMethod; }
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

		private AbstractTypeBuilderList _typeBuilders;
		public  AbstractTypeBuilderList  TypeBuilders
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

		public FieldBuilder GetField(string fieldName)
		{
			return (FieldBuilder)Items["$BLToolkit.Field." + fieldName];
		}

		public FieldBuilder CreateField(string fieldName, Type type, FieldAttributes attributes)
		{
			FieldBuilder field = TypeBuilder.DefineField(fieldName, type, attributes);

			field.SetCustomAttribute(MethodBuilder.Type.Assembly.BLToolkitAttribute);

			Items.Add("$BLToolkit.Field." + fieldName, field);

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

		public MethodBuilderHelper GetFieldInstanceEnsurer(string fieldName)
		{
			return (MethodBuilderHelper)Items["$BLToolkit.FieldInstanceEnsurer." + fieldName];
		}

		#endregion
	}
}
