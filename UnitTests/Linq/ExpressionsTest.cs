using System;
using System.Linq;
using System.Linq.Expressions;

using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class ExpressionsTest : TestBase
	{
		static int Count1(Parent p) { return p.Children.Count(c => c.ChildID > 0); }

		[Test]
		public void MapMember1()
		{
			Expressions.MapMember<Parent,int>(p => Count1(p), p => p.Children.Count(c => c.ChildID > 0));

			ForEachProvider(db => AreEqual(Parent.Select(p => Count1(p)), db.Parent.Select(p => Count1(p))));
		}

		static int Count2(Parent p, int id) { return p.Children.Count(c => c.ChildID > id); }

		[Test]
		public void MapMember2()
		{
			Expressions.MapMember<Parent,int,int>((p,id) => Count2(p, id), (p, id) => p.Children.Count(c => c.ChildID > id));

			ForEachProvider(db => AreEqual(Parent.Select(p => Count2(p, 1)), db.Parent.Select(p => Count2(p, 1))));
		}

		static int Count3(Parent p, int id) { return p.Children.Count(c => c.ChildID > id) + id; }

		[Test]
		public void MapMember3()
		{
			Expressions.MapMember<Parent,int,int>((p,id) => Count3(p, id), (p, id) => p.Children.Count(c => c.ChildID > id) + id);

			ForEachProvider(new[] { ProviderName.SqlCe }, db => AreEqual(Parent.Select(p => Count3(p, 2)), db.Parent.Select(p => Count3(p, 2))));
		}

		[MethodExpression("Count4Expression")]
		static int Count4(Parent p, int id, int n)
		{
			return (_count4Expression ?? (_count4Expression = Count4Expression().Compile()))(p, id, n);
		}

		static Func<Parent,int,int,int> _count4Expression;

		static Expression<Func<Parent,int,int,int>> Count4Expression()
		{
			return (p, id, n) => p.Children.Count(c => c.ChildID > id) + n;
		}

		[Test]
		public void MethodExpression1()
		{
			ForEachProvider(db => AreEqual(
				   Parent.Select(p => Count4(p, 3, 4)),
				db.Parent.Select(p => Count4(p, 3, 4))));
		}
	}
}
