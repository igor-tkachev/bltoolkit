using System;
using System.Collections.Generic;
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
				var list = new GenericConcatQuery(db, new object[] { "A", 1 }).Query().ToList();
				
				foreach (var i in list)
					Console.WriteLine(i.ToString());
			}
		}
	}
	
	public abstract class GenericQueryBase
	{
		private readonly IdlPatientSource m_ds;

		protected GenericQueryBase(ITestDataContext ds)
		{
			m_ds = new IdlPatientSource(ds);
		}

		#region Object sources

		protected IQueryable<IdlPerson> AllPersons
		{
			get { return m_ds.Persons(); }
		}

		protected IQueryable<IdlPatient> AllPatients
		{
			get { return m_ds.Patients(); }
		}

		protected IQueryable<IdlGrandChild> AllGrandChilds
		{
			get { return m_ds.GrandChilds(); }
		}

		#endregion

		public abstract IEnumerable<object> Query();
	}

	public class GenericConcatQuery : GenericQueryBase
	{
		private System.String @p1;
		private System.Int32 @p2;

		public GenericConcatQuery(ITestDataContext ds, object[] args)
			: base(ds)
		{
			@p1 = (System.String)args[0];
			@p2 = (System.Int32)args[1];
		}

		public override IEnumerable<object> Query()
		{
			return (from y in AllPersons
					select y.Name)
						.Concat(
							from x in AllPersons
							from z in AllPatients
							where (x.Name == @p1 || z.Id == new ObjectId { Value = @p2 })
							select x.Name
						);
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
