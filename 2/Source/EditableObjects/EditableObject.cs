using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace BLToolkit.EditableObjects
{
	[ImplementInterface(typeof(IEditable))]
#if FW21
	[GlobalInstanceType(typeof(byte),     typeof(EditableValue<byte>))]
	[GlobalInstanceType(typeof(char),     typeof(EditableValue<char>))]
	[GlobalInstanceType(typeof(ushort),   typeof(EditableValue<ushort>))]
	[GlobalInstanceType(typeof(uint),     typeof(EditableValue<uint>))]
	[GlobalInstanceType(typeof(ulong),    typeof(EditableValue<ulong>))]
	[GlobalInstanceType(typeof(bool),     typeof(EditableValue<bool>))]
	[GlobalInstanceType(typeof(sbyte),    typeof(EditableValue<sbyte>))]
	[GlobalInstanceType(typeof(short),    typeof(EditableValue<short>))]
	[GlobalInstanceType(typeof(int),      typeof(EditableValue<int>))]
	[GlobalInstanceType(typeof(long),     typeof(EditableValue<long>))]
	[GlobalInstanceType(typeof(float),    typeof(EditableValue<float>))]
	[GlobalInstanceType(typeof(double),   typeof(EditableValue<double>))]
	[GlobalInstanceType(typeof(string),   typeof(EditableValue<string>), "")]
	[GlobalInstanceType(typeof(DateTime), typeof(EditableValue<DateTime>))]
	[GlobalInstanceType(typeof(decimal),  typeof(EditableValue<decimal>))]
	[GlobalInstanceType(typeof(Guid),     typeof(EditableValue<Guid>))]
#else
	[GlobalInstanceType(typeof(byte),     typeof(EditableValue), (byte)0)]
	[GlobalInstanceType(typeof(char),     typeof(EditableValue), (char)0)]
	[GlobalInstanceType(typeof(ushort),   typeof(EditableValue), (ushort)0)]
	[GlobalInstanceType(typeof(uint),     typeof(EditableValue), (uint)0)]
	[GlobalInstanceType(typeof(ulong),    typeof(EditableValue), (ulong)0)]
	[GlobalInstanceType(typeof(bool),     typeof(EditableValue), false)]
	[GlobalInstanceType(typeof(sbyte),    typeof(EditableValue), (sbyte)0)]
	[GlobalInstanceType(typeof(short),    typeof(EditableValue), (short)0)]
	[GlobalInstanceType(typeof(int),      typeof(EditableValue), (int)0)]
	[GlobalInstanceType(typeof(long),     typeof(EditableValue), (long)0)]
	[GlobalInstanceType(typeof(float),    typeof(EditableValue), (float)0)]
	[GlobalInstanceType(typeof(double),   typeof(EditableValue), (double)0)]
	[GlobalInstanceType(typeof(string),   typeof(EditableValue), "")]
//	[GlobalInstanceType(typeof(DateTime), typeof(EditableDateTime))]
//	[GlobalInstanceType(typeof(decimal),  typeof(EditableDecimal))]
//	[GlobalInstanceType(typeof(Guid),     typeof(EditableGuid))]
#endif
	public abstract class EditableObject : ISupportMapping, IPropertyChanged, INotifyPropertyChanged, IEditableObject
	{
		#region IEditable

		public virtual void AcceptChanges()
		{
			if (this is IEditable)
				((IEditable)this).AcceptChanges();
		}

		public virtual void RejectChanges()
		{
			if (this is IEditable)
				((IEditable)this).RejectChanges();
		}

		[MapIgnore]
		public virtual bool IsDirty
		{
			get { return this is IEditable? ((IEditable)this).IsDirty: false; }
		}

		public virtual void AcceptMemberChanges(string memberName)
		{
			if (this is IEditable)
				((IEditable)this).AcceptMemberChanges(null, memberName);
		}

		public virtual void RejectMemberChanges(string memberName)
		{
			if (this is IEditable)
				((IEditable)this).RejectMemberChanges(null, memberName);
		}

		public virtual bool IsDirtyMember(string memberName)
		{
			bool isDirty = false;

			if (this is IEditable)
				((IEditable)this).IsDirtyMember(null, memberName, ref isDirty);

			return isDirty;
		}

		public virtual PropertyInfo[] GetDirtyMembers()
		{
			ArrayList list = new ArrayList();

			if (this is IEditable)
				((IEditable)this).GetDirtyMembers(null, list);

			return (PropertyInfo[])list.ToArray(typeof(PropertyInfo));
		}

		public virtual string PrintDebugState
		{
			get
			{
#if DEBUG
				string s = string.Format(
					"====== {0} ======\r\nIsDirty: {1}\r\n" +
					"Property       IsDirty Original                                 Current\r\n" +
					"==================== = ======================================== ========================================\r\n",
					GetType().Name, IsDirty);

				if (this is IEditable)
					((IEditable)this).PrintDebugState(null, ref s);

				return s + "\r\n";
#else
				return "";
#endif
			}
		}

		#endregion

		#region ISupportMapping Members

		private bool _inMapping;
		public  bool  InMapping
		{
			get { return _inMapping; }
		}

		public void BeginMapping(InitContext initContext)
		{
			_inMapping = true;
		}

		public void EndMapping(InitContext initContext)
		{
			AcceptChanges();

			_inMapping = false;

			OnPropertyChanged("");
		}

		#endregion

		#region Notify Changes

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private bool _notifyChanges = true;
		[MapIgnore]
		public  bool  NotifyChanges
		{
			get { return _notifyChanges;  }
			set { _notifyChanges = value; }
		}

		#endregion

		#region IPropertyChanged Members

		void IPropertyChanged.OnPropertyChanged(PropertyInfo propertyInfo)
		{
			if (_inMapping == false && _notifyChanges)
				OnPropertyChanged(propertyInfo.Name);
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region IEditableObject Members

		public virtual void BeginEdit()
		{
		}

		public virtual void CancelEdit()
		{
			RejectChanges();
		}

		public virtual void EndEdit()
		{
			AcceptChanges();
		}

		#endregion
	}
}
