using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Demo.WebServices.Client.WebClient;
using Demo.WebServices.ObjectModel;

namespace Demo.WebServices.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			WebClientBase.BaseUrl = args.Length == 0? "localhost": args[0];

			foreach (Person p in PersonClient.Instance.SelectAll())
			{
				PrintPerson(p);
			}

			XmlMap<string, Person> map = PersonClient.Instance.SelectMap();
			foreach (KeyValuePair<string, Person> pair in map)
			{
				Console.WriteLine("{0}: {1} {2} ({3})",
					pair.Key,
					pair.Value.FirstName, pair.Value.LastName, pair.Value.Gender);
			}

			// Async call to server
			//
			PersonClient.Instance.SelectByKey(1, PrintPerson);

			string strVal;
			Guid   guidVal;
			int intVal = PersonClient.Instance.MethodWithOutParams(out strVal, out guidVal);
			Console.WriteLine("int: {0}, str: {1}, guid: {2}", intVal, strVal, guidVal);

			PersonClient.Instance.MethodWithOutParams(delegate(int i, string s, Guid g)
			{
				Console.WriteLine("[Callback] int: {0}, str: {1}, guid: {2}", i, s, g);
			});

			Console.WriteLine();
			Console.WriteLine("Press [Enter] key to continue");
			Console.WriteLine();

			while (!Console.KeyAvailable)
			{
				Application.DoEvents();
				Thread.Sleep(200);
			}
		}

		private static void PrintPerson(Person p)
		{
			Console.WriteLine("{0} {1} ({2})", p.FirstName, p.LastName, p.Gender);
		}
	}
}