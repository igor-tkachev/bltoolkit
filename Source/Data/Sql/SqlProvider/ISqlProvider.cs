using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	public interface ISqlProvider
	{
		int            CommandCount      (SqlQuery sqlQuery);
		int            BuildSql          (int commandNumber, SqlQuery sqlQuery, StringBuilder sb, int indent, int nesting, bool skipAlias);
		ISqlExpression ConvertExpression (ISqlExpression expression);
		ISqlPredicate  ConvertPredicate  (ISqlPredicate  predicate);
		SqlQuery       Finalize          (SqlQuery sqlQuery);

		string         Name                        { get; }
		SqlQuery       SqlQuery                    { get; set; }

		bool           SkipAcceptsParameter        { get; }
		bool           TakeAcceptsParameter        { get; }
		bool           IsSkipSupported             { get; }
		bool           IsTakeSupported             { get; }
		bool           IsSubQueryTakeSupported     { get; }
		bool           IsSubQueryColumnSupported   { get; }
		bool           IsCountSubQuerySupported    { get; }
		bool           IsIdentityParameterRequired { get; }

		LambdaExpression ConvertMember    (MemberInfo mi);
	}
}
