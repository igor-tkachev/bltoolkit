using System;
using System.Collections.Generic;

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
				_table  = table;
				_field  = field;
				_mapper = mapper;

				ParsingTracer.WriteLine(this);
			}

			readonly QuerySource.Table _table;
			readonly SqlField          _field;
			readonly MemberMapper      _mapper;

			public override QuerySource[] Sources { get { return new[] { _table }; } }

			public override FieldIndex[] Select<T>(ExpressionParser<T> parser)
			{
				ParsingTracer.WriteLine(this);
				ParsingTracer.WriteLine("table", _table);
				ParsingTracer.IncIndentLevel();

				var index =  new[] { new FieldIndex { Index = _table.SqlBuilder.Select.Add(_field, _field.Name), Field = this } };

				ParsingTracer.DecIndentLevel();
				ParsingTracer.WriteLine("table", _table);
				return index;
			}

			public override ISqlExpression[] GetExpressions<T>(ExpressionParser<T> parser)
			{
				return new [] { _field };
			}

			public override ICloneableElement Clone(Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone)
			{
				if (!doClone(this))
					return this;

				ICloneableElement clone;

				if (!objectTree.TryGetValue(this, out clone))
					objectTree.Add(this, clone = new Column(
						(QuerySource.Table)_table.Clone(objectTree, doClone),
						(SqlField)_field.Clone(objectTree, doClone),
						_mapper));

				return clone;
			}

			public override string ToString()
			{
				return _field.ToString();
			}
		}

		#endregion

		#region ExprColumn

		public class ExprColumn : QueryField
		{
			public ExprColumn(QuerySource source, ParseInfo expr, string alias)
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
			public readonly ParseInfo   Expr;

			readonly string _alias;
			FieldIndex[]    _index;
			ISqlExpression  _sqlExpression;

			public override QuerySource[] Sources { get { return new[] { QuerySource }; } }

			public override FieldIndex[] Select<T>(ExpressionParser<T> parser)
			{
				ParsingTracer.WriteLine(this);
				ParsingTracer.WriteLine(QuerySource);
				ParsingTracer.IncIndentLevel();

				if (_index == null)
				{
					if (_sqlExpression == null)
						_sqlExpression = parser.ParseExpression(QuerySource.ParentQueries.Length == 0 ? null: QuerySource.ParentQueries[0], Expr);

					_index = new[] { new FieldIndex { Index = QuerySource.SqlBuilder.Select.Add(_sqlExpression, _alias), Field = this } };
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
					_sqlExpression = parser.ParseExpression(QuerySource, Expr);
					_inParsing = false;
				}

				return new [] { _sqlExpression };
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

			public override string ToString()
			{
				return _sqlExpression != null ? _sqlExpression.ToString() : Expr.ToString();
			}
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
					if (_subIndex == null)
						_subIndex = Field.Select(parser);

					_index = new FieldIndex[_subIndex.Length];

					for (var i = 0; i < _subIndex.Length; i++)
					{
						var col = QuerySource.SubSql.Select.Columns[_subIndex[i].Index];
						_index[i] = new FieldIndex { Index = QuerySource.SqlBuilder.Select.Add(col), Field = this };
					}
				}

				ParsingTracer.DecIndentLevel();
				ParsingTracer.WriteLine(QuerySource);
				return _index;
			}

			public override ISqlExpression[] GetExpressions<T>(ExpressionParser<T> parser)
			{
				if (_subIndex == null)
					_subIndex = Field.Select(parser);

				if (_subIndex.Length != 1)
					throw new LinqException("Cannot convert '{0}' to SQL.", Field.GetExpressions(parser)[0]);

				return new [] { QuerySource.SubSql.Select.Columns[_subIndex[0].Index] };
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

			public override string ToString()
			{
				return Field.ToString();
			}
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
				ParsingTracer.WriteLine(GroupBySource.ParentQueries[0]);
				ParsingTracer.IncIndentLevel();

				if (_index == null)
					_index = GroupBySource.ParentQueries[0].Select(parser);

				ParsingTracer.DecIndentLevel();
				ParsingTracer.WriteLine(GroupBySource.ParentQueries[0]);
				return _index;
			}

			public override ISqlExpression[] GetExpressions<T>(ExpressionParser<T> parser)
			{
				return GroupBySource.ParentQueries[0].GetExpressions(parser);
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

			public override string ToString()
			{
				return GroupBySource.ToString();
			}
		}

		#endregion

		#region base

		public object Clone()
		{
			return Clone(new Dictionary<ICloneableElement,ICloneableElement>(), _ => true);
		}

		public abstract QuerySource[]    Sources { get; }

		public abstract ICloneableElement Clone            (Dictionary<ICloneableElement, ICloneableElement> objectTree, Predicate<ICloneableElement> doClone);
		public abstract FieldIndex[]      Select        <T>(ExpressionParser<T> parser);
		public abstract ISqlExpression[]  GetExpressions<T>(ExpressionParser<T> parser);

		#endregion
	}
}
