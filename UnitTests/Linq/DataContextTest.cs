﻿using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.Linq;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class DataContextTest
	{
		[Test]
		public void TestContext()
		{
			var ctx = new DataContext("Sql2008");

			ctx.GetTable<Person>().ToList();

			ctx.KeepConnectionAlive = true;

			ctx.GetTable<Person>().ToList();
			ctx.GetTable<Person>().ToList();

			ctx.KeepConnectionAlive = false;

			using (var tran = new DataContextTransaction(ctx))
			{
				ctx.GetTable<Person>().ToList();

				tran.BeginTransaction();

				ctx.GetTable<Person>().ToList();
				ctx.GetTable<Person>().ToList();

				tran.CommitTransaction();
			}
		}
	}
}
