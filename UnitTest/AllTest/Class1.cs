using System;
using System.Data.SqlTypes;

using NUnit.Framework;

using Rsdn.Framework.Data;
using Rsdn.Framework.Data.Mapping;

namespace AllTest
{
	public abstract class TestClass
	{
		public abstract string Name
		{
			get;
			//set;
		}

		private string _name1;
		public  string  Name1
		{
			get { return _name1;  }
			set { _name1 = value; }
		}

		public Type type
		{
			get { return typeof(TestClass); }
		}
	}

	public class TestClass2 : TestClass
	{
		private string _Name = "123";

		public override string Name
		{
			get { return _Name;  }
			//set { _Name = value; }
		}
	}

	[TestFixture]
	public class Test
	{
		[Test]
		public void AbstractProperty()
		{
			TestClass tc = (TestClass)Map.ToObject(new TestClass2(), typeof(TestClass));

			string s = tc.Name;

			Console.WriteLine(s);
		}
	}

	public class Dest
	{
		public SqlString m_str;
		public SqlInt32  m_int;
		public SqlGuid   m_guid;

		public virtual object GetValue1(object obj)
		{
			SqlInt32 i = ((Dest)obj).m_int;
			if (i.IsNull)
				return null;
			return i.Value;
		}

		public virtual object GetValue2(object obj)
		{
			SqlString s = ((Dest)obj).m_str;
			if (s.IsNull)
				return null;
			return s.Value;
		}

		public virtual void SetValue1(object obj, object value)
		{
			((Dest)obj).m_str = 
				value == null?      SqlString.Null:
				value is SqlString? (SqlString)value:
				new SqlString(Convert.ToString(value));
		}

		public virtual void SetValue2(object obj, object value)
		{
			((Dest)obj).m_int = 
				value == null?      SqlInt32.Null:
				value is SqlInt32? (SqlInt32)value:
				new SqlInt32(Convert.ToInt32(value));
		}

		public virtual void SetValue3(object obj, object value)
		{
			((Dest)obj).m_guid = 
				value == null || value is DBNull? SqlGuid.Null:
				value is SqlGuid? (SqlGuid)value:
				new SqlGuid(value is Guid? (Guid)value: new Guid(value.ToString()));
		}

		private static MapDescriptor _dd_MapDescriptor;
		private TestClass _tc;

		public Dest(MapInitializingData data)
		{
			if (_dd_MapDescriptor == null)
				_dd_MapDescriptor = MapDescriptor.GetDescriptor(typeof(TestClass));

			_tc = _dd_MapDescriptor.CreateInstanceEx(data) as TestClass;
		}
	}

	public class EditableString { public EditableString(ulong s, bool i, short d, long i64) {} }
	public class EditableSqlString {}

	[MapXml("mapping.xml", "category")]
	public abstract class Category
	{
		public abstract int ID { get; }

		static object[] param = new object[] { "123", 30000, 35, new SqlInt32(12345) };
		EditableString es;

		void Test(SqlInt64 i64)
		{
			es = new EditableString(
				100000000000000,
				true,
				-2,
				-3);
		}

		[MapType(typeof(EditableString))]
		public abstract string Name { get; set; }

		[MapType(typeof(EditableSqlString))]
		public abstract SqlString Description { get; set; }

		[MapIgnore]
		public int ItemCount
		{
			get { return GetItemCount(); }
		}

		int GetItemCount()
		{
				return 0; 
		}
	}
}
