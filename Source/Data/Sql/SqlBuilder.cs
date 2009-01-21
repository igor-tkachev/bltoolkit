using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public class SqlBuilder : ISqlExpression, ITableSource
	{
		#region Init

		public SqlBuilder()
		{
			_select  = new SelectClause (this);
			_from    = new FromClause   (this);
			_where   = new WhereClause  (this);
			_groupBy = new GroupByClause(this);
			_having  = new HavingClause (this);
			_orderBy = new OrderByClause(this);
		}

		#endregion

		#region Column

		public class Column : IEquatable<Column>
		{
			public Column(ISqlExpression expression, string alias)
			{
				if (expression == null) throw new ArgumentNullException("expression");

				_expression = expression;
				_alias      = alias;
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

		#region TableSource

		public class TableSource : ITableSource
		{
			public TableSource(ITableSource source, string alias)
			{
				if (source == null) throw new ArgumentNullException("source");

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

			private List<TableJoin> _joins = new List<TableJoin>();
			public  List<TableJoin>  Joins
			{
				get { return _joins;  }
			}
		}

		#endregion

		#region TableJoin

		public enum JoinType
		{
			Delayed,
			InnerJoin,
			LeftJoin
		}

		public class TableJoin
		{
			public TableJoin(JoinType joinType, TableSource table, bool isWeak)
			{
				_joinType = joinType;
				_table    = table;
				_isWeak   = isWeak;
			}

			public TableJoin(JoinType joinType, ITableSource table, string alias, bool isWeak)
				: this(joinType, new TableSource(table, alias), isWeak)
			{
			}

			private JoinType _joinType;
			private JoinType  JoinType
			{
				get { return _joinType;  }
				set { _joinType = value; }
			}

			private TableSource _table;
			private TableSource  Table
			{
				get { return _table;  }
				set { _table = value; }
			}

			private ISqlExpression _joinExpression;
			private ISqlExpression  JoinExpression
			{
				get { return _joinExpression;  }
				set { _joinExpression = value; }
			}

			private bool _isWeak;
			private bool  IsWeak
			{
				get { return _isWeak;  }
				set { _isWeak = value; }
			}
		}

		#endregion

		#region Predicate

		public class Predicate
		{
		}

		#endregion

		#region SearchCondition

		public class SearchCondition
		{
			private bool      _begGroup;  public bool      BegGroup  { get { return _begGroup;  } set { _begGroup  = value; } }
			private bool      _isNot;     public bool      IsNot     { get { return _isNot;     } set { _isNot     = value; } }
			private Predicate _predicate; public Predicate Predicate { get { return _predicate; } set { _predicate = value; } }
			private bool      _endGroup;  public bool      EndGroup  { get { return _endGroup;  } set { _endGroup  = value; } }
			private bool      _isOr;      public bool      IsOr      { get { return _isOr;      } set { _isOr      = value; } }
		}

		#endregion

		#region ClauseBase

		public abstract class ClauseBase
		{
			protected ClauseBase(SqlBuilder builder)
			{
				_builder = builder;
			}

			public SelectClause  Select  { get { return Builder.Select;  } }
			public FromClause    From    { get { return Builder.From;    } }
			public GroupByClause GroupBy { get { return Builder.GroupBy; } }
			public HavingClause  Having  { get { return Builder.Having;  } }
			public OrderByClause OrderBy { get { return Builder.OrderBy; } }
			public SqlBuilder    End()   { return Builder; }

			readonly SqlBuilder _builder;
			public   SqlBuilder  Builder
			{
				get { return _builder; }
			}
		}

		#endregion

		#region SelectClause

		public class SelectClause : ClauseBase
		{
			internal SelectClause(SqlBuilder builder) : base(builder)
			{
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

			public SelectClause SubQuery(SqlBuilder sql)
			{
				AddOrGetColumn(new Column(sql));
				return this;
			}

			public SelectClause SubQuery(SqlBuilder sql, string alias)
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

		#region FromClause

		public class FromClause : ClauseBase
		{
			#region Table

			#region Join

			public class Join : FromTable
			{
				internal Join(SqlBuilder builder, TableSource table)
					: base(builder, table)
				{
				}
			}

			#endregion

			public class FromTable : ClauseBase
			{
				internal FromTable(SqlBuilder builder, TableSource table)
					: base(builder)
				{
					_table = table;
				}

				private TableSource _table;

				public FromTable Table(ITableSource table)               { return Builder.From.Table(table); }
				public FromTable Table(ITableSource table, string alias) { return Builder.From.Table(table, alias); }

				public Join Join(ITableSource table)
				{
					return Join(table, null);
				}

				public Join Join(ITableSource table, string alias)
				{
					return new Join(Builder, _table);
				}

				/*
				public static TableJoin InnerJoin(ITableSource table, string alias, ISqlExpression joinExpression, params TableJoin[] joins)
				{
					return new TableJoin(JoinType.InnerJoin, table, alias, joinExpression, false, joins);
				}

				public static TableJoin InnerJoin(ITableSource table, ISqlExpression joinExpression, params TableJoin[] joins)
				{
					return InnerJoin(table, null, joinExpression, joins);
				}

				public static TableJoin LeftJoin(ITableSource table, string alias, ISqlExpression joinExpression, params TableJoin[] joins)
				{
					return new TableJoin(JoinType.LeftJoin, table, alias, joinExpression, false, joins);
				}

				public static TableJoin LeftJoin(ITableSource table, ISqlExpression joinExpression, params TableJoin[] joins)
				{
					return LeftJoin(table, null, joinExpression, joins);
				}

				public static TableJoin DelayedJoin(ITableSource table, string alias, ISqlExpression joinExpression, params TableJoin[] joins)
				{
					return new TableJoin(JoinType.Delayed, table, alias, joinExpression, false, joins);
				}

				public static TableJoin DelayedJoin(ITableSource table, ISqlExpression joinExpression, params TableJoin[] joins)
				{
					return DelayedJoin(table, null, joinExpression, joins);
				}
				*/
			}

			#endregion

			internal FromClause(SqlBuilder builder) : base(builder)
			{
			}

			public FromTable Table(ITableSource table)
			{
				return Table(table, null);
			}

			public FromTable Table(ITableSource table, string alias)
			{
				return new FromTable(Builder, AddOrGetTable(table, alias));
			}

			TableSource AddOrGetTable(ITableSource table, string alias)
			{
				foreach (TableSource ts in Tables)
					if (ts.Source == table)
						if (alias == null || ts.Alias == alias)
							return ts;
						else
							throw new ArgumentException("alias");

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

		#region WhereClause

		public class WhereClause : ClauseBase
		{
			internal WhereClause(SqlBuilder builder) : base(builder)
			{
			}

			private List<SearchCondition> _conditions = new List<SearchCondition>();
			public  List<SearchCondition>  Conditions
			{
				get { return _conditions; }
			}
		}

		readonly WhereClause _where;
		public   WhereClause  Where
		{
			get { return _where; }
		}

		#endregion

		#region GroupByClause

		public class GroupByClause : ClauseBase
		{
			internal GroupByClause(SqlBuilder builder) : base(builder)
			{
			}

			//private List<SearchCondition> _conditions = new List<SearchCondition>();
			//public  List<SearchCondition>  Conditions
			//{
			//	get { return _conditions; }
			//}
		}

		readonly GroupByClause _groupBy;
		public   GroupByClause  GroupBy
		{
			get { return _groupBy; }
		}

		#endregion

		#region HavingClause

		public class HavingClause : ClauseBase
		{
			internal HavingClause(SqlBuilder builder) : base(builder)
			{
			}

			//private List<SearchCondition> _conditions = new List<SearchCondition>();
			//public  List<SearchCondition>  Conditions
			//{
			//	get { return _conditions; }
			//}
		}

		readonly HavingClause _having;
		public   HavingClause  Having
		{
			get { return _having; }
		}

		#endregion

		#region OrderByClause

		public class OrderByClause : ClauseBase
		{
			internal OrderByClause(SqlBuilder builder) : base(builder)
			{
			}

			//private List<SearchCondition> _conditions = new List<SearchCondition>();
			//public  List<SearchCondition>  Conditions
			//{
			//	get { return _conditions; }
			//}
		}

		readonly OrderByClause _orderBy;
		public   OrderByClause  OrderBy
		{
			get { return _orderBy; }
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
