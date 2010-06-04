using System;
using System.Collections.Generic;

#region ReSharper C# 2.0 disable
// ReSharper disable SuggestUseVarKeywordEvident
// ReSharper disable SuggestUseVarKeywordEverywhere
// ReSharper disable ConvertToLambdaExpression
#endregion

namespace BLToolkit.Data.Sql
{
	using VisitFunc   = Action<IQueryElement>;
	using FindFunc    = Func<IQueryElement,bool>;
	using ConvertFunc = Func<IQueryElement,IQueryElement>;

	public class QueryVisitor
	{
		readonly Dictionary<IQueryElement,IQueryElement> _visitedElements = new Dictionary<IQueryElement, IQueryElement>();
		public   Dictionary<IQueryElement,IQueryElement>  VisitedElements
		{
			get { return _visitedElements; }
		}

		public void Visit(IQueryElement element, VisitFunc action)
		{
			_visitedElements.Clear();
			Visit(element, false, action);
		}

		public void VisitAll(IQueryElement element, VisitFunc action)
		{
			_visitedElements.Clear();
			Visit(element, true, action);
		}

		void Visit(IQueryElement element, bool all, VisitFunc action)
		{
			if (element == null || !all && _visitedElements.ContainsKey(element))
				return;

			switch (element.ElementType)
			{
				case QueryElementType.SqlFunction:
					{
						foreach (ISqlExpression p in ((SqlFunction)element).Parameters) Visit(p, all, action);
						break;
					}

				case QueryElementType.SqlExpression:
					{
						foreach (ISqlExpression v in ((SqlExpression)element).Parameters) Visit(v, all, action);
						break;
					}

				case QueryElementType.SqlBinaryExpression:
					{
						SqlBinaryExpression bexpr = (SqlBinaryExpression)element;
						Visit(bexpr.Expr1, all, action);
						Visit(bexpr.Expr2, all, action);
						break;
					}

				case QueryElementType.SqlTable:
					{
						SqlTable table = (SqlTable)element;

						Visit(table.All, all, action);
						foreach (SqlField field in table.Fields.Values) Visit(field, all, action);
						foreach (Join     join  in table.Joins)         Visit(join,  all, action);
						break;
					}

				case QueryElementType.Join:
					{
						foreach (JoinOn j in ((Join)element).JoinOns) Visit(j, all, action);
						break;
					}

				case QueryElementType.Column:
					{
						Visit(((SqlQuery.Column)element).Expression, all, action);
						break;
					}

				case QueryElementType.TableSource:
					{
						SqlQuery.TableSource table = (SqlQuery.TableSource)element;

						Visit(table.Source, all, action);
						foreach (SqlQuery.JoinedTable j in table.Joins) Visit(j, all, action);
						break;
					}

				case QueryElementType.JoinedTable:
					{
						SqlQuery.JoinedTable join = (SqlQuery.JoinedTable)element;
						Visit(join.Table,     all, action);
						Visit(join.Condition, all, action);
						break;
					}

				case QueryElementType.SearchCondition:
					{
						foreach (SqlQuery.Condition c in ((SqlQuery.SearchCondition)element).Conditions) Visit(c, all, action);
						break;
					}

				case QueryElementType.Condition:
					{
						Visit(((SqlQuery.Condition)element).Predicate, all, action);
						break;
					}

				case QueryElementType.ExprPredicate:
					{
						Visit(((SqlQuery.Predicate.Expr)element).Expr1, all, action);
						break;
					}

				case QueryElementType.NotExprPredicate:
					{
						Visit(((SqlQuery.Predicate.NotExpr)element).Expr1, all, action);
						break;
					}

				case QueryElementType.ExprExprPredicate:
					{
						SqlQuery.Predicate.ExprExpr p = (SqlQuery.Predicate.ExprExpr)element;
						Visit(p.Expr1, all, action);
						Visit(p.Expr2, all, action);
						break;
					}

				case QueryElementType.LikePredicate:
					{
						SqlQuery.Predicate.Like p = (SqlQuery.Predicate.Like)element;
						Visit(p.Expr1,  all, action);
						Visit(p.Expr2,  all, action);
						Visit(p.Escape, all, action);
						break;
					}

				case QueryElementType.BetweenPredicate:
					{
						SqlQuery.Predicate.Between p = (SqlQuery.Predicate.Between)element;
						Visit(p.Expr1, all, action);
						Visit(p.Expr2, all, action);
						Visit(p.Expr3, all, action);
						break;
					}

				case QueryElementType.IsNullPredicate:
					{
						Visit(((SqlQuery.Predicate.IsNull)element).Expr1, all, action);
						break;
					}

				case QueryElementType.InSubqueryPredicate:
					{
						SqlQuery.Predicate.InSubQuery p = (SqlQuery.Predicate.InSubQuery)element;
						Visit(p.Expr1,    all, action);
						Visit(p.SubQuery, all, action);
						break;
					}

				case QueryElementType.InListPredicate:
					{
						SqlQuery.Predicate.InList p = (SqlQuery.Predicate.InList)element;
						Visit(p.Expr1, all, action);
						foreach (ISqlExpression value in p.Values) Visit(value, all, action);
						break;
					}

				case QueryElementType.FuncLikePredicate:
					{
						SqlQuery.Predicate.FuncLike p = (SqlQuery.Predicate.FuncLike)element;
						Visit(p.Function, all, action);
						break;
					}

				case QueryElementType.SetExpression:
					{
						SqlQuery.SetExpression s = (SqlQuery.SetExpression)element;
						Visit(s.Column,     all, action);
						Visit(s.Expression, all, action);
						break;
					}

				case QueryElementType.SetClause:
					{
						SqlQuery.SetClause sc = (SqlQuery.SetClause)element;

						if (sc.Into != null)
							Visit(sc.Into, all, action);

						foreach (SqlQuery.SetExpression c in sc.Items.ToArray()) Visit(c, all, action);
						break;
					}

				case QueryElementType.SelectClause:
					{
						SqlQuery.SelectClause sc = (SqlQuery.SelectClause)element;
						Visit(sc.TakeValue, all, action);
						Visit(sc.SkipValue, all, action);

						foreach (SqlQuery.Column c in sc.Columns.ToArray()) Visit(c, all, action);
						break;
					}

				case QueryElementType.FromClause:
					{
						foreach (SqlQuery.TableSource t in ((SqlQuery.FromClause)element).Tables) Visit(t, all, action);
						break;
					}

				case QueryElementType.WhereClause:
					{
						Visit(((SqlQuery.WhereClause)element).SearchCondition, all, action);
						break;
					}

				case QueryElementType.GroupByClause:
					{
						foreach (ISqlExpression i in ((SqlQuery.GroupByClause)element).Items) Visit(i, all, action);
						break;
					}

				case QueryElementType.OrderByClause:
					{
						foreach (SqlQuery.OrderByItem i in ((SqlQuery.OrderByClause)element).Items) Visit(i, all, action);
						break;
					}

				case QueryElementType.OrderByItem:
					{
						Visit(((SqlQuery.OrderByItem)element).Expression, all, action);
						break;
					}

				case QueryElementType.Union:
					Visit(((SqlQuery.Union)element).SqlQuery, all, action);
					break;

				case QueryElementType.SqlQuery:
					{
						if (all)
						{
							if (_visitedElements.ContainsKey(element))
								return;
							_visitedElements.Add(element, element);
						}

						SqlQuery q = (SqlQuery)element;

						if (q.QueryType == QueryType.Update || q.QueryType == QueryType.Insert)
							Visit(q.Set, all, action);
						else
							Visit(q.Select, all, action);

						Visit(q.From,    all, action);
						Visit(q.Where,   all, action);
						Visit(q.GroupBy, all, action);
						Visit(q.Having,  all, action);
						Visit(q.OrderBy, all, action);

						if (q.HasUnion)
						{
							foreach (SqlQuery.Union i in q.Unions)
							{
								if (i.SqlQuery == q)
									throw new InvalidOperationException();

								Visit(i, all, action);
							}
						}

						break;
					}
			}

			action(element);

			if (!all)
				_visitedElements.Add(element, element);
		}

		IQueryElement Find<T>(IEnumerable<T> arr, FindFunc find)
			where T : class, IQueryElement
		{
			foreach (T item in arr)
			{
				IQueryElement e = Find(item, find);
				if (e != null)
					return e;
			}

			return null;
		}

		public IQueryElement Find(IQueryElement element, FindFunc find)
		{
			if (element == null || find(element))
				return element;

			switch (element.ElementType)
			{
				case QueryElementType.SqlFunction       : return Find(((SqlFunction)                element).Parameters,      find);
				case QueryElementType.SqlExpression     : return Find(((SqlExpression)              element).Parameters,      find);
				case QueryElementType.Join              : return Find(((Join)                       element).JoinOns,         find);
				case QueryElementType.Column            : return Find(((SqlQuery.Column)            element).Expression,      find);
				case QueryElementType.SearchCondition   : return Find(((SqlQuery.SearchCondition)   element).Conditions,      find);
				case QueryElementType.Condition         : return Find(((SqlQuery.Condition)         element).Predicate,       find);
				case QueryElementType.ExprPredicate     : return Find(((SqlQuery.Predicate.Expr)    element).Expr1,           find);
				case QueryElementType.NotExprPredicate  : return Find(((SqlQuery.Predicate.NotExpr) element).Expr1,           find);
				case QueryElementType.IsNullPredicate   : return Find(((SqlQuery.Predicate.IsNull)  element).Expr1,           find);
				case QueryElementType.FromClause        : return Find(((SqlQuery.FromClause)        element).Tables,          find);
				case QueryElementType.WhereClause       : return Find(((SqlQuery.WhereClause)       element).SearchCondition, find);
				case QueryElementType.GroupByClause     : return Find(((SqlQuery.GroupByClause)     element).Items,           find);
				case QueryElementType.OrderByClause     : return Find(((SqlQuery.OrderByClause)     element).Items,           find);
				case QueryElementType.OrderByItem       : return Find(((SqlQuery.OrderByItem)       element).Expression,      find);
				case QueryElementType.Union             : return Find(((SqlQuery.Union)             element).SqlQuery,        find);
				case QueryElementType.FuncLikePredicate : return Find(((SqlQuery.Predicate.FuncLike)element).Function,        find);

				case QueryElementType.SqlBinaryExpression:
					{
						SqlBinaryExpression bexpr = (SqlBinaryExpression)element;
						return
							Find(bexpr.Expr1, find) ??
							Find(bexpr.Expr2, find);
					}

				case QueryElementType.SqlTable:
					{
						SqlTable table = (SqlTable)element;
						return
							Find(table.All,           find) ?? 
							Find(table.Fields.Values, find) ?? 
							Find(table.Joins,         find);
					}

				case QueryElementType.TableSource:
					{
						SqlQuery.TableSource table = (SqlQuery.TableSource)element;
						return
							Find(table.Source, find) ??
							Find(table.Joins,  find);
					}

				case QueryElementType.JoinedTable:
					{
						SqlQuery.JoinedTable join = (SqlQuery.JoinedTable)element;
						return
							Find(join.Table,     find) ??
							Find(join.Condition, find);
					}

				case QueryElementType.ExprExprPredicate:
					{
						SqlQuery.Predicate.ExprExpr p = (SqlQuery.Predicate.ExprExpr)element;
						return
							Find(p.Expr1, find) ??
							Find(p.Expr2, find);
					}

				case QueryElementType.LikePredicate:
					{
						SqlQuery.Predicate.Like p = (SqlQuery.Predicate.Like)element;
						return
							Find(p.Expr1,  find) ??
							Find(p.Expr2,  find) ??
							Find(p.Escape, find);
					}

				case QueryElementType.BetweenPredicate:
					{
						SqlQuery.Predicate.Between p = (SqlQuery.Predicate.Between)element;
						return
							Find(p.Expr1, find) ??
							Find(p.Expr2, find) ??
							Find(p.Expr3, find);
					}

				case QueryElementType.InSubqueryPredicate:
					{
						SqlQuery.Predicate.InSubQuery p = (SqlQuery.Predicate.InSubQuery)element;
						return
							Find(p.Expr1,    find) ??
							Find(p.SubQuery, find);
					}

				case QueryElementType.InListPredicate:
					{
						SqlQuery.Predicate.InList p = (SqlQuery.Predicate.InList)element;
						return
							Find(p.Expr1,  find) ??
							Find(p.Values, find);
					}

				case QueryElementType.SetExpression:
					{
						SqlQuery.SetExpression s = (SqlQuery.SetExpression)element;
						return
							Find(s.Column,     find) ??
							Find(s.Expression, find);
					}

				case QueryElementType.SetClause:
					{
						SqlQuery.SetClause sc = (SqlQuery.SetClause)element;
						return
							Find(sc.Into,  find) ??
							Find(sc.Items, find);
					}

				case QueryElementType.SelectClause:
					{
						SqlQuery.SelectClause sc = (SqlQuery.SelectClause)element;
						return
							Find(sc.TakeValue, find) ??
							Find(sc.SkipValue, find) ??
							Find(sc.Columns,   find);
					}

				case QueryElementType.SqlQuery:
					{
						SqlQuery q = (SqlQuery)element;

						return
							Find(q.Select,  find) ??
							(q.QueryType == QueryType.Update || q.QueryType == QueryType.Insert ? Find(q.Set, find) : null) ??
							Find(q.From,    find) ??
							Find(q.Where,   find) ??
							Find(q.GroupBy, find) ??
							Find(q.Having,  find) ??
							Find(q.OrderBy, find) ??
							(q.HasUnion ? Find(q.Unions, find) : null);
					}
			}

			return null;
		}

		public T Convert<T>(T element, ConvertFunc action)
			where T : class, IQueryElement
		{
			_visitedElements.Clear();
			return (T)ConvertInternal(element, action) ?? element;
		}

		IQueryElement ConvertInternal(IQueryElement element, ConvertFunc action)
		{
			if (element == null)
				return null;

			IQueryElement newElement;

			if (_visitedElements.TryGetValue(element, out newElement))
				return newElement;

			switch (element.ElementType)
			{
				case QueryElementType.SqlFunction:
					{
						SqlFunction      func  = (SqlFunction)element;
						ISqlExpression[] parms = Convert(func.Parameters, action);

						if (parms != null && !ReferenceEquals(parms, func.Parameters))
							newElement = new SqlFunction(func.SystemType, func.Name, func.Precedence, parms);

						break;
					}

				case QueryElementType.SqlExpression:
					{
						SqlExpression    expr      = (SqlExpression)element;
						ISqlExpression[] parameter = Convert(expr.Parameters, action);

						if (parameter != null && !ReferenceEquals(parameter, expr.Parameters))
							newElement = new SqlExpression(expr.SystemType, expr.Expr, expr.Precedence, parameter);

						break;
					}

				case QueryElementType.SqlBinaryExpression:
					{
						SqlBinaryExpression bexpr = (SqlBinaryExpression)element;
						ISqlExpression      expr1 = (ISqlExpression)ConvertInternal(bexpr.Expr1, action);
						ISqlExpression      expr2 = (ISqlExpression)ConvertInternal(bexpr.Expr2, action);

						if (expr1 != null && !ReferenceEquals(expr1, bexpr.Expr1) ||
							expr2 != null && !ReferenceEquals(expr2, bexpr.Expr2))
							newElement = new SqlBinaryExpression(bexpr.SystemType, expr1 ?? bexpr.Expr1, bexpr.Operation, expr2 ?? bexpr.Expr2, bexpr.Precedence);

						break;
					}

				case QueryElementType.SqlTable:
					{
						SqlTable table = (SqlTable)element;

						SqlField[] fields1 = ToArray(table.Fields);
						SqlField[] fields2 = Convert(fields1, action, delegate(SqlField f) { return new SqlField(f); });
						List<Join> joins   = Convert(table.Joins, action, delegate(Join j) { return j.Clone();       });

						bool fe = fields2 == null || ReferenceEquals(fields1,     fields2);
						bool je = joins   == null || ReferenceEquals(table.Joins, joins);

						if (!fe || !je)
						{
							if (fe)
							{
								fields2 = fields1;

								for (int i = 0; i < fields2.Length; i++)
								{
									SqlField field = fields2[i];

									fields2[i] = new SqlField(field);

									_visitedElements[field] = fields2[i];
								}
							}

							newElement = new SqlTable(table, fields2, joins ?? table.Joins);

							_visitedElements[((SqlTable)newElement).All] = table.All;
						}

						break;
					}

				case QueryElementType.Join:
					{
						Join         join = (Join)element;
						List<JoinOn> ons  = Convert(join.JoinOns, action);

						if (ons != null && !ReferenceEquals(join.JoinOns, ons))
							newElement = new Join(join.TableName, join.Alias, ons);

						break;
					}

				case QueryElementType.Column:
					{
						SqlQuery.Column col  = (SqlQuery.Column)element;
						ISqlExpression  expr = (ISqlExpression)ConvertInternal(col.Expression, action);

						IQueryElement parent;
						_visitedElements.TryGetValue(col.Parent, out parent);

						if (parent != null || expr != null && !ReferenceEquals(expr, col.Expression))
							newElement = new SqlQuery.Column(parent == null ? col.Parent : (SqlQuery)parent, expr ?? col.Expression, col._alias);

						break;
					}

				case QueryElementType.TableSource:
					{
						SqlQuery.TableSource       table  = (SqlQuery.TableSource)element;
						ISqlTableSource            source = (ISqlTableSource)ConvertInternal(table.Source, action);
						List<SqlQuery.JoinedTable> joins  = Convert(table.Joins, action);

						if (source != null && !ReferenceEquals(source, table.Source) ||
							joins  != null && !ReferenceEquals(table.Joins, joins))
							newElement = new SqlQuery.TableSource(source ?? table.Source, table._alias, joins ?? table.Joins);

						break;
					}

				case QueryElementType.JoinedTable:
					{
						SqlQuery.JoinedTable     join  = (SqlQuery.JoinedTable)element;
						SqlQuery.TableSource     table = (SqlQuery.TableSource)    ConvertInternal(join.Table,     action);
						SqlQuery.SearchCondition cond  = (SqlQuery.SearchCondition)ConvertInternal(join.Condition, action);

						if (table != null && !ReferenceEquals(table, join.Table) ||
							cond  != null && !ReferenceEquals(cond,  join.Condition))
							newElement = new SqlQuery.JoinedTable(join.JoinType, table ?? join.Table, join.IsWeak, cond ?? join.Condition);

						break;
					}

				case QueryElementType.SearchCondition:
					{
						SqlQuery.SearchCondition sc    = (SqlQuery.SearchCondition)element;
						List<SqlQuery.Condition> conds = Convert(sc.Conditions, action);

						if (conds != null && !ReferenceEquals(sc.Conditions, conds))
							newElement = new SqlQuery.SearchCondition(conds);

						break;
					}

				case QueryElementType.Condition:
					{
						SqlQuery.Condition c = (SqlQuery.Condition)element;
						ISqlPredicate      p = (ISqlPredicate)ConvertInternal(c.Predicate, action);

						if (p != null && !ReferenceEquals(c.Predicate, p))
							newElement = new SqlQuery.Condition(c.IsNot, p, c.IsOr);

						break;
					}

				case QueryElementType.ExprPredicate:
					{
						SqlQuery.Predicate.Expr p = (SqlQuery.Predicate.Expr)element;
						ISqlExpression          e = (ISqlExpression)ConvertInternal(p.Expr1, action);

						if (e != null && !ReferenceEquals(p.Expr1, e))
							newElement = new SqlQuery.Predicate.Expr(e, p.Precedence);

						break;
					}

				case QueryElementType.NotExprPredicate:
					{
						SqlQuery.Predicate.NotExpr p = (SqlQuery.Predicate.NotExpr)element;
						ISqlExpression             e = (ISqlExpression)ConvertInternal(p.Expr1, action);

						if (e != null && !ReferenceEquals(p.Expr1, e))
							newElement = new SqlQuery.Predicate.NotExpr(e, p.IsNot, p.Precedence);

						break;
					}

				case QueryElementType.ExprExprPredicate:
					{
						SqlQuery.Predicate.ExprExpr p  = (SqlQuery.Predicate.ExprExpr)element;
						ISqlExpression              e1 = (ISqlExpression)ConvertInternal(p.Expr1, action);
						ISqlExpression              e2 = (ISqlExpression)ConvertInternal(p.Expr2, action);

						if (e1 != null && !ReferenceEquals(p.Expr1, e1) || e2 != null && !ReferenceEquals(p.Expr2, e2))
							newElement = new SqlQuery.Predicate.ExprExpr(e1 ?? p.Expr1, p.Operator, e2 ?? p.Expr2);

						break;
					}

				case QueryElementType.LikePredicate:
					{
						SqlQuery.Predicate.Like p  = (SqlQuery.Predicate.Like)element;
						ISqlExpression          e1 = (ISqlExpression)ConvertInternal(p.Expr1,  action);
						ISqlExpression          e2 = (ISqlExpression)ConvertInternal(p.Expr2,  action);
						ISqlExpression          es = (ISqlExpression)ConvertInternal(p.Escape, action);

						if (e1 != null && !ReferenceEquals(p.Expr1, e1) ||
							e2 != null && !ReferenceEquals(p.Expr2, e2) ||
							es != null && !ReferenceEquals(p.Escape, es))
							newElement = new SqlQuery.Predicate.Like(e1 ?? p.Expr1, p.IsNot, e2 ?? p.Expr2, es ?? p.Escape);

						break;
					}

				case QueryElementType.BetweenPredicate:
					{
						SqlQuery.Predicate.Between p = (SqlQuery.Predicate.Between)element;
						ISqlExpression             e1 = (ISqlExpression)ConvertInternal(p.Expr1, action);
						ISqlExpression             e2 = (ISqlExpression)ConvertInternal(p.Expr2, action);
						ISqlExpression             e3 = (ISqlExpression)ConvertInternal(p.Expr3, action);

						if (e1 != null && !ReferenceEquals(p.Expr1, e1) ||
							e2 != null && !ReferenceEquals(p.Expr2, e2) ||
							e3 != null && !ReferenceEquals(p.Expr3, e3))
							newElement = new SqlQuery.Predicate.Between(e1 ?? p.Expr1, p.IsNot, e2 ?? p.Expr2, e3 ?? p.Expr3);

						break;
					}

				case QueryElementType.IsNullPredicate:
					{
						SqlQuery.Predicate.IsNull p = (SqlQuery.Predicate.IsNull)element;
						ISqlExpression            e = (ISqlExpression)ConvertInternal(p.Expr1, action);

						if (e != null && !ReferenceEquals(p.Expr1, e))
							newElement = new SqlQuery.Predicate.IsNull(e, p.IsNot);

						break;
					}

				case QueryElementType.InSubqueryPredicate:
					{
						SqlQuery.Predicate.InSubQuery p = (SqlQuery.Predicate.InSubQuery)element;
						ISqlExpression                e = (ISqlExpression)ConvertInternal(p.Expr1,    action);
						SqlQuery                      q = (SqlQuery)ConvertInternal(p.SubQuery, action);

						if (e != null && !ReferenceEquals(p.Expr1, e) || q != null && !ReferenceEquals(p.SubQuery, q))
							newElement = new SqlQuery.Predicate.InSubQuery(e ?? p.Expr1, p.IsNot, q ?? p.SubQuery);

						break;
					}

				case QueryElementType.InListPredicate:
					{
						SqlQuery.Predicate.InList p = (SqlQuery.Predicate.InList)element;
						ISqlExpression            e = (ISqlExpression)ConvertInternal(p.Expr1,    action);
						List<ISqlExpression>      v = Convert(p.Values, action);

						if (e != null && !ReferenceEquals(p.Expr1, e) || v != null && !ReferenceEquals(p.Values, v))
							newElement = new SqlQuery.Predicate.InList(e ?? p.Expr1, p.IsNot, v ?? p.Values);

						break;
					}

				case QueryElementType.FuncLikePredicate:
					{
						SqlQuery.Predicate.FuncLike p = (SqlQuery.Predicate.FuncLike)element;
						SqlFunction                 f = (SqlFunction)ConvertInternal(p.Function, action);

						if (f != null && !ReferenceEquals(p.Function, f))
							newElement = new SqlQuery.Predicate.FuncLike(f);

						break;
					}

				case QueryElementType.SetExpression:
					{
						SqlQuery.SetExpression s = (SqlQuery.SetExpression)element;
						ISqlExpression         c = (ISqlExpression)ConvertInternal(s.Column,     action);
						ISqlExpression         e = (ISqlExpression)ConvertInternal(s.Expression, action);

						if (c != null && !ReferenceEquals(s.Column, c) || e != null && !ReferenceEquals(s.Expression, e))
							newElement = new SqlQuery.SetExpression(c ?? s.Column, e ?? s.Expression);

						break;
					}

				case QueryElementType.SetClause:
					{
						SqlQuery.SetClause           s = (SqlQuery.SetClause)element;
						SqlTable                     t = s.Into != null ? (SqlTable)ConvertInternal(s.Into, action) : null;
						List<SqlQuery.SetExpression> i = Convert(s.Items, action);

						if (t != null && !ReferenceEquals(s.Into, t) || i != null && !ReferenceEquals(s.Items, i))
						{
							SqlQuery.SetClause sc = new SqlQuery.SetClause();
							sc.Into = t ?? sc.Into;
							sc.Items.AddRange(i);
							newElement = sc;
						}

						break;
					}

				case QueryElementType.SelectClause:
					{
						SqlQuery.SelectClause sc   = (SqlQuery.SelectClause)element;
						List<SqlQuery.Column> cols = Convert(sc.Columns, action);
						ISqlExpression        take = (ISqlExpression)ConvertInternal(sc.TakeValue, action);
						ISqlExpression        skip = (ISqlExpression)ConvertInternal(sc.SkipValue, action);

						IQueryElement parent;
						_visitedElements.TryGetValue(sc.SqlQuery, out parent);

						if (parent != null ||
							cols != null && !ReferenceEquals(sc.Columns,   cols) ||
							take != null && !ReferenceEquals(sc.TakeValue, take) ||
							skip != null && !ReferenceEquals(sc.SkipValue, skip))
						{
							newElement = new SqlQuery.SelectClause(sc.IsDistinct, take ?? sc.TakeValue, skip ?? sc.SkipValue, cols ?? sc.Columns);
							((SqlQuery.SelectClause)newElement).SetSqlQuery((SqlQuery)parent);
						}

						break;
					}

				case QueryElementType.FromClause:
					{
						SqlQuery.FromClause        fc   = (SqlQuery.FromClause)element;
						List<SqlQuery.TableSource> ts = Convert(fc.Tables, action);

						IQueryElement parent;
						_visitedElements.TryGetValue(fc.SqlQuery, out parent);

						if (parent != null || ts != null && !ReferenceEquals(fc.Tables, ts))
						{
							newElement = new SqlQuery.FromClause(ts ?? fc.Tables);
							((SqlQuery.FromClause)newElement).SetSqlQuery((SqlQuery)parent);
						}

						break;
					}

				case QueryElementType.WhereClause:
					{
						SqlQuery.WhereClause     wc   = (SqlQuery.WhereClause)element;
						SqlQuery.SearchCondition cond = (SqlQuery.SearchCondition)ConvertInternal(wc.SearchCondition, action);

						IQueryElement parent;
						_visitedElements.TryGetValue(wc.SqlQuery, out parent);

						if (parent != null || cond != null && !ReferenceEquals(wc.SearchCondition, cond))
						{
							newElement = new SqlQuery.WhereClause(cond ?? wc.SearchCondition);
							((SqlQuery.WhereClause)newElement).SetSqlQuery((SqlQuery)parent);
						}

						break;
					}

				case QueryElementType.GroupByClause:
					{
						SqlQuery.GroupByClause gc = (SqlQuery.GroupByClause)element;
						List<ISqlExpression>   es = Convert(gc.Items, action);

						IQueryElement parent;
						_visitedElements.TryGetValue(gc.SqlQuery, out parent);

						if (parent != null || es != null && !ReferenceEquals(gc.Items, es))
						{
							newElement = new SqlQuery.GroupByClause(es ?? gc.Items);
							((SqlQuery.GroupByClause)newElement).SetSqlQuery((SqlQuery)parent);
						}

						break;
					}

				case QueryElementType.OrderByClause:
					{
						SqlQuery.OrderByClause     oc = (SqlQuery.OrderByClause)element;
						List<SqlQuery.OrderByItem> es = Convert(oc.Items, action);

						IQueryElement parent;
						_visitedElements.TryGetValue(oc.SqlQuery, out parent);

						if (parent != null || es != null && !ReferenceEquals(oc.Items, es))
						{
							newElement = new SqlQuery.OrderByClause(es ?? oc.Items);
							((SqlQuery.OrderByClause)newElement).SetSqlQuery((SqlQuery)parent);
						}

						break;
					}

				case QueryElementType.OrderByItem:
					{
						SqlQuery.OrderByItem i = (SqlQuery.OrderByItem)element;
						ISqlExpression       e = (ISqlExpression)ConvertInternal(i.Expression, action);

						if (e != null && !ReferenceEquals(i.Expression, e))
							newElement = new SqlQuery.OrderByItem(e, i.IsDescending);

						break;
					}

				case QueryElementType.Union:
					{
						SqlQuery.Union u = (SqlQuery.Union)element;
						SqlQuery       q = (SqlQuery)ConvertInternal(u.SqlQuery, action);

						if (q != null && !ReferenceEquals(u.SqlQuery, q))
							newElement = new SqlQuery.Union(q, u.IsAll);

						break;
					}

				case QueryElementType.SqlQuery:
					{
						SqlQuery q = (SqlQuery)element;
						IQueryElement parent = null;

						bool doConvert = q.ParentSql != null && !_visitedElements.TryGetValue(q.ParentSql, out parent);

						if (!doConvert)
						{
							doConvert = null != Find(q, delegate(IQueryElement e)
							{
								IQueryElement ret = action(e);

								if (ret != null && !ReferenceEquals(e, ret))
								{
									_visitedElements.Add(e, ret);
									return true;
								}

								return false;
							});
						}

						if (!doConvert)
							break;

						SqlQuery nq = new SqlQuery();

						_visitedElements.Add(q, nq);

						SqlQuery.FromClause    fc = (SqlQuery.FromClause)   ConvertInternal(q.From,    action) ?? q.From;
						SqlQuery.SelectClause  sc = (SqlQuery.SelectClause) ConvertInternal(q.Select,  action) ?? q.Select;
						SqlQuery.SetClause     tc = q.QueryType == QueryType.Update || q.QueryType == QueryType.Insert ? ((SqlQuery.SetClause)ConvertInternal(q.Set, action) ?? q.Set) : null;
						SqlQuery.WhereClause   wc = (SqlQuery.WhereClause)  ConvertInternal(q.Where,   action) ?? q.Where;
						SqlQuery.GroupByClause gc = (SqlQuery.GroupByClause)ConvertInternal(q.GroupBy, action) ?? q.GroupBy;
						SqlQuery.WhereClause   hc = (SqlQuery.WhereClause)  ConvertInternal(q.Having,  action) ?? q.Having;
						SqlQuery.OrderByClause oc = (SqlQuery.OrderByClause)ConvertInternal(q.OrderBy, action) ?? q.OrderBy;
						List<SqlQuery.Union>   us = q.HasUnion ? Convert(q.Unions, action) : q.Unions;

						List<SqlParameter> ps = new List<SqlParameter>(q.Parameters.Count);

						foreach (SqlParameter p in q.Parameters)
						{
							IQueryElement e;

							if (_visitedElements.TryGetValue(p, out e))
							{
								if (e == null)
									ps.Add(p);
								else if (e is SqlParameter)
									ps.Add((SqlParameter)e);
							}
						}

						nq.Init(tc, sc, fc, wc, gc, hc, oc, us, (SqlQuery)parent, q.ParameterDependent, ps);

						_visitedElements[q] = action(nq) ?? nq;

						return nq;
					}
			}

			newElement = newElement == null ? action(element) : (action(newElement) ?? newElement);

			_visitedElements.Add(element, newElement);

			return newElement;
		}

		static TE[] ToArray<TK,TE>(IDictionary<TK,TE> dic)
		{
			TE[] es = new TE[dic.Count];
			int  i  = 0;

			foreach (TE e in dic.Values)
				es[i++] = e;

			return es;
		}

		delegate T Clone<T>(T obj);

		T[] Convert<T>(T[] arr, ConvertFunc action)
			where T : class, IQueryElement
		{
			return Convert(arr, action, null);
		}

		T[] Convert<T>(T[] arr1, ConvertFunc action, Clone<T> clone)
			where T : class, IQueryElement
		{
			T[] arr2 = null;

			for (int i = 0; i < arr1.Length; i++)
			{
				T elem1 = arr1[i];
				T elem2 = (T)ConvertInternal(elem1, action);

				if (elem2 != null && !ReferenceEquals(elem1, elem2))
				{
					if (arr2 == null)
					{
						arr2 = new T[arr1.Length];

						for (int j = 0; j < i; j++)
							arr2[j] = clone == null ? arr1[j] : clone(arr1[j]);
					}

					arr2[i] = elem2;
				}
				else if (arr2 != null)
					arr2[i] = clone == null ? elem1 : clone(elem1);
			}

			return arr2;
		}

		List<T> Convert<T>(List<T> list, ConvertFunc action)
			where T : class, IQueryElement
		{
			return Convert(list, action, null);
		}

		List<T> Convert<T>(List<T> list1, ConvertFunc action, Clone<T> clone)
			where T : class, IQueryElement
		{
			List<T> list2 = null;

			for (int i = 0; i < list1.Count; i++)
			{
				T elem1 = list1[i];
				T elem2 = (T)ConvertInternal(elem1, action);

				if (elem2 != null && !ReferenceEquals(elem1, elem2))
				{
					if (list2 == null)
					{
						list2 = new List<T>(list1.Count);

						for (int j = 0; j < i; j++)
							list2.Add(clone == null ? list1[j] : clone(list1[j]));
					}

					list2.Add(elem2);
				}
				else if (list2 != null)
					list2.Add(clone == null ? elem1 : clone(elem1));
			}

			return list2;
		}
	}
}
