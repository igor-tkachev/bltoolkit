using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;

	class TableAttributeParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			return methodCall.IsQueryable("TableName", "DatabaseName", "OwnerName");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			var sequence = parser.ParseSequence(new ParseInfo(parseInfo, methodCall.Arguments[0]));

			var table = (TableParser.TableContext)sequence;
			var value = (string)((ConstantExpression)methodCall.Arguments[1]).Value;

			switch (methodCall.Method.Name)
			{
				case "TableName"    : table.SqlTable.PhysicalName = value; break;
				case "DatabaseName" : table.SqlTable.Database     = value; break;
				case "OwnerName"    : table.SqlTable.Owner        = value; break;
			}

			return sequence;
		}
	}
}
