using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;
	using Mapping;
	using Reflection;

	public class BasicSqlProvider : ISqlProvider
	{
		#region Init

		public BasicSqlProvider(DataProviderBase dataProvider)
		{
			_dataProvider = dataProvider;
		}

		readonly DataProviderBase _dataProvider;
		public   DataProviderBase  DataProvider
		{
			get { return _dataProvider; }
		}

		private SqlQuery _sqlQuery;
		public  SqlQuery  SqlQuery
		{
			get { return _sqlQuery;  }
			set { _sqlQuery = value; }
		}

		private int _indent;
		public  int  Indent
		{
			get { return _indent;  }
			set { _indent = value; }
		}

		private int _nextNesting = 1;
		private int _nesting;
		public  int  Nesting
		{
			get { return _nesting; }
		}

		bool _skipAlias;

		private Step _buildStep;
		public  Step  BuildStep
		{
			get { return _buildStep;  }
			set { _buildStep = value; }
		}

		#endregion

		#region Support Flags

		public virtual bool SkipAcceptsParameter            { get { return true;  } }
		public virtual bool TakeAcceptsParameter            { get { return true;  } }
		public virtual bool IsTakeSupported                 { get { return true;  } }
		public virtual bool IsSkipSupported                 { get { return true;  } }
		public virtual bool IsSubQueryTakeSupported         { get { return true;  } }
		public virtual bool IsSubQueryColumnSupported       { get { return true;  } }
		public virtual bool IsCountSubQuerySupported        { get { return true;  } }
		public virtual bool IsNestedJoinSupported           { get { return true;  } }
		public virtual bool IsNestedJoinParenthesisRequired { get { return false; } }
		public virtual bool IsIdentityParameterRequired     { get { return false; } }

		#endregion

		#region CommandCount

		public virtual int CommandCount(SqlQuery sqlQuery)
		{
			return 1;
		}

		#endregion

		#region BuildSql

		public virtual int BuildSql(int commandNumber, SqlQuery sqlQuery, StringBuilder sb, int indent, int nesting, bool skipAlias)
		{
			_sqlQuery    = sqlQuery;
			_indent      = indent;
			_nesting     = nesting;
			_nextNesting = _nesting + 1;
			_skipAlias   = skipAlias;

			if (commandNumber == 0)
			{
				BuildSql(sb);

				if (sqlQuery.HasUnion)
				{
					foreach (var union in sqlQuery.Unions)
					{
						AppendIndent(sb);
						sb.Append("UNION");
						if (union.IsAll) sb.Append(" ALL");
						sb.AppendLine();

						CreateSqlProvider().BuildSql(commandNumber, union.SqlQuery, sb, indent, nesting, skipAlias);
					}
				}
			}
			else
			{
				BuildCommand(commandNumber, sb);
			}

			return _nextNesting;
		}

		protected virtual void BuildCommand(int commandNumber, StringBuilder sb)
		{
		}

		#endregion

		#region Overrides

		protected virtual int BuildSqlBuilder(SqlQuery sqlQuery, StringBuilder sb, int indent, int nesting, bool skipAlias)
		{
			if (!IsSkipSupported && sqlQuery.Select.SkipValue != null)
				throw new SqlException("Skip for subqueries is not supported by the '{0}' provider.", _dataProvider.Name);

			if (!IsTakeSupported && sqlQuery.Select.TakeValue != null)
				throw new SqlException("Take for subqueries is not supported by the '{0}' provider.", _dataProvider.Name);

			return CreateSqlProvider().BuildSql(0, sqlQuery, sb, indent, nesting, skipAlias);
		}

		protected virtual ISqlProvider CreateSqlProvider()
		{
			return DataProvider.CreateSqlProvider();
		}

		protected virtual bool ParenthesizeJoin()
		{
			return false;
		}

		protected virtual void BuildSql(StringBuilder sb)
		{
			switch (_sqlQuery.QueryType)
			{
				case QueryType.Delete : _buildStep = Step.DeleteClause; BuildDeleteClause(sb); break;
				case QueryType.Update : _buildStep = Step.UpdateClause; BuildUpdateClause(sb); break;
				case QueryType.Insert : _buildStep = Step.InsertClause; BuildInsertClause(sb);
					if (_sqlQuery.From.Tables.Count == 0)
						break;
					goto default;
				default               : _buildStep = Step.SelectClause; BuildSelectClause(sb); break;
			}

			_buildStep = Step.FromClause;    BuildFromClause   (sb);
			_buildStep = Step.WhereClause;   BuildWhereClause  (sb);
			_buildStep = Step.GroupByClause; BuildGroupByClause(sb);
			_buildStep = Step.HavingClause;  BuildHavingClause (sb);
			_buildStep = Step.OrderByClause; BuildOrderByClause(sb);
			_buildStep = Step.OffsetLimit;   BuildOffsetLimit  (sb);

			
			if (SqlQuery.QueryType == QueryType.Insert && SqlQuery.Set.WithIdentity)
				BuildGetIdentity(sb);
		}

		#endregion

		#region Build Select

		protected virtual void BuildSelectClause(StringBuilder sb)
		{
			AppendIndent(sb);
			sb.Append("SELECT");

			if (SqlQuery.Select.IsDistinct)
				sb.Append(" DISTINCT");

			BuildSkipFirst(sb);

			sb.AppendLine();
			BuildColumns(sb);
		}

		protected virtual IEnumerable<SqlQuery.Column> GetSelectedColumns()
		{
			return _sqlQuery.Select.Columns;
		}

		protected virtual void BuildColumns(StringBuilder sb)
		{
			_indent++;

			var first = true;

			foreach (var col in GetSelectedColumns())
			{
				if (!first)
					sb.Append(',').AppendLine();
				first = false;

				var addAlias = true;

				AppendIndent(sb);
				BuildColumn(sb, col, ref addAlias);

				if (!_skipAlias && addAlias && col.Alias != null)
					sb.Append(" as ").Append(DataProvider.Convert(col.Alias, ConvertType.NameToQueryFieldAlias));
			}

			if (first)
				AppendIndent(sb).Append("*");

			_indent--;

			sb.AppendLine();
		}

		protected virtual void BuildColumn(StringBuilder sb, SqlQuery.Column col, ref bool addAlias)
		{
			BuildExpression(sb, col.Expression, true, true, col.Alias, ref addAlias);
		}

		#endregion

		#region Build Delete

		protected virtual void BuildDeleteClause(StringBuilder sb)
		{
			AppendIndent(sb);
			sb.Append("DELETE ");
		}

		#endregion

		#region Build Update

		protected virtual void BuildUpdateClause(StringBuilder sb)
		{
			BuildUpdateTable(sb);
			BuildUpdateSet  (sb);
		}

		protected virtual void BuildUpdateTable(StringBuilder sb)
		{
			AppendIndent(sb)
				.AppendLine("UPDATE")
				.Append('\t');
			BuildUpdateTableName(sb);
			sb.AppendLine();
		}

		protected virtual void BuildUpdateTableName(StringBuilder sb)
		{
			BuildTableName(sb, SqlQuery.From.Tables[0], true, true);
		}

		protected virtual void BuildUpdateSet(StringBuilder sb)
		{
			AppendIndent(sb)
				.AppendLine("SET");

			_indent++;

			var first = true;

			foreach (var expr in _sqlQuery.Set.Items)
			{
				if (!first)
					sb.Append(',').AppendLine();
				first = false;

				AppendIndent(sb);
				BuildExpression(sb, expr.Column, false, true);
				sb.Append(" = ");
				BuildExpression(sb, expr.Expression);
			}

			_indent--;

			sb.AppendLine();
		}

		#endregion

		#region Build Insert

		protected virtual void BuildInsertClause(StringBuilder sb)
		{
			AppendIndent(sb).Append("INSERT INTO ");
			BuildPhysicalTable(sb, SqlQuery.Set.Into);
			sb.AppendLine(" ");

			AppendIndent(sb).AppendLine("(");

			_indent++;

			var first = true;

			foreach (var expr in _sqlQuery.Set.Items)
			{
				if (!first)
					sb.Append(',').AppendLine();
				first = false;

				AppendIndent(sb);
				BuildExpression(sb, expr.Column, false, true);
			}

			_indent--;

			sb.AppendLine();
			AppendIndent(sb).AppendLine(")");

			if (_sqlQuery.From.Tables.Count == 0)
			{
				AppendIndent(sb).AppendLine("VALUES");
				AppendIndent(sb).AppendLine("(");

				_indent++;

				first = true;

				foreach (var expr in _sqlQuery.Set.Items)
				{
					if (!first)
						sb.Append(',').AppendLine();
					first = false;

					AppendIndent(sb);
					BuildExpression(sb, expr.Expression);
				}

				_indent--;

				sb.AppendLine();
				AppendIndent(sb).AppendLine(")");
			}
		}

		protected virtual void BuildGetIdentity(StringBuilder sb)
		{
			//throw new SqlException("Insert with identity is not supported by the '{0}' sql provider.", Name);
		}

		#endregion

		#region Build From

		protected virtual void BuildFromClause(StringBuilder sb)
		{
			if (_sqlQuery.From.Tables.Count == 0)
				return;

			AppendIndent(sb);

			sb.Append("FROM").AppendLine();

			_indent++;
			AppendIndent(sb);

			var first = true;

			foreach (var ts in _sqlQuery.From.Tables)
			{
				if (!first)
					sb.Append(", ");
				first = false;

				var jn = ParenthesizeJoin() ? ts.GetJoinNumber() : 0;

				if (jn > 0)
				{
					jn--;
					for (var i = 0; i < jn; i++)
						sb.Append("(");
				}

				BuildTableName(sb, ts, true, true);

				foreach (var jt in ts.Joins)
					BuildJoinTable(sb, jt, ref jn);
			}

			_indent--;

			sb.AppendLine();
		}

		protected void BuildPhysicalTable(StringBuilder sb, ISqlTableSource table)
		{
			switch (table.ElementType)
			{
				case QueryElementType.SqlTable    :
				case QueryElementType.TableSource :
					sb.Append(GetTablePhysicalName(table));
					break;

				case QueryElementType.SqlQuery    :
					sb.Append("(").AppendLine();
					_nextNesting = BuildSqlBuilder((SqlQuery)table, sb, _indent + 1, _nextNesting, false);
					AppendIndent(sb).Append(")");

					break;

				default:
					throw new InvalidOperationException();
			}
		}

		protected void BuildTableName(StringBuilder sb, SqlQuery.TableSource ts, bool buildName, bool buildAlias)
		{
			if (buildName)
				BuildPhysicalTable(sb, ts.Source);

			if (buildAlias)
			{
				var alias = GetTableAlias(ts);

				if (!string.IsNullOrEmpty(alias))
				{
					if (buildName)
						sb.Append(" ");
					sb.Append(DataProvider.Convert(alias, ConvertType.NameToQueryTableAlias));
				}
			}
		}

		void BuildJoinTable(StringBuilder sb, SqlQuery.JoinedTable join, ref int joinCounter)
		{
			sb.AppendLine();
			_indent++;
			AppendIndent(sb);

			switch (join.JoinType)
			{
				case SqlQuery.JoinType.Inner : sb.Append("INNER JOIN "); break;
				case SqlQuery.JoinType.Left  : sb.Append("LEFT JOIN ");  break;
				default: throw new InvalidOperationException();
			}

			if (IsNestedJoinParenthesisRequired && join.Table.Joins.Count != 0)
				sb.Append('(');

			BuildTableName(sb, join.Table, true, true);

			if (IsNestedJoinSupported && join.Table.Joins.Count != 0)
			{
				foreach (var jt in join.Table.Joins)
					BuildJoinTable(sb, jt, ref joinCounter);

				if (IsNestedJoinParenthesisRequired && join.Table.Joins.Count != 0)
					sb.Append(')');

				sb.AppendLine();
				AppendIndent(sb);
				sb.Append("ON ");
			}
			else
				sb.Append(" ON ");

			if (join.Condition.Conditions.Count != 0)
				BuildSearchCondition(sb, Precedence.Unknown, join.Condition);
			else
				sb.Append("1=1");

			if (joinCounter > 0)
			{
				joinCounter--;
				sb.Append(")");
			}

			if (!IsNestedJoinSupported)
				foreach (var jt in join.Table.Joins)
					BuildJoinTable(sb, jt, ref joinCounter);

			_indent--;
		}

		#endregion

		#region Where Clause

		protected virtual bool BuildWhere()
		{
			return _sqlQuery.Where.SearchCondition.Conditions.Count != 0;
		}

		protected virtual void BuildWhereClause(StringBuilder sb)
		{
			if (!BuildWhere())
				return;

			AppendIndent(sb);

			sb.Append("WHERE").AppendLine();

			_indent++;
			AppendIndent(sb);
			BuildWhereSearchCondition(sb, _sqlQuery.Where.SearchCondition);
			_indent--;

			sb.AppendLine();
		}

		#endregion

		#region GroupBy Clause

		protected virtual void BuildGroupByClause(StringBuilder sb)
		{
			if (_sqlQuery.GroupBy.Items.Count == 0)
				return;

			AppendIndent(sb);

			sb.Append("GROUP BY").AppendLine();

			_indent++;

			for (var i = 0; i < _sqlQuery.GroupBy.Items.Count; i++)
			{
				AppendIndent(sb);

				BuildExpression(sb, _sqlQuery.GroupBy.Items[i]);

				if (i + 1 < _sqlQuery.GroupBy.Items.Count)
					sb.Append(',');

				sb.AppendLine();
			}

			_indent--;
		}

		#endregion

		#region Having Clause

		protected virtual void BuildHavingClause(StringBuilder sb)
		{
			if (_sqlQuery.Having.SearchCondition.Conditions.Count == 0)
				return;

			AppendIndent(sb);

			sb.Append("HAVING").AppendLine();

			_indent++;
			AppendIndent(sb);
			BuildWhereSearchCondition(sb, _sqlQuery.Having.SearchCondition);
			_indent--;

			sb.AppendLine();
		}

		#endregion

		#region OrderBy Clause

		protected virtual void BuildOrderByClause(StringBuilder sb)
		{
			if (_sqlQuery.OrderBy.Items.Count == 0)
				return;

			AppendIndent(sb);

			sb.Append("ORDER BY").AppendLine();

			_indent++;

			for (var i = 0; i < _sqlQuery.OrderBy.Items.Count; i++)
			{
				AppendIndent(sb);

				var item = _sqlQuery.OrderBy.Items[i];

				BuildExpression(sb, item.Expression);

				if (item.IsDescending)
					sb.Append(" DESC");

				if (i + 1 < _sqlQuery.OrderBy.Items.Count)
					sb.Append(',');

				sb.AppendLine();
			}

			_indent--;
		}

		#endregion

		#region Skip/Take

		protected virtual bool   SkipFirst    { get { return true; } }
		protected virtual string SkipFormat   { get { return null; } }
		protected virtual string FirstFormat  { get { return null; } }
		protected virtual string LimitFormat  { get { return null; } }
		protected virtual string OffsetFormat { get { return null; } }

		protected bool NeedSkip { get { return SqlQuery.Select.SkipValue != null && IsSkipSupported; } }
		protected bool NeedTake { get { return SqlQuery.Select.TakeValue != null && IsTakeSupported; } }

		protected virtual void BuildSkipFirst(StringBuilder sb)
		{
			if (SkipFirst && NeedSkip && SkipFormat != null)
				sb.Append(' ').AppendFormat(SkipFormat,  BuildExpression(new StringBuilder(), SqlQuery.Select.SkipValue));

			if (NeedTake && FirstFormat != null)
				sb.Append(' ').AppendFormat(FirstFormat, BuildExpression(new StringBuilder(), SqlQuery.Select.TakeValue));

			if (!SkipFirst && NeedSkip && SkipFormat != null)
				sb.Append(' ').AppendFormat(SkipFormat,  BuildExpression(new StringBuilder(), SqlQuery.Select.SkipValue));
		}

		protected virtual void BuildOffsetLimit(StringBuilder sb)
		{
			var doSkip = NeedSkip && OffsetFormat != null;
			var doTake = NeedTake && LimitFormat  != null;

			if (doSkip || doTake)
			{
				AppendIndent(sb);

				if (doTake)
				{
					sb.AppendFormat(LimitFormat, BuildExpression(new StringBuilder(), SqlQuery.Select.TakeValue));

					if (doSkip)
						sb.Append(' ');
				}

				if (doSkip)
					sb.AppendFormat(OffsetFormat, BuildExpression(new StringBuilder(), SqlQuery.Select.SkipValue));

				sb.AppendLine();
			}
		}

		#endregion

		#region Builders

		#region BuildSearchCondition

		protected virtual void BuildWhereSearchCondition(StringBuilder sb, SqlQuery.SearchCondition condition)
		{
			BuildSearchCondition(sb, Precedence.Unknown, condition);
		}

		protected virtual void BuildSearchCondition(StringBuilder sb, SqlQuery.SearchCondition condition)
		{
			var isOr = (bool?)null;
			var len  = sb.Length;
			var prevPrecedence = condition.Conditions.Count > 1 && !condition.Conditions[0].IsOr ? Precedence.LogicalConjunction : Precedence.LogicalDisjunction;

			foreach (var cond in condition.Conditions)
			{
				if (isOr != null)
				{
					sb.Append(isOr.Value ? " OR" : " AND");

					if (condition.Conditions.Count < 4 && sb.Length - len < 50 || condition != _sqlQuery.Where.SearchCondition)
					{
						sb.Append(' ');
					}
					else
					{
						sb.AppendLine();
						AppendIndent(sb);
						len = sb.Length;
					}
				}

				if (cond.IsNot)
					sb.Append("NOT ");

				BuildPredicate(sb, cond.IsNot ? Precedence.LogicalNegation : prevPrecedence, cond.Predicate);

				isOr = cond.IsOr;

				prevPrecedence = cond.Precedence;
			}
		}

		protected virtual void BuildSearchCondition(StringBuilder sb, int parentPrecedence, SqlQuery.SearchCondition condition)
		{
			var wrap = Wrap(GetPrecedence(condition as ISqlExpression), parentPrecedence);

			if (wrap) sb.Append('(');
			BuildSearchCondition(sb, condition);
			if (wrap) sb.Append(')');
		}

		#endregion

		#region BuildPredicate

		protected virtual void BuildPredicate(StringBuilder sb, ISqlPredicate predicate)
		{
			switch (predicate.ElementType)
			{
				case QueryElementType.ExprExprPredicate :
					{
						var expr = (SqlQuery.Predicate.ExprExpr)predicate;

						switch (expr.Operator)
						{
							case SqlQuery.Predicate.Operator.Equal :
							case SqlQuery.Predicate.Operator.NotEqual :
								{
									ISqlExpression e = null;

									if (expr.Expr1 is SqlValue && ((SqlValue)expr.Expr1).Value == null)
										e = expr.Expr2;
									else if (expr.Expr2 is SqlValue && ((SqlValue)expr.Expr2).Value == null)
										e = expr.Expr1;

									if (e != null)
									{
										BuildExpression(sb, GetPrecedence(expr), e);
										sb.Append(expr.Operator == SqlQuery.Predicate.Operator.Equal ? " IS NULL" : " IS NOT NULL");
										return;
									}

									break;
								}
						}

						BuildExpression(sb, GetPrecedence(expr), expr.Expr1);

						switch (expr.Operator)
						{
							case SqlQuery.Predicate.Operator.Equal :
								sb.Append(" = ");
								break;
							case SqlQuery.Predicate.Operator.NotEqual :
								sb.Append(" <> ");
								break;
							case SqlQuery.Predicate.Operator.Greater :
								sb.Append(" > ");
								break;
							case SqlQuery.Predicate.Operator.GreaterOrEqual :
								sb.Append(" >= ");
								break;
							case SqlQuery.Predicate.Operator.NotGreater :
								sb.Append(" !> ");
								break;
							case SqlQuery.Predicate.Operator.Less :
								sb.Append(" < ");
								break;
							case SqlQuery.Predicate.Operator.LessOrEqual :
								sb.Append(" <= ");
								break;
							case SqlQuery.Predicate.Operator.NotLess :
								sb.Append(" !< ");
								break;
						}

						BuildExpression(sb, GetPrecedence(expr), expr.Expr2);
					}

					break;

				case QueryElementType.LikePredicate :
					BuildLikePredicate(sb, (SqlQuery.Predicate.Like)predicate);
					break;

				case QueryElementType.BetweenPredicate :
					{
						var p = (SqlQuery.Predicate.Between)predicate;
						BuildExpression(sb, GetPrecedence(p), p.Expr1);
						if (p.IsNot) sb.Append(" NOT");
						sb.Append(" BETWEEN ");
						BuildExpression(sb, GetPrecedence(p), p.Expr2);
						sb.Append(" AND ");
						BuildExpression(sb, GetPrecedence(p), p.Expr3);
					}

					break;

				case QueryElementType.IsNullPredicate :
					{
						var p = (SqlQuery.Predicate.IsNull)predicate;
						BuildExpression(sb, GetPrecedence(p), p.Expr1);
						sb.Append(p.IsNot ? " IS NOT NULL" : " IS NULL");
					}

					break;

				case QueryElementType.InSubqueryPredicate :
					{
						var p = (SqlQuery.Predicate.InSubQuery)predicate;
						BuildExpression(sb, GetPrecedence(p), p.Expr1);
						sb.Append(p.IsNot ? " NOT IN " : " IN ");
						BuildExpression(sb, GetPrecedence(p), p.SubQuery);
					}

					break;

				case QueryElementType.InListPredicate :
					BuildInListPredicate(predicate, sb);
					break;

				case QueryElementType.FuncLikePredicate :
					{
						var f = (SqlQuery.Predicate.FuncLike)predicate;
						BuildExpression(sb, f.Function.Precedence, f.Function);
					}

					break;

				case QueryElementType.SearchCondition :
					BuildSearchCondition(sb, predicate.Precedence, (SqlQuery.SearchCondition)predicate);
					break;

				case QueryElementType.NotExprPredicate :
					{
						var p = (SqlQuery.Predicate.NotExpr)predicate;

						if (p.IsNot)
							sb.Append("NOT ");

						BuildExpression(sb, p.IsNot ? Precedence.LogicalNegation : GetPrecedence(p), p.Expr1);
					}

					break;

				case QueryElementType.ExprPredicate :
					{
						var p = (SqlQuery.Predicate.Expr)predicate;

						if (p.Expr1 is SqlValue)
						{
							var value = ((SqlValue)p.Expr1).Value;

							if (value is bool)
							{
								sb.Append((bool)value ? "1 = 1" : "1 = 0");
								return;
							}
						}

						BuildExpression(sb, GetPrecedence(p), p.Expr1);
					}

					break;

				default :
					throw new InvalidOperationException();
			}
		}

		static SqlField GetUnderlayingField(ISqlExpression expr)
		{
			switch (expr.ElementType)
			{
				case QueryElementType.SqlField: return (SqlField)expr;
				case QueryElementType.Column  : return GetUnderlayingField(((SqlQuery.Column)expr).Expression);
			}

			throw new InvalidOperationException();
		}

		void BuildInListPredicate(ISqlPredicate predicate, StringBuilder sb)
		{
			var p = (SqlQuery.Predicate.InList)predicate;

			if (p.Values == null || p.Values.Count == 0)
			{
				BuildPredicate(sb, new SqlQuery.Predicate.Expr(new SqlValue(false)));
			}
			else
			{
				ICollection values = p.Values;

				if (p.Values.Count == 1 && p.Values[0] is SqlParameter)
				{
					var pr = (SqlParameter)p.Values[0];

					if (pr.Value == null)
					{
						BuildPredicate(sb, new SqlQuery.Predicate.Expr(new SqlValue(false)));
						return;
					}

					if (pr.Value is IEnumerable)
					{
						var items      = (IEnumerable)pr.Value;
						var firstValue = true;

						if (p.Expr1 is ISqlTableSource)
						{
							var table = (ISqlTableSource)p.Expr1;
							var keys  = table.GetKeys(true);

							if (keys == null || keys.Count == 0)
								throw new SqlException("Cant create IN expression.");

							if (keys.Count == 1)
							{
								foreach (var item in items)
								{
									if (firstValue)
									{
										firstValue = false;
										BuildExpression(sb, GetPrecedence(p), keys[0]);
										sb.Append(p.IsNot ? " NOT IN (" : " IN (");
									}

									var field = GetUnderlayingField(keys[0]);
									var value = field.MemberMapper.GetValue(item);

									if (value is ISqlExpression)
										BuildExpression(sb, (ISqlExpression)value);
									else
										BuildValue(sb, value);

									sb.Append(", ");
								}
							}
							else
							{
								var len = sb.Length;
								var rem = 1;

								foreach (var item in items)
								{
									if (firstValue)
									{
										firstValue = false;
										sb.Append('(');
									}

									foreach (var key in keys)
									{
										var field = GetUnderlayingField(key);
										var value = field.MemberMapper.GetValue(item);

										BuildExpression(sb, GetPrecedence(p), key);

										if (value == null)
										{
											sb.Append(" IS NULL");
										}
										else
										{
											sb.Append(" = ");
											BuildValue(sb, value);
										}

										sb.Append(" AND ");
									}

									sb.Remove(sb.Length - 4, 4).Append("OR ");

									if (sb.Length - len >= 50)
									{
										sb.AppendLine();
										AppendIndent(sb);
										sb.Append(' ');
										len = sb.Length;
										rem = 5 + _indent;
									}
								}

								if (!firstValue)
									sb.Remove(sb.Length - rem, rem);
							}
						}
						else
							foreach (var item in items)
							{
								if (firstValue)
								{
									firstValue = false;
									BuildExpression(sb, GetPrecedence(p), p.Expr1);
									sb.Append(p.IsNot ? " NOT IN (" : " IN (");
								}

								if (item is ISqlExpression)
									BuildExpression(sb, (ISqlExpression)item);
								else
									BuildValue(sb, item);

								sb.Append(", ");
							}

						if (firstValue)
							BuildPredicate(sb, new SqlQuery.Predicate.Expr(new SqlValue(false)));
						else
							sb.Remove(sb.Length - 2, 2).Append(')');

						return;
					}
				}

				BuildExpression(sb, GetPrecedence(p), p.Expr1);
				sb.Append(p.IsNot ? " NOT IN (" : " IN (");

				foreach (var value in values)
				{
					if (value is ISqlExpression)
						BuildExpression(sb, (ISqlExpression)value);
					else
						BuildValue(sb, value);

					sb.Append(", ");
				}

				sb.Remove(sb.Length - 2, 2).Append(')');
			}
		}

		protected void BuildPredicate(StringBuilder sb, int parentPrecedence, ISqlPredicate predicate)
		{
			var wrap = Wrap(GetPrecedence(predicate), parentPrecedence);

			if (wrap) sb.Append('(');
			BuildPredicate(sb, predicate);
			if (wrap) sb.Append(')');
		}

		protected virtual void BuildLikePredicate(StringBuilder sb, SqlQuery.Predicate.Like predicate)
		{
			var precedence = GetPrecedence(predicate);

			BuildExpression(sb, precedence, predicate.Expr1);
			sb.Append(predicate.IsNot? " NOT LIKE ": " LIKE ");
			BuildExpression(sb, precedence, predicate.Expr2);

			if (predicate.Escape != null)
			{
				sb.Append(" ESCAPE ");
				BuildExpression(sb, predicate.Escape);
			}
		}

		#endregion

		#region BuildExpression

		protected virtual StringBuilder BuildExpression(
			StringBuilder  sb,
			ISqlExpression expr,
			bool           buildTableName,
			bool           checkParentheses,
			string         alias,
			ref bool       addAlias)
		{
			expr = ConvertExpression(expr);

			switch (expr.ElementType)
			{
				case QueryElementType.SqlField:
					{
						var field = (SqlField)expr;
						if (field == field.Table.All)
						{
							sb.Append("*");
						}
						else
						{
							if (buildTableName)
							{
								var ts = _sqlQuery.GetTableSource(field.Table);

								if (ts == null)
									throw new SqlException(string.Format("Table {0} not found.", field.Table));

								var table = GetTableAlias(ts);

								table = table == null ?
									GetTablePhysicalName(field.Table) :
									DataProvider.Convert(table, ConvertType.NameToQueryTableAlias).ToString();

								if (string.IsNullOrEmpty(table))
									throw new SqlException(string.Format("Table {0} should have an alias.", field.Table));

								addAlias = alias != field.PhysicalName;

								sb
									.Append(table)
									.Append('.');
							}

							sb.Append(_dataProvider.Convert(field.PhysicalName, ConvertType.NameToQueryField));
						}
					}

					break;

				case QueryElementType.Column:
					{
						var column = (SqlQuery.Column)expr;

#if DEBUG
						//if (column.ToString() == "t8.ParentID")
						//{
						//    column.ToString();
						//}
#endif

						var table  = _sqlQuery.GetTableSource(column.Parent);

						if (table == null)
							throw new SqlException(string.Format("Table not found for '{0}'.", column));

						var tableAlias = GetTableAlias(table) ?? GetTablePhysicalName(column.Parent);

						if (string.IsNullOrEmpty(tableAlias))
							throw new SqlException(string.Format("Table {0} should have an alias.", column.Parent));

						addAlias = alias != column.Alias;

						sb
							.Append(DataProvider.Convert(tableAlias, ConvertType.NameToQueryTableAlias))
							.Append('.')
							.Append(_dataProvider.Convert(column.Alias, ConvertType.NameToQueryField));
					}

					break;

				case QueryElementType.SqlQuery:
					{
						var hasParentheses = checkParentheses && sb[sb.Length - 1] == '(';

						if (!hasParentheses)
							sb.Append("(");
						sb.AppendLine();

						_nextNesting = BuildSqlBuilder((SqlQuery)expr, sb, _indent + 1, _nextNesting, _buildStep != Step.FromClause);

						AppendIndent(sb);

						if (!hasParentheses)
							sb.Append(")");
					}

					break;

				case QueryElementType.SqlValue:
					BuildValue(sb, ((SqlValue)expr).Value);
					break;

				case QueryElementType.SqlExpression:
					{
						var e = (SqlExpression)expr;
						var s = new StringBuilder();

						if (e.Parameters == null || e.Parameters.Length == 0)
							sb.Append(e.Expr);
						else
						{
							var values = new object[e.Parameters.Length];

							for (var i = 0; i < values.Length; i++)
							{
								var value = e.Parameters[i];

								s.Length = 0;
								BuildExpression(s, GetPrecedence(e), value);
								values[i] = s.ToString();
							}

							sb.AppendFormat(e.Expr, values);
						}
					}

					break;

				case QueryElementType.SqlBinaryExpression:
					BuildBinaryExpression(sb, (SqlBinaryExpression)expr);
					break;

				case QueryElementType.SqlFunction:
					BuildFunction(sb, (SqlFunction)expr);
					break;

				case QueryElementType.SqlParameter:
					{
						var parm = (SqlParameter)expr;

						if (parm.IsQueryParameter)
							sb.Append(_dataProvider.Convert(parm.Name, ConvertType.NameToQueryParameter));
						else
							BuildValue(sb, parm.Value);
					}

					break;

				case QueryElementType.SqlDataType:
					BuildDataType(sb, (SqlDataType)expr);
					break;

				case QueryElementType.SearchCondition:
					BuildSearchCondition(sb, expr.Precedence, (SqlQuery.SearchCondition)expr);
					break;

				default:
					throw new InvalidOperationException();
			}

			return sb;
		}

		protected void BuildExpression(StringBuilder sb, int parentPrecedence, ISqlExpression expr, string alias, ref bool addAlias)
		{
			var wrap = Wrap(GetPrecedence(expr), parentPrecedence);

			if (wrap) sb.Append('(');
			BuildExpression(sb, expr, true, true, alias, ref addAlias);
			if (wrap) sb.Append(')');
		}

		protected StringBuilder BuildExpression(StringBuilder sb, ISqlExpression expr)
		{
			var dummy = false;
			return BuildExpression(sb, expr, true, true, null, ref dummy);
		}

		protected StringBuilder BuildExpression(StringBuilder sb, ISqlExpression expr, bool buildTableName, bool checkParentheses)
		{
			var dummy = false;
			return BuildExpression(sb, expr, buildTableName, checkParentheses, null, ref dummy);
		}

		protected void BuildExpression(StringBuilder sb, int precedence, ISqlExpression expr)
		{
			var dummy = false;
			BuildExpression(sb, precedence, expr, null, ref dummy);
		}

		#endregion

		#region BuildValue

		interface INullableValueReader
		{
			object GetValue(object value);
		}

		class NullableValueReader<T> : INullableValueReader where T : struct
		{
			public object GetValue(object value)
			{
				return ((T?)value).Value;
			}
		}

		static readonly Dictionary<Type,INullableValueReader> _nullableValueReader = new Dictionary<Type,INullableValueReader>();

		protected virtual void BuildValue(StringBuilder sb, object value)
		{
			if      (value == null)                   sb.Append("NULL");
			else if (value is string)                 BuildString(sb, value.ToString());
			else if (value is char || value is char?) sb.Append('\'').Append(value.ToString().Replace("'", "''")).Append('\'');
			else if (value is bool || value is bool?) sb.Append((bool)value ? "1" : "0");
			else if (value is DateTime)               sb.AppendFormat("'{0:yyyy-MM-dd HH:mm:ss.fffffff}'", value);
			else if (value is Guid)                   sb.Append('\'').Append(value).Append('\'');
			else if (value is decimal)                sb.Append(((decimal)value).ToString(NumberFormatInfo.InvariantInfo));
			else if (value is double)                 sb.Append(((double) value).ToString(NumberFormatInfo.InvariantInfo));
			else if (value is float)                  sb.Append(((float)  value).ToString(NumberFormatInfo.InvariantInfo));
			else
			{
				var type = value.GetType();

				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					type = type.GetGenericArguments()[0];

					if (type.IsEnum)
					{
						lock (_nullableValueReader)
						{
							INullableValueReader reader;

							if (_nullableValueReader.TryGetValue(type, out reader) == false)
							{
								reader = (INullableValueReader)Activator.CreateInstance(typeof(NullableValueReader<>).MakeGenericType(type));
								_nullableValueReader.Add(type, reader);
							}

							value = reader.GetValue(value);
						}
					}
				}

				if (type.IsEnum)
				{
					value = Map.EnumToValue(value);

					if (value != null && !value.GetType().IsEnum)
						BuildValue(sb, value);
					else
						sb.Append(value);
				}
				else
					sb.Append(value);
			}
		}

		protected virtual void BuildString(StringBuilder sb, string value)
		{
			for (var i = 0; i < value.Length; i++)
			{
				if (value[i] > 127)
				{
					BuildUnicodeString(sb, value);
					return;
				}
			}

			sb
				.Append('\'')
				.Append(value.Replace("'", "''"))
				.Append('\'');
		}

		protected virtual void BuildUnicodeString(StringBuilder sb, string value)
		{
			sb
				.Append('\'')
				.Append(value.Replace("'", "''"))
				.Append("\'");
		}

		#endregion

		#region BuildBinaryExpression

		protected virtual void BuildBinaryExpression(StringBuilder sb, SqlBinaryExpression expr)
		{
			BuildBinaryExpression(sb, expr.Operation, expr);
		}

		protected void BuildFunction(StringBuilder sb, string name, SqlBinaryExpression expr)
		{
			sb.Append(name);
			sb.Append("(");
			BuildExpression(sb, expr.Expr1);
			sb.Append(", ");
			BuildExpression(sb, expr.Expr2);
			sb.Append(')');
		}

		protected void BuildBinaryExpression(StringBuilder sb, string op, SqlBinaryExpression expr)
		{
			if (expr.Operation == "*" && expr.Expr1 is SqlValue)
			{
				var value = (SqlValue)expr.Expr1;

				if (value.Value is int && (int)value.Value == -1)
				{
					sb.Append('-');
					BuildExpression(sb, GetPrecedence(expr), expr.Expr2);
					return;
				}
			}

			BuildExpression(sb, GetPrecedence(expr), expr.Expr1);
			sb.Append(' ').Append(op).Append(' ');
			BuildExpression(sb, GetPrecedence(expr), expr.Expr2);
		}

		#endregion

		#region BuildFunction

		protected virtual void BuildFunction(StringBuilder sb, SqlFunction func)
		{
			if (func.Name == "CASE")
			{
				sb.Append(func.Name).AppendLine();

				_indent++;

				var i = 0;

				for (; i < func.Parameters.Length - 1; i += 2)
				{
					AppendIndent(sb).Append("WHEN ");

					var len = sb.Length;

					BuildExpression(sb, func.Parameters[i]);

					if (SqlExpression.NeedsEqual(func.Parameters[i]))
					{
						sb.Append(" = ");
						BuildValue(sb, true);
					}

					if (sb.Length - len > 20)
					{
						sb.AppendLine();
						AppendIndent(sb).Append("\tTHEN ");
					}
					else
						sb.Append(" THEN ");

					BuildExpression(sb, func.Parameters[i+1]);
					sb.AppendLine();
				}

				if (i < func.Parameters.Length)
				{
					AppendIndent(sb).Append("ELSE ");
					BuildExpression(sb, func.Parameters[i]);
					sb.AppendLine();
				}

				_indent--;

				AppendIndent(sb).Append("END");
			}
			else
				BuildFunction(sb, func.Name, func.Parameters);
		}

		protected void BuildFunction(StringBuilder sb, string name, ISqlExpression[] exprs)
		{
			sb.Append(name).Append('(');

			var first = true;

			foreach (var parameter in exprs)
			{
				if (!first)
					sb.Append(", ");

				BuildExpression(sb, parameter, true, !first || name == "EXISTS");

				first = false;
			}

			sb.Append(')');
		}

		#endregion

		#region BuildDataType
	
		protected virtual void BuildDataType(StringBuilder sb, SqlDataType type)
		{
			sb.Append(type.DbType.ToString());

			if (type.Length > 0)
				sb.Append('(').Append(type.Length).Append(')');

			if (type.Precision > 0)
				sb.Append('(').Append(type.Precision).Append(',').Append(type.Scale).Append(')');
		}

		#endregion

		#region GetPrecedence

		protected virtual int GetPrecedence(ISqlExpression expr)
		{
			return expr.Precedence;
		}

		protected virtual int GetPrecedence(ISqlPredicate predicate)
		{
			return predicate.Precedence;
		}

		#endregion

		#endregion

		#region Internal Types

		public enum Step
		{
			SelectClause,
			DeleteClause,
			UpdateClause,
			InsertClause,
			FromClause,
			WhereClause,
			GroupByClause,
			HavingClause,
			OrderByClause,
			OffsetLimit
		}

		#endregion

		#region Alternative Builders

		protected virtual void BuildAliases(StringBuilder sb, string table, List<SqlQuery.Column> columns, string postfix)
		{
			_indent++;

			var first = true;

			foreach (var col in columns)
			{
				if (!first)
					sb.Append(',').AppendLine();
				first = false;

				AppendIndent(sb).AppendFormat("{0}.{1}", table, DataProvider.Convert(col.Alias, ConvertType.NameToQueryFieldAlias));

				if (postfix != null)
					sb.Append(postfix);
			}

			_indent--;

			sb.AppendLine();
		}

		protected void AlternativeBuildSql(StringBuilder sb, bool implementOrderBy, Action<StringBuilder> buildSql)
		{
			if (NeedSkip)
			{
				var aliases  = GetTempAliases(2, "t");
				var rnaliase = GetTempAliases(1, "rn")[0];

				AppendIndent(sb).Append("SELECT *").AppendLine();
				AppendIndent(sb).Append("FROM").    AppendLine();
				AppendIndent(sb).Append("(").       AppendLine();
				_indent++;

				AppendIndent(sb).Append("SELECT").AppendLine();

				_indent++;
				AppendIndent(sb).AppendFormat("{0}.*,", aliases[0]).AppendLine();
				AppendIndent(sb).Append("ROW_NUMBER() OVER");

				if (!SqlQuery.OrderBy.IsEmpty && !implementOrderBy)
					sb.Append("()");
				else
				{
					sb.AppendLine();
					AppendIndent(sb).Append("(").AppendLine();

					_indent++;

					if (SqlQuery.OrderBy.IsEmpty)
					{
						AppendIndent(sb).Append("ORDER BY").AppendLine();
						BuildAliases(sb, aliases[0], SqlQuery.Select.Columns, null);
					}
					else
						BuildAlternativeOrderBy(sb, true);

					_indent--;
					AppendIndent(sb).Append(")");
				}

				sb.Append(" as ").Append(rnaliase).AppendLine();
				_indent--;

				AppendIndent(sb).Append("FROM").AppendLine();
				AppendIndent(sb).Append("(").AppendLine();

				_indent++;
				buildSql(sb);
				_indent--;

				AppendIndent(sb).AppendFormat(") {0}", aliases[0]).AppendLine();

				_indent--;

				AppendIndent(sb).AppendFormat(") {0}", aliases[1]).AppendLine();
				AppendIndent(sb).Append("WHERE").AppendLine();

				_indent++;

				if (NeedTake)
				{
					var expr1 = Add(SqlQuery.Select.SkipValue, 1);
					var expr2 = Add<int>(SqlQuery.Select.SkipValue, SqlQuery.Select.TakeValue);

					if (expr1 is SqlValue && expr2 is SqlValue && Equals(((SqlValue)expr1).Value, ((SqlValue)expr2).Value))
					{
						AppendIndent(sb).AppendFormat("{0}.{1} = ", aliases[1], rnaliase);
						BuildExpression(sb, expr1);
					}
					else
					{
						AppendIndent(sb).AppendFormat("{0}.{1} BETWEEN ", aliases[1], rnaliase);
						BuildExpression(sb, expr1);
						sb.Append(" AND ");
						BuildExpression(sb, expr2);
					}
				}
				else
				{
					AppendIndent(sb).AppendFormat("{0}.{1} > ", aliases[1], rnaliase);
					BuildExpression(sb, SqlQuery.Select.SkipValue);
				}

				sb.AppendLine();
				_indent--;
			}
			else
				buildSql(sb);
		}

		protected void AlternativeBuildSql2(StringBuilder sb, Action<StringBuilder> buildSql)
		{
			var aliases = GetTempAliases(3, "t");

			AppendIndent(sb).Append("SELECT *").AppendLine();
			AppendIndent(sb).Append("FROM")    .AppendLine();
			AppendIndent(sb).Append("(")       .AppendLine();
			_indent++;

			AppendIndent(sb).Append("SELECT TOP ");
			BuildExpression(sb, SqlQuery.Select.TakeValue);
			sb.Append(" *").AppendLine();
			AppendIndent(sb).Append("FROM").AppendLine();
			AppendIndent(sb).Append("(")   .AppendLine();
			_indent++;

			if (SqlQuery.OrderBy.IsEmpty)
			{
				AppendIndent(sb).Append("SELECT TOP ");

				var p = SqlQuery.Select.SkipValue as SqlParameter;

				if (p != null && !p.IsQueryParameter && SqlQuery.Select.TakeValue is SqlValue)
					BuildValue(sb, (int)p.Value + (int)((SqlValue)(SqlQuery.Select.TakeValue)).Value);
				else
					BuildExpression(sb, Add<int>(SqlQuery.Select.SkipValue, SqlQuery.Select.TakeValue));

				sb.Append(" *").AppendLine();
				AppendIndent(sb).Append("FROM").AppendLine();
				AppendIndent(sb).Append("(")   .AppendLine();
				_indent++;
			}

			buildSql(sb);

			if (SqlQuery.OrderBy.IsEmpty)
			{
				_indent--;
				AppendIndent(sb).AppendFormat(") {0}", aliases[2]).AppendLine();
				AppendIndent(sb).Append("ORDER BY").AppendLine();
				BuildAliases(sb, aliases[2], SqlQuery.Select.Columns, null);
			}

			_indent--;
			AppendIndent(sb).AppendFormat(") {0}", aliases[1]).AppendLine();

			if (SqlQuery.OrderBy.IsEmpty)
			{
				AppendIndent(sb).Append("ORDER BY").AppendLine();
				BuildAliases(sb, aliases[1], SqlQuery.Select.Columns, " DESC");
			}
			else
			{
				BuildAlternativeOrderBy(sb, false);
			}

			_indent--;
			AppendIndent(sb).AppendFormat(") {0}", aliases[0]).AppendLine();

			if (SqlQuery.OrderBy.IsEmpty)
			{
				AppendIndent(sb).Append("ORDER BY").AppendLine();
				BuildAliases(sb, aliases[0], SqlQuery.Select.Columns, null);
			}
			else
			{
				BuildAlternativeOrderBy(sb, true);
			}
		}

		protected void BuildAlternativeOrderBy(StringBuilder sb, bool ascending)
		{
			AppendIndent(sb).Append("ORDER BY").AppendLine();

			var obys = GetTempAliases(SqlQuery.OrderBy.Items.Count, "oby");

			_indent++;

			for (var i = 0; i < obys.Length; i++)
			{
				AppendIndent(sb).Append(obys[i]);

				if ( ascending &&  SqlQuery.OrderBy.Items[i].IsDescending ||
					!ascending && !SqlQuery.OrderBy.Items[i].IsDescending)
					sb.Append(" DESC");

				if (i + 1 < obys.Length)
					sb.Append(',');

				sb.AppendLine();
			}

			_indent--;
		}

		protected delegate IEnumerable<SqlQuery.Column> ColumnSelector();

		protected IEnumerable<SqlQuery.Column> AlternativeGetSelectedColumns(ColumnSelector columnSelector)
		{
			foreach (var col in columnSelector())
				yield return col;

			var obys = GetTempAliases(SqlQuery.OrderBy.Items.Count, "oby");

			for (var i = 0; i < obys.Length; i++)
				yield return new SqlQuery.Column(SqlQuery, SqlQuery.OrderBy.Items[i].Expression, obys[i]);
		}

		protected bool IsDateDataType(ISqlExpression expr, string dateName)
		{
			switch (expr.ElementType)
			{
				case QueryElementType.SqlDataType   : return ((SqlDataType)expr).DbType == SqlDbType.Date;
				case QueryElementType.SqlExpression : return ((SqlExpression)expr).Expr == dateName;
			}

			return false;
		}

		protected bool IsTimeDataType(ISqlExpression expr)
		{
			switch (expr.ElementType)
			{
				case QueryElementType.SqlDataType   : return ((SqlDataType)expr).DbType == SqlDbType.Time;
				case QueryElementType.SqlExpression : return ((SqlExpression)expr).Expr == "Time";
			}

			return false;
		}

		protected ISqlExpression FloorBeforeConvert(SqlFunction func)
		{
			var par1 = func.Parameters[1];

			return TypeHelper.IsFloatType(par1.SystemType) && TypeHelper.IsIntegerType(func.SystemType) ?
				new SqlFunction(func.SystemType, "Floor", par1) : par1;
		}

		protected ISqlExpression AlternativeConvertToBoolean(SqlFunction func, int paramNumber)
		{
			var par = func.Parameters[paramNumber];

			if (TypeHelper.IsFloatType(par.SystemType) || TypeHelper.IsIntegerType(par.SystemType))
			{
				var sc = new SqlQuery.SearchCondition();

				sc.Conditions.Add(
					new SqlQuery.Condition(false, new SqlQuery.Predicate.ExprExpr(par, SqlQuery.Predicate.Operator.Equal, new SqlValue(0))));

				return ConvertExpression(new SqlFunction(func.SystemType, "CASE", sc, new SqlValue(false), new SqlValue(true)));
			}

			return null;
		}

		protected SqlQuery GetAlternativeDelete(SqlQuery sqlQuery)
		{
			if (sqlQuery.QueryType == QueryType.Delete && 
				(sqlQuery.From.Tables.Count > 1 || sqlQuery.From.Tables[0].Joins.Count > 0) && 
				sqlQuery.From.Tables[0].Source is SqlTable)
			{
				var sql = new SqlQuery { QueryType = QueryType.Delete };

				sqlQuery.ParentSql = sql;
				sqlQuery.QueryType = QueryType.Select;

				var table = (SqlTable)sqlQuery.From.Tables[0].Source;
				var copy  = new SqlTable(table) { Alias = null };

				var tableKeys = table.GetKeys(true);
				var copyKeys  = copy. GetKeys(true);

				for (var i = 0; i < tableKeys.Count; i++)
					sqlQuery.Where
						.Expr(copyKeys[i]).Equal.Expr(tableKeys[i]);

				sql.From.Table(copy).Where.Exists(sqlQuery);
				sql.Parameters.AddRange(sqlQuery.Parameters);

				sqlQuery.Parameters.Clear();

				sqlQuery = sql;
			}

			return sqlQuery;
		}

		protected SqlQuery GetAlternativeUpdate(SqlQuery sqlQuery)
		{
			if (sqlQuery.QueryType == QueryType.Update && sqlQuery.From.Tables[0].Source is SqlTable)
			{
				if (sqlQuery.From.Tables.Count > 1 || sqlQuery.From.Tables[0].Joins.Count > 0)
				{
					var sql = new SqlQuery { QueryType = QueryType.Update };

					sqlQuery.ParentSql = sql;
					sqlQuery.QueryType = QueryType.Select;

					var table = (SqlTable)sqlQuery.From.Tables[0].Source;
					var copy  = new SqlTable(table);

					var tableKeys = table.GetKeys(true);
					var copyKeys  = copy. GetKeys(true);

					for (var i = 0; i < tableKeys.Count; i++)
						sqlQuery.Where
							.Expr(copyKeys[i]).Equal.Expr(tableKeys[i]);

					sql.From.Table(copy).Where.Exists(sqlQuery);

					var map = new Dictionary<SqlField, SqlField>(table.Fields.Count);

					foreach (var field in table.Fields.Values)
						map.Add(field, copy[field.Name]);

					foreach (var item in sqlQuery.Set.Items)
					{
						((ISqlExpressionWalkable)item).Walk(false, expr =>
						{
							var fld = expr as SqlField;
							return fld != null && map.TryGetValue(fld, out fld) ? fld : expr;
						});

						sql.Set.Items.Add(item);
					}

					sql.Parameters.AddRange(sqlQuery.Parameters);

					sqlQuery.Parameters.Clear();
					sqlQuery.Set.Items.Clear();

					sqlQuery = sql;
				}

				sqlQuery.From.Tables[0].Alias = "$";
			}

			return sqlQuery;
		}

		#endregion

		#region Helpers

		protected SqlField GetIdentityField(SqlTable table)
		{
			foreach (var field in table.Fields.Values)
				if (field.IsIdentity)
					return field;

			var keys = table.GetKeys(true);

			if (keys != null && keys.Count == 1)
				return (SqlField)keys[0];

			return null;
		}

		protected SequenceNameAttribute GetSequenceNameAttribute()
		{
			var table         = SqlQuery.Set.Into;
			var identityField = GetIdentityField(table);

			if (identityField == null)
				throw new SqlException("Identity field must be defined for '{0}'.", table.Name);

			if (table.ObjectType == null)
				throw new SqlException("Sequence name can not be retrieved for the '{0}' table.", table.Name);

			var om = table.MappingSchema.GetObjectMapper(table.ObjectType);
			var mm = om[identityField.Name, true];

			var attrs = mm.MapMemberInfo.MemberAccessor.GetAttributes<SequenceNameAttribute>();

			if (attrs == null)
				throw new SqlException("Sequence name can not be retrieved for the '{0}' table.", table.Name);

			SequenceNameAttribute defaultAttr = null;

			foreach (var attr in attrs)
			{
				if (attr.ProviderName == Name)
					return attr;

				if (defaultAttr == null && attr.ProviderName == null)
					defaultAttr = attr;
			}

			if (defaultAttr == null)
				throw new SqlException("Sequence name can not be retrieved for the '{0}' table.", table.Name);

			return defaultAttr;
		}

		static string SetAlias(string alias)
		{
			if (alias == null)
				return null;

			alias = alias.TrimStart('_');

			var cs      = alias.ToCharArray();
			var replace = false;

			for (var i = 0; i < cs.Length; i++)
			{
				var c = cs[i];

				if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c >= '0' && c <= '9' || c == '_')
					continue;

				cs[i] = ' ';
				replace = true;
			}

			if (replace)
				alias = new string(cs).Replace(" ", "");

			return alias.Length == 0 ? null : alias;
		}

		protected void CheckAliases(SqlQuery sqlQuery)
		{
			new QueryVisitor().Visit(sqlQuery, delegate(IQueryElement e)
			{
				switch (e.ElementType)
				{
					case QueryElementType.SqlField     : ((SqlField)            e).Alias = SetAlias(((SqlField)            e).Alias); break;
					case QueryElementType.SqlParameter : ((SqlParameter)        e).Name  = SetAlias(((SqlParameter)        e).Name);  break;
					case QueryElementType.SqlTable     : ((SqlTable)            e).Alias = SetAlias(((SqlTable)            e).Alias); break;
					case QueryElementType.Join         : ((Join)                e).Alias = SetAlias(((Join)                e).Alias); break;
					case QueryElementType.Column       : ((SqlQuery.Column)     e).Alias = SetAlias(((SqlQuery.Column)     e).Alias); break;
					case QueryElementType.TableSource  : ((SqlQuery.TableSource)e).Alias = SetAlias(((SqlQuery.TableSource)e).Alias); break;
				}
			});
		}

		static bool Wrap(int precedence, int parentPrecedence)
		{
			return
				precedence == 0 ||
				precedence < parentPrecedence ||
				(precedence == parentPrecedence && 
					(parentPrecedence == Precedence.Subtraction ||
					 parentPrecedence == Precedence.LogicalNegation));
		}

		protected string[] GetTempAliases(int n, string defaultAlias)
		{
			return SqlQuery.GetTempAliases(n, defaultAlias + (Nesting == 0? "": "n" + Nesting));
		}

		protected static string GetTableAlias(ISqlTableSource table)
		{
			switch (table.ElementType)
			{
				case QueryElementType.TableSource :
					var ts    = (SqlQuery.TableSource)table;
					var alias = string.IsNullOrEmpty(ts.Alias) ? GetTableAlias(ts.Source) : ts.Alias;
					return alias != "$" ? alias : null;

				case QueryElementType.SqlTable :
					return ((SqlTable)table).Alias;

				default :
					throw new InvalidOperationException();
			}
		}

		string GetTablePhysicalName(ISqlTableSource table)
		{
			switch (table.ElementType)
			{
				case QueryElementType.SqlTable :
					{
						var tbl = (SqlTable)table;

						return _dataProvider.BuildTableName(new StringBuilder(), 
							tbl.Database     == null ? null : _dataProvider.Convert(tbl.Database,     ConvertType.NameToDatabase).  ToString(),
							tbl.Owner        == null ? null : _dataProvider.Convert(tbl.Owner,        ConvertType.NameToOwner).     ToString(),
							tbl.PhysicalName == null ? null : _dataProvider.Convert(tbl.PhysicalName, ConvertType.NameToQueryTable).ToString()).ToString();
					}

				case QueryElementType.TableSource :
					return GetTablePhysicalName(((SqlQuery.TableSource)table).Source);

				default :
					throw new InvalidOperationException();
			}
		}

		protected StringBuilder AppendIndent(StringBuilder sb)
		{
			if (_indent > 0)
				sb.Append('\t', _indent);

			return sb;
		}

		public ISqlExpression Add(ISqlExpression expr1, ISqlExpression expr2, Type type)
		{
			return ConvertExpression(new SqlBinaryExpression(type, expr1, "+", expr2, Precedence.Additive));
		}

		public ISqlExpression Add<T>(ISqlExpression expr1, ISqlExpression expr2)
		{
			return Add(expr1, expr2, typeof(T));
		}

		public ISqlExpression Add(ISqlExpression expr1, int value)
		{
			return Add<int>(expr1, new SqlValue(value));
		}

		public ISqlExpression Inc(ISqlExpression expr1)
		{
			return Add(expr1, 1);
		}

		public ISqlExpression Sub(ISqlExpression expr1, ISqlExpression expr2, Type type)
		{
			return ConvertExpression(new SqlBinaryExpression(type, expr1, "-", expr2, Precedence.Subtraction));
		}

		public ISqlExpression Sub<T>(ISqlExpression expr1, ISqlExpression expr2)
		{
			return Sub(expr1, expr2, typeof(T));
		}

		public ISqlExpression Sub(ISqlExpression expr1, int value)
		{
			return Sub<int>(expr1, new SqlValue(value));
		}

		public ISqlExpression Dec(ISqlExpression expr1)
		{
			return Sub(expr1, 1);
		}

		public ISqlExpression Mul(ISqlExpression expr1, ISqlExpression expr2, Type type)
		{
			return ConvertExpression(new SqlBinaryExpression(type, expr1, "*", expr2, Precedence.Multiplicative));
		}

		public ISqlExpression Mul<T>(ISqlExpression expr1, ISqlExpression expr2)
		{
			return Mul(expr1, expr2, typeof(T));
		}

		public ISqlExpression Mul(ISqlExpression expr1, int value)
		{
			return Mul<int>(expr1, new SqlValue(value));
		}

		public ISqlExpression Div(ISqlExpression expr1, ISqlExpression expr2, Type type)
		{
			return ConvertExpression(new SqlBinaryExpression(type, expr1, "/", expr2, Precedence.Multiplicative));
		}

		public ISqlExpression Div<T>(ISqlExpression expr1, ISqlExpression expr2)
		{
			return Div(expr1, expr2, typeof(T));
		}

		public ISqlExpression Div(ISqlExpression expr1, int value)
		{
			return Div<int>(expr1, new SqlValue(value));
		}

		#endregion

		#region DataTypes

		protected virtual int GetMaxLength     (SqlDataType type) { return SqlDataType.GetMaxLength     (type.DbType); }
		protected virtual int GetMaxPrecision  (SqlDataType type) { return SqlDataType.GetMaxPrecision  (type.DbType); }
		protected virtual int GetMaxScale      (SqlDataType type) { return SqlDataType.GetMaxScale      (type.DbType); }
		protected virtual int GetMaxDisplaySize(SqlDataType type) { return SqlDataType.GetMaxDisplaySize(type.DbType); }

		protected virtual ISqlExpression ConvertConvertion(SqlFunction func)
		{
			var from = (SqlDataType)func.Parameters[1];
			var to   = (SqlDataType)func.Parameters[0];

			if (to.Type == typeof(object))
				return func.Parameters[2];

			if (to.Precision > 0)
			{
				var maxPrecision = GetMaxPrecision(from);
				var maxScale     = GetMaxScale    (from);
				var newPrecision = maxPrecision >= 0 ? Math.Min(to.Precision, maxPrecision) : to.Precision;
				var newScale     = maxScale     >= 0 ? Math.Min(to.Scale,     maxScale)     : to.Scale;

				if (to.Precision != newPrecision || to.Scale != newScale)
					to = new SqlDataType(to.DbType, to.Type, newPrecision, newScale);
			}
			else if (to.Length > 0)
			{
				var maxLength = to.Type == typeof(string) ? GetMaxDisplaySize(from) : GetMaxLength(from);
				var newLength = maxLength >= 0 ? Math.Min(to.Length, maxLength) : to.Length;

				if (to.Length != newLength)
					to = new SqlDataType(to.DbType, to.Type, newLength);
			}
			else if (from.Type == typeof(short) && to.Type == typeof(int))
				return func.Parameters[2];

			return ConvertExpression(new SqlFunction(func.SystemType, "Convert", to, func.Parameters[2]));
		}

		#endregion

		#region ISqlProvider Members

		public virtual ISqlExpression ConvertExpression(ISqlExpression expression)
		{
			switch (expression.ElementType)
			{
				case QueryElementType.SqlBinaryExpression:

					#region SqlBinaryExpression

					{
						var be = (SqlBinaryExpression)expression;

						switch (be.Operation)
						{
							case "+":
								if (be.Expr1 is SqlValue)
								{
									var v1 = (SqlValue)be.Expr1;
									if (v1.Value is int    && (int)   v1.Value == 0 ||
										v1.Value is string && (string)v1.Value == "") return be.Expr2;
								}

								if (be.Expr2 is SqlValue)
								{
									var v2 = (SqlValue) be.Expr2;

									if (v2.Value is int)
									{
										if ((int)v2.Value == 0) return be.Expr1;

										if (be.Expr1 is SqlBinaryExpression)
										{
											var be1 = (SqlBinaryExpression) be.Expr1;

											if (be1.Expr2 is SqlValue)
											{
												var be1v2 = (SqlValue)be1.Expr2;

												if (be1v2.Value is int)
												{
													switch (be1.Operation)
													{
														case "+":
															{
																var value = (int)be1v2.Value + (int)v2.Value;
																var oper  = be1.Operation;

																if (value < 0)
																{
																	value = - value;
																	oper  = "-";
																}

																return new SqlBinaryExpression(be.SystemType, be1.Expr1, oper, new SqlValue(value), be.Precedence);
															}

														case "-":
															{
																var value = (int)be1v2.Value - (int)v2.Value;
																var oper  = be1.Operation;

																if (value < 0)
																{
																	value = - value;
																	oper  = "+";
																}

																return new SqlBinaryExpression(be.SystemType, be1.Expr1, oper, new SqlValue(value), be.Precedence);
															}
													}
												}
											}
										}
									}
									else if (v2.Value is string)
									{
										if ((string)v2.Value == "") return be.Expr1;

										if (be.Expr1 is SqlBinaryExpression)
										{
											var be1 = (SqlBinaryExpression)be.Expr1;

											if (be1.Expr2 is SqlValue)
											{
												var value = ((SqlValue)be1.Expr2).Value;

												if (value is string)
													return new SqlBinaryExpression(
														be1.SystemType,
														be1.Expr1,
														be1.Operation,
														new SqlValue(string.Concat(value, v2.Value)));
											}
										}
									}
								}

								if (be.Expr1 is SqlValue && be.Expr2 is SqlValue)
								{
									var v1 = (SqlValue)be.Expr1;
									var v2 = (SqlValue)be.Expr2;
									if (v1.Value is int    && v2.Value is int)    return new SqlValue((int)v1.Value + (int)v2.Value);
									if (v1.Value is string || v2.Value is string) return new SqlValue(v1.Value.ToString() + v2.Value);
								}

								if (be.Expr1.SystemType == typeof(string) && be.Expr2.SystemType != typeof(string))
								{
									var len = be.Expr2.SystemType == null ? 100 : SqlDataType.GetMaxDisplaySize(SqlDataType.GetDataType(be.Expr2.SystemType).DbType);

									if (len <= 0)
										len = 100;

									return new SqlBinaryExpression(
										be.SystemType,
										be.Expr1,
										be.Operation,
										ConvertExpression(new SqlFunction(typeof(string), "Convert", new SqlDataType(SqlDbType.VarChar, len), be.Expr2)),
										be.Precedence);
								}

								if (be.Expr1.SystemType != typeof(string) && be.Expr2.SystemType == typeof(string))
								{
									var len = be.Expr1.SystemType == null ? 100 : SqlDataType.GetMaxDisplaySize(SqlDataType.GetDataType(be.Expr1.SystemType).DbType);

									if (len <= 0)
										len = 100;

									return new SqlBinaryExpression(
										be.SystemType,
										ConvertExpression(new SqlFunction(typeof(string), "Convert", new SqlDataType(SqlDbType.VarChar, len), be.Expr1)),
										be.Operation,
										be.Expr2,
										be.Precedence);
								}

								break;

							case "-":
								if (be.Expr2 is SqlValue)
								{
									var v2 = (SqlValue) be.Expr2;

									if (v2.Value is int)
									{
										if ((int)v2.Value == 0) return be.Expr1;

										if (be.Expr1 is SqlBinaryExpression)
										{
											var be1 = (SqlBinaryExpression)be.Expr1;

											if (be1.Expr2 is SqlValue)
											{
												var be1v2 = (SqlValue)be1.Expr2;

												if (be1v2.Value is int)
												{
													switch (be1.Operation)
													{
														case "+":
															{
																var value = (int)be1v2.Value - (int)v2.Value;
																var oper  = be1.Operation;

																if (value < 0)
																{
																	value = -value;
																	oper  = "-";
																}

																return new SqlBinaryExpression(be.SystemType, be1.Expr1, oper, new SqlValue(value), be.Precedence);
															}

														case "-":
															{
																var value = (int)be1v2.Value + (int)v2.Value;
																var oper  = be1.Operation;

																if (value < 0)
																{
																	value = -value;
																	oper  = "+";
																}

																return new SqlBinaryExpression(be.SystemType, be1.Expr1, oper, new SqlValue(value), be.Precedence);
															}
													}
												}
											}
										}
									}
								}

								if (be.Expr1 is SqlValue && be.Expr2 is SqlValue)
								{
									var v1 = (SqlValue)be.Expr1;
									var v2 = (SqlValue)be.Expr2;
									if (v1.Value is int && v2.Value is int) return new SqlValue((int)v1.Value - (int)v2.Value);
								}

								break;

							case "*":
								if (be.Expr1 is SqlValue)
								{
									var v1 = (SqlValue)be.Expr1;

									if (v1.Value is int)
									{
										var v1v = (int)v1.Value;

										switch (v1v)
										{
											case  0 : return new SqlValue(0);
											case  1 : return be.Expr2;
											default :
												{
													var be2 = be.Expr2 as SqlBinaryExpression;

													if (be2 != null && be2.Operation == "*" && be2.Expr1 is SqlValue)
													{
														var be2v1 = be2.Expr1 as SqlValue;

														if (be2v1.Value is int)
															return ConvertExpression(
																new SqlBinaryExpression(be2.SystemType, new SqlValue(v1v * (int)be2v1.Value), "*", be2.Expr2));
													}

													break;
												}

										}
									}
								}

								if (be.Expr2 is SqlValue)
								{
									var v2 = (SqlValue)be.Expr2;
									if (v2.Value is int && (int)v2.Value == 1) return be.Expr1;
									if (v2.Value is int && (int)v2.Value == 0) return new SqlValue(0);
								}

								if (be.Expr1 is SqlValue && be.Expr2 is SqlValue)
								{
									var v1 = (SqlValue)be.Expr1;
									var v2 = (SqlValue)be.Expr2;

									if (v1.Value is int)
									{
										if (v2.Value is int)    return new SqlValue((int)   v1.Value * (int)   v2.Value);
										if (v2.Value is double) return new SqlValue((int)   v1.Value * (double)v2.Value);
									}
									else if (v1.Value is double)
									{
										if (v2.Value is int)    return new SqlValue((double)v1.Value * (int)   v2.Value);
										if (v2.Value is double) return new SqlValue((double)v1.Value * (double)v2.Value);
									}
								}

								break;
						}
					}

					#endregion

					break;

				case QueryElementType.SqlFunction:

					#region SqlFunction

					{
						var func = (SqlFunction)expression;

						switch (func.Name)
						{
							case "ConvertToCaseCompareTo":
								return ConvertExpression(new SqlFunction(func.SystemType, "CASE",
									new SqlQuery.SearchCondition().Expr(func.Parameters[0]). Greater .Expr(func.Parameters[1]).ToExpr(), new SqlValue(1),
									new SqlQuery.SearchCondition().Expr(func.Parameters[0]). Equal   .Expr(func.Parameters[1]).ToExpr(), new SqlValue(0),
									new SqlValue(-1)));

							case "$Convert$": return ConvertConvertion(func);
							case "Average"  : return new SqlFunction(func.SystemType, "Avg", func.Parameters);

							case "CASE":
								{
									var parms = func.Parameters;
									var len   = parms.Length;

									for (var i = 0; i < parms.Length - 1; i += 2)
									{
										var value = parms[i] as SqlValue;

										if (value != null)
										{
											if ((bool)value.Value == false)
											{
												var newParms = new ISqlExpression[parms.Length - 2];

												if (i != 0)
													Array.Copy(parms, 0, newParms, 0, i);

												Array.Copy(parms, i + 2, newParms, i, parms.Length - i - 2);

												parms = newParms;
												i -= 2;
											}
											else
											{
												var newParms = new ISqlExpression[i + 1];

												if (i != 0)
													Array.Copy(parms, 0, newParms, 0, i);

												newParms[i] = parms[i + 1];

												parms = newParms;
												break;
											}
										}
									}

									if (parms.Length == 1)
										return parms[0];

									if (parms.Length != len)
										return new SqlFunction(func.SystemType, func.Name, func.Precedence, parms);
								}

								break;

							case "Convert":
								{
									var from  = func.Parameters[1] as SqlFunction;
									var typef = TypeHelper.GetUnderlyingType(func.SystemType);

									if (from != null && from.Name == "Convert" && TypeHelper.GetUnderlyingType(from.Parameters[1].SystemType) == typef)
										return from.Parameters[1];

									var fe = func.Parameters[1] as SqlExpression;

									if (fe != null && fe.Expr == "Cast({0} as {1})" && TypeHelper.GetUnderlyingType(fe.Parameters[0].SystemType) == typef)
										return fe.Parameters[0];
								}

								break;
						}
					}

					#endregion

					break;

				case QueryElementType.SearchCondition:
					SqlQuery.OptimizeSearchCondition((SqlQuery.SearchCondition)expression);
					break;
			}

			return expression;
		}

		public virtual ISqlPredicate ConvertPredicate(ISqlPredicate predicate)
		{
			switch (predicate.ElementType)
			{
				case QueryElementType.ExprExprPredicate:
					{
						var expr = (SqlQuery.Predicate.ExprExpr)predicate;

						if (expr.Operator == SqlQuery.Predicate.Operator.Equal && expr.Expr1 is SqlValue && expr.Expr2 is SqlValue)
						{
							var value = Equals(((SqlValue)expr.Expr1).Value, ((SqlValue)expr.Expr2).Value);
							return new SqlQuery.Predicate.Expr(new SqlValue(value), Precedence.Comparison);
						}

						switch (expr.Operator)
						{
							case SqlQuery.Predicate.Operator.Equal:
							case SqlQuery.Predicate.Operator.Greater:
							case SqlQuery.Predicate.Operator.Less :
								predicate = OptimizeCase(expr);
								break;
						}

						if (predicate is SqlQuery.Predicate.ExprExpr)
						{
							expr = (SqlQuery.Predicate.ExprExpr)predicate;

							switch (expr.Operator)
							{
								case SqlQuery.Predicate.Operator.Equal :
								case SqlQuery.Predicate.Operator.NotEqual :
									var expr1 = expr.Expr1;
									var expr2 = expr.Expr2;

									if (expr1.CanBeNull() && expr2.CanBeNull())
									{
										if (expr1 is SqlParameter || expr2 is SqlParameter)
											SqlQuery.ParameterDependent = true;
										else
											if (expr1 is SqlQuery.Column || expr1 is SqlField)
											if (expr2 is SqlQuery.Column || expr2 is SqlField)
												predicate = ConvertEqualPredicate(expr);
									}

									break;
							}
						}
					}

					break;

				case QueryElementType.NotExprPredicate:
					{
						var expr = (SqlQuery.Predicate.NotExpr)predicate;

						if (expr.IsNot && expr.Expr1 is SqlQuery.SearchCondition)
						{
							var sc = (SqlQuery.SearchCondition)expr.Expr1;

							if (sc.Conditions.Count == 1)
							{
								var cond = sc.Conditions[0];

								if (cond.IsNot)
									return cond.Predicate;

								if (cond.Predicate is SqlQuery.Predicate.ExprExpr)
								{
									var ee = (SqlQuery.Predicate.ExprExpr)cond.Predicate;

									if (ee.Operator == SqlQuery.Predicate.Operator.Equal)
										return new SqlQuery.Predicate.ExprExpr(ee.Expr1, SqlQuery.Predicate.Operator.NotEqual, ee.Expr2);

									if (ee.Operator == SqlQuery.Predicate.Operator.NotEqual)
										return new SqlQuery.Predicate.ExprExpr(ee.Expr1, SqlQuery.Predicate.Operator.Equal, ee.Expr2);
								}
							}
						}
					}

					break;
			}

			return predicate;
		}

		protected ISqlPredicate ConvertEqualPredicate(SqlQuery.Predicate.ExprExpr expr)
		{
			var expr1 = expr.Expr1;
			var expr2 = expr.Expr2;
			var cond  = new SqlQuery.SearchCondition();

			if (expr.Operator == SqlQuery.Predicate.Operator.Equal)
				cond
					.Expr(expr1).IsNull.    And .Expr(expr2).IsNull. Or
					.Expr(expr1).IsNotNull. And .Expr(expr2).IsNotNull. And .Expr(expr1).Equal.Expr(expr2);
			else
				cond
					.Expr(expr1).IsNull.    And .Expr(expr2).IsNotNull. Or
					.Expr(expr1).IsNotNull. And .Expr(expr2).IsNull.    Or
					.Expr(expr1).NotEqual.Expr(expr2);

			return cond;
		}

		ISqlPredicate OptimizeCase(SqlQuery.Predicate.ExprExpr expr)
		{
			var value = expr.Expr1 as SqlValue;
			var func  = expr.Expr2 as SqlFunction;
			var valueFirst = false;

			if (value != null && func != null)
			{
				valueFirst = true;
			}
			else
			{
				value = expr.Expr2 as SqlValue;
				func  = expr.Expr1 as SqlFunction;
			}

			if (value != null && func != null && func.Name == "CASE")
			{
				if (value.Value is int && func.Parameters.Length == 5)
				{
					var c1 = func.Parameters[0] as SqlQuery.SearchCondition;
					var v1 = func.Parameters[1] as SqlValue;
					var c2 = func.Parameters[2] as SqlQuery.SearchCondition;
					var v2 = func.Parameters[3] as SqlValue;
					var v3 = func.Parameters[4] as SqlValue;

					if (c1 != null && c1.Conditions.Count == 1 && v1 != null && v1.Value is int &&
						c2 != null && c2.Conditions.Count == 1 && v2 != null && v2.Value is int && v3 != null && v3.Value is int)
					{
						var ee1 = c1.Conditions[0].Predicate as SqlQuery.Predicate.ExprExpr;
						var ee2 = c2.Conditions[0].Predicate as SqlQuery.Predicate.ExprExpr;

						if (ee1 != null && ee2 != null && ee1.Expr1.Equals(ee2.Expr1) && ee1.Expr2.Equals(ee2.Expr2))
						{
							int e = 0, g = 0, l = 0;

							if (ee1.Operator == SqlQuery.Predicate.Operator.Equal   || ee2.Operator == SqlQuery.Predicate.Operator.Equal)   e = 1;
							if (ee1.Operator == SqlQuery.Predicate.Operator.Greater || ee2.Operator == SqlQuery.Predicate.Operator.Greater) g = 1;
							if (ee1.Operator == SqlQuery.Predicate.Operator.Less    || ee2.Operator == SqlQuery.Predicate.Operator.Less)    l = 1;

							if (e + g + l == 2)
							{
								var n  = (int)value.Value;
								var i1 = (int)v1.Value;
								var i2 = (int)v2.Value;
								var i3 = (int)v3.Value;

								var n1 = Compare(valueFirst ? n : i1, valueFirst ? i1 : n, expr.Operator) ? 1 : 0;
								var n2 = Compare(valueFirst ? n : i2, valueFirst ? i2 : n, expr.Operator) ? 1 : 0;
								var n3 = Compare(valueFirst ? n : i3, valueFirst ? i3 : n, expr.Operator) ? 1 : 0;

								if (n1 + n2 + n3 == 1)
								{
									if (n1 == 1) return ee1;
									if (n2 == 1) return ee2;

									return ConvertPredicate(new SqlQuery.Predicate.ExprExpr(
										ee1.Expr1,
										e == 0 ? SqlQuery.Predicate.Operator.Equal :
										g == 0 ? SqlQuery.Predicate.Operator.Greater :
												 SqlQuery.Predicate.Operator.Less,
										ee1.Expr2));
								}
							}

						}
					}
				}
				else if (value.Value is bool && func.Parameters.Length == 3)
				{
					var c1 = func.Parameters[0] as SqlQuery.SearchCondition;
					var v1 = func.Parameters[1] as SqlValue;
					var v2 = func.Parameters[2] as SqlValue;

					if (c1 != null && c1.Conditions.Count == 1 && v1 != null && v1.Value is bool && v2 != null && v2.Value is bool)
					{
						var bv  = (bool)value.Value;
						var bv1 = (bool)v1.Value;
						var bv2 = (bool)v2.Value;

						if (bv == bv1 && expr.Operator == SqlQuery.Predicate.Operator.Equal ||
						    bv != bv1 && expr.Operator == SqlQuery.Predicate.Operator.NotEqual)
						{
							return c1;
						}

						if (bv == bv2 && expr.Operator == SqlQuery.Predicate.Operator.NotEqual ||
						    bv != bv1 && expr.Operator == SqlQuery.Predicate.Operator.Equal)
						{
							var ee = c1.Conditions[0].Predicate as SqlQuery.Predicate.ExprExpr;

							if (ee != null)
							{
								SqlQuery.Predicate.Operator op;

								switch (ee.Operator)
								{
									case SqlQuery.Predicate.Operator.Equal          : op = SqlQuery.Predicate.Operator.NotEqual;       break;
									case SqlQuery.Predicate.Operator.NotEqual       : op = SqlQuery.Predicate.Operator.Equal;          break;
									case SqlQuery.Predicate.Operator.Greater        : op = SqlQuery.Predicate.Operator.LessOrEqual;    break;
									case SqlQuery.Predicate.Operator.NotLess        :
									case SqlQuery.Predicate.Operator.GreaterOrEqual : op = SqlQuery.Predicate.Operator.Less;           break;
									case SqlQuery.Predicate.Operator.Less           : op = SqlQuery.Predicate.Operator.GreaterOrEqual; break;
									case SqlQuery.Predicate.Operator.NotGreater     :
									case SqlQuery.Predicate.Operator.LessOrEqual    : op = SqlQuery.Predicate.Operator.Greater;        break;
									default: throw new InvalidOperationException();
								}

								return new SqlQuery.Predicate.ExprExpr(ee.Expr1, op, ee.Expr2);
							}

							var sc = new SqlQuery.SearchCondition();

							sc.Conditions.Add(new SqlQuery.Condition(true, c1));

							return sc;
						}
					}
				}
				else if (expr.Operator == SqlQuery.Predicate.Operator.Equal && func.Parameters.Length == 3)
				{
					var sc = func.Parameters[0] as SqlQuery.SearchCondition;
					var v1 = func.Parameters[1] as SqlValue;
					var v2 = func.Parameters[2] as SqlValue;

					if (sc != null && v1 != null && v2 != null)
					{
						if (Equals(value.Value, v1.Value))
							return sc;

						if (Equals(value.Value, v2.Value) && !sc.CanBeNull())
							return ConvertPredicate(new SqlQuery.Predicate.NotExpr(sc, true, Precedence.LogicalNegation));
					}
				}
			}

			return expr;
		}

		static bool Compare(int v1, int v2, SqlQuery.Predicate.Operator op)
		{
			switch (op)
			{
				case SqlQuery.Predicate.Operator.Equal:           return v1 == v2;
				case SqlQuery.Predicate.Operator.NotEqual:        return v1 != v2;
				case SqlQuery.Predicate.Operator.Greater:         return v1 >  v2;
				case SqlQuery.Predicate.Operator.NotLess:
				case SqlQuery.Predicate.Operator.GreaterOrEqual:  return v1 >= v2;
				case SqlQuery.Predicate.Operator.Less:            return v1 <  v2;
				case SqlQuery.Predicate.Operator.NotGreater:
				case SqlQuery.Predicate.Operator.LessOrEqual:     return v1 <= v2;
			}

			throw new InvalidOperationException();
		}

		public virtual SqlQuery Finalize(SqlQuery sqlQuery)
		{
			sqlQuery.FinalizeAndValidate();
			return sqlQuery;
		}

		private        string _name;
		public virtual string  Name
		{
			get { return _name ?? (_name = GetType().Name.Replace("SqlProvider", "")); }
		}

		#endregion

		#region Linq Support

		public virtual LambdaExpression ConvertMember(MemberInfo mi)
		{
			Dictionary<MemberInfo,LambdaExpression> dic;
			LambdaExpression expr;

			if (Linq.Expressions.Members.TryGetValue(Name, out dic))
				if (dic.TryGetValue(mi, out expr))
					return expr;

			return Linq.Expressions.Members[""].TryGetValue(mi, out expr) ? expr : null;
		}

		#endregion
	}
}
