using System;

using BLToolkit.Demo.BusinessLogic.DataAccess;
using BLToolkit.Demo.ObjectModel;

namespace BLToolkit.Demo.BusinessLogic
{
	public class PersonManager : ManagerBase<Person>
	{
		protected override AccessorBase<Person> DataAccessor
		{
			get { return PersonAccessor.CreateInstance(); }
		}
	}
}
