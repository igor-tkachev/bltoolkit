﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;

	class SubQueryContext : PassThroughContext
	{
		public readonly IBuildContext SubQuery;

		public SubQueryContext(IBuildContext subQuery, SqlQuery sqlQuery, bool addToSql)
			: base(subQuery)
		{
			if (sqlQuery == subQuery.SqlQuery)
				throw new ArgumentException("Wrong subQuery argument.", "subQuery");

			SubQuery = subQuery;
			SubQuery.Parent = this;

			SqlQuery = sqlQuery;

			if (addToSql)
				sqlQuery.From.Table(SubQuery.SqlQuery);
		}

		public SubQueryContext(IBuildContext subQuery, bool addToSql)
			: this(subQuery, new SqlQuery { ParentSql = subQuery.SqlQuery.ParentSql }, addToSql)
		{
		}

		public SubQueryContext(IBuildContext subQuery)
			: this(subQuery, true)
		{
		}

		public override SqlQuery      SqlQuery { get; set; }
		public override IBuildContext Parent   { get; set; }

		public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
		{
			if (Expression.NodeType == ExpressionType.Lambda)
			{
				var le = (LambdaExpression)Expression;

				if (le.Parameters.Count == 1 && null != Expression.Find(
					e => e.NodeType == ExpressionType.Call && ((MethodCallExpression)e).IsQueryable()))
				{
					if (le.Body.NodeType == ExpressionType.New)
					{
						var ne = (NewExpression)le.Body;
						var p  = Expression.Parameter(ne.Type, "p");

						var seq = new SelectContext(
							Parent,
							Expression.Lambda(
								Expression.New(
									ne.Constructor,
									ne.Members.Select(m => Expression.MakeMemberAccess(p, m)).ToArray(),
									ne.Members),
								p),
							this);

						seq.BuildQuery(query, queryParameter);

						return;
					}

					if (le.Body.NodeType == ExpressionType.MemberInit)
					{
						var mi = (MemberInitExpression)le.Body;

						if (mi.NewExpression.Arguments.Count == 0 && mi.Bindings.All(b => b is MemberAssignment))
						{
							var p = Expression.Parameter(mi.Type, "p");

							var seq = new SelectContext(
								Parent,
								Expression.Lambda(
									Expression.MemberInit(
										mi.NewExpression,
										mi.Bindings
											.OfType<MemberAssignment>()
											.Select(ma => Expression.Bind(ma.Member, Expression.MakeMemberAccess(p, ma.Member)))
											.ToArray()),
									p),
								this);

							seq.BuildQuery(query, queryParameter);

							return;
						}
					}
				}
			}

			base.BuildQuery(query, queryParameter);
		}

		public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
		{
			return SubQuery
				.ConvertToIndex(expression, level, flags)
				.Select(idx => new SqlInfo(idx.Members) { Sql = SubQuery.SqlQuery.Select.Columns[idx.Index] })
				.ToArray();
		}

		// JoinContext has similar logic. Consider to review it.
		//
		public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
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

		public override IsExpressionResult IsExpression(Expression expression, int level, RequestFor testFlag)
		{
			switch (testFlag)
			{
				case RequestFor.SubQuery : return IsExpressionResult.True;
			}

			return base.IsExpression(expression, level, testFlag);
		}

		internal protected readonly Dictionary<ISqlExpression,int> ColumnIndexes = new Dictionary<ISqlExpression,int>();

		protected virtual int GetIndex(SqlQuery.Column column)
		{
			int idx;

			if (!ColumnIndexes.TryGetValue(column, out idx))
			{
				idx = SqlQuery.Select.Add(column);
				ColumnIndexes.Add(column, idx);
			}

			return idx;
		}

		public override int ConvertToParentIndex(int index, IBuildContext context)
		{
			var idx = GetIndex(context.SqlQuery.Select.Columns[index]);
			return Parent == null ? idx : Parent.ConvertToParentIndex(idx, this);
		}

		public override void SetAlias(string alias)
		{
			if (alias.Contains('<'))
				return;

			if (SqlQuery.From.Tables[0].Alias == null)
				SqlQuery.From.Tables[0].Alias = alias;
		}

		public override ISqlExpression GetSubQuery(IBuildContext context)
		{
			return null;
		}
	}
}
