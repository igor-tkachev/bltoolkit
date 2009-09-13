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
		int            BuildSql         (SqlBuilder sqlBuilder, StringBuilder sb, int indent, int nesting);
		void           UpdateParameters (SqlBuilder sql);
		ISqlExpression ConvertExpression(ISqlExpression expression);
		ISqlPredicate  ConvertPredicate (ISqlPredicate  predicate);

		string         Name                 { get; }
		SqlBuilder     SqlBuilder           { get; set; }
		bool           IsSkipSupported      { get; }
		bool           IsTakeSupported      { get; }
		bool           SkipAcceptsParameter { get; }
		bool           TakeAcceptsParameter { get; }

#if FW3
		Expression     ConvertMember    (MemberInfo mi);
#endif
	}
}
