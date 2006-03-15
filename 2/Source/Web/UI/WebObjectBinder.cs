using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;

using BLToolkit.ComponentModel;
using System.Web.Compilation;
using BLToolkit.EditableObjects;

namespace BLToolkit.Web.UI
{
	[DefaultProperty("TypeName")]
	[ToolboxBitmap(typeof(WebObjectBinder))]
	//[ControlBuilder(typeof(DataSourceControlBuilder))]
	//[NonVisualControl]
	[Designer("System.Web.UI.Design.DataSourceDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	//[Bindable(false)]
	[PersistChildren(false)]
	[ParseChildren(true)]
	[Description("BLToolkit Web Object Binder")]
	[DisplayName("Object Binder")]
	[AspNetHostingPermission(SecurityAction.LinkDemand,        Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebObjectBinder : Control, IDataSource, IListSource, ITypedList
	{
		#region Constructors

		public WebObjectBinder()
		{
			_objectBinder.ListChanged += new ListChangedEventHandler(_objectBinder_ListChanged);
		}

		#endregion

		#region Public Members

		/*
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(null)]
		[Category("Data")]
		[TypeConverter(typeof(TypeTypeConverter))]
#if FW2
		[Editor(typeof(BLToolkit.ComponentModel.Design.TypeEditor), typeof(UITypeEditor))]
#endif
		public Type ItemType
		{
			get { return _objectBinder.ItemType;  }
			set { _objectBinder.ItemType = value; }
		}
		*/

		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(null)]
		[Category("Data")]
#if FW2
		[Editor(typeof(BLToolkit.ComponentModel.Design.TypeNameEditor), typeof(UITypeEditor))]
#endif
		public string TypeName
		{
			get
			{
				Type type = _objectBinder.ItemType;
				return type == null ? "(none)" : type.FullName;
			}
			set
			{
				_objectBinder.ItemType = value == null || value.Length == 0 || value == "(none)"?
					null: BuildManager.GetType(value, false, true);
			}
		}

		[Browsable(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Object
		{
			get { return _objectBinder.Object;  }
			set { _objectBinder.Object = value; }
		}

		[Browsable(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IList List
		{
			get { return _objectBinder.List;  }
			set { _objectBinder.List = value; }
		}

		#endregion

		#region Protected members

		private ObjectBinder _objectBinder = new ObjectBinder();

		private void _objectBinder_ListChanged(object sender, ListChangedEventArgs e)
		{
			if (DataSourceChanged != null)
			{
				switch (e.ListChangedType)
				{
					case ListChangedType.PropertyDescriptorAdded:
					case ListChangedType.PropertyDescriptorChanged:
					case ListChangedType.PropertyDescriptorDeleted:
						DataSourceChanged(this, e);
						break;
				}
			}
		}

		#endregion

		#region IListSource Members

		[Browsable(false)]
		public bool ContainsListCollection
		{
			get { return false; }
		}

		public IList GetList()
		{
			return _objectBinder.List;
		}

		#endregion

		#region IDataSource Members

		public event EventHandler DataSourceChanged;

		private ObjectDataSourceView _view;

		public DataSourceView GetView(string viewName)
		{
			if (_view == null)
				_view = new ObjectDataSourceView(this, "DefaultView");

			return _view;
		}

		public ICollection GetViewNames()
		{
			return new string[] { "DefaultView" };
		}

		#endregion

		#region ITypedList Members

		public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			return ((ITypedList)_objectBinder).GetItemProperties(listAccessors);
		}

		public string GetListName(PropertyDescriptor[] listAccessors)
		{
			return ((ITypedList)_objectBinder).GetListName(listAccessors);
		}

		#endregion

		#region ObjectDataSourceView

		class ObjectDataSourceView : DataSourceView, ITypedList
		{
			public ObjectDataSourceView(WebObjectBinder owner, string viewName)
				: base(owner, viewName)
			{
				_owner = owner;
			}

			WebObjectBinder _owner;

			protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
			{
				return new ObjectEnumerator(_owner._objectBinder);
			}

			#region ITypedList Members

			public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
			{
				return ((ITypedList)_owner._objectBinder).GetItemProperties(listAccessors);
			}

			public string GetListName(PropertyDescriptor[] listAccessors)
			{
				return ((ITypedList)_owner._objectBinder).GetListName(listAccessors);
			}

			#endregion
		}

		class ObjectEnumerator : EditableArrayList, IEnumerable
		{
			public ObjectEnumerator(ObjectBinder objectBinder)
				: base(objectBinder.ItemType, (ArrayList)objectBinder.List)
			{
				_objectBinder = objectBinder;
			}

			private ObjectBinder _objectBinder;

			public override IEnumerator GetEnumerator()
			{
				foreach (object o in List)
					yield return new ObjectDescriptor(o, _objectBinder);;
			}
		}

		class ObjectDescriptor : ICustomTypeDescriptor
		{
			public ObjectDescriptor(object obj, ObjectBinder objectBinder)
			{
				_object       = obj;
				_objectBinder = objectBinder;
			}

			object       _object;
			ObjectBinder _objectBinder;

			#region ICustomTypeDescriptor Members

			public System.ComponentModel.AttributeCollection GetAttributes()
			{
				return System.ComponentModel.AttributeCollection.Empty;
			}

			public string GetClassName()
			{
				return _object.GetType().Name;
			}

			public string GetComponentName()
			{
				return null;
			}

			public TypeConverter GetConverter()
			{
				return null;
			}

			public EventDescriptor GetDefaultEvent()
			{
				return null;
			}

			public PropertyDescriptor GetDefaultProperty()
			{
				return null;
			}

			public object GetEditor(Type editorBaseType)
			{
				return null;
			}

			public EventDescriptorCollection GetEvents(Attribute[] attributes)
			{
				return null;
			}

			public EventDescriptorCollection GetEvents()
			{
				return null;
			}

			public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
			{
				return ((ITypedList)_objectBinder).GetItemProperties(null);
			}

			public PropertyDescriptorCollection GetProperties()
			{
				return ((ICustomTypeDescriptor)this).GetProperties(null);
			}

			public object GetPropertyOwner(PropertyDescriptor pd)
			{
				return this;//pd is ObjectDescriptor? this: null;
			}

			#endregion
		}

		#endregion
	}
}
