using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;
	using Mapping;

	public partial class BasicSqlProvider : ISqlProvider
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

		#endregion

		#region BuildSql

		public int BuildSql(SqlQuery sqlQuery, StringBuilder sb, int indent, int nesting, bool skipAlias)
		{
			_sqlQuery    = sqlQuery;
			_indent      = indent;
			_nesting     = nesting;
			_nextNesting = _nesting + 1;
			_skipAlias   = skipAlias;

			BuildSql(sb);

			if (sqlQuery.HasUnion)
			{
				foreach (SqlQuery.Union union in sqlQuery.Unions)
				{
					AppendIndent(sb);
					sb.Append("UNION");
					if (union.IsAll) sb.Append(" ALL");
					sb.AppendLine();

					CreateSqlProvider().BuildSql(union.SqlQuery, sb, indent, nesting, skipAlias);
				}
			}

			return _nextNesting;
		}

		#endregion

		#region Overrides

		protected virtual int BuildSqlBuilder(SqlQuery sqlQuery, StringBuilder sb, int indent, int nesting, bool skipAlias)
		{
			if (!IsSkipSupported && sqlQuery.Select.SkipValue != null)
				throw new SqlException("Skip for subqueries is not supported by the '{0}' provider.", _dataProvider.Name);

			if (!IsTakeSupported && sqlQuery.Select.TakeValue != null)
				throw new SqlException("Take for subqueries is not supported by the '{0}' provider.", _dataProvider.Name);

			return CreateSqlProvider().BuildSql(sqlQuery, sb, indent, nesting, skipAlias);
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
			_buildStep = Step.SelectClause;  BuildSelectClause (sb);
			_buildStep = Step.FromClause;    BuildFromClause   (sb);
			_buildStep = Step.WhereClause;   BuildWhereClause  (sb);
			_buildStep = Step.GroupByClause; BuildGroupByClause(sb);
			_buildStep = Step.HavingClause;  BuildHavingClause (sb);
			_buildStep = Step.OrderByClause; BuildOrderByClause(sb);
			_buildStep = Step.OffsetLimit;   BuildOffsetLimit  (sb);
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

			bool first = true;

			foreach (SqlQuery.Column col in GetSelectedColumns())
			{
				if (!first)
					sb.Append(',').AppendLine();
				first = false;

				bool addAlias = true;

				AppendIndent(sb);
				BuildColumn(sb, col, ref addAlias);

				if (!_skipAlias && addAlias)
					sb.Append(" as ").Append(DataProvider.Convert(col.Alias, ConvertType.NameToQueryFieldAlias));
			}

			if (first)
				AppendIndent(sb).Append("*");

			_indent--;

			sb.AppendLine();
		}

		protected virtual void BuildColumn(StringBuilder sb, SqlQuery.Column col, ref bool addAlias)
		{
			BuildExpression(sb, col.Expression, col.Alias, ref addAlias);
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

			bool first = true;

			foreach (SqlQuery.TableSource ts in _sqlQuery.From.Tables)
			{
				if (!first)
					sb.Append(", ");
				first = false;

				int jn = ParenthesizeJoin() ? ts.GetJoinNumber() : 0;

				if (jn > 0)
				{
					jn--;
					for (int i = 0; i < jn; i++)
						sb.Append("(");
				}

				BuildPhysicalTable(sb, ts.Source);

				string alias = GetTableAlias(ts);

				if (!string.IsNullOrEmpty(alias))
					sb.Append(" ").Append(DataProvider.Convert(alias, ConvertType.NameToQueryTableAlias));

				foreach (SqlQuery.JoinedTable jt in ts.Joins)
					BuildJoinTable(sb, jt, ref jn);
			}

			_indent--;

			sb.AppendLine();
		}

		void BuildPhysicalTable(StringBuilder sb, ISqlTableSource table)
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

			BuildPhysicalTable(sb, join.Table.Source);

			string alias = GetTableAlias(join.Table);

			if (!string.IsNullOrEmpty(alias))
				sb.Append(" ").Append(DataProvider.Convert(alias, ConvertType.NameToQueryTableAlias));

			if (IsNestedJoinSupported && join.Table.Joins.Count != 0)
			{
				foreach (SqlQuery.JoinedTable jt in join.Table.Joins)
					BuildJoinTable(sb, jt, ref joinCounter);

				if (IsNestedJoinParenthesisRequired && join.Table.Joins.Count != 0)
					sb.Append(')');

				sb.AppendLine();
				AppendIndent(sb);
				sb.Append("ON ");
			}
			else
				sb.Append(" ON ");

			BuildSearchCondition(sb, Precedence.Unknown, join.Condition);

			if (joinCounter > 0)
			{
				joinCounter--;
				sb.Append(")");
			}

			if (!IsNestedJoinSupported)
				foreach (SqlQuery.JoinedTable jt in join.Table.Joins)
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

			for (int i = 0; i < _sqlQuery.GroupBy.Items.Count; i++)
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

		#region Where Having

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

			for (int i = 0; i < _sqlQuery.OrderBy.Items.Count; i++)
			{
				AppendIndent(sb);

				SqlQuery.OrderByItem item = _sqlQuery.OrderBy.Items[i];

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
			bool doSkip = NeedSkip && OffsetFormat != null;
			bool doTake = NeedTake && LimitFormat  != null;

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
			bool? isOr = null;
			int   len  = sb.Length;
			int   prevPrecedence = condition.Conditions.Count > 1 && !condition.Conditions[0].IsOr ? Precedence.LogicalConjunction : Precedence.LogicalDisjunction;

			foreach (SqlQuery.Condition cond in condition.Conditions)
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
			bool wrap = Wrap(GetPrecedence(condition as ISqlExpression), parentPrecedence);

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
				case QueryElementType.ExprExprPredicate:
					{
						SqlQuery.Predicate.ExprExpr expr = (SqlQuery.Predicate.ExprExpr)predicate;

						switch (expr.Operator)
						{
							case SqlQuery.Predicate.Operator.Equal:
							case SqlQuery.Predicate.Operator.NotEqual:
								{
									ISqlExpression e = null;

									if (expr.Expr1 is SqlValue && ((SqlValue) expr.Expr1).Value == null)
										e = expr.Expr2;
									else if (expr.Expr2 is SqlValue && ((SqlValue) expr.Expr2).Value == null)
										e = expr.Expr1;

									if (e != null)
									{
										BuildExpression(sb, GetPrecedence(expr), e);
										sb.Append(expr.Operator == SqlQuery.Predicate.Operator.Equal? " IS NULL": " IS NOT NULL");
										return;
									}

									break;
								}
						}

						BuildExpression(sb, GetPrecedence(expr), expr.Expr1);

						switch (expr.Operator)
						{
							case SqlQuery.Predicate.Operator.Equal         : sb.Append(" = ");  break;
							case SqlQuery.Predicate.Operator.NotEqual      : sb.Append(" <> "); break;
							case SqlQuery.Predicate.Operator.Greater       : sb.Append(" > ");  break;
							case SqlQuery.Predicate.Operator.GreaterOrEqual: sb.Append(" >= "); break;
							case SqlQuery.Predicate.Operator.NotGreater    : sb.Append(" !> "); break;
							case SqlQuery.Predicate.Operator.Less          : sb.Append(" < ");  break;
							case SqlQuery.Predicate.Operator.LessOrEqual   : sb.Append(" <= "); break;
							case SqlQuery.Predicate.Operator.NotLess       : sb.Append(" !< "); break;
						}

						BuildExpression(sb, GetPrecedence(expr), expr.Expr2);
					}

					break;

				case QueryElementType.LikePredicate:
					BuildLikePredicate(sb, (SqlQuery.Predicate.Like)predicate);
					break;

				case QueryElementType.BetweenPredicate:
					throw new NotImplementedException();

				case QueryElementType.IsNullPredicate:
					{
						SqlQuery.Predicate.IsNull p = (SqlQuery.Predicate.IsNull)predicate;
						BuildExpression(sb, GetPrecedence(p), p.Expr1);
						sb.Append(p.IsNot? " IS NOT NULL": " IS NULL");
					}

					break;

				case QueryElementType.InSubqueryPredicate:
					throw new NotImplementedException();

				case QueryElementType.InListPredicate:
					{
						SqlQuery.Predicate.InList p = (SqlQuery.Predicate.InList)predicate;

						if (p.Values == null || p.Values.Count == 0)
						{
							BuildPredicate(sb, new SqlQuery.Predicate.Expr(new SqlValue(false)));
						}
						else
						{
							ICollection values = p.Values;

							if (p.Values.Count == 1 && p.Values[0] is SqlParameter)
							{
								SqlParameter pr = (SqlParameter)p.Values[0];

								if (pr.Type != null && pr.Type.IsArray)
								{
									values = (ICollection)pr.Value;

									if (values == null || values.Count == 0)
									{
										BuildPredicate(sb, new SqlQuery.Predicate.Expr(new SqlValue(false)));
										return;
									}
								}
							}

							BuildExpression(sb, GetPrecedence(p), p.Expr1);
							sb.Append(p.IsNot ? " NOT IN (" : " IN (");

							foreach (object value in values)
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

					break;

				case QueryElementType.FuncLikePredicate:
					{
						SqlQuery.Predicate.FuncLike f = (SqlQuery.Predicate.FuncLike)predicate;
						BuildExpression(sb, f.Function.Precedence, f.Function);
					}

					break;

				case QueryElementType.SearchCondition:
					BuildSearchCondition(sb, predicate.Precedence, (SqlQuery.SearchCondition)predicate);
					break;

				case QueryElementType.NotExprPredicate:
					{
						SqlQuery.Predicate.NotExpr p = (SqlQuery.Predicate.NotExpr)predicate;

						if (p.IsNot)
							sb.Append("NOT ");

						BuildExpression(sb, p.IsNot ? Precedence.LogicalNegation : GetPrecedence(p), p.Expr1);
					}

					break;

				case QueryElementType.ExprPredicate:
					{
						SqlQuery.Predicate.Expr p = (SqlQuery.Predicate.Expr)predicate;

						if (p.Expr1 is SqlValue)
						{
							object value = ((SqlValue)p.Expr1).Value;

							if (value is bool)
							{
								sb.Append((bool)value ? "1 = 1" : "1 = 0");
								return;
							}
						}

						BuildExpression(sb, GetPrecedence(p), p.Expr1);
					}

					break;

				default:
					throw new InvalidOperationException();
			}
		}

		protected void BuildPredicate(StringBuilder sb, int parentPrecedence, ISqlPredicate predicate)
		{
			bool wrap = Wrap(GetPrecedence(predicate), parentPrecedence);

			if (wrap) sb.Append('(');
			BuildPredicate(sb, predicate);
			if (wrap) sb.Append(')');
		}

		protected virtual void BuildLikePredicate(StringBuilder sb, SqlQuery.Predicate.Like predicate)
		{
			int precedence = GetPrecedence(predicate);

			BuildExpression(sb, precedence, predicate.Expr1);
			sb.Append(predicate.IsNot? " NOT LIKE ": " LIKE ");
			BuildExpression(sb, precedence, predicate.Expr2);

			if (predicate.Escape != null)
			{
				sb.Append(" ESCAPE '");
				sb.Append(predicate.Escape);
				sb.Append("'");
			}
		}

		#endregion

		#region BuildExpression

		protected virtual StringBuilder BuildExpression(StringBuilder sb, ISqlExpression expr, string alias, ref bool addAlias)
		{
			expr = ConvertExpression(expr);

			switch (expr.ElementType)
			{
				case QueryElementType.SqlField:
					{
						SqlField field = (SqlField)expr;
						if (field == field.Table.All)
						{
							sb.Append("*");
						}
						else
						{
							SqlQuery.TableSource ts = _sqlQuery.GetTableSource(field.Table);

							if (ts == null)
								throw new SqlException(string.Format("Table {0} not found.", field.Table));

							string table = GetTableAlias(ts) ?? GetTablePhysicalName(field.Table);

							if (string.IsNullOrEmpty(table))
								throw new SqlException(string.Format("Table {0} should have an alias.", field.Table));

							addAlias = alias != field.PhysicalName;

							sb
								.Append(DataProvider.Convert(table, ConvertType.NameToQueryTableAlias))
								.Append('.')
								.Append(_dataProvider.Convert(field.PhysicalName, ConvertType.NameToQueryField));
						}
					}

					break;

				case QueryElementType.Column:
					{
						SqlQuery.Column column = (SqlQuery.Column)expr;
						ISqlTableSource table  = _sqlQuery.GetTableSource(column.Parent);

						if (table == null)
							throw new SqlException(string.Format("Table not found for '{0}'.", column));

						string tableAlias = GetTableAlias(table) ?? GetTablePhysicalName(column.Parent);

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
						bool hasParentheses = sb[sb.Length - 1] == '(';

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
						SqlExpression e = (SqlExpression)expr;
						StringBuilder s = new StringBuilder();
						object[] values = new object[e.Values.Length];

						for (int i = 0; i < values.Length; i++)
						{
							ISqlExpression value = e.Values[i];

							s.Length = 0;
							BuildExpression(s, GetPrecedence(e), value);
							values[i] = s.ToString();
						}

						sb.AppendFormat(e.Expr, values);
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
						SqlParameter parm = (SqlParameter)expr;

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
			bool wrap = Wrap(GetPrecedence(expr), parentPrecedence);

			if (wrap) sb.Append('(');
			BuildExpression(sb, expr, alias, ref addAlias);
			if (wrap) sb.Append(')');
		}

		protected virtual StringBuilder BuildExpression(StringBuilder sb, ISqlExpression expr)
		{
			bool dummy = false;
			return BuildExpression(sb, expr, null, ref dummy);
		}

		protected void BuildExpression(StringBuilder sb, int precedence, ISqlExpression expr)
		{
			bool dummy = false;
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
			else if (value is string)                 sb.Append('\'').Append(value.ToString().Replace("'", "''")).Append('\'');
			else if (value is char || value is char?) sb.Append('\'').Append(value.ToString().Replace("'", "''")).Append('\'');
			else if (value is bool || value is bool?) sb.Append((bool)value ? "1" : "0");
			else
			{
				Type type = value.GetType();

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

				int i = 0;

				for (; i < func.Parameters.Length - 1; i += 2)
				{
					AppendIndent(sb).Append("WHEN ");
					BuildExpression(sb, func.Parameters[i]);
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

			bool first = true;

			foreach (ISqlExpression parameter in exprs)
			{
				if (!first)
					sb.Append(", ");
				first = false;

				BuildExpression(sb, parameter);
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

			bool first = true;

			foreach (SqlQuery.Column col in columns)
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
				string[] aliases = GetTempAliases(2, "t");
				string  rnaliase = GetTempAliases(1, "rn")[0];

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
					AppendIndent(sb).AppendFormat("{0}.{1} BETWEEN ", aliases[1], rnaliase);
					BuildExpression(sb, Add(SqlQuery.Select.SkipValue, 1));
					sb.Append(" AND ");
					BuildExpression(sb, Add<int>(SqlQuery.Select.SkipValue, SqlQuery.Select.TakeValue));
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
			string[] aliases = GetTempAliases(3, "t");

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

			string[] obys = GetTempAliases(SqlQuery.OrderBy.Items.Count, "oby");

			_indent++;

			for (int i = 0; i < obys.Length; i++)
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
			foreach (SqlQuery.Column col in columnSelector())
				yield return col;

			string[] obys = GetTempAliases(SqlQuery.OrderBy.Items.Count, "oby");

			for (int i = 0; i < obys.Length; i++)
				yield return new SqlQuery.Column(SqlQuery, SqlQuery.OrderBy.Items[i].Expression, obys[i]);
		}

		#endregion

		#region Helpers

		static bool Wrap(int precedence, int parentPrecedence)
		{
			return
				precedence == 0 ||
				precedence < parentPrecedence ||
				(precedence == parentPrecedence && parentPrecedence == Precedence.Subtraction); 
		}

		protected string[] GetTempAliases(int n, string defaultAlias)
		{
			return SqlQuery.GetTempAliases(n, defaultAlias + (Nesting == 0? "": "n" + Nesting.ToString()));
		}

		static string GetTableAlias(ISqlTableSource table)
		{
			switch (table.ElementType)
			{
				case QueryElementType.TableSource :
					SqlQuery.TableSource ts = (SqlQuery.TableSource)table;
					return string.IsNullOrEmpty(ts.Alias) ? GetTableAlias(ts.Source) : ts.Alias;

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
						SqlTable tbl = (SqlTable)table;

						string tableName = _dataProvider.Convert(tbl.PhysicalName, ConvertType.NameToQueryTable).ToString();

						if (tbl.Database == null && tbl.Owner == null)
							return tableName;

						string name = string.Empty;

						if (tbl.Database != null)
							name = _dataProvider.Convert(tbl.Database, ConvertType.NameToDatabase) + _dataProvider.DatabaseOwnerDelimiter;

						if (tbl.Owner != null)
							name +=
								_dataProvider.Convert(tbl.Owner, ConvertType.NameToOwner) +
								_dataProvider.OwnerTableDelimiter;
						else
							name += _dataProvider.DatabaseTableDelimiter;

						return name + tableName;
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
			return ConvertExpression(new SqlBinaryExpression(expr1, "+", expr2, type, Precedence.Additive));
		}

		public ISqlExpression Add<T>(ISqlExpression expr1, ISqlExpression expr2)
		{
			return Add(expr1, expr2, typeof(T));
		}

		public ISqlExpression Add(ISqlExpression expr1, int value)
		{
			return Add<int>(expr1, new SqlValue(value));
		}

		public ISqlExpression Sub(ISqlExpression expr1, ISqlExpression expr2, Type type)
		{
			return ConvertExpression(new SqlBinaryExpression(expr1, "-", expr2, type, Precedence.Subtraction));
		}

		public ISqlExpression Sub<T>(ISqlExpression expr1, ISqlExpression expr2)
		{
			return Sub(expr1, expr2, typeof(T));
		}

		public ISqlExpression Sub(ISqlExpression expr1, int value)
		{
			return Sub<int>(expr1, new SqlValue(value));
		}

		public ISqlExpression Mul(ISqlExpression expr1, ISqlExpression expr2, Type type)
		{
			return ConvertExpression(new SqlBinaryExpression(expr1, "*", expr2, type, Precedence.Subtraction));
		}

		public ISqlExpression Mul<T>(ISqlExpression expr1, ISqlExpression expr2)
		{
			return Mul(expr1, expr2, typeof(T));
		}

		public ISqlExpression Mul(ISqlExpression expr1, int value)
		{
			return Mul<int>(expr1, new SqlValue(value));
		}

		#endregion

		#region DataTypes

		protected virtual int GetMaxLength     (SqlDataType type) { return SqlDataType.GetMaxLength     (type.DbType); }
		protected virtual int GetMaxPrecision  (SqlDataType type) { return SqlDataType.GetMaxPrecision  (type.DbType); }
		protected virtual int GetMaxScale      (SqlDataType type) { return SqlDataType.GetMaxScale      (type.DbType); }
		protected virtual int GetMaxDisplaySize(SqlDataType type) { return SqlDataType.GetMaxDisplaySize(type.DbType); }

		protected virtual ISqlExpression ConvertConvertion(SqlFunction func)
		{
			SqlDataType from = (SqlDataType)func.Parameters[1];
			SqlDataType to   = (SqlDataType)func.Parameters[0];

			if (to.Precision > 0)
			{
				int maxPrecision = GetMaxPrecision(from);
				int maxScale     = GetMaxScale    (from);

				to = new SqlDataType(
					to.DbType,
					to.Type,
					maxPrecision >= 0 ? Math.Min(to.Precision, maxPrecision) : to.Precision,
					maxScale     >= 0 ? Math.Min(to.Scale,     maxScale)     : to.Scale);
			}
			else if (to.Length > 0)
			{
				int maxLength = GetMaxLength(from);
				to = new SqlDataType(to.DbType, to.Type, maxLength >= 0 ? Math.Min(to.Length, maxLength) : to.Length);
			}

			return ConvertExpression(new SqlFunction("Convert", to, func.Parameters[2]));
		}

		#endregion

		#region ISqlProvider Members

		public virtual ISqlExpression ConvertExpression(ISqlExpression expression)
		{
			if (expression is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expression;

				switch (be.Operation)
				{
					case "+":
						if (be.Expr1 is SqlValue)
						{
							SqlValue v1 = (SqlValue)be.Expr1;
							if (v1.Value is int    && (int)   v1.Value == 0 ||
								v1.Value is string && (string)v1.Value == "") return be.Expr2;
						}

						if (be.Expr2 is SqlValue)
						{
							SqlValue v2 = (SqlValue) be.Expr2;

							if (v2.Value is int)
							{
								if ((int)v2.Value == 0) return be.Expr1;

								if (be.Expr1 is SqlBinaryExpression)
								{
									SqlBinaryExpression be1 = (SqlBinaryExpression) be.Expr1;

									if (be1.Expr2 is SqlValue)
									{
										SqlValue be1v2 = (SqlValue)be1.Expr2;

										if (be1v2.Value is int)
										{
											switch (be1.Operation)
											{
												case "+": return new SqlBinaryExpression(be1.Expr1, be1.Operation, new SqlValue((int)be1v2.Value + (int)v2.Value), be.Type, be.Precedence);
												case "-": return new SqlBinaryExpression(be1.Expr1, be1.Operation, new SqlValue((int)be1v2.Value - (int)v2.Value), be.Type, be.Precedence);
											}
										}
									}
								}
							}
							else if (v2.Value is string && (string)v2.Value == "") return be.Expr1;
						}

						if (be.Expr1 is SqlValue && be.Expr2 is SqlValue)
						{
							SqlValue v1 = (SqlValue)be.Expr1;
							SqlValue v2 = (SqlValue)be.Expr2;
							if (v1.Value is int    && v2.Value is int)    return new SqlValue((int)   v1.Value + (int)   v2.Value);
							if (v1.Value is string && v2.Value is string) return new SqlValue((string)v1.Value + (string)v2.Value);
						}

						break;

					case "-":
						if (be.Expr2 is SqlValue)
						{
							SqlValue v2 = (SqlValue) be.Expr2;
							if (v2.Value is int && (int) v2.Value == 0) return be.Expr1;
						}

						if (be.Expr1 is SqlValue && be.Expr2 is SqlValue)
						{
							SqlValue v1 = (SqlValue)be.Expr1;
							SqlValue v2 = (SqlValue)be.Expr2;
							if (v1.Value is int && v2.Value is int) return new SqlValue((int)v1.Value - (int)v2.Value);
						}

						break;

					case "*":
						if (be.Expr1 is SqlValue)
						{
							SqlValue v1 = (SqlValue)be.Expr1;
							if (v1.Value is int && (int)v1.Value == 1) return be.Expr2;
							if (v1.Value is int && (int)v1.Value == 0) return new SqlValue(0);
						}

						if (be.Expr2 is SqlValue)
						{
							SqlValue v2 = (SqlValue)be.Expr2;
							if (v2.Value is int && (int)v2.Value == 1) return be.Expr1;
							if (v2.Value is int && (int)v2.Value == 0) return new SqlValue(0);
						}

						if (be.Expr1 is SqlValue && be.Expr2 is SqlValue)
						{
							SqlValue v1 = (SqlValue)be.Expr1;
							SqlValue v2 = (SqlValue)be.Expr2;
							if (v1.Value is int && v2.Value is int) return new SqlValue((int)v1.Value * (int)v2.Value);
						}

						break;
				}
			}
			else if (expression is SqlFunction)
			{
				SqlFunction func = (SqlFunction)expression;

				switch (func.Name)
				{
					case "ConvertToCaseCompareTo":
						return ConvertExpression(new SqlFunction("CASE",
							new SqlQuery.SearchCondition().Expr(func.Parameters[0]). Greater .Expr(func.Parameters[1]).ToExpr(), new SqlValue(1),
							new SqlQuery.SearchCondition().Expr(func.Parameters[0]). Equal   .Expr(func.Parameters[1]).ToExpr(), new SqlValue(0),
							new SqlValue(-1)));

					case "$Convert$": return ConvertConvertion(func);
					case "Average"  : return new SqlFunction("Avg", func.Parameters);

					case "CASE":
						{
							ISqlExpression[] parms = func.Parameters;
							int              len   = parms.Length;

							for (int i = 0; i < parms.Length - 1; i += 2)
							{
								SqlValue value = parms[i] as SqlValue;

								if (value != null)
								{
									if ((bool)value.Value == false)
									{
										ISqlExpression[] newParms = new ISqlExpression[parms.Length - 2];

										if (i != 0)
											Array.Copy(parms, 0, newParms, 0, i);

										Array.Copy(parms, i + 2, newParms, i, parms.Length - i - 2);

										parms = newParms;
										i -= 2;
									}
									else
									{
										ISqlExpression[] newParms = new ISqlExpression[i + 1];

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
								return new SqlFunction(func.Name, func.Precedence, parms);
						}

						break;
				}
			}
			else if (expression is SqlQuery.SearchCondition)
			{
				SqlQuery.OptimizeSearchCondition((SqlQuery.SearchCondition)expression);
			}

			return expression;
		}

		public virtual ISqlPredicate ConvertPredicate(ISqlPredicate predicate)
		{
			switch (predicate.ElementType)
			{
				case QueryElementType.ExprExprPredicate:
					{
						SqlQuery.Predicate.ExprExpr expr = (SqlQuery.Predicate.ExprExpr)predicate;

						if (expr.Operator == SqlQuery.Predicate.Operator.Equal && expr.Expr1 is SqlValue && expr.Expr2 is SqlValue)
						{
							bool value = object.Equals(((SqlValue)expr.Expr1).Value, ((SqlValue)expr.Expr2).Value);
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
									ISqlExpression expr1 = expr.Expr1;
									ISqlExpression expr2 = expr.Expr2;

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
						SqlQuery.Predicate.NotExpr expr = (SqlQuery.Predicate.NotExpr)predicate;

						if (expr.IsNot && expr.Expr1 is SqlQuery.SearchCondition)
						{
							SqlQuery.SearchCondition sc = (SqlQuery.SearchCondition)expr.Expr1;

							if (sc.Conditions.Count == 1)
							{
								SqlQuery.Condition cond = sc.Conditions[0];

								if (cond.IsNot)
									return cond.Predicate;

								if (cond.Predicate is SqlQuery.Predicate.ExprExpr)
								{
									SqlQuery.Predicate.ExprExpr ee = (SqlQuery.Predicate.ExprExpr)cond.Predicate;

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
			ISqlExpression expr1 = expr.Expr1;
			ISqlExpression expr2 = expr.Expr2;

			SqlQuery.SearchCondition cond = new SqlQuery.SearchCondition();

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
			SqlValue    value = expr.Expr1 as SqlValue;
			SqlFunction func  = expr.Expr2 as SqlFunction;
			bool        valueFirst = false;

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
					SqlQuery.SearchCondition c1 = func.Parameters[0] as SqlQuery.SearchCondition;
					SqlValue                   v1 = func.Parameters[1] as SqlValue;
					SqlQuery.SearchCondition c2 = func.Parameters[2] as SqlQuery.SearchCondition;
					SqlValue                   v2 = func.Parameters[3] as SqlValue;
					SqlValue                   v3 = func.Parameters[4] as SqlValue;

					if (c1 != null && c1.Conditions.Count == 1 && v1 != null && v1.Value is int &&
						c2 != null && c2.Conditions.Count == 1 && v2 != null && v2.Value is int && v3 != null && v3.Value is int)
					{
						SqlQuery.Predicate.ExprExpr ee1 = c1.Conditions[0].Predicate as SqlQuery.Predicate.ExprExpr;
						SqlQuery.Predicate.ExprExpr ee2 = c2.Conditions[0].Predicate as SqlQuery.Predicate.ExprExpr;

						if (ee1 != null && ee2 != null && ee1.Expr1.Equals(ee2.Expr1) && ee1.Expr2.Equals(ee2.Expr2))
						{
							int e = 0, g = 0, l = 0;

							if (ee1.Operator == SqlQuery.Predicate.Operator.Equal   || ee2.Operator == SqlQuery.Predicate.Operator.Equal)   e = 1;
							if (ee1.Operator == SqlQuery.Predicate.Operator.Greater || ee2.Operator == SqlQuery.Predicate.Operator.Greater) g = 1;
							if (ee1.Operator == SqlQuery.Predicate.Operator.Less    || ee2.Operator == SqlQuery.Predicate.Operator.Less)    l = 1;

							if (e + g + l == 2)
							{
								int n  = (int)value.Value;
								int i1 = (int)v1.Value;
								int i2 = (int)v2.Value;
								int i3 = (int)v3.Value;

								int n1 = Compare(valueFirst ? n : i1, valueFirst ? i1 : n, expr.Operator) ? 1 : 0;
								int n2 = Compare(valueFirst ? n : i2, valueFirst ? i2 : n, expr.Operator) ? 1 : 0;
								int n3 = Compare(valueFirst ? n : i3, valueFirst ? i3 : n, expr.Operator) ? 1 : 0;

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
				else if (expr.Operator == SqlQuery.Predicate.Operator.Equal && func.Parameters.Length == 3)
				{
					SqlQuery.SearchCondition sc = func.Parameters[0] as SqlQuery.SearchCondition;
					SqlValue                   v1 = func.Parameters[1] as SqlValue;
					SqlValue                   v2 = func.Parameters[2] as SqlValue;

					if (sc != null && v1 != null && v2 != null)
					{
						if (object.Equals(value.Value, v1.Value))
							return sc;

						if (object.Equals(value.Value, v2.Value) && !sc.CanBeNull())
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

		public virtual string Name
		{
			get { return GetType().Name.Replace("SqlProvider", ""); }
		}

		#endregion
	}
}
