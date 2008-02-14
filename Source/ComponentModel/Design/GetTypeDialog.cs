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
				treeView.Nodes.Clear();

				Hashtable assemblyNodes  = new Hashtable();
				Hashtable namespaceNodes = new Hashtable();
				Hashtable typeNodes      = new Hashtable();

				ITypeDiscoveryService service = 
					(ITypeDiscoveryService)_serviceProvider.GetService(typeof(ITypeDiscoveryService));

				ICollection cTypes = service.GetTypes(_baseType, systemCheckBox.Checked);
				List<Type>  types  = new List<Type>(cTypes.Count);

				foreach (Type type in cTypes)
					types.Add(type);

				types.Sort(delegate(Type a, Type b)
				{
					return a.Assembly == b.Assembly?
						a.FullName.CompareTo(b.FullName):
						a.Assembly.FullName.CompareTo(b.Assembly.FullName);
				});

				foreach (Type type in types)
				{
					if (_filter != null && _filter(type) == false)
						continue;

					Assembly assembly     = type.Assembly;
					TreeNode assemblyNode = (TreeNode)assemblyNodes[assembly];

					if (assemblyNode == null)
					{
						assemblyNodes[assembly] = assemblyNode =
							treeView.Nodes.Add(assembly.FullName, assembly.GetName().Name, 1, 1);
					}

					string  @namespace     = type.Namespace ?? string.Empty;
					string   namespaceKey  = assembly.FullName + ", " + @namespace;
					TreeNode namespaceNode = (TreeNode)namespaceNodes[namespaceKey];

					if (namespaceNode == null)
					{
						namespaceNodes[namespaceKey] = namespaceNode =
							assemblyNode.Nodes.Add(namespaceKey, @namespace, 2, 2);
					}

					GetTypeNode getTypeNode = null; getTypeNode = delegate(Type t)
					{
						TypePicker.TypeNode node = (TypePicker.TypeNode)typeNodes[t];

						if (node != null)
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

						typeNodes[t] = node;

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
			TypePicker.TypeNode node = e.Node as TypePicker.TypeNode;

			_resultType = node != null && node.IsSelectable? node.Type: null;

			okButton.Enabled = _resultType != null;
		}

		private void treeView_DoubleClick(object sender, EventArgs e)
		{
			if (okButton.Enabled)
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}
	}
}