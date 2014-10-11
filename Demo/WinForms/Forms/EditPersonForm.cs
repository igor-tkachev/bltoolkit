using System;

using BLToolkit.Demo.ObjectModel;

namespace BLToolkit.Demo.Forms
{
	public partial class EditPersonForm : EditPersonFormBase
	{
		public EditPersonForm()
		{
			InitializeComponent();
		}

		protected override void SetBizEntity(Person person)
		{
			personBinder.Object = person;
		}
	}

	// This additional inheritance is required to avoid a WinForms bug,
	// which does not allow using generic classes as base classes for forms and uer controls.
	//
	public class EditPersonFormBase : BizEntityForm<EditPersonForm, Person> {}
}
