using System;
using System.Threading;
using System.Windows.Forms;

namespace Demo.WebServices.Client
{
	using ObjectModel;
	using WebClient;

	class Program
	{
		static void Main(string[] args)
		{
			WebClientBase.BaseUrl = args.Length == 0? "localhost": args[0];

			foreach (var p in PersonClient.Instance.SelectAll())
			{
				PrintPerson(p);
			}

			var map = PersonClient.Instance.SelectMap();

			foreach (var pair in map)
			{
				Console.WriteLine("{0}: {1} {2} ({3})",
					pair.Key,
					pair.Value.FirstName,
					pair.Value.LastName,
					pair.Value.Gender);
			}

			// Async call to server
			//
			PersonClient.Instance.SelectByKey(1, PrintPerson);

			string strVal;
			Guid   guidVal;
			var    intVal = PersonClient.Instance.MethodWithOutParams(out strVal, out guidVal);

			Console.WriteLine("int: {0}, str: {1}, guid: {2}", intVal, strVal, guidVal);

			PersonClient.Instance.MethodWithOutParams(
				(i,s,g) => Console.WriteLine("[Callback] int: {0}, str: {1}, guid: {2}", i, s, g));

			Console.WriteLine();
			Console.WriteLine("Press [Enter] key to continue");
			Console.WriteLine();

			while (!Console.KeyAvailable)
			{
				Application.DoEvents();
				Thread.Sleep(200);
			}

			Console.ReadKey(true);
		}

		private static void PrintPerson(Person p)
		{
			Console.WriteLine("{0} {1} ({2})", p.FirstName, p.LastName, p.Gender);
		}
	}
}
