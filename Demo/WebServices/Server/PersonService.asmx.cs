using System;
using System.Collections.Generic;
using System.Web.Services;
using BLToolkit.DataAccess;

namespace Demo.WebServices.Server
{
	using ObjectModel;
	using WebServices;

	[GenerateWebService("http://tempuri.org/PersonService.asmx")]
	[GenerateXmlInclude(typeof(Person))]
	public abstract class PersonService: DataAccessor<Person>, IDataAccessor
	{
		[GenerateWebMethod]
		public abstract Person SelectByKey(int id);

		[GenerateWebMethod(true)]
		public abstract List<Person> SelectAll();

		[GenerateWebMethod, ActionName("SelectAll")]
		public abstract XmlMap<string, Person> SelectMap();

		[WebMethod]
		public int MethodWithOutParams(out string str, out Guid guid)
		{
			str = "string";
			guid = Guid.NewGuid();
			return 123;
		}
	}
}