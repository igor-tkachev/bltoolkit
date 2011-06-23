using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	public class SequenceConvertInfo
	{
		public Expression                         Expression;
		public Type                               NewType;
		public Dictionary<Expression, Expression> ExpressionsToReplace;
	}
}
