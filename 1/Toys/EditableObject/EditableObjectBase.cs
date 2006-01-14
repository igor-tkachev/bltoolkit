using System;
using System.Collections;
using System.ComponentModel;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.EditableObject
{
	[MapAction(typeof(IEditable))]
#if VER2
	[MapType(typeof(byte),     typeof(EditableValue<byte>))]
	[MapType(typeof(char),     typeof(EditableValue<char>))]
	[MapType(typeof(ushort),   typeof(EditableValue<ushort>))]
	[MapType(typeof(uint),     typeof(EditableValue<uint>))]
	[MapType(typeof(ulong),    typeof(EditableValue<ulong>))]
	[MapType(typeof(bool),     typeof(EditableValue<bool>))]
	[MapType(typeof(sbyte),    typeof(EditableValue<sbyte>))]
	[MapType(typeof(short),    typeof(EditableValue<short>))]
	[MapType(typeof(int),      typeof(EditableValue<int>))]
	[MapType(typeof(long),     typeof(EditableValue<long>))]
	[MapType(typeof(float),    typeof(EditableValue<float>))]
	[MapType(typeof(double),   typeof(EditableValue<double>))]
	[MapType(typeof(string),   typeof(EditableValue<string>), "")]
	[MapType(typeof(DateTime), typeof(EditableValue<DateTime>))]
	[MapType(typeof(decimal),  typeof(EditableValue<decimal>))]
	[MapType(typeof(Guid),     typeof(EditableValue<Guid>))]
#else
	[MapType(typeof(byte),     typeof(EditableValue), (byte)0)]
	[MapType(typeof(char),     typeof(EditableValue), (char)0)]
	[MapType(typeof(ushort),   typeof(EditableValue), (ushort)0)]
	[MapType(typeof(uint),     typeof(EditableValue), (uint)0)]
	[MapType(typeof(ulong),    typeof(EditableValue), (ulong)0)]
	[MapType(typeof(bool),     typeof(EditableValue), false)]
	[MapType(typeof(sbyte),    typeof(EditableValue), (sbyte)0)]
	[MapType(typeof(short),    typeof(EditableValue), (short)0)]
	[MapType(typeof(int),      typeof(EditableValue), (int)0)]
	[MapType(typeof(long),     typeof(EditableValue), (long)0)]
	[MapType(typeof(float),    typeof(EditableValue), (float)0)]
	[MapType(typeof(double),   typeof(EditableValue), (double)0)]
	[MapType(typeof(string),   typeof(EditableValue), "")]
	[MapType(typeof(DateTime), typeof(EditableDateTime))]
	[MapType(typeof(decimal),  typeof(EditableDecimal))]
	[MapType(typeof(Guid),     typeof(EditableGuid))]
#endif
	public abstract class EditableObjectBase :
		ISupportInitialize, IEditableObject, INotifyPropertyChanged,
		IMapNotifyPropertyChanged
	{
		#region Protected Members

		protected virtual void OnPropertyChanged(MapPropertyInfo pi)
		{
			OnPropertyChanged(pi.PropertyName);
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region IEditable

		public virtual void AcceptChanges()
		{
			if (this is IEditable)
			{
				((IEditable)this).AcceptChanges();
				_editingCount = 0;
			}
		}

		public virtual void RejectChanges()
		{
			if (this is IEditable)
			{
				((IEditable)this).RejectChanges();
				_editingCount = 0;
			}
		}

		public virtual void AcceptChanges(string memberName)
		{
			((IEditable)this).AcceptChanges(memberName, null);
		}

		public virtual void RejectChanges(string memberName)
		{
			((IEditable)this).RejectChanges(memberName, null);
		}

		[MapIgnore]
		[Bindable(false)]
		public virtual bool IsDirty
		{
			get
			{
				if (this is IEditable)
					return ((IEditable)this).IsDirty;

				return false;
			}
		}

		public virtual bool IsDirtyMember(string memberName)
		{
			bool isDirty = false;

			return ((IEditable)this).IsDirtyMember(memberName, null, ref isDirty) && isDirty;
		}

		public virtual ArrayList GetDirtyMembers()
		{
			ArrayList list = new ArrayList();

			((IEditable)this).GetDirtyMembers(null, list);

			return list;
		}

		[MapIgnore, Bindable(false)]
		public string DebugState
		{
			get
			{
#if DEBUG
				string s = string.Format(
					"====== {0} ======\r\nIsDirty: {1}\r\n" + 
					"Property            IsDirty Original                                 Current\r\n" +
					"========================= = ======================================== ========================================\r\n",
					GetType().Name, IsDirty);

				((IEditable)this).PrintDebugState(null, ref s);

				return s + "\r\n";
#else
				return "";
#endif
			}
		}

		#endregion

		#region ISupportInitialize Members

		private   bool _isInInit;
		protected bool  IsInInit
		{
			get { return _isInInit; }
		}

		public virtual void BeginInit()
		{
			_isInInit = true;
		}

		public virtual void EndInit()
		{
			AcceptChanges();

			_isInInit = false;
		}

		#endregion

		#region IEditableObject Members

		[NonSerialized]
		private int _editingCount;

		public virtual void BeginEdit()
		{
			_editingCount++;
		}

		public virtual void CancelEdit()
		{
			_editingCount--;

			if (_editingCount == 0)
				RejectChanges();
		}

		public virtual void EndEdit()
		{
			_editingCount--;

			if (_editingCount == 0)
				AcceptChanges();
		}

		#endregion

		#region IMapNotifyPropertyChanged Members

		void IMapNotifyPropertyChanged.PropertyChanged(MapPropertyInfo pi)
		{
			if (_isInInit == false && _notifyChanges)
				OnPropertyChanged(pi);
		}

		private bool _notifyChanges = true;
		[MapIgnore]
		[Bindable(false)]
		public bool NotifyChanges
		{
			get { return _notifyChanges;  }
			set { _notifyChanges = value; }
		}

		#endregion

		#region INotifyPropertyChanged Members

		public virtual event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
