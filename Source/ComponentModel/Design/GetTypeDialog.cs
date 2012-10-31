using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace BLToolkit.ComponentModel.Design
{
	public partial class GetTypeDialog : Form
	{
		public GetTypeDialog(IServiceProvider serviceProvider, Type baseType, Predicate<Type> filter)
		{
			_serviceProvider = serviceProvider;
			_baseType        = baseType;
			_filter          = filter;

			InitializeComponent();

			LoadTypes();
		}

		private readonly IServiceProvider _serviceProvider;
		private readonly Type             _baseType;
		private readonly Predicate<Type>  _filter;

		private Type _resultType;
		public  Type  ResultType
		{
			get { return _resultType; }
		}

		delegate TypePicker.TypeNode GetTypeNode(Type t);

		private void LoadTypes()
		{
			Cursor = Cursors.WaitCursor;

			try
			{
				_treeView.Nodes.Clear();

				var assemblyNodes  = new Dictionary<Assembly, TreeNode>();
				var namespaceNodes = new Dictionary<string, TreeNode>();
				var typeNodes      = new Dictionary<Type, TypePicker.TypeNode>();
				var service        = (ITypeDiscoveryService)_serviceProvider.GetService(typeof(ITypeDiscoveryService));
				var cTypes         = service.GetTypes(_baseType, _systemCheckBox.Checked);
				var types          = new List<Type>(cTypes.Count);

				foreach (Type type in cTypes)
					types.Add(type);

				types.Sort((a, b) =>
					a.Assembly == b.Assembly ?
						string.Compare(a.FullName, b.FullName) :
						string.Compare(a.Assembly.FullName, b.Assembly.FullName));

				foreach (var type in types)
				{
					if (_filter != null && _filter(type) == false)
						continue;

					var assembly = type.Assembly;

					TreeNode assemblyNode;

					if (!assemblyNodes.TryGetValue(assembly, out assemblyNode))
					{
						assemblyNodes[assembly] = assemblyNode =
							_treeView.Nodes.Add(assembly.FullName, assembly.GetName().Name, 1, 1);
					}

					var @namespace    = type.Namespace ?? string.Empty;
					var namespaceKey = assembly.FullName + ", " + @namespace;

					TreeNode namespaceNode;

					if (!namespaceNodes.TryGetValue(namespaceKey, out namespaceNode))
					{
						namespaceNodes[namespaceKey] = namespaceNode =
							assemblyNode.Nodes.Add(namespaceKey, @namespace, 2, 2);
					}

					GetTypeNode getTypeNode = null; getTypeNode = t =>
					{
						TypePicker.TypeNode node;
						
						if (typeNodes.TryGetValue(t, out node))
							return node;

						if (t.DeclaringType == null)
						{
							namespaceNode.Nodes.Add(node = new TypePicker.TypeNode(t.Name, t, false));
						}
						else
						{
							TreeNode parent = getTypeNode(t.DeclaringType);

							parent.Nodes.Add(node = new TypePicker.TypeNode(t.Name, t, false));
						}

						typeNodes.Add(t, node);

						return node;
					};

					getTypeNode(type).IsSelectable = true;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void systemCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			LoadTypes();
		}

		private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			var node = e.Node as TypePicker.TypeNode;

			_resultType = node != null && node.IsSelectable? node.Type: null;

			_okButton.Enabled = _resultType != null;
		}

		private void treeView_DoubleClick(object sender, EventArgs e)
		{
			_okButton.PerformClick();
		}

	}
}