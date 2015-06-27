using System;
using BLToolkit.Data.Linq;

using NUnit.Framework;

namespace Data.Exceptions
{
	using Linq;
	using Linq.Model;

	[TestFixture]
	public class DmlTest : TestBase
	{
		[Test, ExpectedException(typeof(LinqException))]
		public void InsertOrUpdate1()
		{
			try
			{
				ForEachProvider(
					new[] { "Northwind" },
					db =>
					db.Doctor.InsertOrUpdate(
						() => new Doctor
						{
							PersonID  = 10,
							Taxonomy = "....",
						},
						p => new Doctor
						{
							Taxonomy = "...",
						}));
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex.Message.StartsWith("InsertOrUpdate method requires the 'Doctor' table to have a primary key."));
				throw;
			}
		}

		[Test, ExpectedException(typeof(LinqException))]
		public void InsertOrUpdate2(/*[DataContexts("Northwind")] string config*/)
		{
			try
			{
				ForEachProvider(
					typeof(LinqException),
					new[] { "Northwind" },
					db =>
					db.Patient.InsertOrUpdate(
						() => new Patient
						{
							Diagnosis = "....",
						},
						p => new Patient
						{
							Diagnosis = "...",
						}));
			}
			catch (Exception ex)
			{
				Assert.IsTrue(ex.Message.StartsWith("InsertOrUpdate method requires the 'Patient.PersonID' field to be included in the insert setter."));
				throw;
			}
		}
	}
}
