using System;

namespace BLToolkit.Data.Linq
{
	using Mapping;
	using Sql;

	abstract class QueryField : ReflectionHelper
	{
		public class Column : QueryField
		{
			public Column(QuerySource source, SqlField field, MemberMapper mapper)
				: base(source)
			{
				Field   = field;
				_mapper = mapper;
			}

			public  readonly SqlField     Field;
			private readonly MemberMapper _mapper;

			public int Select()
			{
				return QuerySource.SqlBuilder.Select.Add(Field, Field.Name);
			}
		}

		public class ColumnExpr : QueryField
		{
			public ColumnExpr(QuerySource source, ParseInfo expr)
				: base(source)
			{
				Expr = expr;
			}

			public readonly ParseInfo Expr;
		}

		protected QueryField(QuerySource source)
		{
			QuerySource = source;
		}

		public QuerySource QuerySource;
	}
}
