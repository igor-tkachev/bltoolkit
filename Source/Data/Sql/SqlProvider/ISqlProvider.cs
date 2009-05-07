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
		ISqlExpression ConvertExpression(ISqlExpression expression);
		ISqlPredicate  ConvertPredicate (ISqlPredicate  predicate);

#if FW3
		Expression     ConvertMember    (MemberInfo mi);
#endif
	}
}
