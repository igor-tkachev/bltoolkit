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
		StringBuilder  BuildSql         (SqlBuilder sqlBuilder, StringBuilder sb, int indent);
		void           UpdateParameters (SqlBuilder sql);
		ISqlExpression ConvertExpression(ISqlExpression expression);
		ISqlPredicate  ConvertPredicate (ISqlPredicate  predicate);

		bool           IsSkipSupported { get; }
		bool           IsTakeSupported { get; }

#if FW3
		Expression     ConvertMember    (MemberInfo mi);
#endif

	}
}
