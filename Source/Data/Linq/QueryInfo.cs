using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq
{
	using Sql;

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
				Members    = (from b in body.Expr.Bindings select b.Member).ToList();
			}

			public QueryInfo                       SourceInfo;
			public ParseInfo<ParameterExpression>  Parameter;
			public ParseInfo<MemberInitExpression> Body;
			public List<MemberInfo>                Members;
		}

		public class SubQuery : QueryInfo
		{
			public SubQuery(QueryInfo sourceInfo, SqlBuilder subSql)
			{
				SourceInfo = sourceInfo;
				SubSql     = subSql;

				SqlBuilder = new SqlBuilder();
				SqlBuilder.From.Table(subSql);
			}

			public QueryInfo  SourceInfo;
			public SqlBuilder SubSql;
			public SqlBuilder SqlBuilder;

			public Dictionary<object,ISqlExpression> Columns = new Dictionary<object,ISqlExpression>();
		}

		public void Match(
			Action<Constant>   sourceAction,
			Action<New>        newAction,
			Action<MemberInit> memberInitAction,
			Action<SubQuery>   subQueryAction)
		{
			if      (this is Constant)   sourceAction    (this as Constant);
			else if (this is New)        newAction       (this as New);
			else if (this is MemberInit) memberInitAction(this as MemberInit);
			else if (this is SubQuery)   subQueryAction  (this as SubQuery);
		}
	}
}
