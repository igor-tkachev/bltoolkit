using System;

namespace BLToolkit.Data.Linq
{
	using Mapping;
	using Data.Sql;

	abstract class QueryField : ReflectionHelper
	{
		public class Column : QueryField
		{
			public Column(QuerySource.Table table, SqlField field, MemberMapper mapper)
			{
				Table   = table;
				Field   = field;
				_mapper = mapper;
			}

			public  readonly QuerySource.Table Table;
			public  readonly SqlField          Field;

			private readonly MemberMapper _mapper;

			public override int[] Select<T>(ExpressionParser<T> parser)
			{
				return new[] { Table.SqlBuilder.Select.Add(Field, Field.Name) };
			}

			public override ISqlExpression GetExpression<T>(ExpressionParser<T> parser)
			{
				return Field;
			}
		}

		public class ExprColumn : QueryField
		{
			public ExprColumn(QuerySource source, ParseInfo expr, string alias)
			{
				QuerySource = source;
				Expr        = expr;

				_alias      = alias;
			}

			public readonly QuerySource QuerySource;
			public readonly ParseInfo   Expr;

			readonly string _alias;
			int[]           _index;
			ISqlExpression  _sqlExpression;

			public override int[] Select<T>(ExpressionParser<T> parser)
			{
				if (_index == null)
				{
					if (_sqlExpression == null)
						_sqlExpression = parser.ParseExpression(QuerySource.ParentQueries[0], Expr);

					_index = new[] { QuerySource.SqlBuilder.Select.Add(_sqlExpression, _alias) };
				}

				return _index;
			}

			public override ISqlExpression GetExpression<T>(ExpressionParser<T> parser)
			{
				if (_sqlExpression == null)
					_sqlExpression = parser.ParseExpression(QuerySource, Expr);

				return _sqlExpression;
			}
		}

		public class SubQueryColumn : QueryField
		{
			public SubQueryColumn(QuerySource.SubQuery querySource, QueryField field)
			{
				QuerySource = querySource;
				Field       = field;
			}

			public readonly QuerySource.SubQuery QuerySource;
			public readonly QueryField           Field;

			int[] _index;
			int[] _subIndex;

			public override int[] Select<T>(ExpressionParser<T> parser)
			{
				if (_index == null)
				{
					if (_subIndex == null)
						_subIndex = Field.Select(parser);

					_index = new int[_subIndex.Length];

					for (var i = 0; i < _subIndex.Length; i++)
					{
						var col = QuerySource.SubSql.Select.Columns[_subIndex[i]];
						_index[i] = QuerySource.SqlBuilder.Select.Add(col);
					}
				}

				return _index;
			}

			public override ISqlExpression GetExpression<T>(ExpressionParser<T> parser)
			{
				if (_subIndex == null)
					_subIndex = Field.Select(parser);

				if (_subIndex.Length != 1)
					throw new LinqException("Cannot convert '{0}' to SQL.", Field.GetExpression(parser));

				return QuerySource.SubSql.Select.Columns[_subIndex[0]];
			}
		}

		public abstract int[]          Select       <T>(ExpressionParser<T> parser);
		public abstract ISqlExpression GetExpression<T>(ExpressionParser<T> parser);
	}
}
