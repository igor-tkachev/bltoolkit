using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	class SubQueryContext : IParseContext
	{
		public readonly IParseContext SubQuery;

		public SubQueryContext(IParseContext subQuery, SqlQuery sqlQuery, bool addToSql)
		{
			SubQuery = subQuery;
			SubQuery.Parent = this;

			SqlQuery = sqlQuery;

			if (addToSql)
				SqlQuery.From.Table(SubQuery.SqlQuery);

			//_subQuery.SqlQuery.ParentSql = SqlQuery;
		}

		public SubQueryContext(IParseContext subQuery, bool addToSql)
			: this(subQuery, new SqlQuery { ParentSql = subQuery.SqlQuery.ParentSql }, addToSql)
		{
		}

		public SubQueryContext(IParseContext subQuery)
			: this(subQuery, true)
		{
		}

		public ExpressionParser Parser     { get { return SubQuery.Parser;     } }
		public Expression       Expression { get { return SubQuery.Expression; } }
		public SqlQuery         SqlQuery   { get; set; }
		public IParseContext    Parent     { get; set; }

		public void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
		{
			SubQuery.BuildQuery(query, queryParameter);
		}

		public Expression BuildExpression(Expression expression, int level)
		{
			return SubQuery.BuildExpression(expression, level);
			//throw new NotImplementedException();
		}

		public SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
		{
			return SubQuery
				.ConvertToIndex(expression, level, flags)
				.Select(idx => new SqlInfo { Sql = SubQuery.SqlQuery.Select.Columns[idx.Index], Member = idx.Member })
				.ToArray();
		}

		readonly Dictionary<ISqlExpression,int> _indexes = new Dictionary<ISqlExpression,int>();

		public SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
		{
			return ConvertToSql(expression, level, flags)
				.Select(_ => { _.Index = GetIndex(_.Sql); return _; })
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

		public virtual IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
		{
			return SubQuery.GetContext(expression, level, currentSql);
		}

		int GetIndex(ISqlExpression sql)
		{
			int idx;

			if (!_indexes.TryGetValue(sql, out idx))
			{
				idx = SqlQuery.Select.Add(sql);
				_indexes.Add(sql, idx);
			}

			return idx;
		}

		public int ConvertToParentIndex(int index, IParseContext context)
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
	}
}
