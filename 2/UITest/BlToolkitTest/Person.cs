using System;
using System.Collections.Generic;
using System.Text;

namespace BlToolkitTest
{
	public class Person
	{
		private string _firstName;
		public  string  FirstName
		{
			get { return _firstName;  }
			set { _firstName = value; }
		}

		private string _lastName;
		public  string  LastName
		{
			get { return _lastName;  }
			set { _lastName = value; }
		}
	}
}
