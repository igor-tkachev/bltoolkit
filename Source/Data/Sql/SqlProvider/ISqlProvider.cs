using System;
using System.Text;

#if FW3
using System.Linq.Expressions;
using System.Reflection;
#endif

namespace BLToolkit.Data.Sql.SqlProvider
{
	public interface ISqlProvider
	{
		int            BuildSql               (SqlQuery sqlQuery, StringBuilder sb, int indent, int nesting);
		ISqlExpression ConvertExpression      (ISqlExpression expression);
		ISqlPredicate  ConvertPredicate       (ISqlPredicate  predicate);
		void           ConvertSearchCondition (SqlQuery.SearchCondition searchCondition);

		string         Name                      { get; }
		SqlQuery     SqlQuery                { get; set; }

		bool           SkipAcceptsParameter      { get; }
		bool           TakeAcceptsParameter      { get; }
		bool           IsSkipSupported           { get; }
		bool           IsTakeSupported           { get; }
		bool           IsSubQueryColumnSupported { get; }
		bool           IsCountSubQuerySupported  { get; }

#if FW3
		Expression     ConvertMember    (MemberInfo mi);
#endif
	}
}
