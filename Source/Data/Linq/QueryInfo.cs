using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BLToolkit.Data.Sql;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Linq
{
	abstract class QueryInfo
	{
		public class Constant : QueryInfo
		{
			public Constant(Type objectType, SqlTable table)
			{
				ObjectType = objectType;
				Table      = table;
			}

			public Type     ObjectType;
			public SqlTable Table;
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

		public ISqlExpression GetField(ParseInfo<MemberExpression> memberExpr)
		{
			ISqlExpression expr = null;

			Match(
				constantExpr =>
				{
					expr = constantExpr.Table[memberExpr.Expr.Member.Name];

					if (expr == null)
						throw new LinqException("Member '{0}.{1}' is not an SQL column.", constantExpr.ObjectType, memberExpr.Expr.Member.Name);
				},
				newExpr    => expr = newExpr.   SourceInfo.GetField(memberExpr),
				memberInit => expr = memberInit.SourceInfo.GetField(memberExpr)
			);

			return expr;
		}
	}
}
