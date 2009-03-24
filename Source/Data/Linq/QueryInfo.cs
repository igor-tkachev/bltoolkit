using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BLToolkit.Data.Sql;

namespace BLToolkit.Data.Linq
{
	abstract class QueryInfo
	{
		public class Constant : QueryInfo
		{
			public Constant(SqlTable table)
			{
				foreach (var field in table.Fields.Values)
					Columns.Add(field.Name, field);
			}

			public Dictionary<string,ISqlExpression> Columns = new Dictionary<string,ISqlExpression>();
		}

		public class New : QueryInfo
		{
			public New(QueryInfo sourceInfo, ParseInfo<ParameterExpression> parameter, ParseInfo<NewExpression> body)
			{
				SourceInfo = sourceInfo;
				Parameter  = parameter;
				Body       = body;
			}

			public QueryInfo                      SourceInfo;
			public ParseInfo<ParameterExpression> Parameter;
			public ParseInfo<NewExpression>       Body;
		}

		public class MemberInit : QueryInfo
		{
			public MemberInit(QueryInfo sourceInfo, ParseInfo<ParameterExpression> parameter, ParseInfo<MemberInitExpression> body)
			{
				SourceInfo = sourceInfo;
				Parameter  = parameter;
				Body       = body;
			}

			public QueryInfo                       SourceInfo;
			public ParseInfo<ParameterExpression>  Parameter;
			public ParseInfo<MemberInitExpression> Body;
		}

		public void Match(Action<Constant> sourceAction, Action<New> newAction, Action<MemberInit> memberInitAction)
		{
			if      (this is Constant)   sourceAction    (this as Constant);
			else if (this is New)        newAction       (this as New);
			else if (this is MemberInit) memberInitAction(this as MemberInit);
		}
	}
}
