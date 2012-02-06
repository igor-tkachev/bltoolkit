using System;
using System.Linq;

using Data.Linq;

namespace Test
{
	class Program
	{
		static void Main()
		{
			BLToolkit.Data.DbManager.AddDataProvider(typeof(BLToolkit.Data.DataProvider.MySqlDataProvider));
			
			using (var db = new TestDbManager("MySql"))
			{
				var ds = new IdlPatientSource(db);
				var t = "A";
				var query =
                    (ds.Persons().Select(y => y.Name))
                        .Concat(
                        	ds.Persons().Where(x => x.Name == t).Select(x => x.Name)
                    	);
				
				var list = query.ToList();
				
				foreach (var i in list)
					Console.WriteLine(i.ToString());
			}
		}
	}

	public struct ObjectId
	{
		public ObjectId(int value)
		{
			m_value = value;
		}

		private int m_value;

		public int Value
		{
			get { return m_value; }
			set { m_value = value; }
		}

		public static implicit operator int(ObjectId val)
		{
			return val.m_value;
		}
	}

	public struct NullableObjectId
	{
		public NullableObjectId(int? value)
		{
			m_value = value;
		}

		private int? m_value;

		public int? Value
		{
			get { return m_value; }
			set { m_value = value; }
		}

		public static implicit operator int?(NullableObjectId val)
		{
			return val.m_value;
		}
	}

	public class IdlPatient
	{
		public ObjectId Id { get; set; }
	}

	public class IdlPerson
	{
		public ObjectId Id { get; set; }
		public string Name { get; set; }
	}

	public class IdlGrandChild
	{
		public ObjectId ParentID { get; set; }
		public ObjectId ChildID { get; set; }
		public ObjectId GrandChildID { get; set; }
	}

	public class IdlPatientEx : IdlPatient
	{
		public IdlPerson Person { get; set; }
	}

	public class IdlPatientSource
	{
		private readonly ITestDataContext m_dc;

		public IdlPatientSource(ITestDataContext dc)
		{
			m_dc = dc;
		}

		public IQueryable<IdlGrandChild> GrandChilds()
		{
				return m_dc.GrandChild.Select(x => new IdlGrandChild
					{
						ChildID = new ObjectId {Value = x.ChildID.Value},
						GrandChildID = new ObjectId { Value = x.GrandChildID.Value },
						ParentID = new ObjectId { Value = x.ParentID.Value }
					});
		}

		public IQueryable<IdlPatient> Patients()
		{
			return m_dc.Patient.Select(x => new IdlPatient { Id = new ObjectId { Value = x.PersonID }, });
		}

		public IQueryable<IdlPerson> Persons()
		{
			return
				m_dc.Person.Select(
					x => new IdlPerson { Id = new ObjectId { Value = x.ID }, Name = x.FirstName, });
		}
	}
}
