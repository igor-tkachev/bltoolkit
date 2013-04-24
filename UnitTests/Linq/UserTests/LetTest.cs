using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BLToolkit.Data.Linq;
using BLToolkit.Mapping;

using NUnit.Framework;

namespace Data.Linq.UserTests
{
	[TestFixture]
	public class LetTest : TestBase
	{
		class Table1
		{
			public int  Field3;
			public int? Field5;

			[Association(ThisKey="Field5", OtherKey="Field3", CanBeNull=true)]
			public Table1 Ref1 { get; set; }

			[Association(ThisKey="Field3", OtherKey="Field3", CanBeNull=true)]
			public List<Table3> Ref2 { get; set; }
		}

		class Table2
		{
			public int? Field6;

			[Association(ThisKey = "Field6", OtherKey = "Field6", CanBeNull = true)]
			public Table3 Ref3 { get; set; }
		}

		class Table3
		{
			public int? Field6;
			public int  Field3;
			public int  Field4;

			[Association(ThisKey="Field3", OtherKey="Field3", CanBeNull=true)]
			public Table1 Ref4 { get; set; }

			[Association(ThisKey="Field4", OtherKey="Field4", CanBeNull=true)]
			public Table7 Ref5 { get; set; }

			[Association(ThisKey = "Field6", OtherKey = "Field6", CanBeNull = true)]
			public List<Table2> Ref9 { get; set; }
		}

		class Table7
		{
			public int    Field4;
			public string Field8;
		}

		[MethodExpression("GetSubQueryExpression")]
		static IEnumerable<Table2> GetSubQuery(Table2 t2)
		{
			throw new NotImplementedException();
		}

		private static Expression<Func<Table2,IEnumerable<Table2>>> GetSubQueryExpression()
		{
			return t1 =>
				from t2 in t1.Ref3.Ref4.Ref1.Ref2
				let  t3 = t1.Ref3
				where t3.Ref5.Field8 == t2.Ref5.Field8
				from t4 in t2.Ref9
				select t4;
		}

		[Test]
		public void LetTest1()
		{
			using (var repository = new TestDbManager())
			{
				var q =
					from t1 in repository.GetTable<Table2>()
					from t2 in GetSubQuery(t1)
					select t1;

				var linqResult = q.ToString();
			}
		}

		[MethodExpression("GetSubQueryExpression1")]
		static IEnumerable<Table2> GetSubQuery1(Table2 t2)
		{
			throw new NotImplementedException();
		}

		private static Expression<Func<Table2,IEnumerable<Table2>>> GetSubQueryExpression1()
		{
			return t1 =>
				from t2 in t1.Ref3.Ref4.Ref1.Ref2
				let  t3 = t1.Ref3
				where t3.Ref5 == t2.Ref5
				from t4 in t2.Ref9
				select t4;
		}

		[Test]
		public void LetTest2()
		{
			using (var repository = new TestDbManager())
			{
				var q =
					from t1 in repository.GetTable<Table2>()
					from t2 in GetSubQuery1(t1)
					select t1;

				var linqResult = q.ToString();
			}
		}
	}
}
