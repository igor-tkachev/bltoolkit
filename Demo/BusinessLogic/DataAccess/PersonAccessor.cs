using System;

using BLToolkit.DataAccess;

using BLToolkit.Demo.ObjectModel;

namespace BLToolkit.Demo.BusinessLogic.DataAccess
{
	public abstract class PersonAccessor : AccessorBase<Person>
	{
		public static PersonAccessor CreateInstance()
		{
			return CreateInstance<PersonAccessor>();
		}
	}
}
