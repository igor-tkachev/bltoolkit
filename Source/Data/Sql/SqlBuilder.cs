using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public class SqlBuilder : ISqlExpression, ITableSource
	{
		#region Column

		public class Column : IEquatable<Column>
		{
			public Column(ISqlExpression expression, string alias)
			{
				_expression  = expression;
				_alias       = alias;
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

		public static Column CreateColumn(ISqlExpression expression)
		{
			return CreateColumn(expression, null);
		}

		public static Column CreateColumn(ISqlExpression expression, string alias)
		{
			if (expression == null) throw new ArgumentNullException("expression");

			return new Column(expression, alias);
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
				_builder.AddColumn(CreateColumn(field));
				return this;
			}

			public SelectClause Field(Field field, string alias)
			{
				_builder.AddColumn(CreateColumn(field, alias));
				return this;
			}

			public SelectClause Sql(SqlBuilder sql)
			{
				_builder.AddColumn(CreateColumn(sql));
				return this;
			}

			public SelectClause Sql(SqlBuilder sql, string alias)
			{
				_builder.AddColumn(CreateColumn(sql, alias));
				return this;
			}

			public SelectClause Expr(ISqlExpression expr)
			{
				_builder.AddColumn(CreateColumn(expr));
				return this;
			}

			public SelectClause Expr(ISqlExpression expr, string alias)
			{
				_builder.AddColumn(CreateColumn(expr, alias));
				return this;
			}

			public SelectClause Expr(string expr, params ISqlExpression[] values)
			{
				_builder.AddColumn(CreateColumn(new SqlExpression(expr, values)));
				return this;
			}

			public SelectClause Expr(string alias, string expr, params ISqlExpression[] values)
			{
				_builder.AddColumn(CreateColumn(new SqlExpression(expr, values)));
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
		}

		Column AddColumn(Column col)
		{
			foreach (Column c in SelectList)
				if (c.Equals(col))
					return col;

			//col.Index = SelectList.Count;

			SelectList.Add(col);

			return col;
		}

		public SelectClause Select
		{
			get { return new SelectClause(this); }
		}

		readonly List<Column> _selectList = new List<Column>();
		public   List<Column>  SelectList
		{
			get { return _selectList; }
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
				TableSource t = new TableSource(table, alias);

				_builder._fromTable = t;

				return t;
			}
		}

		private TableSource _fromTable;
		public  TableSource  FromTable
		{
			get { return _fromTable; }
		}

		public FromClause From
		{
			get { return new FromClause(this); }
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
