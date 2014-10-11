using System;

using BLToolkit.ComponentModel;

using BLToolkit.Demo.ObjectModel;

namespace BLToolkit.Demo.Forms.ObjectViews
{
	public class PersonView : IObjectView
	{
		#region IObjectView Members

		private Person _person;
		object IObjectView.Object
		{
			get { return _person; }
			set { _person = (Person)value; }
		}

		#endregion

		public string FormTitle
		{
			get { return _person.ID > 0? _person.FullName: "New Person"; }
		}
	}
}
