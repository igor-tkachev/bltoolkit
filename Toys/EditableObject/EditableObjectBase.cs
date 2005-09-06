using System;
using System.ComponentModel;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.EditableObject
{
	public abstract class EditableObjectBase :
		ISupportInitialize, IEditableObject, INotifyPropertyChanged,
		IMapNotifyPropertyChanged,
		IEditableTypeList
	{
		#region Protected Members

		protected virtual void OnPropertyChanged(MapPropertyInfo pi)
		{
			if (_isEditing == false && PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(pi.PropertyName));
		}

		#endregion

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

		#endregion

		#region ISupportInitialize Members

		private bool _isInInit;

		void ISupportInitialize.BeginInit()
		{
			_isInInit = true;
		}

		void ISupportInitialize.EndInit()
		{
			AcceptChanges();

			_isInInit = false;
		}

		#endregion

		#region IEditableObject Members

		private bool _isEditing;

		void IEditableObject.BeginEdit()
		{
			_isEditing = true;
		}

		void IEditableObject.CancelEdit()
		{
			RejectChanges();
			
			_isEditing = false;
		}

		void IEditableObject.EndEdit()
		{
			AcceptChanges();

			_isEditing = false;
		}

		#endregion

		#region IMapNotifyPropertyChanged Members

		void IMapNotifyPropertyChanged.PropertyChanged(MapPropertyInfo pi)
		{
			if (_isInInit == false)
				OnPropertyChanged(pi);
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
