using System;
using System.Collections.Generic;

namespace Demo.WebServices.ObjectModel
{
	public interface IDataAccessor
	{
		Person                 SelectByKey(int id);
		List<Person>           SelectAll();
		XmlMap<string, Person> SelectMap();

		int MethodWithOutParams(out string str, out Guid guid);
	}
}