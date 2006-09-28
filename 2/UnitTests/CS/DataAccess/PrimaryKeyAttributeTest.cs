using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace DataAccess
{
	[TestFixture]
	public class PrimaryKeyAttributeTest
	{
		public class Base
		{
			[PrimaryKey]
			public int ID;
		}

		public class Derived : Base
		{
			public new int ID
			{
				get { return base.ID;  }
				set { base.ID = value; }
			}
		}

		[Test]
		public void PrimaryKeyOverrideTest()
		{
			using (DbManager db = new DbManager())
			{
				SqlQuery q = new SqlQuery();

				MemberMapper[] personPrimaryKeys = q.GetKeyFieldList(db, typeof (Base));
				Assert.IsNotNull(personPrimaryKeys);
				Assert.AreEqual(1, personPrimaryKeys.Length);
				Assert.AreEqual("ID", personPrimaryKeys[0].MemberName);

				MemberMapper[] derivedPrimaryKeys = q.GetKeyFieldList(db, typeof (Derived));
				Assert.IsNotNull(derivedPrimaryKeys);
				Assert.AreEqual(0, derivedPrimaryKeys.Length);
			}
		}

		public class Base2
		{
			protected      int _ID;

			[PrimaryKey]
			public virtual int  ID
			{
				get { return _ID;  }
				set { _ID = value; }
			}
		}

		public class Derived2 : Base2
		{
			public override int ID
			{
				get { return _ID;  }
				set { _ID = value; }
			}
		}

		[Test]
		public void PrimaryKeyOverrideTest2()
		{
			using (DbManager db = new DbManager())
			{
				SqlQuery q = new SqlQuery();

				MemberMapper[] personPrimaryKeys = q.GetKeyFieldList(db, typeof (Base2));
				Assert.IsNotNull(personPrimaryKeys);
				Assert.AreEqual(1, personPrimaryKeys.Length);
				Assert.AreEqual("ID", personPrimaryKeys[0].MemberName);

				MemberMapper[] derivedPrimaryKeys = q.GetKeyFieldList(db, typeof (Derived2));
				Assert.IsNotNull(derivedPrimaryKeys);
				Assert.AreEqual(0, derivedPrimaryKeys.Length);
			}
		}

		public class Base3
		{
			[PrimaryKey]
			public int ID;
		}

		public class Derived3 : Base3
		{
			public new int ID;
		}

		[Test]
		public void PrimaryKeyOverrideTest3()
		{
			using (DbManager db = new DbManager())
			{
				SqlQuery q = new SqlQuery();

				MemberMapper[] personPrimaryKeys = q.GetKeyFieldList(db, typeof (Base3));
				Assert.IsNotNull(personPrimaryKeys);
				Assert.AreEqual(1, personPrimaryKeys.Length);
				Assert.AreEqual("ID", personPrimaryKeys[0].MemberName);

				MemberMapper[] derivedPrimaryKeys = q.GetKeyFieldList(db, typeof (Derived3));
				Assert.IsNotNull(derivedPrimaryKeys);
				Assert.AreEqual(0, derivedPrimaryKeys.Length);
			}
		}
	}
}
