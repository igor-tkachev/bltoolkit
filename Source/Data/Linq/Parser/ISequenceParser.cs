using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	interface ISequenceParser
	{
		int       ParsingCounter { get; set; }
		ParseInfo ParseSequence(ExpressionParser parser, Expression expression);
	}
}
