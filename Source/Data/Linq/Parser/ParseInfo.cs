using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	public class ParseInfo
	{
		public ParseInfo(IParseContext parent, Expression expression, SqlQuery sqlQuery)
		{
			Parent     = parent;
			Expression = expression;
			SqlQuery   = sqlQuery;
		}

		public ParseInfo(ParseInfo parseInfo, Expression expression)
			: this(parseInfo.Parent, expression, parseInfo.SqlQuery)
		{
		}

		public ParseInfo(Expression expression, SqlQuery sqlQuery)
			: this(null, expression, sqlQuery)
		{
		}

		public IParseContext Parent     { get; set; }
		public Expression    Expression { get; set; }
		public SqlQuery      SqlQuery   { get; set; }

		public bool          IsSubQuery { get; set; }
	}
}
