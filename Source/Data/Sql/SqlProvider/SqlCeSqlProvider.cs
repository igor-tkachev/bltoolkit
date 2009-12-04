using System;
using System.Collections.Generic;
using System.Reflection;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

#if FW3
	using Linq;

	using C = Char;
	using S = String;
	using I = Int32;
#endif

	public class SqlCeSqlProvider : BasicSqlProvider
	{
		public SqlCeSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override bool IsSkipSupported           { get { return false; } }
		public override bool IsTakeSupported           { get { return false; } }
		public override bool IsSubQueryTakeSupported   { get { return false; } }
		public override bool IsSubQueryColumnSupported { get { return false; } }

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlExpression)
			{
				SqlExpression ex = (SqlExpression)expr;

				//switch (ex.Expr)
				//{
				//	case "CURRENT_TIMESTAMP" : return new SqlFunction("GetDate");
				//}
			}

			return expr;
		}

		public override SqlQuery Finalize(SqlQuery sqlQuery)
		{
			new QueryVisitor().Visit(sqlQuery, delegate(IQueryElement element)
			{
				if (element.ElementType != QueryElementType.SqlQuery)
					return;

				SqlQuery query = (SqlQuery)element;

				for (int i = 0; i < query.Select.Columns.Count; i++)
				{
					SqlQuery.Column col = query.Select.Columns[i];

					if (col.Expression.ElementType == QueryElementType.SqlQuery)
					{
						SqlQuery                           subQuery = (SqlQuery)col.Expression;
						Dictionary<ISqlTableSource,object> tables   = new Dictionary<ISqlTableSource,object>();

						new QueryVisitor().Visit(subQuery, delegate(IQueryElement e)
						{
							if (e.ElementType == QueryElementType.SqlTable)
								tables.Add((SqlTable)e, null);
						});

						bool refersParent = false;

						new QueryVisitor().Visit(subQuery, delegate(IQueryElement e)
						{
							switch (e.ElementType)
							{
								case QueryElementType.SqlField : if (!tables.ContainsKey(((SqlField)e).Table))         refersParent = true; break;
								case QueryElementType.Column   : if (!tables.ContainsKey(((SqlQuery.Column)e).Parent)) refersParent = true; break;
							}
						});

						if (!refersParent)
							continue;

						SqlQuery.FromClause.Join join = SqlQuery.LeftJoin(subQuery);

						query.From.Tables[0].Joins.Add(join.JoinedTable);

						SqlQuery.OptimizeSearchCondition(subQuery.Where.SearchCondition);

						bool isAggregated = false;

						new QueryVisitor().Visit(subQuery, delegate(IQueryElement e)
						{
							if (e.ElementType == QueryElementType.SqlFunction)
								switch (((SqlFunction)e).Name)
								{
									case "Min"     :
									case "Max"     :
									case "Count"   :
									case "Average" : isAggregated = true; break;
								}
						});

						bool allAnd = true;

						for (int j = 0; allAnd && j < subQuery.Where.SearchCondition.Conditions.Count - 1; j++)
						{
							SqlQuery.Condition cond = subQuery.Where.SearchCondition.Conditions[j];

							if (cond.IsOr)
								allAnd = false;
						}

						if (!allAnd)
							continue;

						bool modified = false;

						for (int j = 0; j < subQuery.Where.SearchCondition.Conditions.Count; j++)
						{
							SqlQuery.Condition cond = subQuery.Where.SearchCondition.Conditions[j];

							bool hasParentRef = false;

							new QueryVisitor().Visit(cond, delegate(IQueryElement e)
							{
								switch (e.ElementType)
								{
									case QueryElementType.SqlField : if (!tables.ContainsKey(((SqlField)e).Table))         hasParentRef = true; break;
									case QueryElementType.Column   : if (!tables.ContainsKey(((SqlQuery.Column)e).Parent)) hasParentRef = true; break;
								}
							});

							if (!hasParentRef)
								continue;

							SqlQuery.Condition nc = new QueryVisitor().Convert(cond, delegate(IQueryElement e)
							{
								switch (e.ElementType)
								{
									case QueryElementType.SqlField :
										if (tables.ContainsKey(((SqlField)e).Table))
										{
											if (isAggregated)
												subQuery.GroupBy.Expr((SqlField)e);
											return subQuery.Select.Columns[subQuery.Select.Add((SqlField)e)];
										}

										break;
									case QueryElementType.Column   :
										if (tables.ContainsKey(((SqlQuery.Column)e).Parent))
										{
											if (isAggregated)
												subQuery.GroupBy.Expr((SqlQuery.Column)e);
											return subQuery.Select.Columns[subQuery.Select.Add((SqlQuery.Column)e)];
										}

										break;
								}

								return e;
							});

							if (nc != null && !object.ReferenceEquals(nc, cond))
							{
								modified = true;

								join.JoinedTable.Condition.Conditions.Add(nc);
								subQuery.Where.SearchCondition.Conditions.RemoveAt(j);
								j--;
							}
						}

						if (modified)
							query.Select.Columns[i] = new SqlQuery.Column(query, subQuery.Select.Columns[0]);
					}
				}
			});

			return base.Finalize(sqlQuery);
		}

#if FW3
		protected override Dictionary<MemberInfo,BaseExpressor> GetExpressors() { return _members; }
		static    readonly Dictionary<MemberInfo,BaseExpressor> _members = new Dictionary<MemberInfo,BaseExpressor>
		{
			{ MI(() => Sql.Left    ("",0)    ), new F<S,I,S>  ((p0,p1)    => Sql.Substring(p0, 1, p1)) },
			{ MI(() => Sql.Right   ("",0)    ), new F<S,I,S>  ((p0,p1)    => Sql.Substring(p0, p0.Length - p1 + 1, p1)) },
			{ MI(() => Sql.PadRight("",0,' ')), new F<S,I,C,S>((p0,p1,p2) => p0.Length > p1 ? p0 : p0 + Replicate(p2, p1 - p0.Length)) },
			{ MI(() => Sql.PadLeft ("",0,' ')), new F<S,I,C,S>((p0,p1,p2) => p0.Length > p1 ? p0 : Replicate(p2, p1 - p0.Length) + p0) },
			{ MI(() => Sql.Trim    ("")      ), new F<S,S>    ( p0        => Sql.TrimLeft(Sql.TrimRight(p0))) },
		};
#endif
	}
}
