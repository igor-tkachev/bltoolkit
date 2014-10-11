using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace BLToolkit.ComponentModel.Design
{
	[DesignTimeVisible(false)]
	[ToolboxItem(false)]
	public partial class TypePicker : UserControl
	{
		public TypePicker()
		{
			InitializeComponent();

			if (!_size.IsEmpty)
				Size = _size;
		}

		ITypeResolutionService     _typeResolutionService;
		IWindowsFormsEditorService _windowsFormsEditorService;
		IServiceProvider           _serviceProvider;

		Type                       _resultType;
		Predicate<Type>            _filter;

		static Size _size;

		private T GetService<T>()
		{
			return (T)_serviceProvider.GetService(typeof(T));
		}

		public Type PickType(IServiceProvider serviceProvider, Type type, Predicate<Type> filter)
		{
			_resultType = type;
			_filter     = filter;

			_serviceProvider           = serviceProvider;
			_typeResolutionService     = GetService<ITypeResolutionService>();
			_windowsFormsEditorService = GetService<IWindowsFormsEditorService>();

			InitUI  ();
			AddTypes();

			if (_windowsFormsEditorService != null)
				_windowsFormsEditorService.DropDownControl(this);

			return _resultType;
		}

		private void InitUI()
		{
			IUIService uiService = GetService<IUIService>();

			if (uiService != null)
			{
				object color = uiService.Styles["VsColorPanelHyperLink"];

				if (color is Color)
					addNewLinkLabel.LinkColor = (Color)color;

				color = uiService.Styles["VsColorPanelHyperLinkPressed"];

				if (color is Color)
					addNewLinkLabel.ActiveLinkColor = (Color)color;
			}

			// Add None node.
			//
			TreeNode node = new TypeNode("None");

			treeView.Nodes.Add(node);
			treeView.SelectedNode = node;
		}

		private TypeNode GetTypeNode(DataSourceDescriptor ds)
		{
			Type type = null;

			if (_typeResolutionService != null)
				type = _typeResolutionService.GetType(ds.TypeName);

			try
			{
				if (type == null)
					type = Type.GetType(ds.TypeName, true);
			}
			catch
			{
				return null;
			}

			if (_filter != null && _filter(type) == false)
				return null;

			return new TypeNode(ds.Name, type);
		}

		private void AddGroup(DataSourceGroup group)
		{
			TreeNode groupNode = null;

			foreach (DataSourceDescriptor d in group.DataSources)
			{
				if (d == null)
					continue;

				TypeNode node = GetTypeNode(d);

				if (node == null)
					continue;

				if (group.IsDefault)
				{
					treeView.Nodes.Add(node);
				}
				else
				{
					if (groupNode == null)
						treeView.Nodes.Add(groupNode = new TreeNode(group.Name, 2, 2));

					groupNode.Nodes.Add(node);
				}

				if (_resultType == node.Type)
					treeView.SelectedNode = node;
			}
		}

		private void AddTypes()
		{
			DataSourceProviderService dspService = GetService<DataSourceProviderService>();

			if (dspService == null || !dspService.SupportsAddNewDataSource)
				return;

			DataSourceGroupCollection dataSources = null;

			try
			{
				dataSources = dspService.GetDataSources();
			}
			catch (Exception ex)
			{
				IUIService ui = GetService<IUIService>();

				string message = 
					"Cant retrieve Data Source Collection: " + ex.Message +
					"\nCheck the 'Properties\\DataSources' folder of your project.";

				if (ui != null)
					ui.ShowError(ex, message);
				else
					MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			if (dataSources == null)
				return;

			foreach (DataSourceGroup group in dataSources)
				if (group != null)
					AddGroup(group);
		}

		private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node is TypeNode)
			{
				_resultType = ((TypeNode)e.Node).Type;
				_windowsFormsEditorService.CloseDropDown();
			}
		}

		private void addNewLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			using (GetTypeDialog dlg = new GetTypeDialog(_serviceProvider, typeof (object), _filter))
			{
				IUIService   uiService = GetService<IUIService>();
				IWin32Window owner     = uiService == null? null: uiService.GetDialogOwnerWindow();
				DialogResult result    = dlg.ShowDialog(owner);

				if (result == DialogResult.OK && dlg.ResultType != null)
				{
					_resultType = dlg.ResultType;

					SaveType(_resultType);

					_windowsFormsEditorService.CloseDropDown();
				}
			}
		}

		private void TypePicker_Resize(object sender, EventArgs e)
		{
			_size = Size;
		}

		private void SaveType(Type type)
		{
			DataSourceProviderService dspService = GetService<DataSourceProviderService>();

			if (dspService == null || !dspService.SupportsAddNewDataSource)
				return;

			try
			{
				const string vs9TypeName = "Microsoft.VSDesigner.VSDesignerPackage.IGenericObjectDataSourcesService, Microsoft.VSDesigner, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
				const string vs8TypeName = "Microsoft.VSDesigner.VSDesignerPackage.IGenericObjectDataSourcesService, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

				Type   serviceType = Type.GetType(vs9TypeName) ?? Type.GetType(vs8TypeName);

				if (serviceType == null)
					return;

				object service = _serviceProvider.GetService(serviceType);

				if (service == null)
					return;

				MethodInfo mi = serviceType.GetMethod("AddGenericObjectDataSource");

				mi.Invoke(service, new object[] { _serviceProvider, null, type });
			}
			catch (Exception ex)
			{
				IUIService ui = GetService<IUIService>();

				if (ui != null)
					ui.ShowError(ex);
				else
					MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		internal class TypeNode : TreeNode
		{
			public TypeNode(string name)
				: base(name, 0, 0)
			{
				_isSelectable = true;
			}

			public TypeNode(string name, Type type)
				: this(name, type, true)
			{
			}

			public TypeNode(string name, Type type, bool isSelectable)
				: base(name, 3, 3)
			{
				_type         = type;
				_isSelectable = isSelectable;
			}

			private bool _isSelectable;
			public  bool  IsSelectable
			{
				get { return _isSelectable;  }
				set { _isSelectable = value; }
			}

			private readonly Type _type;
			public           Type  Type
			{
				get { return _type; }
			}
		}

		class NewLink : LinkLabel
		{
			protected override bool IsInputKey(Keys key)
			{
				return key == Keys.Return? true: base.IsInputKey(key);
			}
		}
	}
}
