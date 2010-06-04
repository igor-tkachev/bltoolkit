using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	using Mapping;
	using Data.Sql;

	abstract class QueryField : ReflectionHelper, ICloneableElement
	{
		#region Column

		public class Column : QueryField
		{
			public Column(QuerySource.Table table, SqlField field, MemberMapper mapper)
			{
				Field  = field;
				Table  = table;
				Mapper = mapper;

				ParsingTracer.WriteLine(this);
			}

			public readonly SqlField          Field;
			public readonly QuerySource.Table Table;
			public readonly MemberMapper      Mapper;

			public override QuerySource[] Sources { get { return new[] { Table }; } }

			public override FieldIndex[] Select<T>(ExpressionParser<T> _)
			{
				ParsingTracer.WriteLine(this);
				ParsingTracer.WriteLine("table", Table);
				ParsingTracer.IncIndentLevel();

				var index =  new[] { new FieldIndex { Index = Table.SqlQuery.Select.Add(Field, Field.Alias), Field = this } };

				ParsingTracer.DecIndentLevel();
				ParsingTracer.WriteLine("table", Table);
				return index;
			}

			public override ISqlExpression[] GetExpressions<T>(ExpressionParser<T> parser)
			{
				return new [] { Field };
			}

			public override bool CanBeNull()
			{
				return Field.CanBeNull();
			}

			public override ICloneableElement Clone(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				if (!doClone(this))
					return this;

				ICloneableElement clone;

				if (!objectTree.TryGetValue(this, out clone))
					objectTree.Add(this, clone = new Column(
						(QuerySource.Table)Table.Clone(objectTree, doClone),
						(SqlField)Field.Clone(objectTree, doClone),
						Mapper));

				return clone;
			}

#if OVERRIDETOSTRING

			public override string ToString()
			{
				return Field.ToString();
			}

#endif
		}

		#endregion

		#region ExprColumn

		public class ExprColumn : QueryField
		{
			public ExprColumn(QuerySource source, Expression expr, string alias)
			{
				QuerySource = source;
				Expr        = expr;
				_alias      = alias;

				ParsingTracer.WriteLine(this);
			}

			public ExprColumn(QuerySource source, ISqlExpression expr, string alias)
			{
				QuerySource    = source;
				_sqlExpression = expr;
				_alias         = alias;

				ParsingTracer.WriteLine("sql", this);
			}

			public readonly QuerySource QuerySource;
			public readonly Expression  Expr;

			readonly string _alias;
			FieldIndex[]    _index;
			ISqlExpression  _sqlExpression;

			public          ISqlExpression SqlExpression { get { return _sqlExpression;        } }
			public override QuerySource[]  Sources       { get { return new[] { QuerySource }; } }

			public override FieldIndex[] Select<T>(ExpressionParser<T> parser)
			{
				ParsingTracer.WriteLine(this);
				ParsingTracer.WriteLine(QuerySource);
				ParsingTracer.IncIndentLevel();

				if (_index == null)
				{
					if (_sqlExpression == null)
						_sqlExpression = Parse(parser, QuerySource.Sources);

					_index = new[] { new FieldIndex { Index = QuerySource.SqlQuery.Select.Add(_sqlExpression, _alias), Field = this } };
				}

				ParsingTracer.DecIndentLevel();
				ParsingTracer.WriteLine(QuerySource);
				return _index;
			}

			bool _inParsing;

			public override ISqlExpression[] GetExpressions<T>(ExpressionParser<T> parser)
			{
				if (_sqlExpression == null)
				{
					if (_inParsing)
						return null;

					_inParsing = true;
					_sqlExpression = Parse(parser, QuerySource.Sources);
					_inParsing = false;
				}

				return new [] { _sqlExpression };
			}

			ISqlExpression Parse<T>(ExpressionParser<T> parser, params QuerySource[] sources)
			{
				var expr = parser.ParseExpression(Expr, sources);

				if (expr is SqlQuery.SearchCondition)
				{
					/*
					expr = parser.Convert(new SqlFunction("CASE",
						expr, new SqlValue(true),
						new SqlQuery.SearchCondition(new[] { new SqlQuery.Condition(true, (SqlQuery.SearchCondition)expr) }), new SqlValue(false),
						new SqlValue(false)));
					*/

					expr = parser.Convert(new SqlFunction(typeof(bool), "CASE", expr, new SqlValue(true), new SqlValue(false)));
				}

				return expr;
			}

			public override bool CanBeNull()
			{
				if (_sqlExpression != null)
					return _sqlExpression.CanBeNull();

				return SqlDataType.CanBeNull(Expr.Type);
			}

			public override ICloneableElement Clone(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				if (!doClone(this))
					return this;

				ICloneableElement clone;

				if (!objectTree.TryGetValue(this, out clone))
				{
					var col = new ExprColumn((QuerySource)QuerySource.Clone(objectTree, doClone), Expr, _alias);

					if (_sqlExpression != null)
						col._sqlExpression = (ISqlExpression)_sqlExpression.Clone(objectTree, doClone);

					objectTree.Add(this, clone = col);
				}

				return clone;
			}

#if OVERRIDETOSTRING

			public override string ToString()
			{
				return _sqlExpression != null ? _sqlExpression.ToString() : Expr.ToString();
			}

#endif
		}

		#endregion

		#region SubQueryColumn

		public class SubQueryColumn : QueryField
		{
			public SubQueryColumn(QuerySource.SubQuery querySource, QueryField field)
			{
				QuerySource = querySource;
				Field       = field;

				ParsingTracer.WriteLine(field);
			}

			public readonly QuerySource.SubQuery QuerySource;
			public readonly QueryField           Field;

			FieldIndex[] _index;
			FieldIndex[] _subIndex;

			public override QuerySource[] Sources { get { return new[] { QuerySource }; } }

			public override FieldIndex[] Select<T>(ExpressionParser<T> parser)
			{
				ParsingTracer.WriteLine(this);
				ParsingTracer.WriteLine(QuerySource);
				ParsingTracer.IncIndentLevel();

				if (_index == null)
				{
					SetSubIndex(parser);

					_index = new FieldIndex[_subIndex.Length];

					for (var i = 0; i < _subIndex.Length; i++)
					{
						var col = QuerySource.SubSql.Select.Columns[_subIndex[i].Index];
						_index[i] = new FieldIndex { Index = QuerySource.SqlQuery.Select.Add(col), Field = this };
					}
				}

				ParsingTracer.DecIndentLevel();
				ParsingTracer.WriteLine(QuerySource);
				return _index;
			}

			void SetSubIndex<T>(ExpressionParser<T> parser)
			{
				if (_subIndex == null)
				{
					_subIndex = Field.Select(parser);

					if (QuerySource.SubSql.HasUnion)
					{
						var sub = QuerySource.BaseQuery;
						var idx = sub.Fields.IndexOf(Field);
						var mi  = sub.GetMember(Field);

						foreach (var union in QuerySource.Unions)
						{
							if (mi != null)
							{
								var f = union.GetField(mi);

								if (f != null)
								{
									f.Select(parser);
									continue;
								}

								if (union is QuerySource.Expr)
								{
									union.SqlQuery.Select.Add(new SqlValue(null));
									continue;
								}
							}

							union.Fields[idx].Select(parser);
						}
					}
				}
			}

			public override ISqlExpression[] GetExpressions<T>(ExpressionParser<T> parser)
			{
				SetSubIndex(parser);

				if (_subIndex.Length != 1)
					throw new LinqException("Cannot convert '{0}' to SQL.", Field.GetExpressions(parser)[0]);

				return new [] { QuerySource.SubSql.Select.Columns[_subIndex[0].Index] };
			}

			public override bool CanBeNull()
			{
				return Field.CanBeNull();
			}

			public override ICloneableElement Clone(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				if (!doClone(this))
					return this;

				ICloneableElement clone;

				if (!objectTree.TryGetValue(this, out clone))
					objectTree.Add(this, clone = new SubQueryColumn(
						(QuerySource.SubQuery)QuerySource.Clone(objectTree, doClone),
						(QueryField)Field.Clone(objectTree, doClone)));

				return clone;
			}

#if OVERRIDETOSTRING

			public override string ToString()
			{
				return Field.ToString();
			}

#endif
		}

		#endregion

		#region GroupByColumn

		public class GroupByColumn : QueryField
		{
			public GroupByColumn(QuerySource.GroupBy groupBySource)
			{
				GroupBySource = groupBySource;

				ParsingTracer.WriteLine("groupBy", this);
			}

			public readonly QuerySource.GroupBy GroupBySource;

			FieldIndex[] _index;

			public override QuerySource[] Sources { get { return new[] { GroupBySource }; } }

			public override FieldIndex[] Select<T>(ExpressionParser<T> parser)
			{
				ParsingTracer.WriteLine(this);
				ParsingTracer.WriteLine(GroupBySource.BaseQuery);
				ParsingTracer.IncIndentLevel();

				if (_index == null)
					_index = GroupBySource.BaseQuery.Select(parser);

				ParsingTracer.DecIndentLevel();
				ParsingTracer.WriteLine(GroupBySource.BaseQuery);
				return _index;
			}

			public override ISqlExpression[] GetExpressions<T>(ExpressionParser<T> parser)
			{
				return GroupBySource.BaseQuery.GetExpressions(parser);
			}

			public override bool CanBeNull()
			{
				return false;
			}

			public override ICloneableElement Clone(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				if (!doClone(this))
					return this;

				ICloneableElement clone;

				if (!objectTree.TryGetValue(this, out clone))
					objectTree.Add(this, clone = new GroupByColumn((QuerySource.GroupBy)GroupBySource.Clone(objectTree, doClone)));

				return clone;
			}

#if OVERRIDETOSTRING

			public override string ToString()
			{
				return GroupBySource.ToString();
			}

#endif
		}

		#endregion

		#region base

		public object Clone()
		{
			return Clone(new Dictionary<ICloneableElement,ICloneableElement>(), _ => true);
		}

		public abstract QuerySource[]    Sources { get; }

		public abstract ICloneableElement     Clone              (Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone);
		public abstract FieldIndex[]          Select          <T>(ExpressionParser<T> parser);
		public abstract ISqlExpression[]      GetExpressions  <T>(ExpressionParser<T> parser);
		public abstract bool                  CanBeNull          ();

		#endregion
	}
}
