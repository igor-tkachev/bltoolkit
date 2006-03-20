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
	[Designer(typeof(Design.WebObjectBinderDesigner))]
	[PersistChildren(false)]
	[ParseChildren(true)]
	[Description("BLToolkit Web Object Binder")]
	[DisplayName("Object Binder")]
	//[AspNetHostingPermission(SecurityAction.LinkDemand,        Level = AspNetHostingPermissionLevel.Minimal)]
	//[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebObjectBinder : DataSourceControl, IListSource
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

		internal ObjectBinder _objectBinder = new ObjectBinder();

		private void _objectBinder_ListChanged(object sender, ListChangedEventArgs e)
		{
			switch (e.ListChangedType)
			{
				case ListChangedType.PropertyDescriptorAdded:
				case ListChangedType.PropertyDescriptorChanged:
				case ListChangedType.PropertyDescriptorDeleted:
					RaiseDataSourceChangedEvent(e);
					break;
			}
		}

		#endregion

		#region IListSource Members

		bool IListSource.ContainsListCollection
		{
		    get { return false; }
		}

		IList IListSource.GetList()
		{
			return _objectBinder.List;
		}

		#endregion

		#region IDataSource Members

		private ObjectDataSourceView _view;

		protected override DataSourceView GetView(string viewName)
		{
			if (_view == null)
				_view = new ObjectDataSourceView(this, "DefaultView");

			return _view;
		}

		protected override ICollection GetViewNames()
		{
			return new string[] { "DefaultView" };
		}

		#endregion

		#region ObjectDataSourceView

		class ObjectDataSourceView : DataSourceView
		{
			public ObjectDataSourceView(WebObjectBinder owner, string viewName)
				: base(owner, viewName)
			{
				_owner = owner;
			}

			WebObjectBinder _owner;

			protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
			{
				return new ObjectEnumerator(_owner._objectBinder, arguments);
			}

			public override bool CanDelete { get { return _owner._objectBinder.AllowRemove; } }
			public override bool CanInsert { get { return _owner._objectBinder.AllowNew;    } }
			public override bool CanUpdate { get { return _owner._objectBinder.AllowEdit;   } }
			public override bool CanPage   { get { return true;                             } }
			public override bool CanSort   { get { return true;                             } }
			public override bool CanRetrieveTotalRowCount { get { return true; } }
		}

		#endregion

		#region ObjectEnumerator

		class ObjectEnumerator : ICollection, IEnumerable
		{
			public ObjectEnumerator(ObjectBinder objectBinder, DataSourceSelectArguments arguments)
			{
				_objectBinder = objectBinder;
				_arguments    = arguments;
			}

			private ObjectBinder              _objectBinder;
			private DataSourceSelectArguments _arguments;

			#region ICollection Members

			public void CopyTo(Array array, int index)
			{
				_objectBinder.List.CopyTo(array, index);
			}

			public int Count
			{
				get { return _objectBinder.List.Count; }
			}

			public bool IsSynchronized
			{
				get { return _objectBinder.List.IsSynchronized; }
			}

			public object SyncRoot
			{
				get { return _objectBinder.List.SyncRoot; }
			}

			#endregion

			#region IEnumerable Members

			public IEnumerator GetEnumerator()
			{
				_arguments.AddSupportedCapabilities(DataSourceCapabilities.Page);
				_arguments.AddSupportedCapabilities(DataSourceCapabilities.Sort);
				_arguments.AddSupportedCapabilities(DataSourceCapabilities.RetrieveTotalRowCount);

				EditableArrayList list = (EditableArrayList)_objectBinder.List;

				_arguments.TotalRowCount = list.Count;

				if (!string.IsNullOrEmpty(_arguments.SortExpression))
				{
					list = new EditableArrayList(list.ItemType, list.Count);
					list.AddRange(_objectBinder.List);

					list.Sort(_arguments.SortExpression);
				}

				int start = _arguments.StartRowIndex >= 0? _arguments.StartRowIndex: 0;
				int count = _arguments.MaximumRows    > 0?
					Math.Min(_arguments.MaximumRows, list.Count): list.Count;

				for (int i = 0; i < count; i++)
				{
					object o = list[i + start];

					yield return o is ICustomTypeDescriptor? o: new ObjectHolder(o, _objectBinder);
				}
			}

			#endregion
		}

		#endregion
	}
}
