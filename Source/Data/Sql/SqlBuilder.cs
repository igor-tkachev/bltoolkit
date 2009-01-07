using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public class SqlBuilder : ISqlExpression, ITableSource
	{
		#region Init

		public SqlBuilder()
		{
			_select = new SelectClause(this);
			_from   = new FromClause(this);
		}

		#endregion

		#region Column

		public class Column : IEquatable<Column>
		{
			public Column(ISqlExpression expression, string alias)
			{
				if (expression == null) throw new ArgumentNullException("expression");

				_expression  = expression;
				_alias       = alias;
			}

			public Column(ISqlExpression expression)
				: this(expression, null)
			{
			}

			private ISqlExpression _expression;
			public  ISqlExpression  Expression
			{
				get { return _expression;  }
				set { _expression = value; }
			}

			private string _alias;
			public  string  Alias
			{
				get { return _alias;  }
				set { _alias = value; }
			}

			public bool Equals(Column other)
			{
				return _alias == other.Alias && _expression.Equals(other._expression);
			}
		}

		#endregion

		#region Select Clause

		public class SelectClause
		{
			readonly SqlBuilder _builder;

			internal SelectClause(SqlBuilder builder)
			{
				_builder = builder;
			}

			public SelectClause Field(Field field)
			{
				AddOrGetColumn(new Column(field));
				return this;
			}

			public SelectClause Field(Field field, string alias)
			{
				AddOrGetColumn(new Column(field, alias));
				return this;
			}

			public SelectClause Sql(SqlBuilder sql)
			{
				AddOrGetColumn(new Column(sql));
				return this;
			}

			public SelectClause Sql(SqlBuilder sql, string alias)
			{
				AddOrGetColumn(new Column(sql, alias));
				return this;
			}

			public SelectClause Expr(ISqlExpression expr)
			{
				AddOrGetColumn(new Column(expr));
				return this;
			}

			public SelectClause Expr(ISqlExpression expr, string alias)
			{
				AddOrGetColumn(new Column(expr, alias));
				return this;
			}

			public SelectClause Expr(string expr, params ISqlExpression[] values)
			{
				AddOrGetColumn(new Column(new SqlExpression(expr, values)));
				return this;
			}

			public SelectClause Expr(string alias, string expr, params ISqlExpression[] values)
			{
				AddOrGetColumn(new Column(new SqlExpression(expr, values)));
				return this;
			}

			public FromClause From
			{
				get { return _builder.From; }
			}

			public SqlBuilder End()
			{
				return _builder;
			}

			Column AddOrGetColumn(Column col)
			{
				foreach (Column c in Columns)
					if (c.Equals(col))
						return col;

				//col.Index = SelectList.Count;

				Columns.Add(col);

				return col;
			}

			readonly List<Column> _columns = new List<Column>();
			public   List<Column>  Columns
			{
				get { return _columns; }
			}
		}

		readonly SelectClause _select;
		public   SelectClause  Select
		{
			get { return _select; }
		}

		#endregion

		#region TableSource

		public class TableSource : ITableSource
		{
			public TableSource(ITableSource source, string alias)
			{
				_source = source;
				_alias  = alias;
			}

			private ITableSource _source;
			public  ITableSource  Source
			{
				get { return _source;  }
				set { _source = value; }
			}

			private string _alias;
			public  string  Alias
			{
				get { return _alias;  }
				set { _alias = value; }
			}
		}

		#endregion

		#region From

		public class FromClause
		{
			internal FromClause(SqlBuilder builder)
			{
				_builder = builder;
			}

			internal SqlBuilder _builder;

			public TableSource Table(ITableSource table)
			{
				return Table(table, null);
			}

			public TableSource Table(ITableSource table, string alias)
			{
				foreach (TableSource ts in Tables)
					if (ts.Source == table)
						if (alias == null || ts.Alias == alias)
							return ts;
						else
							throw new ArithmeticException("alias");

				TableSource t = new TableSource(table, alias);

				Tables.Add(t);

				return t;
			}

			private List<TableSource> _tables = new List<TableSource>();
			public  List<TableSource>  Tables
			{
				get { return _tables; }
			}
		}

		readonly FromClause _from;
		public   FromClause  From
		{
			get { return _from; }
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			return (object)this == other;
		}

		#endregion
	}
}
