using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	class FirstSingleParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			if (!methodCall.IsQueryable())
				return false;

			switch (methodCall.Method.Name)
			{
				case "First"           :
				case "FirstOrDefault"  :
				case "Single"          :
				case "SingleOrDefault" : return true;
				default                : return false;
			}
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			MethodType methodType;

			switch (methodCall.Method.Name)
			{
				case "First"           : methodType = MethodType.First;           break;
				case "FirstOrDefault"  : methodType = MethodType.FirstOrDefault;  break;
				case "Single"          : methodType = MethodType.Single;          break;
				case "SingleOrDefault" : methodType = MethodType.SingleOrDefault; break;
				default                : return null;
			}

			var sequence = parser.ParseSequence(methodCall.Arguments[0], sqlQuery);

			var take = 0;

			if (!parser.IsSubQueryParsing || parser.SqlProvider.IsSubQueryTakeSupported)
				switch (methodType)
				{
					case MethodType.First           :
					case MethodType.FirstOrDefault  :
						take = 1;
						break;

					case MethodType.Single          :
					case MethodType.SingleOrDefault :
						if (!parser.IsSubQueryParsing)
							take = 2;
							break;
				}

			if (take != 0)
				parser.ParseTake(sequence, new SqlValue(take));

			sequence.Root = sequence;
			sequence.BuildQuery();

			return new FirstSingleContext(sequence, methodType);
		}

		enum MethodType
		{
			First,
			FirstOrDefault,
			Single,
			SingleOrDefault,
		}

		class FirstSingleContext : SequenceContextBase
		{
			public FirstSingleContext(IParseContext sequence, MethodType methodType)
				: base(sequence, null)
			{
				_methodType = methodType;
			}

			readonly MethodType _methodType;

			public override Expression BuildQuery()
			{
				throw new NotImplementedException();
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				throw new NotImplementedException();
			}

			public override ISqlExpression[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public override int[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public override bool IsExpression(Expression expression, int level, RequestFor testFlag)
			{
				throw new NotImplementedException();
			}

			public override IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				throw new NotImplementedException();
			}
		}
	}
}
