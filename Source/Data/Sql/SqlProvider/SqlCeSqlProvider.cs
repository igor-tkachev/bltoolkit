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

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

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
				SqlFunction func = (SqlFunction)expr;

				switch (func.Name)
				{
					case "Convert" :
						switch (Type.GetTypeCode(func.SystemType))
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
								Type type1 = func.Parameters[1].SystemType;

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
	}
}
