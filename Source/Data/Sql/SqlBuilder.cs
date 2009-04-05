using System;
using System.Collections.Generic;

namespace BLToolkit.Data.Sql
{
	using FJoin = SqlBuilder.FromClause.Join;

	public class SqlBuilder : ISqlExpression, ISqlTableSource
	{
		#region Init

		public SqlBuilder()
		{
			_select  = new SelectClause (this);
			_from    = new FromClause   (this);
			_where   = new WhereClause  (this);
			_groupBy = new GroupByClause(this);
			_having  = new WhereClause  (this);
			_orderBy = new OrderByClause(this);
		}

		#endregion

		#region Column

		public class Column : IEquatable<Column>, ISqlExpression, IChild<ISqlTableSource>
		{
			public Column(SqlBuilder builder, ISqlExpression expression, string alias)
			{
				if (expression == null) throw new ArgumentNullException("expression");

				_parent     = builder;
				_expression = expression;
				_alias      = alias;
			}

			public Column(SqlBuilder builder, ISqlExpression expression)
				: this(builder, expression, null)
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
				get
				{
					if (_alias == null)
					{
						if (_expression is SqlField)
						{
							SqlField field = (SqlField)_expression;
							return field.Name ?? field.PhysicalName;
						}
					}

					return _alias;
				}
				set { _alias = value; }
			}

			public bool Equals(Column other)
			{
				return _alias == other._alias && _expression.Equals(other._expression);
			}

			#region IEquatable<ISqlExpression> Members

			bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
			{
				return Equals((Column)other);
			}

			#endregion

			#region ISqlExpressionScannable Members

			public void ForEach(Action<ISqlExpression> action)
			{
				action(this);
				_expression.ForEach(action);
			}

			#endregion

			#region IChild<ISqlTableSource> Members

			string IChild<ISqlTableSource>.Name
			{
				get { return Alias; }
			}

			private ISqlTableSource _parent;
			public  ISqlTableSource  Parent
			{
				get { return _parent;  }
				set { _parent = value; }
			}

			#endregion
		}

		#endregion

		#region TableSource

		public class TableSource : ISqlTableSource, ISqlExpressionScannable
		{
			public TableSource(ISqlTableSource source, string alias)
			{
				if (source == null) throw new ArgumentNullException("source");

				_source = source;
				_alias  = alias;
			}

			private ISqlTableSource _source;
			public  ISqlTableSource  Source
			{
				get { return _source;  }
				set { _source = value; }
			}

			private string _alias;
			public  string  Alias
			{
				get
				{
					if (string.IsNullOrEmpty(_alias))
					{
						if (_source is TableSource)
							return (_source as TableSource).Alias;

						if (_source is SqlTable)
							return ((SqlTable)_source).Alias;
					}

					return _alias;
				}
				set { _alias = value; }
			}

			public TableSource this[ISqlTableSource table]
			{
				get { return this[table, null]; }
			}

			public TableSource this[ISqlTableSource table, string alias]
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

			readonly List<JoinedTable> _joins = new List<JoinedTable>();
			public   List<JoinedTable>  Joins
			{
				get { return _joins;  }
			}

			public void ForEach(Action<TableSource> action)
			{
				action(this);
				foreach (JoinedTable join in Joins)
					join.ForEach(action);
			}

			#region ISqlExpressionScannable Members

			public void ForEach(Action<ISqlExpression> action)
			{
				if (Source is ISqlExpression)
					((ISqlExpression)Source).ForEach(action);

				foreach (JoinedTable join in Joins)
					((ISqlExpressionScannable)join).ForEach(action);
			}

			#endregion
		}

		#endregion

		#region TableJoin

		public enum JoinType
		{
			Auto,
			Inner,
			Left
		}

		public class JoinedTable : ISqlExpressionScannable
		{
			public JoinedTable(JoinType joinType, TableSource table, bool isWeak)
			{
				_joinType = joinType;
				_table    = table;
				_isWeak   = isWeak;
			}

			public JoinedTable(JoinType joinType, ISqlTableSource table, string alias, bool isWeak)
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

			readonly SearchCondition _condition = new SearchCondition();
			public   SearchCondition  Condition
			{
				get { return _condition;  }
			}

			private bool _isWeak;
			public  bool  IsWeak
			{
				get { return _isWeak;  }
				set { _isWeak = value; }
			}

			public void ForEach(Action<TableSource> action)
			{
				Table.ForEach(action);
			}

			#region ISqlExpressionScannable Members

			public void ForEach(Action<ISqlExpression> action)
			{
				((ISqlExpressionScannable)Condition).ForEach(action);
				((ISqlExpressionScannable)Table).    ForEach(action);
			}

			#endregion
		}

		#endregion

		#region Predicate

		public interface IPredicate : ISqlExpressionScannable
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
				protected ExprBase(ISqlExpression exp1)
				{
					_expr1 = exp1;
				}

				readonly ISqlExpression _expr1; public ISqlExpression Expr1 { get { return _expr1; } }

				protected override void ForEach(Action<ISqlExpression> action)
				{
					_expr1.ForEach(action);
				}
			}

			public abstract class NotExprBase : ExprBase
			{
				protected NotExprBase(ISqlExpression exp1, bool isNot)
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

				protected override void ForEach(Action<ISqlExpression> action)
				{
					base.ForEach(action);
					_expr2.ForEach(action);
				}
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

				protected override void ForEach(Action<ISqlExpression> action)
				{
					base.ForEach(action);
					_expr2.ForEach(action);
				}
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

				protected override void ForEach(Action<ISqlExpression> action)
				{
					base.ForEach(action);
					_expr2.ForEach(action);
					_expr3.ForEach(action);
				}
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
				public InSubquery(ISqlExpression exp1, bool isNot, SqlBuilder subQuery)
					: base(exp1, isNot)
				{
					_subQuery = subQuery;
				}

				readonly SqlBuilder _subQuery; public SqlBuilder SubQuery { get { return _subQuery; } }

				protected override void ForEach(Action<ISqlExpression> action)
				{
					base.ForEach(action);
					((ISqlExpression)_subQuery).ForEach(action);
				}
			}

			public class InList : NotExprBase
			{
				public InList(ISqlExpression exp1, bool isNot, params ISqlExpression[] values)
					: base(exp1, isNot)
				{
					if (values != null && values.Length > 0)
						_values.AddRange(values);
				}

				readonly List<ISqlExpression> _values = new List<ISqlExpression>();
				public   List<ISqlExpression>  Values { get { return _values; } }

				protected override void ForEach(Action<ISqlExpression> action)
				{
					base.ForEach(action);
					foreach (ISqlExpression expr in _values)
						expr.ForEach(action);
				}
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
		
				protected override void ForEach(Action<ISqlExpression> action)
				{
					((ISqlExpression)_func).ForEach(action);
				}
			}

			#region IPredicate Members

			protected abstract void ForEach(Action<ISqlExpression> action);

			void ISqlExpressionScannable.ForEach(Action<ISqlExpression> action)
			{
				ForEach(action);
			}

			#endregion
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
			readonly List<Condition> _conditions = new List<Condition>();
			public   List<Condition>  Conditions
			{
				get { return _conditions; }
			}

			#region IPredicate Members

			void ISqlExpressionScannable.ForEach(Action<ISqlExpression> action)
			{
				foreach (Condition condition in Conditions)
					condition.Predicate.ForEach(action);
			}

			#endregion
		}

		#endregion

		#region ConditionBase

		interface IConditionExpr<T>
		{
			T Expr    (ISqlExpression expr);
			T Field   (SqlField          field);
			T SubQuery(SqlBuilder     sqlBuilder);
			T Value   (object         value);
		}

		public abstract class ConditionBase<T1,T2> : IConditionExpr<ConditionBase<T1,T2>.Expr_>
			where T1 : ConditionBase<T1,T2>
		{
			public class Expr_
			{
				internal Expr_(ConditionBase<T1,T2> condition, bool isNot, ISqlExpression expr)
				{
					_condition = condition;
					_isNot     = isNot;
					_expr      = expr;
				}

				readonly ConditionBase<T1,T2> _condition;
				readonly bool                 _isNot;
				readonly ISqlExpression       _expr;

				T2 Add(IPredicate predicate)
				{
					_condition.Conditions.Conditions.Add(new Condition(_isNot, predicate));
					return _condition.GetNext();
				}

				#region Predicate.ExprExpr

				public class Op_ : IConditionExpr<T2>
				{
					internal Op_(Expr_ expr, Predicate.Operator op) 
					{
						_expr = expr;
						_op   = op;
					}

					readonly Expr_              _expr;
					readonly Predicate.Operator _op;

					public T2 Expr    (ISqlExpression expr)     { return _expr.Add(new Predicate.ExprExpr(_expr._expr, _op, expr)); }
					public T2 Field   (SqlField          field)    { return Expr(field);               }
					public T2 SubQuery(SqlBuilder     subQuery) { return Expr(subQuery);            }
					public T2 Value   (object         value)    { return Expr(new SqlValue(value)); }

					public T2 All     (SqlBuilder     subQuery) { return Expr(new SqlFunction.All (subQuery)); }
					public T2 Some    (SqlBuilder     subQuery) { return Expr(new SqlFunction.Some(subQuery)); }
					public T2 Any     (SqlBuilder     subQuery) { return Expr(new SqlFunction.Any (subQuery)); }
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

				public T2 In   (SqlBuilder subQuery) { return Add(new Predicate.InSubquery(_expr, false, subQuery)); }
				public T2 NotIn(SqlBuilder subQuery) { return Add(new Predicate.InSubquery(_expr, true,  subQuery)); }

				Predicate.InList CreateInList(bool isNot, object[] exprs)
				{
					Predicate.InList list = new Predicate.InList(_expr, isNot, null);

					if (exprs != null && exprs.Length > 0)
					{
						foreach (object item in exprs)
						{
							if (item == null || item is SqlValue && ((SqlValue)item).Value == null)
								continue;

							if (item is ISqlExpression)
								list.Values.Add((ISqlExpression)item);
							else
								list.Values.Add(new SqlValue(item));
						}
					}

					return list;
				}

				public T2 In   (params object[] exprs) { return Add(CreateInList(false, exprs)); }
				public T2 NotIn(params object[] exprs) { return Add(CreateInList(true,  exprs)); }

				#endregion
			}

			public class Not_ : IConditionExpr<Expr_>
			{
				internal Not_(ConditionBase<T1,T2> condition)
				{
					_condition = condition;
				}

				readonly ConditionBase<T1,T2> _condition;

				public Expr_ Expr    (ISqlExpression expr)     { return new Expr_(_condition, true, expr);  }
				public Expr_ Field   (SqlField          field)    { return Expr(field);               }
				public Expr_ SubQuery(SqlBuilder     subQuery) { return Expr(subQuery);            }
				public Expr_ Value   (object         value)    { return Expr(new SqlValue(value)); }

				public T2 Exists(SqlBuilder subQuery)
				{
					_condition.Conditions.Conditions.Add(new Condition(true, new Predicate.FuncLike(new SqlFunction.Exists(subQuery))));
					return _condition.GetNext();
				}
			}

			protected abstract SearchCondition Conditions { get; }
			protected abstract T2              GetNext();

			protected T1 SetOr(bool value)
			{
				Conditions.Conditions[Conditions.Conditions.Count - 1].IsOr = value;
				return (T1)this;
			}

			public Not_  Not { get { return new Not_(this); } }

			public Expr_ Expr    (ISqlExpression expr)     { return new Expr_(this, false, expr); }
			public Expr_ Field   (SqlField          field)    { return Expr(field);                  }
			public Expr_ SubQuery(SqlBuilder     subQuery) { return Expr(subQuery);               }
			public Expr_ Value   (object         value)    { return Expr(new SqlValue(value));    }

			public T2 Exists(SqlBuilder subQuery)
			{
				Conditions.Conditions.Add(new Condition(false, new Predicate.FuncLike(new SqlFunction.Exists(subQuery))));
				return GetNext();
			}
		}

		#endregion

		#region OrderByItem

		public class OrderByItem
		{
			public OrderByItem(ISqlExpression expression, bool isDescending)
			{
				_expression   = expression;
				_isDescending = isDescending;
			}

			readonly ISqlExpression _expression;   public ISqlExpression Expression   { get { return _expression;   } }
			readonly bool           _isDescending; public bool           IsDescending { get { return _isDescending; } }
		}

		#endregion

		#region ClauseBase

		public abstract class ClauseBase
		{
			protected ClauseBase(SqlBuilder sqlBuilder)
			{
				_sqlBuilder = sqlBuilder;
			}

			public SelectClause  Select  { get { return SqlBuilder.Select;  } }
			public FromClause    From    { get { return SqlBuilder.From;    } }
			public WhereClause   Where   { get { return SqlBuilder.Where;   } }
			public GroupByClause GroupBy { get { return SqlBuilder.GroupBy; } }
			public WhereClause   Having  { get { return SqlBuilder.Having;  } }
			public OrderByClause OrderBy { get { return SqlBuilder.OrderBy; } }
			public SqlBuilder    End()   { return SqlBuilder; }

			readonly  SqlBuilder _sqlBuilder;
			protected SqlBuilder  SqlBuilder
			{
				get { return _sqlBuilder; }
			}
		}

		public abstract class ClauseBase<T1, T2> : ConditionBase<T1, T2>
			where T1 : ClauseBase<T1, T2>
		{
			protected ClauseBase(SqlBuilder sqlBuilder)
			{
				_sqlBuilder = sqlBuilder;
			}

			public SelectClause  Select  { get { return SqlBuilder.Select;  } }
			public FromClause    From    { get { return SqlBuilder.From;    } }
			public GroupByClause GroupBy { get { return SqlBuilder.GroupBy; } }
			public WhereClause   Having  { get { return SqlBuilder.Having;  } }
			public OrderByClause OrderBy { get { return SqlBuilder.OrderBy; } }
			public SqlBuilder    End()   { return SqlBuilder; }

			readonly  SqlBuilder _sqlBuilder;
			protected SqlBuilder  SqlBuilder
			{
				get { return _sqlBuilder; }
			}
		}

		#endregion

		#region SelectClause

		public class SelectClause : ClauseBase, ISqlExpressionScannable
		{
			#region Init

			internal SelectClause(SqlBuilder sqlBuilder) : base(sqlBuilder)
			{
			}

			#endregion

			#region Columns

			public SelectClause Field(SqlField field)
			{
				AddOrGetColumn(new Column(SqlBuilder, field));
				return this;
			}

			public SelectClause Field(SqlField field, string alias)
			{
				AddOrGetColumn(new Column(SqlBuilder, field, alias));
				return this;
			}

			public SelectClause SubQuery(SqlBuilder subQuery)
			{
				AddOrGetColumn(new Column(SqlBuilder, subQuery));
				return this;
			}

			public SelectClause SubQuery(SqlBuilder sqlQuery, string alias)
			{
				AddOrGetColumn(new Column(SqlBuilder, sqlQuery, alias));
				return this;
			}

			public SelectClause Expr(ISqlExpression expr)
			{
				AddOrGetColumn(new Column(SqlBuilder, expr));
				return this;
			}

			public SelectClause Expr(ISqlExpression expr, string alias)
			{
				AddOrGetColumn(new Column(SqlBuilder, expr, alias));
				return this;
			}

			public SelectClause Expr(string expr, params ISqlExpression[] values)
			{
				AddOrGetColumn(new Column(SqlBuilder, new SqlExpression(expr, values)));
				return this;
			}

			public SelectClause Expr(string alias, string expr, params ISqlExpression[] values)
			{
				AddOrGetColumn(new Column(SqlBuilder, new SqlExpression(expr, values)));
				return this;
			}

			public int Add(ISqlExpression expr)
			{
				return Columns.IndexOf(AddOrGetColumn(new Column(SqlBuilder, expr)));
			}

			public int Add(ISqlExpression expr, string alias)
			{
				return Columns.IndexOf(AddOrGetColumn(new Column(SqlBuilder, expr, alias)));
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

			#endregion

			#region Distinct

			public SelectClause Distinct
			{
				get { _isDistinct = true; return this; }
			}

			private bool _isDistinct;
			private bool  IsDistinct { get { return _isDistinct; } set { _isDistinct = value; } }

			#endregion

			#region Take

			public SelectClause Take(int value)
			{
				_takeValue = value;
				return this;
			}

			private int _takeValue;
			private int  TakeValue { get { return _takeValue; } set { _takeValue = value; } }

			#endregion

			#region Skip

			public SelectClause Skip(int value)
			{
				_skipValue = value;
				return this;
			}

			private int _skipValue;
			private int  SkipeValue { get { return _skipValue; } set { _skipValue = value; } }

			#endregion

			#region ISqlExpressionScannable Members

			void ISqlExpressionScannable.ForEach(Action<ISqlExpression> action)
			{
				foreach (Column column in Columns)
					column.ForEach(action);
			}

			#endregion
		}

		readonly SelectClause _select;
		public   SelectClause  Select
		{
			get { return _select; }
		}

		#endregion

		#region FromClause

		public class FromClause : ClauseBase, ISqlExpressionScannable
		{
			#region Join

			public class Join : ConditionBase<Join,Join.Next>
			{
				public class Next
				{
					internal Next(Join parent)
					{
						_parent = parent;
					}

					readonly Join _parent;

					public Join Or  { get { return _parent.SetOr(true);  } }
					public Join And { get { return _parent.SetOr(false); } }

					public static implicit operator Join(Next next)
					{
						return next._parent;
					}
				}

				protected override SearchCondition Conditions
				{
					get { return _joinedTable.Condition; }
				}

				protected override Next GetNext()
				{
					return new Next(this);
				}

				internal Join(JoinType joinType, ISqlTableSource table, string alias, bool isWeak, Join[] joins)
				{
					_joinedTable = new JoinedTable(joinType, table, alias, isWeak);

					if (joins != null && joins.Length > 0)
						foreach (Join join in joins)
							_joinedTable.Table.Joins.Add(join._joinedTable);
				}

				readonly JoinedTable _joinedTable;
				internal JoinedTable  JoinedTable
				{
					get { return _joinedTable; }
				}
			}

			#endregion

			internal FromClause(SqlBuilder sqlBuilder) : base(sqlBuilder)
			{
			}

			public FromClause Table(ISqlTableSource table, params FJoin[] joins)
			{
				return Table(table, null, joins);
			}

			public FromClause Table(ISqlTableSource table, string alias, params FJoin[] joins)
			{
				TableSource ts = AddOrGetTable(table, alias);

				if (joins != null && joins.Length > 0)
					foreach (Join join in joins)
						ts.Joins.Add(join.JoinedTable);

				return this;
			}

			TableSource GetTable(ISqlTableSource table, string alias)
			{
				foreach (TableSource ts in Tables)
					if (ts.Source == table)
						if (alias == null || ts.Alias == alias)
							return ts;
						else
							throw new ArgumentException("alias");

				return null;
			}

			TableSource AddOrGetTable(ISqlTableSource table, string alias)
			{
				TableSource ts = GetTable(table, alias);

				if (ts != null)
					return ts;

				TableSource t = new TableSource(table, alias);

				Tables.Add(t);

				return t;
			}

			public TableSource this[ISqlTableSource table]
			{
				get { return this[table, null]; }
			}

			public TableSource this[ISqlTableSource table, string alias]
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

			readonly List<TableSource> _tables = new List<TableSource>();
			public   List<TableSource>  Tables
			{
				get { return _tables; }
			}

			#region ISqlExpressionScannable Members

			void ISqlExpressionScannable.ForEach(Action<ISqlExpression> action)
			{
				foreach (TableSource table in Tables)
					((ISqlExpressionScannable)table).ForEach(action);
			}

			#endregion
		}

		public static FJoin InnerJoin    (ISqlTableSource table,               params FJoin[] joins) { return new FJoin(JoinType.Inner, table, null,  false, joins); }
		public static FJoin InnerJoin    (ISqlTableSource table, string alias, params FJoin[] joins) { return new FJoin(JoinType.Inner, table, alias, false, joins); }
		public static FJoin LeftJoin     (ISqlTableSource table,               params FJoin[] joins) { return new FJoin(JoinType.Left,  table, null,  false, joins); }
		public static FJoin LeftJoin     (ISqlTableSource table, string alias, params FJoin[] joins) { return new FJoin(JoinType.Left,  table, alias, false, joins); }
		public static FJoin Join         (ISqlTableSource table,               params FJoin[] joins) { return new FJoin(JoinType.Auto,  table, null,  false, joins); }
		public static FJoin Join         (ISqlTableSource table, string alias, params FJoin[] joins) { return new FJoin(JoinType.Auto,  table, alias, false, joins); }

		public static FJoin WeakInnerJoin(ISqlTableSource table,               params FJoin[] joins) { return new FJoin(JoinType.Inner, table, null,  true,  joins); }
		public static FJoin WeakInnerJoin(ISqlTableSource table, string alias, params FJoin[] joins) { return new FJoin(JoinType.Inner, table, alias, true,  joins); }
		public static FJoin WeakLeftJoin (ISqlTableSource table,               params FJoin[] joins) { return new FJoin(JoinType.Left,  table, null,  true,  joins); }
		public static FJoin WeakLeftJoin (ISqlTableSource table, string alias, params FJoin[] joins) { return new FJoin(JoinType.Left,  table, alias, true,  joins); }
		public static FJoin WeakJoin     (ISqlTableSource table,               params FJoin[] joins) { return new FJoin(JoinType.Auto,  table, null,  true,  joins); }
		public static FJoin WeakJoin     (ISqlTableSource table, string alias, params FJoin[] joins) { return new FJoin(JoinType.Auto,  table, alias, true,  joins); }

		readonly FromClause _from;
		public   FromClause  From
		{
			get { return _from; }
		}

		#endregion

		#region WhereClause

		public class WhereClause : ClauseBase<WhereClause, WhereClause.Next>, ISqlExpressionScannable
		{
			public class Next : ClauseBase
			{
				internal Next(WhereClause parent) : base(parent.SqlBuilder)
				{
					_parent = parent;
				}

				readonly WhereClause _parent;

				public WhereClause Or  { get { return _parent.SetOr(true);  } }
				public WhereClause And { get { return _parent.SetOr(false); } }
			}

			internal WhereClause(SqlBuilder sqlBuilder) : base(sqlBuilder)
			{
			}

			readonly SearchCondition _searchCondition = new SearchCondition();
			public   SearchCondition  SearchCondition
			{
				get { return _searchCondition; }
			}

			protected override SearchCondition Conditions
			{
				get { return _searchCondition; }
			}

			protected override Next GetNext()
			{
				return new Next(this);
			}

			#region ISqlExpressionScannable Members

			void ISqlExpressionScannable.ForEach(Action<ISqlExpression> action)
			{
				((ISqlExpressionScannable)SearchCondition).ForEach(action);
			}

			#endregion
		}

		readonly WhereClause _where;
		public   WhereClause  Where
		{
			get { return _where; }
		}

		#endregion

		#region GroupByClause

		public class GroupByClause : ClauseBase, ISqlExpressionScannable
		{
			internal GroupByClause(SqlBuilder sqlBuilder) : base(sqlBuilder)
			{
			}

			public GroupByClause Expr(ISqlExpression expr)
			{
				Add(expr);
				return this;
			}

			public GroupByClause Field(SqlField field)
			{
				return Expr(field);
			}

			void Add(ISqlExpression expr)
			{
				foreach (ISqlExpression e in Items)
					if (e.Equals(expr))
						return;

				Items.Add(expr);
			}

			readonly List<ISqlExpression> _items = new List<ISqlExpression>();
			public   List<ISqlExpression>  Items
			{
				get { return _items; }
			}

			#region ISqlExpressionScannable Members

			void ISqlExpressionScannable.ForEach(Action<ISqlExpression> action)
			{
				foreach (ISqlExpression item in Items)
					item.ForEach(action);
			}

			#endregion
		}

		readonly GroupByClause _groupBy;
		public   GroupByClause  GroupBy
		{
			get { return _groupBy; }
		}

		#endregion

		#region HavingClause

		readonly WhereClause _having;
		public   WhereClause  Having
		{
			get { return _having; }
		}

		#endregion

		#region OrderByClause

		public class OrderByClause : ClauseBase, ISqlExpressionScannable
		{
			internal OrderByClause(SqlBuilder sqlBuilder) : base(sqlBuilder)
			{
			}

			public OrderByClause Expr(ISqlExpression expr, bool isDescending)
			{
				Add(expr, isDescending);
				return this;
			}

			public OrderByClause Expr     (ISqlExpression expr)            { return Expr(expr,  false);        }
			public OrderByClause ExprAsc  (ISqlExpression expr)            { return Expr(expr,  false);        }
			public OrderByClause ExprDesc (ISqlExpression expr)            { return Expr(expr,  true);         }
			public OrderByClause Field    (SqlField field, bool isDescending) { return Expr(field, isDescending); }
			public OrderByClause Field    (SqlField field)                    { return Expr(field, false);        }
			public OrderByClause FieldAsc (SqlField field)                    { return Expr(field, false);        }
			public OrderByClause FieldDesc(SqlField field)                    { return Expr(field, true);         }

			void Add(ISqlExpression expr, bool isDescending)
			{
				foreach (OrderByItem item in Items)
					if (item.Expression.Equals(expr))
						return;

				Items.Add(new OrderByItem(expr, isDescending));
			}

			readonly List<OrderByItem> _items = new List<OrderByItem>();
			public   List<OrderByItem>  Items
			{
				get { return _items; }
			}

			#region ISqlExpressionScannable Members

			void ISqlExpressionScannable.ForEach(Action<ISqlExpression> action)
			{
				foreach (OrderByItem item in Items)
					item.Expression.ForEach(action);
			}

			#endregion
		}

		readonly OrderByClause _orderBy;
		public   OrderByClause  OrderBy
		{
			get { return _orderBy; }
		}

		#endregion

		#region FinalizeAndValidate

		public void FinalizeAndValidate()
		{
			((ISqlExpressionScannable)this).ForEach(delegate(ISqlExpression expr)
			{
				SqlBuilder sb = expr as SqlBuilder;

				if (sb != null && sb != this)
					sb.FinalizeAndValidate();
			});

			ResolveWeakJoins();
			SetAliases();
		}

		void ForEachTable(Action<TableSource> action)
		{
			From.Tables.ForEach(delegate(TableSource tbl) { tbl.ForEach(action); });
		}

		delegate bool FindTableSource(TableSource table);

		void ResolveWeakJoins()
		{
			List<ISqlTableSource> tables = null;

			FindTableSource findTable = null; findTable = delegate(TableSource table)
			{
				if (tables.Contains(table.Source))
					return true;

				foreach (JoinedTable join in table.Joins)
					if (findTable(join.Table))
					{
						join.IsWeak = false;
						return true;
					}

				return false;
			};

			ForEachTable(delegate(TableSource table)
			{
				for (int i = 0; i < table.Joins.Count; i++)
				{
					JoinedTable join = table.Joins[i];

					if (join.IsWeak)
					{
						if (tables == null)
						{
							tables = new List<ISqlTableSource>();

							Action<ISqlExpression> tableCollector = delegate(ISqlExpression expr)
							{
								SqlField field = expr as SqlField;

								if (field != null && !tables.Contains(field.Table))
									tables.Add(field.Table);
							};

							((ISqlExpressionScannable)Select) .ForEach(tableCollector);
							((ISqlExpressionScannable)Where)  .ForEach(tableCollector);
							((ISqlExpressionScannable)GroupBy).ForEach(tableCollector);
							((ISqlExpressionScannable)Having) .ForEach(tableCollector);
							((ISqlExpressionScannable)OrderBy).ForEach(tableCollector);
						}

						if (findTable(join.Table))
						{
							join.IsWeak = false;
						}
						else
						{
							table.Joins.RemoveAt(i);
							i--;
							continue;
						}
					}
				}
			});
		}

		void SetAliases()
		{
			{
				Dictionary<string,ISqlTableSource> aliases = new Dictionary<string,ISqlTableSource>();

				ForEachTable(delegate(TableSource table)
				{
					string alias = table.Alias;

					if (string.IsNullOrEmpty(alias))
					{
						table.Alias = "t";
						alias = "t1";
					}

					for (int i = 2; aliases.ContainsKey(alias); i++)
						alias = table.Alias + i;

					aliases.Add(table.Alias = alias, table);
				});
			}
			{
				Dictionary<string, Column> aliases = new Dictionary<string,Column>();

				foreach (Column c in Select.Columns)
				{
					string alias = c.Alias;

					if (alias == "*")
						continue;

					if (string.IsNullOrEmpty(alias))
						alias = "column1";

					for (int i = 2; aliases.ContainsKey(alias); i++)
						alias = "column" + i;

					aliases.Add(c.Alias = alias, c);
				}
			}
		}

		#endregion

		#region ISqlExpressionScannable Members

		void ISqlExpressionScannable.ForEach(Action<ISqlExpression> action)
		{
			action(this);
			((ISqlExpressionScannable)Select) .ForEach(action);
			((ISqlExpressionScannable)From)   .ForEach(action);
			((ISqlExpressionScannable)Where)  .ForEach(action);
			((ISqlExpressionScannable)GroupBy).ForEach(action);
			((ISqlExpressionScannable)Having) .ForEach(action);
			((ISqlExpressionScannable)OrderBy).ForEach(action);
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
