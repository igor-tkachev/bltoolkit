using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;

namespace Mono.Model
{
	[TableName("Parent")]
	public abstract class EditableParent : EditableObject<EditableParent>
	{
		public abstract int  ParentID { get; set; }
		public abstract int? Value1   { get; set; }
	}
}
