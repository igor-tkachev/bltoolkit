using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Mapping;

namespace HowTo.Data
{
	[TestFixture]
	public class CompareDbManagerWithAdo
	{
		public enum Gender
		{
			[MapValue("F")] Female,
			[MapValue("M")] Male,
			[MapValue("U")] Unknown,
			[MapValue("O")] Other
		}

		public class Person
		{
			[MapField("PersonID")]
			public int    ID;
			public string FirstName;
			public string MiddleName;
			public string LastName;
			public Gender Gender;
		}

		public List<Person> AdoDemo(Gender gender)
		{
			string cs = ConfigurationManager.ConnectionStrings["Demo"].ConnectionString;

			using (SqlConnection con = new SqlConnection(cs))
			{
				con.Open();

				using (SqlCommand cmd = con.CreateCommand())
				{
					cmd.CommandText = "SELECT * FROM Person WHERE Gender = @gender";

					string p = "";

					switch (gender)
					{
						case Gender.Female:  p = "F"; break;
						case Gender.Male:    p = "M"; break;
						case Gender.Unknown: p = "U"; break;
						case Gender.Other:   p = "O"; break;
					}

					cmd.Parameters.AddWithValue("@gender", p);

					using (SqlDataReader rd = cmd.ExecuteReader())
					{
						List<Person> list = new List<Person>();

						while (rd.Read())
						{
							Person person = new Person();

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
		public void AdoDemo()
		{
			List<Person> list = AdoDemo(Gender.Male);

			Assert.Greater(list.Count, 0);
		}

		public List<Person> BLToolkitDemo(Gender gender)
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand(@"SELECT * FROM Person WHERE Gender = @gender",
						db.Parameter("@gender", Map.EnumToValue(gender)))
					.ExecuteList<Person>();
			}
		}

		[Test]
		public void BLToolkitDemo()
		{
			List<Person> list = BLToolkitDemo(Gender.Male);

			Assert.Greater(list.Count, 0);
		}
	}
}
