using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	interface ISequenceParser
	{
		int        ParsingCounter { get; set; }
		IParseInfo ParseSequence(IParseInfo parseInfo, Expression expression);
	}
}
