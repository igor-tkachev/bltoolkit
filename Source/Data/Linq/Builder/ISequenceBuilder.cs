using System;

namespace BLToolkit.Data.Linq.Parser
{
	public interface ISequenceParser
	{
		int           ParsingCounter { get; set; }
		bool          CanParse     (ExpressionParser parser, ParseInfo parseInfo);
		IParseContext ParseSequence(ExpressionParser parser, ParseInfo parseInfo);
	}
}
