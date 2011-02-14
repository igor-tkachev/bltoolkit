using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using BLToolkit.Data.Linq;
using BLToolkit.Data.Linq.Parser;
using BLToolkit.Data.Sql;
using Data.Linq.Model;
using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class ParserTest : TestBase
	{
		static ParserTest()
		{
			ExpressionParser.AddParser(new ContextParser());
		}

		#region IsExpressionTable

		[Test]
		public void IsExpressionTable1()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => p1.ParentID)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		[Test]
		public void IsExpressionTable2()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => p1.ParentID + 1)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		[Test]
		public void IsExpressionTable3()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => p1)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		#endregion

		#region IsExpressionScalar

		[Test]
		public void IsExpressionScalar1()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => p1.ParentID)
					.Select    (p2 => p2)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		[Test]
		public void IsExpressionScalar2()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => p1.ParentID + 1)
					.Select    (p2 => p2)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		[Test]
		public void IsExpressionScalar3()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => p1)
					.Select    (p2 => p2)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		[Test]
		public void IsExpressionScalar4()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => p1.ParentID + 1)
					.Where     (p3 => p3 == 1)
					.Select    (p2 => p2)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Expression));
				//Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		[Test]
		public void IsExpressionScalar5()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => p1)
					.Select    (p2 => p2.ParentID)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		[Test]
		public void IsExpressionScalar6()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Child
					.Select    (p => p.Parent)
					.Select    (p => p)
					.GetContext();

				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		[Test]
		public void IsExpressionScalar7()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Child
					.Select    (p => p)
					.Select    (p => p)
					.Select    (p => p.Parent)
					.GetContext();

				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		[Test]
		public void IsExpressionScalar8()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Child
					.Select    (p  => p)
					.Select    (p3 => new { p1 = new { p2 = new { p = p3 } } })
					.Select    (p  => p.p1.p2.p.Parent)
					.GetContext();

				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		[Test]
		public void IsExpressionScalar9()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Child
					.Select    (p  => p)
					.Select    (p3 => new { p1 = new { p2 = new { p = p3.Parent } } })
					.Select    (p  => p.p1.p2.p)
					.GetContext();

				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}


		[Test]
		public void IsExpressionScalar10()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Child
					.Select    (p => p)
					.Select    (p => new { p = new { p } })
					.Select    (p => p.p)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		[Test]
		public void IsExpressionScalar11()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Child
					.Select    (p => p)
					.Select    (p => new { p = new Child { ChildID = p.ChildID } })
					.Select    (p => p.p)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.SubQuery));
			}
		}

		#endregion

		#region IsExpressionSelect

		[Test]
		public void IsExpressionSelect1()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => new { p1.ParentID })
					.Select    (p2 => p2.ParentID)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void IsExpressionSelect2()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => new { p = p1.ParentID + 1 })
					.Select    (p2 => p2.p)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void IsExpressionSelect3()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => new { p1 })
					.Select    (p2 => p2.p1)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void IsExpressionSelect4()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => new { p = p1.ParentID + 1 })
					.Where     (p3 => p3.p == 1)
					.Select    (p2 => p2.p)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void IsExpressionSelect42()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => new { p = p1.ParentID + 1 })
					.Where     (p3 => p3.p == 1)
					.Select    (p2 => p2)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void IsExpressionSelect5()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => new { p1 })
					.Select    (p2 => p2.p1.ParentID)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void IsExpressionSelect6()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p => new { p })
					.Select    (p => p)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void IsExpressionSelect7()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Child
					.Select    (p => new { p, p.Parent })
					.Select    (p => new { p.Parent, p.p.ChildID })
					.Select    (p => p.Parent)
					.GetContext();

				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void IsExpressionSelect8()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Child
					.Select    (p => new { p, p.Parent })
					.Select    (p => new { p.Parent.ParentID, p.p.ChildID })
					.Select    (p => p.ParentID)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void IsExpressionSelect9()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.GrandChild
					.Select    (p => new { p, p.Child })
					.Select    (p => new { p.Child.Parent.ParentID, p.p.ChildID })
					.Select    (p => p.ParentID)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void IsExpressionSelect10()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p => p.Children.Max(c => (int?)c.ChildID) ?? p.Value1)
					.Select    (p => p)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		#endregion

		#region ConvertToIndexTable

		[Test]
		public void ConvertToIndexTable1()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent1
					.Select    (t => t)
					.GetContext();

				Assert.AreEqual(new[] { 0, 1 }, ctx.ConvertToIndex(null, 0, ConvertFlags.All));
				Assert.AreEqual(new[] { 0    }, ctx.ConvertToIndex(null, 0, ConvertFlags.Key));
			}
		}

		[Test]
		public void ConvertToIndexTable2()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (t => t)
					.GetContext();

				Assert.AreEqual(new[] { 0, 1 }, ctx.ConvertToIndex(null, 0, ConvertFlags.All));
				Assert.AreEqual(new[] { 0, 1 }, ctx.ConvertToIndex(null, 0, ConvertFlags.Key));
			}
		}

		[Test]
		public void ConvertToIndexTable3()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (t => t.ParentID)
					.GetContext();

				Assert.AreEqual(new[] { 0 }, ctx.ConvertToIndex(null, 0, ConvertFlags.Field));
			}
		}

		[Test]
		public void ConvertToIndexTable4()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (t => t.Value1)
					.GetContext();

				Assert.AreEqual(new[] { 0 }, ctx.ConvertToIndex(null, 0, ConvertFlags.Field));
			}
		}

		[Test]
		public void ConvertToIndexTable5()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (t => new { t = new { t } })
					.Select    (t => t.t.t.ParentID)
					.Select    (t => t)
					.GetContext();

				Assert.AreEqual(new[] { 0 }, ctx.ConvertToIndex(null, 0, ConvertFlags.Field));
			}
		}

		#endregion

		#region ConvertToIndexScalar

		[Test]
		public void ConvertToIndexScalar1()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => p1.ParentID)
					.Select    (p2 => p2)
					.GetContext();

				Assert.AreEqual(new[] { 0 }, ctx.ConvertToIndex(null, 0, ConvertFlags.Field));
			}
		}

		[Test]
		public void ConvertToIndexScalar2()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => p1.ParentID + 1)
					.Select    (p2 => p2)
					.GetContext();

				Assert.AreEqual(new[] { 0 }, ctx.ConvertToIndex(null, 0, ConvertFlags.Field));
			}
		}

		[Test]
		public void ConvertToIndexScalar3()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => p1.ParentID + 1)
					.Where     (p3 => p3 == 1)
					.Select    (p2 => p2)
					.GetContext();

				Assert.AreEqual(new[] { 0 }, ctx.ConvertToIndex(null, 0, ConvertFlags.Field));
			}
		}

		[Test]
		public void ConvertToIndexScalar4()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => new { p = new { p = p1.ParentID } })
					.Select    (p2 => p2.p.p)
					.GetContext();

				Assert.AreEqual(new[] { 0 }, ctx.ConvertToIndex(null, 0, ConvertFlags.Field));
			}
		}

		#endregion

		#region ConvertToIndexSelect

		[Test]
		public void ConvertToIndexSelect1()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => new { p1.ParentID })
					.Select    (p2 => p2.ParentID)
					.GetContext();

				var sql = ctx.ConvertToSql(null, 0, ConvertFlags.Field);

				Assert.AreEqual        (1, sql.Length);
				Assert.IsAssignableFrom(typeof(SqlField), sql[0]);
				Assert.AreEqual        ("ParentID", ((SqlField)sql[0].Sql).Name);
			}
		}

		[Test]
		public void ConvertToIndexSelect2()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => new { p = p1.ParentID + 1 })
					.Select    (p2 => p2.p)
					.GetContext();

				var sql = ctx.ConvertToSql(null, 0, ConvertFlags.Field);

				Assert.AreEqual        (1, sql.Length);
				Assert.IsAssignableFrom(typeof(SqlBinaryExpression), sql[0]);
			}
		}

		[Test]
		public void ConvertToIndexSelect3()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => new { p = p1.ParentID + 1 })
					.Where     (p3 => p3.p == 1)
					.Select    (p2 => p2.p)
					.GetContext();

				var sql = ctx.ConvertToSql(null, 0, ConvertFlags.Field);

				Assert.AreEqual        (1, sql.Length);
				Assert.IsAssignableFrom(typeof(SqlQuery.Column), sql[0]);
			}
		}

		[Test]
		public void ConvertToIndexSelect4()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p1 => new { p1 })
					.Select    (p2 => p2.p1.ParentID)
					.GetContext();

				var sql = ctx.ConvertToSql(null, 0, ConvertFlags.Field);

				Assert.AreEqual        (1, sql.Length);
				Assert.IsAssignableFrom(typeof(SqlField), sql[0]);
				Assert.AreEqual        ("ParentID", ((SqlField)sql[0].Sql).Name);
			}
		}

		[Test]
		public void ConvertToIndexSelect5()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Child
					.Select    (p => new { p, p.Parent })
					.Select    (p => new { p.Parent.ParentID, p.p.ChildID })
					.Select    (p => p.ParentID)
					.GetContext();

				var sql = ctx.ConvertToSql(null, 0, ConvertFlags.Field);

				Assert.AreEqual        (1, sql.Length);
				Assert.IsAssignableFrom(typeof(SqlField), sql[0]);
				Assert.AreEqual        ("ParentID", ((SqlField)sql[0].Sql).Name);
			}
		}

		[Test]
		public void ConvertToIndexSelect9()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.GrandChild
					.Select    (p => new { p, p.Child })
					.Select    (p => new { p.Child.Parent.ParentID, p.p.ChildID })
					.Select    (p => p.ParentID)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void ConvertToIndexSelect10()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Select    (p => p.Children.Max(c => (int?)c.ChildID) ?? p.Value1)
					.Select    (p => p)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void ConvertToIndexJoin1()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Join  (db.Child, p => p.ParentID, c => c.ParentID, (p, c) => new { p, c })
					.Where (t => t.c.ChildID > 20)
					.Select(t => t.p)
					.Select(p => p.ParentID)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse (ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		[Test]
		public void ConvertToIndexJoin2()
		{
			using (var db = new TestDbManager())
			{
				var ctx = db.Parent
					.Join  (db.Child, p => p.ParentID, c => c.ParentID, (p, c) => c)
					.Select(t => t)
					.Select(p => p.ParentID)
					.GetContext();

				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Association));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Object));
				Assert.IsTrue (ctx.IsExpression(null, 0, RequestFor.Field));
				Assert.IsFalse(ctx.IsExpression(null, 0, RequestFor.Expression));
			}
		}

		#endregion
	}

	class ContextParser : ISequenceParser
	{
		public int ParsingCounter { get; set; }

		public bool CanParse(ExpressionParser parser, Expression expression, SqlQuery sqlQuery)
		{
			var call = expression as MethodCallExpression;
			return call != null && call.Method.Name == "GetContext";
		}

		public IParseContext ParseSequence(ExpressionParser parser, Expression expression, SqlQuery sqlQuery)
		{
			var call = (MethodCallExpression)expression;
			return new Context(parser.ParseSequence(call.Arguments[0], sqlQuery));
		}

		public class Context : IParseContext
		{
			public Context(IParseContext sequence)
			{
				Sequence = sequence;
			}

			public IParseContext    Sequence   { get; set; }
			public ExpressionParser Parser     { get { return Sequence.Parser; } }
			public Expression       Expression { get; set; }
			public SqlQuery         SqlQuery   { get { return Sequence.SqlQuery; } }
			public IParseContext    Parent     { get; set; }

			public void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				query.GetElement = (ctx,db,expr,ps) => this;
			}

			public Expression BuildExpression(Expression expression, int level)
			{
				return Sequence.BuildExpression(expression, level);
			}

			public SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				return Sequence.ConvertToSql(expression, level, flags);
			}

			public SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				return Sequence.ConvertToIndex(expression, level, flags);
			}

			public bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				return Sequence.IsExpression(null,  0, requestFlag);
			}

			public IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				return Sequence.GetContext(expression, level, currentSql);
			}

			public int ConvertToParentIndex(int index, IParseContext context)
			{
				return Sequence.ConvertToParentIndex(index, context);
			}

			public void SetAlias(string alias)
			{
			}
		}
	}

	static class Extensions
	{
		public static ContextParser.Context GetContext<T>(this IQueryable<T> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			return source.Provider.Execute<ContextParser.Context>(
				Expression.Call(
					null,
					((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new[] { typeof(T) }),
					new[] { source.Expression }));
		}

		static public Expression Unwrap(this Expression ex)
		{
			if (ex == null)
				return null;

			switch (ex.NodeType)
			{
				case ExpressionType.Quote          :
				case ExpressionType.Convert        :
				case ExpressionType.ConvertChecked : return ((UnaryExpression)ex).Operand.Unwrap();
			}

			return ex;
		}
	}
}
