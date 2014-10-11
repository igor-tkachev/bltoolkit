#if FW2

namespace System
{
	public delegate void Action();

	public delegate void Action<T1, T2>(T1 arg1, T2 arg2);

	public delegate void Action<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

	public delegate void Action<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

	public delegate TResult Func<TResult>();

	public delegate TResult Func<T1, TResult>(T1 arg1);

	public delegate TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2);

	public delegate TResult Func<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3);

	public delegate TResult Func<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
}

namespace System.Collections.Specialized
{
	public interface INotifyCollectionChanged
	{
		event NotifyCollectionChangedEventHandler CollectionChanged;
	}

	/// <summary>
	/// This enum describes the action that caused a CollectionChanged event.
	/// </summary>
	public enum NotifyCollectionChangedAction
	{
		/// <summary> One or more items were added to the collection. </summary> 
		Add,
		/// <summary> One or more items were removed from the collection. </summary>
		Remove,
		/// <summary> One or more items were replaced in the collection. </summary>
		Replace,
		/// <summary> One or more items were moved within the collection. </summary>
		Move,
		/// <summary> The contents of the collection changed dramatically. </summary>
		Reset,
	}

	/// <summary> 
	/// Arguments for the CollectionChanged event. A collection that supports
	/// INotifyCollectionChangedThis raises this event whenever an item is added or
	/// removed, or when the contents of the collection changes dramatically. 
	/// </summary>
	public class NotifyCollectionChangedEventArgs : EventArgs
	{
		#region .ctors

		/// <summary>
		/// Construct a NotifyCollectionChangedEventArgs that describes a reset change. 
		/// </summary> 
		/// <param name="action">The action that caused the event (must be Reset).</param>
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
			: this(action, null, -1)
		{
		}

		/// <summary>
		/// Construct a NotifyCollectionChangedEventArgs that describes a one-item change. 
		/// </summary>
		/// <param name="action">The action that caused the event; can only be Reset, Add or Remove action.</param>
		/// <param name="changedItem">The item affected by the change.</param>
		public NotifyCollectionChangedEventArgs(
			NotifyCollectionChangedAction action,
			object                        changedItem)
			: this(action, changedItem, -1)
		{
		}

		/// <summary>
		/// Construct a NotifyCollectionChangedEventArgs that describes a one-item change. 
		/// </summary> 
		/// <param name="action">The action that caused the event.</param>
		/// <param name="changedItem">The item affected by the change.</param> 
		/// <param name="index">The index where the change occurred.</param>
		public NotifyCollectionChangedEventArgs(
			NotifyCollectionChangedAction action,
			object                        changedItem,
			int                           index)
			: this(action, changedItem == null? null: new[]{ changedItem }, index)
		{
		}

		/// <summary>
		/// Construct a NotifyCollectionChangedEventArgs that describes a multi-item change. 
		/// </summary> 
		/// <param name="action">The action that caused the event.</param>
		/// <param name="changedItems">The items affected by the change.</param> 
		public NotifyCollectionChangedEventArgs(
			NotifyCollectionChangedAction action,
			IList                         changedItems)
			: this(action, changedItems, -1)
		{
		}

		/// <summary>
		/// Construct a NotifyCollectionChangedEventArgs that describes a multi-item change (or a reset). 
		/// </summary> 
		/// <param name="action">The action that caused the event.</param>
		/// <param name="changedItems">The items affected by the change.</param> 
		/// <param name="startingIndex">The index where the change occurred.</param>
		public NotifyCollectionChangedEventArgs(
			NotifyCollectionChangedAction action,
			IList                         changedItems,
			int                           startingIndex)
		{
			_action = action;

			if (action == NotifyCollectionChangedAction.Add)
			{
				_newItems = changedItems;
				_newStartingIndex = startingIndex;
			}
			else if (action == NotifyCollectionChangedAction.Remove)
			{
				_oldItems = changedItems;
				_oldStartingIndex = startingIndex;
			}
		}

		/// <summary>
		/// Construct a NotifyCollectionChangedEventArgs that describes a one-item Replace event.
		/// </summary>
		/// <param name="action">Can only be a Replace action.</param> 
		/// <param name="newItem">The new item replacing the original item.</param>
		/// <param name="oldItem">The original item that is replaced.</param> 
		public NotifyCollectionChangedEventArgs(
			NotifyCollectionChangedAction action,
			object                        newItem,
			object                        oldItem)
			: this(action, new[] { newItem }, new[] { oldItem }, -1)
		{
		}

		/// <summary> 
		/// Construct a NotifyCollectionChangedEventArgs that describes a one-item Replace event. 
		/// </summary>
		/// <param name="action">Can only be a Replace action.</param> 
		/// <param name="newItem">The new item replacing the original item.</param>
		/// <param name="oldItem">The original item that is replaced.</param>
		/// <param name="index">The index of the item being replaced.</param>
		public NotifyCollectionChangedEventArgs(
			NotifyCollectionChangedAction action,
			object                        newItem,
			object                        oldItem,
			int                           index)
			: this(action, new[] { newItem }, new[] { oldItem }, index)
		{
		}

		/// <summary>
		/// Construct a NotifyCollectionChangedEventArgs that describes a multi-item Replace event. 
		/// </summary>
		/// <param name="action">Can only be a Replace action.</param> 
		/// <param name="newItems">The new items replacing the original items.</param> 
		/// <param name="oldItems">The original items that are replaced.</param>
		public NotifyCollectionChangedEventArgs(
			NotifyCollectionChangedAction action,
			IList                         newItems,
			IList                         oldItems)
			: this(action, newItems, oldItems, -1)
		{
		}

		/// <summary>
		/// Construct a NotifyCollectionChangedEventArgs that describes a multi-item Replace event. 
		/// </summary>
		/// <param name="action">Can only be a Replace action.</param> 
		/// <param name="newItems">The new items replacing the original items.</param> 
		/// <param name="oldItems">The original items that are replaced.</param>
		/// <param name="startingIndex">The starting index of the items being replaced.</param> 
		public NotifyCollectionChangedEventArgs(
			NotifyCollectionChangedAction action,
			IList                         newItems,
			IList                         oldItems,
			int                           startingIndex)
		{
			_action = action;

			_newItems = newItems;
			_newStartingIndex = startingIndex;
			_oldItems = oldItems;
			_oldStartingIndex = startingIndex;
		}

		/// <summary> 
		/// Construct a NotifyCollectionChangedEventArgs that describes a one-item Move event.
		/// </summary> 
		/// <param name="action">Can only be a Move action.</param> 
		/// <param name="changedItem">The item affected by the change.</param>
		/// <param name="index">The new index for the changed item.</param> 
		/// <param name="oldIndex">The old index for the changed item.</param>
		public NotifyCollectionChangedEventArgs(
			NotifyCollectionChangedAction action,
			object                        changedItem,
			int                           index,
			int                           oldIndex)
			: this(action, new[]{changedItem}, index, oldIndex)
		{
		}

		/// <summary> 
		/// Construct a NotifyCollectionChangedEventArgs that describes a multi-item Move event.
		/// </summary> 
		/// <param name="action">The action that caused the event.</param> 
		/// <param name="changedItems">The items affected by the change.</param>
		/// <param name="index">The new index for the changed items.</param> 
		/// <param name="oldIndex">The old index for the changed items.</param>
		public NotifyCollectionChangedEventArgs(
			NotifyCollectionChangedAction action,
			IList                         changedItems,
			int                           index,
			int                           oldIndex)
		{
			_action = action;

			_newItems = changedItems;
			_newStartingIndex = index;
			_oldItems = changedItems;
			_oldStartingIndex = oldIndex;
		}

		#endregion

		#region Properties

		private readonly NotifyCollectionChangedAction _action;
		/// <summary>
		/// The action that caused the event. 
		/// </summary>
		public NotifyCollectionChangedAction Action
		{
			get { return _action; }
		}

		private readonly IList _newItems;
		/// <summary>
		/// The items affected by the change.
		/// </summary> 
		public IList NewItems
		{
			get { return _newItems; }
		}

		private readonly IList _oldItems;
		/// <summary>
		/// The old items affected by the change (for Replace events).
		/// </summary>
		public IList OldItems
		{
			get { return _oldItems; }
		}

		private readonly int _newStartingIndex = -1;
		/// <summary> 
		/// The index where the change occurred.
		/// </summary>
		public int NewStartingIndex
		{
			get { return _newStartingIndex; }
		}

		private readonly int _oldStartingIndex = -1;
		/// <summary>
		/// The old index where the change occurred (for Move events). 
		/// </summary>
		public int OldStartingIndex
		{
			get { return _oldStartingIndex; }
		}

		#endregion
	}

	/// <summary>
	/// The delegate to use for handlers that receive the CollectionChanged event.
	/// </summary>
	public delegate void NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e);
}

namespace System.Runtime.CompilerServices
{
	internal class ExtensionAttribute : Attribute {}
}

#endif