using System;

using BLToolkit.Demo.ObjectModel;

namespace BLToolkit.Demo.Forms
{
	public partial class EditPersonForm : BizEntityForm, IBizEntityForm<Person>
	{
		public EditPersonForm()
		{
			InitializeComponent();
		}

		public void SetBizEntity(Person person)
		{
			personBinder.Object = person;
		}
	}
}
