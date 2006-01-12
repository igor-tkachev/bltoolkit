using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

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
			[DebuggerStepThrough] get { return _type;  }
			[DebuggerStepThrough] set { _type = value; }
		}

		private AssemblyBuilderHelper _assemblyBuilder;
		public  AssemblyBuilderHelper  AssemblyBuilder
		{
			[DebuggerStepThrough] get { return _assemblyBuilder;  }
			[DebuggerStepThrough] set { _assemblyBuilder = value; }
		}

		private TypeBuilderHelper _typeBuilder;
		public  TypeBuilderHelper  TypeBuilder
		{
			[DebuggerStepThrough] get { return _typeBuilder;  }
			[DebuggerStepThrough] set { _typeBuilder = value; }
		}

		private Hashtable  _items = new Hashtable(100);
		public  IDictionary Items
		{
			[DebuggerStepThrough] get { return _items; }
		}

		class PropertyInfoComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				PropertyInfo px = (PropertyInfo)x;
				PropertyInfo py = (PropertyInfo)y;

				return px.Name.CompareTo(py.Name);
			}
		}

		private static PropertyInfoComparer _piComparer = new PropertyInfoComparer();

		private SortedList  _fields;
		public  IDictionary  Fields
		{
			get
			{
				if (_fields == null)
					_fields = new SortedList(_piComparer, 10);

				return _fields;
			}
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
			[DebuggerStepThrough] get { return _currentInterface;  }
			[DebuggerStepThrough] set { _currentInterface = value; }
		}

		private MethodBuilderHelper _methodBuilder;
		public  MethodBuilderHelper  MethodBuilder
		{
			[DebuggerStepThrough] get { return _methodBuilder;  }
			[DebuggerStepThrough] set { _methodBuilder = value; }
		}

		private LocalBuilder _returnValue;
		public  LocalBuilder  ReturnValue
		{
			[DebuggerStepThrough] get { return _returnValue;  }
			[DebuggerStepThrough] set { _returnValue = value; }
		}

		private Label _returnLabel;
		public  Label  ReturnLabel
		{
			[DebuggerStepThrough] get { return _returnLabel;  }
			[DebuggerStepThrough] set { _returnLabel = value; }
		}

		#region BuildElement

		private BuildElement _element;
		public  BuildElement  BuildElement
		{
			[DebuggerStepThrough] get { return _element;  }
			[DebuggerStepThrough] set { _element = value; }
		}

		public bool IsAbstractProperty
		{
			[DebuggerStepThrough] get { return IsAbstractGetter || IsAbstractSetter; }
		}

		public bool IsSetter
		{
			[DebuggerStepThrough] get { return IsAbstractSetter || IsVirtualSetter; }
		}

		public bool IsAbstractGetter
		{
			[DebuggerStepThrough] get { return BuildElement == BuildElement.AbstractGetter; }
		}

		public bool IsAbstractSetter
		{
			[DebuggerStepThrough] get { return BuildElement == BuildElement.AbstractSetter; }
		}

		public bool IsAbstractMethod
		{
			[DebuggerStepThrough] get { return BuildElement == BuildElement.AbstractMethod; }
		}

		public bool IsVirtualProperty
		{
			[DebuggerStepThrough] get { return IsVirtualGetter|| IsVirtualSetter; }
		}

		public bool IsVirtualGetter
		{
			[DebuggerStepThrough] get { return BuildElement == BuildElement.VirtualGetter; }
		}

		public bool IsVirtualSetter
		{
			[DebuggerStepThrough] get { return BuildElement == BuildElement.VirtualSetter; }
		}

		public bool IsVirtualMethod
		{
			[DebuggerStepThrough] get { return BuildElement == BuildElement.VirtualMethod; }
		}

		#endregion

		#region BuildStep

		private BuildStep _step;
		public  BuildStep  Step
		{
			[DebuggerStepThrough] get { return _step;  }
			[DebuggerStepThrough] set { _step = value; }
		}

		public bool IsBeforeStep
		{
			[DebuggerStepThrough] get { return Step == BuildStep.Before; }
		}

		public bool IsBuildStep
		{
			[DebuggerStepThrough] get { return Step == BuildStep.Build; }
		}

		public bool IsAfterStep
		{
			[DebuggerStepThrough] get { return Step == BuildStep.After; }
		}

		public bool IsBeforeOrBuildStep
		{
			[DebuggerStepThrough] get { return Step == BuildStep.Before || Step == BuildStep.Build; }
		}

		#endregion

		private AbstractTypeBuilderList _typeBuilders;
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public  AbstractTypeBuilderList  TypeBuilders
		{
			[DebuggerStepThrough] get { return _typeBuilders;  }
			[DebuggerStepThrough] set { _typeBuilders = value; }
		}

		private PropertyInfo _currentProperty;
		public  PropertyInfo  CurrentProperty
		{
			[DebuggerStepThrough] get { return _currentProperty;  }
			[DebuggerStepThrough] set { _currentProperty = value; }
		}

		private MethodInfo _currentMethod;
		public  MethodInfo  CurrentMethod
		{
			[DebuggerStepThrough] get { return _currentMethod;  }
			[DebuggerStepThrough] set { _currentMethod = value; }
		}

		#region Internal Methods

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

		public FieldBuilder CreatePrivateField(PropertyInfo propertyInfo, string fieldName, Type type)
		{
			FieldBuilder field = CreateField(fieldName, type, FieldAttributes.Private);

			if (propertyInfo != null)
				Fields[propertyInfo] = field;

			return field;
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
