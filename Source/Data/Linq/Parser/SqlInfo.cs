using System;
using System.Reflection;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	public class SqlInfo
	{
		public ISqlExpression Sql;
		public int            Index = -1;
		public MemberInfo     Member;
	}
}
