using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	class LambdaInfo
	{
		public LambdaInfo(ParseInfo b, params ParseInfo<ParameterExpression>[] parms)
		{
			Body       = b;
			Parameters = parms;
		}

		public ParseInfo                        Body;
		public ParseInfo<ParameterExpression>[] Parameters;

		public LambdaInfo ConvertTo<T>()
			where T : Expression
		{
			return new LambdaInfo(Body.ConvertTo<T>(), Parameters);
		}
	}
}
