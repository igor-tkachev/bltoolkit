using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Linq
{
	class QueryContext
	{
		public class DbManagerContext
		{
			public DbManager DbManager;
			public bool      InUse;
		}

		public DbManager RootDbManager;
		public int       Counter;

		List<DbManagerContext> _contexts;

		public DbManagerContext GetDbManager()
		{
			if (_contexts == null)
			{
				RootDbManager.OnClosing += OnRootClosing;
				_contexts = new List<DbManagerContext>(1);
			}

			foreach (var context in _contexts)
			{
				if (!context.InUse)
				{
					context.InUse = true;
					return context;
				}
			}

			var ctx = new DbManagerContext { DbManager = RootDbManager.Clone(), InUse = true };

			_contexts.Add(ctx);

			return ctx;
		}

		public void ReleaseDbManager(DbManagerContext context)
		{
			context.InUse = false;
		}

		void OnRootClosing(object sender, EventArgs e)
		{
			foreach (var context in _contexts)
				context.DbManager.Dispose();

			RootDbManager.OnClosing -= OnRootClosing;

			_contexts = null;
		}

		public void AfterQuery()
		{
			Counter++;
		}
	}
}
