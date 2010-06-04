using BLToolkit.TypeBuilder;

namespace BLToolkit.EditableObjects
{
	public interface IEditable
	{
		void AcceptChanges();
		void RejectChanges();
		bool IsDirty { [return: ReturnIfTrue] get; }
	}
}
