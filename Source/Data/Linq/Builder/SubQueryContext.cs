using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	using Data.Sql;

	class SubQueryContext : IBuildContext
	{
		public readonly IBuildContext SubQuery;

		public SubQueryContext(IBuildContext subQuery, SqlQuery sqlQuery, bool addToSql)
		{
			if (sqlQuery == subQuery.SqlQuery)
				throw new ArgumentException("Wrong subQuery argument.", "subQuery");

			SubQuery = subQuery;
			SubQuery.Parent = this;

			SqlQuery = sqlQuery;

			if (addToSql)
				SqlQuery.From.Table(SubQuery.SqlQuery);

			//_subQuery.SqlQuery.ParentSql = SqlQuery;
		}

		public SubQueryContext(IBuildContext subQuery, bool addToSql)
			: this(subQuery, new SqlQuery { ParentSql = subQuery.SqlQuery.ParentSql }, addToSql)
		{
		}

		public SubQueryContext(IBuildContext subQuery)
			: this(subQuery, true)
		{
		}

#if DEBUG
		public string _sqlQueryText { get { return SqlQuery == null ? "" : SqlQuery.SqlText; } }
#endif

		public ExpressionBuilder   Builder    { get { return SubQuery.Builder;     } }
		public Expression          Expression { get { return SubQuery.Expression; } }
		public SqlQuery            SqlQuery   { get; set; }
		public IBuildContext       Parent     { get; set; }

		public IBuildContext Union;

		public void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
		{
			SubQuery.BuildQuery(query, queryParameter);
		}

		public virtual Expression BuildExpression(Expression expression, int level)
		{
			return SubQuery.BuildExpression(expression, level);
		}

		public SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
		{
			return SubQuery
				.ConvertToIndex(expression, level, flags)
				.Select(idx => new SqlInfo { Sql = SubQuery.SqlQuery.Select.Columns[idx.Index], Member = idx.Member })
				.ToArray();
		}

		// JoinContext has similar logic. Consider to review it.
		//
		public virtual SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
		{
			return ConvertToSql(expression, level, flags)
				.Select(idx =>
				{
					idx.Query = SqlQuery;
					idx.Index = GetIndex((SqlQuery.Column)idx.Sql);

					return idx;
				})
				.ToArray();
		}

		public bool IsExpression(Expression expression, int level, RequestFor testFlag)
		{
			switch (testFlag)
			{
				case RequestFor.SubQuery : return true;
			}

			return SubQuery.IsExpression(expression, level, testFlag);
		}

		public virtual IBuildContext GetContext(Expression expression, int level, BuildInfo buildInfo)
		{
			return SubQuery.GetContext(expression, level, buildInfo);
		}

		readonly Dictionary<ISqlExpression,int> _indexes = new Dictionary<ISqlExpression,int>();

		private bool _checkUnion;

		int GetIndex(SqlQuery.Column column)
		{
			int idx;

			if (!_indexes.TryGetValue(column, out idx))
			{
				if (Union != null && !_checkUnion)
				{
					_checkUnion = true;

					var subSql   = SubQuery.ConvertToIndex(null, 0, ConvertFlags.All).OrderBy(_ => _.Index).ToList();
					var unionSql = Union.   ConvertToIndex(null, 0, ConvertFlags.All).OrderBy(_ => _.Index).ToList();
					var sub      = SubQuery.SqlQuery.Select.Columns;
					var union    = Union.   SqlQuery.Select.Columns;

					for (var i = 0; i < sub.Count; i++)
					{
						if (i >= subSql.Count || subSql[i].Index != i)
						{
							if (i < subSql.Count && subSql[i].Index < i)
								throw new InvalidOperationException();
							subSql.Insert(i, new SqlInfo { Index = i, Sql = sub[i].Expression });
						}
					}

					for (var i = 0; i < union.Count; i++)
					{
						if (i >= unionSql.Count || unionSql[i].Index != i)
						{
							if (i < unionSql.Count && unionSql[i].Index < i)
								throw new InvalidOperationException();
							unionSql.Insert(i, new SqlInfo { Index = i, Sql = union[i].Expression });
						}
					}

					var reorder = false;

					for (var i = 0; i < subSql.Count && i < unionSql.Count; i++)
					{
						if (subSql[i].Member != unionSql[i].Member)
						{
							reorder = true;

							var sm = subSql[i].Member;

							if (sm != null)
							{
								var um = unionSql.Select((s,n) => new { s, n }).Where(_ => _.s.Member == sm).FirstOrDefault();

								if (um != null)
								{
									unionSql.RemoveAt(um.n);
									unionSql.Insert(i, um.s);
								}
								else
								{
									if (unionSql[i].Member != null)
										unionSql.Insert(i, new SqlInfo());
								}
							}
							else
							{
								if (unionSql[i].Member != null)
									unionSql.Insert(i, new SqlInfo());
							}
						}
					}

					if (reorder)
					{
						var cols = union.ToList();

						union.Clear();

						foreach (var info in unionSql)
						{
							if (info.Index < 0)
								union.Add(new SqlQuery.Column(Union.SqlQuery, new SqlValue(null)));
							else
								union.Add(cols[info.Index]);
						}
					}

					while (sub.Count < union.Count) sub.  Add(new SqlQuery.Column(SubQuery.SqlQuery, new SqlValue(null)));
					while (union.Count < sub.Count) union.Add(new SqlQuery.Column(Union.   SqlQuery, new SqlValue(null)));
				}

				idx = SqlQuery.Select.Add(column);
				_indexes.Add(column, idx);

				if (Union != null)
					while (SubQuery.SqlQuery.Select.Columns.Count < Union.SqlQuery.Select.Columns.Count)
						Union.SqlQuery.Select.Columns.Add(new SqlQuery.Column(Union.SqlQuery, new SqlValue(null)));
			}

			return idx;
		}

		public int ConvertToParentIndex(int index, IBuildContext context)
		{
			var idx = GetIndex(context.SqlQuery.Select.Columns[index]);
			return Parent == null ? idx : Parent.ConvertToParentIndex(idx, this);
		}

		public void SetAlias(string alias)
		{
			if (alias.Contains('<'))
				return;

			if (SqlQuery.From.Tables[0].Alias == null)
				SqlQuery.From.Tables[0].Alias = alias;
		}

		public ISqlExpression GetSubQuery(IBuildContext context)
		{
			return null;
		}
	}
}
