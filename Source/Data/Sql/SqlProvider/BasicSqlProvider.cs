using System;
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
			get { return _sqlBuilder; }
		}

		private int _indent;
		public  int  Indent
		{
			get { return _indent;  }
			set { _indent = value; }
		}

		#endregion

		#region BuildSQL

		public StringBuilder BuildSql(SqlBuilder sqlBuilder, StringBuilder sb, int indent)
		{
			_sqlBuilder = sqlBuilder;
			_indent     = indent;

			BuildSelectClause(sb);
			BuildFromClause  (sb);
			BuildWhereClause (sb);

			return sb;
		}

		#endregion

		#region Overrides

		protected virtual void BuildSqlBuilder(SqlBuilder sqlBuilder, StringBuilder sb, int indent)
		{
			DataProvider.CreateSqlProvider().BuildSql(sqlBuilder, sb, indent);
		}

		protected virtual bool ParenthesizeJoin()
		{
			return false;
		}

		#endregion

		#region Build Select

		protected virtual void BuildSelectClause(StringBuilder sb)
		{
			AppendIndent(sb);
			sb.Append("SELECT").AppendLine();
			BuildColumns(sb);
		}

		protected virtual void BuildColumns(StringBuilder sb)
		{
			_indent++;

			bool first = true;

			foreach (SqlBuilder.Column col in _sqlBuilder.Select.Columns)
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
				BuildSqlBuilder((SqlBuilder)table, sb, _indent + 1);
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

			sb.Append("INNER JOIN ");

			BuildPhysicalTable(sb, join.Table.Source);

			string alias = GetTableAlias(join.Table);

			if (!string.IsNullOrEmpty(alias))
				sb.Append(" ").Append(alias);

			sb.Append(" ON ");

			BuildSearchCondition(sb, join.Condition);

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

		protected virtual void BuildWhereClause(StringBuilder sb)
		{
			if (_sqlBuilder.Where.SearchCondition.Conditions.Count == 0)
				return;

			AppendIndent(sb);

			sb.Append("WHERE").AppendLine();

			_indent++;
			AppendIndent(sb);
			BuildSearchCondition(sb, _sqlBuilder.Where.SearchCondition);
			_indent--;

			sb.AppendLine();
		}

		#endregion

		#region Builders

		#region BuildSearchCondition

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
			else if (predicate is SqlBuilder.Predicate.Expr)
			{
				SqlBuilder.Predicate.Expr p = (SqlBuilder.Predicate.Expr)predicate;
				BuildExpression(sb, GetPrecedence(p), p.Expr1);
			}
			else if (predicate is SqlBuilder.Predicate.NotExpr)
			{
				throw new NotImplementedException();
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

		protected virtual void BuildExpression(StringBuilder sb, ISqlExpression expr, string alias, ref bool addAlias)
		{
			expr = ConvertExpression(expr);

			if (expr is SqlField)
			{
				SqlField field = (SqlField)expr;

				string table = GetTableAlias(_sqlBuilder.From[field.Table]) ?? GetTablePhysicalName(field.Table);

				if (string.IsNullOrEmpty(table))
					throw new SqlException(string.Format("Table {0} should have an alias.", field.Table));

				addAlias = alias != field.PhysicalName;

				sb
					.Append(table)
					.Append('.')
					.Append(field.Name == "*"? field.PhysicalName: _dataProvider.Convert(field.PhysicalName, ConvertType.NameToQueryField));
			}
			else if (expr is SqlBuilder.Column)
			{
				SqlBuilder.Column column = (SqlBuilder.Column)expr;
				ISqlTableSource   table  = _sqlBuilder.From[column.Parent];

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
				SqlBuilder builder = (SqlBuilder)expr;
				throw new NotImplementedException();
			}
			else if (expr is SqlValue)
			{
				object value = ((SqlValue)expr).Value;

				if      (value == null)   sb.Append("NULL");
				else if (value is string) sb.Append('\'').Append(value.ToString().Replace("'", "''")).Append('\'');
				else if (value is char)   sb.Append('\'').Append(value.ToString().Replace("'", "''")).Append('\'');
				else if (value is bool)   sb.Append((bool)value? "1 = 1": "1 = 0");
				else    sb.Append(value);
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
				sb.Append(_dataProvider.Convert(parm.Name, ConvertType.NameToQueryParameter));
			}
			else if (expr is SqlBuilder.SearchCondition)
			{
				BuildSearchCondition(sb, expr.Precedence, (SqlBuilder.SearchCondition)expr);
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		protected void BuildExpression(StringBuilder sb, int parentPrecedence, ISqlExpression expr, string alias, ref bool addAlias)
		{
			bool wrap = Wrap(GetPrecedence(expr), parentPrecedence);

			if (wrap) sb.Append('(');
			BuildExpression(sb, expr, alias, ref addAlias);
			if (wrap) sb.Append(')');
		}

		protected virtual void BuildExpression(StringBuilder sb, ISqlExpression expr)
		{
			bool dummy = false;
			BuildExpression(sb, expr, null, ref dummy);
		}

		protected void BuildExpression(StringBuilder sb, int precedence, ISqlExpression expr)
		{
			bool dummy = false;
			BuildExpression(sb, precedence, expr, null, ref dummy);
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
				sb.Append("CASE").AppendLine();

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

		#region Helpers

		static bool Wrap(int precedence, int parentPrecedence)
		{
			return
				precedence == 0 ||
				precedence < parentPrecedence ||
				(precedence == parentPrecedence && parentPrecedence == Precedence.Subtraction); 
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
				return _dataProvider.Convert(((SqlTable)table).PhysicalName, ConvertType.NameToQueryTable).ToString();

			if (table is SqlBuilder.TableSource)
				return GetTablePhysicalName(table);

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
							new SqlBuilder.SearchCondition().Expr(func.Parameters[0]). Greater .Expr(func.Parameters[1]).ToExpr(),   new SqlValue(1),
							new SqlBuilder.SearchCondition().Expr(func.Parameters[0]). Equal   .Expr(func.Parameters[1]).ToExpr(),   new SqlValue(0),
							new SqlValue(-1)));

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

					/*
					case "Length":
						if (func.Parameters[0] is SqlValue)
						{
							SqlValue v = (SqlValue)func.Parameters[0];

							if (v.Value is string)
								return new SqlValue(v.Value.ToString().Length);
						}

						break;

					case "Reverse":
						if (func.Parameters[0] is SqlValue)
						{
							SqlValue v = (SqlValue)func.Parameters[0];

							if (v.Value is string)
							{
								string str   = v.Value.ToString();
								char[] chars = str.ToCharArray();

								Array.Reverse(chars);
								return new SqlValue(new string(chars));
							}
						}

						break;
					*/
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
					case SqlBuilder.Predicate.Operator.Less : return OptimizeCase(expr);
				}
			}

			return predicate;
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

			if (value != null && func != null && value.Value is int && func.Name == "CASE" && func.Parameters.Length == 5)
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

								return new SqlBuilder.Predicate.ExprExpr(
									ee1.Expr1,
									e == 0 ? SqlBuilder.Predicate.Operator.Equal :
									g == 0 ? SqlBuilder.Predicate.Operator.Greater :
									         SqlBuilder.Predicate.Operator.Less,
									ee1.Expr2);
							}
						}

					}
				}
			}

			return expr;
		}

		bool Compare(int v1, int v2, SqlBuilder.Predicate.Operator op)
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

		#endregion
	}
}
