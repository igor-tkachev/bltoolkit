using System;
using System.Collections.Generic;
using System.Text;

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

		readonly List<SqlParameter> _parameters = new List<SqlParameter>();
		public   List<SqlParameter>  Parameters
		{
			get { return _parameters; }
		}

		private bool _parameterDependent;
		public  bool  ParameterDependent
		{
			get { return _parameterDependent;  }
			set { _parameterDependent = value; }
		}

		#endregion

		#region Column

		public class Column : IEquatable<Column>, ISqlExpression, IChild<ISqlTableSource>
		{
			public Column(ISqlTableSource builder, ISqlExpression expression, string alias)
			{
				if (expression == null) throw new ArgumentNullException("expression");

				_parent     = builder;
				_expression = expression;
				_alias      = alias;
			}

			public Column(ISqlTableSource builder, ISqlExpression expression)
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

						if (_expression is Column)
						{
							Column col = (Column)_expression;
							return col.Alias;
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

			public override string ToString()
			{
				return RemoveAlias(Expression) + " as " + (Alias ?? "field");
			}

			#region ISqlExpression Members

			public int Precedence
			{
				get { return Sql.Precedence.Primary; } // _expression.Precedence; }
			}

			#endregion

			#region IEquatable<ISqlExpression> Members

			bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
			{
				if (this == other)
					return true;

				return Equals((Column)other);
			}

			#endregion

			#region ISqlExpressionWalkable Members

			public ISqlExpression Walk(bool skipColumns, WalkingFunc func)
			{
				if (!(skipColumns && _expression is Column))
					_expression = _expression.Walk(skipColumns, func);

				return func(this);
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

		public class TableSource : ISqlTableSource, ISqlExpressionWalkable
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

				if (Source is SqlBuilder)
					((SqlBuilder)Source).ForEachTable(action);
			}

			public override string ToString()
			{
				string s = Source is SqlBuilder ?
					"(\n\t\t" + Source.ToString().Replace("\n", "\n\t\t") + "\n\t)" :
					RemoveAlias(Source);

				return s + " as " + (Alias ?? "[table]");
			}

			#region ISqlExpressionWalkable Members

			public ISqlExpression Walk(bool skipColumns, WalkingFunc func)
			{
				if (_source is ISqlExpression)
					_source = (ISqlTableSource)((ISqlExpression)_source).Walk(skipColumns, func);

				for (int i = 0; i < Joins.Count; i++)
					((ISqlExpressionWalkable)Joins[i]).Walk(skipColumns, func);

				return null;
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

		public class JoinedTable : ISqlExpressionWalkable
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

			public void ForEach(Action<TableSource> action)
			{
				Table.ForEach(action);
			}

			#region ISqlExpressionWalkable Members

			public ISqlExpression Walk(bool skipColumns, WalkingFunc action)
			{
				_condition = (SearchCondition)((ISqlExpressionWalkable)_condition).Walk(skipColumns, action);

				_table.Walk(skipColumns, action);

				return null;
			}

			#endregion
		}

		#endregion

		#region Predicate

		public abstract class Predicate : ISqlPredicate
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

			public class Expr : Predicate
			{
				public Expr(ISqlExpression exp1, int precedence)
					: base(precedence)
				{
					_expr1 = exp1;
				}

				ISqlExpression _expr1; public ISqlExpression Expr1 { get { return _expr1; } }

				protected override void Walk(bool skipColumns, WalkingFunc func)
				{
					_expr1 = _expr1.Walk(skipColumns, func);
				}
			}

			public class NotExpr : Expr
			{
				public NotExpr(ISqlExpression exp1, bool isNot, int precedence)
					: base(exp1, precedence)
				{
					_isNot = isNot;
				}

				bool _isNot; public bool IsNot { get { return _isNot; } set { _isNot = value; } }
			}

			// { expression { = | <> | != | > | >= | ! > | < | <= | !< } expression
			//
			public class ExprExpr : Expr
			{
				public ExprExpr(ISqlExpression exp1, Operator op, ISqlExpression exp2)
					: base(exp1, Sql.Precedence.Comparison)
				{
					_op    = op;
					_expr2 = exp2;
				}

				readonly Operator       _op;    public new Operator   Operator { get { return _op;    } }
				private  ISqlExpression _expr2; public ISqlExpression Expr2    { get { return _expr2; } }

				protected override void Walk(bool skipColumns, WalkingFunc func)
				{
					base.Walk(skipColumns, func);
					_expr2 = _expr2.Walk(skipColumns, func);
				}

				#region Overrides

				public override string ToString()
				{
					string op;

					switch (_op)
					{
						case Operator.Equal         : op = "=";  break;
						case Operator.NotEqual      : op = "<>"; break;
						case Operator.Greater       : op = ">";  break;
						case Operator.GreaterOrEqual: op = ">="; break;
						case Operator.NotGreater    : op = "!>"; break;
						case Operator.Less          : op = "<";  break;
						case Operator.LessOrEqual   : op = "<="; break;
						case Operator.NotLess       : op = "!<"; break;
						default: throw new InvalidOperationException();
					}

					return Expr1 + " " + op + " " + Expr2;
				}

				#endregion
			}

			// string_expression [ NOT ] LIKE string_expression [ ESCAPE 'escape_character' ]
			//
			public class Like : NotExpr
			{
				public Like(ISqlExpression exp1, bool isNot, ISqlExpression exp2, ISqlExpression escape)
					: base(exp1, isNot, Sql.Precedence.Comparison)
				{
					_expr2  = exp2;
					_escape = escape;
				}

				ISqlExpression _expr2;  public ISqlExpression Expr2  { get { return _expr2;  } }
				ISqlExpression _escape; public ISqlExpression Escape { get { return _escape; } }

				protected override void Walk(bool skipColumns, WalkingFunc func)
				{
					base.Walk(skipColumns, func);
					_expr2 = _expr2.Walk(skipColumns, func);

					if (_escape != null)
						_escape = _escape.Walk(skipColumns, func);
				}
			}

			// expression [ NOT ] BETWEEN expression AND expression
			//
			public class Between : NotExpr
			{
				public Between(ISqlExpression exp1, bool isNot, ISqlExpression exp2, ISqlExpression exp3)
					: base(exp1, isNot, Sql.Precedence.Comparison)
				{
					_expr2 = exp2;
					_expr3 = exp3;
				}

				ISqlExpression _expr2; public ISqlExpression Expr2 { get { return _expr2; } }
				ISqlExpression _expr3; public ISqlExpression Expr3 { get { return _expr3; } }

				protected override void Walk(bool skipColumns, WalkingFunc func)
				{
					base.Walk(skipColumns, func);
					_expr2 = _expr2.Walk(skipColumns, func);
					_expr3 = _expr3.Walk(skipColumns, func);
				}
			}

			// expression IS [ NOT ] NULL
			//
			public class IsNull : NotExpr
			{
				public IsNull(ISqlExpression exp1, bool isNot)
					: base(exp1, isNot, Sql.Precedence.Comparison)
				{
				}
			}

			// expression [ NOT ] IN ( subquery | expression [ ,...n ] )
			//
			public class InSubquery : NotExpr
			{
				public InSubquery(ISqlExpression exp1, bool isNot, SqlBuilder subQuery)
					: base(exp1, isNot, Sql.Precedence.Comparison)
				{
					_subQuery = subQuery;
				}

				SqlBuilder _subQuery; public SqlBuilder SubQuery { get { return _subQuery; } }

				protected override void Walk(bool skipColumns, WalkingFunc func)
				{
					base.Walk(skipColumns, func);
					_subQuery = (SqlBuilder)((ISqlExpression)_subQuery).Walk(skipColumns, func);
				}
			}

			public class InList : NotExpr
			{
				public InList(ISqlExpression exp1, bool isNot, params ISqlExpression[] values)
					: base(exp1, isNot, Sql.Precedence.Comparison)
				{
					if (values != null && values.Length > 0)
						_values.AddRange(values);
				}

				readonly List<ISqlExpression> _values = new List<ISqlExpression>();
				public   List<ISqlExpression>  Values { get { return _values; } }

				protected override void Walk(bool skipColumns, WalkingFunc action)
				{
					base.Walk(skipColumns, action);
					for (int i = 0; i < _values.Count; i++)
						_values[i] = _values[i].Walk(skipColumns, action);
				}
			}

			// CONTAINS ( { column | * } , '< contains_search_condition >' )
			// FREETEXT ( { column | * } , 'freetext_string' )
			// expression { = | <> | != | > | >= | !> | < | <= | !< } { ALL | SOME | ANY } ( subquery )
			// EXISTS ( subquery )

			public class FuncLike : Predicate
			{
				public FuncLike(SqlFunction func)
					: base(func.Precedence)
				{
					_func = func;
				}

				SqlFunction _func; public SqlFunction Function { get { return _func; } }
		
				protected override void Walk(bool skipColumns, WalkingFunc func)
				{
					_func = (SqlFunction)((ISqlExpression)_func).Walk(skipColumns, func);
				}
			}

			protected Predicate(int precedence)
			{
				_precedence = precedence;
			}

			#region IPredicate Members

			readonly int _precedence;
			public   int  Precedence
			{
				get { return _precedence;  }
			}

			protected abstract void Walk(bool skipColumns, WalkingFunc action);

			ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc func)
			{
				Walk(skipColumns, func);
				return null;
			}

			#endregion
		}

		#endregion

		#region Condition

		public class Condition
		{
			public Condition(bool isNot, ISqlPredicate predicate)
			{
				_isNot     = isNot;
				_predicate = predicate;
			}

			private bool          _isNot;     public bool          IsNot     { get { return _isNot;     } set { _isNot     = value; } }
			private ISqlPredicate _predicate; public ISqlPredicate Predicate { get { return _predicate; } set { _predicate = value; } }
			private bool          _isOr;      public bool          IsOr      { get { return _isOr;      } set { _isOr      = value; } }

			public int Precedence
			{
				get
				{
					return
						_isNot ? Sql.Precedence.LogicalNegation :
						_isOr  ? Sql.Precedence.LogicalDisjunction :
						         Sql.Precedence.LogicalConjunction;
				}
			}

			public override string ToString()
			{
				return "(" + (IsNot ? "NOT " : "") + Predicate + ")" + (IsOr ? " OR " : " AND ");
			}
		}

		#endregion

		#region SearchCondition

		public class SearchCondition : ConditionBase<SearchCondition, SearchCondition.Next>, ISqlPredicate, ISqlExpression
		{
			public class Next
			{
				internal Next(SearchCondition parent)
				{
					_parent = parent;
				}

				readonly SearchCondition _parent;

				public SearchCondition Or  { get { return _parent.SetOr(true);  } }
				public SearchCondition And { get { return _parent.SetOr(false); } }

				public ISqlExpression  ToExpr() { return _parent; }
			}

			readonly List<Condition> _conditions = new List<Condition>();
			public   List<Condition>  Conditions
			{
				get { return _conditions; }
			}

			protected override SearchCondition Search
			{
				get { return this; }
			}

			protected override Next GetNext()
			{
				return new Next(this);
			}

			#region Overrides

			public override string ToString()
			{
				StringBuilder sb = new StringBuilder();

				foreach (Condition c in Conditions)
					sb.Append(c);

				return sb.Remove(sb.Length - 4, 4).ToString();
			}

			#endregion

			#region IPredicate Members

			public int Precedence
			{
				get
				{
					if (_conditions.Count == 0) return Sql.Precedence.Unknown;
					if (_conditions.Count == 1) return _conditions[0].Precedence;
					return Math.Min(_conditions[0].Precedence, _conditions[_conditions.Count-1].Precedence);
				}
			}

			ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc func)
			{
				foreach (Condition condition in Conditions)
					condition.Predicate.Walk(skipColumns, func);

				return func(this);
			}

			#endregion

			#region IEquatable<ISqlExpression> Members

			bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
			{
				return this == other;
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

				T2 Add(ISqlPredicate predicate)
				{
					_condition.Search.Conditions.Add(new Condition(_isNot, predicate));
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
					public T2 Field   (SqlField       field)    { return Expr(field);               }
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

				public T2 Like(ISqlExpression expression, SqlValue escape) { return Add(new Predicate.Like(_expr, false, expression, escape)); }
				public T2 Like(ISqlExpression expression)                  { return Like(expression, null); }
				public T2 Like(string expression,         SqlValue escape) { return Like(new SqlValue(expression), escape); }
				public T2 Like(string expression)                          { return Like(new SqlValue(expression), null);   }

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
				public Expr_ Field   (SqlField       field)    { return Expr(field);               }
				public Expr_ SubQuery(SqlBuilder     subQuery) { return Expr(subQuery);            }
				public Expr_ Value   (object         value)    { return Expr(new SqlValue(value)); }

				public T2 Exists(SqlBuilder subQuery)
				{
					_condition.Search.Conditions.Add(new Condition(true, new Predicate.FuncLike(new SqlFunction.Exists(subQuery))));
					return _condition.GetNext();
				}
			}

			protected abstract SearchCondition Search { get; }
			protected abstract T2              GetNext();

			protected T1 SetOr(bool value)
			{
				Search.Conditions[Search.Conditions.Count - 1].IsOr = value;
				return (T1)this;
			}

			public Not_  Not { get { return new Not_(this); } }

			public Expr_ Expr    (ISqlExpression expr)     { return new Expr_(this, false, expr); }
			public Expr_ Field   (SqlField       field)    { return Expr(field);                  }
			public Expr_ SubQuery(SqlBuilder     subQuery) { return Expr(subQuery);               }
			public Expr_ Value   (object         value)    { return Expr(new SqlValue(value));    }

			public T2 Exists(SqlBuilder subQuery)
			{
				Search.Conditions.Add(new Condition(false, new Predicate.FuncLike(new SqlFunction.Exists(subQuery))));
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

			private  ISqlExpression _expression;   public ISqlExpression Expression   { get { return _expression;   } }
			readonly bool           _isDescending; public bool           IsDescending { get { return _isDescending; } }

			internal void Walk(bool skipColumns, WalkingFunc func)
			{
				_expression = _expression.Walk(skipColumns, func);
			}
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

		public class SelectClause : ClauseBase, ISqlExpressionWalkable
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

			public SelectClause Expr(string expr, int priority, params ISqlExpression[] values)
			{
				AddOrGetColumn(new Column(SqlBuilder, new SqlExpression(expr, priority, values)));
				return this;
			}

			public SelectClause Expr(string alias, string expr, int priority, params ISqlExpression[] values)
			{
				AddOrGetColumn(new Column(SqlBuilder, new SqlExpression(expr, priority, values)));
				return this;
			}

			public SelectClause Expr<T>(ISqlExpression expr1, string operation, ISqlExpression expr2)
			{
				AddOrGetColumn(new Column(SqlBuilder, new SqlBinaryExpression(expr1, operation, expr2, typeof(T))));
				return this;
			}

			public SelectClause Expr<T>(ISqlExpression expr1, string operation, ISqlExpression expr2, int priority)
			{
				AddOrGetColumn(new Column(SqlBuilder, new SqlBinaryExpression(expr1, operation, expr2, typeof(T), priority)));
				return this;
			}

			public SelectClause Expr<T>(string alias, ISqlExpression expr1, string operation, ISqlExpression expr2, int priority)
			{
				AddOrGetColumn(new Column(SqlBuilder, new SqlBinaryExpression(expr1, operation, expr2, typeof(T), priority), alias));
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
			public  bool  IsDistinct { get { return _isDistinct; } set { _isDistinct = value; } }

			#endregion

			#region Take

			public SelectClause Take(int value)
			{
				_takeValue = value;
				return this;
			}

			private int _takeValue;
			public  int  TakeValue { get { return _takeValue; } set { _takeValue = value; } }

			#endregion

			#region Skip

			public SelectClause Skip(int value)
			{
				_skipValue = value;
				return this;
			}

			private int _skipValue;
			public  int  SkipValue { get { return _skipValue; } set { _skipValue = value; } }

			#endregion

			#region Overrides

			public override string ToString()
			{
				StringBuilder sb = new StringBuilder("SELECT \n");

				foreach (Column c in Columns)
					sb.Append("\t").Append(c.ToString()).Append(", \n");

				return sb.Remove(sb.Length - 3, 3).ToString();
			}

			#endregion

			#region ISqlExpressionWalkable Members

			ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc func)
			{
				for (int i = 0; i < Columns.Count; i++)
				{
					Column         col  = Columns[i];
					ISqlExpression expr = col.Walk(skipColumns, func);

					if (expr is Column)
						Columns[i] = (Column)expr;
					else
						Columns[i] = new Column(col.Parent, expr, col.Alias);
				}

				return null;
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

		public class FromClause : ClauseBase, ISqlExpressionWalkable
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

				protected override SearchCondition Search
				{
					get { return _joinedTable.Condition; }
				}

				protected override Next GetNext()
				{
					return new Next(this);
				}

				internal Join(JoinType joinType, ISqlTableSource table, string alias, bool isWeak, ICollection<Join> joins)
				{
					_joinedTable = new JoinedTable(joinType, table, alias, isWeak);

					if (joins != null && joins.Count > 0)
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

						if (ts.Source is SqlBuilder)
						{
							TableSource s = ((SqlBuilder)ts.Source).From[table, alias];

							if (s != null)
								return s;
						}
					}

					return null;
				}
			}

			readonly List<TableSource> _tables = new List<TableSource>();
			public   List<TableSource>  Tables
			{
				get { return _tables; }
			}

			#region Overrides

			public override string ToString()
			{
				StringBuilder sb = new StringBuilder(" \nFROM \n\t");

				if (Tables.Count > 0)
				{
					foreach (TableSource ts in Tables)
						sb.Append(ts.ToString()).Append(", ");

					sb.Remove(sb.Length - 2, 2);
				}

				return sb.ToString();
			}

			#endregion

			#region ISqlExpressionWalkable Members

			ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc func)
			{
				for (int i = 0; i <	Tables.Count; i++)
					((ISqlExpressionWalkable)Tables[i]).Walk(skipColumns, func);

				return null;
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

		public class WhereClause : ClauseBase<WhereClause, WhereClause.Next>, ISqlExpressionWalkable
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

			private SearchCondition _searchCondition = new SearchCondition();
			public  SearchCondition  SearchCondition
			{
				get { return _searchCondition; }
			}

			public bool IsEmpty
			{
				get { return SearchCondition.Conditions.Count == 0; }
			}

			protected override SearchCondition Search
			{
				get { return _searchCondition; }
			}

			protected override Next GetNext()
			{
				return new Next(this);
			}

			public override string ToString()
			{
				return Search.Conditions.Count == 0 ? "" : "\nWHERE\n\t" + Search;
			}

			#region ISqlExpressionWalkable Members

			ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc action)
			{
				_searchCondition = (SearchCondition)((ISqlExpressionWalkable)_searchCondition).Walk(skipColumns, action);
				return null;
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

		public class GroupByClause : ClauseBase, ISqlExpressionWalkable
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

			public bool IsEmpty
			{
				get { return Items.Count == 0; }
			}

			public override string ToString()
			{
				if (Items.Count == 0)
					return "";

				StringBuilder sb = new StringBuilder(" \nGROUP BY \n");

				foreach (ISqlExpression item in Items)
					sb.Append(item.ToString()).Append(",");

				return sb.Remove(sb.Length - 1, 1).ToString();
			}

			#region ISqlExpressionWalkable Members

			ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc func)
			{
				for (int i = 0; i < Items.Count; i++)
					Items[i] = Items[i].Walk(skipColumns, func);

				return null;
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

		public class OrderByClause : ClauseBase, ISqlExpressionWalkable
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

			public bool IsEmpty
			{
				get { return Items.Count == 0; }
			}

			public override string ToString()
			{
				if (Items.Count == 0)
					return "";

				return base.ToString();
			}

			#region ISqlExpressionWalkable Members

			ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc func)
			{
				for (int i = 0; i < Items.Count; i++)
					Items[i].Walk(skipColumns, func);

				return null;
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
			((ISqlExpressionWalkable)this).Walk(false, delegate(ISqlExpression expr)
			{
				SqlBuilder sb = expr as SqlBuilder;

				if (sb != null && sb != this)
					sb.FinalizeAndValidate();

				return expr;
			});

			ResolveWeakJoins();
			OptimizeSubQueries();
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

							WalkingFunc tableCollector = delegate(ISqlExpression expr)
							{
								SqlField field = expr as SqlField;

								if (field != null && !tables.Contains(field.Table))
									tables.Add(field.Table);

								return expr;
							};

							((ISqlExpressionWalkable)Select) .Walk(false, tableCollector);
							((ISqlExpressionWalkable)Where)  .Walk(false, tableCollector);
							((ISqlExpressionWalkable)GroupBy).Walk(false, tableCollector);
							((ISqlExpressionWalkable)Having) .Walk(false, tableCollector);
							((ISqlExpressionWalkable)OrderBy).Walk(false, tableCollector);
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

		TableSource OptimizeSubQuery(TableSource source)
		{
			foreach (JoinedTable jt in source.Joins)
				OptimizeSubQuery(jt.Table);

			if (source.Source is SqlBuilder)
			{
				SqlBuilder builder = (SqlBuilder)source.Source;

				if (builder.From.Tables.Count == 1 &&
				    builder.From.Tables[0].Joins.Count == 0 &&
				    builder.GroupBy.IsEmpty &&
				    builder.OrderBy.IsEmpty &&
				   !builder.Select.Columns.Exists(delegate(Column c) { return !(c.Expression is SqlField); }))
				{
					Dictionary<ISqlExpression,SqlField> map = new Dictionary<ISqlExpression,SqlField>(builder.Select.Columns.Count);

					foreach (Column c in builder.Select.Columns)
						map.Add(c, (SqlField)c.Expression);

					((ISqlExpressionWalkable)this).Walk(false, delegate(ISqlExpression expr)
					{
						SqlField fld;
						return map.TryGetValue(expr, out fld)? fld: expr;
					});

					if (!builder.Where. IsEmpty) ConcatSearchCondition(Where,  builder.Where);
					if (!builder.Having.IsEmpty) ConcatSearchCondition(Having, builder.Having);

					return builder.From.Tables[0];
				}
			}

			return source;
		}

		void ConcatSearchCondition(WhereClause where1, WhereClause where2)
		{
			if (where1.IsEmpty)
			{
				where1.SearchCondition.Conditions.AddRange(where2.SearchCondition.Conditions);
			}
			else
			{
				SearchCondition sc1 = new SearchCondition();
				SearchCondition sc2 = new SearchCondition();

				sc1.Conditions.AddRange(where1.SearchCondition.Conditions);
				sc2.Conditions.AddRange(where2.SearchCondition.Conditions);

				where1.SearchCondition.Conditions.Clear();
				where1.SearchCondition.Conditions.Add(new Condition(false, sc1));
				where1.SearchCondition.Conditions.Add(new Condition(false, sc2));
			}
		}

		void OptimizeSubQueries()
		{
			for (int i = 0; i < From.Tables.Count; i++)
				From.Tables[i] = OptimizeSubQuery(From.Tables[i]);
		}

		void SetAliases()
		{
			Dictionary<string,object> names = new Dictionary<string,object>();

			ForEachTable(delegate(TableSource table)
			{
				string alias = table.Alias;

				if (string.IsNullOrEmpty(alias))
				{
					table.Alias = "t";
					alias       = "t1";
				}

				for (int i = 1; names.ContainsKey(alias); i++)
					alias = table.Alias + i;

				names.Add(table.Alias = alias, table);
			});

			foreach (Column c in Select.Columns)
			{
				string alias = c.Alias;

				if (alias == "*")
					continue;

				if (string.IsNullOrEmpty(alias))
				{
					c.Alias = "c";
					alias   = "c1";
				}

				for (int i = 1; names.ContainsKey(alias); i++)
					alias = c.Alias + i;

				names.Add(c.Alias = alias, c);
			}

			Parameters.Clear();

			((ISqlExpressionWalkable)this).Walk(true, delegate(ISqlExpression expr)
			{
				if (expr is SqlParameter)
				{
					SqlParameter p = (SqlParameter)expr;

					string alias = p.Name;

					if (string.IsNullOrEmpty(alias))
					{
						p.Name = "p";
						alias  = "p1";
					}

					for (int i = 1; names.ContainsKey(alias); i++)
						alias = p.Name + i;

					names.Add(p.Name = alias, p);
					Parameters.Add(p);
				}

				return expr;
			});
		}

		#endregion

		#region Overrides

		public override string ToString()
		{
			return Select.ToString() + From + Where + GroupBy + Having + OrderBy;
		}

		internal static string RemoveAlias(object obj)
		{
			string str  = obj.ToString();
			int    idx1 = str.LastIndexOf(')');
			int    idx2 = str.LastIndexOf(" as ");

			return idx2 < 0 || idx2 < idx1? str : str.Substring(0, idx2);
		}

		internal static string LeaveAlias(object obj)
		{
			string str  = obj.ToString();
			int    idx1 = str.LastIndexOf(')');
			int    idx2 = str.LastIndexOf(" as ");

			return idx2 < 0 || idx2 < idx1? str : str.Substring(idx2 + 4);
		}

		#endregion

		#region ISqlExpression Members

		public int Precedence
		{
			get { return Sql.Precedence.Unknown; }
		}

		#endregion

		#region ISqlExpressionWalkable Members

		ISqlExpression ISqlExpressionWalkable.Walk(bool skipColumns, WalkingFunc func)
		{
			((ISqlExpressionWalkable)Select) .Walk(skipColumns, func);
			((ISqlExpressionWalkable)From)   .Walk(skipColumns, func);
			((ISqlExpressionWalkable)Where)  .Walk(skipColumns, func);
			((ISqlExpressionWalkable)GroupBy).Walk(skipColumns, func);
			((ISqlExpressionWalkable)Having) .Walk(skipColumns, func);
			((ISqlExpressionWalkable)OrderBy).Walk(skipColumns, func);

			return func(this);
		}

		#endregion

		#region IEquatable<ISqlExpression> Members

		bool IEquatable<ISqlExpression>.Equals(ISqlExpression other)
		{
			return this == other;
		}

		#endregion
	}
}
