using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	class SubQueryContext : IParseContext
	{
		readonly IParseContext _subQuery;

		public SubQueryContext(IParseContext subQuery)
		{
			_subQuery = subQuery;

			SqlQuery = new SqlQuery { ParentSql = _subQuery.SqlQuery.ParentSql };

			SqlQuery.From.Table(_subQuery.SqlQuery);

			_subQuery.SqlQuery.ParentSql = SqlQuery;
		}

		public ExpressionParser Parser     { get { return _subQuery.Parser;     } }
		public Expression       Expression { get { return _subQuery.Expression; } }
		public SqlQuery         SqlQuery   { get; private set; }
		public IParseContext    Root
		{
			get { return _subQuery.Root;  }
			set { _subQuery.Root = value; }
		}

		public Expression BuildQuery()
		{
			return _subQuery.BuildQuery();
		}

		public Expression BuildExpression(Expression expression, int level)
		{
			//return _subQuery.BuildExpression(expression, level);
			throw new NotImplementedException();
		}

		public ISqlExpression[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
		{
			return _subQuery
				.ConvertToIndex(expression, level, flags)
				.Select(idx => _subQuery.SqlQuery.Select.Columns[idx])
				.ToArray();
		}

		readonly Dictionary<ISqlExpression,int> _indexes = new Dictionary<ISqlExpression,int>();

		public int[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
		{
			return ConvertToSql(expression, level, flags)
				.Select(expr => 
				{
					int idx;

					if (!_indexes.TryGetValue(expr, out idx))
					{
						idx = SqlQuery.Select.Add(expr);
						_indexes.Add(expr, idx);
					}

					return idx;
				})
				.ToArray();
		}

		public bool IsExpression(Expression expression, int level, RequestFor testFlag)
		{
			return _subQuery.IsExpression(expression, level, testFlag);
		}

		public IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
		{
			return _subQuery.GetContext(expression, level, currentSql);
		}

		public void SetAlias(string alias)
		{
			if (SqlQuery.From.Tables[0].Alias == null)
				SqlQuery.From.Tables[0].Alias = alias;
		}
	}
}
