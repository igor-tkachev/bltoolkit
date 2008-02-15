using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Web.Compilation;
using System.Web.UI;

using BLToolkit.ComponentModel;
using BLToolkit.ComponentModel.Design;
using BLToolkit.EditableObjects;
using BLToolkit.Web.UI.Design;

namespace BLToolkit.Web.UI
{
	[DefaultProperty("TypeName")]
	[ToolboxBitmap(typeof(WebObjectBinder))]
	[Designer(typeof(WebObjectBinderDesigner))]
	[PersistChildren(false)]
	[ParseChildren(true)]
	[Description("BLToolkit Web Object Binder")]
	[DisplayName("Object Binder")]
	public class WebObjectBinder : DataSourceControl, IListSource
	{
		#region Constructors

		public WebObjectBinder()
		{
			_objectBinder.ListChanged += _objectBinder_ListChanged;
		}

		#endregion

		#region Public Members

		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(null)]
		[Category("Data")]
		[Editor(typeof(TypeNameEditor), typeof(UITypeEditor))]
		public string TypeName
		{
			get
			{
				Type type = _objectBinder.ItemType;
				return type == null ? "(none)" : type.FullName;
			}
			set
			{
				_objectBinder.ItemType = string.IsNullOrEmpty(value) || value == "(none)"?
					null: BuildManager.GetType(value, false, true);
			}
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(null)]
		[Category("Data")]
		[Editor(typeof(ObjectViewTypeNameEditor), typeof(UITypeEditor))]
		public string ObjectViewTypeName
		{
			get
			{
				Type type = _objectBinder.ObjectViewType;
				return type == null ? "(none)" : type.FullName;
			}
			set
			{
				_objectBinder.ObjectViewType = string.IsNullOrEmpty(value) || value == "(none)"?
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

		public override void Dispose()
		{
			_objectBinder.Dispose();

			base.Dispose();
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

			readonly WebObjectBinder _owner;

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

		class ObjectEnumerator : ICollection
		{
			public ObjectEnumerator(ObjectBinder objectBinder, DataSourceSelectArguments arguments)
			{
				_objectBinder = objectBinder;
				_arguments    = arguments;
			}

			private readonly ObjectBinder              _objectBinder;
			private readonly DataSourceSelectArguments _arguments;

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
					list.SortEx(_arguments.SortExpression);
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
