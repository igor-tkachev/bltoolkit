using System;
using System.Web.UI.Design;
using System.Security.Permissions;
using System.ComponentModel;

namespace BLToolkit.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
	class  WebObjectBinderDesigner : DataSourceDesigner
	{
		private DesignerDataSourceView _view;
		private WebObjectBinder        _component;

		public override string[] GetViewNames()
		{
			return new string[] { "DefaultView" };
		}

		public override DesignerDataSourceView GetView(string viewName)
		{
			if (string.IsNullOrEmpty(viewName))
				viewName = "DefaultView";

			if (viewName != "DefaultView")
				return null;

			if (_view == null)
				_view = new ObjectDesignerDataSourceView(this, viewName);

			return _view;
		}

		public override void Initialize(IComponent component)
		{
			_component = (WebObjectBinder)component;

			base.Initialize(component);
		}

		public override bool CanConfigure
		{
			get { return true; }
		}

		public override void Configure()
		{
		}

		#region ObjectDesignerDataSourceView

		class ObjectDesignerDataSourceView : DesignerDataSourceView
		{
			public ObjectDesignerDataSourceView(WebObjectBinderDesigner owner, string viewName)
				: base(owner, viewName)
			{
				_owner = owner;
			}

			private readonly WebObjectBinderDesigner _owner;

			public override IDataSourceViewSchema Schema
			{
				get { return new ObjectViewSchema(_owner); }
			}

			public override bool CanDelete { get { return _owner._component._objectBinder.AllowRemove; } }
			public override bool CanInsert { get { return _owner._component._objectBinder.AllowNew;    } }
			public override bool CanUpdate { get { return _owner._component._objectBinder.AllowEdit;   } }
			public override bool CanPage   { get { return true;                                        } }
			public override bool CanSort   { get { return true;                                        } }
			public override bool CanRetrieveTotalRowCount { get { return true; } }

			class ObjectViewSchema : IDataSourceViewSchema
			{
				public ObjectViewSchema(WebObjectBinderDesigner owner)
				{
					_owner = owner;
				}

				private readonly WebObjectBinderDesigner _owner;

				public IDataSourceViewSchema[] GetChildren()
				{
					return null;
				}

				public IDataSourceFieldSchema[] GetFields()
				{
					PropertyDescriptorCollection fields =
						((ITypedList)_owner._component._objectBinder).GetItemProperties(null);

					IDataSourceFieldSchema[] schema = new IDataSourceFieldSchema[fields.Count];

					for (int i = 0; i < schema.Length; i++)
						schema[i] = new ObjectFieldSchema(fields[i]);

					return schema;
				}

				public string Name
				{
					get
					{
						Type type = _owner._component._objectBinder.ItemType;

						return type != null? type.Name: string.Empty;
					}
				}

				class ObjectFieldSchema : IDataSourceFieldSchema
				{
					public ObjectFieldSchema(PropertyDescriptor propertyDescriptor)
					{
						_propertyDescriptor = propertyDescriptor;

						DataObjectFieldAttribute attr =
							(DataObjectFieldAttribute)_propertyDescriptor.Attributes[typeof(DataObjectFieldAttribute)];

						if (attr != null)
						{
							_length     = attr.Length;
							_primaryKey = attr.PrimaryKey;
							_isIdentity = attr.IsIdentity;
							_isNullable = attr.IsNullable;
						}
					}

					private readonly PropertyDescriptor _propertyDescriptor;
					private readonly int                _length = -1;
					private readonly bool               _isIdentity;
					private readonly bool               _isNullable;
					private readonly bool               _primaryKey;

					public Type   DataType   { get { return _propertyDescriptor.PropertyType; } }
					public bool   Identity   { get { return _isIdentity;                      } }
					public bool   IsReadOnly { get { return _propertyDescriptor.IsReadOnly;   } }
					public bool   IsUnique   { get { return false;                            } }
					public int    Length     { get { return _length;                          } }
					public string Name       { get { return _propertyDescriptor.Name;         } }
					public int    Precision  { get { return -1;                               } }
					public bool   PrimaryKey { get { return _primaryKey;                      } }
					public int    Scale      { get { return -1;                               } }

					public bool Nullable
					{
						get
						{
							Type type           = _propertyDescriptor.PropertyType;
							Type underlyingType = System.Nullable.GetUnderlyingType(type);

							return underlyingType != null? true: _isNullable;
						}
					}
				}
			}
		}

		#endregion
	}
}
