using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class SqlCeSqlProvider : BasicSqlProvider
	{
		public SqlCeSqlProvider(DataProviderBase dataProvider) : base(dataProvider)
		{
		}

		public override bool IsSkipSupported           { get { return false; } }
		public override bool IsTakeSupported           { get { return false; } }
		public override bool IsSubQueryTakeSupported   { get { return false; } }
		public override bool IsSubQueryColumnSupported { get { return false; } }

		public override int CommandCount(SqlQuery sqlQuery)
		{
			return sqlQuery.QueryType == QueryType.Insert && sqlQuery.Set.WithIdentity ? 2 : 1;
		}

		protected override void BuildCommand(int commandNumber, StringBuilder sb)
		{
			sb.AppendLine("SELECT @@IDENTITY");
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				var be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "%":
						return TypeHelper.IsIntegerType(be.Expr1.SystemType)?
							be :
							new SqlBinaryExpression(
								typeof(int),
								new SqlFunction(typeof(int), "Convert", SqlDataType.Int32, be.Expr1),
								be.Operation,
								be.Expr2,
								be.Precedence);
				}
			}
			else if (expr is SqlFunction)
			{
				var func = (SqlFunction)expr;

				switch (func.Name)
				{
					case "Convert" :
						switch (Type.GetTypeCode(TypeHelper.GetUnderlyingType(func.SystemType)))
						{
							case TypeCode.UInt64 :
								if (TypeHelper.IsFloatType(func.Parameters[1].SystemType))
									return new SqlFunction(
										func.SystemType,
										func.Name,
										func.Precedence,
										func.Parameters[0],
										new SqlFunction(func.SystemType, "Floor", func.Parameters[1]));

								break;

							case TypeCode.DateTime :
								var type1 = TypeHelper.GetUnderlyingType(func.Parameters[1].SystemType);

								if (IsTimeDataType(func.Parameters[0]))
								{
									if (type1 == typeof(DateTime) || type1 == typeof(DateTimeOffset))
										return new SqlExpression(
											func.SystemType, "Cast(Convert(NChar, {0}, 114) as DateTime)", Precedence.Primary, func.Parameters[1]);

									if (func.Parameters[1].SystemType == typeof(string))
										return func.Parameters[1];

									return new SqlExpression(
										func.SystemType, "Convert(NChar, {0}, 114)", Precedence.Primary, func.Parameters[1]);
								}

								if (type1 == typeof(DateTime) || type1 == typeof(DateTimeOffset))
								{
									if (IsDateDataType(func.Parameters[0], "Datetime"))
										return new SqlExpression(
											func.SystemType, "Cast(Floor(Cast({0} as Float)) as DateTime)", Precedence.Primary, func.Parameters[1]);
								}

								break;
						}

						break;
				}
			}

			return expr;
		}

		public override SqlQuery Finalize(SqlQuery sqlQuery)
		{
			sqlQuery = base.Finalize(sqlQuery);

			new QueryVisitor().Visit(sqlQuery.Select, delegate(IQueryElement element)
			{
				if (element.ElementType == QueryElementType.SqlParameter)
					((SqlParameter)element).IsQueryParameter = false;
			});

			new QueryVisitor().Visit(sqlQuery, delegate(IQueryElement element)
			{
				if (element.ElementType != QueryElementType.SqlQuery)
					return;

				var query = (SqlQuery)element;

				for (var i = 0; i < query.Select.Columns.Count; i++)
				{
					var col = query.Select.Columns[i];

					if (col.Expression.ElementType == QueryElementType.SqlQuery)
					{
						var subQuery = (SqlQuery)col.Expression;
						var tables   = new Dictionary<ISqlTableSource,object>();

						new QueryVisitor().Visit(subQuery, delegate(IQueryElement e)
						{
							if (e is ISqlTableSource && subQuery.From.IsChild((ISqlTableSource)e))
								tables.Add((ISqlTableSource)e, null);
						});

						var refersParent = null != new QueryVisitor().Find(subQuery, delegate(IQueryElement e)
						{
							switch (e.ElementType)
							{
								case QueryElementType.SqlField : return !tables.ContainsKey(((SqlField)e).Table);
								case QueryElementType.Column   : return !tables.ContainsKey(((SqlQuery.Column)e).Parent);
							}
							return false;
						});

						if (!refersParent)
							continue;

						var join = SqlQuery.LeftJoin(subQuery);

						query.From.Tables[0].Joins.Add(join.JoinedTable);

						SqlQuery.OptimizeSearchCondition(subQuery.Where.SearchCondition);

						var isAggregated = null != new QueryVisitor().Find(subQuery, delegate(IQueryElement e)
						{
							if (e.ElementType == QueryElementType.SqlFunction)
								switch (((SqlFunction)e).Name)
								{
									case "Min"     :
									case "Max"     :
									case "Count"   :
									case "Sum"     :
									case "Average" : return true;
								}
							return false;
						});

						var allAnd = true;

						for (var j = 0; allAnd && j < subQuery.Where.SearchCondition.Conditions.Count - 1; j++)
						{
							var cond = subQuery.Where.SearchCondition.Conditions[j];

							if (cond.IsOr)
								allAnd = false;
						}

						if (!allAnd)
							continue;

						var modified = false;

						for (var j = 0; j < subQuery.Where.SearchCondition.Conditions.Count; j++)
						{
							var cond = subQuery.Where.SearchCondition.Conditions[j];

							var hasParentRef = null != new QueryVisitor().Find(cond, delegate(IQueryElement e)
							{
								switch (e.ElementType)
								{
									case QueryElementType.SqlField : return !tables.ContainsKey(((SqlField)e).Table);
									case QueryElementType.Column   : return !tables.ContainsKey(((SqlQuery.Column)e).Parent);
								}
								return false;
							});

							if (!hasParentRef)
								continue;

							var replaced = new Dictionary<IQueryElement,IQueryElement>();

							var nc = new QueryVisitor().Convert(cond, delegate(IQueryElement e)
							{
								var ne = e;

								switch (e.ElementType)
								{
									case QueryElementType.SqlField :
										if (replaced.TryGetValue(e, out ne))
											return ne;

										if (tables.ContainsKey(((SqlField)e).Table))
										{
											if (isAggregated)
												subQuery.GroupBy.Expr((SqlField)e);
											ne = subQuery.Select.Columns[subQuery.Select.Add((SqlField)e)];
											break;
										}

										break;
									case QueryElementType.Column   :
										if (replaced.TryGetValue(e, out ne))
											return ne;

										if (tables.ContainsKey(((SqlQuery.Column)e).Parent))
										{
											if (isAggregated)
												subQuery.GroupBy.Expr((SqlQuery.Column)e);
											ne = subQuery.Select.Columns[subQuery.Select.Add((SqlQuery.Column)e)];
											break;
										}

										break;
								}

								if (!ReferenceEquals(e, ne))
									replaced.Add(e, ne);

								return ne;
							});

							if (nc != null && !ReferenceEquals(nc, cond))
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

			sqlQuery = base.Finalize(sqlQuery);

			switch (sqlQuery.QueryType)
			{
				case QueryType.Delete :
					sqlQuery = GetAlternativeDelete(sqlQuery);
					sqlQuery.From.Tables[0].Alias = "$";
					break;

				case QueryType.Update :
					sqlQuery = GetAlternativeUpdate(sqlQuery);
					break;
			}

			return sqlQuery;
		}

		protected override void BuildDataType(StringBuilder sb, SqlDataType type)
		{
			switch (type.DbType)
			{
				case SqlDbType.Char          : base.BuildDataType(sb, new SqlDataType(SqlDbType.NChar,    type.Length)); break;
				case SqlDbType.VarChar       : base.BuildDataType(sb, new SqlDataType(SqlDbType.NVarChar, type.Length)); break;
				case SqlDbType.SmallMoney    : sb.Append("Decimal(10,4)");   break;
				case SqlDbType.Time          :
				case SqlDbType.Date          :
				case SqlDbType.SmallDateTime :
				case SqlDbType.DateTime2     : sb.Append("DateTime");        break;
				default                      : base.BuildDataType(sb, type); break;
			}
		}

		protected override void BuildFromClause(StringBuilder sb)
		{
			if (SqlQuery.QueryType != QueryType.Update)
				base.BuildFromClause(sb);
		}
	}
}
