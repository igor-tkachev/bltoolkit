using System;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class BasicSqlProvider : ISqlProvider
	{
		#region Init

		public BasicSqlProvider(DataProviderBase dataProvider)
		{
			_dataProvider = dataProvider;
		}

		readonly DataProviderBase _dataProvider;

		SqlBuilder    _sqlBuilder;
		StringBuilder _sb;
		int           _indent;

		#endregion

		#region BuildSQL

		public string BuildSql(SqlBuilder sqlBuilder)
		{
			StringBuilder sb = new StringBuilder();

			BuildSql(sqlBuilder, sb, 0);

			return sb.ToString();
		}

		void BuildSql(SqlBuilder sqlBuilder, StringBuilder sb, int indent)
		{
			_sqlBuilder = sqlBuilder;
			_sb         = sb;
			_indent     = indent;

			BuildSelectClause();
			BuildFromClause  ();
			BuildWhereClause();
		}

		#endregion

		#region Overrides

		protected virtual void BuildSqlBuilder(SqlBuilder sqlBuilder, StringBuilder sb, int indent)
		{
			new BasicSqlProvider(_dataProvider).BuildSql(sqlBuilder, sb, indent);
		}

		#endregion

		#region Build Select

		protected virtual void BuildSelectClause()
		{
			AppendIndent();

			_sb.Append("SELECT").AppendLine();

			_indent++;

			foreach (SqlBuilder.Column col in _sqlBuilder.Select.Columns)
			{
				bool addAlias = true;

				AppendIndent().Append(BuildExpression(col.Expression, col.Alias, ref addAlias));

				if (addAlias)
					_sb.Append(" as ").Append(col.Alias);

				_sb.Append(',').AppendLine();
			}

			_indent--;

			_sb
				.Remove(_sb.Length - Environment.NewLine.Length - 1, Environment.NewLine.Length + 1)
				.AppendLine();
		}

		#endregion

		#region Build From

		protected virtual void BuildFromClause()
		{
			AppendIndent();

			_sb.Append("FROM").AppendLine();

			_indent++;

			foreach (SqlBuilder.TableSource ts in _sqlBuilder.From.Tables)
			{
				AppendIndent();

				BuildPhysicalTable(ts.Source);

				string alias = GetTableAlias(ts);

				if (!string.IsNullOrEmpty(alias))
					_sb.Append(" ").Append(alias);
			}

			_indent--;

			_sb.AppendLine();
		}

		void BuildPhysicalTable(ISqlTableSource table)
		{
			if (table is SqlTable || table is SqlBuilder.TableSource)
			{
				_sb.Append(GetTablePhysicalName(table));
			}
			else if (table is SqlBuilder)
			{
				_sb.Append("(").AppendLine();
				BuildSqlBuilder((SqlBuilder)table, _sb, _indent + 1);
				AppendIndent().Append(")");
			}
			else
				throw new InvalidOperationException();
		}

		#endregion

		#region Where Clause

		protected virtual void BuildWhereClause()
		{
			if (_sqlBuilder.Where.SearchCondition.Conditions.Count == 0)
				return;

			AppendIndent();

			_sb.Append("WHERE").AppendLine();

			_indent++;

			bool? isOr = null;

			foreach (SqlBuilder.Condition col in _sqlBuilder.Where.SearchCondition.Conditions)
			{
				if (isOr != null)
					_sb.Append(isOr.Value ? " OR" : " AND");

				AppendIndent();
				BuildPredicate(col.Predicate);
				isOr = col.IsOr;
			}

			_indent--;

			_sb.AppendLine();
		}

		#endregion

		#region Helpers

		protected virtual void BuildPredicate(SqlBuilder.IPredicate predicate)
		{
			bool dummy = false;

			if (predicate is SqlBuilder.Predicate.ExprExpr)
			{
				var expr = (SqlBuilder.Predicate.ExprExpr)predicate;

				_sb.Append(BuildExpression(expr.Expr1, null, ref dummy));

				switch (expr.Operator)
				{
					case SqlBuilder.Predicate.Operator.Equal         : _sb.Append(" = ");  break;
					case SqlBuilder.Predicate.Operator.NotEqual      : _sb.Append(" <> "); break;
					case SqlBuilder.Predicate.Operator.Greater       : _sb.Append(" > ");  break;
					case SqlBuilder.Predicate.Operator.GreaterOrEqual: _sb.Append(" >= "); break;
					case SqlBuilder.Predicate.Operator.NotGreater    : _sb.Append(" !> "); break;
					case SqlBuilder.Predicate.Operator.Less          : _sb.Append(" <  "); break;
					case SqlBuilder.Predicate.Operator.LessOrEqual   : _sb.Append(" <= "); break;
					case SqlBuilder.Predicate.Operator.NotLess       : _sb.Append(" !< "); break;
				}

				_sb.Append(BuildExpression(expr.Expr2, null, ref dummy));
			}
			else if (predicate is SqlBuilder.Predicate.Like)
			{
				throw new NotImplementedException();
			}
			else if (predicate is SqlBuilder.Predicate.Between)
			{
				throw new NotImplementedException();
			}
			else if (predicate is SqlBuilder.Predicate.IsNull)
			{
				throw new NotImplementedException();
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
			else
			{
				throw new InvalidOperationException();
			}
		}

		protected virtual string BuildExpression(ISqlExpression expr, string alias, ref bool addAlias)
		{
			if (expr is SqlField)
			{
				SqlField field = (SqlField)expr;

				string table = GetTableAlias(_sqlBuilder.From[field.Table]) ?? GetTablePhysicalName(field.Table);

				if (string.IsNullOrEmpty(table))
					throw new SqlException(string.Format("Table {0} should have alias.", field.Table));

				addAlias = alias != field.PhysicalName;

				return string.Format("{0}.{1}",
					table,
					field.Name == "*"? field.PhysicalName: _dataProvider.Convert(field.PhysicalName, ConvertType.NameToQueryField));
			}

			if (expr is SqlBuilder.Column)
			{
				SqlBuilder.Column column = (SqlBuilder.Column)expr;

				string table = GetTableAlias(_sqlBuilder.From[column.Parent]) ?? GetTablePhysicalName(column.Parent);

				if (string.IsNullOrEmpty(table))
					throw new SqlException(string.Format("Table {0} should have alias.", column.Parent));

				addAlias = alias != column.Alias;

				return string.Format("{0}.{1}",
					table,
					_dataProvider.Convert(column.Alias, ConvertType.NameToQueryField));
			}

			if (expr is SqlBuilder)
			{
				SqlBuilder builder = (SqlBuilder)expr;
				throw new NotImplementedException();
			}

			if (expr is SqlValue)
			{
				SqlValue value = (SqlValue)expr;
				return
					value.Value == null?
					"NULL" : value.Value.ToString();
			}

			if (expr is SqlExpression)
			{
				SqlExpression e = (SqlExpression)expr;

				object[] values = new object[e.Values.Length];
				bool     dummy  = false;

				for (int i = 0; i < values.Length; i++)
					values[i] = BuildExpression(e.Values[i], "", ref dummy);

				return string.Format(e.Expr, values);
			}

			if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction)expr;
				throw new NotImplementedException();
			}

			if (expr is SqlParameter)
			{
				SqlParameter parm = (SqlParameter)expr;
				throw new NotImplementedException();
			}

			throw new InvalidOperationException();
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

		StringBuilder AppendIndent()
		{
			if (_indent > 0)
				_sb.Append('\t', _indent);

			return _sb;
		}

		#endregion
	}
}
