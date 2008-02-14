using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using BLToolkit.TypeBuilder;

namespace BLToolkit.EditableObjects
{
	[Serializable]
	public class EditableXmlDocument : IEditable, ISetParent, IMemberwiseEditable, IPrintDebugState
	{
		private readonly XmlNodeChangedEventHandler _handler;
		private          Stack                      _changedNodes;
		private          XmlDocument                _original;
		private          XmlDocument                _current;
		private          IPropertyChanged           _parent;
		private          PropertyInfo               _propertyInfo;

		public EditableXmlDocument()
			: this(new XmlDocument())
		{
		}

		public EditableXmlDocument(XmlDocument value)
		{
			_changedNodes = null;
			_handler      = HandleNodeChanged;
			_current      = value;
			_original     = value;

			StartXmlDocTracking();
		}

		[GetValue, SetValue]
		public XmlDocument Value
		{
			get { return _current;  }
			set
			{
				if (_current != value)
				{
					if (_current == _original)
						StopXmlDocTracking();

					// Drop changes, if any.
					//
					if (_changedNodes != null)
						_changedNodes.Clear();

					_current = value;

					if (_current == _original)
						StartXmlDocTracking();
				}
			}
		}

		private void StartXmlDocTracking()
		{
			if (_current != null)
			{
				_current.NodeInserting += _handler;
				_current.NodeRemoving  += _handler;
				_current.NodeChanging  += _handler;
			}
		}

		private void StopXmlDocTracking()
		{
			if (_current != null)
			{
				_current.NodeInserting -= _handler;
				_current.NodeRemoving  -= _handler;
				_current.NodeChanging  -= _handler;
			}
		}

		private void HandleNodeChanged(object sender, XmlNodeChangedEventArgs ea)
		{
			if (ea.Action == XmlNodeChangedAction.Change && ea.NewValue == ea.OldValue)
			{
				// A void change can be ignored.
				//
				return;
			}

			if (_changedNodes == null)
				_changedNodes = new Stack();

			_changedNodes.Push(new XmlNodeTrackBack(ea));

			// Propagate changes to parent object, if set.
			//
			if (_parent != null)
				_parent.OnPropertyChanged(_propertyInfo);
		}

		#region IEditable Members

		public void AcceptChanges()
		{
			if (_original != _current)
			{
				_original = _current;
				StartXmlDocTracking();
			}
			else
			{
				// Let them go away.
				//
				if (_changedNodes != null)
					_changedNodes.Clear();
			}
		}

		public void RejectChanges()
		{
			if (_original != _current)
			{
				_current = _original;
				StartXmlDocTracking();
			}
			else if (_changedNodes != null && _changedNodes.Count > 0)
			{
				// Don't fall into an infinite loop.
				//
				StopXmlDocTracking();

				// A Stack enumerates from back to front.
				//
				foreach (XmlNodeTrackBack nodeTrackBack in _changedNodes)
				{
					switch (nodeTrackBack.Action)
					{
						case XmlNodeChangedAction.Insert:
							if (nodeTrackBack.Node.NodeType == XmlNodeType.Attribute)
								((XmlElement)nodeTrackBack.Value).Attributes.Remove((XmlAttribute)nodeTrackBack.Node);
							else
								((XmlNode)nodeTrackBack.Value).RemoveChild(nodeTrackBack.Node);
							break;
						case XmlNodeChangedAction.Remove:
							// NB: The order of children nodes may change.
							//
							if (nodeTrackBack.Node.NodeType == XmlNodeType.Attribute)
								((XmlElement)nodeTrackBack.Value).Attributes.Append((XmlAttribute)nodeTrackBack.Node);
							else
								((XmlNode)nodeTrackBack.Value).AppendChild(nodeTrackBack.Node);
							break;
						case XmlNodeChangedAction.Change:
							nodeTrackBack.Node.Value = (string)nodeTrackBack.Value;
							break;
					}
				}

				_changedNodes.Clear();
				StartXmlDocTracking();
			}
		}

		public bool IsDirty
		{
			get
			{
				if (_current == _original)
					return _changedNodes != null && _changedNodes.Count > 0;

				if (_current == null || _original == null)
					return true;

				return _current.InnerXml.TrimEnd() != _original.InnerXml.TrimEnd();
			}
		}

		#endregion

		#region IMemberwiseEditable Members

		public bool AcceptMemberChanges(PropertyInfo propertyInfo, string memberName)
		{
			if (memberName != propertyInfo.Name)
				return false;

			AcceptChanges();

			return true;
		}

		public bool RejectMemberChanges(PropertyInfo propertyInfo, string memberName)
		{
			if (memberName != propertyInfo.Name)
				return false;

			RejectChanges();

			return true;
		}

		public bool IsDirtyMember(PropertyInfo propertyInfo, string memberName, ref bool isDirty)
		{
			if (memberName != propertyInfo.Name)
				return false;

			isDirty = IsDirty;

			return true;
		}

		public void GetDirtyMembers(PropertyInfo propertyInfo, ArrayList list)
		{
			if (IsDirty)
				list.Add(propertyInfo);
		}

		#endregion

		#region IPrintDebugState Members

		public void PrintDebugState(PropertyInfo propertyInfo, ref string str)
		{
			str += string.Format("{0,-20} {1} {2,-80}\r\n",
				propertyInfo.Name, IsDirty? "*": " ", _current != null? _current.OuterXml: "(null)");
		}

		#endregion

		#region ISetParent Members

		public void SetParent(object parent, PropertyInfo propertyInfo)
		{
			_parent       = parent as IPropertyChanged;
			_propertyInfo = propertyInfo;
		}

		#endregion

		#region Inner types

		private class XmlNodeTrackBack
		{
			public readonly XmlNode              Node;
			public readonly XmlNodeChangedAction Action;
			public readonly object               Value;

			public XmlNodeTrackBack(XmlNodeChangedEventArgs ea)
			{
				Node   = ea.Node;
				Action = ea.Action;
				switch(ea.Action)
				{
					case XmlNodeChangedAction.Insert:
						Value = ea.NewParent;
						break;
					case XmlNodeChangedAction.Remove:
						Value = ea.OldParent;
						break;
					case XmlNodeChangedAction.Change:
						Value = ea.OldValue;
						break;
					default:
						throw new ArgumentOutOfRangeException("ea", ea.Action, string.Format("Unknown XmlNodeChangedAction"));
				}
			}
		}

		#endregion

	}
}
