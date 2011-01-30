using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	public interface ISequenceParser
	{
		int           ParsingCounter { get; set; }
		bool          CanParse     (ExpressionParser parser, Expression expression, SqlQuery sqlQuery);
		IParseContext ParseSequence(ExpressionParser parser, Expression expression, SqlQuery sqlQuery);
	}
}
