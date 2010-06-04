using System;
using System.Collections.Generic;
using System.Web.Services;

namespace Demo.WebServices.Client
{
	using WebClient;
	using ObjectModel;

	[WebServiceBinding(Namespace = "http://tempuri.org/PersonService.asmx")]
	[GenerateXmlInclude(typeof(Person))]
	public abstract class PersonClient: WebClientBase<PersonClient>, IDataAccessor
	{
		public abstract List<Person>           SelectAll();
		public abstract XmlMap<string, Person> SelectMap();
		public abstract Person                 SelectByKey(int id);
		public abstract void                   SelectByKey(int id, Action<Person> callback);

		public abstract int  MethodWithOutParams(out string str, out Guid guid);
		public abstract void MethodWithOutParams(Action<int, string, Guid> callback);
	}
}