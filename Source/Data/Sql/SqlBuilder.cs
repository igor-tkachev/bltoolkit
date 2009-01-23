using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	public class SqlBuilder : ISqlExpression, ITableSource
	{
		#region Init

		public SqlBuilder()
		{
			_select  = new SelectClause    (this);
			_from    = new FromClause      (this);
			_where   = new WhereClause.Next(this);
			_groupBy = new GroupByClause   (this);
			_having  = new HavingClause    (this);
			_orderBy = new OrderByClause   (this);
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

			public TableSource this[ITableSource table]
			{
				get { return this[table, null]; }
			}

			public TableSource this[ITableSource table, string alias]
			{
				get
				{
					foreach (JoinedTable tj in Joins)
					{
						TableSource ts = tj.Table;

						if (ts.Source == table && (alias == null || ts.Alias == alias))
							return ts;
					}

					return null;
				}
			}

			private List<JoinedTable> _joins = new List<JoinedTable>();
			public  List<JoinedTable>  Joins
			{
				get { return _joins;  }
			}
		}

		#endregion

		#region TableJoin

		public enum JoinType
		{
			Auto,
			Inner,
			Left
		}

		public class JoinedTable
		{
			public JoinedTable(JoinType joinType, TableSource table, bool isWeak)
			{
				_joinType = joinType;
				_table    = table;
				_isWeak   = isWeak;
			}

			public JoinedTable(JoinType joinType, ITableSource table, string alias, bool isWeak)
				: this(joinType, new TableSource(table, alias), isWeak)
			{
			}

			private JoinType _joinType;
			public  JoinType  JoinType
			{
				get { return _joinType;  }
				set { _joinType = value; }
			}

			private TableSource _table;
			public  TableSource  Table
			{
				get { return _table;  }
				set { _table = value; }
			}

			private SearchCondition _condition = new SearchCondition();
			public  SearchCondition  Condition
			{
				get { return _condition;  }
			}

			private bool _isWeak;
			public  bool  IsWeak
			{
				get { return _isWeak;  }
				set { _isWeak = value; }
			}
		}

		#endregion

		#region Predicate

		public interface IPredicate
		{
		}

		public abstract class Predicate : IPredicate
		{
			public enum Operator
			{
				Equal,          // =     Is the operator used to test the equality between two expressions.
				NotEqual,       // <> != Is the operator used to test the condition of two expressions not being equal to each other.
				Greater,        // >     Is the operator used to test the condition of one expression being greater than the other.
				GreaterOrEqual, // >=    Is the operator used to test the condition of one expression being greater than or equal to the other expression.
				NotGreater,     // !>    Is the operator used to test the condition of one expression not being greater than the other expression.
				Less,           // <     Is the operator used to test the condition of one expression being less than the other.
				LessOrEqual,    // <=    Is the operator used to test the condition of one expression being less than or equal to the other expression.
				NotLess         // !<    Is the operator used to test the condition of one expression not being less than the other expression.
			}

			public abstract class ExprBase : Predicate
			{
				public ExprBase(ISqlExpression exp1)
				{
					_expr1 = exp1;
				}

				readonly ISqlExpression _expr1; public ISqlExpression Expr1    { get { return _expr1; } }
			}

			public abstract class NotExprBase : ExprBase
			{
				public NotExprBase(ISqlExpression exp1, bool isNot)
					: base(exp1)
				{
					_isNot = isNot;
				}

				readonly bool _isNot; public bool IsNot { get { return _isNot; } }
			}

			// { expression { = | <> | != | > | >= | ! > | < | <= | !< } expression
			//
			public class ExprExpr : ExprBase
			{
				public ExprExpr(ISqlExpression exp1, Operator op, ISqlExpression exp2)
					: base(exp1)
				{
					_op    = op;
					_expr2 = exp2;
				}

				readonly Operator       _op;    public new Operator   Operator { get { return _op;    } }
				readonly ISqlExpression _expr2; public ISqlExpression Expr2    { get { return _expr2; } }
			}

			// string_expression [ NOT ] LIKE string_expression [ ESCAPE 'escape_character' ]
			//
			public class Like : NotExprBase
			{
				public Like(ISqlExpression exp1, bool isNot, ISqlExpression exp2, char escape)
					: base(exp1, isNot)
				{
					_expr2  = exp2;
					_escape = escape;
				}

				readonly ISqlExpression _expr2;  public ISqlExpression Expr2  { get { return _expr2;  } }
				readonly char           _escape; public char           Escape { get { return _escape; } }
			}

			// expression [ NOT ] BETWEEN expression AND expression
			//
			public class Between : NotExprBase
			{
				public Between(ISqlExpression exp1, bool isNot, ISqlExpression exp2, ISqlExpression exp3)
					: base(exp1, isNot)
				{
					_expr2 = exp2;
					_expr3 = exp3;
				}

				readonly ISqlExpression _expr2; public ISqlExpression Expr2 { get { return _expr2; } }
				readonly ISqlExpression _expr3; public ISqlExpression Expr3 { get { return _expr3; } }
			}

			// expression IS [ NOT ] NULL
			//
			public class IsNull : NotExprBase
			{
				public IsNull(ISqlExpression exp1, bool isNot)
					: base(exp1, isNot)
				{
				}
			}

			// expression [ NOT ] IN ( subquery | expression [ ,...n ] )
			//
			public class InSubquery : NotExprBase
			{
				public InSubquery(ISqlExpression exp1, bool isNot, SqlBuilder subquery)
					: base(exp1, isNot)
				{
					_subquery = subquery;
				}

				readonly SqlBuilder _subquery; public SqlBuilder Subquery { get { return _subquery; } }
			}

			public class InList : NotExprBase
			{
				public InList(ISqlExpression exp1, bool isNot, params ISqlExpression[] list)
					: base(exp1, isNot)
				{
					_list = list;
				}

				readonly ISqlExpression[] _list; public ISqlExpression[] List { get { return _list; } }
			}

			// CONTAINS ( { column | * } , '< contains_search_condition >' )
			// FREETEXT ( { column | * } , 'freetext_string' )
			// expression { = | <> | != | > | >= | !> | < | <= | !< } { ALL | SOME | ANY } ( subquery )
			// EXISTS ( subquery )

			public class FuncLike : Predicate
			{
				public FuncLike(SqlFunction func)
				{
					_func = func;
				}

				readonly SqlFunction _func; public SqlFunction Function { get { return _func; } }
			}
		}

		#endregion

		#region Condition

		public class Condition
		{
			public Condition(bool isNot, IPredicate predicate)
			{
				_isNot     = isNot;
				_predicate = predicate;
			}

			private bool       _isNot;     public bool       IsNot     { get { return _isNot;     } set { _isNot     = value; } }
			private IPredicate _predicate; public IPredicate Predicate { get { return _predicate; } set { _predicate = value; } }
			private bool       _isOr;      public bool       IsOr      { get { return _isOr;      } set { _isOr      = value; } }
		}

		#endregion

		#region SearchCondition

		public class SearchCondition : IPredicate
		{
			private List<Condition> _conditions = new List<Condition>();
			public  List<Condition>  Conditions
			{
				get { return _conditions; }
			}
		}

		#endregion

		#region ConditionBase

		interface IConditionExpr<T>
		{
			T Expr    (ISqlExpression expr);
			T Field   (Field          field);
			T SubQuery(SqlBuilder     sql);
			T Value   (object         value);
		}

		public abstract class ConditionBase<T1,T2> : IConditionExpr<ConditionBase<T1,T2>.Expr_>
			where T1 : ConditionBase<T1,T2>
			where T2 : T1
		{
			public class Expr_
			{
				internal Expr_(ConditionBase<T1,T2> condition, bool isNot, ISqlExpression expr)
				{
					_condition = condition;
					_isNot     = isNot;
					_expr      = expr;
				}

				ConditionBase<T1,T2> _condition;
				bool                 _isNot;
				ISqlExpression       _expr;

				T2 Add(IPredicate predicate)
				{
					_condition.Conditions.Conditions.Add(new Condition(_isNot, predicate));
					return (T2)_condition;
				}

				#region Predicate.ExprExpr

				public class Op_ : IConditionExpr<T2>
				{
					internal Op_(Expr_ expr, Predicate.Operator op) 
					{
						_expr = expr;
						_op   = op;
					}

					Expr_              _expr;
					Predicate.Operator _op;

					public T2 Expr    (ISqlExpression expr)  { return _expr.Add(new Predicate.ExprExpr(_expr._expr, _op, expr)); }
					public T2 Field   (Field          field) { return Expr(field);                    }
					public T2 SubQuery(SqlBuilder     sql)   { return Expr(sql);                      }
					public T2 Value   (object         value) { return Expr(new SqlValue(value));      }

					public T2 All     (SqlBuilder     sql)   { return Expr(new SqlFunction.All (sql)); }
					public T2 Some    (SqlBuilder     sql)   { return Expr(new SqlFunction.Some(sql)); }
					public T2 Any     (SqlBuilder     sql)   { return Expr(new SqlFunction.Any (sql)); }
				}

				public Op_ Equal          { get { return new Op_(this, Predicate.Operator.Equal);          } }
				public Op_ NotEqual       { get { return new Op_(this, Predicate.Operator.NotEqual);       } }
				public Op_ Greater        { get { return new Op_(this, Predicate.Operator.Greater);        } }
				public Op_ GreaterOrEqual { get { return new Op_(this, Predicate.Operator.GreaterOrEqual); } }
				public Op_ NotGreater     { get { return new Op_(this, Predicate.Operator.NotGreater);     } }
				public Op_ Less           { get { return new Op_(this, Predicate.Operator.Less);           } }
				public Op_ LessOrEqual    { get { return new Op_(this, Predicate.Operator.LessOrEqual);    } }
				public Op_ NotLess        { get { return new Op_(this, Predicate.Operator.NotLess);        } }

				#endregion

				#region Predicate.Like

				public T2 Like(ISqlExpression expression, char escape) { return Add(new Predicate.Like(_expr, false, expression, escape)); }
				public T2 Like(ISqlExpression expression)              { return Like(expression, '\x0'); }
				public T2 Like(string expression,         char escape) { return Like(new SqlValue(expression), escape); }
				public T2 Like(string expression)                      { return Like(new SqlValue(expression), '\x0'); }

				#endregion

				#region Predicate.Between

				public T2 Between   (ISqlExpression expr1, ISqlExpression expr2) { return Add(new Predicate.Between(_expr, false, expr1, expr2)); }
				public T2 NotBetween(ISqlExpression expr1, ISqlExpression expr2) { return Add(new Predicate.Between(_expr, true,  expr1, expr2)); }

				#endregion

				#region Predicate.IsNull

				public T2 IsNull    { get { return Add(new Predicate.IsNull(_expr, false)); } }
				public T2 IsNotNull { get { return Add(new Predicate.IsNull(_expr, true));  } }

				#endregion

				#region Predicate.In

				public T2 In   (SqlBuilder sql) { return Add(new Predicate.InSubquery(_expr, false, sql)); }
				public T2 NotIn(SqlBuilder sql) { return Add(new Predicate.InSubquery(_expr, true,  sql)); }

				static ISqlExpression[] _emptyList = new ISqlExpression[0];

				ISqlExpression[] Convert(object[] exprs)
				{
					if (exprs == null || exprs.Length == 0)
						return _emptyList;

					List<ISqlExpression> list = new List<ISqlExpression>(exprs.Length);

					foreach (object item in exprs)
					{
						if (item == null || item is SqlValue && ((SqlValue)item).Value == null)
							continue;

						if (item is ISqlExpression)
							list.Add((ISqlExpression)item);
						else
							list.Add(new SqlValue(item));
					}

					return list.ToArray();
				}

				public T2 In   (params object[] exprs) { return Add(new Predicate.InList(_expr, false, Convert(exprs))); }
				public T2 NotIn(params object[] exprs) { return Add(new Predicate.InList(_expr, true,  Convert(exprs))); }

				#endregion
			}

			public class Not_ : IConditionExpr<Expr_>
			{
				internal Not_(ConditionBase<T1,T2> condition)
				{
					_condition = condition;
				}

				ConditionBase<T1,T2> _condition;

				public Expr_ Expr    (ISqlExpression expr)  { return new Expr_(_condition, true, expr);  }
				public Expr_ Field   (Field          field) { return Expr(field);               }
				public Expr_ SubQuery(SqlBuilder     sql)   { return Expr(sql);                 }
				public Expr_ Value   (object         value) { return Expr(new SqlValue(value)); }

				public T2 Exists(SqlBuilder subQuery)
				{
					_condition.Conditions.Conditions.Add(new Condition(true, new Predicate.FuncLike(new SqlFunction.Exists(subQuery))));
					return (T2)_condition;
				}
			}

			protected abstract SearchCondition Conditions { get; }

			protected T1 SetOr(bool value)
			{
				Conditions.Conditions[Conditions.Conditions.Count - 1].IsOr = value;
				return (T1)this;
			}

			public Not_  Not { get { return new Not_(this); } }

			public Expr_ Expr    (ISqlExpression expr)  { return new Expr_(this, false, expr);  }
			public Expr_ Field   (Field          field) { return Expr(field);               }
			public Expr_ SubQuery(SqlBuilder     sql)   { return Expr(sql);                 }
			public Expr_ Value   (object         value) { return Expr(new SqlValue(value)); }

			public T2 Exists(SqlBuilder subQuery)
			{
				Conditions.Conditions.Add(new Condition(false, new Predicate.FuncLike(new SqlFunction.Exists(subQuery))));
				return (T2)this;
			}
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
			public WhereClause   Where   { get { return Builder.Where;   } }
			public GroupByClause GroupBy { get { return Builder.GroupBy; } }
			public HavingClause  Having  { get { return Builder.Having;  } }
			public OrderByClause OrderBy { get { return Builder.OrderBy; } }
			public SqlBuilder    End()   { return Builder; }

			readonly  SqlBuilder _builder;
			protected SqlBuilder  Builder
			{
				get { return _builder; }
			}
		}

		public abstract class ClauseBase<T1, T2> : ConditionBase<T1, T2>
			where T1 : ClauseBase<T1, T2>
			where T2 : T1
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

			readonly  SqlBuilder _builder;
			protected SqlBuilder  Builder
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
			#region FromTable

			public class Table_ : ClauseBase
			{
				#region Join

				public class Join_ : Table_
				{
					#region On

					public abstract class On_ : ConditionBase<On_, On_.Next>
					{
						public class Next : On_
						{
							internal Next(Join_ parent) : base(parent) {}

							public On_ Or  { get { return SetOr(true);  } }
							public On_ And { get { return SetOr(false); } }

							
						}

						private On_(Join_ parent)
						{
							_parent = parent;
						}

						Join_ _parent;

						protected override SearchCondition Conditions
						{
							get { return _parent._join.Condition; }
						}
					}

					#endregion

					internal Join_(Table_ parent, JoinedTable join)
						: base(parent.Builder, join.Table)
					{
						_parent = parent;
						_join   = join;
					}

					Table_      _parent;
					JoinedTable _join;

					public On_ On
					{
						get { return new On_.Next(this); }
					}
				}

				#endregion

				internal Table_(SqlBuilder builder, TableSource table)
					: base(builder)
				{
					_table = table;
				}

				private TableSource _table;

				public Table_ Table(ITableSource table)               { return Builder.From.Table(table);        }
				public Table_ Table(ITableSource table, string alias) { return Builder.From.Table(table, alias); }

				public Join_ InnerJoin (ITableSource table)               { return AddJoin(JoinType.Inner, table, null,  false); }
				public Join_ InnerJoin (ITableSource table, string alias) { return AddJoin(JoinType.Inner, table, alias, false); }
				public Join_ LeftJoin  (ITableSource table)               { return AddJoin(JoinType.Left,  table, null,  false); }
				public Join_ LeftJoin  (ITableSource table, string alias) { return AddJoin(JoinType.Left,  table, alias, false); }
				public Join_ Join      (ITableSource table)               { return AddJoin(JoinType.Auto,  table, null,  false); }
				public Join_ Join      (ITableSource table, string alias) { return AddJoin(JoinType.Auto,  table, alias, false); }

				Join_ AddJoin(JoinType joinType, ITableSource table, string alias, bool isWeak)
				{
					foreach (JoinedTable tj in _table.Joins)
					{
						TableSource ts = tj.Table;

						if (ts.Source == table)
						{
							if (alias != null && ts.Alias != alias) throw new ArgumentException("alias");
							if (tj.JoinType != joinType)            throw new ArgumentException("joinType");
							if (tj.IsWeak != isWeak)                throw new ArgumentException("isWeak");

							return new Join_(this, tj);
						}
					}

					if (Builder.From[table, alias] != null)
						throw new ArgumentException("alias");

					JoinedTable join = new JoinedTable(joinType, table, alias, isWeak);

					_table.Joins.Add(join);

					return new Join_(this, join);
				}
			}

			#endregion

			internal FromClause(SqlBuilder builder) : base(builder)
			{
			}

			public Table_ Table(ITableSource table)
			{
				return Table(table, null);
			}

			public Table_ Table(ITableSource table, string alias)
			{
				return new Table_(Builder, AddOrGetTable(table, alias));
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

			public TableSource this[ITableSource table]
			{
				get { return this[table, null]; }
			}

			public TableSource this[ITableSource table, string alias]
			{
				get
				{
					foreach (TableSource ts in Tables)
					{
						if (ts.Source == table && (alias == null || ts.Alias == alias))
							return ts;

						TableSource join = ts[table, alias];

						if (join != null)
							return join;
					}

					return null;
				}
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

		public abstract class WhereClause : ClauseBase<WhereClause, WhereClause.Next>
		{
			public class Next : WhereClause
			{
				internal Next(SqlBuilder builder) : base(builder) {}

				public WhereClause Or  { get { return SetOr(true);  } }
				public WhereClause And { get { return SetOr(false); } }
			}

			private WhereClause(SqlBuilder builder) : base(builder)
			{
			}

			private SearchCondition _searchCondition = new SearchCondition();
			public  SearchCondition  SearchCondition
			{
				get { return _searchCondition; }
			}

			protected override SearchCondition Conditions
			{
				get { return _searchCondition; }
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

		public class HavingClause : WhereClause.Next
		{
			internal HavingClause(SqlBuilder builder) : base(builder)
			{
			}
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
