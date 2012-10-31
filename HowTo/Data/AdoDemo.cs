using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

using NUnit.Framework;

namespace HowTo.Data
{
	[TestFixture]
	public class AdoDemo
	{
		// Typified definition of the Gender database field.
		//
		public enum Gender
		{
			Female,
			Male,
			Unknown,
			Other
		}

		// Business object.
		//
		public class Person
		{
			public int    ID         { get; set; }
			public string FirstName  { get; set; }
			public string MiddleName { get; set; }
			public string LastName   { get; set; }
			public /*[a]*/Gender/*[/a]*/ Gender     { get; set; }
		}

		// ADO.NET data access method.
		//
		public List<Person> /*[a]*/GetList/*[/a]*/(Gender gender)
		{
			// Map the typified parameter value to its database representation.
			//
			string paramValue = "";

			switch (gender)
			{
				case Gender.Female:  paramValue = "F"; break;
				case Gender.Male:    paramValue = "M"; break;
				case Gender.Unknown: paramValue = "U"; break;
				case Gender.Other:   paramValue = "O"; break;
			}

			// Read a database configuration string.
			//
			string cs = ConfigurationManager.ConnectionStrings["DemoConnection"].ConnectionString;

			// Create and open a database connection.
			//
			using (SqlConnection con = new SqlConnection(cs))
			{
				con.Open();

				// Create and initialize a Command object.
				//
				using (SqlCommand cmd = con.CreateCommand())
				{
					cmd.CommandText = "SELECT * FROM Person WHERE Gender = @gender";
					cmd.Parameters.AddWithValue("@gender", paramValue);

					// Execute query.
					//
					using (SqlDataReader rd = cmd.ExecuteReader())
					{
						List<Person> list = new List<Person>();

						while (rd.Read())
						{
							Person person = new Person();

							// Map a data reader row to a business object.
							//
							person.ID         = Convert.ToInt32 (rd["PersonID"]);
							person.FirstName  = Convert.ToString(rd["FirstName"]);
							person.MiddleName = Convert.ToString(rd["MiddleName"]);
							person.LastName   = Convert.ToString(rd["LastName"]);

							switch (rd["Gender"].ToString())
							{
								case "F": person.Gender = Gender.Female;  break;
								case "M": person.Gender = Gender.Male;    break;
								case "U": person.Gender = Gender.Unknown; break;
								case "O": person.Gender = Gender.Other;   break;
							}

							list.Add(person);
						}

						return list;
					}
				}
			}
		}

		[Test]
		public void Test()
		{
			List<Person> list = GetList(Gender.Male);
			Assert.Greater(list.Count, 0);
		}
	}
}
