using System;
using System.Collections.Generic;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

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

		private SqlBuilder _sqlBuilder;
		public  SqlBuilder  SqlBuilder
		{
			get { return _sqlBuilder;  }
			set { _sqlBuilder = value; }
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

		#endregion

		#region Support Flags

		public virtual bool SkipAcceptsParameter      { get { return true; } }
		public virtual bool TakeAcceptsParameter      { get { return true; } }

		public virtual bool IsTakeSupported           { get { return true; } }
		public virtual bool IsSkipSupported           { get { return true; } }
		public virtual bool IsSubQueryColumnSupported { get { return true; } }
		public virtual bool IsCountSubQuerySupported  { get { return true; } }

		#endregion

		#region UpdateParameters

		public void UpdateParameters(SqlBuilder sqlBuilder)
		{
			_sqlBuilder = sqlBuilder;

			sqlBuilder.Parameters.Clear();

			((ISqlExpressionWalkable)sqlBuilder).Walk(true, delegate(ISqlExpression expr)
			{
				if (expr is SqlParameter)
				{
					SqlParameter p = (SqlParameter)expr;

					if (p.Value == null)
						return new SqlValue(null);

					if (p.IsQueryParameter)
						sqlBuilder.Parameters.Add(p);
				}

				return expr;
			});
		}

		#endregion

		#region BuildSql

		public int BuildSql(SqlBuilder sqlBuilder, StringBuilder sb, int indent, int nesting)
		{
			_sqlBuilder  = sqlBuilder;
			_indent      = indent;
			_nesting     = nesting;
			_nextNesting = _nesting + 1;

			BuildSql(sb);

			return _nextNesting;
		}

		#endregion

		#region Overrides

		protected virtual int BuildSqlBuilder(SqlBuilder sqlBuilder, StringBuilder sb, int indent, int nesting)
		{
			if (!IsSkipSupported && sqlBuilder.Select.SkipValue != null)
				throw new SqlException("Skip for subqueries is not supported by the '{0}' provider.", _dataProvider.Name);

			if (!IsTakeSupported && sqlBuilder.Select.TakeValue != null)
				throw new SqlException("Take for subqueries is not supported by the '{0}' provider.", _dataProvider.Name);

			return DataProvider.CreateSqlProvider().BuildSql(sqlBuilder, sb, indent, nesting);
		}

		protected virtual bool ParenthesizeJoin()
		{
			return false;
		}

		protected virtual void BuildSql(StringBuilder sb)
		{
			BuildSelectClause (sb);
			BuildFromClause   (sb);
			BuildWhereClause  (sb);
			BuildGroupByClause(sb);
			BuildHavingClause (sb);
			BuildOrderByClause(sb);
			BuildOffsetLimit  (sb);
		}

		#endregion

		#region Build Select

		protected virtual void BuildSelectClause(StringBuilder sb)
		{
			AppendIndent(sb);
			sb.Append("SELECT");

			if (SqlBuilder.Select.IsDistinct)
				sb.Append(" DISTINCT");

			BuildSkipFirst(sb);

			sb.AppendLine();
			BuildColumns(sb);
		}

		protected virtual IEnumerable<SqlBuilder.Column> GetSelectedColumns()
		{
			return _sqlBuilder.Select.Columns;
		}

		protected virtual void BuildColumns(StringBuilder sb)
		{
			_indent++;

			bool first = true;

			foreach (SqlBuilder.Column col in GetSelectedColumns())
			{
				if (!first)
					sb.Append(',').AppendLine();
				first = false;

				bool addAlias = true;

				AppendIndent(sb);
				BuildColumn(sb, col, ref addAlias);

				if (addAlias)
					sb.Append(" as ").Append(DataProvider.Convert(col.Alias, ConvertType.NameToQueryFieldAlias));
			}

			if (first)
				AppendIndent(sb).Append("*");

			_indent--;

			sb.AppendLine();
		}

		protected virtual void BuildColumn(StringBuilder sb, SqlBuilder.Column col, ref bool addAlias)
		{
			BuildExpression(sb, col.Expression, col.Alias, ref addAlias);
		}

		#endregion

		#region Build From

		protected virtual void BuildFromClause(StringBuilder sb)
		{
			if (_sqlBuilder.From.Tables.Count == 0)
				return;

			AppendIndent(sb);

			sb.Append("FROM").AppendLine();

			_indent++;
			AppendIndent(sb);

			bool first = true;

			foreach (SqlBuilder.TableSource ts in _sqlBuilder.From.Tables)
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
					sb.Append(" ").Append(alias);

				foreach (SqlBuilder.JoinedTable jt in ts.Joins)
					BuildJoinTable(sb, jt, ref jn);
			}

			_indent--;

			sb.AppendLine();
		}

		void BuildPhysicalTable(StringBuilder sb, ISqlTableSource table)
		{
			if (table is SqlTable || table is SqlBuilder.TableSource)
			{
				sb.Append(GetTablePhysicalName(table));
			}
			else if (table is SqlBuilder)
			{
				sb.Append("(").AppendLine();
				_nextNesting = BuildSqlBuilder((SqlBuilder)table, sb, _indent + 1, _nextNesting);
				AppendIndent(sb).Append(")");
			}
			else
				throw new InvalidOperationException();
		}

		void BuildJoinTable(StringBuilder sb, SqlBuilder.JoinedTable join, ref int joinCounter)
		{
			sb.AppendLine();
			_indent++;
			AppendIndent(sb);

			switch (join.JoinType)
			{
				case SqlBuilder.JoinType.Inner : sb.Append("INNER JOIN "); break;
				case SqlBuilder.JoinType.Left  : sb.Append("LEFT JOIN ");  break;
				default: throw new InvalidOperationException();
			}

			BuildPhysicalTable(sb, join.Table.Source);

			string alias = GetTableAlias(join.Table);

			if (!string.IsNullOrEmpty(alias))
				sb.Append(" ").Append(alias);

			sb.Append(" ON ");

			BuildSearchCondition(sb, Precedence.Unknown, join.Condition);

			if (joinCounter > 0)
			{
				joinCounter--;
				sb.Append(")");
			}

			foreach (SqlBuilder.JoinedTable jt in join.Table.Joins)
				BuildJoinTable(sb, jt, ref joinCounter);

			_indent--;
		}

		#endregion

		#region Where Clause

		protected virtual bool BuildWhere()
		{
			return _sqlBuilder.Where.SearchCondition.Conditions.Count != 0;
		}

		protected virtual void BuildWhereClause(StringBuilder sb)
		{
			if (!BuildWhere())
				return;

			AppendIndent(sb);

			sb.Append("WHERE").AppendLine();

			_indent++;
			AppendIndent(sb);
			BuildWhereSearchCondition(sb, _sqlBuilder.Where.SearchCondition);
			_indent--;

			sb.AppendLine();
		}

		#endregion

		#region GroupBy Clause

		protected virtual void BuildGroupByClause(StringBuilder sb)
		{
			if (_sqlBuilder.GroupBy.Items.Count == 0)
				return;

			AppendIndent(sb);

			sb.Append("GROUP BY").AppendLine();

			_indent++;

			for (int i = 0; i < _sqlBuilder.GroupBy.Items.Count; i++)
			{
				AppendIndent(sb);

				BuildExpression(sb, _sqlBuilder.GroupBy.Items[i]);

				if (i + 1 < _sqlBuilder.GroupBy.Items.Count)
					sb.Append(',');

				sb.AppendLine();
			}

			_indent--;
		}

		#endregion

		#region Where Having

		protected virtual void BuildHavingClause(StringBuilder sb)
		{
			if (_sqlBuilder.Having.SearchCondition.Conditions.Count == 0)
				return;

			AppendIndent(sb);

			sb.Append("HAVING").AppendLine();

			_indent++;
			AppendIndent(sb);
			BuildWhereSearchCondition(sb, _sqlBuilder.Having.SearchCondition);
			_indent--;

			sb.AppendLine();
		}

		#endregion

		#region OrderBy Clause

		protected virtual void BuildOrderByClause(StringBuilder sb)
		{
			if (_sqlBuilder.OrderBy.Items.Count == 0)
				return;

			AppendIndent(sb);

			sb.Append("ORDER BY").AppendLine();

			_indent++;

			for (int i = 0; i < _sqlBuilder.OrderBy.Items.Count; i++)
			{
				AppendIndent(sb);

				SqlBuilder.OrderByItem item = _sqlBuilder.OrderBy.Items[i];

				BuildExpression(sb, item.Expression);

				if (item.IsDescending)
					sb.Append(" DESC");

				if (i + 1 < _sqlBuilder.OrderBy.Items.Count)
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

		protected bool NeedSkip { get { return SqlBuilder.Select.SkipValue != null && IsSkipSupported; } }
		protected bool NeedTake { get { return SqlBuilder.Select.TakeValue != null && IsTakeSupported; } }

		protected virtual void BuildSkipFirst(StringBuilder sb)
		{
			if (SkipFirst && NeedSkip && SkipFormat != null)
				sb.Append(' ').AppendFormat(SkipFormat,  BuildExpression(new StringBuilder(), SqlBuilder.Select.SkipValue));

			if (NeedTake && FirstFormat != null)
				sb.Append(' ').AppendFormat(FirstFormat, BuildExpression(new StringBuilder(), SqlBuilder.Select.TakeValue));

			if (!SkipFirst && NeedSkip && SkipFormat != null)
				sb.Append(' ').AppendFormat(SkipFormat,  BuildExpression(new StringBuilder(), SqlBuilder.Select.SkipValue));
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
					sb.AppendFormat(LimitFormat, BuildExpression(new StringBuilder(), SqlBuilder.Select.TakeValue));

					if (doSkip)
						sb.Append(' ');
				}

				if (doSkip)
					sb.AppendFormat(OffsetFormat, BuildExpression(new StringBuilder(), SqlBuilder.Select.SkipValue));

				sb.AppendLine();
			}
		}

		#endregion

		#region Builders

		#region BuildSearchCondition

		protected virtual void BuildWhereSearchCondition(StringBuilder sb, SqlBuilder.SearchCondition condition)
		{
			BuildSearchCondition(sb, Precedence.Unknown, condition);
		}

		protected virtual void BuildSearchCondition(StringBuilder sb, SqlBuilder.SearchCondition condition)
		{
			bool? isOr = null;
			int   len  = sb.Length;

			foreach (SqlBuilder.Condition cond in condition.Conditions)
			{
				if (isOr != null)
				{
					sb.Append(isOr.Value ? " OR" : " AND");

					if (condition.Conditions.Count < 4 && sb.Length - len < 50 || condition != _sqlBuilder.Where.SearchCondition)
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

				int precedence;

				if (cond.IsNot)
				{
					sb.Append("NOT ");
					precedence = Precedence.LogicalNegation;
				}
				else
				{
					precedence = GetPrecedence(condition as ISqlExpression);
				}

				BuildPredicate(sb, precedence, cond.Predicate);

				isOr = cond.IsOr;
			}
		}

		protected virtual void BuildSearchCondition(StringBuilder sb, int parentPrecedence, SqlBuilder.SearchCondition condition)
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
			if (predicate is SqlBuilder.Predicate.ExprExpr)
			{
				SqlBuilder.Predicate.ExprExpr expr = (SqlBuilder.Predicate.ExprExpr)predicate;

				switch (expr.Operator)
				{
					case SqlBuilder.Predicate.Operator.Equal:
					case SqlBuilder.Predicate.Operator.NotEqual:
						{
							ISqlExpression e = null;

							if (expr.Expr1 is SqlValue && ((SqlValue) expr.Expr1).Value == null)
								e = expr.Expr2;
							else if (expr.Expr2 is SqlValue && ((SqlValue) expr.Expr2).Value == null)
								e = expr.Expr1;

							if (e != null)
							{
								BuildExpression(sb, GetPrecedence(expr), e);
								sb.Append(expr.Operator == SqlBuilder.Predicate.Operator.Equal? " IS NULL": " IS NOT NULL");
								return;
							}

							break;
						}
				}

				BuildExpression(sb, GetPrecedence(expr), expr.Expr1);

				switch (expr.Operator)
				{
					case SqlBuilder.Predicate.Operator.Equal         : sb.Append(" = ");  break;
					case SqlBuilder.Predicate.Operator.NotEqual      : sb.Append(" <> "); break;
					case SqlBuilder.Predicate.Operator.Greater       : sb.Append(" > ");  break;
					case SqlBuilder.Predicate.Operator.GreaterOrEqual: sb.Append(" >= "); break;
					case SqlBuilder.Predicate.Operator.NotGreater    : sb.Append(" !> "); break;
					case SqlBuilder.Predicate.Operator.Less          : sb.Append(" < ");  break;
					case SqlBuilder.Predicate.Operator.LessOrEqual   : sb.Append(" <= "); break;
					case SqlBuilder.Predicate.Operator.NotLess       : sb.Append(" !< "); break;
				}

				BuildExpression(sb, GetPrecedence(expr), expr.Expr2);
			}
			else if (predicate is SqlBuilder.Predicate.Like)
			{
				BuildLikePredicate(sb, (SqlBuilder.Predicate.Like)predicate);
			}
			else if (predicate is SqlBuilder.Predicate.Between)
			{
				throw new NotImplementedException();
			}
			else if (predicate is SqlBuilder.Predicate.IsNull)
			{
				SqlBuilder.Predicate.IsNull p = (SqlBuilder.Predicate.IsNull)predicate;
				BuildExpression(sb, GetPrecedence(p), p.Expr1);
				sb.Append(p.IsNot? " IS NOT NULL": " IS NULL");
			}
			else if (predicate is SqlBuilder.Predicate.InSubquery)
			{
				throw new NotImplementedException();
			}
			else if (predicate is SqlBuilder.Predicate.InList)
			{
				throw new NotImplementedException();
			}
			else if (predicate is SqlBuilder.Predicate.FuncLike)
			{
				throw new NotImplementedException();
			}
			else if (predicate is SqlBuilder.SearchCondition)
			{
				BuildSearchCondition(sb, predicate.Precedence, (SqlBuilder.SearchCondition)predicate);
			}
			else if (predicate is SqlBuilder.Predicate.NotExpr)
			{
				SqlBuilder.Predicate.NotExpr p = (SqlBuilder.Predicate.NotExpr)predicate;

				if (p.IsNot)
					sb.Append("NOT ");

				BuildExpression(sb, p.IsNot ? Precedence.LogicalNegation : GetPrecedence(p), p.Expr1);
			}
			else if (predicate is SqlBuilder.Predicate.Expr)
			{
				SqlBuilder.Predicate.Expr p = (SqlBuilder.Predicate.Expr)predicate;

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
			else
			{
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

		protected virtual void BuildLikePredicate(StringBuilder sb, SqlBuilder.Predicate.Like predicate)
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

			if (expr is SqlField)
			{
				SqlField field = (SqlField)expr;

				if (field == field.Table.All)
					sb.Append("*");
				else
				{
					string table = GetTableAlias(_sqlBuilder.GetTableSource(field.Table)) ?? GetTablePhysicalName(field.Table);

					if (string.IsNullOrEmpty(table))
						throw new SqlException(string.Format("Table {0} should have an alias.", field.Table));

					addAlias = alias != field.PhysicalName;

					sb
						.Append(table)
						.Append('.')
						.Append(_dataProvider.Convert(field.PhysicalName, ConvertType.NameToQueryField));
				}
			}
			else if (expr is SqlBuilder.Column)
			{
				SqlBuilder.Column column = (SqlBuilder.Column)expr;
				ISqlTableSource   table  = _sqlBuilder.GetTableSource(column.Parent);

				if (table == null)
					throw new SqlException(string.Format("Table not found for '{0}'.", column));

				string tableAlias = GetTableAlias(table) ?? GetTablePhysicalName(column.Parent);

				if (string.IsNullOrEmpty(tableAlias))
					throw new SqlException(string.Format("Table {0} should have an alias.", column.Parent));

				addAlias = alias != column.Alias;

				sb
					.Append(tableAlias)
					.Append('.')
					.Append(_dataProvider.Convert(column.Alias, ConvertType.NameToQueryField));
			}
			else if (expr is SqlBuilder)
			{
				sb.Append("(").AppendLine();
				_nextNesting = BuildSqlBuilder((SqlBuilder)expr, sb, _indent + 1, _nextNesting);
				AppendIndent(sb).Append(")");
			}
			else if (expr is SqlValue)
			{
				BuildValue(sb, ((SqlValue)expr).Value);
			}
			else if (expr is SqlExpression)
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
			else if (expr is SqlBinaryExpression)
			{
				BuildBinaryExpression(sb, (SqlBinaryExpression)expr);
			}
			else if (expr is SqlFunction)
			{
				BuildFunction(sb, (SqlFunction)expr);
			}
			else if (expr is SqlParameter)
			{
				SqlParameter parm = (SqlParameter)expr;

				if (parm.IsQueryParameter)
					sb.Append(_dataProvider.Convert(parm.Name, ConvertType.NameToQueryParameter));
				else
					BuildValue(sb, parm.Value);
			}
			else if (expr is SqlDataType)
			{
				BuildDataType(sb, (SqlDataType)expr);
			}
			else if (expr is SqlBuilder.SearchCondition)
			{
				BuildSearchCondition(sb, expr.Precedence, (SqlBuilder.SearchCondition)expr);
			}
			else
			{
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

		protected virtual void BuildValue(StringBuilder sb, object value)
		{
			if      (value == null)   sb.Append("NULL");
			else if (value is string) sb.Append('\'').Append(value.ToString().Replace("'", "''")).Append('\'');
			else if (value is char)   sb.Append('\'').Append(value.ToString().Replace("'", "''")).Append('\'');
			else if (value is bool)   sb.Append((bool)value? "1": "0");
			else    sb.Append(value);
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

		#region Alternative Builders

		protected virtual void BuildAliases(StringBuilder sb, string table, List<SqlBuilder.Column> columns, string postfix)
		{
			_indent++;

			bool first = true;

			foreach (SqlBuilder.Column col in columns)
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

				if (!SqlBuilder.OrderBy.IsEmpty && !implementOrderBy)
					sb.Append("()");
				else
				{
					sb.AppendLine();
					AppendIndent(sb).Append("(").AppendLine();

					_indent++;

					if (SqlBuilder.OrderBy.IsEmpty)
					{
						AppendIndent(sb).Append("ORDER BY").AppendLine();
						BuildAliases(sb, aliases[0], SqlBuilder.Select.Columns, null);
					}
					else
						BuildAlternativeOrderBy(sb, true);

					_indent--;
					AppendIndent(sb).Append(")");
				}

				sb.AppendFormat(" as {0}", rnaliase).AppendLine();
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
					BuildExpression(sb, Add(SqlBuilder.Select.SkipValue, 1));
					sb.Append(" AND ");
					BuildExpression(sb, Add<int>(SqlBuilder.Select.SkipValue, SqlBuilder.Select.TakeValue));
				}
				else
				{
					AppendIndent(sb).AppendFormat("{0}.{1} > ", aliases[1], rnaliase);
					BuildExpression(sb, SqlBuilder.Select.SkipValue);
				}

				sb.AppendLine();
				_indent--;

				//AppendIndent(sb).Append("ORDER BY").AppendLine();
				//_indent++;
				//AppendIndent(sb).AppendFormat("{0}.{1}", aliases[1], rnaliase).AppendLine();
				//_indent--;
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
			BuildExpression(sb, SqlBuilder.Select.TakeValue);
			sb.Append(" *").AppendLine();
			AppendIndent(sb).Append("FROM").AppendLine();
			AppendIndent(sb).Append("(")   .AppendLine();
			_indent++;

			if (SqlBuilder.OrderBy.IsEmpty)
			{
				AppendIndent(sb).Append("SELECT TOP ");
				BuildExpression(sb, Add<int>(SqlBuilder.Select.SkipValue, SqlBuilder.Select.TakeValue));
				sb.Append(" *").AppendLine();
				AppendIndent(sb).Append("FROM").AppendLine();
				AppendIndent(sb).Append("(")   .AppendLine();
				_indent++;
			}

			buildSql(sb);

			if (SqlBuilder.OrderBy.IsEmpty)
			{
				_indent--;
				AppendIndent(sb).AppendFormat(") {0}", aliases[2]).AppendLine();
				AppendIndent(sb).Append("ORDER BY").AppendLine();
				BuildAliases(sb, aliases[2], SqlBuilder.Select.Columns, null);
			}

			_indent--;
			AppendIndent(sb).AppendFormat(") {0}", aliases[1]).AppendLine();

			if (SqlBuilder.OrderBy.IsEmpty)
			{
				AppendIndent(sb).Append("ORDER BY").AppendLine();
				BuildAliases(sb, aliases[1], SqlBuilder.Select.Columns, " DESC");
			}
			else
			{
				BuildAlternativeOrderBy(sb, false);
			}

			_indent--;
			AppendIndent(sb).AppendFormat(") {0}", aliases[0]).AppendLine();

			if (SqlBuilder.OrderBy.IsEmpty)
			{
				AppendIndent(sb).Append("ORDER BY").AppendLine();
				BuildAliases(sb, aliases[0], SqlBuilder.Select.Columns, null);
			}
			else
			{
				BuildAlternativeOrderBy(sb, true);
			}
		}

		protected void BuildAlternativeOrderBy(StringBuilder sb, bool ascending)
		{
			AppendIndent(sb).Append("ORDER BY").AppendLine();

			string[] obys = GetTempAliases(SqlBuilder.OrderBy.Items.Count, "oby");

			_indent++;

			for (int i = 0; i < obys.Length; i++)
			{
				AppendIndent(sb).Append(obys[i]);

				if ( ascending &&  SqlBuilder.OrderBy.Items[i].IsDescending ||
					!ascending && !SqlBuilder.OrderBy.Items[i].IsDescending)
					sb.Append(" DESC");

				if (i + 1 < obys.Length)
					sb.Append(',');

				sb.AppendLine();
			}

			_indent--;
		}

#pragma warning disable 1911

		protected delegate IEnumerable<SqlBuilder.Column> ColumnSelector();

		protected IEnumerable<SqlBuilder.Column> AlternativeGetSelectedColumns(ColumnSelector columnSelector)
		{
			foreach (SqlBuilder.Column col in columnSelector())
				yield return col;

			string[] obys = GetTempAliases(SqlBuilder.OrderBy.Items.Count, "oby");

			for (int i = 0; i < obys.Length; i++)
				yield return new SqlBuilder.Column(SqlBuilder, SqlBuilder.OrderBy.Items[i].Expression, obys[i]);
		}

#pragma warning restore 1911

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
			return SqlBuilder.GetTempAliases(n, defaultAlias + (Nesting == 0? "": "n" + Nesting.ToString()));
		}

		static string GetTableAlias(ISqlTableSource table)
		{
			if (table is SqlBuilder.TableSource)
			{
				SqlBuilder.TableSource ts = (SqlBuilder.TableSource)table;
				return string.IsNullOrEmpty(ts.Alias) ? GetTableAlias(ts.Source) : ts.Alias;
			}

			if (table is SqlTable)
				return ((SqlTable)table).Alias;

			throw new InvalidOperationException();
		}

		string GetTablePhysicalName(ISqlTableSource table)
		{
			if (table is SqlTable)
			{
				SqlTable tbl = (SqlTable)table;

				string tableName = _dataProvider.Convert(tbl.PhysicalName, ConvertType.NameToQueryTable).ToString();

				if (tbl.Database == null && tbl.Owner == null)
					return tableName;

				string name = string.Empty;

				if (tbl.Database != null)
					name = _dataProvider.Convert(tbl.Database, ConvertType.NameToDatabase).ToString();

				if (tbl.Owner != null)
					name +=
						_dataProvider.DatabaseOwnerDelimiter +
						_dataProvider.Convert(tbl.Owner, ConvertType.NameToOwner) +
						_dataProvider.OwnerTableDelimiter;
				else
					name += _dataProvider.DatabaseTableDelimiter;

				return name + tableName;
			}

			if (table is SqlBuilder.TableSource)
				return GetTablePhysicalName(((SqlBuilder.TableSource)table).Source);

			throw new InvalidOperationException();
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
							new SqlBuilder.SearchCondition().Expr(func.Parameters[0]). Greater .Expr(func.Parameters[1]).ToExpr(), new SqlValue(1),
							new SqlBuilder.SearchCondition().Expr(func.Parameters[0]). Equal   .Expr(func.Parameters[1]).ToExpr(), new SqlValue(0),
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

			return expression;
		}

		public virtual ISqlPredicate ConvertPredicate(ISqlPredicate predicate)
		{
			if (predicate is SqlBuilder.Predicate.ExprExpr)
			{
				SqlBuilder.Predicate.ExprExpr expr = (SqlBuilder.Predicate.ExprExpr)predicate;

				if (expr.Operator == SqlBuilder.Predicate.Operator.Equal && expr.Expr1 is SqlValue && expr.Expr2 is SqlValue)
				{
					bool value = object.Equals(((SqlValue)expr.Expr1).Value, ((SqlValue)expr.Expr2).Value);
					return new SqlBuilder.Predicate.Expr(new SqlValue(value), Precedence.Comparison);
				}

				switch (expr.Operator)
				{
					case SqlBuilder.Predicate.Operator.Equal:
					case SqlBuilder.Predicate.Operator.Greater:
					case SqlBuilder.Predicate.Operator.Less :
						predicate = OptimizeCase(expr);
						break;
				}

				if (predicate is SqlBuilder.Predicate.ExprExpr)
				{
					expr = (SqlBuilder.Predicate.ExprExpr)predicate;

					switch (expr.Operator)
					{
						case SqlBuilder.Predicate.Operator.Equal :
						case SqlBuilder.Predicate.Operator.NotEqual :
							ISqlExpression expr1 = expr.Expr1;
							ISqlExpression expr2 = expr.Expr2;

							if (expr1.CanBeNull() && expr2.CanBeNull())
							{
								if (expr1 is SqlParameter || expr2 is SqlParameter)
									SqlBuilder.ParameterDependent = true;
								else
									if (expr1 is SqlBuilder.Column || expr1 is SqlField)
									if (expr2 is SqlBuilder.Column || expr2 is SqlField)
										predicate = ConvertEqualPredicate(expr);
							}

							break;
					}
				}
			}
			else if (predicate.GetType() == typeof(SqlBuilder.Predicate.NotExpr))
			{
				SqlBuilder.Predicate.NotExpr expr = (SqlBuilder.Predicate.NotExpr)predicate;

				if (expr.IsNot && expr.Expr1 is SqlBuilder.SearchCondition)
				{
					SqlBuilder.SearchCondition sc = (SqlBuilder.SearchCondition)expr.Expr1;

					if (sc.Conditions.Count == 1)
					{
						SqlBuilder.Condition cond = sc.Conditions[0];

						if (cond.IsNot)
							return cond.Predicate;

						if (cond.Predicate is SqlBuilder.Predicate.ExprExpr)
						{
							SqlBuilder.Predicate.ExprExpr ee = (SqlBuilder.Predicate.ExprExpr)cond.Predicate;

							if (ee.Operator == SqlBuilder.Predicate.Operator.Equal)
								return new SqlBuilder.Predicate.ExprExpr(ee.Expr1, SqlBuilder.Predicate.Operator.NotEqual, ee.Expr2);

							if (ee.Operator == SqlBuilder.Predicate.Operator.NotEqual)
								return new SqlBuilder.Predicate.ExprExpr(ee.Expr1, SqlBuilder.Predicate.Operator.Equal, ee.Expr2);
						}
					}
				}
			}

			return predicate;
		}

		protected ISqlPredicate ConvertEqualPredicate(SqlBuilder.Predicate.ExprExpr expr)
		{
			ISqlExpression expr1 = expr.Expr1;
			ISqlExpression expr2 = expr.Expr2;

			SqlBuilder.SearchCondition cond = new SqlBuilder.SearchCondition();

			if (expr.Operator == SqlBuilder.Predicate.Operator.Equal)
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

		ISqlPredicate OptimizeCase(SqlBuilder.Predicate.ExprExpr expr)
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
					SqlBuilder.SearchCondition c1 = func.Parameters[0] as SqlBuilder.SearchCondition;
					SqlValue                   v1 = func.Parameters[1] as SqlValue;
					SqlBuilder.SearchCondition c2 = func.Parameters[2] as SqlBuilder.SearchCondition;
					SqlValue                   v2 = func.Parameters[3] as SqlValue;
					SqlValue                   v3 = func.Parameters[4] as SqlValue;

					if (c1 != null && c1.Conditions.Count == 1 && v1 != null && v1.Value is int &&
						c2 != null && c2.Conditions.Count == 1 && v2 != null && v2.Value is int && v3 != null && v3.Value is int)
					{
						SqlBuilder.Predicate.ExprExpr ee1 = c1.Conditions[0].Predicate as SqlBuilder.Predicate.ExprExpr;
						SqlBuilder.Predicate.ExprExpr ee2 = c2.Conditions[0].Predicate as SqlBuilder.Predicate.ExprExpr;

						if (ee1 != null && ee2 != null && ee1.Expr1.Equals(ee2.Expr1) && ee1.Expr2.Equals(ee2.Expr2))
						{
							int e = 0, g = 0, l = 0;

							if (ee1.Operator == SqlBuilder.Predicate.Operator.Equal   || ee2.Operator == SqlBuilder.Predicate.Operator.Equal)   e = 1;
							if (ee1.Operator == SqlBuilder.Predicate.Operator.Greater || ee2.Operator == SqlBuilder.Predicate.Operator.Greater) g = 1;
							if (ee1.Operator == SqlBuilder.Predicate.Operator.Less    || ee2.Operator == SqlBuilder.Predicate.Operator.Less)    l = 1;

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

									return ConvertPredicate(new SqlBuilder.Predicate.ExprExpr(
										ee1.Expr1,
										e == 0 ? SqlBuilder.Predicate.Operator.Equal :
										g == 0 ? SqlBuilder.Predicate.Operator.Greater :
												 SqlBuilder.Predicate.Operator.Less,
										ee1.Expr2));
								}
							}

						}
					}
				}
				else if (expr.Operator == SqlBuilder.Predicate.Operator.Equal && func.Parameters.Length == 3)
				{
					SqlBuilder.SearchCondition sc = func.Parameters[0] as SqlBuilder.SearchCondition;
					SqlValue                   v1 = func.Parameters[1] as SqlValue;
					SqlValue                   v2 = func.Parameters[2] as SqlValue;

					if (sc != null && v1 != null && v2 != null)
					{
						if (object.Equals(value.Value, v1.Value))
							return sc;

						if (object.Equals(value.Value, v2.Value) && !sc.CanBeNull())
							return ConvertPredicate(new SqlBuilder.Predicate.NotExpr(sc, true, Precedence.LogicalNegation));
					}
				}
			}

			return expr;
		}

		static bool Compare(int v1, int v2, SqlBuilder.Predicate.Operator op)
		{
			switch (op)
			{
				case SqlBuilder.Predicate.Operator.Equal:           return v1 == v2;
				case SqlBuilder.Predicate.Operator.NotEqual:        return v1 != v2;
				case SqlBuilder.Predicate.Operator.Greater:         return v1 >  v2;
				case SqlBuilder.Predicate.Operator.NotLess:
				case SqlBuilder.Predicate.Operator.GreaterOrEqual:  return v1 >= v2;
				case SqlBuilder.Predicate.Operator.Less:            return v1 <  v2;
				case SqlBuilder.Predicate.Operator.NotGreater:
				case SqlBuilder.Predicate.Operator.LessOrEqual:     return v1 <= v2;
			}

			throw new InvalidOperationException();
		}

		public virtual string Name
		{
			get { return GetType().Name.Replace("SqlProvider", ""); }
		}

		#endregion
	}
}
